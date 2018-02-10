using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenRuler
{
    /// <summary>
    /// 用Win32绘制
    /// </summary>
    class PngFormGenerator
    {
        Form PngWinForm;

        public PngFormGenerator(Form PngWinForm)
        {
            this.PngWinForm = PngWinForm;
        }

        public enum Bool
        {
            False = 0,
            True
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public Int32 x;
            public Int32 y;
            public Point(Int32 x, Int32 y)
            {
                this.x = x; this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Size
        {
            public Int32 cx;
            public Int32 cy;
            public Size(Int32 cx, Int32 cy)
            {
                this.cx = cx; this.cy = cy;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct ARGB
        {
            public byte Blue;
            public byte Green;
            public byte Red;
            public byte Alpha;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        public const Int32 ULW_COLORKEY = 0x00000001;
        public const Int32 ULW_ALPHA = 0x00000002;
        public const Int32 ULW_OPAQUE = 0x00000004;

        public const byte AC_SRC_OVER = 0x00;
        public const byte AC_SRC_ALPHA = 0x01;

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool DeleteDC(IntPtr hdc);
        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// 使用指定的图像来生成PNG窗体。
        /// </summary>
        /// <param name="bitmap"></param>
        public void SetBitmap(Bitmap bitmap)
        {
            SetBitmap(bitmap, 255);
        }

        /// <summary>
        /// 使用构造函数中的窗体的背景图像生成 PNG 窗体。
        /// </summary>
        public void SetBitmap()
        {
            if (PngWinForm.BackgroundImage != null)
                SetBitmap((Bitmap)PngWinForm.BackgroundImage, 255);
            else
                throw new Exception("窗体没有可用的背景图像");
        }

        /// <summary>
        /// 设置图象
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="opacity"></param>
        public void SetBitmap(Bitmap bitmap, byte opacity)
        {
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                throw new ApplicationException("The bitmap must be 32ppp with alpha-channel.");

            // The ideia of this is very simple,
            // 1. Create a compatible DC with screen;
            // 2. Select the bitmap with 32bpp with alpha-channel in the compatible DC;
            // 3. Call the UpdateLayeredWindow.

            IntPtr screenDc = PngFormGenerator.GetDC(IntPtr.Zero);
            IntPtr memDc = PngFormGenerator.CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr oldBitmap = IntPtr.Zero;
            try
            {
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));  // grab a GDI handle from this GDI+ bitmap
                oldBitmap = PngFormGenerator.SelectObject(memDc, hBitmap);
                PngFormGenerator.Size size = new PngFormGenerator.Size(bitmap.Width, bitmap.Height);
                PngFormGenerator.Point pointSource = new PngFormGenerator.Point(0, 0);
                PngFormGenerator.Point topPos = new PngFormGenerator.Point(PngWinForm.Left, PngWinForm.Top);
                PngFormGenerator.BLENDFUNCTION blend = new PngFormGenerator.BLENDFUNCTION();
                blend.BlendOp = PngFormGenerator.AC_SRC_OVER;
                blend.BlendFlags = 0;
                blend.SourceConstantAlpha = opacity;
                blend.AlphaFormat = PngFormGenerator.AC_SRC_ALPHA;
                PngFormGenerator.UpdateLayeredWindow(PngWinForm.Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, PngFormGenerator.ULW_ALPHA);
            }
            finally
            {
                PngFormGenerator.ReleaseDC(IntPtr.Zero, screenDc);

                if (hBitmap != IntPtr.Zero)
                {
                    PngFormGenerator.SelectObject(memDc, oldBitmap);

                    //Windows.DeleteObject(hBitmap); // The documentation says that we have to use the Windows.DeleteObject... but since there is no such method I use the normal DeleteObject from Win32 GDI and it's working fine without any resource leak.

                    PngFormGenerator.DeleteObject(hBitmap);
                }
                PngFormGenerator.DeleteDC(memDc);
            }
            bitmap.Dispose();
        }
    }

}
