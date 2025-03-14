using Eden.Util;
using Raylib_cs;

namespace Eden
{
    public class Engine
    {
        public static Engine Instance;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public string Title { get; private set; }

        public Scene Scene { get => currentScene; set => nextScene = value; }

        public static string Log = "";

        public int FPS {
            get => TargetFPS;
            set {
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

            FPS = Raylib.GetMonitorRefreshRate(0);
        }

        public void Run()
        {
            Init();

            while (!Raylib.WindowShouldClose())
            {
                Update();
                Draw();
            }
        }

        public virtual void Init()
        {
            Raylib.SetTargetFPS(TargetFPS);
            Raylib.InitWindow(Width, Height, Title);

            Tracker.Initialize();
        }


        public void Update()
        {
            if(nextScene != null){
                currentScene = nextScene;
            }
            Scene.BeforeUpdate();
            Scene.Update();
        }

        public void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
            Scene.Render();
            Raylib.DrawText(Log, 1, 1, 20, Color.White);
            Raylib.EndDrawing();
        }

        public static void WriteLine(string str){
            Log += str + "\n";
        }
    }
}