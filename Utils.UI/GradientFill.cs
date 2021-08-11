using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;


namespace System
{
    /// <summary>
    /// 此类包装平台调用的GradientFill方法.
    /// </summary>
    public sealed class GradientFill
    {
        /// <summary>
        ///  渐变填充
        /// </summary>
        /// <param name="gr"></param>
        /// <param name="rc"></param>
        /// <param name="startColor"></param>
        /// <param name="endColor"></param>
        /// <param name="fillDir"></param>
        /// <returns></returns>
        public static bool Fill( Graphics g, Rectangle rc, Color startColor, Color endColor, FillDirection fillDir )
        {
            TRIVERTEX[] tva = new TRIVERTEX[2];
            tva[0] = new TRIVERTEX(rc.X, rc.Y, startColor);
            tva[1] = new TRIVERTEX(rc.Right, rc.Bottom, endColor);

            GRADIENT_RECT[] gra = new GRADIENT_RECT[] 
            { 
                new GRADIENT_RECT(0, 1)
            };

            IntPtr hdc = g.GetHdc();

            bool InvokeResult = Win32.GradientFill(hdc, tva, (uint)tva.Length, gra, (uint)gra.Length, (uint)fillDir);
            if (!InvokeResult)
            {
                System.Diagnostics.Debug.Assert(
                    InvokeResult, 
                    string.Format("GradientFill failed: {0}", System.Runtime.InteropServices.Marshal.GetLastWin32Error()));
            }

            g.ReleaseHdc(hdc);

            return InvokeResult;
        }
    }

    public enum FillDirection
    {
        /// <summary>
        ///  水平方向填充
        /// </summary>
        LeftToRight = 0x00000000,
        /// <summary>
        ///  垂直方向填充
        /// </summary>
        TopToBottom = 0x00000001
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TRIVERTEX
    {
        public int x;
        public int y;
        public ushort Red;
        public ushort Green;
        public ushort Blue;
        public ushort Alpha;

        private TRIVERTEX(int x, int y, ushort red, ushort green, ushort blue, ushort alpha)
        {
            this.x = x;
            this.y = y;
            this.Red = (ushort)(red << 8);
            this.Green = (ushort)(green << 8);
            this.Blue = (ushort)(blue << 8);
            this.Alpha = (ushort)(alpha << 8);
        }

        public TRIVERTEX(int x, int y, Color color)
            : this(x, y, color.R, color.G, color.B, color.A)
        {
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GRADIENT_RECT
    {
        public uint UpperLeft;
        public uint LowerRight;
        public GRADIENT_RECT(uint UpperLeft, uint LowerRight)
        {
            this.UpperLeft = UpperLeft;
            this.LowerRight = LowerRight;
        }
    }


    public partial class Win32
    {

        /// <summary>
        /// 该函数填充矩形和三角形结构
        /// </summary>
        /// <param name="hdc">指向目标设备环境的句柄</param>
        /// <param name="pVertex">指向TRIVERTEX结构数组的指针，该数组中的每项定义了三角形顶点。</param>
        /// <param name="dwNumVertex">顶点数目</param>
        /// <param name="pMesh">三角形模式下的GRADIENT_TRIANGLE结构数组，或矩形模式下的GRADIENT_RECT结构数组</param>
        /// <param name="dwNumMesh">参数pMesh中的成员数目（这些成员是三角形或矩形）</param>
        /// <param name="dwMode">指定倾斜填充模式。该参数可以包含下列值，这些值的含义为：
        /// GRADIENT_FILL_RECT_H：在该模式下，两个端点表示一个矩形。该矩形被定义成左右边界具有固定颜色（由TRIVERTEX结构指定）。GDI从上至下插入颜色，并填充内部区域。
        /// GRADIENT_FILL_RECT_V：在该模式下，两个端点表示一个矩形。该矩形定义其顶部和底部边界的颜色为固定值（通过TRIVERTEX结构指定），GDI从顶至底部边界插入颜色，并填充内部区域。
        /// GRADIENT＿FILL＿TRIANGLE：在该模式下，TRIVERTEX结构数组以及描述单个三角形的数组索引序列被传给GDI。GDI在三角形顶点之间进行线性插值，并填充内部区域。在24和32位／像素模式下，绘图是直接进行。在16、8、4和1位／像素模式中进行抖动处理。
        /// </param>
        /// <returns>如果函数执行成功，那么返回值为TRUE；如果函数执行失败，则返回值为FALSE</returns>
        [DllImport("msimg32.dll", SetLastError = true, EntryPoint = "GradientFill")]
        public extern static bool GradientFill(IntPtr hdc, TRIVERTEX[] pVertex, uint dwNumVertex, GRADIENT_RECT[] pMesh, uint dwNumMesh, uint dwMode);

    }
}
