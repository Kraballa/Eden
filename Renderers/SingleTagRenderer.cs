using System.Numerics;
using Eden.Util;
using Raylib_cs;

namespace Eden.Renderers
{
    public class SingleTagRenderer : Renderer
    {
        public BitTag Tag;

        public SingleTagRenderer(BitTag tag)
        {
            Tag = tag;
        }

        public override void BeforeRender(Scene scene)
        {
        }

        public override void Render(Scene scene)
        {
            foreach (var entity in scene[Tag])
                if (entity.Visible)
                    entity.Render();
        }

        public override void AfterRender(Scene scene)
        {
        }
    }
}