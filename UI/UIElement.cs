using Raylib_cs;

namespace Eden.UI
{
    /// <summary>
    /// parent element of all ui elements including containers.
    /// </summary>
    public class UIElement
    {
        public static float RenderScale = 1;

        public int ChildIndex = -1;
        public Point Position;
        public Point Dimension;
        public Sizing SizingX;
        public Sizing SizingY;
        public Layout Layout;
        public bool Selectable = false;
        public Action OnClick;

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
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].ChildIndex = i;
            }
            OnBeforeCalcLayout();
            FitSizing();
            GrowSizing();
            PositionChildren();
        }

        protected virtual void OnBeforeCalcLayout()
        {
            foreach (UIElement child in Children)
            {
                child.OnBeforeCalcLayout();
            }
        }

        public virtual void Render()
        {
            Raylib.DrawRectangle(Position.X, Position.Y, Dimension.X, Dimension.Y, Background);
            if (Engine.Instance != null && Engine.Instance.Debug)
            {
                string str = Position.ToString() + " ";
                if (SizingX.Type == SType.Fit) str += "*";
                else if (SizingX.Type == SType.Grow) str += "^";
                str += Width + ":";
                if (SizingY.Type == SType.Fit) str += "*";
                else if (SizingY.Type == SType.Grow) str += "^";
                str += Height;
                Raylib.DrawText(str, Position.X, Position.Y, 10, Color.White);
            }

            foreach (UIElement child in Children)
            {
                child.Render();
            }
        }

        public UIElement? Hovering()
        {
            Point mPos = (Raylib.GetMousePosition() / RenderScale).ToPoint();

            if (mPos.X < X || mPos.X > X + Width - 1 || mPos.Y < Y || mPos.Y > Y + Height - 1)
            {
                return null;
            }

            foreach (UIElement child in Children)
            {
                UIElement? maybeHover = child.Hovering();
                if (child.Hovering() != null)
                {
                    return maybeHover;
                }
            }

            return Selectable ? this : null;
        }

        protected virtual void FitSizing()
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
                if (Layout == Layout.Vertical)
                {
                    Height += ChildGap * (Children.Count - 1);
                }
            }

            foreach (UIElement child in Children)
            {
                child.FitSizing();

                if (Layout == Layout.Horizontal)
                {
                    if (SizingX.Type == SType.Fit) Width += child.Width;
                    if (SizingY.Type == SType.Fit) Height = Math.Max(Height, child.Height + Top + Bottom);
                }
                else if (Layout == Layout.Vertical)
                {
                    if (SizingX.Type == SType.Fit) Width = Math.Max(Width, child.Width + Left + Right);
                    if (SizingY.Type == SType.Fit) Height += child.Height;
                }
            }
        }

        protected virtual void GrowSizing()
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

        protected virtual void PositionChildren()
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

        public UIElement GetNextSibling(bool wrap)
        {
            if (Parent == null || ChildIndex == -1) //same thing but w/e
            {
                return this;
            }
            int nextIdx = ChildIndex + 1;
            if (nextIdx >= Parent.Children.Count)
            {
                if (wrap)
                {
                    nextIdx %= Parent.Children.Count;
                }
                else
                {
                    nextIdx = Parent.Children.Count - 1;
                }
            }
            return Parent.Children[nextIdx];
        }

        public UIElement GetPrevSibling(bool wrap)
        {
            if (Parent == null || ChildIndex == -1) //same thing but w/e
            {
                return this;
            }
            int nextIdx = ChildIndex - 1;
            if (nextIdx < 0)
            {
                if (wrap)
                {
                    nextIdx += Parent.Children.Count;
                }
                else
                {
                    nextIdx = 0;
                }
            }
            return Parent.Children[nextIdx];
        }

        // public UIElement GetSiblingRight(bool wrap){

        // }
    }
}