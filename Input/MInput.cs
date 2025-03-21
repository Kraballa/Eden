using System.Numerics;
using Raylib_cs;

namespace Eden
{
    public static class MInput
    {
        public static KeyboardData Keyboard { get; private set; }
        public static MouseData Mouse { get; private set; }
        public static GamePadData[] GamePads { get; private set; }

        internal static List<VirtualInput> VirtualInputs;

        public static bool Active = true;
        public static bool Disabled = false;

        internal static void Initialize()
        {
            Keyboard = new KeyboardData();
            Mouse = new MouseData();
            GamePads = new GamePadData[4];
            for (int i = 0; i < 4; i++)
                GamePads[i] = new GamePadData(i);
            VirtualInputs = new List<VirtualInput>();
        }

        internal static void Shutdown()
        {
            foreach (var gamepad in GamePads)
                gamepad.StopRumble();
        }

        internal static void Update()
        {
            UpdateVirtualInputs();
        }

        public static void UpdateNull()
        {
            UpdateVirtualInputs();
        }

        private static void UpdateVirtualInputs()
        {
            foreach (var virtualInput in VirtualInputs)
                virtualInput.Update();
        }

        #region Keyboard

        public class KeyboardData
        {
            #region Basic Checks

            public bool Check(KeyboardKey key)
            {
                if (Disabled)
                    return false;

                return Raylib.IsKeyDown(key);
            }

            public bool Pressed(KeyboardKey key)
            {
                if (Disabled)
                    return false;

                return Raylib.IsKeyPressed(key);
            }

            public bool Released(KeyboardKey key)
            {
                if (Disabled)
                    return false;

                return Raylib.IsKeyReleased(key);
            }

            #endregion

            #region Convenience Checks

            public bool Check(KeyboardKey keyA, KeyboardKey keyB)
            {
                return Check(keyA) || Check(keyB);
            }

            public bool Pressed(KeyboardKey keyA, KeyboardKey keyB)
            {
                return Pressed(keyA) || Pressed(keyB);
            }

            public bool Released(KeyboardKey keyA, KeyboardKey keyB)
            {
                return Released(keyA) || Released(keyB);
            }

            public bool Check(KeyboardKey keyA, KeyboardKey keyB, KeyboardKey keyC)
            {
                return Check(keyA) || Check(keyB) || Check(keyC);
            }

            public bool Pressed(KeyboardKey keyA, KeyboardKey keyB, KeyboardKey keyC)
            {
                return Pressed(keyA) || Pressed(keyB) || Pressed(keyC);
            }

            public bool Released(KeyboardKey keyA, KeyboardKey keyB, KeyboardKey keyC)
            {
                return Released(keyA) || Released(keyB) || Released(keyC);
            }

            #endregion

            #region Axis

            public int AxisCheck(KeyboardKey negative, KeyboardKey positive)
            {
                if (Check(negative))
                {
                    if (Check(positive))
                        return 0;
                    else
                        return -1;
                }
                else if (Check(positive))
                    return 1;
                else
                    return 0;
            }

            public int AxisCheck(KeyboardKey negative, KeyboardKey positive, int both)
            {
                if (Check(negative))
                {
                    if (Check(positive))
                        return both;
                    else
                        return -1;
                }
                else if (Check(positive))
                    return 1;
                else
                    return 0;
            }

            #endregion
        }

        #endregion

        #region Mouse

        public class MouseData
        {
            internal MouseData()
            {
            }

            #region Buttons

            public bool CheckLeftButton => Raylib.IsMouseButtonDown(MouseButton.Left);

            public bool CheckRightButton => Raylib.IsMouseButtonDown(MouseButton.Right);

            public bool CheckMiddleButton => Raylib.IsMouseButtonDown(MouseButton.Middle);

            public bool PressedLeftButton => Raylib.IsMouseButtonPressed(MouseButton.Left);

            public bool PressedRightButton => Raylib.IsMouseButtonPressed(MouseButton.Right);

            public bool PressedMiddleButton => Raylib.IsMouseButtonPressed(MouseButton.Middle);

            public bool ReleasedLeftButton => Raylib.IsMouseButtonReleased(MouseButton.Left);

            public bool ReleasedRightButton => Raylib.IsMouseButtonReleased(MouseButton.Right);

            public bool ReleasedMiddleButton => Raylib.IsMouseButtonReleased(MouseButton.Middle);

            #endregion

            #region Wheel

            //todo: check fix
            public float Wheel
            {
                get { return Raylib.GetMouseWheelMove(); }
            }

            public int WheelDelta
            {
                get { return Math.Sign(Raylib.GetMouseWheelMove()); }
            }

            #endregion

            #region Position

            public bool WasMoved
            {
                get
                {
                    return Raylib.GetMouseDelta().Length() > 0.1f;
                }
            }

            public float X
            {
                get { return Position.X;}// - Engine.ViewPadding; }
                set { Position = new Vector2(value, Position.Y); }
            }

            public float Y
            {
                get { return Position.Y; }
                set { Position = new Vector2(Position.X, value); }
            }

            //todo: check transform
            public Vector2 Position
            {
                get
                {
                    return Raylib.GetMousePosition();
                    //return Vector2.Transform(new Vector2(CurrentState.X, CurrentState.Y), Matrix.Invert(Engine.ScreenMatrix));
                }

                set
                {
                    Raylib.SetMousePosition((int)MathF.Round(value.X), (int)MathF.Round(value.Y));
                    // var vector = Vector2.Transform(value, Engine.ScreenMatrix);
                    // Microsoft.Xna.Framework.Input.Mouse.SetPosition((int)Math.Round(vector.X), (int)Math.Round(vector.Y));
                }
            }

            #endregion
        }

        #endregion

        #region GamePads

        public class GamePadData
        {
            public int PlayerIndex { get; private set; }
            public bool Attached;

            private float rumbleStrength;
            private float rumbleTime;

            internal GamePadData(int playerIndex)
            {
                PlayerIndex = playerIndex;
            }

            //todo: check rumble updating
            // public void Update()
            // {
            //     // if (rumbleTime > 0)
            //     // {
            //     //     rumbleTime -= Raylib.GetFrameTime();
            //     //     if (rumbleTime <= 0)
            //     //         Raylib.SetGamepadVibration(PlayerIndex, 0, 0, 0);
            //     // }
            // }

            public void Rumble(float strength, float time)
            {
                Raylib.SetGamepadVibration(PlayerIndex, strength, strength, time);
            }

            public void StopRumble()
            {
                Raylib.SetGamepadVibration(PlayerIndex, 0, 0, 0);
                rumbleTime = 0;
            }

            #region Buttons

            public bool Check(GamepadButton button)
            {
                if (Disabled)
                    return false;

                return Raylib.IsGamepadButtonDown(PlayerIndex, button);
            }

            public bool Pressed(GamepadButton button)
            {
                if (Disabled)
                    return false;

                return Raylib.IsGamepadButtonPressed(PlayerIndex, button);
            }

            public bool Released(GamepadButton button)
            {
                if (Disabled)
                    return false;

                return Raylib.IsGamepadButtonReleased(PlayerIndex, button);
            }

            public bool Check(GamepadButton buttonA, GamepadButton buttonB)
            {
                return Check(buttonA) || Check(buttonB);
            }

            public bool Pressed(GamepadButton buttonA, GamepadButton buttonB)
            {
                return Pressed(buttonA) || Pressed(buttonB);
            }

            public bool Released(GamepadButton buttonA, GamepadButton buttonB)
            {
                return Released(buttonA) || Released(buttonB);
            }

            public bool Check(GamepadButton buttonA, GamepadButton buttonB, GamepadButton buttonC)
            {
                return Check(buttonA) || Check(buttonB) || Check(buttonC);
            }

            public bool Pressed(GamepadButton buttonA, GamepadButton buttonB, GamepadButton buttonC)
            {
                return Pressed(buttonA) || Pressed(buttonB) || Check(buttonC);
            }

            public bool Released(GamepadButton buttonA, GamepadButton buttonB, GamepadButton buttonC)
            {
                return Released(buttonA) || Released(buttonB) || Check(buttonC);
            }

            #endregion

            #region Sticks

            public Vector2 GetLeftStick()
            {
                Vector2 ret = new()
                {
                    X = Raylib.GetGamepadAxisMovement(PlayerIndex, GamepadAxis.LeftX),
                    Y = -Raylib.GetGamepadAxisMovement(PlayerIndex, GamepadAxis.LeftY)
                };
                return ret;
            }

            public Vector2 GetLeftStick(float deadzone)
            {
                Vector2 ret = GetLeftStick();
                if (ret.LengthSquared() < deadzone * deadzone)
                    ret = Vector2.Zero;
                else
                    ret.Y = -ret.Y;
                return ret;
            }

            public Vector2 GetRightStick()
            {
                Vector2 ret = new()
                {
                    X = Raylib.GetGamepadAxisMovement(PlayerIndex, GamepadAxis.RightX),
                    Y = -Raylib.GetGamepadAxisMovement(PlayerIndex, GamepadAxis.RightY)
                };
                return ret;
            }

            public Vector2 GetRightStick(float deadzone)
            {
                Vector2 ret = GetRightStick();
                if (ret.LengthSquared() < deadzone * deadzone)
                    ret = Vector2.Zero;
                else
                    ret.Y = -ret.Y;
                return ret;
            }

            #region Left Stick Directions

            public bool LeftStickLeftCheck(float deadzone)
            {
                return Raylib.GetGamepadAxisMovement(PlayerIndex, GamepadAxis.LeftX) <= -deadzone;
            }

            public bool LeftStickRightCheck(float deadzone)
            {
                return Raylib.GetGamepadAxisMovement(PlayerIndex, GamepadAxis.LeftX) >= deadzone;
            }

            public bool LeftStickDownCheck(float deadzone)
            {
                return Raylib.GetGamepadAxisMovement(PlayerIndex, GamepadAxis.LeftY) <= -deadzone;
            }

            public bool LeftStickUpCheck(float deadzone)
            {
                return Raylib.GetGamepadAxisMovement(PlayerIndex, GamepadAxis.LeftY) >= deadzone;
            }

            public float LeftStickHorizontal(float deadzone)
            {
                float h = Raylib.GetGamepadAxisMovement(PlayerIndex, GamepadAxis.LeftX);
                if (Math.Abs(h) < deadzone)
                    return 0;
                else
                    return h;
            }

            public float LeftStickVertical(float deadzone)
            {
                float v = Raylib.GetGamepadAxisMovement(PlayerIndex, GamepadAxis.LeftY);
                if (Math.Abs(v) < deadzone)
                    return 0;
                else
                    return -v;
            }

            #endregion

            #region Right Stick Directions

            public bool RightStickLeftCheck(float deadzone)
            {
                return Raylib.GetGamepadAxisMovement(PlayerIndex, GamepadAxis.RightX) <= -deadzone;
            }

            public bool RightStickRightCheck(float deadzone)
            {
                return Raylib.GetGamepadAxisMovement(PlayerIndex, GamepadAxis.RightX) >= deadzone;
            }

            public bool RightStickUpCheck(float deadzone)
            {
                return Raylib.GetGamepadAxisMovement(PlayerIndex, GamepadAxis.RightY) <= -deadzone;
            }

            public bool RightStickDownCheck(float deadzone)
            {
                return Raylib.GetGamepadAxisMovement(PlayerIndex, GamepadAxis.RightY) >= deadzone;
            }

            public float RightStickHorizontal(float deadzone)
            {
                float h = Raylib.GetGamepadAxisMovement(PlayerIndex, GamepadAxis.RightX);
                if (Math.Abs(h) < deadzone)
                    return 0;
                else
                    return h;
            }

            public float RightStickVertical(float deadzone)
            {
                float v = Raylib.GetGamepadAxisMovement(PlayerIndex, GamepadAxis.RightY);
                if (Math.Abs(v) < deadzone)
                    return 0;
                else
                    return -v;
            }

            #endregion

            #endregion

            #region DPad

            public int DPadHorizontal
            {
                get
                {
                    return Raylib.IsGamepadButtonDown(PlayerIndex, GamepadButton.LeftFaceRight) ? 1 : (Raylib.IsGamepadButtonDown(PlayerIndex, GamepadButton.LeftFaceLeft) ? -1 : 0);
                }
            }

            public int DPadVertical
            {
                get
                {
                    return Raylib.IsGamepadButtonDown(PlayerIndex, GamepadButton.LeftFaceDown) ? 1 : (Raylib.IsGamepadButtonDown(PlayerIndex, GamepadButton.LeftFaceUp) ? -1 : 0);
                }
            }

            public Vector2 DPad
            {
                get
                {
                    return new Vector2(DPadHorizontal, DPadVertical);
                }
            }

            public bool DPadLeftCheck
            {
                get
                {
                    return Raylib.IsGamepadButtonDown(PlayerIndex, GamepadButton.LeftFaceLeft);
                }
            }

            public bool DPadLeftPressed
            {
                get
                {
                    return Raylib.IsGamepadButtonPressed(PlayerIndex, GamepadButton.LeftFaceLeft);
                }
            }

            public bool DPadLeftReleased
            {
                get
                {
                    return Raylib.IsGamepadButtonReleased(PlayerIndex, GamepadButton.LeftFaceLeft);
                }
            }

            public bool DPadRightCheck
            {
                get
                {
                    return Raylib.IsGamepadButtonDown(PlayerIndex, GamepadButton.LeftFaceRight);
                }
            }

            public bool DPadRightPressed
            {
                get
                {
                    return Raylib.IsGamepadButtonPressed(PlayerIndex, GamepadButton.LeftFaceRight);
                }
            }

            public bool DPadRightReleased
            {
                get
                {
                    return Raylib.IsGamepadButtonReleased(PlayerIndex, GamepadButton.LeftFaceRight);
                }
            }

            public bool DPadUpCheck
            {
                get
                {
                    return Raylib.IsGamepadButtonDown(PlayerIndex, GamepadButton.LeftFaceUp);
                }
            }

            public bool DPadUpPressed
            {
                get
                {
                    return Raylib.IsGamepadButtonPressed(PlayerIndex, GamepadButton.LeftFaceUp);
                }
            }

            public bool DPadUpReleased
            {
                get
                {
                    return Raylib.IsGamepadButtonReleased(PlayerIndex, GamepadButton.LeftFaceUp);
                }
            }

            public bool DPadDownCheck
            {
                get
                {
                    return Raylib.IsGamepadButtonDown(PlayerIndex, GamepadButton.LeftFaceDown);
                }
            }

            public bool DPadDownPressed
            {
                get
                {
                    return Raylib.IsGamepadButtonPressed(PlayerIndex, GamepadButton.LeftFaceDown);
                }
            }

            public bool DPadDownReleased
            {
                get
                {
                    return Raylib.IsGamepadButtonReleased(PlayerIndex, GamepadButton.LeftFaceDown);
                }
            }

            #endregion

            #region Triggers

            public bool LeftTriggerCheck(float threshold)
            {
                if (Disabled)
                    return false;

                return Raylib.GetGamepadAxisMovement(PlayerIndex, GamepadAxis.LeftTrigger) >= threshold;
            }

            public bool RightTriggerCheck(float threshold)
            {
                if (Disabled)
                    return false;

                return Raylib.GetGamepadAxisMovement(PlayerIndex, GamepadAxis.RightTrigger) >= threshold;
            }

            #endregion
        }

        #endregion

        #region Helpers

        public static void RumbleFirst(float strength, float time)
        {
            GamePads[0].Rumble(strength, time);
        }

        public static int Axis(bool negative, bool positive, int bothValue)
        {
            if (negative)
            {
                if (positive)
                    return bothValue;
                else
                    return -1;
            }
            else if (positive)
                return 1;
            else
                return 0;
        }

        public static int Axis(float axisValue, float deadzone)
        {
            if (Math.Abs(axisValue) >= deadzone)
                return Math.Sign(axisValue);
            else
                return 0;
        }

        public static int Axis(bool negative, bool positive, int bothValue, float axisValue, float deadzone)
        {
            int ret = Axis(axisValue, deadzone);
            if (ret == 0)
                ret = Axis(negative, positive, bothValue);
            return ret;
        }

        #endregion
    }
}