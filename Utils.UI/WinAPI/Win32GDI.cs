using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using System.ComponentModel;

namespace System
{
    public partial class Win32
    {
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth,
      int nHeight, IntPtr hObjSource, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);


        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, CopyPixelOperation dwRop);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);




        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, BITMAPINFOHEADER hdr, uint colors, ref IntPtr pBits, IntPtr hFile, uint offset);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, IntPtr hdr, uint colors, ref IntPtr pBits, IntPtr hFile, uint offset);


        [DllImport("gdi32.dll", EntryPoint = "CreateFontIndirect", SetLastError = true)]
        private static extern IntPtr CreateFontIndirectCE(IntPtr pLogFont);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreatePen(int fnPenStyle, int nWidth, int crColor);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateSolidBrush(int[] color);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateSolidBrush(int color);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int DrawFrameControl(IntPtr hdc, ref System.Drawing.Rectangle rect, int uType, int uState);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int DrawText(IntPtr hDC, string Text, int nLen, IntPtr pRect, uint uFormat);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int DrawText(IntPtr hDC, string Text, int nLen, ref RECT rect, uint uFormat);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int Ellipse(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int ExtTextOut(IntPtr hdc, int X, int Y, uint fuOptions, ref RECT lprc, string lpString, int cbCount, int[] lpDx);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr GetCapture();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr GetFocus();

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int GetObject(IntPtr hgdiobj, int cbBuffer, ref BITMAP lpvObject);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int GetTextExtentExPoint(IntPtr hdc, string lpszStr, int cchString, int nMaxExtent, out int lpnFit, int[] alpDx, ref SIZE lpSize);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int GetTextMetrics(IntPtr hdc, ref TEXTMETRIC lptm);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int GetWindowRect(IntPtr hWnd, IntPtr lpRect);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool GradientFill(IntPtr hdc, uint[] pVertex, int dwNumVertex, int[] pMesh, int dwNumMesh, int dwMode);


        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int LineTo(IntPtr hdc, int nXEnd, int nYEnd);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr LocalAlloc(int flags, int size);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern void LocalFree(IntPtr p);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int MoveToEx(IntPtr hdc, int X, int Y, ref POINT lpPoint);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int Polyline(IntPtr hdc, int[] lppt, int cPoints);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool Rectangle(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern void ReleaseDC(IntPtr hDC);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int RoundRect(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidth, int nHeight);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int SelectClipRgn(IntPtr hDC, IntPtr hRgn);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int SetBitmapBits(IntPtr hBitmap, int flag, IntPtr hData);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int SetBkColor(IntPtr hDC, int cColor);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int SetBkMode(IntPtr hDC, int nMode);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr SetCapture(IntPtr hWnd);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern uint SetPixel(IntPtr hdc, int X, int Y, uint crColor);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr SetROP2(IntPtr hDC, ROP rop);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int SetTextColor(IntPtr hDC, int cColor);


        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int StretchBlt(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest, int nHeightDest, IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc, uint dwRop);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int TransparentImage(IntPtr IntPtr, int DstX, int DstY, int DstCx, int DstCy, IntPtr hSrc, int SrcX, int SrcY, int SrcCx, int SrcCy, int TransparentColor);



        [DllImport("gdi32.dll", EntryPoint = "DeleteObject", CharSet = CharSet.Ansi)]
        public static extern int DeleteObject(int hObject);

        [DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        public static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
         );


        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr DeleteObject(IntPtr hObject);
    }
}
