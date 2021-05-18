using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Select.Services.Input
{
    public class GlobalEventProvider : Component
    {
        protected override bool CanRaiseEvents => true;
        
        #region Mouse events

        private event MouseEventHandler MMouseMove;
        public event MouseEventHandler MouseMove
        {
            add
            {
                if (MMouseMove == null)
                {
                    HookManager.MouseMove += HookManager_MouseMove;
                }
                MMouseMove += value;
            }
            remove
            {
                MMouseMove -= value;
                if (MMouseMove == null)
                {
                    HookManager.MouseMove -= HookManager_MouseMove;
                }
            }
        }

        void HookManager_MouseMove(object sender, MouseEventArgs e)
        {
            MMouseMove?.Invoke(this, e);
        }

        private event MouseEventHandler MMouseClick;
        public event MouseEventHandler MouseClick
        {
            add
            {
                if (MMouseClick == null)
                {
                    HookManager.MouseClick += HookManager_MouseClick;
                }
                MMouseClick += value;
            }
            remove
            {
                MMouseClick -= value;
                if (MMouseClick == null)
                {
                    HookManager.MouseClick -= HookManager_MouseClick;
                }
            }
        }

        void HookManager_MouseClick(object sender, MouseEventArgs e)
        {
            MMouseClick?.Invoke(this, e);
        }

        private event MouseEventHandler MMouseDown;
        
        public event MouseEventHandler MouseDown
        {
            add
            {
                if (MMouseDown == null)
                {
                    HookManager.MouseDown += HookManager_MouseDown;
                }
                MMouseDown += value;
            }
            remove
            {
                MMouseDown -= value;
                if (MMouseDown == null)
                {
                    HookManager.MouseDown -= HookManager_MouseDown;
                }
            }
        }

        void HookManager_MouseDown(object sender, MouseEventArgs e)
        {
            MMouseDown?.Invoke(this, e);
        }


        private event MouseEventHandler MMouseUp;
        public event MouseEventHandler MouseUp
        {
            add
            {
                if (MMouseUp == null)
                {
                    HookManager.MouseUp += HookManager_MouseUp;
                }
                MMouseUp += value;
            }

            remove
            {
                MMouseUp -= value;
                if (MMouseUp == null)
                {
                    HookManager.MouseUp -= HookManager_MouseUp;
                }
            }
        }

        void HookManager_MouseUp(object sender, MouseEventArgs e)
        {
            MMouseUp?.Invoke(this, e);
        }

        private event MouseEventHandler MMouseDoubleClick;
        public event MouseEventHandler MouseDoubleClick
        {
            add
            {
                if (MMouseDoubleClick == null)
                {
                    HookManager.MouseDoubleClick += HookManager_MouseDoubleClick;
                }
                MMouseDoubleClick += value;
            }

            remove
            {
                MMouseDoubleClick -= value;
                if (MMouseDoubleClick == null)
                {
                    HookManager.MouseDoubleClick -= HookManager_MouseDoubleClick;
                }
            }
        }

        void HookManager_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MMouseDoubleClick?.Invoke(this, e);
        }


        private event EventHandler<MouseEventExtArgs> MMouseMoveExt;
        public event EventHandler<MouseEventExtArgs> MouseMoveExt
        {
            add
            {
                if (MMouseMoveExt == null)
                {
                    HookManager.MouseMoveExt += HookManager_MouseMoveExt;
                }
                MMouseMoveExt += value;
            }

            remove
            {
                MMouseMoveExt -= value;
                if (MMouseMoveExt == null)
                {
                    HookManager.MouseMoveExt -= HookManager_MouseMoveExt;
                }
            }
        }

        void HookManager_MouseMoveExt(object sender, MouseEventExtArgs e)
        {
            if (MMouseMoveExt != null)
            {
                MMouseMoveExt.Invoke(this, e);
            }
        }

        private event EventHandler<MouseEventExtArgs> MMouseClickExt;
        public event EventHandler<MouseEventExtArgs> MouseClickExt
        {
            add
            {
                if (MMouseClickExt == null)
                {
                    HookManager.MouseClickExt += HookManager_MouseClickExt;
                }
                MMouseClickExt += value;
            }

            remove
            {
                MMouseClickExt -= value;
                if (MMouseClickExt == null)
                {
                    HookManager.MouseClickExt -= HookManager_MouseClickExt;
                }
            }
        }

        void HookManager_MouseClickExt(object sender, MouseEventExtArgs e)
        {
            if (MMouseClickExt != null)
            {
                MMouseClickExt.Invoke(this, e);
            }
        }
        #endregion
    }
}
