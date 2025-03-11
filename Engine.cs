using Raylib_cs;

namespace Eden
{
    public class Engine
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public string Title { get; private set; }

        public Scene Scene { get => currentScene; set => nextScene = value; }

        protected int FPS {
            get => TargetFPS;
            set {
                TargetFPS = value;
                Raylib.SetTargetFPS(value);
            }
        }

        private int TargetFPS = 60;

        private Scene currentScene = new Scene();
        private Scene? nextScene = null;

        public Engine(int width, int height, string title)
        {
            Width = width;
            Height = height;
            Title = title;
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
        }


        public void Update()
        {
            if(nextScene != null){
                currentScene = nextScene;
            }
            Scene.Update();
        }

        public void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
            Scene.Draw();
            Raylib.EndDrawing();
        }
    }
}