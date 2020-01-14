using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace WinformEx
{
    /// <summary>
    ///  包含UIElement控件索引的集合类.
    /// </summary>
    public sealed class UIElementCollection : List<UIElement>
    {
        #region 委任和事件

        public delegate void SetMemeberEventHandler(UIElement obj);
        /// <summary>
        ///  在将UIElement加入到集合时设置UIElement的事件或属性
        /// </summary>
        public event SetMemeberEventHandler SetMember;

        #endregion

        #region 字段

        // 自身成员的名称和索引对照表
        private Dictionary<string, int> _memberTable;

        #endregion
        
        #region 索引器

        /// <summary>
        ///  获取指定名称的成员.
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public UIElement this[ string memberName ]
        {
            get
            {
                return this[ _memberTable[ memberName ] ];                
            }
        }

        #endregion

        #region 构造

        /// <summary>
        ///  创建一个UIElment集合
        /// </summary>
        public UIElementCollection()
        {
            this._memberTable = new Dictionary<string, int>();
        }

        #endregion

        #region 方法

        /// <summary>
        ///  添加UIElement成员
        /// </summary>
        /// <param name="element"></param>
        public new void Add(UIElement element)
        {
            if (element.Name != "")
            {
                _memberTable.Add(element.Name, this.Count);
            }
            else
            {
                _memberTable.Add("elementName" + this.Count.ToString(), this.Count);
            }
            base.Add(element);

            if (SetMember != null)
            {
                SetMember( element );
            }
        }

        #endregion


    }

}
