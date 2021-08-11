using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    public partial class IconStackView : NonFlickerUserControl//UserControl
    {
        public new static Size DefaultSize = new Size(54, 74);

        public enum StackModes
        {
            TopToBottom,
            Center,
            BottomToTop
        }

        #region 字段

        protected List<Image> _imageLayers;
        protected string _overrideText = string.Empty;
        protected string _textToDisplay = string.Empty;
        protected StackModes _stackMode = StackModes.TopToBottom;

        #endregion

        #region 属性

        public bool ShowCaption { get; set; }

        public string Caption
        {
            get
            {
                return _overrideText;
            }
            set
            {
                _overrideText = value;
                Invalidate();
            }
        }

        public List<Image> Images
        {
            get
            {
                return _imageLayers;
            }
            set
            {
                _imageLayers = value;
            }
        }

        #endregion

        #region 构造

        public IconStackView()
        {
            InitializeComponent();

            _imageLayers = new List<Image>(2);// new Image[2];
            //_imageLayers[0] = new Bitmap(54, 54);
            //_imageLayers[1] = new Bitmap(34, 34);
            _imageLayers.Add(new Bitmap(54, 54));
            _imageLayers.Add(new Bitmap(34, 34));

            this.SuspendLayout();
            this.Padding = new Padding(0);
            this.Size = DefaultSize;
            this.ResumeLayout();
        }
        

        public IconStackView(params Image[] images) 
            : this()
        {
            if (images.Length > 0)
            {
                _imageLayers = new List<Image>();// new Image[images.Length];                
                for (int i = 0; i < images.Length; i++)
                {
                    if (images[i] != null)
                    {
                        _imageLayers.Add(images[i]);
                    }
                }
            }
        }



        #endregion

        #region 重写的成员

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // 画图层
            DrawLayers(g);

            // 画文字
            if (this.ShowCaption && !string.IsNullOrEmpty(this.Caption))
            {
                RectangleF textArea = GetTextArea();
                if (!string.IsNullOrEmpty(_textToDisplay))
                {
                    Brush fontBrush = new SolidBrush(this.ForeColor);
                    g.DrawString(_textToDisplay, this.Font, fontBrush, textArea);
                    fontBrush.Dispose();
                }
            }

            base.OnPaint(e);

        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);
            
            DragDropInfo info = drgevent.Data.GetData(typeof(DragDropInfo)) as DragDropInfo;
            if (info != null && info.CarriedData as PortraitIcon != null)
            {
                (info.CarriedData as PortraitIcon).SetDragRelease();
            }
        }

        #endregion

        protected virtual void DrawLayers(Graphics g)
        {
            for (int i = 0; i < _imageLayers.Count; i++)
            {
                if (_imageLayers[i] != null)
                {
                    g.DrawImage(_imageLayers[i], GetLayerArea(i));
                }
            }

        }

        protected virtual Rectangle GetLayerArea(int layerIndex)
        {
            int x = 0, y = 0, width = 1, height = 1;

            Image img = _imageLayers[layerIndex];
            switch (_stackMode)
            {
                case StackModes.Center:
                    width = img.Width;
                    height = img.Height;
                    x = (this.Width - width - this.Padding.Left - this.Padding.Right) / 2;
                    y =(this.Height - height - this.Padding.Top - this.Padding.Bottom) / 2;
                    break;
                case StackModes.TopToBottom:
                    width = img.Width;
                    height = img.Height;
                    x = this.Padding.Left + (this.Width - width) / 2;
                    y = this.Padding.Top;
                    break;
                case StackModes.BottomToTop:
                    // 未实现
                    break;
                default:
                    break;
            }
            

            return new Rectangle(x, y, width, height);
        }

        /// <summary>
        /// 获取文字区域
        /// </summary>
        /// <returns></returns>
        protected virtual RectangleF GetTextArea()
        {
            RectangleF rect = RectangleF.Empty;

            if (!String.IsNullOrEmpty(this.Caption))
            {
                string textToDraw = this.Caption;
                string ellipsisText = "...";

                SizeF stringSize = SizeF.Empty;
                SizeF ellipsisSize = SizeF.Empty;
                using (Graphics g = this.CreateGraphics())
                {
                    stringSize = g.MeasureString(textToDraw, this.Font);
                    ellipsisSize = g.MeasureString(ellipsisText, this.Font);

                    // 文字过长的处理
                    bool trimming = false;
                    int textLength = this.Caption.Length;
                    int textToRemove = this.Caption.Length - 1;
                    float availableWidth = this.Bounds.Width;
                    while (textToRemove > 0 && stringSize.Width > availableWidth)
                    {
                        textToDraw = this.Caption.Remove(textToRemove);
                        stringSize = g.MeasureString(textToDraw + ellipsisText, this.Font);

                        textToRemove--;
                        trimming = true;
                    }

                    if (trimming == true)
                    {
                        _textToDisplay = textToDraw + ellipsisText;
                    }
                    else
                    {
                        _textToDisplay = textToDraw;
                    }
                }

                float stringPosX = (this.Bounds.Width - stringSize.Width) / 2;
                float stringPosY = this.Height - stringSize.Height;

                rect = new RectangleF(stringPosX, stringPosY, stringSize.Width, stringSize.Height);
            }

            return rect;
        }
            
    }
}
