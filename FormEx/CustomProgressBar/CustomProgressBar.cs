﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;

namespace System.Windows.Forms
{
    /// <summary>
    /// 自定义进度条
    /// </summary>
    public class CustomProgressBar : NonFlickerUserControl
    {
        #region 委托和事件

        public event Action OnProgressCompleted;

        #endregion

        #region 字段

        private Color _borderColor;
        private Color _progressBarColor;
        private Color _progressBackColor;

        private bool _isWorking;
        private Timer _animateTimer;

        /// <summary>
        /// 控件宽度和总进度的比值
        /// </summary>
        private float _pixelUnit;
        /// <summary>
        /// 当前增长的值
        /// </summary>
        private float _animateIncreasedValue;

        /// <summary>
        /// 动画要到达的目标值
        /// </summary>
        private float _animateTargetValue;

        private float LastIncreament;
        private float LastIncreamentASync;
        /// <summary>
        /// 增长加速度(目前没用)
        /// </summary>
        private float _increasingSpeed;
        /// <summary>
        /// 进度条的值
        /// </summary>
        private float _value;
        /// <summary>
        /// 进度条缓冲图像
        /// </summary>
        private Image _bufferImage;


        /// <summary>
        /// 边框圆角半径
        /// </summary>
        private int _borderRadius;

        #endregion

        #region 属性

        /// <summary>
        /// 控件背景色, 不包含进度条
        /// </summary>
        [Category("Custom")]
        public override Color BackColor 
        { 
            get => base.BackColor;
            set
            {
                base.BackColor = value;
                ForceRender();
            }
        }

        [Category("Custom")]
        [DefaultValue(typeof(Color),"#FFFFFF")]
        public override Color ForeColor         
        {
            get
            {
                return
                    base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                ForceRender();
            }
        }
        /// <summary>
        /// 边框色
        /// </summary>
        [Category("Custom")]
        public Color BorderColor
        {
            get
            {
                return _borderColor;
            }
            set
            {
                _borderColor = value;
                ForceRender();
            }
        }


        /// <summary>
        /// 进度条前景色
        /// </summary>
        [Category("Custom")]
        public Color ProgressBarColor { get
            {
                return _progressBarColor;
            }
            set
            {
                _progressBarColor = value;
                ForceRender();
            }
        }

        /// <summary>
        /// 进度条背景色
        /// </summary>
        [Category("Custom")]
        public Color ProgressBackColor { get
            {
                return _progressBackColor;
            }
            set
            {
                _progressBackColor = value;
                ForceRender();
            }
        }


        /// <summary>
        /// 边框圆角
        /// </summary>
        [Category("Custom")]
        [DefaultValue(20)]
        public int BorderRadius
        {
            get
            {
                return _borderRadius;
            }
            set
            {
                _borderRadius = value;
                ForceRender();
            }
        }


        /// <summary>
        /// 进度条值
        /// </summary>
        [Category("Custom")]
        [DefaultValue(0)]
        public float Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                ForceRender();
            }
        }

        /// <summary>
        /// 进度条最小值
        /// </summary>
        [Category("Custom")]
        [DefaultValue(0)]
        public float MinValue { get; set; }

        /// <summary>
        /// 进度条最大值
        /// </summary>
        [Category("Custom")]
        [DefaultValue(100)]
        public float MaxValue { get; set; }

        /// <summary>
        /// 缓冲图像
        /// </summary>
        protected Image BufferImage
        {
            get
            {
                return _bufferImage;
            }
            set
            {
                _bufferImage = value;
            }
        }

        /// <summary>
        /// 动画Timer
        /// </summary>
        protected Timer AnimateTimer
        {
            get
            {
                if (_animateTimer == null)
                {
                    _animateTimer = new Timer();
                }
                return _animateTimer;
            }
            set
            {
                _animateTimer = value;
            }
        }

        #endregion

        #region 构造

        public CustomProgressBar()
        {
            _increasingSpeed = 1;
               
            BorderRadius = 20;
            Value = 0;
            MinValue = 0;
            MaxValue = 100;
            Padding = new Padding(5);
            ForeColor = Color.White;
            BackColor = Color.Transparent;
            ProgressBackColor = Color.MidnightBlue;
            ProgressBarColor = Color.RoyalBlue;

            _animateTargetValue = MaxValue;
        }

        #endregion

        #region 重写的成员

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _animateIncreasedValue = Value;
            ForceRender();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (this.IsHandleCreated && !this.IsDisposed)
            {
                ForceRender();
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            if (BufferImage != null)
            {
                g.DrawImage(BufferImage, 0, 0);
            }

        }

        #endregion

        #region 公开方法 

        public void Pause()
        {
            if (_animateTimer != null)
            {
                _isWorking = false;
                _animateTimer.Stop();
                _animateTimer.Dispose();
                _animateTimer = null;
            }
        }

        public void Reset()
        {
            Pause();
            this.Value = 0;
            this._animateIncreasedValue = 0;
            LastIncreament = 0;


            ForceRender();
        }

        /// <summary>
        /// 增加到指定百分比进度
        /// </summary>
        /// <param name="addUpValue">预估工作到达的总进度</param>
        /// <param name="estimateTime">预估工作时间</param>
        public void MakeProgress(float addUpValue, int estimateTime)
        {
            if (addUpValue > MaxValue)
            {
                addUpValue = MaxValue;
            }
            
            if (addUpValue <= _animateIncreasedValue)
            {
                return;
            }
            else
            {
                if (_isWorking)
                {
                    Pause();
                }

                _animateTargetValue = addUpValue;

                float increament = (addUpValue - LastIncreament);
                LastIncreament = increament;

                AnimateTimer.Interval = (int)(estimateTime / increament);
                AnimateTimer.Tick += new EventHandler(this.AnimateTimer_Tick);
                AnimateTimer.Start();
            }
        }

        /// <summary>
        /// 未实现
        /// </summary>
        public void AddAsyncPlan(float addUpValue, int estimateTime)
        { 
            
        }

           

        /// <summary>
        /// 增加指定百分比进度
        /// </summary>
        /// <param name="increment">指定进度条增加的量</param>
        public void MakeProgress(int increment)
        {
            if (_animateIncreasedValue + increment < MaxValue)
            {
                _animateTargetValue = _animateIncreasedValue + increment;
            }
            else if (_animateIncreasedValue + increment >= MaxValue)
            {
                _animateTargetValue = MaxValue;
            }
            else
            {
                return;
            }

            if (_isWorking)
            {
                Pause();
            }

            AnimateTimer.Interval = 50;
            AnimateTimer.Tick += new EventHandler(this.AnimateTimer_Tick);
            AnimateTimer.Start();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取进度条矩形
        /// </summary>
        /// <returns></returns>
        private RectangleF GetBounds()
        {
            float x = this.Padding.Left;
            float y = this.Padding.Top;
            float width = this.Width - this.Padding.Left - this.Padding.Right;
            float height = this.Height - this.Padding.Top - this.Padding.Bottom;

            return new RectangleF(x, y, width, height);
        }

        /// <summary>
        /// 获取当前进度矩形大小
        /// </summary>
        /// <returns></returns>
        private RectangleF GetProgressBounds()
        {
            _pixelUnit = GetBounds().Width / 100f;

            float x = this.Padding.Left;
            float y = this.Padding.Top;

            float width = _animateIncreasedValue * _pixelUnit;
            if (DesignMode)
            {
                width = Value * _pixelUnit;
            }
            float height = this.Height - this.Padding.Top - this.Padding.Bottom;

            var bounds = GetBounds();
            if (width > bounds.Width)
            {
                width = bounds.Width;
            }
            return new RectangleF(x, y, width, height);
        }

        /// <summary>
        /// 画进度条
        /// </summary>
        /// <returns></returns>
        private Image DrawProgressBar()
        {
            Bitmap bufferImage = new Bitmap(this.Width, this.Height);
            using (Graphics g = Graphics.FromImage(bufferImage))
            {
                g.SetSlowRendering();
                g.Clear(BackColor);

                using (Brush brush = new SolidBrush(ProgressBackColor))
                {
                    g.FillRoundedRectangle(brush, GetBounds(), BorderRadius);
                }

                using (Brush brush = new SolidBrush(_progressBarColor))
                {
                    g.FillRoundedRectangle(brush, GetProgressBounds(), BorderRadius);
                }

                using (Pen pen = new Pen(BorderColor))
                {
                    g.DrawRoundedRectangle(pen, GetBounds(), BorderRadius);
                }

                string progressText = "0%";
                if (DesignMode)
                {
                    progressText = Value + "%";
                }
                else
                {
                    progressText = _animateIncreasedValue + "%";
                }
                SizeF textSize = g.MeasureString(progressText, this.Font);
                g.DrawString(progressText, 
                    this.Font,
                    new SolidBrush(this.ForeColor),
                    new PointF((this.Width - textSize.Width) / 2, (this.Height - textSize.Height) / 2));
            }

            return bufferImage;
        }

        /// <summary>
        /// 强制刷新画面
        /// </summary>
        private void ForceRender()
        {
            this.BufferImage = DrawProgressBar();
            Invalidate();
        }

        #endregion

        #region 动画事件

        private void AnimateTimer_Tick(object sender, EventArgs e)
        {
            _isWorking = true;

            ForceRender();

            _animateIncreasedValue++;




            if (_animateIncreasedValue >= MaxValue)
            {
                _animateIncreasedValue = MaxValue;
                Value = MaxValue;


                Pause();

                OnProgressCompleted?.Invoke();
            }
            else if (_animateIncreasedValue >= _animateTargetValue)
            {
                _animateIncreasedValue = _animateTargetValue;
                Value = _animateTargetValue;

                Pause();
            }
        }

        #endregion

    }
}
