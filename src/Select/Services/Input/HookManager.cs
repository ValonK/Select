using System;
using System.Windows.Forms;

namespace Select.Services.Input {
    
    public static partial class HookManager
    {
        private static event MouseEventHandler SMouseMove;
        public static event MouseEventHandler MouseMove
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                SMouseMove += value;
            }

            remove
            {
                SMouseMove -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private static event EventHandler<MouseEventExtArgs> SMouseMoveExt;
        public static event EventHandler<MouseEventExtArgs> MouseMoveExt
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                SMouseMoveExt += value;
            }

            remove
            {

                SMouseMoveExt -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private static event MouseEventHandler SMouseClick;
        public static event MouseEventHandler MouseClick
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                SMouseClick += value;
            }
            remove
            {
                SMouseClick -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private static event EventHandler<MouseEventExtArgs> SMouseClickExt;
        public static event EventHandler<MouseEventExtArgs> MouseClickExt
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                SMouseClickExt += value;
            }
            remove
            {
                SMouseClickExt -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private static event MouseEventHandler SMouseDown;
        public static event MouseEventHandler  MouseDown
        {
            add 
            { 
                EnsureSubscribedToGlobalMouseEvents();
                SMouseDown += value;
            }
            remove
            {
                SMouseDown -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private static event MouseEventHandler SMouseUp;
        public static event MouseEventHandler MouseUp
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                SMouseUp += value;
            }
            remove
            {
                SMouseUp -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private static event MouseEventHandler SMouseWheel;
        public static event MouseEventHandler MouseWheel
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                SMouseWheel += value;
            }
            remove
            {
                SMouseWheel -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }


        private static event MouseEventHandler SMouseDoubleClick;
        public static event MouseEventHandler MouseDoubleClick
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                if (SMouseDoubleClick == null)
                {
                    _sDoubleClickTimer = new Timer
                    {
                        Interval = GetDoubleClickTime(),
                        Enabled = false
                    };
                    _sDoubleClickTimer.Tick += DoubleClickTimeElapsed;
                    MouseUp += OnMouseUp;
                }
                SMouseDoubleClick += value;
            }
            remove
            {
                if (SMouseDoubleClick != null)
                {
                    SMouseDoubleClick -= value;
                    if (SMouseDoubleClick == null)
                    {
                        MouseUp -= OnMouseUp;
                        _sDoubleClickTimer.Tick -= DoubleClickTimeElapsed;
                        _sDoubleClickTimer = null;
                    }
                }
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private static MouseButtons _sPrevClickedButton;
        private static Timer _sDoubleClickTimer;

        private static void DoubleClickTimeElapsed(object sender, EventArgs e)
        {
            _sDoubleClickTimer.Enabled = false;
            _sPrevClickedButton = MouseButtons.None;
        }

        private static void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Clicks < 1) { return;}
            if (e.Button.Equals(_sPrevClickedButton))
            {
                SMouseDoubleClick?.Invoke(null, e);
                _sDoubleClickTimer.Enabled = false;
                _sPrevClickedButton = MouseButtons.None;
            }
            else
            {
                _sDoubleClickTimer.Enabled = true;
                _sPrevClickedButton = e.Button;
            }
        }
    }
}
