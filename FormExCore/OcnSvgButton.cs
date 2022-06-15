using Svg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormExCore
{
    [DefaultEvent("Click")]
    public class OcnSvgButton : PictureBox
    {
        #region 字段

        private string _sourcePath = string.Empty;
        private string _fullSourcePath = string.Empty;
        private string _sourceName = string.Empty;
        private bool _useSourcePath = true;

        private Color _hoverColor = Color.Empty;
        private Color _normalColor = Color.Empty;

        private Image? _normalCache = null;
        private Image? _hoverCache = null;

        #endregion

        #region 属性

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

        
        public string SourcePath
        {
            get
            {
                return _sourcePath;
            }
            set
            {
                _sourcePath = value;
                _fullSourcePath = value;
                RenderNormalOnce();

            }
        }

        public string SourceName
        {
            get
            {
                return _sourceName;
            }
            set
            {
                _sourceName = value;
                RenderNormalOnce();
            }
        }

        public Color NormalColor
        {
            get
            {
                return _normalColor;
            }
            set
            {
                _normalColor = value;
                _normalCache = null;
                RenderNormalOnce();
            }
        }

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



        [Browsable(false)]
        public new PictureBoxSizeMode SizeMode
        {
            get
            {
                return base.SizeMode;
            }
            set
            {
                base.SizeMode = value;
            }
        }

        #endregion

        #region 构造

        public OcnSvgButton()
        {
            this.SizeMode = PictureBoxSizeMode.AutoSize;
            this.MouseEnter += OcnSvgButton_MouseEnter;
            this.MouseLeave += OcnSvgButton_MouseLeave;
            this.Resize += OcnSvgButton_Resize;
        }

        private void OcnSvgButton_Resize(object? sender, EventArgs e)
        {
            _normalCache = null;
            _hoverCache = null;
            Image?.Dispose();
        }

        public OcnSvgButton(string sourcePath) : this()
        {
            SourcePath = sourcePath;
        }


        #endregion

        #region 方法

        private SvgDocument CreateSvgDocument()
        {
            SvgDocument svgDoc = new SvgDocument();
            if (UseSourcePath)
            {
                svgDoc = SvgDocument.Open(_fullSourcePath);
            }
            else
            {
                byte[]? closeButtonBytes = Properties.Resources.ResourceManager.GetObject(SourceName) as byte[];
                svgDoc = SvgDocument.Open<SvgDocument>(new MemoryStream(closeButtonBytes));
            }

            return svgDoc;
        }


        private Image? Render(Color newColor)
        {
            if (CheckSource())
            {
                var svgDoc = CreateSvgDocument();
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

        private bool CheckSource()
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


        private void RenderNormalOnce()
        {
            if (_normalCache == null)
            {
                Image = Render(NormalColor);
                _normalCache = Image;
            }
            else
            {
                Image = _normalCache;
            }
        }

        private void RenderHoverOnce()
        {
            if (_hoverCache == null)
            {
                Image = Render(HoverColor);
                _hoverCache = Image;
            }
            else
            {
                Image = _hoverCache;
            }
        }

        private void SetInitialSize(SvgDocument document)
        {
            if (document.Height > this.Height)
            {
                document.Width = (int)((document.Width / (double)document.Height) * this.Height);
                document.Height = this.Height;
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
            }
        }




        #endregion


        private void OcnSvgButton_MouseLeave(object? sender, EventArgs e)
        {
            RenderNormalOnce();
        }

        private void OcnSvgButton_MouseEnter(object? sender, EventArgs e)
        {
            RenderHoverOnce();
        }


    }
}
