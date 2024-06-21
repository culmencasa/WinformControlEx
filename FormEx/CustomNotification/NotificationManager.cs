using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
    /// <summary>
    /// 多个通知窗口按顺序显示. 仅支持右下角.
    /// </summary>
    public class NotificationManager
    {
        public EventHandler ClickCallback;
        public EventHandler CloseCallback;

        private ArrayList _positionList = new ArrayList();
        private CustomNotification _lastDialog;

        public CustomNotification LastDialog
        {
            get
            {
                return _lastDialog;
            }
        }

        public void Popup(string caption, string brief, string time)
        {
            CustomNotification AlertForm = new CustomNotification(caption, brief, time);
            AlertForm.SpreadDirection = CustomNotification.Spread.Horizontally;
            AlertForm.FadeInOut = true;
            AlertForm.UseTransparency = true;
            AlertForm.AppearingDelay = 150;
            AlertForm.DisappearingDelay = 150;
            AlertForm.SpecificOpacity = 1.00;
            AlertForm.StayDelay = 4000;

            AlertForm.MessageClick += this.ClickCallback;

            AlertForm.MessageDisappear += delegate (object sender, EventArgs e)
            {
                if (_positionList.Count > 0)
                {
                    CustomNotification form = sender as CustomNotification;
                    _positionList.Remove(form.InitialPos);
                }

                if (this.CloseCallback != null)
                {
                    CloseCallback(sender, e);
                }
            };

            // 本次弹窗初始位置
            Point newpos = AlertForm.InitialPos;
            if (_positionList.Count > 0)
            {
                // 如果已存在集合, 取最后一次位置
                Point lastpos = (Point)_positionList[_positionList.Count - 1];
                // 判断弹出的方向, 并计算新位置
                if (AlertForm.SpreadDirection == CustomNotification.Spread.Horizontally)
                {
                    // 判断是否在屏幕之内, 是则向上移动
                    if (lastpos.Y > Screen.PrimaryScreen.Bounds.Y && lastpos.Y > AlertForm.PopupSize.Height)
                    {
                        newpos = new Point(lastpos.X, lastpos.Y - AlertForm.PopupSize.Height - 5);
                    }
                }
                else
                {
                    // 判断是否在屏幕之内,是则向左移动
                    if (lastpos.X > Screen.PrimaryScreen.Bounds.X && lastpos.X > AlertForm.PopupSize.Width)
                    {
                        newpos = new Point(lastpos.X - AlertForm.PopupSize.Width - 5, lastpos.Y);
                    }
                }
                // 改变当前弹窗位置为新位置
                AlertForm.InitialPos = newpos;
            }
            // 记住本次弹窗位置
            _positionList.Add(newpos);
            _lastDialog = AlertForm;
            AlertForm.ShowPopupInfo();
        }


    }
}
