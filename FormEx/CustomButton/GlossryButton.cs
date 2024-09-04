using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Utils;
using Utils.UI;

namespace System.Windows.Forms
{
    public class GlossyButton : Button
    {
        #region 字段

        private Point _lightSource;
        private Color _backColor;
        private Color _borderColor;
        private bool _isMouseHover = false;

        #endregion

        #region 构造函数

        public GlossyButton()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);

            _lightSource = new Point(0, 0);
            BackColor = Color.FromArgb(138, 102, 181);
            BorderColor = ColorEx.LightenColor(BackColor, 20);


            MouseEnter += GlossyButton_MouseEnter;
            MouseMove += GlossyButton_MouseMove;
            MouseLeave += GlossyButton_MouseLeave;
        }

        #endregion

        #region 设计器属性


        [Category("Custom"), DefaultValue(typeof(Color), "138, 102, 181")]
        public new Color BackColor
        {
            get
            {
                return _backColor;
            }
            set
            {
                _backColor = value;
                Invalidate();
            }
        }

        [Category("Custom"), DefaultValue(typeof(Color), "138, 102, 181")]
        public Color BorderColor
        {
            get
            {
                return _borderColor;
            }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }


        #endregion

        #region 事件处理


        private void GlossyButton_MouseEnter(object? sender, EventArgs e)
        {
            _isMouseHover = true;
        }

        private void GlossyButton_MouseLeave(object? sender, EventArgs e)
        {
            _isMouseHover = false;
        }

        private void GlossyButton_MouseMove(object sender, MouseEventArgs e)
        {
            // 更新光源位置  
            _lightSource = e.Location;
            this.Invalidate(); // 触发重绘  
        }

        #endregion

        #region 重写方法

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            Rectangle rect = this.ClientRectangle;

            g.Clear(this.BackColor);

            if (_isMouseHover)
            {
                #region 绘制光源效果

                // 计算光源相对于按钮的中心位置  
                Point center = new Point(rect.Width / 2, rect.Height / 2);
                float distanceX = _lightSource.X - center.X;
                float distanceY = _lightSource.Y - center.Y;

                // 创建径向模糊效果
                var centerColor = ColorEx.LightenColor(BackColor, 30);
                for (int i = 0; i < 10; i++)
                {
                    using (GraphicsPath glowPath = new GraphicsPath())
                    {
                        int radius = 50 + i * 5; // 增加半径
                        glowPath.AddEllipse(_lightSource.X - radius, _lightSource.Y - radius, radius * 2, radius * 2);

                        using (PathGradientBrush glowBrush = new PathGradientBrush(glowPath))
                        {
                            // 设置中心颜色透明度逐渐降低
                            glowBrush.CenterColor = Color.FromArgb(70 - (i * 5), centerColor);
                            glowBrush.SurroundColors = new Color[] { Color.Transparent };
                            g.FillRectangle(glowBrush, rect);
                        }
                    }
                }

                // 边框颜色设置  
                Color borderColor1 = Color.White;
                Color borderColor2 = BorderColor;

                // 计算渐变角度  
                float angle = (float)(Math.Atan2(-distanceY, -distanceX) * (180 / Math.PI));

                // 设置渐变范围  
                float gradientRange = 50; // 渐变范围  
                Rectangle gradientRect = new Rectangle(
                    rect.X - (int)gradientRange,
                    rect.Y - (int)gradientRange,
                    rect.Width + (int)(2 * gradientRange),
                    rect.Height + (int)(2 * gradientRange)
                );


                // 绘制渐变边框  
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddRectangle(rect);
                    using (LinearGradientBrush borderBrush = new LinearGradientBrush(gradientRect, borderColor1, borderColor2, angle))
                    {
                        // 设置渐变停靠点
                        ColorBlend blend = new ColorBlend();
                        blend.Colors = new Color[]
                        {
                            borderColor1,
                            Color.FromArgb(200, borderColor1),
                            Color.FromArgb(150, borderColor1),
                            Color.FromArgb(50, borderColor1)
                        };
                        blend.Positions = new float[] { 0.0f, 0.4f, 0.6f, 1.0f }; // 白色多一点，更明显一些
                        borderBrush.InterpolationColors = blend;
                        g.DrawPath(new Pen(borderBrush, 4), path);
                    }
                }

                #endregion
            }
            else
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddRectangle(rect);
                    g.DrawPath(new Pen(BorderColor, 4), path);
                }
            }

            // 绘制按钮文本  
            TextRenderer.DrawText(g, this.Text, this.Font, rect, Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        #endregion

    }
}
