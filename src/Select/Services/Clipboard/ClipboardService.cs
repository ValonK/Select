using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;
using Application = System.Windows.Application;

namespace Select.Services.Clipboard
{
	public class ClipboardService : IClipboardService
	{
		#region Fields

		private CancellationTokenSource _cancellationTokenSource;
		private int _delay = 500;
		private InputSimulator _inputSimulator;

		private List<string> _applications => new List<string> { "WindowsTerminal", "CommandPrompt", "Code" };
		#endregion

		public ClipboardService()
		{
			_inputSimulator = new InputSimulator();
		}

		#region Events

		public event EventHandler<string> TextReceived;

		#endregion

		#region Properties

		public bool IsRunning { get; set; }

		#endregion

		#region Start

		public async Task StartAsync()
		{
			_cancellationTokenSource = new CancellationTokenSource();
			await Task.Run(StartInternal, _cancellationTokenSource.Token);
		}

		private async Task StartInternal()
		{
			if (IsRunning)
			{
				return;
			}

			IsRunning = true;


			try
			{
				var clipText = string.Empty;
				while (!_cancellationTokenSource.IsCancellationRequested)
				{
					var activeWindowHwnd = Win32Api.GetWindowUnderCursor();
					if (activeWindowHwnd != IntPtr.Zero)
					{
						continue;
					}

					if (_applications.Any(x => x == GetForegroundProcessName()))
					{
						continue;
					}

					SendCtrlC(activeWindowHwnd);

					await Task.Delay(_delay);

					Application.Current.Dispatcher.Invoke(() =>
					{
						if (System.Windows.Clipboard.ContainsText())
						{
							var clipboardText = System.Windows.Clipboard.GetText();
							if (!string.IsNullOrEmpty(clipboardText) && clipboardText != clipText)
							{
								clipText = clipboardText;
								TextReceived?.Invoke(this, clipText);
							}
						}
					});
				}
			}
			catch (Exception ex)
			{
				Debug.Write(ex.ToString());
			}
		}

		#endregion

		#region Stop

		public void Stop()
		{
			IsRunning = false;
			_cancellationTokenSource.Cancel();
		}

		#endregion

		#region Helpers
		public void SendPaste()
		{
			if (System.Windows.Clipboard.ContainsText())
			{
				var clipboardText = System.Windows.Clipboard.GetText();
				_inputSimulator.Keyboard.TextEntry(clipboardText);
			}
		}


		private void SendCtrlC(IntPtr activeWindowHwnd)
		{
			uint keyEventfKeyup = 2;
			const byte vkControl = 0x11;
			Win32Api.SetForegroundWindow(activeWindowHwnd);
			Win32Api.keybd_event(vkControl, 0, 0, 0);
			Win32Api.keybd_event(0x43, 0, 0, 0);
			Win32Api.keybd_event(0x43, 0, keyEventfKeyup, 0);
			Win32Api.keybd_event(vkControl, 0, keyEventfKeyup, 0);
		}

		private string GetForegroundProcessName()
		{
			IntPtr hwnd = Win32Api.GetForegroundWindow();
			if (hwnd != null)
			{
				uint pid;
				Win32Api.GetWindowThreadProcessId(hwnd, out pid);

				foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
				{
					if (p.Id == pid)
						return p.ProcessName;
				}

				return "Unknown";
			}

			return "Unknown";
		}
		#endregion
	}
}