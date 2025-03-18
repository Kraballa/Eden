using System.Numerics;
using Raylib_cs;

namespace Eden {
    public class Transform2D {
        public Vector2 Position;
        public float Rotation;
        public Vector2 Scale;

        public Transform2D() : this(Vector2.Zero){

        }

        public Transform2D(Vector2 pos){
            Position = pos;
            Scale = Vector2.One;
            Rotation = 0;
        }
    }
}