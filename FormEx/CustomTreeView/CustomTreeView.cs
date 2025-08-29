using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    /// <summary>
    /// 实现了部分属性MVVM
    /// </summary>
    public class CustomTreeView : TreeView, INotifyPropertyChanged
    {

        #region INotifyPropertyChanged 实现


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region 字段

        private TreeNodeData _dataSource;
        private Dictionary<TreeNodeData, TreeNode> _flatHierachyNodeCache = new();
        private bool _touchMode;

        #endregion


        #region 构造

        public CustomTreeView()
        {
            PropertyChanged += OnDataSourceChanged;

        }





        private void TreeView1_MouseDown(object? sender, MouseEventArgs e)
        {
            TreeNode clickedNode = GetNodeAt(e.X, e.Y);
            if (clickedNode != null)
            {
                // 调用两次确保生效
                SelectedNode = clickedNode;
                SelectedNode = clickedNode;
            }
        }
         

        #endregion

        #region 通知属性

        /// <summary>
        /// 绑定的数据源
        /// </summary>
        [Browsable(true)]
        [Category("Custom")]
        public TreeNodeData? DataSource
        {
            get { return _dataSource; }
            set
            {
                UnsubscribeDataSource(_dataSource);
                if (value != null)
                {
                    SubscribeToDataSource(value);
                }

                _dataSource = value; 
                OnPropertyChanged(nameof(DataSource));
            }
        }

        /// <summary>
        /// 选择项
        /// </summary>
        [Category("Custom")]
        public new TreeNode SelectedNode
        {
            get
            {
                return base.SelectedNode;
            }
            set
            {
                if (base.SelectedNode != value)
                {
                    base.SelectedNode = value;
                    UpdateIsSelected(value);
                }
            }
        }


        [Category("Custom")]
        public bool TouchMode
        {
            get
            {
                return _touchMode;
            }
            set
            {
                _touchMode = value;
                OnTouchModeChanged();
            }
        }

        private void OnTouchModeChanged()
        {
            if (_touchMode)
            {
                DrawMode = TreeViewDrawMode.OwnerDrawAll;
                ItemHeight = 60;
                DrawNode += OnDrawNode; 
                MouseDown += TreeView1_MouseDown;
            }
            else
            {
                DrawMode = TreeViewDrawMode.Normal;
                ItemHeight = 30;
                DrawNode -= OnDrawNode;
                MouseDown -= TreeView1_MouseDown;
            }
        }


        #endregion

        #region 私有方法

        private void ClearNodes()
        {
            foreach (var cacheItem in _flatHierachyNodeCache)
            {
                UnsubscribeDataSource(cacheItem.Key);
            }

            _flatHierachyNodeCache.Clear();
            Nodes.Clear();
        }

        private void RefreshDataSource()
        {
            ClearNodes();

            if (_dataSource != null)
            {
                AddNodes(Nodes, _dataSource);

                if (Nodes.Count > 0)
                {
                    this.ExpandAll();
                }
            }
        }
        /// <summary>
        /// 添加根节点及其子节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="data"></param>
        private void AddNodes(TreeNodeCollection nodes, object data)
        {
            if (data is TreeNodeData rootNodeData)
            {
                TreeNode rootNode = new TreeNode(rootNodeData.Text);
                rootNode.ImageIndex = 0;
                rootNode.SelectedImageIndex = 0;

                nodes.Add(rootNode);

                _flatHierachyNodeCache.Add(rootNodeData, rootNode);

                AddChildNodes(rootNode.Nodes, rootNodeData.Children, 1);
            }
        }
        /// <summary>
        /// 添加子节点及其子节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="items"></param>
        /// <param name="level"></param>
        private void AddChildNodes(TreeNodeCollection nodes, IEnumerable<TreeNodeData> items, int level)
        {
            foreach (var item in items)
            {
                TreeNode node = new TreeNode(item.Text);
                node.ImageIndex = level;
                node.SelectedImageIndex = level;
                nodes.Add(node);

                _flatHierachyNodeCache.Add(item, node);

                if (item.Children != null && item.Children.Any())
                {
                    AddChildNodes(node.Nodes, item.Children, level + 1);
                }
            }
        }


        /// <summary>
        /// 更新选择项
        /// </summary>
        /// <param name="value"></param>
        private void UpdateIsSelected(TreeNode value)
        {
            foreach (KeyValuePair<TreeNodeData, TreeNode> pair in _flatHierachyNodeCache)
            {
                if (pair.Value == value)
                {
                    pair.Key.IsSelected = true;
                }
                else
                {
                    pair.Key.IsSelected = false;

                }
            }
        }

        #endregion

        public TreeNodeData GetNodeData(TreeNode node)
        {
            foreach (var item in _flatHierachyNodeCache)
            {
                if (item.Value == node)
                {
                    return item.Key;
                }
            }

            return null;
        }

        #region 订阅DataSource属性通知

        /// <summary>
        /// 订阅通知    
        /// </summary>
        /// <param name="source"></param>
        private void SubscribeToDataSource(TreeNodeData source)
        {
            if (source != null)
            {
                source.PropertyChanged += OnDataSourcePropertyChanged;
                if (source.Children != null)
                {
                    foreach (var item in source.Children)
                    {
                        SubscribeToDataSource(item);
                    }
                }
            }
        }

        /// <summary>
        /// 取消订阅通知
        /// </summary>
        /// <param name="source"></param>
        private void UnsubscribeDataSource(TreeNodeData source)
        {
            if (source != null)
            {
                source.PropertyChanged -= OnDataSourcePropertyChanged; 
                if (source.Children != null)
                {
                    foreach (var item in source.Children)
                    {
                        UnsubscribeDataSource(item);
                    }
                }
            }
        }

        /// <summary>
        /// DataSource属性变化时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDataSourceChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DataSource")
            {
                RefreshDataSource();
            }
        }

        /// <summary>
        /// DataSource子项属性变化时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDataSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        { 
            if (e.PropertyName == "IsSelected")
            {
                var nodeData = sender as TreeNodeData;

                if (_flatHierachyNodeCache.ContainsKey(nodeData))
                {
                    base.SelectedNode = _flatHierachyNodeCache[nodeData];
                }
            }
        }

        #endregion

        #region 隐藏横向滚动条

        [DllImport("user32.dll")]
        private static extern bool ShowScrollBar(IntPtr hWnd, int nBar, bool bShow);

        const int SB_HORZ = 0; // 水平滚动条的标识  

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            HideHorizontalScrollBar();
        }

        private void HideHorizontalScrollBar()
        {
            ShowScrollBar(this.Handle, SB_HORZ, false); // 隐藏横向滚动条  
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            HideHorizontalScrollBar(); // 调整大小时再次隐藏滚动条  
        }

        #endregion

        #region 始终保持选中状态


        private void OnDrawNode(object? sender, DrawTreeNodeEventArgs e)
        {
            var treeView = (TreeView)sender;
            //if ((e.State & TreeNodeStates.Selected) != 0)
            if (e.Node.IsSelected)
            {
                using (Brush backBrush = new SolidBrush(SystemColors.Highlight))
                {
                    e.Graphics.FillRectangle(backBrush, e.Bounds);
                }

                e.Node.BackColor = SystemColors.Highlight;
                e.Node.ForeColor = SystemColors.HighlightText;

                // 使用原生控件的节点加减号图标和虚线图标
                e.DrawDefault = true;
            }
            else
            {
                e.Node.BackColor = treeView.BackColor;
                e.Node.ForeColor = treeView.ForeColor;
                e.DrawDefault = true;
            }
        }


        #endregion
    }
}
