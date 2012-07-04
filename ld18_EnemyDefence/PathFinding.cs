/*
 * 
 * I know this is given as an example of what LD is *NOT* for but search as I might, I haven't got a 2D grid based C# A* lying around :(
 * 
 */

using System;
using System.Drawing;
using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D9;
using SlimDX.Windows;
using System.Collections.Generic;

namespace ld18_EnemyDefence 
{
    public class PathNode
    {
        public Vector3 Location;
        public BoundingBox Bounds;
        public PathNode Parent = null;
        public bool Walkable = true;
        public Point P = new Point(0,0);
        public int H =0;
        public int G =0;
        public int F
        {
            get { return G + H; }
        }
        public int X
        {
            get{return P.X;}
        }
        public int Y
        {
            get { return P.Y; }
        }
    }

    public class PathFinding
    {
        public List<PathNode> Nodes;
        public List<PathNode> Closed;
        public List<PathNode> Open;        

        public int Width;
        public int Height;

        public void init(int width,int height)
        {
            Width = width;
            Height = height;
            Nodes = new List<PathNode>();
            Closed = new List<PathNode>();
            Open = new List<PathNode>();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    PathNode node = new PathNode();
                    node.P = new Point(x, y);
                    node.Location = new Vector3(x * GameTypes.SPRITE_WIDTH, y * GameTypes.SPRITE_HEIGHT, 0);
                    node.Bounds = new BoundingBox(node.Location,new Vector3(node.Location.X+GameTypes.SPRITE_WIDTH,node.Location.Y+GameTypes.SPRITE_HEIGHT,0));
                    Nodes.Add(node);
                }
            }
        }

        public void GetPath(Stack<PathNode> returnPath, Point startPoint, Point endPoint)
        {
            returnPath.Clear();
            Open.Clear();
            Closed.Clear();

            foreach (PathNode clearnode in Nodes)            
                clearnode.Parent = null;            

            SetManhattanHeuristic(GetNode(startPoint),endPoint);
            Open.Add(GetNode(startPoint));
            
            Open.Remove(GetNode(startPoint));
            Closed.Add(GetNode(startPoint));
            OpenWalkableChildren(GetNode(startPoint), endPoint);

            while (!Closed.Contains(GetNode(endPoint)))
            {
                // mmm Generic collections
                Open.Sort(delegate(PathNode n1, PathNode n2)
                    {
                        return -(n1.F.CompareTo(n2.F)); //highest first
                    });


                for (int i = (Open.Count - 1); i >= 0; i--) // deal with list from last to first
                {
                    PathNode n = Open[i];
                    Open.Remove(n);
                    Closed.Add(n);
                    OpenWalkableChildren(n, endPoint);
                }

                //If there's nothing in the open list and we haven't found the end, we've got trouble.
                if (Open.Count == 0 && !Closed.Contains(GetNode(endPoint)))
                {
                    returnPath = null;
                    return;
                }
            }

            PushNewPath(returnPath,GetNode(endPoint));
        }

        public void PushNewPath(Stack<PathNode> path, PathNode node)
        {
            path.Push(node);
            if (node.Parent != null)
                PushNewPath(path, node.Parent);
        }

        public void SetManhattanHeuristic(PathNode node, Point end)
        {
            node.H = Math.Abs(end.X - node.X) + Math.Abs(end.Y - node.Y);
        }

        public void OpenWalkableChildren(PathNode parent, Point end)
        {
            //up
            if (parent.Y > 0 && GetNode(parent.X, parent.Y - 1).Walkable && !Closed.Contains(GetNode(parent.X, parent.Y - 1)))
            {
                //if open and g is higher than it would be with new parent, reparent
                if (Open.Contains(GetNode(parent.X, parent.Y - 1)))
                {
                    if (GetNode(parent.X, parent.Y - 1).G >= parent.G + 1)                    
                        GetNode(parent.X, parent.Y - 1).G = parent.G + 1;                    
                }
                else
                {
                    Open.Add(GetNode(parent.X, parent.Y - 1));
                    SetManhattanHeuristic(GetNode(parent.X, parent.Y - 1), end);
                    GetNode(parent.X, parent.Y - 1).Parent = parent;
                    GetNode(parent.X, parent.Y - 1).G = parent.G+1;
                }
            }
            //down
            if (parent.Y < Height - 1 && GetNode(parent.X, parent.Y + 1).Walkable && !Closed.Contains(GetNode(parent.X, parent.Y + 1)))
            {
                if (Open.Contains(GetNode(parent.X, parent.Y + 1)))
                {
                    if (GetNode(parent.X, parent.Y + 1).G >= parent.G + 1)
                        GetNode(parent.X, parent.Y + 1).G = parent.G + 1;   
                }
                else
                {
                    Open.Add(GetNode(parent.X, parent.Y + 1));
                    SetManhattanHeuristic(GetNode(parent.X, parent.Y + 1), end);
                    GetNode(parent.X, parent.Y + 1).Parent = parent;
                    GetNode(parent.X, parent.Y + 1).G = parent.G + 1;
                }
            }
            //left
            if (parent.X > 0 && GetNode(parent.X - 1, parent.Y).Walkable && !Closed.Contains(GetNode(parent.X -1, parent.Y)))
            {
                if (Open.Contains(GetNode(parent.X -1, parent.Y)))
                {
                    if (GetNode(parent.X -1, parent.Y).G >= parent.G + 1)
                        GetNode(parent.X -1, parent.Y).G = parent.G + 1;   
                }
                else
                {
                    Open.Add(GetNode(parent.X - 1, parent.Y));
                    SetManhattanHeuristic(GetNode(parent.X - 1, parent.Y), end);
                    GetNode(parent.X - 1, parent.Y).Parent = parent;
                    GetNode(parent.X - 1, parent.Y).G = parent.G + 1;
                }
            }
            //right
            if (parent.X < Width - 1 && GetNode(parent.X + 1, parent.Y).Walkable && !Closed.Contains(GetNode(parent.X + 1, parent.Y)))
            {
                if (Open.Contains(GetNode(parent.X +1, parent.Y)))
                {
                    if (GetNode(parent.X+1, parent.Y).G >= parent.G + 1)
                        GetNode(parent.X+1, parent.Y).G = parent.G + 1;   
                }
                else
                {
                    Open.Add(GetNode(parent.X + 1, parent.Y));
                    SetManhattanHeuristic(GetNode(parent.X + 1, parent.Y), end);
                    GetNode(parent.X + 1, parent.Y).Parent = parent;
                    GetNode(parent.X + 1, parent.Y).G = parent.G + 1;
                }
            }
        }
        
        public PathNode GetNode(int x,int y)
        {
            return Nodes[(y * Width) + x];
        }
        public PathNode GetNode(Point p)
        {
            return GetNode(p.X, p.Y);
        }

        #region singleton stuff

        PathFinding()
        {
        }

        public static PathFinding Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly PathFinding instance = new PathFinding();
        }

        #endregion
    }
}