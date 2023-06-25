using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
            Load += MainForm_DpiPatch;
        }


        #region 开始菜单弹出

        private Thread popupThread;
        private Thread popoffThread;


        public enum StartMenuStates
        {
            Closed = 0,
            Opening = 1,
            Opened = 2,
            Closing = 3
        }


        private StartMenuStates startMenuState = StartMenuStates.Closed;


        private void PopUpThreadWork()
        {
            if (startMenuState == StartMenuStates.Closing || startMenuState == StartMenuStates.Opening)
            {
                return;
            }

            while (startMenuState != StartMenuStates.Opened)
            {
                startMenuState = StartMenuStates.Opening;

                this.Invoke((Action)delegate
                {
                    if (pnlStart.Visible == false && pnlStart.Location.Y <= pnlTaskbar.Top)
                    {
                        pnlStart.Visible = true;
                        pnlStart.BringToFront();
                    }
                    

                    if (pnlStart.Location.Y <= pnlTaskbar.Top - pnlStart.Height)
                    {
                        pnlStart.Location = new Point(pnlStart.Left, pnlTaskbar.Top - pnlStart.Height);

                        ActiveControl = pnlStart;
                        pnlStart.BringToFront();

                        startMenuState = StartMenuStates.Opened;
                    }
                    else
                    {
                        
                        pnlStart.Location = new Point(
                            pnlStart.Left,
                            pnlStart.Top - 60
                        );
                        ActiveControl = pnlStart;
                    }
                });

                Thread.Sleep(1);
            }

        }

        private void PopOffThreadWork()
        {
            if (startMenuState == StartMenuStates.Closing || startMenuState == StartMenuStates.Opening)
            {
                return;
            }

            while (startMenuState != StartMenuStates.Closed)
            {
                startMenuState = StartMenuStates.Closing;

                this.Invoke((Action)delegate
                {
                    if (pnlStart.Visible == true && pnlStart.Location.Y > pnlTaskbar.Top)
                    {
                        pnlStart.SendToBack();
                    }
                    if (pnlStart.Location.Y > pnlTaskbar.Top)
                    {
                        pnlStart.Visible = false;
                        pnlStart.Location = new Point(pnlStart.Left, pnlTaskbar.Top);
                        startMenuState = StartMenuStates.Closed;
                        return;
                    }
                    else
                    {
                        pnlStart.Location = new Point(
                            pnlStart.Left,
                            pnlStart.Top + 60
                        );
                    }
                });

                Thread.Sleep(1);
            }

            
            return;
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            if (startMenuState == StartMenuStates.Closed)
            {
                if (popupThread == null || popupThread.ThreadState != ThreadState.Running)
                {
                    popupThread = new Thread(PopUpThreadWork);
                    popupThread.Start();
                }
            }
            else if (startMenuState == StartMenuStates.Opened)
            {
                if (popoffThread == null || popoffThread.ThreadState != ThreadState.Running)
                {
                    popoffThread = new Thread(PopOffThreadWork);
                    popoffThread.Start();
                }
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
            if (startMenuState == StartMenuStates.Opened 
                && ActiveControl != pnlStart 
                && IsLastClickOutsideOfStartMenu())
            {
                pnlStart.Visible = false;
                pnlStart.Location = new Point(pnlStart.Left, pnlTaskbar.Top);
                startMenuState = StartMenuStates.Closed;
                LastClickPoint = Point.Empty;
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

            var clientPos = PointToClient(LastClickPoint);
            var isInTaskBarArea = pnlTaskbar.Bounds.Contains(clientPos);
            var isInStartMenuArea = pnlStart.Bounds.Contains(clientPos);

            if (isInStartMenuArea || isInTaskBarArea)
            {
                return false;
            }

            return true;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            LastClickPoint = Cursor.Position;
            ActiveControl = null;
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            e.Control.MouseDown += Control_MouseClick;
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);

            e.Control.MouseDown -= Control_MouseClick;
        }

        private void Control_MouseClick(object sender, MouseEventArgs e)
        {
            LastClickPoint = Cursor.Position;
            ActiveControl = sender as Control;
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

            AddEventHandlers(form);
        }

        private void AddEventHandlers(Control control)
        {
            control.MouseDown -= Control_MouseClick;
            control.MouseDown += Control_MouseClick;

            // 递归为所有子控件绑定事件处理方法
            foreach (Control child in control.Controls)
            {
                AddEventHandlers(child);
            }
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
                themeDemo.Shadow = false;
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
            //if (e.Button == MouseButtons.Left)
            //{
            //    Form f = new Form();
            //    f.TopLevel = false;
            //    f.FormBorderStyle = FormBorderStyle.None;

            //    f.Location = new Point(300, 300);
            //    this.Controls.Add(f);

            //    f.Show();

            //}
        }


        #endregion

        #region DPI

        private void MainForm_DpiPatch(object sender, EventArgs e)
        {
            btnStart.NormalImage = ScaleImage(Properties.Resources.Win7Normal);
            btnStart.HoverImage = ScaleImage(Properties.Resources.Win7Hover);
            btnStart.DownImage = ScaleImage(Properties.Resources.Win7Pressed);
        }

        private Image ScaleImage(Image originalImage)
        {
            float dpiScaleFactorX = this.ScaleFactorRatioX;
            float dpiScaleFactorY = this.ScaleFactorRatioY;

            if (dpiScaleFactorX > 1 || dpiScaleFactorY > 1)
            {
                double ratio = Math.Min(dpiScaleFactorX, dpiScaleFactorY);

                int newWidth = (int)(originalImage.Width * ratio);
                int newHeight = (int)(originalImage.Height * ratio);
                Image newImage = new Bitmap(newWidth, newHeight);
                using (Graphics g = Graphics.FromImage(newImage))
                {
                    g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                }

                return newImage;
            }
            else
            {
                return originalImage;
            }
        }

        #endregion

        #region TODO

        //todo : 最小化到任务栏

        #endregion

    }



}
