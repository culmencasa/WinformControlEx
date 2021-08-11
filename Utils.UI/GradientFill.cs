using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;


namespace System
{
    /// <summary>
    /// �����װƽ̨���õ�GradientFill����.
    /// </summary>
    public sealed class GradientFill
    {
        /// <summary>
        ///  �������
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
        ///  ˮƽ�������
        /// </summary>
        LeftToRight = 0x00000000,
        /// <summary>
        ///  ��ֱ�������
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
        /// �ú��������κ������νṹ
        /// </summary>
        /// <param name="hdc">ָ��Ŀ���豸�����ľ��</param>
        /// <param name="pVertex">ָ��TRIVERTEX�ṹ�����ָ�룬�������е�ÿ����������ζ��㡣</param>
        /// <param name="dwNumVertex">������Ŀ</param>
        /// <param name="pMesh">������ģʽ�µ�GRADIENT_TRIANGLE�ṹ���飬�����ģʽ�µ�GRADIENT_RECT�ṹ����</param>
        /// <param name="dwNumMesh">����pMesh�еĳ�Ա��Ŀ����Щ��Ա�������λ���Σ�</param>
        /// <param name="dwMode">ָ����б���ģʽ���ò������԰�������ֵ����Щֵ�ĺ���Ϊ��
        /// GRADIENT_FILL_RECT_H���ڸ�ģʽ�£������˵��ʾһ�����Ρ��þ��α���������ұ߽���й̶���ɫ����TRIVERTEX�ṹָ������GDI�������²�����ɫ��������ڲ�����
        /// GRADIENT_FILL_RECT_V���ڸ�ģʽ�£������˵��ʾһ�����Ρ��þ��ζ����䶥���͵ײ��߽����ɫΪ�̶�ֵ��ͨ��TRIVERTEX�ṹָ������GDI�Ӷ����ײ��߽������ɫ��������ڲ�����
        /// GRADIENT��FILL��TRIANGLE���ڸ�ģʽ�£�TRIVERTEX�ṹ�����Լ��������������ε������������б�����GDI��GDI�������ζ���֮��������Բ�ֵ��������ڲ�������24��32λ������ģʽ�£���ͼ��ֱ�ӽ��С���16��8��4��1λ������ģʽ�н��ж�������
        /// </param>
        /// <returns>�������ִ�гɹ�����ô����ֵΪTRUE���������ִ��ʧ�ܣ��򷵻�ֵΪFALSE</returns>
        [DllImport("msimg32.dll", SetLastError = true, EntryPoint = "GradientFill")]
        public extern static bool GradientFill(IntPtr hdc, TRIVERTEX[] pVertex, uint dwNumVertex, GRADIENT_RECT[] pMesh, uint dwNumMesh, uint dwMode);

    }
}
