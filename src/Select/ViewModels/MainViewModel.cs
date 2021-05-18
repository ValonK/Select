using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Select.Extensions;
using Select.Services.Clipboard;
using Select.Services.Input;
using Clipboard = System.Windows.Clipboard;

namespace Select.ViewModels
{
	public class MainViewModel
	{
		private readonly IClipboardService _clipboardService;

		public MainViewModel()
		{
			_clipboardService = new ClipboardService();
		}

		public async void Initialize()
		{
			HookManager.MouseDown += HookManagerOnMouseDown;
			HookManager.MouseClick += HookManagerOnMouseClick;
			HookManager.MouseUp += HookManagerOnMouseUp;

			_clipboardService.TextReceived += ClipboardServiceOnTextReceived;
			await _clipboardService.StartAsync();
		}

		private void HookManagerOnMouseClick(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Middle) != 0)
			{
				 _clipboardService.SendPaste();
			}
		}

		private async void HookManagerOnMouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (!_clipboardService.IsRunning)
				{
					Debug.WriteLine("START");
					await _clipboardService.StartAsync();
				}
			}
		}

		private async void HookManagerOnMouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (_clipboardService.IsRunning)
				{
					Debug.WriteLine("STOP");
					await _clipboardService.Stop();
				}
			}
		}

		private async void ClipboardServiceOnTextReceived(object sender, string text)
		{
			if (!string.IsNullOrEmpty(text) && !text.HasLineBreaks())
			{
#if DEBUG
				Debug.WriteLine($"Text received {text}");
#endif
				await Task.Delay(200);

				try
				{
					Clipboard.SetText(text, System.Windows.TextDataFormat.Text);
				}
				catch (Exception ex)
				{
				}
			}
		}
	}
}