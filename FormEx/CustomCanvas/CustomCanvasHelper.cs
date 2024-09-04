using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms.Canvas
{
    public static class CustomCanvasHelper
    {
        public static Point ToPoint(this PointF point)
        {
            return new Point((int)point.X, (int)point.Y);
        }

        public static Rectangle ToRectangle(this RectangleF rectangle)
        {
            return new Rectangle((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height);
        }

        /// <summary>
        /// 扩展方法，用于将矩形以中心点为基准进行放大或缩小
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static RectangleF InflateCenter(this RectangleF rectangle, float width, float height)
        {
            RectangleF newRectangle = new RectangleF(
                rectangle.X - width / 2,
                rectangle.Y - height / 2,
                rectangle.Width + width,
                rectangle.Height + height
            );

            return newRectangle;
        }

        /// <summary>
        /// 创建一个矩形对象的控制点（ grips ）数组。每个控制点是一个小的矩形，用于调整主矩形的大小或位置。
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="GripSize"></param>
        /// <returns></returns>
        public static RectangleF[] CreateGrips(this RectangleF rectangle, int GripSize)
        {
            var x = rectangle.X;
            var y = rectangle.Y;
            var halfSize = GripSize / 2;

            var grips = new[]
            {
        new RectangleF(x - halfSize, y - halfSize, GripSize, GripSize), // Top-left
        new RectangleF(x + (rectangle.Width / 2) - halfSize, y - halfSize, GripSize, GripSize), // Top-middle
        new RectangleF(x + rectangle.Width - halfSize, y - halfSize, GripSize, GripSize), // Top-right
        new RectangleF(x + rectangle.Width - halfSize, y + (rectangle.Height / 2) - halfSize, GripSize, GripSize), // Right-middle
        new RectangleF(x + rectangle.Width - halfSize, y + rectangle.Height - halfSize, GripSize, GripSize), // Bottom-right
        new RectangleF(x + (rectangle.Width / 2) - halfSize, y + rectangle.Height - halfSize, GripSize, GripSize), // Bottom-middle
        new RectangleF(x - halfSize, y + rectangle.Height - halfSize, GripSize, GripSize), // Bottom-left
        new RectangleF(x - halfSize, y + (rectangle.Height / 2) - halfSize, GripSize, GripSize), // Left-middle
    };

            return grips;
        }


        /// <summary>
        /// 支持RectangleF的DrawRectangle方法
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="pen"></param>
        /// <param name="rectangle"></param>
        public static void DrawRectangle(this Graphics graphics, Pen pen, RectangleF rectangle)
        {
            graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        /// <summary>
        /// 鼠标坐标点变换到显示坐标点
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static PointF MousePointToLocal(this CustomCanvas canvas, Point point)
        {
            return new PointF( (point.X - canvas.CanvasCenterPoint.X) / canvas.Zoom , (point.Y - canvas.CanvasCenterPoint.Y) / canvas.Zoom );
        }



        /// <summary>
        /// 计算两个矩形的重叠比例
        /// </summary>
        /// <param name="rect1"></param>
        /// <param name="rect2"></param>
        /// <returns></returns>
        public static float GetOverlapRatio(RectangleF rect1, RectangleF rect2)
        {
            RectangleF intersection = RectangleF.Intersect(rect1, rect2);
            float intersectionArea = intersection.Width * intersection.Height;
            float area2 = rect2.Width * rect2.Height;

            return intersectionArea / area2; // 返回交集面积与第二个矩形的面积比例
        }


    }
}
