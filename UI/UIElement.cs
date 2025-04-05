using Raylib_cs;

namespace Eden.UI
{
    public class UIElement
    {
        public Point Position;
        public Point Dimension;
        public Sizing SizingX;
        public Sizing SizingY;
        public Layout Layout;

        public int X
        {
            get => Position.X;
            set => Position.X = value;
        }

        public int Y
        {
            get => Position.Y;
            set => Position.Y = value;
        }

        public int Width
        {
            get => Dimension.X;
            set => Dimension.X = value;
        }

        public int Height
        {
            get => Dimension.Y;
            set => Dimension.Y = value;
        }

        public int Left { get => Padding[0]; }
        public int Top { get => Padding[1]; }
        public int Right { get => Padding[2]; }
        public int Bottom { get => Padding[3]; }

        public UIElement? Parent;
        public List<UIElement> Children = new();
        public Color Background;
        /// <summary>
        /// left, top, right, bottom
        /// </summary>
        public int[] Padding = [10, 10, 10, 10];
        public int ChildGap = 10;

        public UIElement()
        {
        }

        public UIElement(Sizing sizingX, Sizing sizingY)
        {
            SizingX = sizingX;
            SizingY = sizingY;
        }

        public UIElement Add(params UIElement[] elems)
        {
            foreach (UIElement elem in elems)
            {
                elem.Parent = this;
                Children.Add(elem);
            }
            return this;
        }

        public void CalcLayout()
        {
            FitSizing();
            GrowSizing();
            PositionChildren();
        }

        private void FitSizing()
        {
            if (SizingX.Type == SType.Fixed)
            {
                Width = SizingX.Value;
            }
            else if (SizingX.Type == SType.Fit)
            {
                Width = Left + Right;
                if (Layout == Layout.Horizontal)
                {
                    Width += ChildGap * (Children.Count - 1);
                }
            }
            if (SizingY.Type == SType.Fixed)
            {
                Height = SizingY.Value;
            }
            else if (SizingY.Type == SType.Fit)
            {
                Height = Top + Bottom;
                if (Layout == Layout.Horizontal)
                {
                    Height += ChildGap * (Children.Count - 1);
                }
            }

            foreach (UIElement child in Children)
            {
                //reverse breadth first
                //OPEN

                child.FitSizing();

                //CLOSE
                if (Parent == null) continue;

                if (Parent.Layout == Layout.Horizontal)
                {
                    if (Parent.SizingX.Type == SType.Fit) Parent.Width += Width;
                    if (Parent.SizingY.Type == SType.Fit) Parent.Height = Math.Max(Parent.Height, Height + Parent.Top + Parent.Bottom);
                }
                else if (Parent.Layout == Layout.Vertical)
                {
                    if (Parent.SizingX.Type == SType.Fit) Parent.Width = Math.Max(Parent.Width, Width + Parent.Left + Parent.Right);
                    if (Parent.SizingY.Type == SType.Fit) Parent.Height += Height;
                }
            }
        }

        private void GrowSizing()
        {
            int remainingWidth = Width - Left - Right;
            int remainingHeight = Height - Top - Bottom;

            int numGrowingChildren = 0;

            foreach (UIElement child in Children)
            {
                if (Layout == Layout.Horizontal)
                {
                    remainingWidth -= child.Width;
                    if (child.SizingX.Type == SType.Grow)
                    {
                        numGrowingChildren++;
                    }
                }
                else
                {
                    remainingHeight -= child.Height;
                    if (child.SizingY.Type == SType.Grow)
                    {
                        numGrowingChildren++;
                    }
                }
            }

            int growingChildValue;
            //this much error is being introduced per child
            float roundingErrorPerChild;
            if (Layout == Layout.Horizontal)
            {
                remainingWidth -= (Children.Count - 1) * ChildGap;
                growingChildValue = (int)Math.Floor((float)remainingWidth / numGrowingChildren);
                roundingErrorPerChild = (float)(remainingWidth - growingChildValue * numGrowingChildren) / numGrowingChildren;
            }
            else
            {
                remainingHeight -= (Children.Count - 1) * ChildGap;
                growingChildValue = (int)Math.Floor((float)remainingHeight / numGrowingChildren);
                roundingErrorPerChild = (float)(remainingHeight - growingChildValue * numGrowingChildren) / numGrowingChildren;
            }

            float runningError = 0;

            foreach (UIElement child in Children)
            {
                if (child.SizingX.Type == SType.Grow)
                {
                    if (Layout == Layout.Horizontal)
                    {
                        runningError += roundingErrorPerChild;
                        child.Width += growingChildValue;
                        if (runningError >= 1)
                        {
                            child.Width++;
                            runningError--;
                        }
                    }
                    else
                    {
                        child.Width += remainingWidth;
                    }
                }
                if (child.SizingY.Type == SType.Grow)
                {
                    if (Layout == Layout.Horizontal)
                    {
                        child.Height += remainingHeight;
                    }
                    else
                    {
                        runningError += roundingErrorPerChild;
                        child.Height += growingChildValue;
                        if (runningError >= 1)
                        {
                            child.Height++;
                            runningError--;
                        }
                    }
                }
            }

            foreach (UIElement child in Children)
            {
                child.GrowSizing();
            }
        }

        private void PositionChildren()
        {
            Point nextPosRelative = new(Left, Top);
            foreach (UIElement child in Children)
            {
                child.Position = nextPosRelative + Position;
                child.PositionChildren();
                if (Layout == Layout.Horizontal) nextPosRelative.X += child.Width + ChildGap;
                if (Layout == Layout.Vertical) nextPosRelative.Y += child.Height + ChildGap;
            }
        }

        public virtual void Render()
        {
            string str = Position.ToString() + " ";
            if (SizingX.Type == SType.Fit) str += "*";
            else if (SizingX.Type == SType.Grow) str += "^";
            str += Width + ":";
            if (SizingY.Type == SType.Fit) str += "*";
            else if (SizingY.Type == SType.Grow) str += "^";
            str += Height;

            Raylib.DrawRectangle(Position.X, Position.Y, Dimension.X, Dimension.Y, Background);
            Raylib.DrawText(str, Position.X, Position.Y, 10, Color.White);
            foreach (UIElement child in Children)
            {
                child.Render();
            }
        }
    }
}