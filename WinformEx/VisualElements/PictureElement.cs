using System;
using System.Drawing;
using System.IO;

namespace WinformEx
{
    public class PictureElement : UIElement
    {
        /// <summary>
        ///  指定图像在PictureElement中的定位方式
        /// </summary>
        public enum PictureSizeMode
        {
            /// <summary>
            ///  在控件区域范围内显示图像原始大小
            /// </summary>
            Normal = 0,
            /// <summary>
            ///  图像拉伸为控件大小显示
            /// </summary>
            Stretch,
            /// <summary>
            ///  显示图像原始大小
            /// </summary>
            AutoSize,
            /// <summary>
            ///  图像居中显示， 超出控件部分会拉伸
            /// </summary>
            CenterImage,
            /// <summary>
            ///  图像按比例在控件中拉伸显示
            /// </summary>
            Zoom
        }
    
        #region 私有字段

        private Bitmap _image;
        private PictureSizeMode _sizeMode;
        private Boolean _transparentBackground;

        public Boolean TransparentBackground
        {
            get { return _transparentBackground; }
            set { _transparentBackground = value; }
        }

        #endregion

        #region 构造方法

        public PictureElement()
        {
            this.Width = 64;
            this.Height = 64;
        }

        #endregion

        #region 属性

        /// <summary>
        ///  获取或设置控件使用的图片
        /// </summary>
        public Bitmap Image
        {
            get { return _image; }
            set 
            {
                _image = value;
                if (_sizeMode == PictureSizeMode.AutoSize && _image != null)
                {    
                    this.Width = _image.Width;
                    this.Height = _image.Height;
                }
            }
        }

        /// <summary>
        ///  控件将如何处理图片位置和控件大小
        /// </summary>
        public PictureSizeMode SizeMode
        {
            get 
            { 
                return _sizeMode; 
            }
            set 
            {
                _sizeMode = value;
                if (_sizeMode == PictureSizeMode.AutoSize && _image != null)
                {    
                    this.Width = _image.Width;
                    this.Height = _image.Height;
                }
            }
        }

        #endregion

        #region 重写UIElement的方法
        
        protected override void OnRender(Graphics graphics)
        {
            if (_image != null)
            {                        
                Rectangle DisplayingRectangle = Rectangle.Empty;
                Rectangle ImageRectangle = new Rectangle(0, 0, _image.Width, _image.Height);
                GraphicsUnit PixelUnit = GraphicsUnit.Pixel;
                switch (_sizeMode)
                {
                    case PictureSizeMode.AutoSize:
                        graphics.DrawImage(_image, this.Left, this.Top, this.ClientRectangle, PixelUnit);
                        break;

                    case PictureSizeMode.CenterImage:
                        if (_image.Width > this.Width)
                        {
                            DisplayingRectangle.X = this.Left;
                            DisplayingRectangle.Width = this.Width;
                        }
                        else
                        {
                            DisplayingRectangle.X = this.Left + (this.Width - _image.Width) / 2;
                            DisplayingRectangle.Width = _image.Width;
                        }

                        if (_image.Height > this.Height)
                        {
                            DisplayingRectangle.Y = this.Top;
                            DisplayingRectangle.Height = this.Height;
                        }
                        else
                        {
                            DisplayingRectangle.Y = this.Top + (this.Height - _image.Height) /2;
                            DisplayingRectangle.Height = _image.Height;
                        }

                        graphics.DrawImage(_image, DisplayingRectangle, ImageRectangle, PixelUnit);
                        break;

                    case PictureSizeMode.Normal:
                        graphics.DrawImage(_image, this.Left, this.Top, this.ClientRectangle, PixelUnit);
                        break;

                    case PictureSizeMode.Stretch:
                        graphics.DrawImage(_image, this.Bounds, ImageRectangle, PixelUnit);
                        break;

                    case PictureSizeMode.Zoom: 
                        int ImageScale = _image.Width / _image.Height;
                        if (ImageScale > 0)   // 宽大于高, 缩放以宽为主
                        {
                            DisplayingRectangle.Width = this.Width;
                            DisplayingRectangle.Height = (int)Math.Ceiling((Convert.ToDouble(this.Width) / ImageScale));
                        }
                        else
                        {
                            DisplayingRectangle.Height = this.Height;
                            DisplayingRectangle.Width = (int)Math.Ceiling((Convert.ToDouble(this.Height) / ImageScale));
                        }                        
                        DisplayingRectangle.X = (this.Width - DisplayingRectangle.Width ) / 2;
                        DisplayingRectangle.Y = (this.Height - DisplayingRectangle.Height ) / 2;    
                    
                        graphics.DrawImage(_image, DisplayingRectangle, ImageRectangle, PixelUnit);
                        break;

                    default:
                        break;
                }
            }
        }
        
        #endregion
    }
}
