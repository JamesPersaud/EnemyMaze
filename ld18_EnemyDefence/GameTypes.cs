using System;
using System.Drawing;

namespace ld18_EnemyDefence
{    
    public class GameTypes
    {
        public const float GAME_INITIAL_SPAWN_FREQ = 16;
        public const float GAME_SPAWN_SPEEDUP_RATE = 0.5f;
        public const float GAME_SPAWN_SPEEDUP_FREQ = 16;
        public const float GAME_SPAWN_MIN_FREQ = 1.0f;
        public static float[] GAME_LEVEL_FREQ = new float[]{16,14,12,10};
        public const int GAME_SCORE_PER_ENEMY = 5;
        public const int GAME_DAMAGE_PER_ENEMY = 10;

        public static float BASE_MOVE_SPEED = 0.1f;

        public  const int MAP_WIDTH = 27;
        public  const int MAP_HEIGHT = 16;

        public  const int SPRITE_WIDTH = 32;
        public  const int SPRITE_HEIGHT = 32;
        public static Point SPRITE_TOWER_OFFSET = new Point(0,128);

        public static Point ENEMY_START_COORDS = new Point(0,8);
        public static Point ENEMY_END_COORDS = new Point(19, 0);
        public  const int ENEMY_START_DIRECTION = GameTypes.ENEMY_DIRECTION_RIGHT;

        public static Point PLAYER_START_COORDS = new Point(14, 8);
        public const int PLAYER_START_DIRECTION = GameTypes.ENEMY_DIRECTION_DOWN;

        public  const int ENEMY_ID_UNI = 0x1;
        public  const int ENEMY_ID_BIKE = 0x2;
        public  const int ENEMY_ID_TRIKE = 0x4;
        public  const int ENEMY_ID_QUAD = 0x6;
        public  const int ENEMY_ID_FLY = 0x8;
        public  const int ENEMY_ID_GUN = 0xA;

        public  const int ENEMY_DIRECTION_UP = 0x1;
        public  const int ENEMY_DIRECTION_DOWN = 0x2;
        public  const int ENEMY_DIRECTION_LEFT = 0x4;
        public  const int ENEMY_DIRECTION_RIGHT = 0x6;     

        public const int MOVEKEY_A = 0x1;
        public const int MOVEKEY_W = 0x2;
        public const int MOVEKEY_S = 0x4;
        public const int MOVEKEY_D = 0x8;


        // player

        public static EnemyType PLAYER_TYPE = new EnemyType(ENEMY_ID_UNI, "Unicykill", "A primitive armed scout model, gun turret fodder.", "Fires pulse lasers.",
            new Rectangle(SPRITE_WIDTH * 2, SPRITE_HEIGHT * 2, SPRITE_WIDTH, SPRITE_HEIGHT),
            new Rectangle(SPRITE_WIDTH * 3, SPRITE_HEIGHT * 2, SPRITE_WIDTH, SPRITE_HEIGHT),
            new Rectangle(SPRITE_WIDTH * 2, SPRITE_HEIGHT * 3, SPRITE_WIDTH, SPRITE_HEIGHT),
            new Rectangle(SPRITE_WIDTH * 3, SPRITE_HEIGHT * 3, SPRITE_WIDTH, SPRITE_HEIGHT),
            20, 14, 5, 1, 5, 5, ENEMY_ID_UNI, 10, 2);


        //start with

        public static EnemyType ENEMY_GUN = new EnemyType(ENEMY_ID_UNI, "Unicykill", "A primitive armed scout model, gun turret fodder.", "Fires pulse lasers.",
            new Rectangle(SPRITE_WIDTH * 0, SPRITE_HEIGHT * 0, SPRITE_WIDTH, SPRITE_HEIGHT),
            new Rectangle(SPRITE_WIDTH * 1, SPRITE_HEIGHT * 0, SPRITE_WIDTH, SPRITE_HEIGHT),
            new Rectangle(SPRITE_WIDTH * 0, SPRITE_HEIGHT * 1, SPRITE_WIDTH, SPRITE_HEIGHT),
            new Rectangle(SPRITE_WIDTH * 1, SPRITE_HEIGHT * 1, SPRITE_WIDTH, SPRITE_HEIGHT),
            20, 8, 5, 1, 5, 5, ENEMY_ID_UNI, 10, 2);


       //start against

        public static EnemyType ENEMY_UNI = new EnemyType(ENEMY_ID_UNI, "Unicykill", "A primitive armed scout model, gun turret fodder.", "Fires pulse lasers.",
            new Rectangle(SPRITE_WIDTH * 0, SPRITE_HEIGHT*0, SPRITE_WIDTH, SPRITE_HEIGHT),
            new Rectangle(SPRITE_WIDTH * 1, SPRITE_HEIGHT * 0, SPRITE_WIDTH, SPRITE_HEIGHT),
            new Rectangle(SPRITE_WIDTH * 0, SPRITE_HEIGHT * 1, SPRITE_WIDTH, SPRITE_HEIGHT),
            new Rectangle(SPRITE_WIDTH * 1, SPRITE_HEIGHT * 1, SPRITE_WIDTH, SPRITE_HEIGHT),
            20, 10, 10, 50, 5, 10, ENEMY_ID_BIKE,10,2);

        public static EnemyType ENEMY_BIKE = new EnemyType(ENEMY_ID_BIKE, "Terrorbike", "An armed scout, fast and hard to target.", "Fires fireballs.",
            new Rectangle(SPRITE_WIDTH * 2, SPRITE_HEIGHT * 0, SPRITE_WIDTH, SPRITE_HEIGHT),
            new Rectangle(SPRITE_WIDTH * 3, SPRITE_HEIGHT * 0, SPRITE_WIDTH, SPRITE_HEIGHT),
            new Rectangle(SPRITE_WIDTH * 2, SPRITE_HEIGHT * 1, SPRITE_WIDTH, SPRITE_HEIGHT),
            new Rectangle(SPRITE_WIDTH * 3, SPRITE_HEIGHT * 1, SPRITE_WIDTH, SPRITE_HEIGHT),
            35, 14, 30, 150, 10, 20, ENEMY_ID_TRIKE, 10,5);

        public static EnemyType ENEMY_TRIKE = new EnemyType(ENEMY_ID_UNI, "Megatrike", "Assault trike, has highly flammable ammo.", "Fires A-P rockets",
            new Rectangle(SPRITE_WIDTH * 4, SPRITE_HEIGHT * 0, SPRITE_WIDTH, SPRITE_HEIGHT),
            new Rectangle(SPRITE_WIDTH * 5, SPRITE_HEIGHT * 0, SPRITE_WIDTH, SPRITE_HEIGHT),
            new Rectangle(SPRITE_WIDTH * 4, SPRITE_HEIGHT * 1, SPRITE_WIDTH, SPRITE_HEIGHT),
            new Rectangle(SPRITE_WIDTH * 5, SPRITE_HEIGHT * 1, SPRITE_WIDTH, SPRITE_HEIGHT),
            50, 16, 60, 450, 30, 20, ENEMY_ID_QUAD,10,10);

        public static EnemyType ENEMY_QUAD = new EnemyType(ENEMY_ID_UNI, "Deathkart", "Frontline tank, has thick armour.", "Fires guided missiles (only targets air)",
            new Rectangle(SPRITE_WIDTH * 0, SPRITE_HEIGHT * 2, SPRITE_WIDTH, SPRITE_HEIGHT),
            new Rectangle(SPRITE_WIDTH * 1, SPRITE_HEIGHT * 2, SPRITE_WIDTH, SPRITE_HEIGHT),
            new Rectangle(SPRITE_WIDTH * 0, SPRITE_HEIGHT * 3, SPRITE_WIDTH, SPRITE_HEIGHT),
            new Rectangle(SPRITE_WIDTH * 1, SPRITE_HEIGHT * 3, SPRITE_WIDTH, SPRITE_HEIGHT),
            300, 20, 200, 1000, 100, 60, ENEMY_ID_FLY,10,20);

        public static EnemyType ENEMY_FLY = new EnemyType(ENEMY_ID_UNI, "The Sauceror", "Flies over towers.", "Fires death rays, can abduct enemies.",
            new Rectangle(SPRITE_WIDTH * 2, SPRITE_HEIGHT * 2, SPRITE_WIDTH, SPRITE_HEIGHT),
            new Rectangle(SPRITE_WIDTH * 3, SPRITE_HEIGHT * 2, SPRITE_WIDTH, SPRITE_HEIGHT),
            new Rectangle(SPRITE_WIDTH * 2, SPRITE_HEIGHT * 3, SPRITE_WIDTH, SPRITE_HEIGHT),
            new Rectangle(SPRITE_WIDTH * 3, SPRITE_HEIGHT * 3, SPRITE_WIDTH, SPRITE_HEIGHT),
            500, 4, 500, 5000, 500, 40, ENEMY_ID_BIKE,10,100);
    }

    public struct EnemyType
    {
        public int id;
        public string Name;
        public string Description;
        public string TowerDescription;
        public Rectangle UpRect;
        public Rectangle DownRect;
        public Rectangle LeftRect;
        public Rectangle RightRect;
        public int Health;
        public int Speed;
        public int Cost;
        public int CaptureCost;
        public int Points;
        public int Damage;
        public int TargetType;
        public int FireSpeed;
        public int Reward;

        public EnemyType(int i, string n, string d, string td, Rectangle ur, Rectangle dr, Rectangle lr, Rectangle rr, int h, int s, int c, int cc, int p, int dm, int tt,int fs,int rw)
        {
            id = i; Name = n; Description = d; TowerDescription = td; UpRect = ur; DownRect = dr; LeftRect = lr; RightRect = rr; Health = h; Speed = s; Cost = c; CaptureCost = cc;
            Points = p; Damage = dm; TargetType = tt; FireSpeed = fs; Reward = rw;
        }
    }
}