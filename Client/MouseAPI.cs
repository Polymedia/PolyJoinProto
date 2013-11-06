using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polymedia.PolyJoin.Client
{
    class MouseAPI
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public static void Move(int x, int y)
        {
            SetCursorPos(x, y);
        }

        public static void LeftButtonDown(int x, int y)
        {
            SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
        }

        public static void LeftButtonUp(int x, int y)
        {
            SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
        }

        public static void LeftButtonClick(int x, int y)
        {
            SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
        }

        public static void RightButtonDown(int x, int y)
        {
            SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_RIGHTDOWN, x, y, 0, 0);
        }

        public static void RightButtonUp(int x, int y)
        {
            SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_RIGHTUP, x, y, 0, 0);
        }

        public static void RightButtonClick(int x, int y)
        {
            SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_RIGHTDOWN, x, y, 0, 0);
            mouse_event(MOUSEEVENTF_RIGHTUP, x, y, 0, 0);
        }
    }
}
