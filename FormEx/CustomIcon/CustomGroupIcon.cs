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
        #region 分组功能

        /// <summary>
        /// 激活后的背景色
        /// </summary>
        public enum SelectionStyles
        {
            AccentBar,
            PureColor
        }

        static Dictionary<string, List<CustomGroupIcon>> groupCaches = new Dictionary<string, List<CustomGroupIcon>>();


        private string _groupName;


        [Category("Group")]
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

        [Category("Group")]
        public SelectionStyles SelectionStyle { get; set; } = SelectionStyles.AccentBar;

        /// <summary>
        /// 构造
        /// </summary>
        public CustomGroupIcon() : base()
        {
            this.KeepSelected = true;
            this.Padding = new Padding(base.Padding.Left + 10, base.Padding.Top, base.Padding.Right, base.Padding.Bottom);
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.GroupName))
            {

                foreach (var item in groupCaches[this.GroupName])
                {
                    if (item.IsSelected && item == this)
                    {
                        return;
                    }
                }


                foreach (var item in groupCaches[this.GroupName])
                {
                    if (!item.Equals(this))
                    {
                        item.IsSelected = false;
                    }
                }

                base.OnMouseDown(e);

            }
            else
            {
                base.OnMouseDown(e);
            }
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


        #endregion
    }



}


