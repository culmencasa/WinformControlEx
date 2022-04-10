using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoNet46
{
    public partial class MainForm : NonFrameForm
    {
        public MainForm()
        {
            InitializeComponent();

            this.CaptionShadowWidth = 5;
            this.Resizable = true;
            this.ShowCaptionShadow = true;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            pnlStart.Location = new Point(pnlStart.Left, pnlTaskbar.Top);
            pnlStart.LostFocus += PnlStart_LostFocus;

            popupTimer = new Timer();
            popupTimer.Interval = 1;
            popupTimer.Tick += new EventHandler(PopupTimer_Tick);

            popoffTimer = new Timer();
            popoffTimer.Interval = 1;
            popoffTimer.Tick += PopoffTimer_Tick;

        }


        #region 开始菜单弹出

        private Timer popupTimer;
        private Timer popoffTimer;
        public enum StartMenuStates
        {
            Closed = 0,
            Opening = 1,
            Opened = 2,
            Closing = 3
        }


        private StartMenuStates startMenuState;

        private void PopoffTimer_Tick(object sender, EventArgs e)
        {
            if (startMenuState == StartMenuStates.Closed || startMenuState == StartMenuStates.Opening)
            {
                popoffTimer.Stop();
                pnlStart.GotFocus += PnlStart_GotFocus;
                pnlStart.LostFocus += PnlStart_LostFocus;
                return;
            }


            startMenuState = StartMenuStates.Closing;
            if (pnlStart.Visible == true && pnlStart.Location.Y < pnlTaskbar.Top)
            {
                pnlStart.SendToBack();
            }
            if (pnlStart.Location.Y > pnlTaskbar.Top)
            {
                popoffTimer.Stop();
                pnlStart.Visible = false;
                pnlStart.Location = new Point(pnlStart.Left, pnlTaskbar.Top);
                startMenuState = StartMenuStates.Closed;
                pnlStart.LostFocus += PnlStart_LostFocus;
            }
            else
            {
                pnlStart.Location = new Point(
                    pnlStart.Left,
                    pnlStart.Top + 30
                );
            }

        }

        private void PopupTimer_Tick(object sender, EventArgs e)
        {
            if (startMenuState == StartMenuStates.Opened)
            {
                popupTimer.Stop();
                pnlStart.LostFocus += PnlStart_LostFocus;
                return;
            }

            while (startMenuState == StartMenuStates.Closing)
            {
                return;
            }

            startMenuState = StartMenuStates.Opening;

            if (pnlStart.Visible == false && pnlStart.Location.Y < pnlTaskbar.Top)
            {
                pnlStart.Visible = true;
            }

            if (pnlStart.Location.Y <= pnlTaskbar.Top - pnlStart.Height)
            {
                pnlStart.BringToFront();
                popupTimer.Stop();
                pnlStart.Location = new Point(pnlStart.Left, pnlTaskbar.Top - pnlStart.Height);

                ActiveControl = pnlStart;

                startMenuState = StartMenuStates.Opened;
                pnlStart.BringToFront();
                pnlStart.LostFocus += PnlStart_LostFocus;
            }
            else
            {
                pnlStart.BringToFront();
                pnlStart.Location = new Point(
                    pnlStart.Left,
                    pnlStart.Top - 30
                );
            }

        }

        private void PnlStart_GotFocus(object sender, EventArgs e)
        {
            ActiveControl = pnlStart;
        }

        private void PnlStart_LostFocus(object sender, EventArgs e)
        {
            /* 麻烦... 未实现. 实际上开始菜单应该是一个窗体, 而不是一个Panel控件.
            if (startMenuState != StartMenuStates.Opened)
            {
                return;
            }
            if (pnlStart.ClientRectangle.Contains(PointToClient(Control.MousePosition)))
            {
                pnlStart.Focus();
                return;
            }

            startMenuState = StartMenuStates.Closing;
            popoffTimer.Start();
            */
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (startMenuState == StartMenuStates.Closing || startMenuState == StartMenuStates.Closed)
            {
                pnlStart.LostFocus -= PnlStart_LostFocus;
                popupTimer.Start();
            }
            else
            {
                pnlStart.LostFocus -= PnlStart_LostFocus;
                popoffTimer.Start();
            }
        }


        #endregion

        #region 桌面记时器

        private void sysTimer_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");

            // 模拟任务栏总在最前
            pnlTaskbar.BringToFront();

            // 模拟开始菜单失焦
            if (!pnlStart.Focused && !IsMouseHoverStartMenu() && IsLastClickOutsideOfStartMenu())
            {
                startMenuState = StartMenuStates.Closing;
                popoffTimer.Start();
            }

        }

        /// <summary>
        /// 判断鼠标是否在开始菜单内部
        /// </summary>
        /// <returns></returns>
        bool IsMouseHoverStartMenu()
        {
            Point arrowPosition = pnlStart.PointToClient(Cursor.Position);
            bool isMouseHoverStartMenu = pnlStart.DisplayRectangle.Contains(arrowPosition);

            return isMouseHoverStartMenu;
        }

        /// <summary>
        /// 判断鼠标是否点击了开始菜单外部
        /// </summary>
        /// <returns></returns>
        bool IsLastClickOutsideOfStartMenu()
        {
            if (LastClickPoint == Point.Empty)
                return false;

            Point arrowPosition = pnlStart.PointToClient(LastClickPoint);

            return !pnlStart.DisplayRectangle.Contains(arrowPosition);
        }



        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            e.Control.MouseClick += Control_MouseClick;
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            e.Control.MouseClick -= Control_MouseClick;
        }

        private void Control_MouseClick(object sender, MouseEventArgs e)
        {
            LastClickPoint = Cursor.Position;
        }

        Point LastClickPoint { get; set; }


        #endregion

        #region 开始菜单按钮

        private void btnShutdown_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion


        #region 子窗体鼠标操作

        private bool mouseDown;
        private Point lastLocation;

        public void AddChildWindow(Form form)
        {
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;

            form.MouseDown -= ChildWindow_MouseDown;
            form.MouseDown += ChildWindow_MouseDown;
            form.MouseMove -= ChildWindow_MouseMove;
            form.MouseMove += ChildWindow_MouseMove;
            form.MouseUp -= ChildWindow_MouseUp;
            form.MouseUp += ChildWindow_MouseUp;

            this.Controls.Add(form);
            form.Location = new Point((this.Width - form.Width) / 2, (this.Height - form.Height) / 2);
            form.BringToFront();


            form.Show();
        }

        private void ChildWindow_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }
        private void ChildWindow_MouseMove(object sender, MouseEventArgs e)
        {
            Form form = sender as Form;
            if (mouseDown)
            {
                form.Location = new Point((form.Location.X - lastLocation.X) + e.X, (form.Location.Y - lastLocation.Y) + e.Y);
                //control.Update();
            }
        }

        private void ChildWindow_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        #endregion


        #region 图标事件

        private void btnShowTestForm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                LayoutDemo form = FormManager.Single<LayoutDemo>();

                AddChildWindow(form);
            }
        }

        private void btnShowThemeDemo_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var themeDemo = FormManager.Single<ThemeDemo>();

                AddChildWindow(themeDemo);

            }

        }

        private void btnShowComponentDemo_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var componentDemo = FormManager.Single<ComponentDemo>();

                AddChildWindow(componentDemo);
            }
        }

        // 回收站
        private void btnRecycle_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Form f = new Form();
                f.TopLevel = false;
                f.FormBorderStyle = FormBorderStyle.None;

                f.Location = new Point(300, 300);
                this.Controls.Add(f);

                f.Show();

            }
        }


        #endregion
    }


}
