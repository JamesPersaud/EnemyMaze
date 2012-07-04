using System;
using System.Drawing;
using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D9;
using SlimDX.Windows;

namespace ld18_EnemyDefence
{
    public sealed class WindowManager
    {    

        public RenderForm Slim;
        public MainForm Main;
        public bool MouseOverDX;

        public Point mouseGridLocation = new Point(0,0);
        public Vector3 mouseVector = Vector3.Zero;

        public void init(Device d)
        {            
        }

        #region singleton stuff

        WindowManager()
        {
        }

        public static WindowManager Instance
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

            internal static readonly WindowManager instance = new WindowManager();
        }

        #endregion
    }
}