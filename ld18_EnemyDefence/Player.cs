using System;
using System.Drawing;
using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D9;
using SlimDX.Windows;
using System.Collections.Generic;

namespace ld18_EnemyDefence
{
    public class Player
    {
        public Point CurrentPoint;
        public Vector3 Location;
        public int Direction;
        public float bounce;
        public float bouncetime;
        public BoundingBox Bounds;
        public BoundingBox BoundsPlacement;
        public Vector3 LocationDraw;
        public bool falling;

        public Rectangle RenderRect
        {
            get
            {
                switch (Direction)
                {
                    case GameTypes.ENEMY_DIRECTION_UP: return GameTypes.PLAYER_TYPE.UpRect; break;
                    case GameTypes.ENEMY_DIRECTION_DOWN: return GameTypes.PLAYER_TYPE.DownRect; break;
                    case GameTypes.ENEMY_DIRECTION_LEFT: return GameTypes.PLAYER_TYPE.LeftRect; break;
                    case GameTypes.ENEMY_DIRECTION_RIGHT: return GameTypes.PLAYER_TYPE.RightRect; break;
                    default: return GameTypes.PLAYER_TYPE.UpRect; break;
                }
            }
        }

        public Player()
        {
            CurrentPoint = GameTypes.PLAYER_START_COORDS;
            Location = new Vector3(CurrentPoint.X * GameTypes.SPRITE_WIDTH, CurrentPoint.Y * GameTypes.SPRITE_HEIGHT, 0);
            Direction = GameTypes.PLAYER_START_DIRECTION;
            bounce = 0.0f;
            Bounds = new BoundingBox(Location, new Vector3(Location.X + GameTypes.SPRITE_WIDTH - 4, Location.Y + GameTypes.SPRITE_HEIGHT - 4, 0));
            BoundsPlacement = new BoundingBox(Location, new Vector3(Location.X + GameTypes.SPRITE_WIDTH, Location.Y + GameTypes.SPRITE_HEIGHT, 0));
            LocationDraw = Vector3.Zero;
            bouncetime = 0;
            falling = false;
        }

        public void Update()
        {
            //update bounce;            
            if (!falling)
            {
                bouncetime += 1.0f;
                if (bounce >= 16)
                    falling = true;
                bounce = 16;
            }
            else
            {
                bouncetime -= 1.0f;
                if (bounce <= 0)
                    falling = false;
                bounce = 0;
            }

            float vi = 0.5f;
            float g = -0.1f;

            bounce = vi * bouncetime -((g * (bouncetime*bouncetime))/2);

            LocationDraw.X = Location.X;
            LocationDraw.Y = Location.Y + bounce-16;
            LocationDraw.Z = 0;
        }

        public bool Move(int movekeys)
        {
            float diagonalcost = 1.4f;

            float dx = 0;
            float dy = 0;           

            if ((movekeys & GameTypes.MOVEKEY_W) != 0)
            {               
                //upleft
                if ((movekeys & GameTypes.MOVEKEY_A) != 0)
                {
                    if (Direction != GameTypes.ENEMY_DIRECTION_UP && Direction != GameTypes.ENEMY_DIRECTION_LEFT)                    
                        Direction = GameTypes.ENEMY_DIRECTION_LEFT;
                    dx = (GameTypes.BASE_MOVE_SPEED / diagonalcost) * -(GameTypes.PLAYER_TYPE.Speed);
                    dy = (GameTypes.BASE_MOVE_SPEED / diagonalcost) * -(GameTypes.PLAYER_TYPE.Speed);
                }
                //upright
                else if ((movekeys & GameTypes.MOVEKEY_D) != 0)
                {
                    if (Direction != GameTypes.ENEMY_DIRECTION_UP && Direction != GameTypes.ENEMY_DIRECTION_RIGHT)
                        Direction = GameTypes.ENEMY_DIRECTION_RIGHT;
                    dx = (GameTypes.BASE_MOVE_SPEED / diagonalcost) * (GameTypes.PLAYER_TYPE.Speed);
                    dy = (GameTypes.BASE_MOVE_SPEED / diagonalcost) * -(GameTypes.PLAYER_TYPE.Speed);
                }
                //uponly
                else
                {
                    Direction = GameTypes.ENEMY_DIRECTION_UP;                  
                    dy = (GameTypes.BASE_MOVE_SPEED) * -(GameTypes.PLAYER_TYPE.Speed);
                }
            }
            else if ((movekeys & GameTypes.MOVEKEY_S) != 0)
            {
                //downleft
                if ((movekeys & GameTypes.MOVEKEY_A) != 0)
                {
                    if (Direction != GameTypes.ENEMY_DIRECTION_DOWN && Direction != GameTypes.ENEMY_DIRECTION_LEFT)
                        Direction = GameTypes.ENEMY_DIRECTION_LEFT;
                    dx = (GameTypes.BASE_MOVE_SPEED / diagonalcost) * -(GameTypes.PLAYER_TYPE.Speed);
                    dy = (GameTypes.BASE_MOVE_SPEED / diagonalcost) * (GameTypes.PLAYER_TYPE.Speed);
                }
                //downright
                else if ((movekeys & GameTypes.MOVEKEY_D) != 0)
                {
                    if (Direction != GameTypes.ENEMY_DIRECTION_DOWN && Direction != GameTypes.ENEMY_DIRECTION_RIGHT)
                        Direction = GameTypes.ENEMY_DIRECTION_RIGHT;
                    dx = (GameTypes.BASE_MOVE_SPEED / diagonalcost) * (GameTypes.PLAYER_TYPE.Speed);
                    dy = (GameTypes.BASE_MOVE_SPEED / diagonalcost) * (GameTypes.PLAYER_TYPE.Speed);
                }
                //downonly
                else
                {
                    Direction = GameTypes.ENEMY_DIRECTION_DOWN;
                    dy = (GameTypes.BASE_MOVE_SPEED) * (GameTypes.PLAYER_TYPE.Speed);
                }
            }
            //left
            else if ((movekeys & GameTypes.MOVEKEY_A) != 0)
            {
                Direction = GameTypes.ENEMY_DIRECTION_LEFT;
                dx = (GameTypes.BASE_MOVE_SPEED) * -(GameTypes.PLAYER_TYPE.Speed);
            }
            //right
            else if ((movekeys & GameTypes.MOVEKEY_D) != 0)
            {
                Direction = GameTypes.ENEMY_DIRECTION_RIGHT;
                dx = (GameTypes.BASE_MOVE_SPEED) * (GameTypes.PLAYER_TYPE.Speed);
            }            

            //prevent wall collision
            Vector3 target = new Vector3(Location.X + dx, Location.Y + dy, 0);
            BoundingBox bb = new BoundingBox(new Vector3(target.X+8,target.Y+8,0),new Vector3(target.X+GameTypes.SPRITE_WIDTH-8,target.Y+GameTypes.SPRITE_HEIGHT-8,0));            
            //check nodes overlapped by bb
            bool collide = false;

            //stop playing running out of maze;
            if (target.X < 0 || target.X+GameTypes.SPRITE_WIDTH > GameTypes.MAP_WIDTH * GameTypes.SPRITE_WIDTH || target.Y < 0 || target.Y+GameTypes.SPRITE_HEIGHT > GameTypes.SPRITE_HEIGHT * GameTypes.MAP_HEIGHT)
                collide = true;

            foreach (PathNode node in PathFinding.Instance.Nodes)  // really poor code
            {
                if (!node.Walkable && BoundingBox.Intersects(node.Bounds, bb))
                {
                    collide = true;
                    break;
                }
            }

            if (!collide)
            {
                Location = target;
                Point newpoint = new Point(((int)Math.Floor((Location.X + 16) / GameTypes.SPRITE_WIDTH)), ((int)Math.Floor((Location.Y + 16) / GameTypes.SPRITE_HEIGHT)));
                if (CurrentPoint != newpoint)
                {
                    GameEngine.Instance.PauseRendering = true;

                    foreach (Enemy enemy in GameEngine.Instance.Enemies)
                    {
                        enemy.DirtyPathing = true;
                    }

                    GameEngine.Instance.PauseRendering = false;
                }
                
                CurrentPoint = new Point(((int)Math.Floor((Location.X+16) / GameTypes.SPRITE_WIDTH)), ((int)Math.Floor((Location.Y+16) / GameTypes.SPRITE_HEIGHT)));
            }
            else
            {
                //Diagonal movement blocked? Look for a sliding direction.
                if ((movekeys & GameTypes.MOVEKEY_W) != 0)
                {
                    if ((movekeys & GameTypes.MOVEKEY_A) != 0)
                    {
                        if(!Move(GameTypes.MOVEKEY_W))
                            Move(GameTypes.MOVEKEY_A);
                    }
                    else if((movekeys & GameTypes.MOVEKEY_D) != 0)
                    {
                        if(!Move(GameTypes.MOVEKEY_W))
                            Move(GameTypes.MOVEKEY_D);
                    }
                }
                if ((movekeys & GameTypes.MOVEKEY_S) != 0)
                {
                    if ((movekeys & GameTypes.MOVEKEY_A) != 0)
                    {
                        if(!Move(GameTypes.MOVEKEY_S))
                            Move(GameTypes.MOVEKEY_A);
                    }
                    else if((movekeys & GameTypes.MOVEKEY_D) != 0)
                    {
                        if(!Move(GameTypes.MOVEKEY_S))
                            Move(GameTypes.MOVEKEY_D);
                    }
                }
            }

            Bounds = bb;
            BoundsPlacement = new BoundingBox(target, new Vector3(target.X + GameTypes.SPRITE_WIDTH, target.Y + GameTypes.SPRITE_HEIGHT, 0));
            return !collide;
        }
    }
}