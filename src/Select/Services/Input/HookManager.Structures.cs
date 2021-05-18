using System.Runtime.InteropServices;

namespace Select.Services.Input {

    public static partial class HookManager {
        
        [StructLayout(LayoutKind.Sequential)]
        private struct Point {
            public readonly int X;
            public readonly int Y;
        }
        
        [StructLayout(LayoutKind.Sequential)]
        private struct MouseLlHookStruct {
            public readonly Point Point;
            public readonly int MouseData;
            private readonly int Flags;
            private readonly int Time;
            private readonly int ExtraInfo;
        }
    }
}
