using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Select
{
	public static class Win32Api
	{

		[DllImport("user32.dll")]
		public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
		public static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

		[DllImport("User32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);
		[DllImport("user32.dll")]
		public static extern IntPtr GetWindowModuleFileName(IntPtr hWnd, string text, uint count);


		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool GetGUIThreadInfo(uint hTreadID, ref GUITHREADINFO lpgui);


		public const int WM_PASTE = 0x0302;

		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll")]
		public static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);


		[DllImport("user32.dll")]
		public static extern IntPtr WindowFromPoint(Point lpPoint);

		[DllImport("user32.dll")]
		public static extern bool GetCursorPos(out Point lpPoint);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
		public static extern int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, string lParam);


		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int iLeft;
			public int iTop;
			public int iRight;
			public int iBottom;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct GUITHREADINFO
		{
			public int cbSize;
			public int flags;
			public IntPtr hwndActive;
			public IntPtr hwndFocus;
			public IntPtr hwndCapture;
			public IntPtr hwndMenuOwner;
			public IntPtr hwndMoveSize;
			public IntPtr hwndCaret;
			public RECT rectCaret;
		}


		public static IntPtr GetFocusedHandle()
		{
			var info = new GUITHREADINFO();
			info.cbSize = Marshal.SizeOf(info);
			if (!GetGUIThreadInfo(0, ref info))
				throw new Win32Exception();
			return info.hwndFocus;
		}

		public const int WM_RBUTTONDOWN = 0x204; 
		public const int WM_SETTEXT = 0x000c; 

		public static IntPtr GetWindowUnderCursor()
		{
			Point ptCursor = new Point();

			if (!(GetCursorPos(out ptCursor)))
				return IntPtr.Zero;

			return WindowFromPoint(ptCursor);
		}
	}
}
