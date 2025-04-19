using System.Numerics;
using Raylib_cs;

namespace Eden
{
    public class Engine
    {
        public static Engine Instance;

        public static Vector2 ScreenCenter => new Vector2(Instance.Width / 2, Instance.Height / 2);

        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool Debug { get; set; }
        public string Title { get; private set; }

        public Scene Scene { get => currentScene; set => nextScene = value; }

        public static string Log = "";

        public int FPS
        {
            get => TargetFPS;
            set
            {
                TargetFPS = value;
                Raylib.SetTargetFPS(value);
            }
        }

        private int TargetFPS;

        private Scene currentScene;
        private Scene? nextScene = null;

        public Engine(int width, int height, string title)
        {
            Instance = this;
            Width = width;
            Height = height;
            Title = title;

            //FPS = 60;
        }

        public void Run()
        {
            Init();

            while (!Raylib.WindowShouldClose())
            {
                MInput.Update();
                Update();
                Draw();
            }
        }

        protected virtual void Init()
        {
            MInput.Initialize();
            Raylib.SetConfigFlags(ConfigFlags.Msaa4xHint);
            Raylib.SetTargetFPS(TargetFPS);
            Raylib.InitWindow(Width, Height, Title);

            Tracker.Initialize();
        }

        public void Update()
        {
#if DEBUG
            if (Raylib.IsKeyPressed(KeyboardKey.LeftAlt))
            {
                Debug = !Debug;
            }
#endif
            if (Raylib.IsWindowResized())
            {
                Width = Raylib.GetScreenWidth();
                Height = Raylib.GetScreenHeight();
            }
            if (nextScene != null)
            {
                currentScene = nextScene;
                currentScene.Begin();
                nextScene = null;
            }
            Scene.BeforeUpdate();
            Scene.Update();
        }

        public void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
            Scene.BeforeRender();
            Scene.Render();
            if(Debug){
            }
            Scene.AfterRender();
            Raylib.DrawText(Log, 1, 1, 20, Color.White);
            if (Debug)
            {
                Raylib.DrawFPS(1, 1);
            }
            Raylib.EndDrawing();
        }

        public static void WriteLine(string str)
        {
            Log += str + "\n";
        }
    }
}