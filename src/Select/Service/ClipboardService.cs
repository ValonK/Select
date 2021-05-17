using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Select.Service
{
    public class ClipboardService : IClipboardService
    {
        #region Fields

        private CancellationTokenSource _cancellationTokenSource;
        private int _delay = 500;
        #endregion

        #region Win32

        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();


        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);


        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        
        private void SendCtrlC(IntPtr activeWindowHwnd)
        {
            uint keyEventfKeyup = 2;
            const byte vkControl = 0x11;
            SetForegroundWindow(activeWindowHwnd);
            keybd_event(vkControl, 0, 0, 0);
            keybd_event(0x43, 0, 0, 0);
            keybd_event(0x43, 0, keyEventfKeyup, 0);
            keybd_event(vkControl, 0, keyEventfKeyup, 0);
        }

        #endregion

        #region Events

        public event EventHandler<string> TextReceived;

        #endregion

        #region Start

        public async Task StartAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            await Task.Run(StartInternal, _cancellationTokenSource.Token);
        }

        private async Task StartInternal()
        {
            try
            {
                var clipText = string.Empty;
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    var activeWindowHwnd = GetActiveWindow();
                    SendCtrlC(activeWindowHwnd);
                    
                    await Task.Delay(_delay);
                    
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (Clipboard.ContainsText())
                        {
                            var clipboardText = Clipboard.GetText();
                            if (!string.IsNullOrEmpty(clipboardText) && clipboardText != clipText)
                            {
                                clipText = clipboardText;
                                TextReceived?.Invoke(this, clipText);
#if DEBUG
                                Debug.Write($"[Text]: {clipText}");
#endif
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                _cancellationTokenSource.Cancel();
            }
        }

        #endregion

        #region Stop

        public async Task StopAsync()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = null;
        }

        #endregion
    }
}