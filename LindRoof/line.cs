using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LindRoof;

[Serializable]
public class line : GraphicObject, ICloneable
{
	private Point TPOTS1;

	private Point TPOTS2;

	private Pen pen1;

	private AdjustableArrowCap ec;

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

	public override GraphicsPath realGraphicsPath
	{
		get
		{
			GraphicsPath graphicsPath = new GraphicsPath();
			graphicsPath.AddLine((float)startPoint.X, (float)startPoint.Y, (float)stopPoint.X, (float)stopPoint.Y);
			return graphicsPath;
		}
	}

	public line(PointD start, PointD stop, Layer lay, int index)
	{
		startPoint.X = Math.Round(start.X, 3);
		startPoint.Y = Math.Round(start.Y, 3);
		stopPoint.X = Math.Round(stop.X, 3);
		stopPoint.Y = Math.Round(stop.Y, 3);
		layer = lay;
		objectIndex = index;
		pen = new Pen(color, 1f);
		selectionColor = Color.Gray;
		selectionPen = new Pen(selectionColor, 1f);
		float[] dashPattern = new float[6] { 4f, 4f, 8f, 4f, 12f, 4f };
		selectionPen.DashPattern = dashPattern;
		TPOTS1 = new Point(0, 0);
		TPOTS2 = new Point(0, 0);
		pen1 = new Pen(Color.Violet, 1f);
		ec = new AdjustableArrowCap(20f, 20f, isFilled: false);
	}

	public override void SimpleDraw(Graphics gt, double scale, double bpx, double bpy)
	{
		pen.Color = layer.colorOfLayer;
		TPOTS1.X = (int)((startPoint.X - bpx) / scale);
		TPOTS1.Y = (int)((bpy - startPoint.Y) / scale);
		TPOTS2.X = (int)((stopPoint.X - bpx) / scale);
		TPOTS2.Y = (int)((bpy - stopPoint.Y) / scale);
		gt.DrawLine(pen1, TPOTS1, TPOTS2);
	}

	public override void SimpleDraw(Graphics gt, double scale, double bpx, double bpy, bool print)
	{
		if (!print)
		{
			pen.Color = layer.colorOfLayer;
		}
		else
		{
			pen.Color = Color.Black;
		}
		TPOTS1.X = (int)((startPoint.X - bpx) / scale);
		TPOTS1.Y = (int)((bpy - startPoint.Y) / scale);
		TPOTS2.X = (int)((stopPoint.X - bpx) / scale);
		TPOTS2.Y = (int)((bpy - stopPoint.Y) / scale);
		gt.DrawLine(pen1, TPOTS1, TPOTS2);
	}

	public override void Draw(Graphics gt, double scale, double bpx, double bpy)
	{
		TPOTS1.X = (int)((startPoint.X - bpx) / scale);
		TPOTS1.Y = (int)((bpy - startPoint.Y) / scale);
		TPOTS2.X = (int)((stopPoint.X - bpx) / scale);
		TPOTS2.Y = (int)((bpy - stopPoint.Y) / scale);
		if (!isSelected)
		{
			pen.Color = layer.colorOfLayer;
			gt.DrawLine(pen, TPOTS1, TPOTS2);
			return;
		}
		SnapPoint snapPoint = new SnapPoint(startPoint, scale, Color.Green, 3, 7);
		SnapPoint snapPoint2 = new SnapPoint(stopPoint, scale, Color.Green, 3, 7);
		gt.DrawLine(selectionPen, TPOTS1, TPOTS2);
		snapPoint.Draw(gt, scale, bpx, bpy);
		snapPoint2.Draw(gt, scale, bpx, bpy);
	}

	public override void Draw(Graphics gt, double scale, double bpx, double bpy, bool print)
	{
		TPOTS1.X = (int)((startPoint.X - bpx) / scale);
		TPOTS1.Y = (int)((bpy - startPoint.Y) / scale);
		TPOTS2.X = (int)((stopPoint.X - bpx) / scale);
		TPOTS2.Y = (int)((bpy - stopPoint.Y) / scale);
		if (!print)
		{
			if (!isSelected)
			{
				pen.Color = layer.colorOfLayer;
				gt.DrawLine(pen, TPOTS1, TPOTS2);
				return;
			}
			SnapPoint snapPoint = new SnapPoint(startPoint, scale, Color.Green, 3, 7);
			SnapPoint snapPoint2 = new SnapPoint(stopPoint, scale, Color.Green, 3, 7);
			gt.DrawLine(selectionPen, TPOTS1, TPOTS2);
			snapPoint.Draw(gt, scale, bpx, bpy);
			snapPoint2.Draw(gt, scale, bpx, bpy);
		}
		else
		{
			pen.Color = Color.Black;
			gt.DrawLine(pen, TPOTS1, TPOTS2);
		}
	}

	public override void UpdateDrawingCoords(double scale, double bpx, double bpy)
	{
		TPOTS1.X = (int)((startPoint.X - bpx) / scale);
		TPOTS1.Y = (int)((bpy - startPoint.Y) / scale);
		TPOTS2.X = (int)((stopPoint.X - bpx) / scale);
		TPOTS2.Y = (int)((bpy - stopPoint.Y) / scale);
	}

	public override bool CheckIfIsInSelectionRectangle(PointD firstPoint, PointD secondPoint)
	{
		bool result = false;
		if (((firstPoint.X >= startPoint.X && startPoint.X >= secondPoint.X) || (firstPoint.X <= startPoint.X && startPoint.X <= secondPoint.X)) && ((firstPoint.Y >= startPoint.Y && startPoint.Y >= secondPoint.Y) || (firstPoint.Y <= startPoint.Y && startPoint.Y <= secondPoint.Y)) && ((firstPoint.X >= stopPoint.X && stopPoint.X >= secondPoint.X) || (firstPoint.X <= stopPoint.X && stopPoint.X <= secondPoint.X)) && ((firstPoint.Y >= stopPoint.Y && stopPoint.Y >= secondPoint.Y) || (firstPoint.Y <= stopPoint.Y && stopPoint.Y <= secondPoint.Y)))
		{
			result = true;
		}
		return result;
	}

	public override GraphicsPath graphicsPath(int wide)
	{
		GraphicsPath graphicsPath = new GraphicsPath();
		if (TPOTS1.X != TPOTS2.X || TPOTS1.Y != TPOTS2.Y)
		{
			if (wide != 0 && wide != 100)
			{
				graphicsPath.AddLine(TPOTS1, TPOTS2);
				graphicsPath.Widen(new Pen(Brushes.Black, wide));
			}
			else if (wide != 100)
			{
				graphicsPath.AddLine((float)Math.Round(startPoint.X, 3), (float)Math.Round(startPoint.Y, 3), (float)Math.Round(stopPoint.X, 3), (float)Math.Round(stopPoint.Y, 3));
				graphicsPath.Widen(new Pen(Brushes.Black, 1f));
			}
			else
			{
				graphicsPath.AddLine(TPOTS1, TPOTS2);
			}
		}
		return graphicsPath;
	}

	public Point TransformFromDrawing(PointD p, double base_point_x, double base_point_y, double scale, Bitmap bm)
	{
		return new Point((int)((p.X - base_point_x) / scale - (double)(bm.Width / 3)), (int)((base_point_y - p.Y) / scale - (double)(bm.Height / 3)));
	}

	public override GraphicsPath graphicsPath(int wide, double base_point_x, double base_point_y, double scale, Bitmap bm)
	{
		GraphicsPath graphicsPath = new GraphicsPath();
		if (TPOTS1.X != TPOTS2.X || TPOTS1.Y != TPOTS2.Y)
		{
			if (wide != 0 && wide != 100)
			{
				graphicsPath.AddLine(TransformFromDrawing(startPoint, base_point_x, base_point_y, scale, bm), TransformFromDrawing(stopPoint, base_point_x, base_point_y, scale, bm));
				graphicsPath.Widen(new Pen(Brushes.Black, wide));
			}
			else if (wide != 100)
			{
				graphicsPath.AddLine((float)Math.Round(startPoint.X, 3), (float)Math.Round(startPoint.Y, 3), (float)Math.Round(stopPoint.X, 3), (float)Math.Round(stopPoint.Y, 3));
				graphicsPath.Widen(new Pen(Brushes.Black, 1f));
			}
			else
			{
				graphicsPath.AddLine(TransformFromDrawing(startPoint, base_point_x, base_point_y, scale, bm), TransformFromDrawing(stopPoint, base_point_x, base_point_y, scale, bm));
			}
		}
		return graphicsPath;
	}

	public override object Clone()
	{
		line line2 = (line)MemberwiseClone();
		line2.startPoint = (PointD)startPoint.Clone();
		line2.stopPoint = (PointD)stopPoint.Clone();
		return line2;
	}
}
