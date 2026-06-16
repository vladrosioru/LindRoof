using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LindRoof;

[Serializable]
public class Panel : IDisposable, ICloneable
{
	public double lungimeMinimaFoaie = 90.5;

	public double lungimeMinimaFoaieModulRandom = 50.5;

	public double lungimeMaximaFoaie = 610.5;

	public CaracteristiciPanou tipPanou = new CaracteristiciPanou(nimic: true);

	public double panelAngle;

	protected bool disposed;

	public double currentScale = 1.0;

	public double currentbpx;

	public double currentbpy;

	public bool evolved;

	private bool initialising = true;

	public PointD startPoint = new PointD();

	public PointD stopPoint = new PointD();

	private PointD intermedPoint = new PointD();

	private ArrayList _objects = new ArrayList();

	private Layer panelLayer;

	public string panelName;

	public GraphicObject bendingObject;

	public bool standardPanels;

	public ArrayList panelingObjects = new ArrayList();

	public ArrayList voidList = new ArrayList();

	public ArrayList objects => _objects;

	public GraphicsPath realGraphicsPath
	{
		get
		{
			GraphicsPath graphicsPath = new GraphicsPath();
			if (objects != null)
			{
				foreach (GraphicObject @object in _objects)
				{
					try
					{
						graphicsPath.AddPath(@object.realGraphicsPath, connect: true);
					}
					catch
					{
					}
				}
				graphicsPath.CloseFigure();
			}
			return graphicsPath;
		}
	}

	public GraphicsPath graphicsPath2
	{
		get
		{
			GraphicsPath graphicsPath = new GraphicsPath(FillMode.Alternate);
			if (objects != null)
			{
				foreach (GraphicObject @object in _objects)
				{
					try
					{
						graphicsPath.AddPath(@object.graphicsPath(100), connect: true);
					}
					catch
					{
					}
				}
				graphicsPath.CloseFigure();
			}
			return graphicsPath;
		}
	}

	public double panelArea
	{
		get
		{
			double num = 0.0;
			if (objects != null)
			{
				bool flag = true;
				double num2 = 0.0;
				double num3 = 0.0;
				double num4 = 0.0;
				double num5 = 0.0;
				double num6 = 0.0;
				double num7 = 0.0;
				foreach (GraphicObject @object in _objects)
				{
					if (flag)
					{
						num2 = @object.startPoint.X;
						num3 = @object.startPoint.Y;
						num4 = num2;
						num5 = num3;
						flag = false;
					}
					else
					{
						num6 = @object.startPoint.X;
						num7 = @object.startPoint.Y;
						num += num4 * num7 - num6 * num5;
						num4 = num6;
						num5 = num7;
					}
				}
				num += num4 * num3 - num2 * num5;
				num /= 2.0;
				num = Math.Abs(num);
			}
			return num;
		}
	}

	public PointD centerOfGravity
	{
		get
		{
			PointD pointD = new PointD();
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			if (objects != null)
			{
				bool flag = true;
				double num4 = 0.0;
				double num5 = 0.0;
				double num6 = 0.0;
				double num7 = 0.0;
				double num8 = 0.0;
				double num9 = 0.0;
				foreach (GraphicObject @object in _objects)
				{
					if (flag)
					{
						num4 = @object.startPoint.X;
						num5 = @object.startPoint.Y;
						num6 = num4;
						num7 = num5;
						flag = false;
					}
					else
					{
						num8 = @object.startPoint.X;
						num9 = @object.startPoint.Y;
						num2 += (num6 + num8) * (num6 * num9 - num8 * num7);
						num3 += (num7 + num9) * (num6 * num9 - num8 * num7);
						num += num6 * num9 - num8 * num7;
						num6 = num8;
						num7 = num9;
					}
				}
				num2 += (num6 + num4) * (num6 * num5 - num4 * num7);
				num3 += (num7 + num5) * (num6 * num5 - num4 * num7);
				num += num6 * num5 - num4 * num7;
				num /= 2.0;
				num2 /= 6.0 * num;
				num3 /= 6.0 * num;
				pointD.X = num2;
				pointD.Y = num3;
			}
			return pointD;
		}
	}

	public double perimeter
	{
		get
		{
			double num = 0.0;
			foreach (GraphicObject @object in objects)
			{
				num += @object.length;
			}
			return num;
		}
	}

	public Panel(Layer lay, string name)
	{
		panelLayer = lay;
		panelName = name;
	}

	public Panel()
	{
	}

	public void Reset()
	{
	}

	public override string ToString()
	{
		return panelName;
	}

	public void RemoveObject(GraphicObject obj)
	{
		_objects.Remove(obj);
	}

	public void RemoveObject(string all)
	{
		foreach (GraphicObject @object in _objects)
		{
			_objects.Remove(@object);
		}
	}

	public void rotateObjects(GraphicObject obj)
	{
		double num = obj.stopPoint.X - obj.startPoint.X;
		double num2 = obj.stopPoint.Y - obj.startPoint.Y;
		double num3 = num2 / obj.length;
		double num4 = num / obj.length;
		for (int i = 0; i < objects.Count; i++)
		{
			double num5 = distance(obj.startPoint, ((GraphicObject)objects[i]).startPoint);
			if (num5 != 0.0)
			{
				double num6 = ((GraphicObject)objects[i]).startPoint.X - obj.startPoint.X;
				double num7 = ((GraphicObject)objects[i]).startPoint.Y - obj.startPoint.Y;
				double num8 = num7 / num5;
				double num9 = num6 / num5;
				double num10 = num8 * num4 - num9 * num3;
				double num11 = num9 * num4 + num8 * num3;
				double num12 = num5 * num11;
				double num13 = num5 * num10;
				((GraphicObject)objects[i]).startPoint.X = obj.startPoint.X + num12;
				((GraphicObject)objects[i]).startPoint.Y = obj.startPoint.Y + num13;
			}
			num5 = distance(obj.startPoint, ((GraphicObject)objects[i]).stopPoint);
			if (num5 != 0.0)
			{
				double num14 = ((GraphicObject)objects[i]).stopPoint.X - obj.startPoint.X;
				double num15 = ((GraphicObject)objects[i]).stopPoint.Y - obj.startPoint.Y;
				double num16 = num15 / num5;
				double num17 = num14 / num5;
				double num18 = num16 * num4 - num17 * num3;
				double num19 = num17 * num4 + num16 * num3;
				double num20 = num5 * num19;
				double num21 = num5 * num18;
				((GraphicObject)objects[i]).stopPoint.X = obj.startPoint.X + num20;
				((GraphicObject)objects[i]).stopPoint.Y = obj.startPoint.Y + num21;
			}
		}
	}

	public void rotateObjects(PointD thePoint, double sin, double cos)
	{
		for (int i = 0; i < objects.Count; i++)
		{
			double num = distance(thePoint, ((GraphicObject)objects[i]).startPoint);
			if (num != 0.0)
			{
				double num2 = ((GraphicObject)objects[i]).startPoint.X - thePoint.X;
				double num3 = ((GraphicObject)objects[i]).startPoint.Y - thePoint.Y;
				double num4 = num3 / num;
				double num5 = num2 / num;
				double num6 = num4 * cos - num5 * sin;
				double num7 = num5 * cos + num4 * sin;
				double num8 = num * num7;
				double num9 = num * num6;
				((GraphicObject)objects[i]).startPoint.X = thePoint.X + num8;
				((GraphicObject)objects[i]).startPoint.Y = thePoint.Y + num9;
			}
			num = distance(thePoint, ((GraphicObject)objects[i]).stopPoint);
			if (num != 0.0)
			{
				double num10 = ((GraphicObject)objects[i]).stopPoint.X - thePoint.X;
				double num11 = ((GraphicObject)objects[i]).stopPoint.Y - thePoint.Y;
				double num12 = num11 / num;
				double num13 = num10 / num;
				double num14 = num12 * cos - num13 * sin;
				double num15 = num13 * cos + num12 * sin;
				double num16 = num * num15;
				double num17 = num * num14;
				((GraphicObject)objects[i]).stopPoint.X = thePoint.X + num16;
				((GraphicObject)objects[i]).stopPoint.Y = thePoint.Y + num17;
			}
		}
	}

	public void sortVertexex()
	{
		bool flag = true;
		PointD pointD = new PointD();
		foreach (GraphicObject @object in _objects)
		{
			if (flag)
			{
				pointD = @object.stopPoint;
				flag = false;
			}
			else if (@object.startPoint.X != pointD.X && @object.startPoint.Y != pointD.Y)
			{
				PointD pointD2 = @object.startPoint;
				@object.startPoint = @object.stopPoint;
				@object.stopPoint = pointD2;
			}
		}
	}

	public void Add(GraphicObject obj)
	{
		GraphicObject graphicObject = (GraphicObject)obj.Clone();
		if (initialising)
		{
			startPoint = graphicObject.startPoint;
			stopPoint = graphicObject.stopPoint;
			_objects.Add(graphicObject);
			initialising = false;
			return;
		}
		double num = Math.Min(Math.Min(distance(startPoint, graphicObject.startPoint), distance(startPoint, graphicObject.stopPoint)), Math.Min(distance(stopPoint, graphicObject.startPoint), distance(stopPoint, graphicObject.stopPoint)));
		if (num == distance(stopPoint, graphicObject.startPoint))
		{
			if (stopPoint.X != graphicObject.startPoint.X || stopPoint.Y != graphicObject.startPoint.Y)
			{
				line value = new line(stopPoint, graphicObject.startPoint, panelLayer, 0);
				_objects.Add(value);
			}
			stopPoint = graphicObject.stopPoint;
			_objects.Add(graphicObject);
		}
		else if (num == distance(stopPoint, graphicObject.stopPoint))
		{
			if (stopPoint.X != graphicObject.stopPoint.X || stopPoint.Y != graphicObject.stopPoint.Y)
			{
				line value2 = new line(stopPoint, graphicObject.stopPoint, panelLayer, 0);
				_objects.Add(value2);
			}
			intermedPoint = graphicObject.startPoint;
			graphicObject.startPoint = graphicObject.stopPoint;
			graphicObject.stopPoint = intermedPoint;
			stopPoint = graphicObject.stopPoint;
			_objects.Add(graphicObject);
		}
		else if (num == distance(startPoint, graphicObject.stopPoint))
		{
			if (startPoint.X != graphicObject.stopPoint.X || startPoint.Y != graphicObject.stopPoint.Y)
			{
				line value3 = new line(graphicObject.stopPoint, startPoint, panelLayer, 0);
				_objects.Add(value3);
			}
			startPoint = graphicObject.startPoint;
			_objects.Insert(0, graphicObject);
		}
		else if (num == distance(startPoint, graphicObject.startPoint))
		{
			if (startPoint.X != graphicObject.startPoint.X || startPoint.Y != graphicObject.startPoint.Y)
			{
				line value4 = new line(graphicObject.startPoint, startPoint, panelLayer, 0);
				_objects.Add(value4);
			}
			intermedPoint = graphicObject.startPoint;
			graphicObject.startPoint = graphicObject.stopPoint;
			graphicObject.stopPoint = intermedPoint;
			startPoint = graphicObject.startPoint;
			_objects.Insert(0, graphicObject);
		}
	}

	public GraphicsPath graphicsPath1(int width)
	{
		GraphicsPath graphicsPath = new GraphicsPath(FillMode.Alternate);
		if (objects != null)
		{
			foreach (GraphicObject @object in _objects)
			{
				graphicsPath.AddPath(@object.graphicsPath(0), connect: true);
			}
			graphicsPath.CloseFigure();
		}
		return graphicsPath;
	}

	public GraphicsPath graphicsPath(int wide, double base_point_x, double base_point_y, double scale, Bitmap bm)
	{
		GraphicsPath graphicsPath = new GraphicsPath(FillMode.Alternate);
		if (objects != null)
		{
			foreach (GraphicObject @object in _objects)
			{
				try
				{
					graphicsPath.AddPath(@object.graphicsPath(wide, base_point_x, base_point_y, scale, bm), connect: true);
				}
				catch
				{
				}
			}
			graphicsPath.CloseFigure();
		}
		return graphicsPath;
	}

	private double distance(PointD firstP, PointD secondP)
	{
		double num = secondP.X - firstP.X;
		double num2 = secondP.Y - firstP.Y;
		return Math.Sqrt(num * num + num2 * num2);
	}

	public void DrawTableSheets(Graphics gr, GraphicTableForPanels parent, double scale, double base_point_x, double base_point_y, bool print)
	{
		foreach (GraphicObject @object in objects)
		{
			@object.UpdateDrawingCoords(scale, base_point_x, base_point_y);
		}
		foreach (PanelObject panelingObject in panelingObjects)
		{
			panelingObject.DrawContainingObjects(gr, parent, this, scale, base_point_x, base_point_y, print);
		}
	}

	~Panel()
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
		if (disposed)
		{
			return;
		}
		if (disposing)
		{
			startPoint.Dispose();
			stopPoint.Dispose();
			intermedPoint.Dispose();
			foreach (GraphicObject @object in _objects)
			{
				@object.Dispose();
			}
		}
		disposed = true;
	}

	public object Clone()
	{
		try
		{
			Panel panel = (Panel)MemberwiseClone();
			panel.startPoint = (PointD)startPoint.Clone();
			panel.stopPoint = (PointD)stopPoint.Clone();
			panel.intermedPoint = (PointD)intermedPoint.Clone();
			panel._objects = (ArrayList)_objects.Clone();
			for (int i = 0; i < panel._objects.Count; i++)
			{
				panel._objects[i] = ((GraphicObject)_objects[i]).Clone();
			}
			return panel;
		}
		catch
		{
			return null;
		}
	}
}
