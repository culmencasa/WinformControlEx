﻿using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace System.Drawing
{
    public class ImageTool {

		public static byte[] ToArray(Image img) {
			if (img == null)
				return new byte[0];

            lock (img)
            {
                return img is Metafile ? GetMetafileArray((Metafile)img) : ToArray(img, img.RawFormat);
            }
		}
		public static byte[] ToArray(Image img, ImageFormat format) {

            return format == ImageFormat.Wmf ?
                GetWmfImageArray(img) : ToArrayCore(img, format);
		}
		static byte[] ToArrayCore(Image img, ImageFormat format) {
			MemoryStream stream = new MemoryStream();
			try {
				SaveImage(img, stream, format);
				return stream.ToArray();
			}
            catch
            {
                // 复制的方式
                using (MemoryStream ms = new MemoryStream())
                {
                    Bitmap bmp = new Bitmap(img);
                    bmp.Save(ms, img.RawFormat);

                    return ms.ToArray();
                }
			}
			finally {
				stream.Close();
				((IDisposable)stream).Dispose();
			}
		}

		public static void SaveImage(Image img, Stream stream, ImageFormat format) {

			ImageCodecInfo info = FindEncoder(format);
			if (info == null)
				info = FindEncoder(ImageFormat.Png);
			lock (img) {
				img.Save(stream, info, null);
			}
		}

		public static Image FromArray(byte[] buffer) {
			if (buffer == null)
				return null;
			Image img = null;

			if (img == null && buffer.Length > 78) {
				if (buffer[0] == 0x15 && buffer[1] == 0x1c)  
					img = FromArrayCore(buffer, 78);
			}
			if (img == null)
				img = FromArrayCore(buffer, 0);
			return img;
		}
		static Image FromArrayCore(byte[] buffer, int offset) {
			if (buffer == null)
				return null;
			try {
				MemoryStream stream = new MemoryStream(buffer, offset, (int)buffer.Length - offset);
				return Image.FromStream(stream);
			}
			catch { return null; }
		}


		public static Bitmap LoadBitmap(string file)
		{
			var bytes = File.ReadAllBytes(file);
			var ms = new MemoryStream(bytes);
			var result = new Bitmap(Image.FromStream(ms));

			return result;
		}

		enum EmfToWmfBitsFlags {
			EmfToWmfBitsFlagsDefault = 0x00000000,
			EmfToWmfBitsFlagsEmbedEmf = 0x00000001,
			EmfToWmfBitsFlagsIncludePlaceable = 0x00000002,
			EmfToWmfBitsFlagsNoXORClip = 0x00000004
		};
		const int MM_ANISOTROPIC = 8;

		static byte[] GetWmfImageArray(Image image) {
			MemoryStream stream = null;
			Metafile metaFile = null;
			IntPtr hemf = IntPtr.Zero;
			try {
				stream = new MemoryStream();
				metaFile = MetafileCreator.CreateInstance(stream);
				using (Graphics graphics = Graphics.FromImage(metaFile)) {
					graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height));
				}
				hemf = metaFile.GetHenhmetafile();
				uint bufferSize = GdipEmfToWmfBits(hemf, 0, null, MM_ANISOTROPIC, EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);
				byte[] buffer = new byte[bufferSize];
				GdipEmfToWmfBits(hemf, bufferSize, buffer, MM_ANISOTROPIC, EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);
				return buffer;
			}
			finally {
				DeleteEnhMetaFile(hemf);
				metaFile.Dispose();
				stream.Close();
			}
		}

		[System.Runtime.InteropServices.DllImportAttribute("gdiplus.dll"), System.Security.SuppressUnmanagedCodeSecurity]
		static extern uint GdipEmfToWmfBits(IntPtr hemf, uint bufferSize, byte[] buffer, int mappingMode, EmfToWmfBitsFlags flags);

		[System.Runtime.InteropServices.DllImportAttribute("gdi32.dll"), System.Security.SuppressUnmanagedCodeSecurity]
		static extern bool DeleteEnhMetaFile(IntPtr hemf);

		static ImageCodecInfo FindEncoder(ImageFormat format) {
			ImageCodecInfo[] infos = ImageCodecInfo.GetImageEncoders();
			for (int i = 0; i < infos.Length; i++) {
				if (infos[i].FormatID.Equals(format.Guid)) {
					return infos[i];
				}
			}
			return null;
		}

		static byte[] GetMetafileArray(Metafile metafile) {
			using (MemoryStream metafileStream = new MemoryStream()) {
				using (Graphics offscreenDC = Graphics.FromHwndInternal(IntPtr.Zero)) {
					IntPtr imagePtr = offscreenDC.GetHdc();
					try {
						using (Metafile metafile2 = new Metafile(metafileStream, imagePtr, EmfType.EmfPlusOnly)) {
							using (Graphics g = Graphics.FromImage(metafile2)) {
								g.DrawImage(metafile, 0, 0);
							}
							return metafileStream.ToArray();
						}
					}
					finally {
						offscreenDC.ReleaseHdc(imagePtr);
					}
				}
			}
		}



#if COMPILE_NET40
        private static readonly Type CursorType = typeof(Cursor);
#else
    private const uint IMAGE_CURSOR = 2;
    private const uint LR_LOADFROMFILE = 0x00000010;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr LoadImage(IntPtr hinst, string lpszName, uint uType, int cx, int cy, uint fuLoad);
#endif

        /// <summary>
        /// 加载自定义光标（避免加载成黑色）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="ComponentModel.Win32Exception"></exception>
        public static Cursor LoadCustomCursor(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Cursor file not found.", path);
            }



#if COMPILE_NET40
            IntPtr handle = Win32.LoadCursorFromFile(path);
            if (handle == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            var cursor = new Cursor(handle);
            var fi = CursorType.GetField("ownHandle", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fi != null)
            {
                fi.SetValue(cursor, true);
            }

            return cursor;
#else
            IntPtr handle = LoadImage(IntPtr.Zero, path, IMAGE_CURSOR, 0, 0, LR_LOADFROMFILE);
			if (handle == IntPtr.Zero)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}

			return new Cursor(handle);
#endif
        }
    }


    public class MetafileCreator
    {
        public static Metafile CreateInstance(RectangleF frameRect, MetafileFrameUnit frameUnit)
        {
            using (Graphics gr = GraphicsExtension.CreateGraphics())
            {
                IntPtr hdc = gr.GetHdc();
                Metafile instance = new Metafile(hdc, frameRect, frameUnit);
                gr.ReleaseHdc(hdc);
                return instance;
            }
        }
        public static Metafile CreateInstance(Stream stream, int width, int height, MetafileFrameUnit frameUnit)
        {
            using (Graphics gr = GraphicsExtension.CreateGraphics())
            {
                IntPtr hdc = gr.GetHdc();
                Metafile instance = new Metafile(stream, hdc, new Rectangle(0, 0, width, height), frameUnit);
                gr.ReleaseHdc(hdc);
                return instance;
            }
        }
        public static Metafile CreateInstance(Stream stream, EmfType type)
        {
            using (Graphics gr = GraphicsExtension.CreateGraphics())
            {
                IntPtr hdc = gr.GetHdc();
                Metafile instance = new Metafile(stream, hdc, type);
                gr.ReleaseHdc(hdc);
                return instance;
            }
        }
        public static Metafile CreateInstance(Stream stream)
        {
            using (Graphics gr = GraphicsExtension.CreateGraphics())
            {
                IntPtr hdc = gr.GetHdc();
                Metafile instance = new Metafile(stream, hdc);
                gr.ReleaseHdc(hdc);
                return instance;
            }
        }
    }
}