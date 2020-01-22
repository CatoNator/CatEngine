using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace CatEngine.Input
{
    class CInputManager
    {
        private static GamePadState gamepadState;
        private static KeyboardState keyboardState;

        private static Dictionary<Buttons, bool> bIsButtonTriggered = new Dictionary<Buttons, bool>();
        private static Dictionary<Keys, bool> bIsKeyTriggered = new Dictionary<Keys, bool>();

        private CInputManager()
        {
        }

        //singletoning the singleton
        public static CInputManager Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly CInputManager instance = new CInputManager();
        }

        public static void InitKeys()
        {
            bIsButtonTriggered.Add(CSettings.Instance.gPJump, false);

            bIsButtonTriggered.Add(CSettings.Instance.gGPause, false);

            bIsButtonTriggered.Add(CSettings.Instance.gCRotateCamLeft, false);
            bIsButtonTriggered.Add(CSettings.Instance.gCRotateCamRight, false);
            bIsButtonTriggered.Add(CSettings.Instance.gCRotateCamUp, false);
            bIsButtonTriggered.Add(CSettings.Instance.gCRotateCamDown, false);

            bIsKeyTriggered.Add(CSettings.Instance.kPTurnLeft, false);
            bIsKeyTriggered.Add(CSettings.Instance.kPTurnRight, false);
            bIsKeyTriggered.Add(CSettings.Instance.kPMoveForward, false);
            bIsKeyTriggered.Add(CSettings.Instance.kPMoveBackward, false);
            bIsKeyTriggered.Add(CSettings.Instance.kPJump, false);

            bIsKeyTriggered.Add(CSettings.Instance.kGPause, false);

            bIsKeyTriggered.Add(CSettings.Instance.kCRotateCamLeft, false);
            bIsKeyTriggered.Add(CSettings.Instance.kCRotateCamRight, false);
            bIsKeyTriggered.Add(CSettings.Instance.kCRotateCamUp, false);
            bIsKeyTriggered.Add(CSettings.Instance.kCRotateCamDown, false);
        }

        public static void Update()
        {
            gamepadState = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One);
            keyboardState = Keyboard.GetState();

            foreach (KeyValuePair<Buttons, bool> p in bIsButtonTriggered.ToList())
            {
                if (!gamepadState.IsButtonDown(p.Key))
                {
                    bIsButtonTriggered[p.Key] = false;
                }
            }

            foreach (KeyValuePair<Keys, bool> p in bIsKeyTriggered.ToList())
            {
                if (!keyboardState.IsKeyDown(p.Key))
                {
                    bIsKeyTriggered[p.Key] = false;
                }
            }
        }

        public static bool ButtonPressed(Buttons button)
        {
            bool state = false;

            if (!bIsButtonTriggered[button] && gamepadState.IsButtonDown(button))
            {
                state = true;
                bIsButtonTriggered[button] = true;
            }

            return state;
        }

        public static bool ButtonDown(Buttons button)
        {
            return gamepadState.IsButtonDown(button);
        }

        public static bool KeyPressed(Keys key)
        {
            bool state = false;

            if (!bIsKeyTriggered[key] && keyboardState.IsKeyDown(key))
            {
                state = true;
                bIsKeyTriggered[key] = true;
            }

            return state;
        }

        public static bool KeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }
    }
}
