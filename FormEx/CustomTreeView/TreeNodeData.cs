using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms
{

    public class TreeNodeData : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged 实现


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region 构造

        public TreeNodeData()
        {
            Text = "";
            Children = new List<TreeNodeData>();
        }

        public TreeNodeData(string? text)
        {
            Text = text;
            Children = new List<TreeNodeData>();
        }


        #endregion

        #region 字段

        private string _text;
        private object _value;
        private bool _isSelected;
        private List<TreeNodeData> _children;

        #endregion

        #region 属性

        public string Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }

        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }


        public List<TreeNodeData> Children
        {
            get { return _children; }
            set
            {
                if (_children != value)
                {
                    _children = value;
                    OnPropertyChanged(nameof(Children));
                }
            }
        }

        #endregion

    }
}
