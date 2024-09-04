using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms.Canvas
{
    internal class SelectGrip
    {
        public SelectGrip(CustomCanvas control)
        {
            this.canvas = control;
            canvas.Paint += Canvas_Paint;
            canvas.MouseDown += Canvas_MouseDown;
            canvas.MouseMove += Canvas_MouseMove;
            canvas.MouseUp += Canvas_MouseUp;
            canvas.Leave += Canvas_Leave;


            selectedItems = new List<CanvasObject>();
        }

        private PointF previousPoint; // 用于存储上一次的鼠标位置


        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                IsMouseLeftDown = true;


                var point = new PointF(((e.X - canvas.Offset.X) / canvas.Zoom), ((e.Y - canvas.Offset.Y) / canvas.Zoom));

                var clickedGrip = GetClickedGrip(point);
                if (clickedGrip != null)
                {
                    // 启用Resize模式
                    currentGrip = clickedGrip.Value;
                    currentCursor = GetCursorForGrip(currentGrip);

                    resizeDirection = GetResizeDirection();
                    isResizing = true;

                    // 获取所有要拉伸的对象
                    if (GrippedObjects == null)
                    {
                        GrippedObjects = new List<CanvasObject>();
                    }
                    GrippedObjects.Clear();
                    GrippedObjects.AddRange(selectedItems.Where(x => x.AllowResize));


                    StartPoint = new PointF(e.X, e.Y); // 记录起始点
                    previousPoint = StartPoint; // 初始位置

                    return;
                }


                GrippedObjects?.Clear();

            }
        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizing && GrippedObjects.Count > 0)
            {
                canvas.Cursor = currentCursor;

                // 计算变化量
                float deltaX = (e.X - previousPoint.X) / canvas.Zoom;
                float deltaY = (e.Y - previousPoint.Y) / canvas.Zoom;

                if (deltaX == 0 && deltaY == 0)
                {
                    return;
                }

                //Debug.WriteLine($"deltaX:{deltaX}, deltaY:{deltaY}");

                foreach (var obj in GrippedObjects)
                {
                    var bounds = obj.Bounds;

                    float newWidth = bounds.Width;
                    float newHeight = bounds.Height;
                    float newX = bounds.X;
                    float newY = bounds.Y;


                    // 根据 currentCursor 来调整对象的位置和尺寸
                    switch (resizeDirection)
                    {
                        case ResizeDirection.BottomRight:
                            newWidth += deltaX;
                            newHeight += deltaY;
                            break;

                        case ResizeDirection.TopLeft:
                            newX += deltaX;
                            newY += deltaY;
                            newWidth -= deltaX;
                            newHeight -= deltaY;
                            break;

                        case ResizeDirection.Top:
                            newY += deltaY;
                            newHeight -= deltaY;
                            break;

                        case ResizeDirection.Bottom:
                            newHeight += deltaY;
                            break;

                        case ResizeDirection.Left:
                            newX += deltaX;
                            newWidth -= deltaX;
                            break;

                        case ResizeDirection.Right:
                            newWidth += deltaX;
                            break;

                        case ResizeDirection.TopRight:
                            newY += deltaY;
                            newHeight -= deltaY;
                            newWidth += deltaX;
                            break;

                        case ResizeDirection.BottomLeft:
                            newX += deltaX;
                            newWidth -= deltaX;
                            newHeight += deltaY;
                            break;
                    }

                    // 确保尺寸不为负数，并更新坐标
                    if (newWidth >= 1 && newHeight >= 1)
                    {
                        obj.Width = newWidth;
                        obj.Height = newHeight;
                        obj.Left = newX;
                        obj.Top = newY;
                    }

                    //Debug.WriteLine($"Updated bounds: X={obj.Left}, Y={obj.Top}, Width={obj.Width}, Height={obj.Height}");

                }

                UpdateBoundingBox(); // 更新手柄位置
                canvas.Invalidate(); // 触发重绘
                previousPoint = new PointF(e.X, e.Y); // 更新鼠标位置
                return;
            }

            else if (IsMouseLeftDown)
            {
                var deltaX = (e.X - StartPoint.X) / canvas.Zoom;
                var deltaY = (e.Y - StartPoint.Y) / canvas.Zoom;

                //if (Math.Abs(deltaX) > 6 || Math.Abs(deltaY) > 6)
                //{
                //    //ShowGrip = true;
                //    canvas.Invalidate();
                //}

                //UpdateBoundingBox(); // 更新手柄位置
                //canvas.Invalidate();
            }
            else
            {
                // 判断是否需要更改光标
                var point = new PointF(((e.X - canvas.Offset.X) / canvas.Zoom), ((e.Y - canvas.Offset.Y) / canvas.Zoom));

                var clickedGrip = GetClickedGrip(point);
                if (clickedGrip != null)
                {
                    canvas.Cursor = GetCursorForGrip(clickedGrip.Value);
                }
                else
                {
                    canvas.Cursor = Cursors.Default;
                }
            }
        }


        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            IsMouseLeftDown = false;
            StartPoint = PointF.Empty;
            EndPoint = PointF.Empty;
            SelectionBox = RectangleF.Empty;
            GrippedObjects?.Clear();


            isResizing = false;
            canvas.Cursor = Cursors.Default; // 重置光标
        }
        private void Canvas_Leave(object sender, EventArgs e)
        {
            IsMouseLeftDown = false;
        }




        //框选开始坐标
        Drawing.PointF StartPoint;
        Drawing.PointF EndPoint;

        bool IsMouseLeftDown = false;


        CustomCanvas canvas;
        // 拉伸方块尺寸
        private const int GripSize = 6;
        private Color gripLineColor = Color.Red;

        private List<RectangleF> gripRectangles = new List<RectangleF>();
        private RectangleF currentGrip; // 当前被点击的Grip
        private List<CanvasObject> GrippedObjects;
        private Cursor currentCursor; // 当前光标
        private ResizeDirection resizeDirection;

        private bool isResizing = false; // 是否正在调整大小

        // 选择元素列表
        private List<CanvasObject> selectedItems;
        public List<CanvasObject> SelectedItems
        {
            get
            {
                return selectedItems;
            }
        }

        /// <summary>
        /// 框选范围
        /// </summary>
        public RectangleF SelectionBox
        {
            get;
            private set;
        }



        public void SetStart(PointF startPoint)
        {
            StartPoint = startPoint;
        }

        public void SetEnd(PointF endPoint)
        {
            EndPoint = endPoint;

            // 计算框选的区域，考虑偏移和缩放 
            float x = ((StartPoint.X - canvas.Offset.X) / canvas.Zoom);
            float y = ((StartPoint.Y - canvas.Offset.Y) / canvas.Zoom);
            float width = ((EndPoint.X - canvas.Offset.X) / canvas.Zoom) - x;
            float height = ((EndPoint.Y - canvas.Offset.Y) / canvas.Zoom) - y;

            SelectionBox = new RectangleF(
                Math.Min(x, x + width),
                Math.Min(y, y + height),
                Math.Abs(width),
                Math.Abs(height));
        }

        /// <summary>
        /// 取消选择
        /// </summary>
        public void Deselect()
        {
            canvas.SuspendRender();

            //ShowGrip = false;
            ShowGrip = false;
            GrippedObjects?.Clear();

            foreach (var item in canvas.Controls)
            {
                item.IsSelected = false;
                item.HighlightState = false;
            }
            selectedItems.Clear(); // 清除之前的选择


            canvas.ResumeRender();
        }

        public void SelectSingle(CanvasObject element)
        {
            if (element.AllowSelect)
            {
                selectedItems.Clear();
                selectedItems.Add(element);
                element.IsSelected = true;
                element.HighlightState = true;
            }
            else
            {
                element.HighlightState = true;
            }
        }


        public void SelectMany(IEnumerable<CanvasObject> canvasControls)
        {
            foreach (var item in canvasControls)
            {
                if (SelectionBox.IntersectsWith(item.Bounds) &&
                     CustomCanvasHelper.GetOverlapRatio(SelectionBox, item.Bounds) >= 0.45)
                {
                    if (!selectedItems.Contains(item))
                    {
                        selectedItems.Add(item);
                        if (item.AllowSelect)
                        {
                            item.IsSelected = true;
                            item.HighlightState = true;
                        }
                        else
                        {
                            item.HighlightState = true;
                        }
                    }
                }
                else
                {
                    item.IsSelected = false;
                    if (selectedItems.Contains(item))
                    {
                        selectedItems.Remove(item); 
                        if (item.AllowSelect)
                        {
                            item.IsSelected = false;
                            item.HighlightState = false;
                        }
                        else
                        {
                            item.HighlightState = false;
                        }
                    }
                }
            }
        }


        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //using (var pen = new Pen(gripLineColor, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
            //{
            //    foreach (var item in canvas.SelectedItems)
            //    {
            //        var border = item.Bounds;
            //        border = border.InflateCenter(10, 10);

            //        var x = border.Left;
            //        var y = border.Top;

            //        var newgrips = border.CreateGrips(GripSize);
            //        var gripBrush = canvas.GetCachedBrush(gripLineColor);
            //        foreach (var gripRect in newgrips)
            //        {
            //            g.FillRectangle(gripBrush, gripRect);
            //        }

            //        g.DrawRectangle(pen, border);
            //    }
            //}

            // 绘制选择框 

            if (IsMouseLeftDown == true && SelectionBox.Width > 0 && SelectionBox.Height > 0)
            {
                using (var brush = new SolidBrush(Color.FromArgb(100, 51, 153, 255)))
                using (var pen = new Pen(Color.FromArgb(255, 51, 153, 255)))
                {
                    g.FillRectangle(brush, SelectionBox);
                    g.DrawRectangle(pen, SelectionBox);

                }
            }


            if (ShowGrip)
            {
                using (var pen = new Pen(gripLineColor, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
                {
                    var gripBrush = canvas.GetCachedBrush(gripLineColor);
                    foreach (var grip in this.gripRectangles)
                    {
                        g.FillRectangle(gripBrush, grip);
                    }

                    g.DrawRectangle(pen, boundingBox);
                }
            }

        }

        #region Resize相关


        private RectangleF boundingBox;

        public bool ShowGrip { get; set; }


        public void UpdateBoundingBox()
        {
            var resizableItems = selectedItems.Where(x => x.AllowResize).ToList();
            if (resizableItems.Count == 0)
            {
                ShowGrip = false;
                canvas.Invalidate();
                return;
            }

            // 计算包含所有选中元素的包围矩形
            var firstElement = resizableItems[0];
            boundingBox = new RectangleF(firstElement.Left, firstElement.Top, firstElement.Width, firstElement.Height);

            foreach (var element in resizableItems)
            {
                boundingBox = RectangleF.Union(boundingBox, new RectangleF(element.Left, element.Top, element.Width, element.Height));
            }

            boundingBox.Inflate(10, 10); // 增加一些边距

            Debug.WriteLine($"BoundingBox: {boundingBox.X},{boundingBox.Y},{boundingBox.Width},{boundingBox.Height}");

            // 实时更新手柄的位置，而不是清空再重建
            var newgrips = boundingBox.CreateGrips(GripSize);
            if (gripRectangles.Count == 0)
            {
                gripRectangles.AddRange(newgrips);
            }
            else
            {
                for (int i = 0; i < newgrips.Length; i++)
                {
                    var rect = gripRectangles[i];
                    rect.X = newgrips[i].X;
                    rect.Y = newgrips[i].Y;
                    rect.Width = newgrips[i].Width;
                    rect.Height = newgrips[i].Height;
                    gripRectangles[i] = rect;
                }
            }

            ShowGrip = true;
        }



        // 判断是否点击在拖动手柄上
        public RectangleF? GetClickedGrip(PointF location)
        {
            // 判断是否点击在手柄上
            foreach (var grip in gripRectangles)
            {
                if (grip.Contains(location))
                {
                    return grip;
                }
            }

            return null;
        }


        // 获取对应的光标方向
        private Cursor GetCursorForGrip(RectangleF grip)
        {
            if (grip == gripRectangles[0] || grip == gripRectangles[4])
                return Cursors.SizeNWSE;
            if (grip == gripRectangles[2] || grip == gripRectangles[6])
                return Cursors.SizeNESW;
            if (grip == gripRectangles[1] || grip == gripRectangles[5])
                return Cursors.SizeNS;
            if (grip == gripRectangles[3] || grip == gripRectangles[7])
                return Cursors.SizeWE;

            return Cursors.Default;
        }
        public enum ResizeDirection
        {
            None,
            Top,
            Bottom,
            Left,
            Right,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }
        private ResizeDirection GetResizeDirection()
        {
            if (currentCursor == Cursors.SizeNWSE)
            {
                // NWSE可能是TopLeft或BottomRight
                if (currentGrip == gripRectangles[0]) return ResizeDirection.TopLeft;
                if (currentGrip == gripRectangles[4]) return ResizeDirection.BottomRight;
            }
            else if (currentCursor == Cursors.SizeNESW)
            {
                // NESW可能是TopRight或BottomLeft
                if (currentGrip == gripRectangles[2]) return ResizeDirection.TopRight;
                if (currentGrip == gripRectangles[6]) return ResizeDirection.BottomLeft;
            }
            else if (currentCursor == Cursors.SizeNS)
            {
                // SizeNS可能是Top或Bottom
                if (currentGrip == gripRectangles[1]) return ResizeDirection.Top;
                if (currentGrip == gripRectangles[5]) return ResizeDirection.Bottom;
            }
            else if (currentCursor == Cursors.SizeWE)
            {
                // SizeWE可能是Left或Right
                if (currentGrip == gripRectangles[7]) return ResizeDirection.Left;
                if (currentGrip == gripRectangles[3]) return ResizeDirection.Right;
            }

            return ResizeDirection.None;
        }


        #endregion
    }
}
