using System.Drawing;

namespace LindRoof;

public class SnapPoint : GraphicObject
{
	private PointD firstPointD = new PointD();

	private PointD secondPointD = new PointD();

	private PointD thirdPointD = new PointD();

	private PointD fourthPointD = new PointD();

	public int width;

	public int dimension;

	public SnapPoint()
	{
	}

	public SnapPoint(PointD centralPoint, double scale, Color col, int wid, int dim)
	{
		dimension = dim;
		startPoint = centralPoint;
		firstPointD.X = startPoint.X - (double)(dimension / 2) * scale;
		firstPointD.Y = startPoint.Y + (double)(dimension / 2) * scale;
		secondPointD.X = startPoint.X + (double)(dimension / 2) * scale;
		secondPointD.Y = startPoint.Y + (double)(dimension / 2) * scale;
		thirdPointD.X = startPoint.X + (double)(dimension / 2) * scale;
		thirdPointD.Y = startPoint.Y - (double)(dimension / 2) * scale;
		fourthPointD.X = startPoint.X - (double)(dimension / 2) * scale;
		fourthPointD.Y = startPoint.Y - (double)(dimension / 2) * scale;
		color = col;
		width = wid;
		pen = new Pen(color, width);
	}

	public override void Draw(Graphics gt, double scale, double bpx, double bpy)
	{
		Point point = new Point((int)((firstPointD.X - bpx) / scale), (int)((bpy - firstPointD.Y) / scale));
		Point point2 = new Point((int)((secondPointD.X - bpx) / scale), (int)((bpy - secondPointD.Y) / scale));
		Point point3 = new Point((int)((thirdPointD.X - bpx) / scale), (int)((bpy - thirdPointD.Y) / scale));
		Point point4 = new Point((int)((fourthPointD.X - bpx) / scale), (int)((bpy - fourthPointD.Y) / scale));
		gt.DrawLines(pen, new Point[6] { point, point2, point3, point4, point, point2 });
	}
}
