using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LindRoof;

[Serializable]
public class PanelObject : ICloneable, IDisposable, IComparable
{
	private double _width;

	private double _height;

	protected bool disposed;

	public double currentScale = 1.0;

	public double currentbpx;

	public double currentbpy;

	private bool initialising = true;

	public PointD startPoint = new PointD();

	public PointD stopPoint = new PointD();

	private PointD intermedPoint = new PointD();

	private ArrayList _objects = new ArrayList();

	public string tipTigla;

	private Layer panelLayer;

	private Layer onduleLayer;

	private Layer selectionLayer;

	private Layer borderMoveLayer;

	private StringFormat sf;

	public bool isSelected;

	public bool topBorderSelected = true;

	private bool doarPentruOptimizare;

	public double pasOndula;

	public double offsetMozaic;

	public double petrecereFoi;

	public double lungimeMinimaFoaieModulRandom;

	public double lungimeMaximaFoaie;

	public double latimeFoaie;

	public double offsetStreasina;

	public int nrMaximOndule;

	public int nrMinimOndule;

	public int numarOndulaStart = -1;

	public int numarColoana = -1;

	private Font txtFont;

	private Point CG;

	private Color stringColor;

	private SolidBrush sb;

	private FontFamily ff;

	public ArrayList objects => _objects;

	public double width => _width;

	public double height => _height;

	public GraphicsPath realGraphicsPath
	{
		get
		{
			GraphicsPath graphicsPath = new GraphicsPath();
			if (objects != null)
			{
				foreach (GraphicObject @object in _objects)
				{
					graphicsPath.AddPath(@object.realGraphicsPath, connect: true);
				}
				graphicsPath.CloseFigure();
			}
			return graphicsPath;
		}
	}

	public double panelArea => _width * _height;

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

	public PanelObject(CaracteristiciPanou specificatiiPanou, bool ptOptimizare, PointD startP, PointD stopP, double yDesfasurarePanouCurent, int rand)
	{
		startPoint = startP;
		stopPoint = stopP;
		offsetStreasina = specificatiiPanou.streasina;
		latimeFoaie = specificatiiPanou.latimeFoaie;
		pasOndula = specificatiiPanou.pasOndula;
		nrMaximOndule = specificatiiPanou.nrMaximOndule;
		nrMinimOndule = specificatiiPanou.nrMinimOndule;
		petrecereFoi = specificatiiPanou.petrecereFoi;
		lungimeMinimaFoaieModulRandom = ((specificatiiPanou.nrMinimOndule == 0) ? 0.0 : pasOndula) + petrecereFoi;
		lungimeMaximaFoaie = (double)nrMaximOndule * pasOndula + petrecereFoi;
		txtFont = specificatiiPanou.txtFont;
		tipTigla = specificatiiPanou.tipTigla;
		panelLayer = specificatiiPanou.panelLayer;
		onduleLayer = specificatiiPanou.onduleLayer;
		selectionLayer = specificatiiPanou.selectionLayer;
		borderMoveLayer = specificatiiPanou.borderMoveLayer;
		sf = specificatiiPanou.sf;
		doarPentruOptimizare = ptOptimizare;
		sb = new SolidBrush(Color.Red);
		numarOndulaStart = (int)Math.Round((startP.Y - yDesfasurarePanouCurent) / pasOndula);
		numarColoana = rand;
		InitializeComponents();
	}

	private void InitializeComponents()
	{
		_width = Math.Round(Math.Abs(startPoint.X - stopPoint.X), 3);
		_height = Math.Round(Math.Abs(startPoint.Y - stopPoint.Y), 3);
		line line2 = new line((PointD)startPoint.Clone(), new PointD(startPoint.X, stopPoint.Y), panelLayer, -99);
		line line3 = new line(new PointD(startPoint.X, stopPoint.Y), (PointD)stopPoint.Clone(), panelLayer, -99);
		line line4 = new line((PointD)stopPoint.Clone(), new PointD(stopPoint.X, startPoint.Y), panelLayer, -99);
		line line5 = new line(new PointD(stopPoint.X, startPoint.Y), (PointD)startPoint.Clone(), panelLayer, -99);
		line2.pen.DashStyle = DashStyle.Dash;
		line3.pen.DashStyle = DashStyle.Dash;
		line4.pen.DashStyle = DashStyle.Dash;
		line5.pen.DashStyle = DashStyle.Dash;
		objects.Add(line2);
		objects.Add(line3);
		objects.Add(line4);
		objects.Add(line5);
	}

	public void DrawContainingObjects(Graphics gr, GraphicTableForPanels parentOfParent, Panel parent, double scale, double base_point_x, double base_point_y, bool print)
	{
		string text = "";
		string panelName = parent.panelName;
		foreach (char c in panelName)
		{
			if (char.IsDigit(c))
			{
				text += c;
			}
		}
		if (!doarPentruOptimizare)
		{
			int num = (int)(_height / pasOndula);
			for (int j = 1; j <= num; j++)
			{
				line line2 = new line(new PointD(startPoint.X, startPoint.Y + (double)j * pasOndula), new PointD(stopPoint.X, startPoint.Y + (double)j * pasOndula), onduleLayer, -99);
				line2.pen.DashStyle = DashStyle.Dot;
				line2.Draw(gr, scale, base_point_x, base_point_y);
			}
		}
		if (!isSelected)
		{
			foreach (GraphicObject @object in objects)
			{
				@object.Draw(gr, scale, base_point_x, base_point_y, print);
			}
			CG.X = (int)((startPoint.X - base_point_x + 3.0) / scale);
			CG.Y = (int)((base_point_y - startPoint.Y - 8.0) / scale);
			sb.Color = ((!print) ? Color.Red : Color.Black);
			bool flag = false;
			if (doarPentruOptimizare)
			{
				return;
			}
			if (!flag)
			{
				flag = true;
				if ((((double)(10f / (float)scale) > 0.1) ? (10f / (float)scale) : 0.1f) != txtFont.Size)
				{
					if (latimeFoaie < 90.0)
					{
						txtFont = new Font(txtFont.FontFamily, (10f / (float)scale * 0.75f > 0.1f) ? (10f / (float)scale * 0.75f) : 0.1f);
					}
					else
					{
						txtFont = new Font(txtFont.FontFamily, ((double)(10f / (float)scale) > 0.1) ? (10f / (float)scale) : 0.1f);
					}
				}
			}
			gr.DrawString(tipTigla + "#" + (parent.panelingObjects.IndexOf(this) + 1) + "/" + text + " \r\nL=" + _height * 10.0 + "mm ", txtFont, sb, CG, sf);
			if (print && scale > 2.0)
			{
				txtFont = new Font(txtFont.FontFamily, 10f);
				StringFormat stringFormat = (StringFormat)sf.Clone();
				stringFormat.FormatFlags = StringFormatFlags.DirectionVertical;
				stringFormat.Alignment = StringAlignment.Center;
				stringFormat.LineAlignment = StringAlignment.Center;
				CG.X += (int)((width / 2.0 - 3.0) / scale);
				CG.Y -= (int)((height / 2.0 + 8.0) / scale);
				gr.DrawString("#" + (parent.panelingObjects.IndexOf(this) + 1) + "/" + text, txtFont, sb, CG, stringFormat);
			}
			return;
		}
		foreach (GraphicObject object2 in objects)
		{
			object2.Draw(gr, scale, base_point_x, base_point_y, print);
		}
		CG.X = (int)((startPoint.X - base_point_x + 3.0) / scale);
		CG.Y = (int)((base_point_y - startPoint.Y - 8.0) / scale);
		sb.Color = ((!print) ? Color.Red : Color.Black);
		if ((((double)(10f / (float)scale) > 0.1) ? (10f / (float)scale) : 0.1f) != txtFont.Size)
		{
			txtFont = new Font(txtFont.FontFamily, ((double)(10f / (float)scale) > 0.1) ? (10f / (float)scale) : 0.1f);
		}
		gr.DrawString(tipTigla + "  #" + (parent.panelingObjects.IndexOf(this) + 1) + "/" + text + " \r\nL=" + _height * 10.0 + "mm", txtFont, sb, CG, sf);
		if (print && scale > 2.0)
		{
			txtFont = new Font(txtFont.FontFamily, 10f);
			StringFormat stringFormat2 = (StringFormat)sf.Clone();
			stringFormat2.FormatFlags = StringFormatFlags.DirectionVertical;
			stringFormat2.Alignment = StringAlignment.Center;
			stringFormat2.LineAlignment = StringAlignment.Center;
			CG.X += (int)((width / 2.0 - 3.0) / scale);
			CG.Y -= (int)((height / 2.0 + 8.0) / scale);
			gr.DrawString("#" + (parent.panelingObjects.IndexOf(this) + 1) + "/" + text, txtFont, sb, CG, stringFormat2);
		}
		if (!print)
		{
			line line3 = (line)((line)objects[0]).Clone();
			line line4 = (line)((line)objects[1]).Clone();
			line line5 = (line)((line)objects[2]).Clone();
			line line6 = (line)((line)objects[3]).Clone();
			line3.layer = selectionLayer;
			line5.layer = selectionLayer;
			if (topBorderSelected)
			{
				line4.layer = borderMoveLayer;
				line4.pen = new Pen(line4.pen.Color, 3f);
				line6.layer = selectionLayer;
			}
			else
			{
				line4.layer = selectionLayer;
				line6.layer = borderMoveLayer;
				line6.pen = new Pen(line6.pen.Color, 3f);
			}
			line3.startPoint.X += 10.0;
			line3.stopPoint.X += 10.0;
			line4.startPoint.Y -= 10.0;
			line4.stopPoint.Y -= 10.0;
			line5.startPoint.X -= 10.0;
			line5.stopPoint.X -= 10.0;
			line6.startPoint.Y += 10.0;
			line6.stopPoint.Y += 10.0;
			line3.pen.DashStyle = DashStyle.Dash;
			line4.pen.DashStyle = DashStyle.Dash;
			line5.pen.DashStyle = DashStyle.Dash;
			line6.pen.DashStyle = DashStyle.Dash;
			line3.Draw(gr, scale, base_point_x, base_point_y);
			line4.Draw(gr, scale, base_point_x, base_point_y);
			line5.Draw(gr, scale, base_point_x, base_point_y);
			line6.Draw(gr, scale, base_point_x, base_point_y);
		}
	}

	public override string ToString()
	{
		return "Start point: " + startPoint.ToString() + " Width:" + _width + " Height:" + _height;
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

	public GraphicsPath graphicsPath(double scale, double base_point_x, double base_point_y)
	{
		GraphicsPath graphicsPath = new GraphicsPath(FillMode.Alternate);
		if (objects != null)
		{
			foreach (GraphicObject @object in _objects)
			{
				@object.UpdateDrawingCoords(scale, base_point_x, base_point_y);
				graphicsPath.AddPath(@object.graphicsPath(0), connect: true);
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

	public bool CheckIfHitted(PointD mouseD)
	{
		bool result = false;
		if (startPoint.X < mouseD.X && mouseD.X < stopPoint.X && startPoint.Y < mouseD.Y && mouseD.Y < stopPoint.Y)
		{
			if (!isSelected)
			{
				if (Math.Abs(startPoint.Y - mouseD.Y) >= Math.Abs(stopPoint.Y - mouseD.Y))
				{
					topBorderSelected = true;
				}
				else
				{
					topBorderSelected = false;
				}
				isSelected = true;
			}
			else
			{
				isSelected = false;
			}
			result = true;
		}
		return result;
	}

	public void UpdateLines()
	{
		_width = Math.Round(Math.Abs(startPoint.X - stopPoint.X), 3);
		_height = Math.Round(Math.Abs(startPoint.Y - stopPoint.Y), 3);
		((line)_objects[0]).startPoint = (PointD)startPoint.Clone();
		((line)_objects[0]).stopPoint = new PointD(startPoint.X, stopPoint.Y);
		((line)_objects[1]).startPoint = new PointD(startPoint.X, stopPoint.Y);
		((line)_objects[1]).stopPoint = (PointD)stopPoint.Clone();
		((line)_objects[2]).startPoint = (PointD)stopPoint.Clone();
		((line)_objects[2]).stopPoint = new PointD(stopPoint.X, startPoint.Y);
		((line)_objects[3]).startPoint = new PointD(stopPoint.X, startPoint.Y);
		((line)_objects[3]).stopPoint = (PointD)startPoint.Clone();
	}

	public bool Change(bool goingUp, int offset)
	{
		bool result = true;
		if (topBorderSelected)
		{
			if (goingUp)
			{
				if (stopPoint.Y - startPoint.Y <= lungimeMaximaFoaie - (double)offset)
				{
					stopPoint.Y += offset;
					((line)_objects[1]).startPoint.Y += offset;
					((line)_objects[1]).stopPoint.Y += offset;
					((line)_objects[0]).stopPoint.Y += offset;
					((line)_objects[2]).startPoint.Y += offset;
				}
				else if (stopPoint.Y - startPoint.Y == lungimeMaximaFoaie)
				{
					result = false;
				}
				else
				{
					stopPoint.Y = startPoint.Y + lungimeMaximaFoaie;
					((line)_objects[1]).startPoint.Y = stopPoint.Y;
					((line)_objects[1]).stopPoint.Y = stopPoint.Y;
					((line)_objects[0]).stopPoint.Y = stopPoint.Y;
					((line)_objects[2]).startPoint.Y = stopPoint.Y;
				}
			}
			else if (stopPoint.Y - startPoint.Y >= lungimeMinimaFoaieModulRandom + (double)offset)
			{
				stopPoint.Y -= offset;
				((line)_objects[1]).startPoint.Y -= offset;
				((line)_objects[1]).stopPoint.Y -= offset;
				((line)_objects[0]).stopPoint.Y -= offset;
				((line)_objects[2]).startPoint.Y -= offset;
			}
			else
			{
				result = false;
			}
		}
		else if (nrMinimOndule > 0)
		{
			double num = pasOndula;
			if (goingUp)
			{
				if (stopPoint.Y - startPoint.Y >= lungimeMinimaFoaieModulRandom + pasOndula)
				{
					startPoint.Y += num;
					((line)_objects[3]).startPoint.Y += num;
					((line)_objects[3]).stopPoint.Y += num;
					((line)_objects[0]).startPoint.Y += num;
					((line)_objects[2]).stopPoint.Y += num;
				}
				else
				{
					result = false;
				}
			}
			else if (stopPoint.Y - startPoint.Y <= lungimeMaximaFoaie - pasOndula)
			{
				startPoint.Y -= num;
				((line)_objects[3]).startPoint.Y -= num;
				((line)_objects[3]).stopPoint.Y -= num;
				((line)_objects[0]).startPoint.Y -= num;
				((line)_objects[2]).stopPoint.Y -= num;
			}
			else
			{
				result = false;
			}
		}
		else if (goingUp)
		{
			if (stopPoint.Y - startPoint.Y >= lungimeMinimaFoaieModulRandom + (double)offset)
			{
				startPoint.Y += offset;
				((line)_objects[3]).startPoint.Y += offset;
				((line)_objects[3]).stopPoint.Y += offset;
				((line)_objects[0]).startPoint.Y += offset;
				((line)_objects[2]).stopPoint.Y += offset;
			}
			else
			{
				result = false;
			}
		}
		else if (stopPoint.Y - startPoint.Y <= lungimeMaximaFoaie - (double)offset)
		{
			startPoint.Y -= offset;
			((line)_objects[3]).startPoint.Y -= offset;
			((line)_objects[3]).stopPoint.Y -= offset;
			((line)_objects[0]).startPoint.Y -= offset;
			((line)_objects[2]).stopPoint.Y -= offset;
		}
		else if (stopPoint.Y - startPoint.Y == lungimeMaximaFoaie)
		{
			result = false;
		}
		else
		{
			startPoint.Y = stopPoint.Y - lungimeMaximaFoaie;
			((line)_objects[3]).startPoint.Y = startPoint.Y;
			((line)_objects[3]).stopPoint.Y = startPoint.Y;
			((line)_objects[0]).startPoint.Y = startPoint.Y;
			((line)_objects[2]).stopPoint.Y = startPoint.Y;
		}
		_height = Math.Round(Math.Abs(startPoint.Y - stopPoint.Y), 3);
		return result;
	}

	~PanelObject()
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

	public int CompareTo(object obj)
	{
		PanelObject panelObject = (PanelObject)obj;
		return height.CompareTo(panelObject.height);
	}

	public object Clone(PointD startPoint, PointD stopPoint)
	{
		isSelected = false;
		PanelObject panelObject = (PanelObject)MemberwiseClone();
		panelObject.startPoint = startPoint;
		panelObject.stopPoint = stopPoint;
		panelObject.panelLayer = panelLayer;
		panelObject.onduleLayer = onduleLayer;
		panelObject.selectionLayer = selectionLayer;
		panelObject.borderMoveLayer = borderMoveLayer;
		panelObject.sf = sf;
		panelObject._width = Math.Round(Math.Abs(panelObject.startPoint.X - panelObject.stopPoint.X), 3);
		panelObject._height = Math.Round(Math.Abs(panelObject.startPoint.Y - panelObject.stopPoint.Y), 3);
		line line2 = new line((PointD)panelObject.startPoint.Clone(), new PointD(panelObject.startPoint.X, panelObject.stopPoint.Y), panelObject.panelLayer, -99);
		line line3 = new line(new PointD(panelObject.startPoint.X, panelObject.stopPoint.Y), (PointD)panelObject.stopPoint.Clone(), panelObject.panelLayer, -99);
		line line4 = new line((PointD)panelObject.stopPoint.Clone(), new PointD(panelObject.stopPoint.X, panelObject.startPoint.Y), panelObject.panelLayer, -99);
		line line5 = new line(new PointD(panelObject.stopPoint.X, panelObject.startPoint.Y), (PointD)panelObject.startPoint.Clone(), panelObject.panelLayer, -99);
		line2.pen.DashStyle = DashStyle.Dash;
		line3.pen.DashStyle = DashStyle.Dash;
		line4.pen.DashStyle = DashStyle.Dash;
		line5.pen.DashStyle = DashStyle.Dash;
		panelObject._objects = new ArrayList();
		panelObject.objects.Add(line2);
		panelObject.objects.Add(line3);
		panelObject.objects.Add(line4);
		panelObject.objects.Add(line5);
		return panelObject;
	}

	public object Clone()
	{
		return MemberwiseClone();
	}
}
