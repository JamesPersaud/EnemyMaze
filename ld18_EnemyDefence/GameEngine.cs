using System;
using System.Drawing;
using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D9;
using SlimDX.Windows;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace ld18_EnemyDefence
{
    public sealed class GameEngine
    {
        public bool PauseRendering = false;

        public GameTimer LifeTimer;
        public GameTimer FPSTimer;
        public GameTimer TestSpriteTimer;
        public GameTimer SpawnTimer;

        public List<Enemy> Enemies;
        public List<Enemy> Towers;

        public Player ThePlayer;

        public Vector3 testvec;

        public Enemy testEnemy;

        public Random Randomiser;

        public float SpawnFrequency;

        public int PlayerScore;
        public int PlayerLife;
        public int PlayerLevel;

        public Point ShinyLocation;
        public bool ShinyInPlay = false;

        public int KEYS_DOWN;

        public int HiScore;

        public void init()
        {
            LifeTimer = new GameTimer();
            FPSTimer = new GameTimer();
            TestSpriteTimer = new GameTimer();
            testvec = new Vector3(0, 0, 0);
            Enemies = new List<Enemy>();
            Towers = new List<Enemy>();
            Randomiser = new Random();
            ThePlayer = new Player();
            SpawnTimer = new GameTimer();
            SpawnTimer.Start();
            SpawnFrequency = GameTypes.GAME_INITIAL_SPAWN_FREQ;
            PlayerScore = 0;
            PlayerLife = 100;
            PlayerLevel = 0;
            LoadHiscore();
        }

        public void LoadHiscore()
        {
            if (File.Exists(Application.UserAppDataPath+"OnlyLosersEditPlaintextHiscoreFiles"))
            {
                string s = File.ReadAllText(Application.UserAppDataPath + "OnlyLosersEditPlaintextHiscoreFiles");
                if (!Int32.TryParse(s, out HiScore))
                {
                    HiScore = 0;
                    SaveHiscore();
                }
            }
            else
            {
                HiScore = 0;
            }
        }

        public void SaveHiscore()
        {
            File.WriteAllText(Application.UserAppDataPath + "OnlyLosersEditPlaintextHiscoreFiles", PlayerScore.ToString());
        }

        public void PlaceShiny()
        {
            ShinyInPlay = false;      
            while (!ShinyInPlay)
            {
                ShinyLocation = new Point(Randomiser.Next(1, GameTypes.MAP_WIDTH), Randomiser.Next(GameTypes.MAP_HEIGHT));
                if (PathFinding.Instance.GetNode(ShinyLocation).Walkable && ShinyLocation != ThePlayer.CurrentPoint)                
                    ShinyInPlay = true;                
            }
        }

        public void CheckCollisions()
        {
            if (ThePlayer.CurrentPoint == ShinyLocation && ShinyInPlay)
            {
                PlayerScore += 100;
                PlaceShiny();
            }

            PauseRendering = true;
            List<Enemy> removelist = new List<Enemy>();

            foreach (Enemy enemy in Enemies)
            {
                if (!removelist.Contains(enemy))
                {
                    if (BoundingBox.Intersects(enemy.Bounds, ThePlayer.Bounds))
                    {
                        if (!removelist.Contains(enemy))
                            removelist.Add(enemy);
                        PlayerLife -= GameTypes.GAME_DAMAGE_PER_ENEMY;

                        if (PlayerLife <= 0)
                        {                            
                            Gameover g = new Gameover();
                            g.ShowDialog();
                            init();
                            PathFinding.Instance.init(GameTypes.MAP_WIDTH,GameTypes.MAP_HEIGHT);

                            Maze testmaze = new Maze();
                            testmaze.Initialize();
                            testmaze.Generate();
                            int[,] walkable = new int[GameTypes.MAP_WIDTH, GameTypes.MAP_WIDTH];
                            walkable = testmaze.getWalkableArray();

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
                            PlaceShiny();

                            PauseRendering = false;
                            KEYS_DOWN = 0;
                            return;
                        }
                    }
                    foreach (Enemy inner in Enemies)
                    {
                        if (inner != enemy && BoundingBox.Intersects(inner.Bounds, enemy.Bounds))
                        {
                            removelist.Add(enemy);
                            removelist.Add(inner);
                            PlayerScore += GameTypes.GAME_SCORE_PER_ENEMY * enemy.Type.id;
                            PlayerScore += GameTypes.GAME_SCORE_PER_ENEMY * inner.Type.id;
                        }
                    }
                }
            }

            foreach (Enemy remove in removelist)
            {
                Enemies.Remove(remove);                
            }

            if (Enemies.Count == 0 || (Enemies.Count<2 && removelist.Count>0))
            {
                SpawnEnemy(PlayerLevel);                
            }

            PauseRendering = false;         
        }

        public void SpawnEnemy(int maxlevel)
        {
            PauseRendering = true;

            int r = Randomiser.Next(1, maxlevel+2);          
            
            Enemy enemy;

            switch (r)
            {
                case 1: enemy = new Enemy(GameTypes.ENEMY_UNI);break;
                case 2: enemy = new Enemy(GameTypes.ENEMY_BIKE);break;
                case 3: enemy = new Enemy(GameTypes.ENEMY_TRIKE);break;
                case 4: enemy = new Enemy(GameTypes.ENEMY_QUAD);break;
                default: enemy = new Enemy(GameTypes.ENEMY_UNI); break;
            }

            LifeTimer.Stop();
            enemy.BirthTime = (float)LifeTimer.Elapsed;
            
            GameEngine.Instance.Enemies.Add(enemy);

            // make sure his path is ok
            bool pathproven = false;
            bool pointclear = false;
            Stack<PathNode> testpath = new Stack<PathNode>();
            Point start = Point.Empty;

            while (!pathproven)
            {
                start = new Point(Randomiser.Next(0, GameTypes.MAP_WIDTH), Randomiser.Next(0, GameTypes.MAP_HEIGHT));
                bool mightbeclear = true;

                foreach(Enemy inner in Enemies)
                {
                    if(enemy != inner)
                    {
                        if (inner.LastPoint == start || inner.NextPoint == start)
                        {
                            mightbeclear = false;
                            break;
                        }
                    }
                }

                if(mightbeclear)
                {
                    if (PathFinding.Instance.GetNode(start).Walkable)
                    {                                        
                        PathFinding.Instance.GetPath(testpath, start, GameEngine.Instance.ThePlayer.CurrentPoint);
                        pathproven = (testpath != null && testpath.Count > 2);                    
                    }
                }
            }

            enemy.BeamTo(start);

            PauseRendering = false;
        }


        #region singleton stuff

        GameEngine()
        {
        }

        public static GameEngine Instance
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

            internal static readonly GameEngine instance = new GameEngine();
        }

        #endregion
    }
}