using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace System.Drawing
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAP
    {
        public int bmType;
        public int bmWidth;
        public int bmHeight;
        public int bmWidthBytes;
        public ushort bmPlanes;
        public ushort bmBitsPixel;
        public int bmBits;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPFILEHEADER
    {
        public ushort bfType;
        public uint bfSize;
        public ushort bfReserved1;
        public ushort bfReserved2;
        public uint bfOffBits;
        private BITMAPFILEHEADER(ushort type, uint size, uint offset)
        {
            this.bfReserved1 = 0;
            this.bfReserved2 = 1;
            this.bfType = type;
            this.bfSize = size;
            this.bfOffBits = offset;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPINFOHEADER
    {
        public uint biSize;
        public int biWidth;
        public int biHeight;
        public ushort biPlanes;
        public ushort biBitCount;
        public uint biCompression;
        public uint biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public uint biClrUsed;
        public uint biClrImportant;
    }

    public class LOGFONT
    {
        public byte lfCharSet;
        public byte lfClipPrecision;
        public int lfEscapement;
        public int lfHeight;
        public byte lfItalic;
        public int lfOrientation;
        public byte lfOutPrecision;
        public byte lfPitchAndFamily;
        public byte lfQuality;
        public byte lfStrikeOut;
        public byte lfUnderline;
        public int lfWeight;
        public int lfWidth;
    }

    public enum PenStyle
    {
        PS_DASH = 1,
        PS_NULL = 5,
        PS_SOLID = 0
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
        public POINT(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    public enum ROP
    {
        R2_BLACK = 1,
        R2_COPYPEN = 13,
        R2_LAST = 0x10,
        R2_MASKNOTPEN = 3,
        R2_MASKPEN = 9,
        R2_MASKPENNOT = 5,
        R2_MERGENOTPEN = 12,
        R2_MERGEPEN = 15,
        R2_MERGEPENNOT = 14,
        R2_NOP = 11,
        R2_NOT = 6,
        R2_NOTCOPYPEN = 4,
        R2_NOTMASKPEN = 8,
        R2_NOTMERGEPEN = 2,
        R2_NOTXORPEN = 10,
        R2_WHITE = 0x10,
        R2_XORPEN = 7
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        public int width;
        public int height;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TEXTMETRIC
    {
        public int tmHeight;
        public int tmAscent;
        public int tmDescent;
        public int tmInternalLeading;
        public int tmExternalLeading;
        public int tmAveCharWidth;
        public int tmMaxCharWidth;
        public int tmWeight;
        public int tmOverhang;
        public int tmDigitizedAspectX;
        public int tmDigitizedAspectY;
        public char tmFirstChar;
        public char tmLastChar;
        public char tmDefaultChar;
        public char tmBreakChar;
        public byte tmItalic;
        public byte tmUnderlined;
        public byte tmStruckOut;
        public byte tmPitchAndFamily;
        public byte tmCharSet;
        private TEXTMETRIC(int tmHeight, int tmAscent, int tmDescent, int tmInternalLeading, int tmExternalLeading, int tmAveCharWidth, int tmMaxCharWidth, int tmWeight, int tmOverhang, int tmDigitizedAspectX, int tmDigitizedAspectY, char tmFirstChar, char tmLastChar, char tmDefaultChar, char tmBreakChar, byte tmItalic, byte tmUnderlined, byte tmStruckOut, byte tmPitchAndFamily, byte tmCharSet)
        {
            this.tmHeight = tmHeight;
            this.tmAscent = tmAscent;
            this.tmDescent = tmDescent;
            this.tmInternalLeading = tmInternalLeading;
            this.tmExternalLeading = tmExternalLeading;
            this.tmAveCharWidth = tmAveCharWidth;
            this.tmMaxCharWidth = tmMaxCharWidth;
            this.tmWeight = tmWeight;
            this.tmOverhang = tmOverhang;
            this.tmDigitizedAspectX = tmDigitizedAspectX;
            this.tmDigitizedAspectY = tmDigitizedAspectY;
            this.tmFirstChar = tmFirstChar;
            this.tmLastChar = tmLastChar;
            this.tmDefaultChar = tmDefaultChar;
            this.tmBreakChar = tmBreakChar;
            this.tmItalic = tmItalic;
            this.tmUnderlined = tmUnderlined;
            this.tmStruckOut = tmStruckOut;
            this.tmPitchAndFamily = tmPitchAndFamily;
            this.tmCharSet = tmCharSet;
        }
    }



    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BlendFunction
    {
        public byte BlendOp;
        public byte BlendFlags;
        public byte SourceConstantAlpha;
        public byte AlphaFormat;
    }


}
