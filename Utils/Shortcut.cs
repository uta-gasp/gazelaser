using System;
using System.Windows.Forms;

namespace GazeLaser.Utils
{
    public class Shortcut
    {
        public string Group { get; private set; }
        public Action Callback { get; private set; }
        public Keys Key { get; private set; }
        public Keys[] Modifiers { get; private set; }

        public Shortcut(string aGroup, Action aCallback, Keys aKey, params Keys[] aModifiers)
        {
            Group = aGroup;
            Callback = aCallback;
            Key = aKey;
            Modifiers = aModifiers;
        }

        public bool isPressed(int aVirtualKey)
        {
            if (Key != (Keys)aVirtualKey)
                return false;

            Keys pressedModifiers = Control.ModifierKeys;
            foreach (Keys modifier in Modifiers)
            {
                if (!pressedModifiers.HasFlag(modifier))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
