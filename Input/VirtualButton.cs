using Raylib_cs;

namespace Eden
{
    /// <summary>
    /// A virtual input that is represented as a boolean. As well as simply checking the current button state, you can ask whether it was just pressed or released this frame. You can also keep the button press stored in a buffer for a limited time, or until it is consumed by calling ConsumeBuffer()
    /// </summary>
    public class VirtualButton : VirtualInput
    {
        public List<Node> Nodes;
        public float BufferTime;
        public bool Repeating { get; private set; }

        private float firstRepeatTime;
        private float multiRepeatTime;
        private float bufferCounter;
        private float repeatCounter;
        private bool canRepeat;
        private bool consumed;

        public VirtualButton(float bufferTime)
            : base()
        {
            Nodes = new List<Node>();
            BufferTime = bufferTime;
        }

        public VirtualButton()
            : this(0)
        {

        }

        public VirtualButton(float bufferTime, params Node[] nodes)
            : base()
        {
            Nodes = new List<Node>(nodes);
            BufferTime = bufferTime;
        }

        public VirtualButton(params Node[] nodes)
            : this(0, nodes)
        {

        }

        public void SetRepeat(float repeatTime)
        {
            SetRepeat(repeatTime, repeatTime);
        }

        public void SetRepeat(float firstRepeatTime, float multiRepeatTime)
        {
            this.firstRepeatTime = firstRepeatTime;
            this.multiRepeatTime = multiRepeatTime;
            canRepeat = (this.firstRepeatTime > 0);
            if (!canRepeat)
                Repeating = false;
        }

        public override void Update()
        {
            consumed = false;
            bufferCounter -= Raylib.GetFrameTime();

            bool check = false;
            foreach (var node in Nodes)
            {
                node.Update();
                if (node.Pressed)
                {
                    bufferCounter = BufferTime;
                    check = true;
                }
                else if (node.Check)
                    check = true;
            }

            if (!check)
            {
                Repeating = false;
                repeatCounter = 0;
                bufferCounter = 0;
            }
            else if (canRepeat)
            {
                Repeating = false;
                if (repeatCounter == 0)
                    repeatCounter = firstRepeatTime;
                else
                {
                    repeatCounter -= Raylib.GetFrameTime();
                    if (repeatCounter <= 0)
                    {
                        Repeating = true;
                        repeatCounter = multiRepeatTime;
                    }
                }
            }
        }

        public bool Check
        {
            get
            {
                if (MInput.Disabled)
                    return false;

                foreach (var node in Nodes)
                    if (node.Check)
                        return true;
                return false;
            }
        }

        public bool Pressed
        {
            get
            {
                if (MInput.Disabled)
                    return false;

                if (consumed)
                    return false;

                if (bufferCounter > 0 || Repeating)
                    return true;

                foreach (var node in Nodes)
                    if (node.Pressed)
                        return true;
                return false;
            }
        }

        public bool Released
        {
            get
            {
                if (MInput.Disabled)
                    return false;

                foreach (var node in Nodes)
                    if (node.Released)
                        return true;
                return false;
            }
        }

        /// <summary>
        /// Ends the Press buffer for this button
        /// </summary>
        public void ConsumeBuffer()
        {
            bufferCounter = 0;
        }

        /// <summary>
        /// This button will not register a Press for the rest of the current frame, but otherwise continues to function normally. If the player continues to hold the button, next frame will not count as a Press. Also ends the Press buffer for this button
        /// </summary>
        public void ConsumePress()
        {
            bufferCounter = 0;
            consumed = true;
        }

        public static implicit operator bool(VirtualButton button)
        {
            return button.Check;
        }

        public abstract class Node : VirtualInputNode
        {
            public abstract bool Check { get; }
            public abstract bool Pressed { get; }
            public abstract bool Released { get; }
        }

        public class BoardKey : Node
        {
            public KeyboardKey Key;

            public BoardKey(KeyboardKey key)
            {
                Key = key;
            }

            public override bool Check
            {
                get { return MInput.Keyboard.Check(Key); }
            }

            public override bool Pressed
            {
                get { return MInput.Keyboard.Pressed(Key); }
            }

            public override bool Released
            {
                get { return MInput.Keyboard.Released(Key); }
            }
        }

        public class PadButton : Node
        {
            public int GamepadIndex;
            public GamepadButton Button;

            public PadButton(int gamepadIndex, GamepadButton button)
            {
                GamepadIndex = gamepadIndex;
                Button = button;
            }

            public override bool Check
            {
                get { return MInput.GamePads[GamepadIndex].Check(Button); }
            }

            public override bool Pressed
            {
                get { return MInput.GamePads[GamepadIndex].Pressed(Button); }
            }

            public override bool Released
            {
                get { return MInput.GamePads[GamepadIndex].Released(Button); }
            }
        }

        #region Pad Left Stick

        public class PadLeftStickRight : Node
        {
            public int GamepadIndex;
            public float Deadzone;

            private bool IsDown;
            private bool WasDown;

            public PadLeftStickRight(int gamepadindex, float deadzone)
            {
                GamepadIndex = gamepadindex;
                Deadzone = deadzone;
            }

            public override void Update()
            {
                WasDown = IsDown;
                base.Update();
                IsDown = MInput.GamePads[GamepadIndex].LeftStickRightCheck(Deadzone);
            }

            public override bool Check
            {
                get { return IsDown; }
            }

            public override bool Pressed
            {
                get { return IsDown && !WasDown; }
            }

            public override bool Released
            {
                get { return WasDown && !IsDown; }
            }
        }

        public class PadLeftStickLeft : Node
        {
            public int GamepadIndex;
            public float Deadzone;

            private bool IsDown;
            private bool WasDown;

            public PadLeftStickLeft(int gamepadindex, float deadzone)
            {
                GamepadIndex = gamepadindex;
                Deadzone = deadzone;
            }

            public override void Update()
            {
                WasDown = IsDown;
                base.Update();
                IsDown = MInput.GamePads[GamepadIndex].LeftStickLeftCheck(Deadzone);
            }

            public override bool Check
            {
                get { return IsDown; }
            }

            public override bool Pressed
            {
                get { return IsDown && !WasDown; }
            }

            public override bool Released
            {
                get { return WasDown && !IsDown; }
            }
        }

        public class PadLeftStickUp : Node
        {
            public int GamepadIndex;
            public float Deadzone;

            private bool IsDown;
            private bool WasDown;

            public PadLeftStickUp(int gamepadindex, float deadzone)
            {
                GamepadIndex = gamepadindex;
                Deadzone = deadzone;
            }

            public override void Update()
            {
                WasDown = IsDown;
                base.Update();
                IsDown = MInput.GamePads[GamepadIndex].LeftStickUpCheck(Deadzone);
            }

            public override bool Check
            {
                get { return IsDown; }
            }

            public override bool Pressed
            {
                get { return IsDown && !WasDown; }
            }

            public override bool Released
            {
                get { return WasDown && !IsDown; }
            }
        }

        public class PadLeftStickDown : Node
        {
            public int GamepadIndex;
            public float Deadzone;

            private bool IsDown;
            private bool WasDown;

            public PadLeftStickDown(int gamepadindex, float deadzone)
            {
                GamepadIndex = gamepadindex;
                Deadzone = deadzone;
            }

            public override void Update()
            {
                WasDown = IsDown;
                base.Update();
                IsDown = MInput.GamePads[GamepadIndex].LeftStickDownCheck(Deadzone);
            }

            public override bool Check
            {
                get { return IsDown; }
            }

            public override bool Pressed
            {
                get { return IsDown && !WasDown; }
            }

            public override bool Released
            {
                get { return WasDown && !IsDown; }
            }
        }

        #endregion

        #region Pad Right Stick

        public class PadRightStickRight : Node
        {
            public int GamepadIndex;
            public float Deadzone;

            private bool IsDown;
            private bool WasDown;

            public PadRightStickRight(int gamepadindex, float deadzone)
            {
                GamepadIndex = gamepadindex;
                Deadzone = deadzone;
            }

            public override void Update()
            {
                WasDown = IsDown;
                base.Update();
                IsDown = MInput.GamePads[GamepadIndex].RightStickRightCheck(Deadzone);
            }

            public override bool Check
            {
                get { return IsDown; }
            }

            public override bool Pressed
            {
                get { return IsDown && !WasDown; }
            }

            public override bool Released
            {
                get { return WasDown && !IsDown; }
            }
        }

        public class PadRightStickLeft : Node
        {
            public int GamepadIndex;
            public float Deadzone;

            private bool IsDown;
            private bool WasDown;

            public PadRightStickLeft(int gamepadindex, float deadzone)
            {
                GamepadIndex = gamepadindex;
                Deadzone = deadzone;
            }

            public override void Update()
            {
                WasDown = IsDown;
                base.Update();
                IsDown = MInput.GamePads[GamepadIndex].RightStickLeftCheck(Deadzone);
            }

            public override bool Check
            {
                get { return IsDown; }
            }

            public override bool Pressed
            {
                get { return IsDown && !WasDown; }
            }

            public override bool Released
            {
                get { return WasDown && !IsDown; }
            }
        }

        public class PadRightStickUp : Node
        {
            public int GamepadIndex;
            public float Deadzone;

            private bool IsDown;
            private bool WasDown;

            public PadRightStickUp(int gamepadindex, float deadzone)
            {
                GamepadIndex = gamepadindex;
                Deadzone = deadzone;
            }

            public override void Update()
            {
                WasDown = IsDown;
                base.Update();
                IsDown = MInput.GamePads[GamepadIndex].RightStickUpCheck(Deadzone);
            }

            public override bool Check
            {
                get { return IsDown; }
            }

            public override bool Pressed
            {
                get { return IsDown && !WasDown; }
            }

            public override bool Released
            {
                get { return WasDown && !IsDown; }
            }
        }

        public class PadRightStickDown : Node
        {
            public int GamepadIndex;
            public float Deadzone;

            private bool IsDown;
            private bool WasDown;

            public PadRightStickDown(int gamepadindex, float deadzone)
            {
                GamepadIndex = gamepadindex;
                Deadzone = deadzone;
            }

            public override void Update()
            {
                WasDown = IsDown;
                base.Update();
                IsDown = MInput.GamePads[GamepadIndex].RightStickDownCheck(Deadzone);
            }

            public override bool Check
            {
                get { return IsDown; }
            }

            public override bool Pressed
            {
                get { return IsDown && !WasDown; }
            }

            public override bool Released
            {
                get { return WasDown && !IsDown; }
            }
        }

        #endregion

        #region Pad Triggers

        public class PadLeftTrigger : Node
        {
            public int GamepadIndex;
            public float Threshold;

            private bool IsDown;
            private bool WasDown;

            public PadLeftTrigger(int gamepadIndex, float threshold)
            {
                GamepadIndex = gamepadIndex;
                Threshold = threshold;
            }

            public override void Update()
            {
                WasDown = IsDown;
                base.Update();
                IsDown = MInput.GamePads[GamepadIndex].LeftTriggerCheck(Threshold);
            }

            public override bool Check
            {
                get { return IsDown; }
            }

            public override bool Pressed
            {
                get { return IsDown && !WasDown; }
            }

            public override bool Released
            {
                get { return WasDown && !IsDown; }
            }
        }

        public class PadRightTrigger : Node
        {
            public int GamepadIndex;
            public float Threshold;

            private bool IsDown;
            private bool WasDown;

            public PadRightTrigger(int gamepadIndex, float threshold)
            {
                GamepadIndex = gamepadIndex;
                Threshold = threshold;
            }

            public override void Update()
            {
                WasDown = IsDown;
                base.Update();
                IsDown = MInput.GamePads[GamepadIndex].RightTriggerCheck(Threshold);
            }

            public override bool Check
            {
                get { return IsDown; }
            }

            public override bool Pressed
            {
                get { return IsDown && !WasDown; }
            }

            public override bool Released
            {
                get { return WasDown && !IsDown; }
            }
        }

        #endregion

        #region Pad DPad

        public class PadDPadRight : Node
        {
            public int GamepadIndex;

            public PadDPadRight(int gamepadIndex)
            {
                GamepadIndex = gamepadIndex;
            }

            public override bool Check
            {
                get { return MInput.GamePads[GamepadIndex].DPadRightCheck; }
            }

            public override bool Pressed
            {
                get { return MInput.GamePads[GamepadIndex].DPadRightPressed; }
            }

            public override bool Released
            {
                get { return MInput.GamePads[GamepadIndex].DPadRightReleased; }
            }
        }

        public class PadDPadLeft : Node
        {
            public int GamepadIndex;

            public PadDPadLeft(int gamepadIndex)
            {
                GamepadIndex = gamepadIndex;
            }

            public override bool Check
            {
                get { return MInput.GamePads[GamepadIndex].DPadLeftCheck; }
            }

            public override bool Pressed
            {
                get { return MInput.GamePads[GamepadIndex].DPadLeftPressed; }
            }

            public override bool Released
            {
                get { return MInput.GamePads[GamepadIndex].DPadLeftReleased; }
            }
        }

        public class PadDPadUp : Node
        {
            public int GamepadIndex;

            public PadDPadUp(int gamepadIndex)
            {
                GamepadIndex = gamepadIndex;
            }

            public override bool Check
            {
                get { return MInput.GamePads[GamepadIndex].DPadUpCheck; }
            }

            public override bool Pressed
            {
                get { return MInput.GamePads[GamepadIndex].DPadUpPressed; }
            }

            public override bool Released
            {
                get { return MInput.GamePads[GamepadIndex].DPadUpReleased; }
            }
        }

        public class PadDPadDown : Node
        {
            public int GamepadIndex;

            public PadDPadDown(int gamepadIndex)
            {
                GamepadIndex = gamepadIndex;
            }

            public override bool Check
            {
                get { return MInput.GamePads[GamepadIndex].DPadDownCheck; }
            }

            public override bool Pressed
            {
                get { return MInput.GamePads[GamepadIndex].DPadDownPressed; }
            }

            public override bool Released
            {
                get { return MInput.GamePads[GamepadIndex].DPadDownReleased; }
            }
        }

        #endregion

        #region Mouse

        public class MouseLeftButton : Node
        {
            public override bool Check
            {
                get { return MInput.Mouse.CheckLeftButton; }
            }

            public override bool Pressed
            {
                get { return MInput.Mouse.PressedLeftButton; }
            }

            public override bool Released
            {
                get { return MInput.Mouse.ReleasedLeftButton; }
            }
        }

        public class MouseRightButton : Node
        {
            public override bool Check
            {
                get { return MInput.Mouse.CheckRightButton; }
            }

            public override bool Pressed
            {
                get { return MInput.Mouse.PressedRightButton; }
            }

            public override bool Released
            {
                get { return MInput.Mouse.ReleasedRightButton; }
            }
        }

        public class MouseMiddleButton : Node
        {
            public override bool Check
            {
                get { return MInput.Mouse.CheckMiddleButton; }
            }

            public override bool Pressed
            {
                get { return MInput.Mouse.PressedMiddleButton; }
            }

            public override bool Released
            {
                get { return MInput.Mouse.ReleasedMiddleButton; }
            }
        }

        #endregion

        #region Other Virtual Inputs

        public class VirtualAxisTrigger : Node
        {
            public enum Modes { LargerThan, LessThan, Equals };

            public VirtualInput.ThresholdModes Mode;
            public float Threshold;

            private VirtualAxis axis;

            public VirtualAxisTrigger(VirtualAxis axis, VirtualInput.ThresholdModes mode, float threshold)
            {
                this.axis = axis;
                Mode = mode;
                Threshold = threshold;
            }

            public override bool Check
            {
                get
                {
                    if (Mode == VirtualInput.ThresholdModes.LargerThan)
                        return axis.Value >= Threshold;
                    else if (Mode == VirtualInput.ThresholdModes.LessThan)
                        return axis.Value <= Threshold;
                    else
                        return axis.Value == Threshold;
                }
            }

            public override bool Pressed
            {
                get
                {
                    if (Mode == VirtualInput.ThresholdModes.LargerThan)
                        return axis.Value >= Threshold && axis.PreviousValue < Threshold;
                    else if (Mode == VirtualInput.ThresholdModes.LessThan)
                        return axis.Value <= Threshold && axis.PreviousValue > Threshold;
                    else
                        return axis.Value == Threshold && axis.PreviousValue != Threshold;
                }
            }

            public override bool Released
            {
                get
                {
                    if (Mode == VirtualInput.ThresholdModes.LargerThan)
                        return axis.Value < Threshold && axis.PreviousValue >= Threshold;
                    else if (Mode == VirtualInput.ThresholdModes.LessThan)
                        return axis.Value > Threshold && axis.PreviousValue <= Threshold;
                    else
                        return axis.Value != Threshold && axis.PreviousValue == Threshold;
                }
            }
        }

        public class VirtualIntegerAxisTrigger : Node
        {
            public enum Modes { LargerThan, LessThan, Equals };

            public VirtualInput.ThresholdModes Mode;
            public int Threshold;

            private VirtualIntegerAxis axis;

            public VirtualIntegerAxisTrigger(VirtualIntegerAxis axis, VirtualInput.ThresholdModes mode, int threshold)
            {
                this.axis = axis;
                Mode = mode;
                Threshold = threshold;
            }

            public override bool Check
            {
                get
                {
                    if (Mode == VirtualInput.ThresholdModes.LargerThan)
                        return axis.Value >= Threshold;
                    else if (Mode == VirtualInput.ThresholdModes.LessThan)
                        return axis.Value <= Threshold;
                    else
                        return axis.Value == Threshold;
                }
            }

            public override bool Pressed
            {
                get
                {
                    if (Mode == VirtualInput.ThresholdModes.LargerThan)
                        return axis.Value >= Threshold && axis.PreviousValue < Threshold;
                    else if (Mode == VirtualInput.ThresholdModes.LessThan)
                        return axis.Value <= Threshold && axis.PreviousValue > Threshold;
                    else
                        return axis.Value == Threshold && axis.PreviousValue != Threshold;
                }
            }

            public override bool Released
            {
                get
                {
                    if (Mode == VirtualInput.ThresholdModes.LargerThan)
                        return axis.Value < Threshold && axis.PreviousValue >= Threshold;
                    else if (Mode == VirtualInput.ThresholdModes.LessThan)
                        return axis.Value > Threshold && axis.PreviousValue <= Threshold;
                    else
                        return axis.Value != Threshold && axis.PreviousValue == Threshold;
                }
            }
        }

        public class VirtualJoystickXTrigger : Node
        {
            public enum Modes { LargerThan, LessThan, Equals };

            public VirtualInput.ThresholdModes Mode;
            public float Threshold;

            private VirtualJoystick joystick;

            public VirtualJoystickXTrigger(VirtualJoystick joystick, VirtualInput.ThresholdModes mode, float threshold)
            {
                this.joystick = joystick;
                Mode = mode;
                Threshold = threshold;
            }

            public override bool Check
            {
                get
                {
                    if (Mode == VirtualInput.ThresholdModes.LargerThan)
                        return joystick.Value.X >= Threshold;
                    else if (Mode == VirtualInput.ThresholdModes.LessThan)
                        return joystick.Value.X <= Threshold;
                    else
                        return joystick.Value.X == Threshold;
                }
            }

            public override bool Pressed
            {
                get
                {
                    if (Mode == VirtualInput.ThresholdModes.LargerThan)
                        return joystick.Value.X >= Threshold && joystick.PreviousValue.X < Threshold;
                    else if (Mode == VirtualInput.ThresholdModes.LessThan)
                        return joystick.Value.X <= Threshold && joystick.PreviousValue.X > Threshold;
                    else
                        return joystick.Value.X == Threshold && joystick.PreviousValue.X != Threshold;
                }
            }

            public override bool Released
            {
                get
                {
                    if (Mode == VirtualInput.ThresholdModes.LargerThan)
                        return joystick.Value.X < Threshold && joystick.PreviousValue.X >= Threshold;
                    else if (Mode == VirtualInput.ThresholdModes.LessThan)
                        return joystick.Value.X > Threshold && joystick.PreviousValue.X <= Threshold;
                    else
                        return joystick.Value.X != Threshold && joystick.PreviousValue.X == Threshold;
                }
            }
        }

        public class VirtualJoystickYTrigger : Node
        {
            public VirtualInput.ThresholdModes Mode;
            public float Threshold;

            private VirtualJoystick joystick;

            public VirtualJoystickYTrigger(VirtualJoystick joystick, VirtualInput.ThresholdModes mode, float threshold)
            {
                this.joystick = joystick;
                Mode = mode;
                Threshold = threshold;
            }

            public override bool Check
            {
                get
                {
                    if (Mode == VirtualInput.ThresholdModes.LargerThan)
                        return joystick.Value.X >= Threshold;
                    else if (Mode == VirtualInput.ThresholdModes.LessThan)
                        return joystick.Value.X <= Threshold;
                    else
                        return joystick.Value.X == Threshold;
                }
            }

            public override bool Pressed
            {
                get
                {
                    if (Mode == VirtualInput.ThresholdModes.LargerThan)
                        return joystick.Value.X >= Threshold && joystick.PreviousValue.X < Threshold;
                    else if (Mode == VirtualInput.ThresholdModes.LessThan)
                        return joystick.Value.X <= Threshold && joystick.PreviousValue.X > Threshold;
                    else
                        return joystick.Value.X == Threshold && joystick.PreviousValue.X != Threshold;
                }
            }

            public override bool Released
            {
                get
                {
                    if (Mode == VirtualInput.ThresholdModes.LargerThan)
                        return joystick.Value.X < Threshold && joystick.PreviousValue.X >= Threshold;
                    else if (Mode == VirtualInput.ThresholdModes.LessThan)
                        return joystick.Value.X > Threshold && joystick.PreviousValue.X <= Threshold;
                    else
                        return joystick.Value.X != Threshold && joystick.PreviousValue.X == Threshold;
                }
            }
        }

        #endregion
    }

}