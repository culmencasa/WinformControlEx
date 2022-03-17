using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    public class CustomGroupIcon : TileIcon
    {
        #region 分组功能

        static Dictionary<string, List<CustomGroupIcon>> groupCaches = new Dictionary<string, List<CustomGroupIcon>>();


        private string _groupName;
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

                if (string.IsNullOrEmpty(newValue))
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

        

        public CustomGroupIcon() : base()
        {
            this.Padding = new Padding(base.Padding.Left + 10, base.Padding.Top, base.Padding.Right, base.Padding.Bottom);
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            foreach (var item in groupCaches[this.GroupName])
            {
                if (item.IsSelected && item == this)
                {
                    return;
                }
            }

            base.OnMouseDown(e);
            

            foreach (var item in groupCaches[this.GroupName])
            {
                if (!item.Equals(this))
                {
                    item.IsSelected = false;
                }
            }

        }

        protected override void DrawSelectedBackground(Graphics g)
        {
            if (KeepSelected && IsSelected)
            {
                using (SolidBrush brush = new SolidBrush(SelectedBackColor))
                {
                    g.FillRectangle(brush, new Rectangle(0,0, 10, this.Height));
                }
            }
        }

        #endregion


    }

}
