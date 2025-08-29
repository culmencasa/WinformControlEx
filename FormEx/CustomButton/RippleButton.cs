using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Windows.Forms
{

    /// <summary>
    /// 波纹按钮控件
    /// </summary>
    [DefaultEvent("Click")]
    public class RippleButton : Control
    {
        #region 构造函数

        public RippleButton()
        {
            this.SetStyle(ControlStyles.Selectable
                | ControlStyles.StandardClick
                | ControlStyles.ResizeRedraw
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.DoubleBuffer
                | ControlStyles.UserPaint
                | ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();


            this.BackColor = Color.BlueViolet;
            this.ForeColor = Color.White;
            this.Width = 120;
            this.Height = 50;
            this.RoundCornerRadius = 50;
            this.BorderColor = Color.LightGray;
            this.HoverColor = BlendColors(Color.FromArgb(200, Color.White), BackColor);
            //this.FocusColor = Color.FromArgb(128, Color.White);

            expandTimer = new Timer();
            expandTimer.Interval = 10; // 设置定时器间隔
            expandTimer.Tick += TimerTick;
        }

        #endregion

        #region 字段

        private int expandSpeed = 12; 
        private bool isMouseDown = false;
        private bool isMouseHover = false;
        private Timer expandTimer;
        private List<Ripple> ripples = new List<Ripple>();


        #endregion

        #region 属性

        public ButtonTypes ButtonType
        {
            get;
            set;
        }

        public int RoundCornerRadius
        {
            get;
            set;
        }

        public Color BorderColor
        {
            get;
            set;
        }

        public Color HoverColor 
        {
            get;
            set;
        }


        //public Color FocusColor
        //{
        //    get;
        //    set;
        //}



        #endregion

        #region 重写的方法

        protected override void OnMouseEnter(EventArgs e)
        {
            isMouseHover = true;
            Invalidate();

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            isMouseHover = false;
            Invalidate();

            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            isMouseDown = true; 

            base.OnMouseDown(mevent);


            ripples.Add(new Ripple(mevent.Location)); 
            if (!expandTimer.Enabled)
            {
                expandTimer.Start();
            }

        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            isMouseDown = false;
            Invalidate();

            base.OnMouseUp(e);

        }


        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            Graphics g = e.Graphics;
            g.SetSlowRendering();

            #region 模拟透明度

            GraphicsContainer gc = g.BeginContainer();
            try
            {
                // 平移图形以模拟透明度   
                g.TranslateTransform(-this.Left, -this.Top);
                PaintEventArgs pe = new PaintEventArgs(g, this.Bounds);
                this.InvokePaintBackground(this.Parent, pe);
                this.InvokePaint(this.Parent, pe);
            }
            catch
            {
                // 处理异常
            }
            finally
            {
                g.ResetTransform();
                g.EndContainer(gc);
            }

            #endregion
             
            switch(ButtonType)
            {
                case ButtonTypes.Contained:
                    DrawContainedButton(g);
                    break;
                case ButtonTypes.Outlined:
                    DrawOutlinedButton(g);
                    break;
                case ButtonTypes.Text:
                    DrawTextButton(g);
                    break;
            }
        
        }

        private void DrawTextButton(Graphics g)
        {
            GraphicsPath path = CreateRoundedRectanglePath(ClientRectangle, RoundCornerRadius);

            if (isMouseHover)
            {
                using (Brush bgBrush = new SolidBrush(HoverColor))
                {
                    g.FillPath(bgBrush, path);
                }
            }
        }

        private void DrawOutlinedButton(Graphics g)
        {
            GraphicsPath path = CreateRoundedRectanglePath(ClientRectangle, RoundCornerRadius);

            if (isMouseHover)
            {
                using (Brush bgBrush = new SolidBrush(HoverColor))
                {
                    g.FillPath(bgBrush, path);
                }
            }

            using (Pen borderPen = new Pen(BorderColor, 1))
            {
                g.DrawPath(borderPen, path);
            }
        }

        private void DrawContainedButton(Graphics g)
        {
            GraphicsPath path = CreateRoundedRectanglePath(ClientRectangle, RoundCornerRadius);
            using (Brush bgBrush = new SolidBrush(BackColor))
            {
                g.FillPath(bgBrush, path);
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        { 
            Graphics g = pevent.Graphics;
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            #region 绘制波纹

            if (ripples.Count > 0)
            {
                Color rippleColor = Color.FromArgb(100, Color.White);

                using (GraphicsPath path = CreateRoundedRectanglePath(ClientRectangle, RoundCornerRadius))
                {
                    g.SetClip(path);
                     
                    foreach (var ripple in ripples)
                    {
                        using (Brush brush = new SolidBrush(rippleColor))
                        {
                            g.FillEllipse(brush, ripple.Position.X - ripple.Radius, ripple.Position.Y - ripple.Radius, ripple.Radius * 2, ripple.Radius * 2);
                        }
                    }

                    g.ResetClip();
                }
            }
            
            #endregion

            #region 绘制文本

            using (Brush textBrush = new SolidBrush(ForeColor))
            {
                StringFormat sf = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip);
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                g.DrawString(this.Text, this.Font, textBrush, this.ClientRectangle, sf);
            }

            #endregion

        }

        #endregion

        #region 私有方法
        private Color BlendColors(Color color1, Color color2)
        {
            // 提取颜色的分量
            float alpha1 = color1.A / 255f; // 第一个颜色的 alpha
            float alpha2 = color2.A / 255f; // 第二个颜色的 alpha

            // 计算混合后的 alpha
            float finalAlpha = alpha1 + alpha2 * (1 - alpha1);

            // 计算混合后的颜色分量
            int red = (int)((color1.R * alpha1 + color2.R * alpha2 * (1 - alpha1)) / finalAlpha);
            int green = (int)((color1.G * alpha1 + color2.G * alpha2 * (1 - alpha1)) / finalAlpha);
            int blue = (int)((color1.B * alpha1 + color2.B * alpha2 * (1 - alpha1)) / finalAlpha);

            // 返回混合后的颜色
            return Color.FromArgb((int)(finalAlpha * 255), red, green, blue);
        }
        private GraphicsPath CreateRoundedRectanglePath(Rectangle bounds, int cornerRadius)
        {
            GraphicsPath roundedRectPath = new GraphicsPath();

            // 检查并确保矩形宽度和高度有效
            if (bounds.Width <= 1 || bounds.Height <= 1)
            {
                // 如果宽度或高度太小，直接添加一个简单的矩形路径
                roundedRectPath.AddRectangle(bounds);
                return roundedRectPath;
            }

            // 限制 cornerRadius，不超过宽度或高度的一半
            cornerRadius = Math.Max(0, Math.Min(cornerRadius, Math.Min(bounds.Width, bounds.Height) / 2));
            float diameter = cornerRadius * 2F;

            // 如果 cornerRadius 为 0，处理为普通矩形
            if (cornerRadius == 0)
            {
                roundedRectPath.AddRectangle(bounds);
                return roundedRectPath;
            }

            // 添加圆角的四个弧形
            roundedRectPath.StartFigure();
            roundedRectPath.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90); // 左上角
            roundedRectPath.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90); // 右上角
            roundedRectPath.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90); // 右下角
            roundedRectPath.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90); // 左下角
            roundedRectPath.CloseFigure();

            return roundedRectPath;
        }


        #endregion

        #region TimerTick事件


        private void TimerTick(object sender, EventArgs e)
        {
            // 更新所有波纹的半径
            for (int i = ripples.Count - 1; i >= 0; i--)
            {
                ripples[i].Radius += expandSpeed;

                // 移除已扩展到最大半径的波纹
                if (ripples[i].Radius > Math.Max(Width, Height))
                {
                    ripples.RemoveAt(i);
                }
            }

            // 如果没有波纹，停止定时器
            if (ripples.Count == 0)
            {
                expandTimer.Stop();
                if (isMouseDown)
                {
                    return;
                }
            }


            Invalidate(); // 触发重绘
        }

        #endregion

        #region 枚举及内嵌类

        public enum ButtonTypes
        { 
            Contained,
            Outlined,
            Text
        }


        private class Ripple
        {
            public Point Position { get; }
            public int Radius { get; set; }

            public Ripple(Point position)
            {
                Position = position;
                Radius = 0;
            }
        }

        #endregion
    }

}