using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using Select.Service;

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
            _clipboardService.TextReceived += ClipboardServiceOnTextReceived;
            await _clipboardService.StartAsync();
        }

        private void ClipboardServiceOnTextReceived(object sender, string text)
        {
            Debug.WriteLine($"Text received {text}");
        }
    }
}