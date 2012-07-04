using System;
using System.Drawing;
using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D9;
using SlimDX.Windows;
using System.Collections.Generic;

namespace ld18_EnemyDefence
{
    public sealed class Enemy
    {
        public EnemyType Type;
        public Point LastPoint;
        public Point NextPoint;
        public Vector3 Position = Vector3.Zero;
        public int Direction;
        public int DamageTaken;
        public bool IsFiring;
        public bool DirtyPathing;
        public bool IsMoving;
        public bool Active;

        public Point CurrentTarget;

        public Stack<PathNode> Path;
        public float BirthTime = 0.0f;

        public BoundingBox Bounds;

        public Rectangle RenderRect
        {
            get
            {
                switch (Direction)
                {
                    case GameTypes.ENEMY_DIRECTION_UP: return Type.UpRect;break;
                    case GameTypes.ENEMY_DIRECTION_DOWN: return Type.DownRect;break;
                    case GameTypes.ENEMY_DIRECTION_LEFT: return Type.LeftRect;break;
                    case GameTypes.ENEMY_DIRECTION_RIGHT: return Type.RightRect;break;
                    default: return Type.UpRect; break;
                }
            }
        }

        //Instantly place at point.
        public void BeamTo(Point p)
        {
            LastPoint = p;
            Position = new Vector3(p.X * GameTypes.SPRITE_WIDTH, p.Y * GameTypes.SPRITE_HEIGHT, 0);
        }

        //Continue moving along path... When a new node is reached, check if path is dirty, if so repath.
        public void UpdateMove()
        {
            if (IsMoving)
            {
                //Get Target node                
                Vector3 targetVector = new Vector3(Position.X, Position.Y, Position.Z);
                Vector3 nextVector = new Vector3(NextPoint.X * GameTypes.SPRITE_WIDTH, NextPoint.Y * GameTypes.SPRITE_HEIGHT, 0);
                Vector3 lastVector = new Vector3(LastPoint.X * GameTypes.SPRITE_WIDTH, LastPoint.Y * GameTypes.SPRITE_HEIGHT, 0);

                switch (Direction)
                {
                    case GameTypes.ENEMY_DIRECTION_UP: targetVector.Y -= GameTypes.BASE_MOVE_SPEED * Type.Speed; break;
                    case GameTypes.ENEMY_DIRECTION_DOWN: targetVector.Y += GameTypes.BASE_MOVE_SPEED * Type.Speed; break;
                    case GameTypes.ENEMY_DIRECTION_LEFT: targetVector.X -= GameTypes.BASE_MOVE_SPEED * Type.Speed; break;
                    case GameTypes.ENEMY_DIRECTION_RIGHT: targetVector.X += GameTypes.BASE_MOVE_SPEED * Type.Speed; break;
                }
                
                if (Math.Abs(Vector3.Distance(lastVector, targetVector)) >= Math.Abs(Vector3.Distance(lastVector, nextVector)))
                {
                    //Reached target square
                    Position = nextVector;
                    LastPoint = NextPoint;
                    IsMoving = false;
                }
                else
                {
                    //Not there yet
                    Position = targetVector;
                }

                Bounds = new BoundingBox(Position, new Vector3(Position.X + 16, Position.Y + 16, 0));
            }
            else
            {
                if (DirtyPathing)                
                    GetPathToEnd();                

                if (Path.Count > 0)
                {
                    NextPoint = Path.Pop().P;
                    if (NextPoint.X < LastPoint.X)
                        Direction = GameTypes.ENEMY_DIRECTION_LEFT;
                    else if(NextPoint.X > LastPoint.X)
                        Direction = GameTypes.ENEMY_DIRECTION_RIGHT;
                    else if(NextPoint.Y < LastPoint.Y)
                        Direction = GameTypes.ENEMY_DIRECTION_UP;
                    else if (NextPoint.Y > LastPoint.Y)
                        Direction = GameTypes.ENEMY_DIRECTION_DOWN;
                    IsMoving = true;
                    DirtyPathing = false;
                }
            }            
        }

        public Enemy(EnemyType type)
        {
            Type = type;
            Direction = GameTypes.ENEMY_START_DIRECTION;           
            DamageTaken = 0;          
            IsFiring = false;
            Path = new Stack<PathNode>();
            //Timer = new GameTimer();
            DirtyPathing = true;
            IsMoving = false;
            BeamTo(GameTypes.ENEMY_START_COORDS);
            Active = false;
            Bounds = new BoundingBox();
        }

        public void GetPathToEnd()
        {
            PathFinding.Instance.GetPath(Path, LastPoint, GameEngine.Instance.ThePlayer.CurrentPoint);            
        }        

        public void StackTest()
        {
            PathFinding.Instance.GetPath(Path, GameTypes.ENEMY_START_COORDS, GameEngine.Instance.ThePlayer.CurrentPoint);
            WindowManager.Instance.Main.showelapsed(0, 0, 0);
        }
    }
}