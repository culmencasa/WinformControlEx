using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    public class CustomGroupIcon : TileIcon
    {
        /// <summary>
        /// 构造
        /// </summary>
        public CustomGroupIcon() : base()
        {
            this.KeepSelected = true;
            this.Padding = new Padding(base.Padding.Left + 10, base.Padding.Top, base.Padding.Right, base.Padding.Bottom);
        }


        #region 分组功能

        public event EventHandler<CustomGroupIconClickEventArgs> BeforeSingleClick;

        /// <summary>
        /// 激活后的背景色
        /// </summary>
        public enum SelectionStyles
        {
            AccentBar,
            PureColor
        }

        #region 静态成员

        static Dictionary<string, List<CustomGroupIcon>> groupCaches = new Dictionary<string, List<CustomGroupIcon>>();

        public static CustomGroupIcon GetGroupSelection(string groupName)
        {
            foreach (var item in groupCaches[groupName])
            {
                if (item.IsSelected)
                {
                    return item;
                }
            }

            return null;
        }

        public static void UpdateSelection(string groupName, CustomGroupIcon newSelection)
        {
            if (string.IsNullOrEmpty(groupName) || newSelection == null)
                return;

            // 取消组内所有项的选择状态
            foreach (var item in groupCaches[groupName])
            {
                item.IsSelected = false;
            }

            // 选中新项
            newSelection.IsSelected = true;
        }

        #endregion

        private string _groupName;
        private int _imageSize = 32;
        private SelectionStyles _selectionStyle = SelectionStyles.AccentBar;
        private CustomGroupIcon _previousSelection;
        private CustomGroupIcon _newSelection;


        /// <summary>
        /// 设置分组。同一个分组名只有一个选中项。
        /// </summary>
        [Category(Consts.DefaultCategory)]
        public string GroupName
        {
            get
            {
                return _groupName;
            }
            set
            {
                string newValue = value;
                string oldValue = _groupName;

                if (newValue == oldValue && string.IsNullOrEmpty(oldValue))
                {
                    _groupName = value;
                }
                else if (string.IsNullOrEmpty(newValue))
                {
                    // 如果新值为空, 则从缓存中删除旧值
                    if (groupCaches.ContainsKey(oldValue))
                    {
                        if (groupCaches[oldValue].Contains(this))
                        {
                            groupCaches[oldValue].Remove(this);
                        }
                    }
                }
                else
                {
                    // 修改旧值集合
                    if (!string.IsNullOrEmpty(oldValue) && oldValue != newValue)
                    {
                        if (groupCaches[oldValue].Contains(this))
                        {
                            groupCaches[oldValue].Remove(this);
                        }
                    }

                    // 添加到新值集合
                    if (!groupCaches.ContainsKey(newValue))
                    {
                        groupCaches.Add(newValue, new List<CustomGroupIcon>() { this });
                    }
                    else
                    {
                        if (!groupCaches[newValue].Contains(this))
                        {
                            groupCaches[newValue].Add(this);
                        }
                    }


                    _groupName = newValue;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Category(Consts.DefaultCategory)]
        public SelectionStyles SelectionStyle
        {
            get
            {
                return _selectionStyle;
            }
            set
            {
                _selectionStyle = value;
                OnSelectionStyleChanged();
            }
        }

        [Category(Consts.DefaultCategory)]
        public int ImageSize
        {
            get => _imageSize;
            set
            {
                _imageSize = value;
                OnImageSizeChanged();
            }
        }



        protected virtual void OnImageSizeChanged()
        {
            Invalidate();
        }
        protected virtual void OnSelectionStyleChanged()
        {
            Invalidate();
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.GroupName))
            {
                _previousSelection = GetGroupSelection(this.GroupName);
                _newSelection = this;

                UpdateSelection(this.GroupName, this);
            }
            else
            {
                base.OnMouseDown(e);
            }
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            var cancelableArgs = new CustomGroupIconClickEventArgs()
            {
                LastSelection = _previousSelection,
                NewSelection = _newSelection
            };
            BeforeSingleClick?.Invoke(this, cancelableArgs);


            if (cancelableArgs.Cancel)
            {
                UpdateSelection(this.GroupName, _previousSelection);

                return;
            }

            base.OnMouseClick(e);
        }


        protected override void DrawSelectedBackground(Graphics g)
        {
            if (SelectionStyle == SelectionStyles.AccentBar)
            {
                if (KeepSelected && IsSelected)
                {
                    if (_isHovering)
                    {
                        g.Clear(HoverBackColor);
                    }
                    else
                    {
                        g.Clear(SelectedBackColor);
                    }
                    Color accentColor = ControlPaint.Dark(SelectedBackColor, 70);
                    using (SolidBrush brush = new SolidBrush(accentColor))
                    {
                        g.FillRectangle(brush, new Rectangle(0, 0, 8, this.Height));
                    }
                }
            }
            else
            {
                base.DrawSelectedBackground(g);
            }

        }

        protected override Rectangle GetImageArea()
        {
            int x, y, width, height;
            width = ImageSize;
            height = ImageSize;
            x = 20;
            y = (this.Height - height) / 2;


            return new Rectangle(x, y, width, height);
        }

        #endregion
    }



}


