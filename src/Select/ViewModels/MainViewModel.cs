using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Select.Extensions;
using Select.Helpers;
using Select.Services.Clipboard;
using Select.Services.Input;
using Select.Views;
using Application = System.Windows.Application;
using Clipboard = System.Windows.Clipboard;
using TextDataFormat = System.Windows.TextDataFormat;

namespace Select.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Fields

        private readonly IClipboardService _clipboardService;

        #endregion

        public MainViewModel()
        {
            _clipboardService = new ClipboardService();

            ExitCommand = new RelayCommand(OnExit);
            SettingsCommand = new RelayCommand(OnSettings);
        }
        
        #region Commands

        public RelayCommand ExitCommand { get; }
        public RelayCommand SettingsCommand { get; }

        #endregion

        public async void Initialize()
        {
            HookManager.MouseClick += HookManagerOnMouseClick;
            HookManager.MouseUp += HookManagerOnMouseUp;

            _clipboardService.TextReceived += ClipboardServiceOnTextReceived;
            await _clipboardService.StartAsync();
        }
        
        #region Mouse

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
                    _clipboardService.Stop();
                }
            }
        }
        

        #endregion

        #region Clipboards

        private async void ClipboardServiceOnTextReceived(object sender, string text)
        {
            if (!string.IsNullOrEmpty(text) && !text.HasLineBreaks())
            {
                Debug.WriteLine(text);

                await Task.Delay(200);

                try
                {
                    Clipboard.SetText(text, TextDataFormat.Text);
                }
                catch (Exception ex)
                {
                }
            }
        }

        #endregion

        #region Settings

        private async void OnSettings(object obj)
        {
            var settingsView = new SettingsView();
            settingsView.ShowDialog();
        }

        #endregion
        
        #region Exit

        private void OnExit(object obj)
        {
            Application.Current.Shutdown();
        }

        #endregion
    }
}