/*
 * Created by SharpDevelop.
 * User: Freddie
 * Date: 20/12/2013
 * Time: 16:36
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using sdi = System.Drawing.Imaging;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using py = IronPython;
using sing = Microsoft.Scripting;

using System.Runtime.InteropServices;

namespace PyBMP
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		[StructLayout(LayoutKind.Explicit)]
		public struct colour
		{
			[FieldOffset(0)]
			public byte b;
			[FieldOffset(1)]
			public byte g;
			[FieldOffset(2)]
			public byte r;
			[FieldOffset(3)]
			public byte a;
			
			[FieldOffset(0)]
			public int argb;
			
			public colour(int argbN) : this()
			{
				argb = argbN;
			}
			
			public colour(byte aN, byte rN, byte gN, byte bN) : this()
			{
				a = aN;
				r = rN;
				g = gN;
				b = bN;
			}
			
			public unsafe colour(byte* s) : this()
			{
				a = *(s + 0);
				r = *(s + 1);
				g = *(s + 2);
				b = *(s + 3);
			}
			
			public unsafe colour(int* s) : this()
			{
				argb = *s;
			}
			
			public colour clone(int aN, int rN, int gN, int bN)
			{
				if (aN < 0)
					aN = a;
				if (rN < 0)
					rN = r;
				if (gN < 0)
					gN = g;
				if (bN < 0)
					bN = b;
				
				return new colour(util.clampToByte(aN),
				                  util.clampToByte(rN),
				                  util.clampToByte(gN),
				                  util.clampToByte(bN));
			}
			
			public colour mul(float ac, float rc, float gc, float bc)
			{
				return new colour(util.clampToByte(ac * (float)a),
				                  util.clampToByte(rc * (float)r),
				                  util.clampToByte(gc * (float)g),
				                  util.clampToByte(bc * (float)b));
			}
			
			public colour add(int ac, int rc, int gc, int bc)
			{
				return new colour(util.clampToByte(ac + (int)a),
				                  util.clampToByte(rc + (int)r),
				                  util.clampToByte(gc + (int)g),
				                  util.clampToByte(bc + (int)b));
			}
		}
		
		public class util
		{
			public delegate int getFunc(int num, int top);
			
			public static int wrap(int num, int top)
			{
				return num % top;
			}
			
			public static int mirror(int num, int top)
			{
				while (true)
				{
					if (num < 0)
						num = -1 - num;
					else if (num > top)
						num = (top * 2 + 1) - num;
					else
						return num;
				}
			}
			
			public static int clamp(int num, int top)
			{
				if (num < 0)
					return 0;
				else if (num > top)
					return top;
				else
					return num;
			}
			
			public static byte clampToByte(int num)
			{
				if (num < 0)
					return 0;
				else if (num > 255)
					return 255;
				else
					return (byte)num;
			}
			
			public static byte clampToByte(float num)
			{
				return clampToByte((int)num);
			}
		}
		
		public unsafe abstract class surface
		{
			int widthL;
			int heightL;
			
			public int width
			{
				get { return widthL; }
				protected set { widthL = value; }
			}
			
			public int height
			{
				get { return heightL; }
				protected set { heightL = value; }
			}
			
			public abstract void close();
			public abstract colour getCol(int x, int y);
			public abstract void setCol(int x, int y, colour c);
			
			public colour getCol(int x, int y, util.getFunc gf)
			{
				return getCol(gf.Invoke(x, width - 1), gf.Invoke(y, height - 1));
			}
			
			public void setCol(int x, int y, colour c, util.getFunc gf)
			{
				setCol(gf.Invoke(x, width - 1), gf.Invoke(y, height - 1), c);
			}
			
			public colour getCol_wrap(int x, int y)
			{
				return getCol(x, y, util.wrap);
			}
			
			public void setCol_wrap(int x, int y, colour c)
			{
				setCol(x, y, c, util.wrap);
			}
			
			public colour getCol_mirror(int x, int y)
			{
				return getCol(x, y, util.mirror);
			}
			
			public void setCol_mirror(int x, int y, colour c)
			{
				setCol(x, y, c, util.mirror);
			}
			
			public colour getCol_clamp(int x, int y)
			{
				return getCol(x, y, util.clamp);
			}
			
			public void setCol_clamp(int x, int y, colour c)
			{
				setCol(x, y, c, util.clamp);
			}
			
			public virtual unsafe void save(string fileName)
			{
				Bitmap bmp = new Bitmap(width, height);
				sdi.BitmapData bmpDat = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), sdi.ImageLockMode.WriteOnly, sdi.PixelFormat.Format32bppArgb);
				
				int* s = (int*)bmpDat.Scan0;
				
				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						*s = getCol(x, y).argb;
						s++;
					}
				}
				
				bmp.UnlockBits(bmpDat);
				bmp.Save(fileName, sdi.ImageFormat.Png);
				bmp.Dispose();
			}
		}
		
		public unsafe class bmpSurface : surface
		{
			private Bitmap bmp;
			private sdi.BitmapData bmpDat;
			
			public bmpSurface(string fileName)
			{
				Bitmap bmpN = (Bitmap)Bitmap.FromFile(fileName);
				init(bmpN);
				bmpN.Dispose();
			}
			
			public bmpSurface(Bitmap bmpN)
			{
				init(bmpN);
			}
			
			void init(Bitmap bmpN)
			{
				bmp = ((Bitmap)bmpN.Clone()).Clone(new Rectangle(0, 0, bmpN.Width, bmpN.Height), sdi.PixelFormat.Format32bppArgb);
				
				bmpDat = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), sdi.ImageLockMode.ReadWrite, sdi.PixelFormat.Format32bppArgb);
				width = bmpDat.Width;
				height = bmpDat.Height;
			}
			
			public override void close()
			{
				bmp.UnlockBits(bmpDat);
				bmp.Dispose();
			}
			
			public override colour getCol(int x, int y)
			{
				int* s = (int*)bmpDat.Scan0;
				
				return new colour(s + y * width + x);
			}
			
			public override void setCol(int x, int y, colour c)
			{
				int* s = (int*)bmpDat.Scan0;
				
				*(s + y * width + x) = c.argb;
			}
		}
		
		public class shader
		{
			string codeString;
			sing.Hosting.ScriptEngine pyEngine;
			sing.Hosting.ScriptScope pyScope;
			sing.Hosting.ScriptSource pySource;
			sing.Hosting.CompiledCode pyCode;
			
			public shader(string codeN)
			{
				codeString = codeN;
				init();
			}
			
			void init()
			{
				try
				{
					pyEngine = py.Hosting.Python.CreateEngine();
					pyScope = pyEngine.CreateScope();
					pySource = pyEngine.CreateScriptSourceFromString(codeString, sing.SourceCodeKind.Statements);
					pyCode = pySource.Compile();
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Invalid Python");
				}
			}
			
			public void process(surface target, surface[] surfs)
			{
				
				pyScope.SetVariable("target", target);
				pyScope.SetVariable("surfs", surfs);
				
				try
				{
					pyCode.Execute(pyScope);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}
		
		public MainForm()
		{
			InitializeComponent();
		}
		
		void ProcessToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (inImgLoc == null)
			{
				MessageBox.Show("Please provide an input image");
				return;
			}
			if (outImgLoc == null)
			{
				outImgLoc = "default.png";
			}
			
			Bitmap tmap = new Bitmap(inImg.Image.Width, inImg.Image.Height);
			
			surface insurf = new bmpSurface((Bitmap)inImg.Image);
			surface target = new bmpSurface(tmap);
			
			shader shade = new shader(codeF.Text);
			shade.process(target, new surface[] { insurf });
			
			target.save(outImgLoc);
			
			insurf.close();
			target.close();
			tmap.Dispose();
			
			updateImgs();
		}
		
		string inImgLoc = null;
		void InImgClick(object sender, EventArgs e)
		{
			DialogResult res = openFileDialog1.ShowDialog();
			
			if (res == DialogResult.OK && System.IO.File.Exists(openFileDialog1.FileName))
				inImgLoc = openFileDialog1.FileName;
			
			updateImgs();
		}
		
		string outImgLoc = null;
		void OutImgClick(object sender, EventArgs e)
		{
			DialogResult res = saveFileDialog1.ShowDialog();
			
			if (res == DialogResult.OK)
				outImgLoc = saveFileDialog1.FileName;
			
			updateImgs();
		}
		
		void updateImgs()
		{
			if (System.IO.File.Exists(inImgLoc))
				inImg.ImageLocation = inImgLoc;
			if (System.IO.File.Exists(outImgLoc))
				outImg.ImageLocation = outImgLoc;
		}
	}
}
