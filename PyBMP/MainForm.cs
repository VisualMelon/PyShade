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

namespace PyShade
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public class fcol
		{
			public float a, r, g, b;

			public fcol(float aN, float rN, float gN, float bN)
			{
				a = aN;
				r = rN;
				g = gN;
				b = bN;
			}
			
			public fcol mul(float ac, float rc, float gc, float bc)
			{
				a *= ac;
				r *= rc;
				g *= gc;
				b *= bc;
				
				return this;
			}
			
			public fcol add(float ac, float rc, float gc, float bc)
			{
				a += ac;
				r += rc;
				g += gc;
				b += bc;
				
				return this;
			}
			
			public fcol mul(fcol ofc)
			{
				a *= ofc.a;
				r *= ofc.r;
				g *= ofc.g;
				b *= ofc.b;
				
				return this;
			}
			
			public fcol add(fcol ofc)
			{
				a += ofc.a;
				r += ofc.r;
				g += ofc.g;
				b += ofc.b;
				
				return this;
			}
			
			public fcol add(fcol ofc, float coof)
			{
				a += ofc.a * coof;
				r += ofc.r * coof;
				g += ofc.g * coof;
				b += ofc.b * coof;
				
				return this;
			}
			
			public colour toColour()
			{
				return new colour(util.clampToByte(a * 255f),
				                  util.clampToByte(r * 255f),
				                  util.clampToByte(g * 255f),
				                  util.clampToByte(b * 255f));
			}
		}
		
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
			
			public colour mul(colour oc)
			{
				return new colour(util.clampToByte(oc.a * (float)a),
				                  util.clampToByte(oc.r * (float)r),
				                  util.clampToByte(oc.g * (float)g),
				                  util.clampToByte(oc.b * (float)b));
			}
			
			public colour add(colour oc)
			{
				return new colour(util.clampToByte(oc.a + (int)a),
				                  util.clampToByte(oc.r + (int)r),
				                  util.clampToByte(oc.g + (int)g),
				                  util.clampToByte(oc.b + (int)b));
			}
			
			public fcol toFCol()
			{
				return new fcol((float)a / 255f, (float)r / 255f, (float)g / 255f, (float)b / 255f);
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
			
			public virtual void close() { }
			public abstract colour getCol(int x, int y);
			public abstract void setCol(int x, int y, colour c);
			public abstract void clear(colour c);
			
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
				return getCol(util.wrap(x, width - 1), util.wrap(y, height - 1));
			}
			
			public void setCol_wrap(int x, int y, colour c)
			{
				setCol(util.wrap(x, width - 1), util.wrap(y, height - 1), c);
			}
			
			public colour getCol_mirror(int x, int y)
			{
				return getCol(util.mirror(x, width - 1), util.mirror(y, height - 1));
			}
			
			public void setCol_mirror(int x, int y, colour c)
			{
				setCol(util.mirror(x, width - 1), util.mirror(y, height - 1), c);
			}
			
			public colour getCol_clamp(int x, int y)
			{
				return getCol(util.clamp(x, width - 1), util.clamp(y, height - 1));
			}
			
			public void setCol_clamp(int x, int y, colour c)
			{
				setCol(util.clamp(x, width - 1), util.clamp(y, height - 1), c);
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
		
		public unsafe class arraySurface : surface
		{
			colour[][] data;
			
			public arraySurface(int w, int h)
			{
				width = w;
				height = h;
				data = new colour[h][];
				
				for (int y = 0; y < h; y++)
				{
					data[y] = new colour[w];
				}
			}
			
			public override colour getCol(int x, int y)
			{
				return data[y][x];
			}
			
			public override void setCol(int x, int y, colour c)
			{
				data[y][x] = c;
			}
			
			public override void clear(colour c)
			{
				for (int y = height - 1; y >= 0; y--)
				{
					colour[] row = data[y];
					
					for (int x = width - 1; x >= 0; x--)
					{
						row[x] = c;
					}
				}
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
				// there is a reason for this
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
			
			public override void clear(colour c)
			{
				int* s = (int*)bmpDat.Scan0;
				
				for (int i = width * height; i > 0; i--)
				{
					*s = c.argb;
					s++;
				}
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
			}
			
			public bool init(Action<string> reporter)
			{
				try
				{
					pyEngine = py.Hosting.Python.CreateEngine();
					pyScope = pyEngine.CreateScope();
					pySource = pyEngine.CreateScriptSourceFromString(codeString, sing.SourceCodeKind.Statements);
					pyCode = pySource.Compile();
					return true;
				}
				catch (Exception ex)
				{
					reporter.Invoke("Compilation error: " + ex.Message);
					return false;
				}
			}
			
			public bool process(surface target, surface[] surfs, Action<string> reporter)
			{
				pyScope.SetVariable("target", target);
				pyScope.SetVariable("surfs", surfs);
				pyScope.SetVariable("reporter", reporter);
				
				try
				{
					pyCode.Execute(pyScope);
					return true;
				}
				catch (Exception ex)
				{
					reporter.Invoke("Processing error: " + ex.Message);
					return false;
				}
			}
		}
		
		public MainForm()
		{
			InitializeComponent();
			reportMsg = new Action<string>(reportFT); // this side
			reporter = new Action<string>(report); // other threads
		}
		
		Action<string> reportMsg;
		void reportFT(string msg)
		{
			if (msg == null)
				return;
			
			reportF.AppendText(Environment.NewLine + msg);
			reportF.Select(reportF.Text.Length, 0);
			reportF.ScrollToCaret();
		}
		
		Action<string> reporter;
		void report(string msg)
		{
			if (msg == null) // save the invokation, cheap anyway
				return;
			
			this.Invoke(reportMsg, msg);
		}
		
		void ProcessToolStripMenuItemClick(object sender, EventArgs e)
		{
			(new Action(process)).BeginInvoke(null, null);
		}
		
		void process()
		{
			try
			{
				System.Diagnostics.Stopwatch osw = new System.Diagnostics.Stopwatch();
				osw.Reset();
				osw.Start();
				
				bool carryon = true;
			
				System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
				
				if (inImgLoc == null)
				{
					report("Please provide an input image");
					goto fail;
				}
				if (outImgLoc == null)
				{
					outImgLoc = "default.png";
					report("No destination image, using default (default.png)");
				}
				
				//Bitmap tmap = new Bitmap(inImg.Image.Width, inImg.Image.Height);
				
				surface insurf = new bmpSurface((Bitmap)inImg.Image);
				surface target = new arraySurface(inImg.Image.Width, inImg.Image.Height);//new bmpSurface(tmap);
				
				sw.Reset();
				sw.Start();
				shader shade = new shader(codeF.Text);
				carryon = shade.init(reporter);
				sw.Stop();
				if (carryon)
					report("Code compiled (" + sw.ElapsedMilliseconds + "ms)");
				else
					goto fail;
				
				sw.Reset();
				sw.Start();
				carryon = shade.process(target, new surface[] { insurf }, reporter);
				sw.Stop();
				if (carryon)
					report("Processed (" + sw.ElapsedMilliseconds + "ms)");
				else
					goto fail;
				
				sw.Reset();
				sw.Start();
				target.save(outImgLoc);
				sw.Stop();
				report("Saved output to " + outImgLoc + " (" + sw.ElapsedMilliseconds + "ms)");
				
				insurf.close();
				target.close();
				
				this.Invoke(new Action(updateImgs));
				report("Finished (" + osw.ElapsedMilliseconds + "ms).");
				osw.Stop();
			}
			catch (Exception ex)
			{
				report("Failed with error " + ex.Message + ".");
			}
			
			return;
		fail:
			report("Failed.");
			return;
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
