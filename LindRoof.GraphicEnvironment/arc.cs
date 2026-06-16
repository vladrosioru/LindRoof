using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LindRoof.GraphicEnvironment;

[Serializable]
public class arc : GraphicObject
{
	private Point TPOTS1;

	private Point TPOTS2;

	private int Height;

	private double _height;

	public override double length
	{
		get
		{
			double num = (stopPoint.X - startPoint.X) * (stopPoint.X - startPoint.X);
			double num2 = (stopPoint.Y - startPoint.Y) * (stopPoint.Y - startPoint.Y);
			double d = num + num2;
			_length = Math.Round(Math.Sqrt(d), 3);
			return _length;
		}
	}

	public arc()
	{
	}

	public arc(PointD start, PointD stop, double height, Layer lay)
	{
		startPoint.X = Math.Round(start.X, 3);
		startPoint.Y = Math.Round(start.Y, 3);
		stopPoint.X = Math.Round(stop.X, 3);
		stopPoint.Y = Math.Round(stop.Y, 3);
		_height = Math.Round(height, 3);
		layer = lay;
		color = lay.colorOfLayer;
		pen = new Pen(color, 1f);
		selectionColor = Color.Gray;
		selectionPen = new Pen(selectionColor, 1f);
		float[] dashPattern = new float[6] { 4f, 4f, 8f, 4f, 12f, 4f };
		selectionPen.DashPattern = dashPattern;
	}

	public override void Draw(Graphics gt, double scale, double bpx, double bpy)
	{
		TPOTS1 = new Point((int)((startPoint.X - bpx) / scale), (int)((bpy - startPoint.Y) / scale));
		TPOTS2 = new Point((int)((stopPoint.X - bpx) / scale), (int)((bpy - stopPoint.Y) / scale));
		Height = (int)(_height / scale);
	}

	public override GraphicsPath graphicsPath(int wide)
	{
		GraphicsPath graphicsPath = new GraphicsPath();
		if (wide != 0 && wide != 100)
		{
			graphicsPath.AddLine(TPOTS1, TPOTS2);
			graphicsPath.Widen(new Pen(Brushes.Black, wide));
		}
		else if (wide != 100)
		{
			graphicsPath.AddLine((float)Math.Round(startPoint.X, 3), (float)Math.Round(startPoint.Y, 3), (float)Math.Round(stopPoint.X, 3), (float)Math.Round(stopPoint.Y, 3));
			graphicsPath.Widen(new Pen(Brushes.Black, wide + 1));
		}
		else
		{
			graphicsPath.AddLine(TPOTS1, TPOTS2);
		}
		return graphicsPath;
	}
}
