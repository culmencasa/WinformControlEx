using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Reflection;

namespace System.ComponentModel
{
    /// <summary>
    /// 防止一段时间内的重复点击
    /// </summary>
    public class ActionLock
    {
        int _runTimes = 0;
        int _tickCount = 0, _span = 0;
        int _spanInterval = 500;
        int _maxRunTimes = 2;
        int _releaseLockDuration = 5000;

        /// <summary>
        /// 要锁定的控件
        /// </summary>
        public Control LockObj { get; set; }

        /// <summary>
        /// 设置运行多少次后不再重复运行, 等待ReleaseLockDuration时长后再运行
        /// </summary>
        public int MaxRunTimes
        {
            get { return _maxRunTimes; }
            set { _maxRunTimes = value; }
        }

        /// <summary>
        /// 设置多长时间后释放锁定 
        /// </summary> 
        public int ReleaseLockDuration
        {
            get { return _releaseLockDuration; }
            set { _releaseLockDuration = value; }
        }


        Thread _lockThread = null;

        public ActionLock(Control target)
            : this(target, 5000, 3)
        {
        }

        public ActionLock(Control target, int blockPeriod, int maxRunTimes)
        {
            this.LockObj = target;
            this.ReleaseLockDuration = blockPeriod;
            this.MaxRunTimes = maxRunTimes;
        }

        public void StartLockThread(Action callback)
        {
            if (_lockThread != null && _lockThread.IsAlive)
            {
                Form parentForm = GetTopForm(LockObj);
                if (parentForm != null)
                {
                    parentForm.BeginInvoke((Action)delegate
                    {
                        ToolTip t = new ToolTip();
                        //t.IsBalloon = true;
                        t.ToolTipIcon = ToolTipIcon.Info;
                        t.ToolTipTitle = "提示";
                        t.UseAnimation = true;
                        t.UseFading = true;

                        Point p = parentForm.PointToClient(new Point(Form.MousePosition.X, Form.MousePosition.Y + 20));
                        t.Show("您的操作太快了，请休息一下。", parentForm, p.X, p.Y, 2000);
                    });
                }

                _runTimes = 0;
                return;
            }

            _lockThread = new Thread(() =>
            {
                if (_tickCount == 0)
                    _tickCount = Environment.TickCount;
                else
                {
                    _span = Environment.TickCount - _tickCount;
                    _tickCount = Environment.TickCount;
                    
                    if (_span <= _spanInterval)
                        Interlocked.Increment(ref _runTimes);
                    else
                        _runTimes = 0;
                }
                
                if (_runTimes >= _maxRunTimes)
                {
                    LockObj.IsAccessible = false;
                    Application.DoEvents();
                    Thread.Sleep(_releaseLockDuration);
                    _runTimes = 0;


                    return;
                }

                if (callback != null)
                {
                    LockObj.IsAccessible = true;
                    LockObj.Invoke(callback);
                }

            });
            _lockThread.Start();
        }


        private Form GetTopForm(Control ctrl)
        {
            Form parentForm = null;
            if (ctrl == null || ctrl.Parent == null)
            {
                return parentForm;
            }

            parentForm = ctrl.Parent as Form;
            if (parentForm != null)
            {
                return parentForm;
            }
            else
            {
                return GetTopForm(ctrl.Parent);
            }
        }
    }
}
