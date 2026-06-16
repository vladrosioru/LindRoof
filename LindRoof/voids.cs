using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LindRoof;

public class voids
{
	public string name;

	public double width;

	public double width2;

	public double heigh;

	public double offsetX;

	public double offsetY;

	private Pen pen = new Pen(Color.Gray, 1f);

	private StringFormat sf = new StringFormat();

	private Font font = new Font("Arial", 10f);

	private SolidBrush sb = new SolidBrush(Color.Black);

	public PointD startPoint = new PointD();

	public PointD startPointN = new PointD();

	private Point TPOTS1;

	private int intWidth;

	private int intHeigh;

	private int intWidth2;

	public bool side;

	public double leftSupport = -1.0;

	public double rightSupport = -1.0;

	public double topSupport;

	public double bottomSupport;

	public bool drawTopSupport = true;

	public bool drawBottomSupport = true;

	public bool trapezoidal;

	public bool isSelected;

	public voids(string nam, PointD startP, double wid, double wid2, double hei, bool trapez)
	{
		trapezoidal = trapez;
		name = nam;
		startPoint.X = startP.X - wid / 2.0;
		startPoint.Y = startP.Y + hei / 2.0;
		width = wid;
		width2 = wid2;
		heigh = hei;
		sf.Alignment = StringAlignment.Center;
		sf.LineAlignment = StringAlignment.Center;
	}

	public void Reinit(double offX, double offY)
	{
		startPoint.X += offX;
		startPoint.Y += offY;
	}

	public void UpdateDrawingCoords(double scale, double bpx, double bpy, Bitmap bm)
	{
		TPOTS1 = TransformFromDrawing(startPoint, scale, bpx, bpy, bm);
		intWidth = (int)(width / scale);
		intHeigh = (int)(heigh / scale);
	}

	public void DrawEvolved(Graphics gr, double scale, double base_point_x, double base_point_y, Bitmap bm, bool print)
	{
		if (!print)
		{
			this.pen = new Pen(Color.Gray, 1f);
			if (!isSelected)
			{
				this.pen.DashStyle = DashStyle.Solid;
			}
			else
			{
				this.pen.Color = Color.White;
				this.pen.DashStyle = DashStyle.Dash;
			}
		}
		else
		{
			this.pen = new Pen(Color.Black, 1f);
			this.pen.DashStyle = DashStyle.Solid;
		}
		TPOTS1 = TransformFromDrawing(startPoint, scale, base_point_x, base_point_y, bm);
		intWidth = (int)(width / scale);
		intHeigh = (int)(heigh / scale);
		if (!trapezoidal)
		{
			gr.DrawRectangle(rect: new Rectangle(TPOTS1.X, TPOTS1.Y, intWidth, intHeigh), pen: this.pen);
			if (intHeigh > 20 || intWidth > 50)
			{
				gr.DrawString(name, font, this.pen.Brush, TPOTS1.X + intWidth / 2, TPOTS1.Y + intHeigh / 2 - 7, sf);
				gr.DrawString(Math.Round(width, 0) * 10.0 + "x \r\n" + Math.Round(heigh, 0) * 10.0 + "mm", font, this.pen.Brush, TPOTS1.X + intWidth / 2, TPOTS1.Y + intHeigh / 2 + 18, sf);
			}
			else
			{
				Pen pen = new Pen(Color.Gray, 1f);
				gr.DrawLine(pen, TPOTS1.X, TPOTS1.Y, TPOTS1.X + intWidth, TPOTS1.Y + intHeigh);
				gr.DrawLine(pen, TPOTS1.X + intWidth, TPOTS1.Y, TPOTS1.X, TPOTS1.Y + intHeigh);
			}
			return;
		}
		intWidth2 = (int)(width2 / scale);
		GraphicsPath graphicsPath = new GraphicsPath();
		graphicsPath.AddLine(TPOTS1.X, TPOTS1.Y + intHeigh, TPOTS1.X + (intWidth - intWidth2) / 2, TPOTS1.Y);
		graphicsPath.AddLine(TPOTS1.X + (intWidth - intWidth2) / 2, TPOTS1.Y, TPOTS1.X + (intWidth + intWidth2) / 2, TPOTS1.Y);
		graphicsPath.AddLine(TPOTS1.X + (intWidth + intWidth2) / 2, TPOTS1.Y, TPOTS1.X + intWidth, TPOTS1.Y + intHeigh);
		graphicsPath.AddLine(TPOTS1.X + intWidth, TPOTS1.Y + intHeigh, TPOTS1.X, TPOTS1.Y + intHeigh);
		gr.DrawPath(this.pen, graphicsPath);
		if (intHeigh > 20)
		{
			gr.DrawString(name, font, this.pen.Brush, TPOTS1.X + intWidth / 2, TPOTS1.Y + intHeigh / 2 - 10, sf);
			if (width2 > 0.0)
			{
				gr.DrawString("(" + Math.Round(width, 0) * 10.0 + "with \r\n" + Math.Round(width2, 0) * 10.0 + ")x \r\n" + Math.Round(heigh, 0) * 10.0 + "mm", font, this.pen.Brush, TPOTS1.X + intWidth / 2, TPOTS1.Y + intHeigh / 2 + 18, sf);
			}
			else
			{
				gr.DrawString(Math.Round(width, 0) * 10.0 + "x \r\n" + Math.Round(heigh, 0) * 10.0 + "mm", font, this.pen.Brush, TPOTS1.X + intWidth / 2, TPOTS1.Y + intHeigh / 2 + 18, sf);
			}
		}
		else
		{
			Pen pen2 = new Pen(Color.Gray, 1f);
			gr.DrawLine(pen2, TPOTS1.X, TPOTS1.Y + intHeigh, TPOTS1.X + (intWidth + intWidth2) / 2, TPOTS1.Y);
			gr.DrawLine(pen2, TPOTS1.X + (intWidth - intWidth2) / 2, TPOTS1.Y, TPOTS1.X + intWidth, TPOTS1.Y + intHeigh);
		}
	}

	public void DrawEvolved2(Graphics gr, double scale, double base_point_x, double base_point_y, Bitmap bm, bool print)
	{
		if (!print)
		{
			this.pen = new Pen(Color.Gray, 1f);
			if (!isSelected)
			{
				this.pen.DashStyle = DashStyle.Solid;
			}
			else
			{
				this.pen.Color = Color.White;
				this.pen.DashStyle = DashStyle.Dash;
			}
		}
		else
		{
			this.pen = new Pen(Color.Black, 1f);
			this.pen.DashStyle = DashStyle.Solid;
		}
		TPOTS1 = TransformFromDrawing2(startPoint, scale, base_point_x, base_point_y, bm);
		intWidth = (int)(width / scale);
		intHeigh = (int)(heigh / scale);
		if (!trapezoidal)
		{
			gr.DrawRectangle(rect: new Rectangle(TPOTS1.X, TPOTS1.Y, intWidth, intHeigh), pen: this.pen);
			if (intHeigh > 20 || intWidth > 50)
			{
				gr.DrawString(name, font, this.pen.Brush, TPOTS1.X + intWidth / 2, TPOTS1.Y + intHeigh / 2 - 7, sf);
				gr.DrawString(Math.Round(width, 0) * 10.0 + "x \r\n" + Math.Round(heigh, 0) * 10.0 + "mm", font, this.pen.Brush, TPOTS1.X + intWidth / 2, TPOTS1.Y + intHeigh / 2 + 18, sf);
			}
			else
			{
				Pen pen = new Pen(Color.Gray, 1f);
				gr.DrawLine(pen, TPOTS1.X, TPOTS1.Y, TPOTS1.X + intWidth, TPOTS1.Y + intHeigh);
				gr.DrawLine(pen, TPOTS1.X + intWidth, TPOTS1.Y, TPOTS1.X, TPOTS1.Y + intHeigh);
			}
			return;
		}
		intWidth2 = (int)(width2 / scale);
		GraphicsPath graphicsPath = new GraphicsPath();
		graphicsPath.AddLine(TPOTS1.X, TPOTS1.Y + intHeigh, TPOTS1.X + (intWidth - intWidth2) / 2, TPOTS1.Y);
		graphicsPath.AddLine(TPOTS1.X + (intWidth - intWidth2) / 2, TPOTS1.Y, TPOTS1.X + (intWidth + intWidth2) / 2, TPOTS1.Y);
		graphicsPath.AddLine(TPOTS1.X + (intWidth + intWidth2) / 2, TPOTS1.Y, TPOTS1.X + intWidth, TPOTS1.Y + intHeigh);
		graphicsPath.AddLine(TPOTS1.X + intWidth, TPOTS1.Y + intHeigh, TPOTS1.X, TPOTS1.Y + intHeigh);
		gr.DrawPath(this.pen, graphicsPath);
		if (intHeigh > 20)
		{
			gr.DrawString(name, font, this.pen.Brush, TPOTS1.X + intWidth / 2, TPOTS1.Y + intHeigh / 2 - 10, sf);
			if (width2 > 0.0)
			{
				gr.DrawString("(" + Math.Round(width, 0) * 10.0 + "with \r\n" + Math.Round(width2, 0) * 10.0 + ")x \r\n" + Math.Round(heigh, 0) * 10.0 + "mm", font, this.pen.Brush, TPOTS1.X + intWidth / 2, TPOTS1.Y + intHeigh / 2 + 18, sf);
			}
			else
			{
				gr.DrawString(Math.Round(width, 0) * 10.0 + "x \r\n" + Math.Round(heigh, 0) * 10.0 + "mm", font, this.pen.Brush, TPOTS1.X + intWidth / 2, TPOTS1.Y + intHeigh / 2 + 18, sf);
			}
		}
		else
		{
			Pen pen2 = new Pen(Color.Gray, 1f);
			gr.DrawLine(pen2, TPOTS1.X, TPOTS1.Y + intHeigh, TPOTS1.X + (intWidth + intWidth2) / 2, TPOTS1.Y);
			gr.DrawLine(pen2, TPOTS1.X + (intWidth - intWidth2) / 2, TPOTS1.Y, TPOTS1.X + intWidth, TPOTS1.Y + intHeigh);
		}
	}

	public bool Hitted(Point mouse)
	{
		GraphicsPath graphicsPath = new GraphicsPath();
		graphicsPath.AddLine(TPOTS1.X + intWidth, TPOTS1.Y + intHeigh, TPOTS1.X, TPOTS1.Y + intHeigh);
		if (trapezoidal)
		{
			graphicsPath.AddLine(TPOTS1.X, TPOTS1.Y + intHeigh, TPOTS1.X + intWidth / 2, TPOTS1.Y);
		}
		else
		{
			graphicsPath.AddLine(TPOTS1.X, TPOTS1.Y + intHeigh, TPOTS1.X, TPOTS1.Y);
			graphicsPath.AddLine(TPOTS1.X, TPOTS1.Y, TPOTS1.X + intWidth, TPOTS1.Y);
		}
		graphicsPath.CloseFigure();
		Region region = new Region();
		region.MakeEmpty();
		region.Union(graphicsPath);
		return region.IsVisible(mouse);
	}

	private Point TransformFromDrawing(PointD p, double scale, double base_point_x, double base_point_y, Bitmap bm)
	{
		return new Point((int)((p.X - base_point_x) / scale), (int)((base_point_y - p.Y) / scale));
	}

	private Point TransformFromDrawing2(PointD p, double scale, double base_point_x, double base_point_y, Bitmap bm)
	{
		return new Point((int)((p.X - base_point_x) / scale - (double)(bm.Width / 3)), (int)((base_point_y - p.Y) / scale - (double)(bm.Height / 3)));
	}
}
