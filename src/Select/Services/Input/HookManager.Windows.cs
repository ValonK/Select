using System;
using System.Runtime.InteropServices;

namespace Select.Services.Input
{
    public static partial class HookManager
    {
        #region Windows constants

        private const int WhMouseLl = 14;
        private const int WhKeyboardLl = 13;
        private const int WhMouse = 7;
        private const int WhKeyboard = 2;
        private const int WmMousemove = 0x200;
        private const int WmLbuttondown = 0x201;
        private const int WmRbuttondown = 0x204;
        private const int WmMbuttondown = 0x207;
        private const int WmLbuttonup = 0x202;
        private const int WmRbuttonup = 0x205;
        private const int WmMbuttonup = 0x208;
        private const int WmLbuttondblclk = 0x203;
        private const int WmRbuttondblclk = 0x206;
        private const int WmMbuttondblclk = 0x209;
        private const int WmMousewheel = 0x020A;
        private const int WmKeydown = 0x100;
        private const int WmKeyup = 0x101;
        private const int WmSyskeydown = 0x104;
        private const int WmSyskeyup = 0x105;
        private const byte VkShift = 0x10;
        private const byte VkCapital = 0x14;
        private const byte VkNumlock = 0x90;

        #endregion

        #region Win32

        [DllImport("user32.dll", CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int UnhookWindowsHookEx(int idHook);

        [DllImport("user32")]
        public static extern int GetDoubleClickTime();

        [DllImport("user32")]
        private static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);

        [DllImport("user32")]
        private static extern int GetKeyboardState(byte[] pbKeyState);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern short GetKeyState(int vKey);

        #endregion
    }
}