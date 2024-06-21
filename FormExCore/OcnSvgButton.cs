using Svg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FormExCore
{
    [DefaultEvent("Click")]
    public class OcnSvgButton : Control, ISupportInitialize
    {

        #region 字段

        string _sourcePath = string.Empty;
        string _fullSourcePath = string.Empty;
        string _sourceName = string.Empty;
        bool _useSourcePath = true;
        bool _initilized = false;

        Color _hoverColor = Color.Empty;
        Color _normalColor = Color.Empty;
        Color _backColor = Color.Empty;
        Color _hoverBackColor = Color.Empty;

        Image? _normalCache = null;
        Image? _hoverCache = null;
        Image? _image = null;

        PictureBoxSizeMode _sizeMode = PictureBoxSizeMode.Zoom;


        #endregion

        #region 属性

        [Category("Custom")]
        [Description("是否使用全路径的方式加载SVG文件")]
        public bool UseSourcePath
        {
            get
            {
                return _useSourcePath;
            }
            set
            {
                _useSourcePath = value;

                RenderNormalOnce();
            }
        }


        [Category("Custom")]
        [Description(@"类似 YourNamespace.Properties.Resources.YourSvgResourceName 的路径形式")]
        public string SourcePath
        {
            get
            {
                return _sourcePath;
            }
            set
            {
                bool changing = _sourcePath != value;
                _sourcePath = value;
                _fullSourcePath = value;
                if (changing)
                {
                    _normalCache = null;
                }
                RenderNormalOnce();

            }
        }

        [Category("Custom")]
        [Description("SVG文件在资源中的名称")]
        public string SourceName
        {
            get
            {
                return _sourceName;
            }
            set
            {
                bool changing = _sourceName != value;
                _sourceName = value;
                _useSourcePath = false;
                if (changing)
                {
                    _normalCache = null;
                }
                RenderNormalOnce();
            }
        }

        [Category("Custom")]
        public Color NormalColor
        {
            get
            {
                return _normalColor;
            }
            set
            {
                _normalColor = value;

                Redraw();
            }
        }

        [Category("Custom")]
        public Color HoverColor
        {
            get
            {
                return _hoverColor;
            }
            set
            {
                _hoverColor = value;
                _hoverCache = null;
            }
        }

        [Category("Custom")]
        public Color HoverBackColor
        {
            get
            {
                return _hoverBackColor;
            }
            set
            {
                _hoverBackColor = value;
            }
        }

        [Category("Custom")]
        protected Image RenderedImage
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
            }

        }


        [Category("Custom")]
        public PictureBoxSizeMode SizeMode
        {
            get
            {
                return _sizeMode;
            }
            set
            {
                bool changing = _sizeMode != value;
                _sizeMode = value;
                if (changing)
                {
                    Redraw();
                }
            }
        }



        /// <summary>
        /// 控件的实际区域（滚动容器中）
        /// </summary>
        public Rectangle ActualBounds
        {
            get
            {
                Rectangle actualBounds = this.Bounds;

                // 如果是在可滚动的容器内， 计算控件实际偏移后的位置
                if (this.Parent as ScrollableControl != null)
                {
                    ScrollableControl? parent = this.Parent as ScrollableControl;
                    if (parent != null)
                    {
                        actualBounds = new Rectangle(
                            this.Bounds.X - parent.HorizontalScroll.Value,
                            this.Bounds.Y - parent.VerticalScroll.Value,
                            this.Bounds.Width,
                            this.Bounds.Height);
                    }
                }

                return actualBounds;
            }
        }

        #endregion

        #region 构造

        public OcnSvgButton()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);



            this.Size = new Size(32, 32);
            this.MouseEnter += OcnSvgButton_MouseEnter;
            this.MouseLeave += OcnSvgButton_MouseLeave;
            this.Resize += OcnSvgButton_Resize;
            this.BackColor = Color.Transparent;

        }

        private void OcnSvgButton_Resize(object? sender, EventArgs e)
        {
            _normalCache = null;
            _hoverCache = null;
            RenderedImage?.Dispose();
        }

        public OcnSvgButton(string sourcePath) : this()
        {
            SourcePath = sourcePath;
        }


        #endregion

        #region 公开的方法


        public void BeginInit()
        {
            _initilized = false;
        }

        public void EndInit()
        {
            _initilized = true;

            Redraw();
        }

        #endregion

        #region 私有的方法

        private SvgDocument CreateSvgDocument()
        {
            SvgDocument svgDoc = new SvgDocument();
            if (UseSourcePath)
            {
                svgDoc = SvgDocument.Open(_fullSourcePath);
            }
            else
            {
                // 增加修改资源需要重新生成
                try
                {
                    Assembly assembly = Assembly.GetEntryAssembly();
                    // 如果设计器在单独的进程中运行，则无法获取当前程序集
                    if (assembly.GetName().Name == "DesignToolsServer")
                    {
                        return null;
                    }

                    System.Resources.ResourceManager rm = new System.Resources.ResourceManager($"{assembly.GetName().Name}.Properties.Resources", assembly);

                    byte[]? imageBytes = rm.GetObject(SourceName) as byte[];
                    svgDoc = SvgDocument.Open<SvgDocument>(new MemoryStream(imageBytes));
                }
                catch
                {
                }
            }

            return svgDoc;
        }


        private Image? Render(Color newColor)
        {
            if (CheckSourceProperties())
            {
                var svgDoc = CreateSvgDocument();
                if (svgDoc != null)
                {
                    SetInitialSize(svgDoc);

                    if (newColor != Color.Empty)
                    {
                        ChangeColor(svgDoc.Children, newColor);
                    }

                    return svgDoc.Draw();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 检查svg来源属性是否设置好
        /// </summary>
        /// <returns></returns>
        private bool CheckSourceProperties()
        {
            if (UseSourcePath)
            {
                if (string.IsNullOrEmpty(_sourcePath))
                {
                    return false;
                }

                _fullSourcePath = ConvertToFullPath();
                if (!File.Exists(_fullSourcePath))
                {
                    return false;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(SourceName))
                {
                    return false;
                }
            }


            return true;
        }

        private string ConvertToFullPath()
        {
            string targetFile = _sourcePath;
            if (!Path.IsPathRooted(_sourcePath))
            {
                // AppDomain.CurrentDomain.BaseDirectory和Application.StartupPath在设计时地址都不对
                // 设计时要显示只能用序列化的SVG作为源, 或者用绝对路径

                targetFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _sourcePath);
            }

            return targetFile;
        }

        private void Redraw()
        {
            if (!_initilized)
                return;

            RenderedImage = Render(NormalColor);
            _normalCache = RenderedImage;
            Invalidate();
        }

        private void RenderNormalOnce()
        {
            if (!_initilized)
                return;

            if (_normalCache == null)
            {
                RenderedImage = Render(NormalColor);
                _normalCache = RenderedImage;
            }
            else
            {
                RenderedImage = _normalCache;
            }

            Invalidate();
        }

        private void RenderHoverOnce()
        {
            if (_hoverCache == null)
            {
                RenderedImage = Render(HoverColor);
                _hoverCache = RenderedImage;
            }
            else
            {
                RenderedImage = _hoverCache;
            }
            Invalidate();
        }

        private void SetInitialSize(SvgDocument document)
        {
            if (SizeMode == PictureBoxSizeMode.Zoom)
            {
                double ratio = document.Width / (double)document.Height;

                int shrinkHeight = this.Height - Padding.Top - Padding.Bottom;
                if (shrinkHeight <= 0)
                {
                    shrinkHeight = this.Height - 4;
                }
                document.Height = shrinkHeight;
                document.Width = (int)(ratio * document.Height);
            }
            //return document;
        }


        private void ChangeColor(IList<SvgElement> children, Color newColor)
        {
            foreach (SvgElement item in children)
            {
                if (item is SvgPath)
                {
                    SvgPath itemPath = (SvgPath)item;
                    SvgColourServer itemColor = (SvgColourServer)itemPath.Fill;
                    if (itemColor.Colour.ToArgb() != newColor.ToArgb())
                    {
                        itemPath.Fill = new SvgColourServer(newColor);
                    }
                }
                else if (item is SvgGroup)
                {
                    ChangeColor(item.Children, newColor);
                }
                else if (item is SvgElement)
                {
                    ChangeColor(item.Children, newColor);
                }
            }
        }



        #endregion

        #region 事件处理

        private void OcnSvgButton_MouseLeave(object? sender, EventArgs e)
        {
            RenderNormalOnce();

            if (_backColor != Color.Empty)
            {
                BackColor = _backColor;
                Invalidate();
            }

        }

        private void OcnSvgButton_MouseEnter(object? sender, EventArgs e)
        {
            RenderHoverOnce();

            if (HoverBackColor != Color.Empty)
            {
                // 备份
                if (_backColor == Color.Empty)
                {
                    _backColor = BackColor;
                }

                this.BackColor = HoverBackColor;
                Invalidate();

            }
        }

        #endregion

        #region 重写的方法


        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            Redraw();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;


            if (this.RenderedImage != null)
            {
                switch (SizeMode)
                {
                    case PictureBoxSizeMode.AutoSize:
                    case PictureBoxSizeMode.Normal:
                        g.DrawImage(this.RenderedImage, 0, 0);
                        break;
                    case PictureBoxSizeMode.CenterImage:
                        g.DrawImage(this.RenderedImage, (this.Width - RenderedImage.Width) / 2, (Height - RenderedImage.Height) / 2);
                        break;
                    case PictureBoxSizeMode.StretchImage:
                        g.DrawImage(this.RenderedImage, 0, 0, this.Width, this.Height);
                        break;
                    case PictureBoxSizeMode.Zoom:
                        float widthRatio = (float)this.Width / this.RenderedImage.Width;
                        float heightRatio = (float)this.Height / this.RenderedImage.Height;
                        float scaleFactor = Math.Min(widthRatio, heightRatio);
                        int newWidth = (int)(this.RenderedImage.Width * scaleFactor);
                        int newHeight = (int)(this.RenderedImage.Height * scaleFactor);
                        g.DrawImage(this.RenderedImage, (this.Width - newWidth) / 2, (this.Height - newHeight) / 2, newWidth, newHeight);
                        break;
                }

            }

            base.OnPaint(pe);
        }

        #endregion

    }
}
