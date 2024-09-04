using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Utils; 

namespace System.Windows.Forms.Canvas
{
    /// <summary>
    /// 表示CanvasElement对象的集合
    /// </summary>
    public sealed class CanvasElementCollection : IEnumerable<CanvasObject>
    {
        #region 委任和事件

        /// <summary>
        ///  在将WidgetBeing加入到集合时设置WidgetBeing的事件或属性
        /// </summary>
        public event Action<CanvasObject> Added;


        public event Action<CanvasObject> Removed;

        #endregion

        #region 字段


        private object _lockObject = new object();

        // 自身成员的名称和索引对照表
        private Dictionary<string, CanvasObject> _memberTable;

        public int Count
        {
            get
            {
                if (_memberTable == null)
                {
                    return 0;
                }
                return _memberTable.Count;
            }
        }



        public bool IsReadOnly
        {
            get;
            set;
        }

        public CanvasObject this[int index] {
            get
            {
                if (_memberTable.Count > index)
                {
                    return _memberTable.Skip(index).First().Value;
                }

                return null;
            }
            set
            {
                if (_memberTable.Count > index)
                {
                    KeyValuePair<string, CanvasObject> keyPairTarget = _memberTable.Skip(index).First();
                    _memberTable[keyPairTarget.Key] = value;
                }
            }
        }




        #endregion

        #region 索引器

        /// <summary>
        ///  获取指定名称的成员.
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public CanvasObject this[string memberName]
        {
            get
            {
                if (_memberTable.ContainsKey(memberName))
                {
                    return _memberTable[memberName];
                }

                return null;
            }
        }

        #endregion

        #region 属性

        CustomCanvas Owner
        {
            get;
            set;
        }

        #endregion

        #region 构造

        /// <summary>
        ///  创建一个WidgetBeing集合
        /// </summary>
        public CanvasElementCollection(CustomCanvas owner) 
        {
            Owner = owner;
            this._memberTable = new Dictionary<string, CanvasObject>();
        }

        #endregion

        #region 方法

        public bool Contains(string memberName)
        {
            return _memberTable.ContainsKey(memberName);
        }


        /// <summary>
        ///  添加WidgetBeing成员
        /// </summary>
        /// <param name="element"></param>
        public void Add(CanvasObject element)
        {
            lock (_lockObject)
            {
                if (!Contains(element))
                {
                    _memberTable.Add(element.Name, element);
                    element.Canvas = Owner;
                    element.OnParentChanged();

                    Added?.Invoke(element);
                }
            }
        }

        public bool Remove(CanvasObject element)
        {
            lock (_lockObject)
            {
                if (Contains(element))
                {
                    try
                    {
                        _memberTable.Remove(element.Name);
                        element.Canvas = null;
                        element.OnParentChanged();

                        return true;
                    }
                    finally
                    {
                        Removed?.Invoke(element);
                    }
                }

                return false;
            }
        }

        public int IndexOf(CanvasObject item)
        {
            int index = 0;
            foreach (var pair in _memberTable)
            {
                if (pair.Value == item)
                {
                    return index;
                }

                index++;
            }

            return -1;
        }


        public void Clear()
        {
            lock (_lockObject)
            {
                foreach (var key in _memberTable.Keys.ToArray())
                {
                    var item = _memberTable[key];
                    _memberTable.Remove(key);
                    Removed?.Invoke(item);
                }
            }
        }

        public bool Contains(CanvasObject item)
        {
            lock (_lockObject)
            {
                if (item.Name.IsEmpty())
                {
                    item.Name = Guid.NewGuid().ToString(); 
                }
                return _memberTable.ContainsKey(item.Name);
            }
        }

        public void CopyTo(CanvasObject[] array, int arrayIndex)
        {

            var source = _memberTable.Values.ToArray();

            Array.Copy(source, 0, array, arrayIndex, source.Length);
        }



        public IEnumerator<CanvasObject> GetEnumerator()
        {
            var values = _memberTable.Values;
            var list = values.ToList();
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _memberTable.Values.GetEnumerator();
        }


        #endregion


    }

}
