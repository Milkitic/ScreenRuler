using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenRuler
{
    public partial class FrmRuler : Form
    {
        // 绘制相关
        Bitmap bitmap;
        Graphics graphic;
        GraphicsPath gPath;
        Rectangle recMain;
        Matrix matrix;

        Rectangle recScreen;
        PointF origin;  // 原点
        Point cursorPoint;  // 临时指针坐标

        bool pressAlt = false, pressShift = false, pressCtrl = false;
        bool isRotating = false;

        Unit unit;
        Style style = new Style(StyleKind.White);

        int width = 600, height = 60; // 控制recMain的变量
        int left, top;
        int resWidth = 1600, resHeight = 900;  // 用于计算scaled px

        float linear_degree = 0;
        float linear_eachWidth = 5;

        #region 控制窗体移动相关
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
        #endregion

        public FrmRuler()
        {
            InitializeComponent();

            // 事件绑定
            Canvas.MouseWheel += new MouseEventHandler(Canvas_MouseWheel);
            Canvas.MouseClick += new MouseEventHandler(Canvas_MouseClick);
            Canvas.KeyDown += new KeyEventHandler(Canvas_KeyDown);
            Canvas.KeyUp += new KeyEventHandler(Canvas_KeyUp);
        }

        private void PreInit()
        {
            this.FormBorderStyle = FormBorderStyle.None;  // 无边框，必须在计算之前
            this.TopMost = true;
            this.ActiveControl = Canvas;
            Canvas.BackColor = style.GetBack();

            recScreen = Screen.GetWorkingArea(this);

            Rectangle rect = Screen.GetWorkingArea(this);

            int margin = (int)((Math.Sqrt(width * width + height * height) - (width > height ? width : height)) / 2) + 5;  // 计算一个完美切边
            if (width > height)  // left和top是确定一下画在中心
            {
                left = margin;
                top = (int)(left + width / 2f - height / 2f);
            }
            else
            {
                top = margin;
                left = (int)(top + height / 2f - width / 2f);
            }

            RePos();
            DrawRuler();
        }

        private void RePos()
        {
            this.Width = left + width + left;  // 正方形窗口
            this.Height = this.Width;
            this.Top = (int)(recScreen.Height / 2f - this.Height / 2f);  // 居中显示
            this.Left = (int)(recScreen.Width / 2f - this.Width / 2f);

            top = (int)(this.Height / 2f - height / 2f);

            recMain = new Rectangle(left, top, width, height);
        }

        private void DrawRuler()
        {
            // 小心爆了内存
            if (gPath != null) gPath.Dispose();
            if (bitmap != null) bitmap.Dispose();
            if (graphic != null) graphic.Dispose();
            if (matrix != null) matrix.Dispose();

            // 坐标变换
            matrix = new Matrix();
            matrix.Translate(width / 2f + left, height / 2f + top);
            matrix.Rotate(linear_degree);
            matrix.Translate(-width / 2f - left, -height / 2f - top);

            // Form Region从此而来
            gPath = new GraphicsPath();
            gPath.AddRectangle(recMain);
            gPath.Transform(matrix);  // 应用

            bitmap = new Bitmap(Canvas.Width, Canvas.Height);
            graphic = Graphics.FromImage(bitmap);
            graphic.SmoothingMode = SmoothingMode.HighQuality;  // 进行一系列的抗锯齿
            graphic.CompositingQuality = CompositingQuality.HighQuality;
            graphic.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            graphic.Transform = matrix;  // 应用，与GraphicPath不同是，绘制是在变换后的坐标进行

            LinearRuler();

            Canvas.Image = bitmap;
            this.Region = new Region(gPath);  // 去边缘
        }

        private void LinearRuler()
        {
            float line = 0;
            int margin = 10;

            //string a = $" ({Left + left},{Top + top}) ({width}*{height})";
            origin = new PointF(Left + left + width / 2f, Top + top + height / 2f);
            string a = $"  {origin.X},{origin.Y}  {width}px  {Math.Round(linear_degree, 2)}°";
            if (unit == Unit.Scale)
                graphic.DrawString("Pixel (scaled)" + a, new Font("Arial", 10), new SolidBrush(style.GetFore()), left + margin / 2f, top + height - 20);
            else if (unit == Unit.Pixel)
                graphic.DrawString("Pixel" + a, new Font("Arial", 10), new SolidBrush(style.GetFore()), left + margin / 2f, top + height - 20);
            else if (unit == Unit.Graduation)
                graphic.DrawString("Graduation" + a, new Font("Arial", 10), new SolidBrush(style.GetFore()), left + margin / 2f, top + height - 20);

            while (margin + linear_eachWidth * line < width - margin)
            {
                int line_height = 7;
                float x = left + margin + line * linear_eachWidth;
                float lineWidth = 1;
                string px = "";
                if (unit == Unit.Scale)
                    px = Math.Round((line * linear_eachWidth / (resHeight * (701 / 900f) / 384)), 2).ToString();
                else if (unit == Unit.Pixel)
                    px = Math.Round((line * linear_eachWidth), 2).ToString();
                else if (unit == Unit.Graduation)
                    px = Math.Round((line), 2).ToString();

                Font font;
                if (line % 10 == 0)
                {
                    line_height = 15;
                    font = new Font("Arial", 10);
                    SizeF sizeF = graphic.MeasureString(px, font);
                    graphic.DrawString(px, font, new SolidBrush(style.GetFore()), x - sizeF.Width / 2, top + 15);
                }
                else if (line % 5 == 0)
                {
                    line_height = 11;
                    font = new Font("Arial", 9);
                    SizeF sizeF = graphic.MeasureString(px, font);
                    if (linear_eachWidth > 10) graphic.DrawString(px, font, new SolidBrush(style.GetLightFore()), x - sizeF.Width / 2, top + 15);
                }
                else
                {
                    font = new Font("Arial", 8);
                    SizeF sizeF = graphic.MeasureString(px, font);
                    if (linear_eachWidth > 45) graphic.DrawString(px, font, new SolidBrush(style.GetUltraLightFore()), x - sizeF.Width / 2, top + 15);
                    lineWidth = 0.5f;
                }
                graphic.DrawLine(new Pen(style.GetFore(), lineWidth), x, top, x, top + line_height);
                line++;
            }
            graphic.DrawRectangle(new Pen(style.GetFormBorder(), 3), recMain.Left, recMain.Top, recMain.Width-1, recMain.Height-1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PreInit();
        }

        private void Form1_Move(object sender, EventArgs e)
        {
            DrawRuler();
        }

        private void Canvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                cursorPoint = Canvas.PointToClient(Cursor.Position);
                tsmEdit.Show(Canvas, cursorPoint);
            }
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (pressCtrl && e.Button == MouseButtons.Left)
            {
                cursorPoint = Cursor.Position;
                isRotating = true;
            }
            else if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
            }

        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            isRotating = false;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRotating)
            {
                Point pt = Cursor.Position;
                if (cursorPoint.X < origin.X)
                {
                    if (pressAlt)
                        linear_degree -= (pt.Y - cursorPoint.Y) / 20f;
                    else
                        linear_degree -= pt.Y - cursorPoint.Y;
                }
                else
                {
                    if (pressAlt)
                        linear_degree += (pt.Y - cursorPoint.Y) / 20f;
                    else
                        linear_degree += pt.Y - cursorPoint.Y;
                }
                cursorPoint.Y = pt.Y;
                DrawRuler();
            }
        }

        private void Canvas_MouseWheel(object sender, MouseEventArgs e)
        {
            float interv = 60f;
            double delta = e.Delta;
            if (pressCtrl)
            {
                var tmpWitdh = this.Width;

                if ((width >= 300 || delta > 0) && (width <= recScreen.Width - left || delta < 0)) width += (int)(delta / 2f);
                RePos();
            }
            else
            {
                if (pressAlt) delta = delta / Math.Abs(delta) * Math.Pow(linear_eachWidth, 0.3);
                else if (pressShift) delta = delta / Math.Abs(delta) * Math.Pow(linear_eachWidth, 1.6);
                else delta = delta / Math.Abs(delta) * Math.Pow(linear_eachWidth, 1.3);

                if (linear_eachWidth + delta / interv > 3 && linear_eachWidth + delta / interv < 160) linear_eachWidth += (float)(delta / interv);
            }
            DrawRuler();
        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Menu) pressAlt = true;
            else if (e.KeyCode == Keys.ShiftKey) pressShift = true;
            else if (e.KeyCode == Keys.ControlKey) pressCtrl = true;
            else if (e.KeyCode == Keys.Left) this.Left--;
            else if (e.KeyCode == Keys.Right) this.Left++;
            else if (e.KeyCode == Keys.Up) this.Top--;
            else if (e.KeyCode == Keys.Down) this.Top++;
        }

        private void Canvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Menu) pressAlt = false;
            else if (e.KeyCode == Keys.ShiftKey) pressShift = false;
            else if (e.KeyCode == Keys.ControlKey) pressCtrl = false;
        }

        private void tsmi_pixel_Click(object sender, EventArgs e)
        {
            unit = Unit.Pixel;
            tsmi_pixel.Checked = true;
            tsmi_pixel2.Checked = false;
            tsmi_count.Checked = false;
            DrawRuler();
        }

        private void tsmi_pixel2_Click(object sender, EventArgs e)
        {
            unit = Unit.Scale;
            tsmi_pixel.Checked = false;
            tsmi_pixel2.Checked = true;
            tsmi_count.Checked = false;
            DrawRuler();
        }

        private void tsmi_count_Click(object sender, EventArgs e)
        {
            unit = Unit.Graduation;
            tsmi_pixel.Checked = false;
            tsmi_pixel2.Checked = false;
            tsmi_count.Checked = true;
            DrawRuler();
        }
    }
}
