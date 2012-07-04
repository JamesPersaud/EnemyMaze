using System;
using System.Drawing;
using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D9;
using SlimDX.Windows;
using System.Collections.Generic;

namespace ld18_EnemyDefence
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            MainForm parent = new MainForm();
            RenderForm form = new RenderForm("SlimDX - MiniTri Direct3D9 Sample");
            form.Location = new Point(8, 24);
            form.TopLevel = false;
            parent.Controls.Add(form);
            parent.Size = new Size(900, 600);    
            form.ClientSize = new Size((GameTypes.MAP_WIDTH*GameTypes.SPRITE_WIDTH), (GameTypes.MAP_HEIGHT*GameTypes.SPRITE_HEIGHT));
            parent.Controls.Add(new Button());
            parent.Show();

            //Icon ico = new Icon("assets//textures//icon.bmp");

            form.Click += new EventHandler(form_Click);
            form.MouseMove += new MouseEventHandler(form_MouseMove);
            form.MouseLeave += new EventHandler(form_MouseLeave);
            form.MouseEnter += new EventHandler(form_MouseEnter);
            form.KeyDown += new KeyEventHandler(form_KeyDown);
            form.KeyUp += new KeyEventHandler(form_KeyUp);
            form.LostFocus += new EventHandler(form_LostFocus);
            form.Focus();

            parent.KeyDown += new KeyEventHandler(form_KeyDown);
            parent.KeyUp += new KeyEventHandler(form_KeyUp);

            form.FormBorderStyle = FormBorderStyle.None;

            Device device = new Device(new Direct3D(), 0, DeviceType.Hardware, form.Handle, CreateFlags.HardwareVertexProcessing, new PresentParameters()
            {
                BackBufferWidth = form.ClientSize.Width,
                BackBufferHeight = form.ClientSize.Height
            });

            Maze testmaze = new Maze();            
            testmaze.Initialize();
            testmaze.Generate();
            int[,] walkable = new int[GameTypes.MAP_WIDTH, GameTypes.MAP_WIDTH];
            walkable = testmaze.getWalkableArray();

            TextureManager.Instance.init(device);
            GameEngine.Instance.init();
            WindowManager.Instance.Main = parent;
            WindowManager.Instance.Slim = form;                      

            GameEngine.Instance.LifeTimer.Start();
            GameEngine.Instance.FPSTimer.Start();
            GameEngine.Instance.TestSpriteTimer.Start();            

            PathFinding.Instance.init(GameTypes.MAP_WIDTH,GameTypes.MAP_HEIGHT);

            GameEngine.Instance.testvec = new Vector3(5 * 32, 5 * 632, 0);

            Enemy e = new Enemy(GameTypes.ENEMY_UNI);
            e.StackTest();

            GameEngine.Instance.testEnemy = new Enemy(GameTypes.ENEMY_UNI);
            GameEngine.Instance.Enemies = new System.Collections.Generic.List<Enemy>();

            GameEngine.Instance.LifeTimer.Stop();
            //for (int i = 0; i < 10; i++)
            //{
            //    Enemy enemy = new Enemy(GameTypes.ENEMY_UNI);
            //    enemy.BirthTime = (float)GameEngine.Instance.LifeTimer.Elapsed + i;
            //    GameEngine.Instance.Enemies.Add(enemy);
            //}            

            //close some nodes
            //PathFinding.Instance.Nodes[4 + 10].Walkable = false;
            //PathFinding.Instance.Nodes[4 + 20].Walkable = false;
            //PathFinding.Instance.Nodes[4 + 30].Walkable = false;
            //PathFinding.Instance.Nodes[4 + 40].Walkable = false;
            //PathFinding.Instance.Nodes[4 + 50].Walkable = false;
            //PathFinding.Instance.Nodes[4 + 60].Walkable = false;
            //PathFinding.Instance.Nodes[4 + 70].Walkable = false;
            //PathFinding.Instance.Nodes[4 + 80].Walkable = false;

            for (int y = 0; y < GameTypes.MAP_HEIGHT; y++)
            {
                for (int x = 0; x < GameTypes.MAP_WIDTH; x++)
                {
                    if (walkable[x, y] == 0 || y == GameTypes.MAP_HEIGHT - 1)
                    {
                        PathFinding.Instance.GetNode(x, y).Walkable = false;
                    }
                }
            }

            PathFinding.Instance.GetNode(GameTypes.PLAYER_START_COORDS).Walkable = true;

            GameEngine.Instance.SpawnEnemy(GameEngine.Instance.PlayerLevel);
            GameEngine.Instance.PlaceShiny();
       
            WindowManager.Instance.Main.lblScore.Text = "SCORE: " + GameEngine.Instance.PlayerScore.ToString();
            WindowManager.Instance.Main.lblLife.Text = "LIFE";
            WindowManager.Instance.Main.ShowLife();
            WindowManager.Instance.Main.lblHiscore.Text = "HI-SCORE  " + GameEngine.Instance.HiScore.ToString();            


            MessagePump.Run(form, () =>
            {
                if (!GameEngine.Instance.PauseRendering)
                {
                    int score = GameEngine.Instance.PlayerScore;
                    int life = GameEngine.Instance.PlayerLife;
                    int hiscore = GameEngine.Instance.HiScore;

                    //Time to spawn?
                    GameEngine.Instance.SpawnTimer.Stop();
                    if ((float)GameEngine.Instance.SpawnTimer.Elapsed >= GameEngine.Instance.SpawnFrequency)
                    {
                        GameEngine.Instance.SpawnEnemy(GameEngine.Instance.PlayerLevel);
                        if((GameEngine.Instance.SpawnFrequency - GameTypes.GAME_SPAWN_SPEEDUP_RATE >= GameTypes.GAME_SPAWN_MIN_FREQ))
                            GameEngine.Instance.SpawnFrequency -= GameTypes.GAME_SPAWN_SPEEDUP_RATE;

                        for(int i =0;i<GameTypes.GAME_LEVEL_FREQ.Length;i++)                        
                            if(GameEngine.Instance.SpawnFrequency <= GameTypes.GAME_LEVEL_FREQ[i])
                                GameEngine.Instance.PlayerLevel = i;                        

                        GameEngine.Instance.SpawnTimer.Start();                        
                    }
                    else
                    {
                        //if (GameEngine.Instance.Enemies.Count == 1)
                          //  GameEngine.Instance.SpawnEnemy(GameEngine.Instance.PlayerLevel);
                    }

                    GameEngine.Instance.LifeTimer.Stop();
                    float time = (float)GameEngine.Instance.LifeTimer.Elapsed;
                    float fps = (float)GameEngine.Instance.FPSTimer.FPS;
                    float ticks = (float)GameEngine.Instance.LifeTimer.GetTicks(0.017f);
                    WindowManager.Instance.Main.showelapsed(time, fps, ticks);

                    device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
                    device.BeginScene();

                    //draw maze

                    TextureManager.Instance.spritey.Begin(SpriteFlags.AlphaBlend | SpriteFlags.DoNotAddRefTexture);
                    for (int y = 0; y < GameTypes.MAP_HEIGHT; y++)
                    {
                        for (int x = 0; x < GameTypes.MAP_WIDTH; x++)
                        {
                            if (PathFinding.Instance.GetNode(x, y).Walkable)
                                TextureManager.Instance.spritey.Draw(TextureManager.Instance.ddsTestTile, new Rectangle(0, 0, 32, 32), new Vector3(0, 0, 0), new Vector3(x * 32, y * 32, 0), new Color4(Color.White));
                            else
                                TextureManager.Instance.spritey.Draw(TextureManager.Instance.ddsTestTileOut, new Rectangle(0, 0, 32, 32), new Vector3(0, 0, 0), new Vector3(x * 32, y * 32, 0), new Color4(Color.White));
                        }
                    }

                    //draw shiny
                    TextureManager.Instance.spritey.Draw(TextureManager.Instance.pngShiny, new Rectangle(0, 0, 32, 32), new Vector3(0, 0, 0), new Vector3(GameEngine.Instance.ShinyLocation.X*32,GameEngine.Instance.ShinyLocation.Y*32,0), new Color4(Color.White));


                    //sprite update
                    GameEngine.Instance.TestSpriteTimer.Stop();
                    if (GameEngine.Instance.TestSpriteTimer.Elapsed > 0.017)
                    {
                        GameEngine.Instance.ThePlayer.Update();
                        //GameEngine.Instance.testvec.X += 0.5f;
                        //GameEngine.Instance.testEnemy.UpdateMove();

                        // move enemies
                        foreach (Enemy enemy in GameEngine.Instance.Enemies)
                        {
                            if (enemy.Active)
                                enemy.UpdateMove();
                            else
                            {
                                if (GameEngine.Instance.LifeTimer.Elapsed > enemy.BirthTime)
                                    enemy.Active = true;
                            }
                        }
                        //move player
                        GameEngine.Instance.ThePlayer.Move(GameEngine.Instance.KEYS_DOWN);

                        GameEngine.Instance.TestSpriteTimer.Start();
                    }

                    //Draw enemies
                    GameEngine.Instance.Enemies.Sort(delegate(Enemy e1, Enemy e2) 
                    {
                        if (e1.Position.Y != e2.Position.Y)
                            return e1.Position.Y.CompareTo(e2.Position.Y);
                        else
                            return e1.BirthTime.CompareTo(e2.BirthTime);
                    });
                    bool playerDrawn = false;
                    bool playerShadowDrawn = false;

                    //SHADOWS first                                        
                    foreach (Enemy enemy in GameEngine.Instance.Enemies)
                    {
                        if (enemy.Active)
                        {
                            if (!playerShadowDrawn && enemy.Position.Y > GameEngine.Instance.ThePlayer.Location.Y)
                            {
                                TextureManager.Instance.spritey.Draw(TextureManager.Instance.pngDropShadow, new Rectangle(0, 0, 32, 32),
                                    new Vector3(0, 0, 0), GameEngine.Instance.ThePlayer.Location, new Color4(Color.White));
                                playerShadowDrawn = true;
                            }

                            if (PathFinding.Instance.GetNode(enemy.LastPoint).Walkable && PathFinding.Instance.GetNode(enemy.NextPoint).Walkable)
                            {
                                TextureManager.Instance.spritey.Draw(TextureManager.Instance.pngDropShadow, new Rectangle(0, 0, 32, 32),
                                    new Vector3(0, 0, 0), enemy.Position, new Color4(Color.White));
                            }
                        }
                    }
                    if (!playerShadowDrawn)
                    {
                        TextureManager.Instance.spritey.Draw(TextureManager.Instance.pngDropShadow, new Rectangle(0, 0, 32, 32),
                                new Vector3(0, 0, 0), GameEngine.Instance.ThePlayer.Location, new Color4(Color.White));
                    }

                    //DRAW ENEMIES
                    foreach (Enemy enemy in GameEngine.Instance.Enemies)
                    {
                        if (enemy.Active)
                        {
                            if (!playerDrawn && enemy.Position.Y > GameEngine.Instance.ThePlayer.Location.Y)
                            {
                                TextureManager.Instance.spritey.Draw(TextureManager.Instance.tgaEnemies, GameEngine.Instance.ThePlayer.RenderRect,
                                    new Vector3(0, 0, 0), GameEngine.Instance.ThePlayer.LocationDraw, new Color4(Color.White));
                                playerDrawn = true;
                            }

                            if (PathFinding.Instance.GetNode(enemy.LastPoint).Walkable && PathFinding.Instance.GetNode(enemy.NextPoint).Walkable)
                            {
                                TextureManager.Instance.spritey.Draw(TextureManager.Instance.tgaEnemies, enemy.RenderRect,
                                    new Vector3(0, 0, 0), enemy.Position, new Color4(Color.White));
                            }
                        }
                    }

                    //Draw PLAYER if at bottom of screen
                    if (!playerDrawn)
                    {                                                 
                        TextureManager.Instance.spritey.Draw(TextureManager.Instance.tgaEnemies, GameEngine.Instance.ThePlayer.RenderRect,
                                    new Vector3(0, 0, 0), GameEngine.Instance.ThePlayer.LocationDraw, new Color4(Color.White));
                    }

                    //Draw mouseover effect
                    if (WindowManager.Instance.MouseOverDX)
                    {
                        TextureManager.Instance.spritey.Draw(TextureManager.Instance.ddsMouseover, new Rectangle(0, 0, 32, 32), new Vector3(0, 0, 0),
                            new Vector3(WindowManager.Instance.mouseGridLocation.X * GameTypes.SPRITE_WIDTH, WindowManager.Instance.mouseGridLocation.Y * GameTypes.SPRITE_HEIGHT, 0),
                            new Color4(Color.White));
                        TextureManager.Instance.spritey.Draw(TextureManager.Instance.pngMouse, new Rectangle(0, 0, 32, 32), new Vector3(16, 16, 0),
                            new Vector3(WindowManager.Instance.mouseVector.X, WindowManager.Instance.mouseVector.Y, 0),
                            new Color4(Color.White));
                    }

                    TextureManager.Instance.spritey.End();

                    device.EndScene();
                    device.Present();

                    //WindowManager.Instance.Main.showkeys();

                    GameEngine.Instance.CheckCollisions();

                    if(score != GameEngine.Instance.PlayerScore)
                        WindowManager.Instance.Main.lblScore.Text = "SCORE: " + GameEngine.Instance.PlayerScore.ToString();
                    if (life != GameEngine.Instance.PlayerLife)
                    {
                        WindowManager.Instance.Main.lblLife.Text = "LIFE";
                        WindowManager.Instance.Main.ShowLife();
                    }
                    if (hiscore != GameEngine.Instance.HiScore)
                        WindowManager.Instance.Main.lblHiscore.Text = "HI-SCORE  " + GameEngine.Instance.HiScore.ToString();
                }
            });            

            foreach (var item in ObjectTable.Objects)
                item.Dispose();
        }

        static void form_LostFocus(object sender, EventArgs e)
        {           
            //GameEngine.Instance.KEYS_DOWN = 0;
        }

        //Very ugly input handling but time is running out :(    
        static void form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W && (GameEngine.Instance.KEYS_DOWN & GameTypes.MOVEKEY_W) ==0)
                GameEngine.Instance.KEYS_DOWN ^= GameTypes.MOVEKEY_W;
            else if (e.KeyCode == Keys.A && (GameEngine.Instance.KEYS_DOWN & GameTypes.MOVEKEY_A) == 0)
                GameEngine.Instance.KEYS_DOWN ^= GameTypes.MOVEKEY_A;
            if (e.KeyCode == Keys.S && (GameEngine.Instance.KEYS_DOWN & GameTypes.MOVEKEY_S) == 0)
                GameEngine.Instance.KEYS_DOWN ^= GameTypes.MOVEKEY_S;
            if (e.KeyCode == Keys.D && (GameEngine.Instance.KEYS_DOWN & GameTypes.MOVEKEY_D) == 0)
                GameEngine.Instance.KEYS_DOWN ^= GameTypes.MOVEKEY_D;
        }

        static void form_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W && (GameEngine.Instance.KEYS_DOWN & GameTypes.MOVEKEY_W) != 0)
                GameEngine.Instance.KEYS_DOWN ^= GameTypes.MOVEKEY_W;
            else if (e.KeyCode == Keys.A && (GameEngine.Instance.KEYS_DOWN & GameTypes.MOVEKEY_A) != 0)
                GameEngine.Instance.KEYS_DOWN ^= GameTypes.MOVEKEY_A;
            if (e.KeyCode == Keys.S && (GameEngine.Instance.KEYS_DOWN & GameTypes.MOVEKEY_S) != 0)
                GameEngine.Instance.KEYS_DOWN ^= GameTypes.MOVEKEY_S;
            if (e.KeyCode == Keys.D && (GameEngine.Instance.KEYS_DOWN & GameTypes.MOVEKEY_D) != 0)
                GameEngine.Instance.KEYS_DOWN ^= GameTypes.MOVEKEY_D;
        }   

        static void form_MouseEnter(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor.Hide();
            WindowManager.Instance.MouseOverDX = true;
            WindowManager.Instance.Slim.Focus();
        }

        static void form_MouseLeave(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor.Show();
            WindowManager.Instance.MouseOverDX = false;
            //GameEngine.Instance.KEYS_DOWN = 0;
        }

        static void form_MouseMove(object sender, MouseEventArgs e)
        {
            WindowManager.Instance.mouseGridLocation = new Point(e.X / GameTypes.SPRITE_WIDTH, e.Y / GameTypes.SPRITE_HEIGHT);
            WindowManager.Instance.mouseVector = new Vector3(e.X, e.Y, 0);                                                     
        }

        static void form_Click(object sender, EventArgs e)
        {
            Stack<PathNode> testpath = new Stack<PathNode>();
            GameEngine.Instance.PauseRendering = true;
            //checks for playing a tower
            bool BadPlacement=false;

            Point clickloc = WindowManager.Instance.mouseGridLocation;

            //first toggle the square            
            PathFinding.Instance.GetNode(clickloc).Walkable = !PathFinding.Instance.GetNode(clickloc).Walkable;
            
            //check obvious ones first
            if (!BadPlacement)
            {
                if (BoundingBox.Intersects(PathFinding.Instance.GetNode(clickloc).Bounds, GameEngine.Instance.ThePlayer.BoundsPlacement))
                    BadPlacement = true;
            }
            //if(clickloc == GameTypes.ENEMY_START_COORDS || clickloc == GameTypes.ENEMY_END_COORDS)
            //{
                //BadPlacement = true;
            //}
            //if(!BadPlacement)
            //{
            //    //check there's still a path from start to finish                
            //    PathFinding.Instance.GetPath(testpath, GameTypes.ENEMY_START_COORDS, GameTypes.ENEMY_END_COORDS);
            //    if (testpath == null || testpath.Count ==0)
            //    BadPlacement = true;            
            //}            
            if(!BadPlacement)
            {
                foreach (Enemy enemy in GameEngine.Instance.Enemies)
                {
                    if (enemy.NextPoint == clickloc || enemy.LastPoint == clickloc)
                    {
                        BadPlacement = true;
                        break;
                    }
                }
            }
            if (!BadPlacement)
            {
                List<Point> done = new List<Point>();

                //check that all moving enemies can get out.
                foreach (Enemy enemy in GameEngine.Instance.Enemies)
                {
                    if (enemy.Active && !done.Contains(enemy.NextPoint))
                    {
                        PathFinding.Instance.GetPath(testpath, enemy.NextPoint, GameEngine.Instance.ThePlayer.CurrentPoint);
                        if (testpath == null || testpath.Count == 0)
                        {
                            BadPlacement = true;
                            break;
                        }
                        else
                        {
                            done.Add(enemy.NextPoint);
                        }
                    }
                }
            }            
            
            //if good - mark all enemies paths dirty
            if (!BadPlacement)
            {
                foreach (Enemy enemy in GameEngine.Instance.Enemies)
                {
                    enemy.DirtyPathing = true;
                }
            }
            
            if (BadPlacement)
            {
                //MessageBox.Show("You can't place there, it's not fair on the enemies!");
                PathFinding.Instance.GetNode(clickloc).Walkable = true;
            }
            GameEngine.Instance.PauseRendering = false;
        }
    }
}
