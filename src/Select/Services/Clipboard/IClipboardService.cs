using System;
using System.Threading.Tasks;

namespace Select.Services.Clipboard
{
    public interface IClipboardService
    {
        Task StartAsync();

        event EventHandler<string> TextReceived;
        
        bool IsRunning { get; }

        void SendPaste();

        void Stop();
    }
}