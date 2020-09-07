namespace Demo
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.uvPV = new OxyPlot.WindowsForms.PlotView();
            this.nmrPV = new OxyPlot.WindowsForms.PlotView();
            this.eprPV = new OxyPlot.WindowsForms.PlotView();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.eprPV, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.uvPV, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.nmrPV, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // uvPV
            // 
            this.uvPV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uvPV.Location = new System.Drawing.Point(403, 228);
            this.uvPV.Name = "uvPV";
            this.uvPV.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.uvPV.Size = new System.Drawing.Size(394, 219);
            this.uvPV.TabIndex = 0;
            this.uvPV.Text = "plotView1";
            this.uvPV.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.uvPV.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.uvPV.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // nmrPV
            // 
            this.nmrPV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nmrPV.Location = new System.Drawing.Point(3, 3);
            this.nmrPV.Name = "nmrPV";
            this.nmrPV.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.nmrPV.Size = new System.Drawing.Size(394, 219);
            this.nmrPV.TabIndex = 0;
            this.nmrPV.Text = "nmrPV";
            this.nmrPV.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.nmrPV.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.nmrPV.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // eprPV
            // 
            this.eprPV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eprPV.Location = new System.Drawing.Point(3, 228);
            this.eprPV.Name = "eprPV";
            this.eprPV.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.eprPV.Size = new System.Drawing.Size(394, 219);
            this.eprPV.TabIndex = 0;
            this.eprPV.Text = "plotView1";
            this.eprPV.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.eprPV.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.eprPV.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private OxyPlot.WindowsForms.PlotView nmrPV;
        private OxyPlot.WindowsForms.PlotView uvPV;
        private OxyPlot.WindowsForms.PlotView eprPV;
    }
}

