using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace System.Windows.Forms.Canvas
{
    public class CustomCanvas : UserControl
    {
        #region 枚举

        /// <summary>
        /// 鼠标行为意图
        /// </summary>
        public enum MouseIntents
        {
            /// <summary>
            /// 无意图
            /// </summary>
            None,
            /// <summary>
            /// 左键点击空白画布
            /// </summary>
            EmptySpaceLeft,

            /// <summary>
            /// 平移画布
            /// </summary>
            PanCanvas,
            /// <summary>
            /// 单选模式
            /// </summary>
            SelectObject,
            /// <summary>
            /// 框选模式
            /// </summary>
            SelectScope,
            /// <summary>
            /// 移动元素
            /// </summary>
            MoveObject,
            /// <summary>
            /// 移动框选的元素
            /// </summary>
            MoveScope,
            /// <summary>
            /// 激活元素。默认是双击。
            /// </summary>
            ActiveObject,
            /// <summary>
            /// 元素菜单
            /// </summary>
            ObjectContext,
            /// <summary>
            /// 画布菜单.通常是右键点击空白区域
            /// </summary>
            CanvasContext,
            /// <summary>
            /// 悬浮在元素上
            /// </summary>
            HoverObject,
            HoverGrip,
        }

        #endregion

        #region 事件

        public event EventHandler Loaded;
        public event Action<CustomCanvas, CanvasObject> ObjectDoubleClick;
        public event Action<CustomCanvas, CanvasObject> ObjectRightClick;

        public event Action<List<CanvasObject>, List<PointF>> ObjectMoved;

        public event Action<PointF, float> CanvasPanned;
        public event Action<CustomCanvas, List<CanvasObject>> SelectionChanged;
        public event MouseEventHandler CanvasContextMenuRequired;
         

        #endregion

        #region 字段

        private float canvasWidth = 3000f; // 画布宽度
        private float canvasHeight = 3000f; // 画布高度


        // 元素列表
        private CanvasElementCollection controls;

        // 移动元素列表
        private List<CanvasObject> movingObjects = new List<CanvasObject>();
        // 移动元素的位置
        List<PointF> movingPositions = new List<PointF>();

        private RectangleF selectionBox;

        private PointF lastLeftMousePosition;
        private PointF lastRightMousePosition;
        private PointF leftMouseDownPosition;
        private PointF rightMouseDownPosition;


        private float zoom = 1.0f;
        private PointF offset = new PointF(0, 0);
        private ToolTip toolTip = new ToolTip();
        private const int SelectionThreshold = 2; // 拖动阈值
        private MouseIntents mouseActionIntent = MouseIntents.None;

        private PointF logicalCenter;
        private bool suspendPainting = false;


        private Cursor handGripCursor;

        SelectGrip selectGrip;
        PanTool panTool;


        #endregion

        #region 属性

        IDpiDefined DpiParent { get; set; }

        public float Zoom
        {
            get
            {
                return zoom;
            }
        }

        public PointF CanvasCenterPoint
        {
            get
            {
                return logicalCenter;
            }
        }


        public PointF Offset
        {
            get
            {
                return offset;
            }
        }


        public new CanvasElementCollection Controls
        {
            get
            {
                return controls;
            }
            private set
            {
                controls = value;
            }
        }

        public List<CanvasObject> SelectedControl
        {
            get
            {
                return controls?.Where(p => p.IsSelected).ToList();
            }
        }


        /// <summary>
        /// 是否处于编辑模式。默认是true。
        /// </summary>
        public bool IsEditMode { get; set; } = true;

        public bool AllowZoom { get; set; } = true;

        public bool AllowPan { get; set; } = true;

        public bool AllowSelect { get; set; } = true;

        public bool AllowMove { get; set; } = true;

        /// <summary>
        /// 以画布的中心为缩放中心。默认是false。
        /// </summary>
        public bool ZoomInCenter { get; set; }

        /// <summary>
        /// 指针在画布上的坐标
        /// </summary>
        public PointF PointerPosition { get; private set; }

        #endregion

        #region 构造函数

        public CustomCanvas()
        {
            this.DoubleBuffered = true;

            InitializeCenterPoint();
            Controls = new CanvasElementCollection(this);
            Load += CanvasLite_Load;

            selectGrip = new SelectGrip(this);
            panTool = new PanTool(this);
        }

        private void CanvasLite_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                DpiParent = this.ParentForm as IDpiDefined;
                handGripCursor = ImageTool.LoadCustomCursor("Resources\\CustomCanvas\\grip-hand.cur");
            }
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 恢复画布回到中心点位置
        /// </summary>
        public void ResetOffset()
        {
            // 如果设置为以画布中心位置缩放, 则回到中心位置
            if (ZoomInCenter)
            {
                // 计算画布的中心位置
                float centerX = (canvasWidth / 2) * zoom; // 画布中心
                float centerY = (canvasHeight / 2) * zoom; // 画布中心

                // 设置画布偏移
                offset = new PointF((this.Width / 2) - centerX, (this.Height / 2) - centerY);
            }
            else
            {
                offset = new PointF(0, 0);
            }


            Invalidate(); // 重新绘制画布
        }

        public void InitializeCenterPoint()
        {
            logicalCenter = new PointF((canvasWidth / 2) * zoom, (canvasHeight / 2) * zoom); // 使用画布大小
        }

        public void SuspendRender()
        {
            suspendPainting = true;
        }

        public void ResumeRender()
        {
            if (suspendPainting)
            {
                suspendPainting = false;

                Invalidate();
            }
        }

        public void HighlightAndCenterElements(List<CanvasObject> list)
        {
            if (list == null || list.Count == 0)
            {
                //Deselect();
                return;
            }


            selectGrip.Deselect();

            // 高亮选中的元素
            foreach (var element in controls)
            {
                //object.HighlightState = list.Contains(object);
                element.IsSelected = list.Contains(element);
            }

            // 计算居中位置
            if (list.Any())
            {
                var minX = list.Min(e => e.Left * zoom);
                var maxX = list.Max(e => (e.Left + e.Width) * zoom); // 50 为元素宽度
                var minY = list.Min(e => e.Top * zoom);
                var maxY = list.Max(e => (e.Top + e.Height) * zoom); // 50 为元素高度
                var centerX = (minX + maxX) / 2;
                var centerY = (minY + maxY) / 2;

                // 更新偏移量以居中
                offset = new PointF((Width / 2) - centerX, (Height / 2) - centerY);
                Invalidate(); // 触发重绘
            }
        }

        public void LoadOffset(PointF targetLocation)
        {
            this.offset = new PointF(targetLocation.X, targetLocation.Y);
            //Invalidate();
        }

        public void LoadZoom(double targetZoom)
        {
            this.zoom = Convert.ToSingle(targetZoom);
            InitializeCenterPoint();
        }


        public void RemoveRange(IEnumerable<CanvasObject> list)
        {
            if (list != null && list.Any())
            {
                for (var i = list.Count() - 1; i >= 0; i--)
                {
                    var canvasElement = list.ElementAt(i);
                    if (controls.Contains(canvasElement))
                    {
                        controls.Remove(canvasElement);
                    }
                }


                this.selectGrip.Deselect();

                this.Invalidate();
            }

        }


        #endregion

        #region 重写的方法

        protected override void OnHandleCreated(EventArgs e)
        {
            Loaded?.Invoke(this, e);

            base.OnHandleCreated(e);
        }

        protected override void OnResize(EventArgs e)
        {
            InitializeCenterPoint();

            base.OnResize(e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //var dpiX = GetDpiFactorX();
                //var dpiY = GetDpiFactorY();
                //var pointOnCanvas = new PointF(((e.X - offset.X) * dpiX / zoom), ((e.Y - offset.Y) * dpiY / zoom));
                var point = new PointF(((e.X - offset.X) / zoom), ((e.Y - offset.Y) / zoom));
                if (HitOnElement(point) is CanvasObject element)
                {
                    mouseActionIntent = MouseIntents.ActiveObject;
                    ObjectDoubleClick?.Invoke(this, element);
                }
            }

            base.OnMouseDoubleClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            //var dpiX = GetDpiFactorX();
            //var dpiY = GetDpiFactorY();
            //var pointOnCanvas = new PointF(((e.X - offset.X) * dpiX / zoom), ((e.Y - offset.Y) * dpiY / zoom));


            var pointOnCanvas = new PointF(((e.X - offset.X) / zoom), ((e.Y - offset.Y) / zoom));


            #region 鼠标右键

            if (e.Button == MouseButtons.Right)
            {
                rightMouseDownPosition = e.Location;

                // 点击在元素上， 显示右键菜单
                if (HitOnElement(pointOnCanvas) is CanvasObject element)
                {
                    mouseActionIntent = MouseIntents.ObjectContext;
                    ObjectRightClick?.Invoke(this, element);
                }
                else
                {
                    // 点击在画布上，默认为平移
                    if (AllowPan)
                    {
                        // 平移模式下会进行二次判断，如果偏移量小于一定值，则切换为右键菜单模式
                        mouseActionIntent = MouseIntents.PanCanvas;
                    }
                    else
                    {
                        mouseActionIntent = MouseIntents.CanvasContext;
                    }
                }

                lastRightMousePosition = e.Location;
                base.OnMouseDown(e);
                // 鼠标光标需要在OnMouseMove中设置

                return;
            }

            #endregion

            #region 鼠标左键

            if (e.Button == MouseButtons.Left)
            {
                leftMouseDownPosition = e.Location;

                // 判断是否点击在手柄上
                if (selectGrip.GetClickedGrip(pointOnCanvas) != null)
                {
                    base.OnMouseDown(e);
                    return;
                }

                // 优先判断点击是否在选择项上
                if (HitOnSelectionScope(pointOnCanvas))
                {
                    mouseActionIntent = MouseIntents.MoveScope;

                    //selectGrip.ShowGrip = false;
                }
                else if (HitOnElement(pointOnCanvas) is CanvasObject element)
                {
                    //selectGrip.Deselect();
                    mouseActionIntent = MouseIntents.SelectObject;

                    selectGrip.Deselect();
                    selectGrip.SelectSingle(element);
                    OnSelectionChanged(this, new List<CanvasObject> { element });

                    //object.IsSelected = true;
                    //BringToFront(object);

                    Invalidate(); // 重新绘制
                }
                else
                {
                    mouseActionIntent = MouseIntents.EmptySpaceLeft;

                    selectGrip.Deselect();
                    selectGrip.SetStart(e.Location);
                }


                // lastLeftMousePosition需要放在最后更新
                lastLeftMousePosition = e.Location;
                base.OnMouseDown(e);
                return;
            }

            #endregion 
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            PointerPosition = new PointF((e.X - offset.X) / zoom, (e.Y - offset.Y) / zoom);
            Invalidate();


            //var dpiX = GetDpiFactorX();
            //var dpiY = GetDpiFactorY(); 
            if (e.Button == MouseButtons.Right)
            {
                // 计算平移距离（平移模式下不除以zoom，否则缩放后平移不跟手，原因未知）
                var deltaX = (e.X - lastRightMousePosition.X);
                var deltaY = (e.Y - lastRightMousePosition.Y);

                #region 按住右键移动

                #region 二次判断HitIntent

                // 效果不太好
                if (mouseActionIntent == MouseIntents.PanCanvas)
                {
                    //if ((deltaX < SelectionThreshold && deltaY < SelectionThreshold))

                    if (deltaX == 0 && deltaY == 0)
                    {
                        //mouseActionIntent = MouseIntents.CanvasContext;

                        base.OnMouseMove(e);
                        return;
                    }
                }

                #endregion


                if (mouseActionIntent == MouseIntents.PanCanvas)
                {
                    offset.X += deltaX;
                    offset.Y += deltaY;
                    lastRightMousePosition = e.Location; // 更新最后鼠标位置 

                    Invalidate(); // 重新绘制
                    base.OnMouseMove(e);

                    // 设置光标要在OnMouseMove之后
                    this.Cursor = handGripCursor;

                    return;
                }




                #endregion

                lastRightMousePosition = e.Location; // 更新最后鼠标位置 
                base.OnMouseMove(e);
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                // 计算移动的距离 
                var deltaX = (e.X - lastLeftMousePosition.X) / zoom;
                var deltaY = (e.Y - lastLeftMousePosition.Y) / zoom;

                #region 按住左键移动

                #region 二次判断HitIntent

                if (mouseActionIntent == MouseIntents.EmptySpaceLeft)
                {
                    //if (Math.Abs(deltaX) > SelectionThreshold || Math.Abs(deltaY) > SelectionThreshold)
                    //{
                    //}
                    mouseActionIntent = MouseIntents.SelectScope;
                }
                else if (mouseActionIntent == MouseIntents.SelectObject)
                {
                    //if (Math.Abs(deltaX) > SelectionThreshold || Math.Abs(deltaY) > SelectionThreshold)
                    //{
                    //    mouseActionIntent = MouseIntents.MoveObject;
                    //}
                    mouseActionIntent = MouseIntents.MoveObject;
                }

                #endregion


                // 处理框选
                if (mouseActionIntent == MouseIntents.SelectScope)
                {
                    selectGrip.SetEnd(e.Location);
                    this.selectGrip.SelectMany(this.Controls);
                    Invalidate();

                    OnSelectionChanged(this, selectGrip.SelectedItems.ToList());
                    // 不更新lastLeftMousePosition
                    base.OnMouseMove(e);
                    return;
                }

                // 处理移动
                if (mouseActionIntent == MouseIntents.MoveObject || mouseActionIntent == MouseIntents.MoveScope)
                {
                    if (IsEditMode)
                    {
                        var selectedElements = selectGrip.SelectedItems;
                        movingObjects = selectedElements.Where(p => p.IsSelected && p.AllowMove).ToList();
                        movingPositions = new List<PointF>();
                        // 更新元素的位置, 记录下来以供ElementMoved事件使用
                        foreach (var item in movingObjects)
                        {
                            item.Location = new PointF(item.Left + deltaX, item.Top + deltaY);
                            movingPositions.Add(item.Location);
                        }

                        if (selectGrip.ShowGrip)
                        {
                            selectGrip.UpdateBoundingBox();
                        }

                        Invalidate();
                    }
                }

                #endregion


                this.Cursor = handGripCursor;
                lastLeftMousePosition = e.Location; // 更新起始位置
                base.OnMouseMove(e);
                return;
            }

            #region 鼠标悬浮

            // 检查鼠标是否在矩形上 
            var point = new PointF(((e.X - offset.X) / zoom), ((e.Y - offset.Y) / zoom));
            var element = HitOnElement(point);
            if (element != null && element.ShowToolTip)
            {
                mouseActionIntent = MouseIntents.HoverObject;
                toolTip.SetToolTip(this, element.ToolTip);
            }
            else
            {
                mouseActionIntent = MouseIntents.None;
                toolTip.SetToolTip(this, string.Empty);
            }

            #endregion

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            #region 修正mouseActionIntent

            bool noMovement = false;
            if (e.Button == MouseButtons.Left)
            {
                noMovement = (leftMouseDownPosition.X == e.X && leftMouseDownPosition.Y == e.Y);
            }
            else if (e.Button == MouseButtons.Right)
            {
                noMovement = (rightMouseDownPosition.X == e.X && rightMouseDownPosition.Y == e.Y);
            }
            if (noMovement)
            {

                if (mouseActionIntent == MouseIntents.MoveObject)
                {
                    mouseActionIntent = MouseIntents.SelectObject;
                }
                else if (mouseActionIntent == MouseIntents.MoveScope)
                {
                    mouseActionIntent = MouseIntents.SelectScope;
                }
                else if (mouseActionIntent == MouseIntents.PanCanvas)
                {
                    mouseActionIntent = MouseIntents.CanvasContext;
                }
            }


            #endregion


            if (mouseActionIntent == MouseIntents.SelectScope || mouseActionIntent == MouseIntents.SelectObject)
            {
                if (IsEditMode)
                {
                    selectGrip.ShowGrip = true;
                    selectGrip.UpdateBoundingBox();
                }

                Invalidate(); // 重新绘制
            }
            else if (mouseActionIntent == MouseIntents.MoveObject || mouseActionIntent == MouseIntents.MoveScope)
            {
                if (IsEditMode)
                {
                    ObjectMoved?.Invoke(movingObjects, movingPositions);
                }
            }
            else if (mouseActionIntent == MouseIntents.PanCanvas)
            {
                CanvasPanned?.Invoke(offset, zoom);
            }
            else if (mouseActionIntent == MouseIntents.CanvasContext)
            {
                CanvasContextMenuRequired?.Invoke(this, e);
            }

            this.Cursor = Cursors.Default;
            mouseActionIntent = MouseIntents.None;
            leftMouseDownPosition = Point.Empty;
            rightMouseDownPosition = Point.Empty;
            movingObjects = null;
            movingPositions = null;
            base.OnMouseUp(e);
        }


        // 根据滚轮方向调整 zoom
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (AllowZoom)
            {
                float oldZoom = zoom;

                // 按系数缩放
                //float zoomFactor = e.Delta > 0 ? 1.1f : 0.9f;
                //zoom *= zoomFactor;
                //zoom = Math.Max(0.1f, Math.Min(zoom, 10f)); // 限制缩放范围

                // 线性缩放
                float zoomIncrement = 0.1f;
                zoom += e.Delta > 0 ? zoomIncrement : -zoomIncrement;
                zoom = Math.Max(0.1f, Math.Min(zoom, 10f)); // 限制缩放范围


                if (ZoomInCenter)
                {
                    // 获取控件的中心点
                    PointF center = new PointF(ClientSize.Width / 2f, ClientSize.Height / 2f);

                    // 除以之前的缩放比例
                    float centerX = (center.X - offset.X) / oldZoom;
                    float centerY = (center.Y - offset.Y) / oldZoom;

                    // 
                    offset.X = center.X - centerX * zoom;
                    offset.Y = center.Y - centerY * zoom;

                    Invalidate(); // 重新绘制
                }
                else
                {
                                                                
                    // 计算鼠标位置在画布上的位置
                    Drawing.Point mousePos = e.Location;
                    float mouseX = (mousePos.X - offset.X) / oldZoom;
                    float mouseY = (mousePos.Y - offset.Y) / oldZoom;

                    // 更新偏移量
                    offset.X = mousePos.X - mouseX * zoom;
                    offset.Y = mousePos.Y - mouseY * zoom;

                    Invalidate(); // 重新绘制
                }

            }


            base.OnMouseWheel(e);
        }


        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (suspendPainting)
            {
                base.OnPaint(e);
                return;
            }

            Graphics g = e.Graphics;
            g.SetSlowRendering();

            // 应用平移和缩放
            g.TranslateTransform(offset.X, offset.Y);
            g.ScaleTransform(zoom, zoom);

            // 背景
            g.Clear(Color.White);
            if (BackgroundImage != null)
            {
                g.DrawImage(BackgroundImage, 0, 0, BackgroundImage.Width, BackgroundImage.Height);
            }


            // 绘制元素
            foreach (var element in controls.OrderBy(p => p.ZIndex))
            {
                element.DrawContent(g);
            }

            // 绘制选择框
            //if (mouseActionIntent == MouseIntents.SelectScope)
            //{
            //    g.DrawRectangle(Pens.Red, selectionBox.X, selectionBox.Y, selectionBox.Width, selectionBox.Height);
            //}

            // 绘制拉伸方块 


            base.OnPaint(e);
        }

        #endregion

        #region 私有方法


        private void OnSelectionChanged(CustomCanvas canvasLite, List<CanvasObject> selectedItems)
        {
            SelectionChanged?.Invoke(canvasLite, selectedItems);
        }

        private CanvasObject HitOnElement(PointF hitPoint)
        {
            foreach (var element in controls.Where(p => p.AllowSelect).OrderByDescending(p => p.ZIndex))
            {
                if (element.HitTest(hitPoint))
                {
                    return element;
                }
            }

            return null;
        }


        //TODO 移到SelectGrip中
        private bool HitOnSelectionScope(PointF hitPoint)
        {
            bool hitOnSelection = false;
            foreach (var item in selectGrip.SelectedItems)
            {
                if (item.HitTest(hitPoint))
                {
                    hitOnSelection = true;
                    break;
                }
            }

            return hitOnSelection;
        }


        /// <summary>
        /// 将元素置于最顶层
        /// </summary>
        /// <param name="object"></param>
        protected void BringToFront(CanvasObject @object)
        {
            // 初始化最大 ZIndex  
            int maxZIndex = int.MinValue;

            // 遍历所有元素，找到最大 ZIndex  
            foreach (var elem in this.controls)
            {
                if (elem.ZIndex > maxZIndex && elem != @object)
                {
                    maxZIndex = elem.ZIndex;
                }
            }

            // 置于最大的一个上面
            @object.ZIndex = maxZIndex + 1;
        }

        /// <summary>
        /// 将元素置于最底层
        /// </summary>
        /// <param name="object"></param>
        protected void SendToBack(CanvasObject @object)
        {
            // 初始化最小 ZIndex  
            int minZIndex = int.MaxValue;

            // 遍历所有元素，ZIndex都加1，同时找到最小的 ZIndex
            foreach (var elem in this.controls)
            {
                // 忽略当前元素  
                if (elem != @object)
                {
                    elem.ZIndex += 1;
                }

                // 找到最小 ZIndex  
                if (elem.ZIndex < minZIndex)
                {
                    minZIndex = elem.ZIndex;
                }
            }

            // 置于最小的一个下面
            @object.ZIndex = Math.Max(0, minZIndex - 1);
        }


        protected virtual float GetDpiFactorX()
        {
            float dpiScaleFactorX = 1;
            if (DpiParent != null)
            {
                dpiScaleFactorX = DpiParent.ScaleFactorRatioX;
            }

            return dpiScaleFactorX;
        }

        protected virtual float GetDpiFactorY()
        {
            float dpiScaleFactorY = 1;
            if (DpiParent != null)
            {
                dpiScaleFactorY = DpiParent.ScaleFactorRatioY;
            }

            return dpiScaleFactorY;
        }

        #endregion

        #region 缓存画刷和笔刷

        private Dictionary<Color, Brush> brushCache = new Dictionary<Color, Brush>();
        private Dictionary<Color, Pen> penCache = new Dictionary<Color, Pen>();

        internal Brush GetCachedBrush(Color color)
        {
            if (!brushCache.TryGetValue(color, out Brush brush))
            {
                brush = new SolidBrush(color);
                brushCache[color] = brush;
            }
            return brush;
        }

        internal Pen GetCachedPen(Color color, float width)
        {
            if (!penCache.TryGetValue(color, out Pen pen))
            {
                pen = new Pen(color, width);
                penCache[color] = pen;
            }
            return pen;
        }

        internal Pen GetCachedPen(Color color, float width, Drawing.Drawing2D.DashStyle style)
        {
            if (!penCache.TryGetValue(color, out Pen pen))
            {
                pen = new Pen(color, width) { DashStyle = style };
                penCache[color] = pen;
            }
            return pen;
        }


        #endregion

    }
}
