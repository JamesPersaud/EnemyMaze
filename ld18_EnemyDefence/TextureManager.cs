using System;
using System.Drawing;
using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D9;
using SlimDX.Windows;

namespace ld18_EnemyDefence
{
    public sealed class TextureManager
    {
        public Device device;
        public Texture ddsTestTile;
        public Texture ddsTestTileOut;
        public Texture ddsTestUni;
        public Texture ddsMouseover;
        public Texture pngDropShadow;
        public Texture pngMouse;
        public Texture pngShiny;

        public Texture tgaEnemies;

        public Sprite spritey;            

        public void init(Device d)
        {
            device = d;
            ddsTestTile = Texture.FromFile(d, "assets\\textures\\test_tile.png");
            ddsTestUni = Texture.FromFile(d, "assets\\textures\\test_uni.dds");
            ddsTestTileOut = Texture.FromFile(d, "assets\\textures\\test_tile_out.png");
            ddsMouseover = Texture.FromFile(d, "assets\\textures\\squaretint.png");
            pngDropShadow = Texture.FromFile(d, "assets\\textures\\dropshadow.png");
            pngMouse = Texture.FromFile(d, "assets\\textures\\mousecursor.png");
            pngShiny = Texture.FromFile(d, "assets\\textures\\shiny.png");
            tgaEnemies = Texture.FromFile(d, "assets\\textures\\enemies.png",192,128,1,Usage.None,Format.A32B32G32R32F,Pool.Default,Filter.Point,Filter.Point,0);
            spritey = new Sprite(device);

            tgaEnemies.AutoMipGenerationFilter = TextureFilter.Anisotropic;            
        }

        #region singleton stuff

        TextureManager()
        {
        }

        public static TextureManager Instance
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

            internal static readonly TextureManager instance = new TextureManager();
        }

        #endregion
    }
}