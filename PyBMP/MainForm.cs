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
			
			public fcol mul(float coof)
			{
				a *= coof;
				r *= coof;
				g *= coof;
				b *= coof;
				
				return this;
			}
			
			public fcol mulRGB(float coof)
			{
				a *= coof;
				r *= coof;
				g *= coof;
				b *= coof;
				
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
			
			public colour mul(float c)
			{
				return new colour(util.clampToByte(c * (float)a),
				                  util.clampToByte(c * (float)r),
				                  util.clampToByte(c * (float)g),
				                  util.clampToByte(c * (float)b));
			}
			
			public colour mulRGB(float c)
			{
				return new colour(a,
				                  util.clampToByte(c * (float)r),
				                  util.clampToByte(c * (float)g),
				                  util.clampToByte(c * (float)b));
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
		
		public interface iterable
		{
			void moveToStart();
			void moveToEnd();
			void qmove();
			void qmoveBack();
			bool move();
			bool moveBack();
			colour getCol();
			colour peekCol();
			bool getCol(out colour c);
			void qsetCol(colour c);
			bool setCol(colour c);
		}
		
		// iterates everything
		public abstract unsafe class iterator : iterable
		{
			public abstract void moveToStart();
			public abstract void moveToEnd();
			public abstract void qmove();
			public abstract void qmoveBack();
			public abstract bool move();
			public abstract bool moveBack();
			public abstract colour getCol();
			public abstract colour peekCol();
			public abstract bool getCol(out colour c);
			public abstract void qsetCol(colour c);
			public abstract bool setCol(colour c);
		}
		
		// iterates "region"
		public abstract unsafe class region : iterable
		{
			public abstract void iterate(int sx, int sy, int ex, int ey); // moves to start
			public abstract void moveToStart();
			public abstract void moveToEnd();
			public abstract void qmove();
			public abstract void qmoveBack();
			public abstract bool move();
			public abstract bool moveBack();
			public abstract colour getCol();
			public abstract colour peekCol();
			public abstract bool getCol(out colour c);
			public abstract void qsetCol(colour c);
			public abstract bool setCol(colour c);
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
			public abstract iterator getIterator();
			public abstract region getRegion();
			
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
				sdi.BitmapData bmpDat = bmp.LockBits(new Rectangle(0, 0, width, height), sdi.ImageLockMode.WriteOnly, sdi.PixelFormat.Format32bppArgb);
				
				for (int i = 0; i < 20; i++)
				{
					int* s = (int*)bmpDat.Scan0;
				
					iterator r = getIterator();
					
					colour c;
					while (r.getCol(out c))
					{
						*s = c.argb;
						s++;
					}
					*s = c.argb; // catch last pil
					
//					do
//					{
//						*s = r.peekCol().argb;
//						s++;
//					}
//					while (r.move());
				
//					s = (int*)bmpDat.Scan0;
//					
//					for (int y = 0; y < height; y++)
//					{
//						for (int x = 0; x < width; x++)
//						{
//							*s = getCol(x, y).argb;
//							s++;
//						}
//					}
				}
				
				bmp.UnlockBits(bmpDat);
				bmp.Save(fileName, sdi.ImageFormat.Png);
				bmp.Dispose();
			}
		}
		
		// iterator - heavily inlined
		public unsafe class arrayIterator : iterator
		{
			colour[][] data;
			colour[] row;
			int wm1;
			int hm1;
			int x;
			int y;
			
			public arrayIterator(colour[][] surfData)
			{
				data = surfData;
				hm1 = data.Length - 1;
				wm1 = data[0].Length - 1;
				moveToStart();
			}
			
			public override void moveToStart()
			{
				x = 0;
				y = 0;
				row = data[y];
			}
			
			public override void moveToEnd()
			{
				x = wm1;
				y = hm1;
				row = data[y];
			}
			
			public override void qmove()
			{
				if (x >= wm1)
				{
					x = 0;
					if (y > hm1)
					{
						y = 0;
					}
					else
						y++;
					row = data[y];
				}
				else
					x++;
			}
			
			public override void qmoveBack()
			{
				if (x <= 0)
				{
					x = wm1;
					if (y < 0)
					{
						y = hm1;
					}
					else
						y--;
					row = data[y];
				}
				else
					x--;
			}
			
			public override bool move()
			{
				if (x >= wm1)
				{
					x = 0;
					if (y >= hm1)
					{
						y = 0;
						row = data[y];
						return false;
					}
					y++;
					row = data[y];
				}
				else
					x++;
				return true;
			}
			
			public override bool moveBack()
			{
				if (x <= 0)
				{
					x = wm1;
					if (y <= 0)
					{
						y = hm1;
						row = data[y];
						return false;
					}
					y--;
					row = data[y];
				}
				else
					x--;
				return true;
			}
			
			public override colour getCol()
			{
				// peek
				colour res = row[x];
				
				// qmove
				if (x >= wm1)
				{
					x = 0;
					if (y >= hm1)
					{
						y = 0;
					}
					else
						y++;
					row = data[y];
				}
				else
					x++;
				
				return res;
			}
			
			public override colour peekCol()
			{
				return row[x];
			}
			
			public override bool getCol(out colour c)
			{
				// peek
				c = row[x];
				
				// move
				if (x >= wm1)
				{
					x = 0;
					if (y >= hm1)
					{
						y = 0;
						row = data[y];
						return false;
					}
					y++;
					row = data[y];
				}
				else
					x++;
				return true;
			}
			
			public override void qsetCol(colour c)
			{
				// set
				row[x] = c;
				
				// qmove
				if (x >= wm1)
				{
					x = 0;
					if (y >= hm1)
					{
						y = 0;
					}
					else
						y++;
					row = data[y];
				}
				else
					x++;
				return;
			}
			
			public override bool setCol(colour c)
			{
				// set
				row[x] = c;
				
				// move
				if (x >= wm1)
				{
					x = 0;
					if (y >= hm1)
					{
						y = 0;
						row = data[y];
						return false;
					}
					y++;
					row = data[y];
				}
				else
					x++;
				return true;
			}
		}
		
		// iterator
		public unsafe class arrayRegion : region
		{
			colour[][] data;
			
			colour[] row;
			int w;
			int h;
			int sx;
			int sy;
			int x;
			int y;
			int ex;
			int ey;
			
			public arrayRegion(colour[][] surfData)
			{
				data = surfData;
				h = data.Length;
				w = data[0].Length;
			}
			
			public override void iterate(int sx, int sy, int ex, int ey)
			{
				this.sx = sx;
				this.sy = sy;
				this.ey = ey;
				this.ex = ex;
				
				moveToStart();
			}
			
			public override void moveToStart()
			{
				x = sx;
				y = sy;
				row = data[y];
			}
			
			public override void moveToEnd()
			{
				x = ex;
				y = ey;
				row = data[y];
			}
			
			public override void qmove()
			{
				if (y == ey && x >= ex)
				{
					x = sx;
					y = sy;
					row = data[y];
					return;
				}
				x++;
				if (x >= w)
				{
					y++;
					x = 0;
					row = data[y];
				}
			}
			
			public override void qmoveBack()
			{
				if (y == sy && x <= sx)
				{
					x = ex;
					y = ey;
					row = data[y];
					return;
				}
				x--;
				if (x < 0)
				{
					y--;
					x = w - 1;
					row = data[y];
				}
			}
			
			public override bool move()
			{
				if (y == ey && x >= ex)
				{
					x = sx;
					y = sy;
					row = data[y];
					return false;
				}
				x++;
				if (x >= w)
				{
					y++;
					x = 0;
					row = data[y];
				}
				return true;
			}
			
			public override bool moveBack()
			{
				if (y == sy && x <= sx)
				{
					x = ex;
					y = ey;
					row = data[y];
					return false;
				}
				x--;
				if (x < 0)
				{
					y--;
					x = w - 1;
					row = data[y];
				}
				return true;
			}
			
			public override colour getCol()
			{
				colour res = peekCol();
				qmove();
				return res;
			}
			
			public override colour peekCol()
			{
				return row[x];
			}
			
			public override bool getCol(out colour c)
			{
				c = peekCol();
				return move();
			}
			
			public override void qsetCol(colour c)
			{
				row[x] = c;
				qmove();
			}
			
			public override bool setCol(colour c)
			{
				row[x] = c;
				return move();
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
			
			public override region getRegion()
			{
				return new arrayRegion(data);
			}
			
			public override iterator getIterator()
			{
				return new arrayIterator(data);
			}
		}
		
		// iterator
		public unsafe class bmpIterator : iterator
		{
			sdi.BitmapData dat;
			int idx;
			int eidx;
			int* scn0;
			
			public bmpIterator(sdi.BitmapData bmpDat)
			{
				dat = bmpDat;
				scn0 = (int*)dat.Scan0;
				eidx = dat.Height * dat.Width;
				moveToStart();
			}
			
			public override void moveToStart()
			{
				idx = 0;
			}
			
			public override void moveToEnd()
			{
				idx = eidx;
			}
			
			public override void qmove()
			{
				if (idx >= eidx)
				{
					idx = 0;
				}
				else
					idx++;
			}
			
			public override void qmoveBack()
			{
				if (idx < 0)
				{
					idx = eidx;
				}
				else
					idx--;
			}
			
			public override bool move()
			{
				if (idx >= eidx)
				{
					idx = 0;
					return false;
				}
				idx++;
				return true;
			}
			
			public override bool moveBack()
			{
				if (idx < 0)
				{
					idx = eidx;
					return false;
				}
				idx--;
				return true;
			}
			
			public override colour getCol()
			{
				// peek
				colour res = peekCol();
				
				// qmove
				if (idx >= eidx)
				{
					idx = 0;
				}
				else
					idx++;
				
				return res;
			}
			
			public override colour peekCol()
			{
				return new colour(scn0 + idx);
			}
			
			public override bool getCol(out colour c)
			{
				// peek
				c = new colour(scn0 + idx);
				
				// move
				if (idx >= eidx)
				{
					idx = 0;
					return false;
				}
				idx++;
				return true;
			}
			
			public override void qsetCol(colour c)
			{
				// set
				*(scn0 + idx) = c.argb;
				
				// qmove
				if (idx >= eidx)
				{
					idx = 0;
				}
				else
					idx++;
			}
			
			public override bool setCol(colour c)
			{
				// set
				*(scn0 + idx) = c.argb;
				
				// move
				if (idx >= eidx)
				{
					idx = 0;
					return false;
				}
				idx++;
				return true;
			}
		}
		
		// iterator
		public unsafe class bmpRegion : region
		{
			sdi.BitmapData dat;
			int sidx;
			int idx;
			int eidx;
			int* scn0;
			
			public bmpRegion(sdi.BitmapData bmpDat)
			{
				dat = bmpDat;
				scn0 = (int*)dat.Scan0;
			}
			
			public override void iterate(int sx, int sy, int ex, int ey)
			{
				sidx = sy * dat.Width + sx;
				eidx = ey * dat.Width + ex;
				
				moveToStart();
			}
			
			public override void moveToStart()
			{
				idx = sidx;
			}
			
			public override void moveToEnd()
			{
				idx = eidx;
			}
			
			public override void qmove()
			{
				if (idx >= eidx)
				{
					idx = sidx;
					return;
				}
				idx++;
			}
			
			public override void qmoveBack()
			{
				if (idx < sidx)
				{
					idx = eidx;
					return;
				}
				idx--;
			}
			
			public override bool move()
			{
				if (idx >= eidx)
				{
					idx = sidx;
					return false;
				}
				idx++;
				return true;
			}
			
			public override bool moveBack()
			{
				if (idx < sidx)
				{
					idx = eidx;
					return false;
				}
				idx--;
				return true;
			}
			
			public override colour getCol()
			{
				colour res = peekCol();
				qmove();
				return res;
			}
			
			public override colour peekCol()
			{
				int* s = scn0 + idx;
				return new colour(s);
			}
			
			public override bool getCol(out colour c)
			{
				c = peekCol();
				return move();
			}
			
			public override void qsetCol(colour c)
			{
				int* s = scn0 + idx;
				*s = c.argb;
				
				qmove();
			}
			
			public override bool setCol(colour c)
			{
				int* s = scn0 + idx;
				*s = c.argb;
				
				return move();
			}
		}
		
		public unsafe class bmpSurface : surface
		{
			private Bitmap bmp;
			private sdi.BitmapData bmpDat;
			
			public bmpSurface(string fileName)
			{
				Bitmap bmpN = (Bitmap)Bitmap.FromFile(fileName);
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
			
			public override region getRegion()
			{
				return new bmpRegion(bmpDat);
			}
			
			public override iterator getIterator()
			{
				return new bmpIterator(bmpDat);
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
			
			public delegate colour shadeFunc1(colour src);
			public static void perPixelShade1(shadeFunc1 shadeFunc, surface target, surface surf)
			{
				iterator src = surf.getIterator();
				iterator trg = target.getIterator();
			
				while (trg.setCol(shadeFunc(src.getCol()))) { }
			}
			
			public delegate colour blendFunc1(colour trg, colour src);
			public static void perPixelBlend1(blendFunc1 blendFunc, surface target, surface surf)
			{
				iterator src = surf.getIterator();
				iterator trg = target.getIterator();
				
				while (trg.setCol(blendFunc(trg.peekCol(), src.getCol()))) { }
			}
			
			public bool process(surface target, surface[] surfs, Action<string> reporter)
			{
				pyScope.SetVariable("target", target);
				pyScope.SetVariable("surfs", surfs);
				pyScope.SetVariable("reporter", reporter);
				pyScope.SetVariable("pps1", new Action<shadeFunc1, surface, surface>(perPixelShade1));
				pyScope.SetVariable("ppb1", new Action<blendFunc1, surface, surface>(perPixelBlend1));
				
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
				
				sw.Reset();
				sw.Start();
				surface insurf = new bmpSurface((Bitmap)inImg.Image);
				surface target = new arraySurface(inImg.Image.Width, inImg.Image.Height);
				sw.Stop();
				report("Setup surfaces (" + sw.ElapsedMilliseconds + "ms)");
				
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
