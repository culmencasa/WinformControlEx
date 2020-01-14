using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using System.ComponentModel;

namespace System.Drawing
{
    public partial class Win32
    {
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth,
           int nHeight, IntPtr hObjSource, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

        [DllImport("gdi32.dll", SetLastError = true)]
        internal static extern IntPtr GetDesktopWindow();
        
        [DllImport("gdi32.dll", SetLastError = true)]
        internal static extern IntPtr GetWindowDC(IntPtr hWnd);


        [DllImport("gdi32.dll", SetLastError = true)]
        internal static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, CopyPixelOperation dwRop);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);


        //public static IntPtr BitmapLockBits(BitmapEx bitmap, int flags, int format, System.Drawing.Imaging.BitmapDataClass bitmapData)
        //{
        //    BITMAPINFOHEADER bitmapinfoheader;
        //    IntPtr dC = GetDC(IntPtr.Zero);
        //    bitmapinfoheader = new BITMAPINFOHEADER
        //    {
        //        biBitCount = 0x18,
        //        biClrUsed = 0,
        //        biClrImportant = 0
        //    };
        //    bitmapinfoheader.biSize = (uint)Marshal.SizeOf(bitmapinfoheader);
        //    if ((bitmapinfoheader.biBitCount == 0x10) || (bitmapinfoheader.biBitCount == 0x20))
        //    {
        //        bitmapinfoheader.biCompression = 3;
        //    }
        //    else
        //    {
        //        bitmapinfoheader.biCompression = 0;
        //    }
        //    bitmapinfoheader.biHeight = bitmap.Height;
        //    bitmapinfoheader.biWidth = bitmap.Width;
        //    bitmapinfoheader.biPlanes = 1;
        //    int num1 = ((bitmapinfoheader.biHeight * bitmapinfoheader.biWidth) * bitmapinfoheader.biBitCount) / 8;
        //    bitmapinfoheader.biSizeImage = (uint)(BytesPerLine(bitmapinfoheader.biBitCount, bitmap.Width) * bitmap.Height);
        //    bitmapinfoheader.biXPelsPerMeter = 0xb12;
        //    bitmapinfoheader.biYPelsPerMeter = 0xb12;
        //    IntPtr zero = IntPtr.Zero;
        //    IntPtr ptr = LocalAlloc(0x40, (int)bitmapinfoheader.biSize);
        //    Marshal.StructureToPtr(bitmapinfoheader, ptr, false);
        //    IntPtr hObject = CreateDIBSection(dC, ptr, 0, ref zero, IntPtr.Zero, 0);
        //    Marshal.PtrToStructure(ptr, bitmapinfoheader);
        //    BITMAPINFOHEADER bitmapinfoheader2 = (BITMAPINFOHEADER)Marshal.PtrToStructure(ptr, typeof(BITMAPINFOHEADER));
        //    IntPtr hDC = CreateCompatibleDC(dC);
        //    IntPtr ptr6 = CreateCompatibleDC(dC);
        //    IntPtr ptr7 = SelectObject(hDC, bitmap.hBitmap);
        //    IntPtr ptr8 = SelectObject(ptr6, hObject);
        //    BitBlt(ptr6, 0, 0, bitmap.Width, bitmap.Height, hDC, 0, 0, 0xcc0020);
        //    bitmapData.Height = bitmap.Height;
        //    bitmapData.Width = bitmap.Width;
        //    bitmapData.Scan0 = zero;
        //    bitmapData.Stride = BytesPerLine(bitmapinfoheader2.biBitCount, bitmap.Width);
        //    SelectObject(hDC, ptr7);
        //    SelectObject(ptr6, ptr8);
        //    DeleteDC(hDC);
        //    DeleteDC(ptr6);
        //    return hObject;
        //}

        private static int BytesPerLine(int nWidth, int nBitsPerPixel)
        {
            return ((((nWidth * nBitsPerPixel) + 0x1f) & -32) / 8);
        }

        private int CountBits(int dw)
        {
            int num = 0;
            while (dw == 0)
            {
                num += dw & 1;
                dw = dw >> 1;
            }
            return num;
        }

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, BITMAPINFOHEADER hdr, uint colors, ref IntPtr pBits, IntPtr hFile, uint offset);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, IntPtr hdr, uint colors, ref IntPtr pBits, IntPtr hFile, uint offset);

        public static IntPtr CreateFontIndirect(IntPtr pLogFont)
        {
            IntPtr ptr = CreateFontIndirectCE(pLogFont);
            if (ptr == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Impossible to create a logical font.");
            }
            return ptr;
        }

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



        [DllImport("aygshell.dll", EntryPoint = "#75", SetLastError = true)]
        public static extern IntPtr SHLoadImageFile(string szFileName);
        public static void Snapshot(IntPtr hReal, string FileName, System.Drawing.Rectangle rect)
        {
            BITMAPINFOHEADER bitmapinfoheader;
            RECT structure = new RECT
            {
                bottom = 0,
                left = 0,
                right = 0,
                top = 0
            };
            IntPtr lpRect = LocalAlloc(0x40, Marshal.SizeOf(structure));
            GetWindowRect(hReal, lpRect);
            structure = (RECT)Marshal.PtrToStructure(lpRect, typeof(RECT));
            int nWidth = (structure.right - structure.left) - 1;
            int nHeight = structure.bottom - structure.top;
            nWidth = rect.Width;
            nHeight = rect.Height;
            IntPtr dC = GetDC(hReal);
            IntPtr hDC = CreateCompatibleDC(dC);
            bitmapinfoheader = new BITMAPINFOHEADER
            {
                biBitCount = 0x18,
                biClrUsed = 0,
                biClrImportant = 0,
                biCompression = 0,
                biHeight = nHeight,
                biWidth = nWidth,
                biPlanes = 1
            };
            bitmapinfoheader.biSize = (uint)Marshal.SizeOf(bitmapinfoheader);
            int length = ((bitmapinfoheader.biHeight * bitmapinfoheader.biWidth) * bitmapinfoheader.biBitCount) / 8;
            bitmapinfoheader.biSizeImage = (uint)length;
            bitmapinfoheader.biXPelsPerMeter = 0xb12;
            bitmapinfoheader.biYPelsPerMeter = 0xb12;
            IntPtr zero = IntPtr.Zero;
            IntPtr ptr = LocalAlloc(0x40, (int)bitmapinfoheader.biSize);
            Marshal.StructureToPtr(bitmapinfoheader, ptr, false);
            IntPtr hObject = CreateDIBSection(dC, ptr, 0, ref zero, IntPtr.Zero, 0);
            Marshal.PtrToStructure(ptr, bitmapinfoheader);
            BITMAPINFOHEADER bitmapinfoheader2 = (BITMAPINFOHEADER)Marshal.PtrToStructure(ptr, typeof(BITMAPINFOHEADER));
            IntPtr ptr7 = SelectObject(hDC, hObject);
            BitBlt(hDC, 0, 0, nWidth, nHeight, dC, 0, 0, 0xcc0020);
            byte[] destination = new byte[length];
            Marshal.Copy(zero, destination, 0, length);
            BITMAPFILEHEADER bitmapfileheader = new BITMAPFILEHEADER
            {
                bfSize = (uint)(length + 0x36),
                bfType = 0x4d42,
                bfOffBits = 0x36
            };
            int index = 14;
            byte[] array = new byte[index];
            BitConverter.GetBytes(bitmapfileheader.bfType).CopyTo(array, 0);
            BitConverter.GetBytes(bitmapfileheader.bfSize).CopyTo(array, 2);
            BitConverter.GetBytes(bitmapfileheader.bfOffBits).CopyTo(array, 10);
            byte[] buffer3 = new byte[length + 0x36];
            array.CopyTo(buffer3, 0);
            array = new byte[Marshal.SizeOf(bitmapinfoheader)];
            IntPtr ptr8 = LocalAlloc(0x40, Marshal.SizeOf(bitmapinfoheader));
            Marshal.StructureToPtr(bitmapinfoheader2, ptr8, false);
            Marshal.Copy(ptr8, array, 0, Marshal.SizeOf(bitmapinfoheader));
            LocalFree(ptr8);
            array.CopyTo(buffer3, index);
            destination.CopyTo(buffer3, 0x36);
            FileStream stream = new FileStream(FileName, FileMode.Create);
            stream.Write(buffer3, 0, buffer3.Length);
            stream.Flush();
            stream.Close();
            buffer3 = null;
            DeleteObject(SelectObject(hDC, ptr7));
            DeleteDC(hDC);
            ReleaseDC(dC);
        }

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
