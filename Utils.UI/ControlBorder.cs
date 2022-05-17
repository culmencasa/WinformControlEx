using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Utils.UI
{
    public class ControlBorder
    {
        #region 构造

        public ControlBorder(Control control)
        {
            source = control;
            source.ParentChanged += Source_ParentChanged;
            source.SizeChanged += Source_SizeChanged;
            source.LocationChanged += Source_LocationChanged;
            source.LostFocus += Source_LostFocus;
            Apply();
        }


        #endregion

        #region 枚举

        public enum DrawBorderTiming
        { 
            Always,
            MouseDown,
            Custom
        }

        #endregion

        #region 字段

        DrawBorderTiming _timing = DrawBorderTiming.MouseDown;
        int _diameter = 8;
        int offset = 1;
        System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        bool _keepBorderOnFocus = false;

        protected Control source;



        #endregion


        #region 属性

        public Color BorderColor { get; set; } = Color.FromArgb(194, 213, 255);

        public int Diameter
        {
            get { return _diameter; }
            set
            {
                _diameter = value;
                if (value < 0)
                {
                    _diameter = 0;
                }
            }
        }


        /// <summary>
        /// 边框厚度
        /// </summary>
        public int BorderWidth { get; set; } = 4;
        /// <summary>
        /// 画出的矩形的中心有问题, 原因未知. 使用Offset修正.
        /// </summary>
        public int Offset { get => offset; set => offset = value; }

        /// <summary>
        /// 是否一直显示
        /// </summary>
        public bool KeepBorderOnFocus
        {
            get
            {
                return _keepBorderOnFocus;
            }
            set
            {
                _keepBorderOnFocus = value;
            }
        }

        /// <summary>
        /// 画边框的时机
        /// </summary>
        public DrawBorderTiming Timing
        {
            get
            {
                return _timing;
            }
            set
            {
                _timing = value;
            }
        }


        /// <summary>
        /// 标记是否重画
        /// </summary>
        private bool DrawRequired { get; set; } = false;

        /// <summary>
        /// 标记边框是否画过
        /// </summary>
        private bool BorderDrawed { get; set; } = false;

        #endregion

        #region 委托

        public Func<bool> DrawBorderCondition;

        #endregion

        #region 公开方法

        public void AddEventHandler(string eventName, Delegate drawBorderCondition)
        {
            var controlType = source.GetType();
            EventInfo eventInfo = controlType.GetEvent(eventName);

            eventInfo.AddEventHandler(source, drawBorderCondition);
        }

        public void Apply()
        {
            Control parentControl = source.Parent;
            if (parentControl != null)
            {
                parentControl.Paint -= ParetnControl_Paint;
                parentControl.Paint += ParetnControl_Paint;

                switch (Timing)
                {
                    case DrawBorderTiming.Always:
                        DrawBorderCondition = () => { return true; };

                        DrawRequired = true;
                        InvalidateParentControl();
                        break;
                    case DrawBorderTiming.MouseDown:
                        source.MouseDown -= OnControlMouseDown;
                        source.MouseUp -= OnControlMouseUp;

                        source.MouseDown += OnControlMouseDown;
                        source.MouseUp += OnControlMouseUp;
                        break;
                    case DrawBorderTiming.Custom:

                        if (DrawBorderCondition == null)
                        {
                            throw new ArgumentNullException("请先指定 DrawBorderCondition 委托");
                        }
                        else
                        {
                            // 使用Timer保持DrawBorderCondition事件发生 
                            InitTimer();
                        }
                        break;
                }
            }
        }

        public void DrawBorder()
        {
            Control parentControl = source.Parent;
            if (parentControl == null)
                return;

            if (DrawBorderCondition != null)
            {
                if (DrawBorderCondition())
                { 
                    //if (BorderDrawed)
                    //{
                    //    return;
                    //}
                    BorderDrawed = true;

                    parentControl.Paint += ParetnControl_Paint;
                    source.Parent.Invalidate();
                    parentControl.Paint -= ParetnControl_Paint;

                    Console.WriteLine("true");
                }
                else
                {
                    //if (!BorderDrawed)
                    //{
                    //    return;
                    //}
                    BorderDrawed = false;
                    parentControl.Paint += ParetnControl_Paint;
                    source.Parent.Invalidate();
                    parentControl.Paint -= ParetnControl_Paint;

                    Console.WriteLine("false");
                }
            }
        }

        #endregion

        #region 私有方法

        private void InitTimer()
        {
            myTimer.Tick += new EventHandler(TimerEventProcessor);
            myTimer.Interval = 500;
            myTimer.Start();
        }

        private void TimerEventProcessor(object myObject, EventArgs myEventArgs)
        {
            if (DrawBorderCondition == null)
            {
                myTimer.Stop();
                return;
            }

            if (DrawBorderCondition())
            {
                DrawRequired = true;
                InvalidateParentControl();
            }
            else
            { 
                
            }
        }

        #region 鼠标按下时画边框

        private void OnControlMouseDown(object sender, MouseEventArgs e)
        {
            if (KeepBorderOnFocus)
            {
                if (!BorderDrawed)
                {
                    DrawRequired = true;
                    InvalidateParentControl();
                    BorderDrawed = true;
                }
                else
                {
                    DrawRequired = false;
                    InvalidateParentControl();
                    BorderDrawed = false;
                }
            }
            else
            {
                DrawRequired = true;
                InvalidateParentControl();
            }
        }

        private void OnControlMouseUp(object sender, MouseEventArgs e)
        {
            if (KeepBorderOnFocus)
            {
            }
            else
            {
                DrawRequired = false;
                InvalidateParentControl();
            }
        }

        #endregion

        protected void InvalidateParentControl()
        {
            Control parentControl = source.Parent;
            if (parentControl != null)
            {
                parentControl.Invalidate();
            }
        }

        private void Source_Paint(object sender, PaintEventArgs e)
        {
            if (DrawBorderCondition != null)
            {
                if (DrawBorderCondition())
                {
                    if (BorderDrawed)
                    {
                        return;
                    }
                    BorderDrawed = true;
                    source.Parent.Invalidate();

                    Console.WriteLine("true");
                }
                else
                {
                    if (!BorderDrawed)
                    {
                        return;
                    }
                    BorderDrawed = false;
                    source.Parent.Invalidate();

                    Console.WriteLine("false");
                }
            }
        }

        #endregion

        #region 事件处理

        private void Source_ParentChanged(object sender, EventArgs e)
        {
            Control paretnControl = source.Parent;
            if (paretnControl != null)
            {
                paretnControl.Paint -= ParetnControl_Paint;
                paretnControl.Paint += ParetnControl_Paint;
            }
        }

        private void ParetnControl_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (DrawRequired)
            {
                Rectangle bounds = new Rectangle(source.Left - Offset, source.Top - Offset, source.Width + Offset, source.Height + Offset);
                Rectangle clipRect = new Rectangle(e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Width, e.ClipRectangle.Height);
                // 检查是否需要画边框
                if (bounds.IntersectsWith(clipRect))
                {
                    if (Diameter <= 0)
                    {
                        Rectangle rect = new Rectangle(source.Left - BorderWidth - Offset, source.Top - BorderWidth - Offset, source.Width + BorderWidth * 2  + Offset, source.Height + BorderWidth * 2  + Offset);
                        using (Pen pen = new Pen(BorderColor, BorderWidth))
                        {
                            g.DrawRectangle(pen, rect);
                        }
                    }
                    else
                    {
                        Rectangle rect = new Rectangle(source.Left - BorderWidth - Offset, source.Top - BorderWidth - Offset, source.Width + BorderWidth + BorderWidth + Offset, source.Height + BorderWidth + BorderWidth + Offset);
                        
                        using (Pen pen = new Pen(BorderColor, BorderWidth))
                        {

                            g.DrawRoundedRectangle(pen, rect, Diameter);

                        }

                    }
                }
            } 
        }

        private void Source_LocationChanged(object sender, EventArgs e)
        {
            Control paretnControl = source.Parent;
            if (paretnControl != null)
            {
                paretnControl.Invalidate();
            }
        }

        private void Source_SizeChanged(object sender, EventArgs e)
        {
            Control paretnControl = source.Parent;
            if (paretnControl != null)
            {
                paretnControl.Invalidate();
            }
        }

        private void Source_LostFocus(object sender, EventArgs e)
        {
            DrawRequired = false;
            InvalidateParentControl();
            BorderDrawed = false;
        }

        #endregion
    }
}
