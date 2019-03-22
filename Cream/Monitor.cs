using System;
using System.Drawing;
using System.Collections;
namespace  Cream
{
	
	[Serializable]
	public class Monitor:System.Windows.Forms.Form
	{
        delegate void SetTextCallback(string text);
        
        private class AnonymousClassActionListener
		{
			public AnonymousClassActionListener(Monitor enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(Monitor enclosingInstance)
			{
				EnclosingInstance = enclosingInstance;
			}

            public Monitor EnclosingInstance { get; set; }

            public virtual void  ActionPerformed(Object eventSender, EventArgs e)
			{
				EnclosingInstance.Dispose();
			}
		}
		private class AnonymousClassActionListener1
		{
		    public AnonymousClassActionListener1(Monitor enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(Monitor enclosingInstance)
			{
				EnclosingInstance = enclosingInstance;
			}
//			private Monitor enclosingInstance;

		    public Monitor EnclosingInstance { get; set; }

		    public virtual void  ActionPerformed(Object eventSender, EventArgs e)
			{
				Environment.Exit(0);
			}
		}
		private long startTime;
		private ArrayList solvers;
        private Hashtable solverData;
		private int currentX;
		private int xmin;
		private int xmax;
		private int ymin;
		private int ymax;
		
		private Image image;
		private long prevPaintTime;
		private int topMargin = 100;
		private int botMargin = 50;
		private int leftMargin = 50;
		private int rightMargin = 50;
		private double xscale;
		private double yscale;
		private Color[] color = new[]{Color.Red, Color.Blue, Color.FromArgb(0, 128, 0), Color.FromArgb(0, 128, 128), Color.Magenta, Color.Green, Color.FromArgb(128, 128, 0), Color.Pink};
		
		public Monitor()
		{
			Init();
			Size = new Size(800, 600);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
			MaximizeBox = true;
			var close = new System.Windows.Forms.MenuItem("Close");
			close.Click += new AnonymousClassActionListener(this).ActionPerformed;
			SupportClass.CommandManager.CheckCommand(close);
			var quit = new System.Windows.Forms.MenuItem("Quit");
			quit.Click += new AnonymousClassActionListener1(this).ActionPerformed;
			SupportClass.CommandManager.CheckCommand(quit);
			var menu = new System.Windows.Forms.MenuItem("Window");
			menu.MenuItems.Add(close);
			menu.MenuItems.Add(quit);
			var mb = new System.Windows.Forms.MainMenu();
			mb.MenuItems.Add(menu);
			Menu = mb;
			//setsetBackground(Color.White);
			Invalidate();
			Visible = true;
		}
		
		public void  Init()
		{
			lock (this)
			{
				startTime = (DateTime.Now.Ticks - 621355968000000000) / 10000;
				solvers = new ArrayList();
				solverData = new Hashtable();
				currentX = 0;
				SetX(0, 10);
				ymin = Int32.MaxValue;
				ymax = Int32.MinValue;
			}
		}
		
		public virtual void  SetX(int xMIN, int xMAX)
		{
			lock (this)
			{
				xmin = Math.Max(0, xMIN);
				xmax = Math.Max(xmin + 60, xMAX);
			}
		}
		
		public virtual void  ADD(Solver solver)
		{
			lock (this)
			{
				solvers.Add(solver);
			}
		}
		
		public virtual void  ADDData(Solver solver, int y)
		{
			lock (this)
			{
				long t0 = (DateTime.Now.Ticks - 621355968000000000) / 10000;
				var x = (int) ((t0 - startTime) / 1000);
				int j = x - xmin;
				if (x < xmin)
					return ;
				if (x > xmax)
				{
					xmax = xmin + 4 * j / 3;
					if (xmax % 60 != 0)
						xmax += 60 - xmax % 60;
				}
				currentX = Math.Max(currentX, x);
				ymin = Math.Min(ymin, y);
				ymax = Math.Max(ymax, y);
				var data = (Int32[]) solverData[solver];
				if (data == null)
				{
					data = new Int32[xmax - xmin];
					solverData.Add(solver, data);
				}
				if (j >= data.Length)
				{
					var newData = new Int32[4 * j / 3];
					for (int i = 0; i < data.Length; i++)
						newData[i] = data[i];
					data = newData;
					solverData.Add(solver, data);
				}
				data[j] = y;
				if (image == null || t0 - prevPaintTime >= 1000)
				{
					RefreshThis("d");
					prevPaintTime = t0;
				}
			}
		}
		
        private void RefreshThis(String text)
        {
            if (InvokeRequired)
            {
                var d = new SetTextCallback(RefreshThis);
                Invoke(d, new object[] { text });
            }
            else
            {
                Refresh();
            }
            
        }
		private int Wpos(int x)
		{
			return leftMargin + (int) (xscale * (x - xmin));
		}
		
		private int Hpos(int y)
		{
			return topMargin + (int) (yscale * (ymax - y));
		}
		
		private void  DrawLine(Graphics g, int x0, int y0, int x1, int y1)
		{
			g.DrawLine(SupportClass.GraphicsManager.Manager.GetPen(g), Wpos(x0), Hpos(y0), Wpos(x1), Hpos(y1));
		}
		
		private void  UpdateImage(int width, int height)
		{
			lock (this)
			{
				image = new Bitmap(width, height);
				int w = width - (leftMargin + rightMargin);
				int h = height - (topMargin + botMargin);
				if (w <= 0 || h <= 0)
					return ;
				if (xmin >= xmax || ymin >= ymax)
					return ;
				xscale = w / (double) (xmax - xmin);
				yscale = h / (double) (ymax - ymin);
				Graphics g = Graphics.FromImage(image);
				// x-axis
				/////////////g.setColor(Color.LightGray);
				DrawLine(g, xmin, ymin, xmax, ymin);
				DrawLine(g, xmin, ymax, xmax, ymax);
				////////////////////g.setColor(Color.Black);
				g.DrawString(Convert.ToString(xmax), SupportClass.GraphicsManager.Manager.GetFont(g), SupportClass.GraphicsManager.Manager.GetBrush(g), Wpos(xmax), Hpos(ymin) + botMargin / 4 - SupportClass.GraphicsManager.Manager.GetFont(g).GetHeight());
				// y-axis
				DrawLine(g, xmin, ymin, xmin, ymax);
// ReSharper disable PossibleLossOfFraction
				g.DrawString(Convert.ToString(ymin), SupportClass.GraphicsManager.Manager.GetFont(g), SupportClass.GraphicsManager.Manager.GetBrush(g), leftMargin / 3, Hpos(ymin) + 5 - SupportClass.GraphicsManager.Manager.GetFont(g).GetHeight());
// ReSharper restore PossibleLossOfFraction
// ReSharper disable PossibleLossOfFraction
				g.DrawString(Convert.ToString(ymax), SupportClass.GraphicsManager.Manager.GetFont(g), SupportClass.GraphicsManager.Manager.GetBrush(g), leftMargin / 3, Hpos(ymax) + 5 - SupportClass.GraphicsManager.Manager.GetFont(g).GetHeight());
// ReSharper restore PossibleLossOfFraction
				g.DrawString("time=" + currentX, SupportClass.GraphicsManager.Manager.GetFont(g), SupportClass.GraphicsManager.Manager.GetBrush(g), Wpos(xmin), Hpos(ymin) + botMargin / 2 - SupportClass.GraphicsManager.Manager.GetFont(g).GetHeight());
				
				for (int i = 0; i < solvers.Count; i++)
				{
					var solver = (Solver) solvers[i];
                    
					var data = (Int32?[]) solverData[solver];
					if (data == null)
						continue;
					SupportClass.GraphicsManager.Manager.SetColor(g, color[i % color.Length]);
					String msg = solver + "=" + solver.BestValue;
					g.DrawString(msg, SupportClass.GraphicsManager.Manager.GetFont(g), SupportClass.GraphicsManager.Manager.GetBrush(g), Wpos(xmin) + 100 * (i + 1), Hpos(ymin) + botMargin / 2 - SupportClass.GraphicsManager.Manager.GetFont(g).GetHeight());
					int? x0 = - 1;
					int? y0 = - 1;
					for (int j = 0; j < data.Length; j++)
					{
						if (data[j] == null)
							continue;
						int? x = xmin + j;
						int? y = data[j];
						if (x0 >= 0)
						{
							DrawLine(g, (Int32)x0, (Int32)y0, (Int32)x, (Int32)y);
						}
						x0 = x;
						y0 = y;
					}
				}
			}
		}
		
		public void  Update(Graphics g)
		{
			Size size = Size;
			UpdateImage(size.Width, size.Height);
			g.DrawImage(image, 0, 0);
		}
		
		protected override void  OnPaint(System.Windows.Forms.PaintEventArgs gEventArg)
		{
			Graphics g = null;
			if (gEventArg != null)
				g = gEventArg.Graphics;
			if (image != null)
			{
			    if (g != null) g.DrawImage(image, 0, 0);
			}
		}
	}
}