using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils;
using Utils.UI;
using static System.Net.Mime.MediaTypeNames;

namespace FormExCore
{
    public class OcnTile : NonFlickerUserControl
    {

        #region 字段

        System.Windows.Forms.Timer _mouseOverTimer = null;
        System.Windows.Forms.Timer _mouseLeaveTimer = null;
        bool _isMouseOver = false;
        int _totalTransitionSteps = 30;
        int _currentTransitionStep = 0;


        #endregion

        #region 属性

        [Category("Custom")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        [Category("Custom")]
        public override Color ForeColor { 
            get => base.ForeColor;
            set
            {
                base.ForeColor = value;
            }
        }


        [Category("Custom")]
        public Color MouseOverBackColor
        {
            get;
            set;
        }

        [Category("Custom")]
        public Color MouseOverForeColor
        {
            get;
            set;
        }

        [Browsable(false)]
        public Color DrawingBackColor
        {
            get;
            set;
        }

        [Browsable(false)]
        public Color DrawingForeColor
        {
            get;
            set;
        }

        private float _opacity = 1;
        public float Opacity
        {
            get
            {
                return _opacity;
            }
            set
            {
                _opacity = value;
                //Invalidate();
            }
        }

        #endregion

        public OcnTile()
        { 

        }

        



        #region 鼠标事件

        protected override void OnLoad(EventArgs e)
        {
            if (DrawingBackColor.IsEmpty)
            {
                DrawingBackColor = BackColor;
            }
            if (DrawingForeColor.IsEmpty)
            {
                DrawingForeColor = ForeColor;
            }
            Invalidate();

            base.OnLoad(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _isMouseOver = true;

            if (_mouseOverTimer == null)
            {


                _totalTransitionSteps = 30;
                _currentTransitionStep = 0;

                _mouseOverTimer = new System.Windows.Forms.Timer();
                _mouseOverTimer.Interval = 10;
                _mouseOverTimer.Tick += mouseOverTimer_Tick;
                _mouseOverTimer.Start();
            }
            

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isMouseOver = false;

            if (_mouseLeaveTimer == null)
            {
                _totalTransitionSteps = 30;
                _currentTransitionStep = 0;

                _mouseLeaveTimer = new System.Windows.Forms.Timer();
                _mouseLeaveTimer.Interval = 10;
                _mouseLeaveTimer.Tick += mouseLeaveTimer_Tick; ;
                _mouseLeaveTimer.Start();
            }

            base.OnMouseLeave(e);
        }

        #endregion


        #region Timer事件

        private void mouseOverTimer_Tick(object sender, EventArgs e)
        {
            if (!_isMouseOver)
            {
                StopMouseOverTimer();
                return;
            }

            float transitionAmount = (float)_currentTransitionStep / (_totalTransitionSteps - 1);

            this.DrawingBackColor = Lerp(BackColor, MouseOverBackColor, transitionAmount);
            this.DrawingForeColor = Lerp(ForeColor, MouseOverForeColor, transitionAmount);
            Invalidate();

            _currentTransitionStep++;
            if (_currentTransitionStep >= _totalTransitionSteps)
            {
                StopMouseOverTimer();
            }

        }

        private void mouseLeaveTimer_Tick(object sender, EventArgs e)
        {
            if (_isMouseOver)
            {
                StopMouseLeavTimer();
                return;
            }
            float transitionAmount = (float)_currentTransitionStep / (_totalTransitionSteps - 1);

            this.DrawingBackColor = Lerp(MouseOverBackColor, BackColor, transitionAmount);
            this.DrawingForeColor = Lerp(MouseOverForeColor, ForeColor, transitionAmount);
            Invalidate();

            _currentTransitionStep++;
            if (_currentTransitionStep >= _totalTransitionSteps)
            {
                StopMouseLeavTimer();
            }
        }

        #endregion

        #region 方法

        private void StopMouseOverTimer()
        {
            _mouseOverTimer.Stop();
            _mouseOverTimer.Dispose();
            _mouseOverTimer = null;
        }
        private void StopMouseLeavTimer()
        {
            _mouseLeaveTimer.Stop();
            _mouseLeaveTimer.Dispose();
            _mouseLeaveTimer = null;
        }

        private Color Lerp(Color color1, Color color2, float transitionAmount)
        {

            int a = (int)Math.Round(color1.A * (1 - transitionAmount) + color2.A * transitionAmount);
            int r = (int)Math.Round(color1.R * (1 - transitionAmount) + color2.R * transitionAmount);
            int g = (int)Math.Round(color1.G * (1 - transitionAmount) + color2.G * transitionAmount);
            int b = (int)Math.Round(color1.B * (1 - transitionAmount) + color2.B * transitionAmount);

            a = Math.Max(Math.Min(a, 255), 0);
            r = Math.Max(Math.Min(r, 255), 0);
            g = Math.Max(Math.Min(g, 255), 0);
            b = Math.Max(Math.Min(b, 255), 0);

            Color transitionColor = Color.FromArgb(a, r, g, b);

            return transitionColor;
        }

        #endregion

        #region 重绘

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);

            Graphics g = e.Graphics;
            var alpha = Math.Min((int)(Opacity * 255), 255);
            var color = Color.FromArgb(alpha, DrawingBackColor.R, DrawingBackColor.G, DrawingBackColor.B);
            g.Clear(color);
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (this.Text.IsNotEmpty())
            {
                var alpha = Math.Min((int)(Opacity * 255), 255);
                var color = Color.FromArgb(alpha, DrawingForeColor.R, DrawingForeColor.G, DrawingForeColor.B);
                using (SolidBrush brush = new SolidBrush(color))
                {
                    g.DrawStringCenterWrap(Text, Font, brush, this.ClientRectangle);
                }
            }

            base.OnPaint(e);
        }

        #endregion

    }
}

