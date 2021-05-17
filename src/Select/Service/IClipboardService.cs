using System;
using System.Threading.Tasks;

namespace Select.Service
{
    public interface IClipboardService
    {
        Task StartAsync();

        event EventHandler<string> TextReceived;

        Task StopAsync();
    }
}