namespace ScreenRuler
{
    partial class FrmRuler
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Canvas = new System.Windows.Forms.PictureBox();
            this.tsmEdit = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiUnit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_pixel = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_pixel2 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_count = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).BeginInit();
            this.tsmEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // Canvas
            // 
            this.Canvas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Canvas.Location = new System.Drawing.Point(0, 0);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(534, 533);
            this.Canvas.TabIndex = 4;
            this.Canvas.TabStop = false;
            this.Canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseDown);
            this.Canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
            this.Canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseUp);
            // 
            // tsmEdit
            // 
            this.tsmEdit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiUnit});
            this.tsmEdit.Name = "tsmEdit";
            this.tsmEdit.Size = new System.Drawing.Size(100, 26);
            this.tsmEdit.Text = "Edit";
            // 
            // tsmiUnit
            // 
            this.tsmiUnit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_pixel,
            this.tsmi_pixel2,
            this.tsmi_count});
            this.tsmiUnit.Name = "tsmiUnit";
            this.tsmiUnit.Size = new System.Drawing.Size(99, 22);
            this.tsmiUnit.Text = "Unit";
            // 
            // tsmi_pixel
            // 
            this.tsmi_pixel.Name = "tsmi_pixel";
            this.tsmi_pixel.Size = new System.Drawing.Size(151, 22);
            this.tsmi_pixel.Text = "Pixel";
            this.tsmi_pixel.Click += new System.EventHandler(this.tsmi_pixel_Click);
            // 
            // tsmi_pixel2
            // 
            this.tsmi_pixel2.Name = "tsmi_pixel2";
            this.tsmi_pixel2.Size = new System.Drawing.Size(151, 22);
            this.tsmi_pixel2.Text = "Pixel (scaled)";
            this.tsmi_pixel2.Click += new System.EventHandler(this.tsmi_pixel2_Click);
            // 
            // tsmi_count
            // 
            this.tsmi_count.Checked = true;
            this.tsmi_count.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsmi_count.Name = "tsmi_count";
            this.tsmi_count.Size = new System.Drawing.Size(151, 22);
            this.tsmi_count.Text = "Graduation";
            this.tsmi_count.Click += new System.EventHandler(this.tsmi_count_Click);
            // 
            // FrmRuler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(534, 533);
            this.Controls.Add(this.Canvas);
            this.DoubleBuffered = true;
            this.Name = "FrmRuler";
            this.Opacity = 0.01D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Move += new System.EventHandler(this.Form1_Move);
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).EndInit();
            this.tsmEdit.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox Canvas;
        private System.Windows.Forms.ContextMenuStrip tsmEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmiUnit;
        private System.Windows.Forms.ToolStripMenuItem tsmi_pixel;
        private System.Windows.Forms.ToolStripMenuItem tsmi_pixel2;
        private System.Windows.Forms.ToolStripMenuItem tsmi_count;
    }
}

