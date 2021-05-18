using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Select.Services.Input
{
    public static partial class HookManager
    {
        #region Fields
        
        private delegate int HookProc(int nCode, int wParam, IntPtr lParam);
        private static HookProc _sMouseDelegate;
        private static int _sMouseHookHandle;
        private static int _mOldX;
        private static int _mOldY;
        #endregion
        
        private static int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var mouseHookStruct =
                    (MouseLlHookStruct) Marshal.PtrToStructure(lParam, typeof(MouseLlHookStruct));

                var button = MouseButtons.None;
                short mouseDelta = 0;
                var clickCount = 0;
                var mouseDown = false;
                var mouseUp = false;

                switch (wParam)
                {
                    case WmLbuttondown:
                        mouseDown = true;
                        button = MouseButtons.Left;
                        clickCount = 1;
                        break;
                    case WmLbuttonup:
                        mouseUp = true;
                        button = MouseButtons.Left;
                        clickCount = 1;
                        break;
                    case WmLbuttondblclk: 
                        button = MouseButtons.Left;
                        clickCount = 2;
                        break;
                    case WmRbuttondown:
                        mouseDown = true;
                        button = MouseButtons.Right;
                        clickCount = 1;
                        break;
                    case WmRbuttonup:
                        mouseUp = true;
                        button = MouseButtons.Right;
                        clickCount = 1;
                        break;
                    case WmMbuttondown:
                        mouseUp = true;
                        button = MouseButtons.Middle;
                        clickCount = 1;
                        break;
                    case WmRbuttondblclk: 
                        button = MouseButtons.Right;
                        clickCount = 2;
                        break;
                    case WmMousewheel:
                        mouseDelta = (short)((mouseHookStruct.MouseData >> 16) & 0xffff);
                        break;
                }

                MouseEventExtArgs e = new MouseEventExtArgs(
                                                   button,
                                                   clickCount,
                                                   mouseHookStruct.Point.X,
                                                   mouseHookStruct.Point.Y,
                                                   mouseDelta);

                if (SMouseUp!=null && mouseUp)
                {
                    SMouseUp.Invoke(null, e);
                }

                if (SMouseDown != null && mouseDown)
                {
                    SMouseDown.Invoke(null, e);
                }

                if (SMouseClick != null && clickCount>0)
                {
                    SMouseClick.Invoke(null, e);
                }

                if (SMouseClickExt != null && clickCount > 0)
                {
                    SMouseClickExt.Invoke(null, e);
                }

                if (SMouseDoubleClick != null && clickCount == 2)
                {
                    SMouseDoubleClick.Invoke(null, e);
                }

                if (SMouseWheel!=null && mouseDelta!=0)
                {
                    SMouseWheel.Invoke(null, e);
                }

                if ((SMouseMove!=null || SMouseMoveExt!=null) && (_mOldX != mouseHookStruct.Point.X || _mOldY != mouseHookStruct.Point.Y))
                {
                    _mOldX = mouseHookStruct.Point.X;
                    _mOldY = mouseHookStruct.Point.Y;
                    SMouseMove?.Invoke(null, e);
                    SMouseMoveExt?.Invoke(null, e);
                }

                if (e.Handled)
                {
                    return -1;
                }
            }

            return CallNextHookEx(_sMouseHookHandle, nCode, wParam, lParam);
        }

        private static void EnsureSubscribedToGlobalMouseEvents()
        {
            if (_sMouseHookHandle == 0)
            {
                _sMouseDelegate = MouseHookProc;
                _sMouseHookHandle = SetWindowsHookEx(
                    WhMouseLl,
                    _sMouseDelegate,
                    Marshal.GetHINSTANCE(
                        Assembly.GetExecutingAssembly().GetModules()[0]),
                    0);
                if (_sMouseHookHandle == 0)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw new Win32Exception(errorCode);
                }
            }
        }

        private static void TryUnsubscribeFromGlobalMouseEvents()
        {
            if (SMouseClick == null &&
                SMouseDown == null &&
                SMouseMove == null &&
                SMouseUp == null &&
                SMouseClickExt == null &&
                SMouseMoveExt == null &&
                SMouseWheel == null)
            {
                ForceUnsunscribeFromGlobalMouseEvents();
            }
        }

        private static void ForceUnsunscribeFromGlobalMouseEvents()
        {
            if (_sMouseHookHandle != 0)
            {
                int result = UnhookWindowsHookEx(_sMouseHookHandle);
                _sMouseHookHandle = 0;
                _sMouseDelegate = null;
                if (result == 0)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw new Win32Exception(errorCode);
                }
            }
        }
    }
}
