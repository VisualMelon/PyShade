/*
 * Created by SharpDevelop.
 * User: Freddie
 * Date: 20/12/2013
 * Time: 16:36
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace PyShade
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.inImg = new System.Windows.Forms.PictureBox();
			this.outImg = new System.Windows.Forms.PictureBox();
			this.codeF = new System.Windows.Forms.TextBox();
			this.reportF = new System.Windows.Forms.RichTextBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.processToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.inImg)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.outImg)).BeginInit();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 24);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.codeF);
			this.splitContainer1.Panel2.Controls.Add(this.reportF);
			this.splitContainer1.Size = new System.Drawing.Size(439, 406);
			this.splitContainer1.SplitterDistance = 142;
			this.splitContainer1.TabIndex = 2;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.inImg);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.outImg);
			this.splitContainer2.Size = new System.Drawing.Size(439, 142);
			this.splitContainer2.SplitterDistance = 211;
			this.splitContainer2.TabIndex = 0;
			// 
			// inImg
			// 
			this.inImg.BackColor = System.Drawing.Color.White;
			this.inImg.Dock = System.Windows.Forms.DockStyle.Fill;
			this.inImg.Location = new System.Drawing.Point(0, 0);
			this.inImg.Name = "inImg";
			this.inImg.Size = new System.Drawing.Size(211, 142);
			this.inImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.inImg.TabIndex = 0;
			this.inImg.TabStop = false;
			this.inImg.Click += new System.EventHandler(this.InImgClick);
			// 
			// outImg
			// 
			this.outImg.BackColor = System.Drawing.Color.White;
			this.outImg.Dock = System.Windows.Forms.DockStyle.Fill;
			this.outImg.Location = new System.Drawing.Point(0, 0);
			this.outImg.Name = "outImg";
			this.outImg.Size = new System.Drawing.Size(224, 142);
			this.outImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.outImg.TabIndex = 1;
			this.outImg.TabStop = false;
			this.outImg.Click += new System.EventHandler(this.OutImgClick);
			// 
			// codeF
			// 
			this.codeF.Dock = System.Windows.Forms.DockStyle.Fill;
			this.codeF.Location = new System.Drawing.Point(0, 0);
			this.codeF.Multiline = true;
			this.codeF.Name = "codeF";
			this.codeF.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.codeF.Size = new System.Drawing.Size(439, 164);
			this.codeF.TabIndex = 0;
			this.codeF.Text = resources.GetString("codeF.Text");
			this.codeF.WordWrap = false;
			// 
			// reportF
			// 
			this.reportF.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.reportF.Location = new System.Drawing.Point(0, 164);
			this.reportF.Name = "reportF";
			this.reportF.Size = new System.Drawing.Size(439, 96);
			this.reportF.TabIndex = 1;
			this.reportF.Text = "";
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.processToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(439, 24);
			this.menuStrip1.TabIndex = 3;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// processToolStripMenuItem
			// 
			this.processToolStripMenuItem.Name = "processToolStripMenuItem";
			this.processToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
			this.processToolStripMenuItem.Text = "Process";
			this.processToolStripMenuItem.Click += new System.EventHandler(this.ProcessToolStripMenuItemClick);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.DefaultExt = "png";
			this.saveFileDialog1.Filter = "PNGs|*.png";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(439, 430);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.menuStrip1);
			this.Name = "MainForm";
			this.Text = "PyBMP";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.inImg)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.outImg)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.RichTextBox reportF;
		private System.Windows.Forms.TextBox codeF;
		private System.Windows.Forms.ToolStripMenuItem processToolStripMenuItem;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.PictureBox outImg;
		private System.Windows.Forms.PictureBox inImg;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.SplitContainer splitContainer1;
	}
}
