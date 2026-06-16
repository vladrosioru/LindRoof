using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LindRoof;

[Serializable]
public class GraphicObject : IDisposable, ICloneable, IComparable
{
	protected bool disposed;

	public Layer layer;

	public Color color;

	public PointD startPoint = new PointD();

	public PointD stopPoint = new PointD();

	public Pen pen;

	public Color selectionColor;

	public Pen selectionPen;

	public double _length;

	public bool isSelected;

	public int objectIndex;

	public virtual double length => 0.0;

	public virtual GraphicsPath realGraphicsPath => null;

	~GraphicObject()
	{
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing)
			{
				startPoint.Dispose();
				stopPoint.Dispose();
			}
			disposed = true;
		}
	}

	public virtual void Draw(Graphics gt, double scale, double base_point_x, double base_point_y)
	{
	}

	public virtual void Draw(Graphics gt, double scale, double base_point_x, double base_point_y, bool print)
	{
	}

	public virtual void SimpleDraw(Graphics gt, double scale, double base_point_x, double base_point_y)
	{
	}

	public virtual void SimpleDraw(Graphics gt, double scale, double base_point_x, double base_point_y, bool print)
	{
	}

	public virtual void UpdateDrawingCoords(double scale, double base_point_x, double base_point_y)
	{
	}

	public virtual bool CheckIfIsInSelectionRectangle(PointD firstPoint, PointD secondPoint)
	{
		return false;
	}

	public virtual GraphicsPath graphicsPath(int wide)
	{
		return null;
	}

	public virtual GraphicsPath graphicsPath(int wide, double base_point_x, double base_point_y, double scale, Bitmap bm)
	{
		return null;
	}

	void IDisposable.Dispose()
	{
	}

	public virtual object Clone()
	{
		return null;
	}

	public int CompareTo(object obj)
	{
		GraphicObject graphicObject = (GraphicObject)obj;
		return objectIndex.CompareTo(graphicObject.objectIndex);
	}
}
