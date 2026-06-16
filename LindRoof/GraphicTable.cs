using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using LindRoof.TextualEnvironment;

namespace LindRoof;

public class GraphicTable : UserControl
{
	public SolidBrush culoarePanouri = new SolidBrush(Color.DarkRed);

	private SolidBrush culoareTextPanouri;

	private Container components;

	private Bitmap bm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

	public double base_point_x;

	public double base_point_y;

	public double scale = 1.0;

	private int start_translate_x;

	private int start_translate_y;

	private int origin_x;

	private int origin_y;

	private bool middleDown;

	public Color background = Color.Black;

	private int sizeOfTable_x;

	private int sizeOfTable_y;

	private string x_axis = "X";

	private string y_axis = "Y";

	private Font theFont = new Font("Arial", 14f);

	private SolidBrush theBrush = new SolidBrush(Color.White);

	private StringFormat theStringFormat = new StringFormat();

	private Pen pen = new Pen(Color.White, 1f);

	private bool amIdrawingTheOrigin = true;

	public ArrayList objects = new ArrayList();

	public ArrayList panels = new ArrayList();

	public ArrayList temporary = new ArrayList();

	public ArrayList panelsForDeveloping = new ArrayList();

	public PointD startPoint;

	public PointD stopPoint;

	private bool startCommand = true;

	private bool hasThePoint;

	public Layer currentLayer = new Layer("Default", Color.Gray);

	public Panel currentPanel;

	public double lastLength;

	public double currentLength;

	public Point firstPoint;

	public Point secondPoint;

	private int radius = 15;

	private double radiusD;

	public bool paning;

	public bool needRedraw;

	public int objectIndex = 1;

	public int panelNo;

	private string unit = "m";

	public float unitscale = 0.01f;

	private StringFormat sf = new StringFormat();

	private PointD mouseD = new PointD();

	private bool iAmSelecting;

	private PointD selectionFirstPoint;

	private PointD selectionSecondPoint;

	public bool amIdrawingTheGrid = true;

	private double gridDistance = 50.0;

	private PrintDocument pd = new PrintDocument();

	private Bitmap primaryGridPointBM = new Bitmap(1, 1);

	private MainWindow mainwindow;

	public Stack undo_stack = new Stack(32);

	public Stack redo_stack = new Stack(32);

	public Color snapPointColor = Color.Red;

	public int snapPointWidth = 2;

	public int snapPointDimension = 10;

	private PointD theSnapPointD = new PointD();

	private double minimumLength;

	private double checkMinimumIfChanged;

	public Color caretLineColor = Color.Red;

	private float[] caretLinePattern = new float[6] { 8f, 8f, 8f, 8f, 8f, 8f };

	private Pen caretLinePen;

	public Color rectangleLineColor = Color.Green;

	private Pen rectangleLinePen;

	private GraphicObject firstSelectedObject;

	private GraphicObject secondSelectedObject;

	private PointD mouse1;

	private PointD mouse2;

	private ContextMenu contextMenu;

	private dictionar_cuvinte dictionar;

	public bool AmIdrawingTheOrigin
	{
		get
		{
			return amIdrawingTheOrigin;
		}
		set
		{
			amIdrawingTheOrigin = value;
			RedrawAll();
		}
	}

	public ArrayList giveMePanels
	{
		get
		{
			ArrayList arrayList = new ArrayList();
			if (panels != null)
			{
				foreach (Panel panel in panels)
				{
					arrayList.Add(panel.panelName);
				}
			}
			return arrayList;
		}
	}

	public GraphicTable(MainWindow mainwnd)
	{
		mainwindow = mainwnd;
		dictionar = mainwindow.dictionar;
		InitializeComponent();
		culoareTextPanouri = new SolidBrush(Color.FromArgb(255 - background.R, 255 - background.G, 255 - background.B));
		RedrawAll();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void GraphicTable_MouseDown(object sender, MouseEventArgs e)
	{
		switch (e.Button.ToString())
		{
		case "Left":
			if (((Document)base.ParentForm).enableCommand)
			{
				switch (((Document)base.ParentForm).activeCommand)
				{
				case 1:
				{
					if (startCommand)
					{
						startPoint = TransformFromScreen(new Point(e.X, e.Y));
						startCommand = false;
						if (mainwindow.statusBarOsnap.BorderStyle == StatusBarPanelBorderStyle.Sunken || mainwindow.statusBarSnap.BorderStyle == StatusBarPanelBorderStyle.Sunken)
						{
							CheckSnapPoint(startPoint);
							if (hasThePoint)
							{
								startPoint = theSnapPointD;
							}
						}
						break;
					}
					PointD mouse2 = TransformFromScreen(new Point(e.X, e.Y));
					if (mainwindow.statusBarOsnap.BorderStyle == StatusBarPanelBorderStyle.Sunken || mainwindow.statusBarSnap.BorderStyle == StatusBarPanelBorderStyle.Sunken)
					{
						CheckSnapPoint(mouse2);
						if (hasThePoint)
						{
							CastingStopPointOfTheLine(theSnapPointD);
						}
						else
						{
							CastingStopPointOfTheLine(secondPoint);
						}
					}
					else
					{
						CastingStopPointOfTheLine(secondPoint);
					}
					break;
				}
				case 4:
				{
					if (startCommand)
					{
						startPoint = TransformFromScreen(new Point(e.X, e.Y));
						startCommand = false;
						if (mainwindow.statusBarOsnap.BorderStyle == StatusBarPanelBorderStyle.Sunken || mainwindow.statusBarSnap.BorderStyle == StatusBarPanelBorderStyle.Sunken)
						{
							CheckSnapPoint(startPoint);
							if (hasThePoint)
							{
								startPoint = theSnapPointD;
							}
						}
						break;
					}
					PointD mouse3 = TransformFromScreen(new Point(e.X, e.Y));
					if (mainwindow.statusBarOsnap.BorderStyle == StatusBarPanelBorderStyle.Sunken || mainwindow.statusBarSnap.BorderStyle == StatusBarPanelBorderStyle.Sunken)
					{
						CheckSnapPoint(mouse3);
						if (hasThePoint)
						{
							CastingAPointOfThePolyline(theSnapPointD);
						}
						else
						{
							CastingAPointOfThePolyline(secondPoint);
						}
					}
					else
					{
						CastingAPointOfThePolyline(secondPoint);
					}
					break;
				}
				case 9:
				{
					if (startCommand)
					{
						GraphicObject graphicObject3 = CheckHittingTestofAnObjectAndReturnTheObject(new Point(e.X, e.Y));
						if (graphicObject3 != null)
						{
							startCommand = false;
							firstSelectedObject = graphicObject3;
							mouse1 = TransformFromScreen(new Point(e.X, e.Y));
							RedrawAll();
						}
						break;
					}
					GraphicObject graphicObject4 = CheckHittingTestofAnObjectAndReturnTheObject(new Point(e.X, e.Y));
					if (graphicObject4 != null && graphicObject4 != firstSelectedObject)
					{
						startCommand = true;
						secondSelectedObject = graphicObject4;
						this.mouse2 = TransformFromScreen(new Point(e.X, e.Y));
						CheckObjectsForFiletingAndFiletThemIfTrue(firstSelectedObject, secondSelectedObject);
						Cursor = Cursors.Cross;
					}
					break;
				}
				case 11:
					if (startCommand)
					{
						GraphicObject graphicObject = CheckHittingTestofAnObjectAndReturnTheObject(new Point(e.X, e.Y));
						if (graphicObject != null)
						{
							currentPanel.objects.Clear();
							currentPanel.RemoveObject("all");
							currentPanel.Add(graphicObject);
							startCommand = false;
							RedrawAll();
						}
					}
					else
					{
						GraphicObject graphicObject2 = CheckHittingTestofAnObjectAndReturnTheObject(new Point(e.X, e.Y));
						if (graphicObject2 != null)
						{
							currentPanel.Add(graphicObject2);
							RedrawAll();
						}
					}
					break;
				case 13:
				{
					if (startCommand)
					{
						startPoint = TransformFromScreen(new Point(e.X, e.Y));
						startCommand = false;
						if (mainwindow.statusBarOsnap.BorderStyle == StatusBarPanelBorderStyle.Sunken || mainwindow.statusBarSnap.BorderStyle == StatusBarPanelBorderStyle.Sunken)
						{
							CheckSnapPoint(startPoint);
							if (hasThePoint)
							{
								startPoint = theSnapPointD;
							}
						}
						break;
					}
					PointD mouse = TransformFromScreen(new Point(e.X, e.Y));
					if (mainwindow.statusBarOsnap.BorderStyle == StatusBarPanelBorderStyle.Sunken || mainwindow.statusBarSnap.BorderStyle == StatusBarPanelBorderStyle.Sunken)
					{
						CheckSnapPoint(mouse);
						if (hasThePoint)
						{
							CastingStopPointOfTheRectangle(theSnapPointD);
						}
						else
						{
							CastingStopPointOfTheRectangle(secondPoint);
						}
					}
					else
					{
						CastingStopPointOfTheRectangle(secondPoint);
					}
					break;
				}
				}
			}
			else
			{
				if (objects == null)
				{
					break;
				}
				if (!iAmSelecting)
				{
					if (!CheckHittingTestofAnObject(new Point(e.X, e.Y)))
					{
						selectionFirstPoint = TransformFromScreen(new Point(e.X, e.Y));
						iAmSelecting = true;
					}
					break;
				}
				selectionSecondPoint = TransformFromScreen(new Point(e.X, e.Y));
				iAmSelecting = false;
				if (CheckForObjectsInSelectedArea(selectionFirstPoint, selectionSecondPoint))
				{
					RedrawAll();
				}
			}
			break;
		case "Middle":
			switch (e.Clicks)
			{
			case 1:
				start_translate_x = e.X;
				start_translate_y = e.Y;
				Cursor.Current = Cursors.Hand;
				middleDown = true;
				needRedraw = true;
				break;
			case 2:
				ZoomExtents();
				break;
			}
			break;
		case "Right":
			CheckCommandForEnding();
			break;
		}
	}

	private void GraphicTable_MouseMove(object sender, MouseEventArgs e)
	{
		if (middleDown)
		{
			base_point_x -= (double)(e.X - start_translate_x) * scale;
			base_point_y += (double)(e.Y - start_translate_y) * scale;
			start_translate_x = e.X;
			start_translate_y = e.Y;
			paning = true;
			RedrawAll();
			paning = false;
		}
		else if (((Document)base.ParentForm).enableCommand)
		{
			PasteContent();
			switch (((Document)base.ParentForm).activeCommand)
			{
			case 1:
			case 4:
				if (!startCommand)
				{
					firstPoint = TransformFromDrawing(startPoint);
					switch (mainwindow.statusBarOrtho.BorderStyle.ToString())
					{
					case "Sunken":
					{
						int num5 = Math.Abs(e.X - firstPoint.X);
						int num6 = Math.Abs(e.Y - firstPoint.Y);
						if (num5 >= num6)
						{
							secondPoint = new Point(e.X, firstPoint.Y);
						}
						else
						{
							secondPoint = new Point(firstPoint.X, e.Y);
						}
						break;
					}
					case "Raised":
						secondPoint = new Point(e.X, e.Y);
						break;
					}
					CreateGraphics().DrawLine(caretLinePen, secondPoint, firstPoint);
					currentLength = CalculateLengthBetween2Points(firstPoint, secondPoint, scale);
					mainwindow.WriteCurrentObjectLength(currentLength, unit);
					toolTip(insertPoint: new Point(e.X, e.Y), theString: dictionar.dictionar[176] + (currentLength * (double)unitscale).ToString("N"));
				}
				else
				{
					toolTip(insertPoint: new Point(e.X, e.Y), theString: dictionar.dictionar[177]);
				}
				break;
			case 9:
				if (startCommand)
				{
					toolTip(insertPoint: new Point(e.X, e.Y), theString: dictionar.dictionar[178]);
					break;
				}
				toolTip(insertPoint: new Point(e.X, e.Y), theString: dictionar.dictionar[179]);
				break;
			case 11:
				if (startCommand)
				{
					toolTip(insertPoint: new Point(e.X, e.Y), theString: dictionar.dictionar[180]);
					break;
				}
				toolTip(insertPoint: new Point(e.X, e.Y), theString: dictionar.dictionar[181]);
				break;
			case 13:
				if (!startCommand)
				{
					firstPoint = TransformFromDrawing(startPoint);
					switch (mainwindow.statusBarOrtho.BorderStyle.ToString())
					{
					case "Sunken":
					{
						int num = Math.Abs(e.X - firstPoint.X);
						int num2 = Math.Abs(e.Y - firstPoint.Y);
						if (num >= num2)
						{
							secondPoint = new Point(e.X, firstPoint.Y);
						}
						else
						{
							secondPoint = new Point(firstPoint.X, e.Y);
						}
						break;
					}
					case "Raised":
						secondPoint = new Point(e.X, e.Y);
						break;
					}
					Point[] points = new Point[5]
					{
						firstPoint,
						new Point(secondPoint.X, firstPoint.Y),
						new Point(secondPoint.X, secondPoint.Y),
						new Point(firstPoint.X, secondPoint.Y),
						firstPoint
					};
					CreateGraphics().DrawLines(rectangleLinePen, points);
					currentLength = CalculateLengthBetween2Points(firstPoint, secondPoint, scale);
					mainwindow.WriteCurrentObjectLength(currentLength, unit);
					double num3 = Math.Abs(firstPoint.X - secondPoint.X);
					double num4 = Math.Abs(firstPoint.Y - secondPoint.Y);
					toolTip(insertPoint: new Point(e.X, e.Y), theString: dictionar.dictionar[182] + (num3 * (double)unitscale).ToString("N") + dictionar.dictionar[183] + (num4 * (double)unitscale).ToString("N"));
				}
				else
				{
					toolTip(insertPoint: new Point(e.X, e.Y), theString: dictionar.dictionar[184]);
				}
				break;
			}
			if ((mainwindow.statusBarOsnap.BorderStyle.ToString() == "Sunken" || mainwindow.statusBarSnap.BorderStyle == StatusBarPanelBorderStyle.Sunken) & (((Document)base.ParentForm).activeCommand != 9) & (((Document)base.ParentForm).activeCommand != 10) & (((Document)base.ParentForm).activeCommand != 11) & (((Document)base.ParentForm).activeCommand != 12))
			{
				mouseD = TransformFromScreen(new Point(e.X, e.Y));
				CheckSnapPoint(mouseD);
				if (hasThePoint)
				{
					DrawSnapPoint(CreateGraphics(), theSnapPointD);
				}
			}
		}
		else if (iAmSelecting)
		{
			PasteContent();
			Point point = TransformFromDrawing(selectionFirstPoint);
			Point[] points2 = new Point[5]
			{
				point,
				new Point(e.X, point.Y),
				new Point(e.X, e.Y),
				new Point(point.X, e.Y),
				point
			};
			CreateGraphics().DrawLines(caretLinePen, points2);
		}
		double x_drawing_in_cm = (double)e.X * scale + base_point_x;
		double y_drawing_in_cm = base_point_y - (double)e.Y * scale;
		mainwindow.UpdateCoords(x_drawing_in_cm, y_drawing_in_cm, unit);
	}

	private void GraphicTable_MouseUp(object sender, MouseEventArgs e)
	{
		if (middleDown)
		{
			base_point_x += (double)(e.X - start_translate_x) * scale;
			base_point_y -= (double)(e.Y - start_translate_y) * scale;
			RedrawAll();
			Cursor.Current = Cursors.Cross;
			middleDown = false;
		}
	}

	private void GraphicTable_SizeChanged(object sender, EventArgs e)
	{
		base_point_x -= (double)(base.Size.Width - sizeOfTable_x) * scale / 2.0;
		base_point_y += (double)(base.Size.Height - sizeOfTable_y) * scale / 2.0;
		sizeOfTable_x = base.Size.Width;
		sizeOfTable_y = base.Size.Height;
		RedrawAll();
	}

	public void GraphicTable_MouseWheel(object sender, MouseEventArgs e)
	{
		double num = e.Delta / 120;
		PointD pointD = TransformFromScreen(new Point(e.X, e.Y));
		scale = Math.Round(scale * (1.0 - num / 10.0), 3);
		if (scale <= 0.01)
		{
			scale = 0.01;
		}
		base_point_x = pointD.X - (double)e.X * scale;
		base_point_y = pointD.Y + (double)e.Y * scale;
		RedrawAll();
	}

	private void GraphicTable_KeyPress(object sender, KeyPressEventArgs e)
	{
		int keyChar = e.KeyChar;
		if (keyChar == 27)
		{
			CheckCommandForEnding(escapeCase: true);
		}
	}

	private void InitializeComponent()
	{
		this.contextMenu = new System.Windows.Forms.ContextMenu();
		this.BackColor = System.Drawing.Color.Black;
		this.Cursor = System.Windows.Forms.Cursors.Cross;
		this.ForeColor = System.Drawing.SystemColors.ControlText;
		base.Name = "GraphicTable";
		base.KeyPress += new System.Windows.Forms.KeyPressEventHandler(GraphicTable_KeyPress);
		base.SizeChanged += new System.EventHandler(GraphicTable_SizeChanged);
		base.MouseUp += new System.Windows.Forms.MouseEventHandler(GraphicTable_MouseUp);
		base.MouseMove += new System.Windows.Forms.MouseEventHandler(GraphicTable_MouseMove);
		base.MouseWheel += new System.Windows.Forms.MouseEventHandler(GraphicTable_MouseWheel);
		base.MouseDown += new System.Windows.Forms.MouseEventHandler(GraphicTable_MouseDown);
		this.pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(OnPrintPage);
		this.caretLinePen = new System.Drawing.Pen(this.caretLineColor, 1f);
		this.caretLinePen.DashPattern = this.caretLinePattern;
		this.rectangleLinePen = new System.Drawing.Pen(this.rectangleLineColor, 1f);
		this.rectangleLinePen.DashPattern = this.caretLinePattern;
		this.panelNo = 1;
		this.currentPanel = new LindRoof.Panel(this.currentLayer, this.dictionar.dictionar[109] + this.panelNo);
		this.primaryGridPointBM.SetPixel(0, 0, System.Drawing.Color.Gray);
		this.pd.DefaultPageSettings.Landscape = true;
		this.pd.DefaultPageSettings.Margins.Bottom = 20;
		this.pd.DefaultPageSettings.Margins.Top = 100;
		this.pd.DefaultPageSettings.Margins.Left = 20;
		this.pd.DefaultPageSettings.Margins.Right = 20;
	}

	[DllImport("gdi32.dll")]
	private static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

	[DllImport("User32.dll")]
	public static extern IntPtr GetDC(IntPtr hWnd);

	[DllImport("User32.dll")]
	public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

	protected override void OnPaint(PaintEventArgs e)
	{
		PasteContent(e.ClipRectangle);
		base.OnPaint(e);
	}

	public void PrinterSettings()
	{
		PageSetupDialog pageSetupDialog = new PageSetupDialog();
		PageSettings defaultPageSettings = pd.DefaultPageSettings;
		pageSetupDialog.PageSettings = defaultPageSettings;
		if (pageSetupDialog.ShowDialog() == DialogResult.OK)
		{
			pd.DefaultPageSettings = pageSetupDialog.PageSettings;
		}
	}

	public void PrintPreview()
	{
		try
		{
			string documentName = dictionar.dictionar[185] + ((Document)base.ParentForm).Text;
			pd.DocumentName = documentName;
			PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
			printPreviewDialog.Document = pd;
			printPreviewDialog.PrintPreviewControl.Zoom = 1.0;
			printPreviewDialog.WindowState = FormWindowState.Maximized;
			printPreviewDialog.ShowDialog(this);
		}
		catch (Exception ex)
		{
			string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
			MessageBox.Show(this, text);
		}
	}

	public void Print()
	{
		PrintDialog printDialog = new PrintDialog();
		printDialog.AllowSomePages = true;
		printDialog.ShowHelp = true;
		printDialog.Document = pd;
		DialogResult dialogResult = printDialog.ShowDialog();
		if (dialogResult == DialogResult.OK)
		{
			pd.Print();
		}
	}

	private void OnPrintPage(object sender, PrintPageEventArgs e)
	{
		PrintAll(e.Graphics);
	}

	public void PrintAll(Graphics g)
	{
		try
		{
			double num = scale;
			double val = (double)pd.DefaultPageSettings.Bounds.Width / (double)base.Size.Width;
			double val2 = (double)pd.DefaultPageSettings.Bounds.Height / (double)base.Size.Height;
			scale /= Math.Min(val, val2);
			origin_x = (int)(base_point_x / scale * -1.0);
			origin_y = (int)(base_point_y / scale);
			g.Clear(Color.White);
			DrawPanels(g, print: true);
			DrawContainingObjects(g, print: true);
			sf.Alignment = StringAlignment.Near;
			sf.LineAlignment = StringAlignment.Near;
			g.DrawString(base.ParentForm.Text + dictionar.dictionar[186], new Font("Arial", 11f), new SolidBrush(Color.Black), 80f, pd.DefaultPageSettings.Bounds.Height - 80, sf);
			g.Dispose();
			scale = num;
		}
		catch (Exception ex)
		{
			string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
			MessageBox.Show(this, text);
		}
	}

	private void CreateSwarmsOfLines(Graphics gr)
	{
		Random random = new Random();
		for (int i = 1; i <= 500; i++)
		{
			int y = random.Next(base.ClientSize.Height);
			int x = random.Next(base.ClientSize.Width);
			int y2 = random.Next(base.ClientSize.Height);
			int x2 = random.Next(base.ClientSize.Width);
			gr.DrawLine(pen, x, y, x2, y2);
		}
	}

	private void DrawThoseObjects(ArrayList objectsToBeDrawn)
	{
		try
		{
			if (objectsToBeDrawn.Count == 0)
			{
				return;
			}
			Graphics gt = Graphics.FromImage(bm);
			foreach (GraphicObject item in objectsToBeDrawn)
			{
				item.Draw(gt, scale, base_point_x, base_point_y);
			}
			PasteContent();
		}
		catch (Exception ex)
		{
			string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
			MessageBox.Show(this, text);
		}
	}

	private void DrawUCS(Graphics gr)
	{
		try
		{
			gr.DrawLine(pen, 10, base.ClientSize.Height - 15, 10, base.ClientSize.Height - 70);
			gr.DrawLine(pen, 15, base.ClientSize.Height - 10, 70, base.ClientSize.Height - 10);
			gr.DrawLine(pen, 10, base.ClientSize.Height - 70, 5, base.ClientSize.Height - 50);
			gr.DrawLine(pen, 10, base.ClientSize.Height - 70, 15, base.ClientSize.Height - 50);
			gr.DrawLine(pen, 70, base.ClientSize.Height - 10, 50, base.ClientSize.Height - 15);
			gr.DrawLine(pen, 70, base.ClientSize.Height - 10, 50, base.ClientSize.Height - 5);
			gr.DrawRectangle(pen, 5, base.ClientSize.Height - 15, 10, 10);
			gr.DrawString(x_axis, theFont, theBrush, 55f, base.ClientSize.Height - 34, theStringFormat);
			gr.DrawString(y_axis, theFont, theBrush, 10f, base.ClientSize.Height - 80, theStringFormat);
		}
		catch (Exception ex)
		{
			string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
			MessageBox.Show(this, text);
		}
	}

	private void DrawGrid(Graphics gr)
	{
		try
		{
			int num = 0;
			int num2 = 0;
			if (!paning)
			{
				double d = Math.Log(scale, 2.0);
				d = Math.Floor(d);
				gridDistance = 50.0 * Math.Pow(2.0, d);
				mainwindow.UpdateGridSize(gridDistance);
			}
			Pen pen = new Pen(Color.FromArgb(255 - background.R, 255 - background.G, 255 - background.B));
			double num3 = Math.Floor(base_point_x / gridDistance) * gridDistance;
			double num4 = Math.Ceiling(base_point_y / gridDistance) * gridDistance;
			double num5 = Math.Floor(base_point_x / gridDistance / 5.0) * gridDistance * 5.0;
			double num6 = Math.Ceiling(base_point_y / gridDistance / 5.0) * gridDistance * 5.0;
			PointD pointD = new PointD(num3, num4);
			PointD pointD2 = new PointD(num5, num6);
			for (num = 1; num < (int)((double)base.ClientSize.Width * scale / gridDistance) + 2; num++)
			{
				pointD.X += gridDistance;
				pointD.Y = num4;
				for (num2 = 1; num2 < (int)((double)base.ClientSize.Height * scale / gridDistance) + 2; num2++)
				{
					pointD.Y -= gridDistance;
					Point point = TransformFromDrawing(pointD);
					gr.DrawImageUnscaled(primaryGridPointBM, point);
				}
			}
			for (num = 1; num < (int)((double)base.ClientSize.Width * scale / gridDistance / 5.0) + 2; num++)
			{
				pointD2.X += gridDistance * 5.0;
				pointD2.Y = num6;
				for (num2 = 1; num2 < (int)((double)base.ClientSize.Height * scale / gridDistance / 5.0) + 2; num2++)
				{
					pointD2.Y -= gridDistance * 5.0;
					Point point = TransformFromDrawing(pointD2);
					gr.DrawLine(pen, new Point(point.X - 3, point.Y), new Point(point.X + 3, point.Y));
					gr.DrawLine(pen, new Point(point.X, point.Y - 3), new Point(point.X, point.Y + 3));
				}
			}
		}
		catch (Exception ex)
		{
			string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
			MessageBox.Show(this, text);
		}
	}

	public void RedrawAll()
	{
		try
		{
			Graphics graphics = Graphics.FromImage(bm);
			origin_x = (int)(base_point_x / scale * -1.0);
			origin_y = (int)(base_point_y / scale);
			graphics.Clear(background);
			DrawPanels(graphics);
			if (amIdrawingTheOrigin)
			{
				DrawUCS(graphics);
			}
			if (mainwindow.statusBarGrid.BorderStyle.ToString() == "Sunken")
			{
				DrawGrid(graphics);
			}
			DrawContainingObjects(graphics);
			graphics.Dispose();
			PasteContent();
		}
		catch (Exception ex)
		{
			string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
			MessageBox.Show(this, text);
		}
	}

	private void toolTip(string theString, Point insertPoint)
	{
		try
		{
			Graphics graphics = CreateGraphics();
			insertPoint.X += 10;
			insertPoint.Y += 10;
			Font font = new Font("Arial", 10f);
			sf.Alignment = StringAlignment.Near;
			sf.LineAlignment = StringAlignment.Near;
			SizeF size = graphics.MeasureString(theString, font);
			RectangleF rect = new RectangleF(insertPoint, size);
			graphics.FillRectangle(Brushes.AntiqueWhite, rect);
			graphics.DrawRectangle(new Pen(Color.Black, 1f), rect.X, rect.Y, rect.Width, rect.Height);
			graphics.DrawString(theString, font, new SolidBrush(Color.Black), insertPoint.X + 3, insertPoint.Y - 1, sf);
			graphics.Dispose();
		}
		catch (Exception ex)
		{
			string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
			MessageBox.Show(this, text);
		}
	}

	private void DrawPanels(Graphics gr, bool print)
	{
		try
		{
			if (panels != null)
			{
				foreach (Panel panel in panels)
				{
					try
					{
						if (panel.objects != null)
						{
							foreach (GraphicObject @object in panel.objects)
							{
								@object.SimpleDraw(gr, scale, base_point_x, base_point_y);
							}
						}
					}
					catch
					{
					}
					HatchBrush hatchBrush = (print ? new HatchBrush(HatchStyle.LightDownwardDiagonal, Color.Gray, Color.Transparent) : new HatchBrush(HatchStyle.LightDownwardDiagonal, Color.Gray, Color.Transparent));
					new Pen(Brushes.Orange);
					panel.currentScale = scale;
					panel.currentbpx = base_point_x;
					panel.currentbpy = base_point_y;
					gr.FillPath(hatchBrush, panel.graphicsPath2);
					Point point = new Point((int)((panel.centerOfGravity.X - base_point_x) / scale), (int)((base_point_y - panel.centerOfGravity.Y) / scale));
					sf.Alignment = StringAlignment.Center;
					sf.LineAlignment = StringAlignment.Center;
					gr.DrawString(brush: new SolidBrush(print ? Color.Black : Color.FromArgb(255 - background.R, 255 - background.G, 255 - background.B)), s: panel.panelName, font: new Font("ArialBold", 12f), point: point, format: sf);
					hatchBrush.Dispose();
				}
			}
			if (currentPanel == null || print)
			{
				return;
			}
			foreach (GraphicObject object2 in currentPanel.objects)
			{
				object2.UpdateDrawingCoords(scale, base_point_x, base_point_y);
			}
			gr.DrawPath(new Pen(Color.AliceBlue, 3f), currentPanel.graphicsPath2);
			HatchBrush hatchBrush2 = new HatchBrush(HatchStyle.DottedDiamond, Color.Gray, Color.Transparent);
			gr.FillPath(hatchBrush2, currentPanel.graphicsPath2);
			hatchBrush2.Dispose();
		}
		catch (Exception ex)
		{
			string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
			MessageBox.Show(this, text);
		}
	}

	private void DrawPanels(Graphics gr)
	{
		try
		{
			if (panels != null)
			{
				foreach (Panel panel in panels)
				{
					try
					{
						if (panel.objects != null)
						{
							foreach (GraphicObject @object in panel.objects)
							{
								@object.SimpleDraw(gr, scale, base_point_x, base_point_y);
							}
						}
					}
					catch
					{
					}
					new Pen(Brushes.Orange);
					panel.currentScale = scale;
					panel.currentbpx = base_point_x;
					panel.currentbpy = base_point_y;
					gr.FillPath(culoarePanouri, panel.graphicsPath2);
					Point point = new Point((int)((panel.centerOfGravity.X - base_point_x) / scale), (int)((base_point_y - panel.centerOfGravity.Y) / scale));
					sf.Alignment = StringAlignment.Center;
					sf.LineAlignment = StringAlignment.Center;
					gr.DrawString(panel.panelName, new Font("ArialBold", 12f), culoareTextPanouri, point, sf);
				}
			}
			if (currentPanel == null)
			{
				return;
			}
			foreach (GraphicObject object2 in currentPanel.objects)
			{
				object2.UpdateDrawingCoords(scale, base_point_x, base_point_y);
			}
			gr.DrawPath(new Pen(Color.AliceBlue, 3f), currentPanel.graphicsPath2);
			HatchBrush hatchBrush = new HatchBrush(HatchStyle.DottedDiamond, Color.Gray, Color.Transparent);
			gr.FillPath(hatchBrush, currentPanel.graphicsPath2);
			hatchBrush.Dispose();
		}
		catch (Exception ex)
		{
			string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
			MessageBox.Show(this, text);
		}
	}

	private void DrawSnapPoint(Graphics gr, PointD thePoint)
	{
		try
		{
			if (objects != null && minimumLength != checkMinimumIfChanged)
			{
				SnapPoint snapPoint = new SnapPoint(thePoint, scale, snapPointColor, snapPointWidth, snapPointDimension);
				snapPoint.Draw(gr, scale, base_point_x, base_point_y);
			}
		}
		catch (Exception ex)
		{
			string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
			MessageBox.Show(this, text);
		}
	}

	private void DrawContainingObjects(Graphics gr)
	{
		try
		{
			if (objects == null)
			{
				return;
			}
			foreach (GraphicObject @object in objects)
			{
				@object.Draw(gr, scale, base_point_x, base_point_y);
			}
		}
		catch (Exception ex)
		{
			string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
			MessageBox.Show(this, text);
		}
	}

	private void DrawContainingObjects(Graphics gr, bool print)
	{
		try
		{
			if (objects == null)
			{
				return;
			}
			foreach (GraphicObject @object in objects)
			{
				@object.Draw(gr, scale, base_point_x, base_point_y, print);
			}
		}
		catch (Exception ex)
		{
			string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
			MessageBox.Show(this, text);
		}
	}

	private void CopyContent()
	{
		try
		{
			if (base.Height > 0)
			{
				IntPtr dC = GetDC(base.Handle);
				bm.Dispose();
				bm = new Bitmap(base.Width, base.Height);
				Graphics graphics = Graphics.FromImage(bm);
				IntPtr hdc = graphics.GetHdc();
				BitBlt(hdc, 0, 0, bm.Width, bm.Height, dC, 0, 0, 13369376);
				ReleaseDC(base.Handle, dC);
				graphics.ReleaseHdc(hdc);
				graphics.Dispose();
			}
		}
		catch (Exception ex)
		{
			string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
			MessageBox.Show(this, text);
		}
	}

	private void PasteContent()
	{
		try
		{
			Graphics graphics = CreateGraphics();
			graphics.DrawImage(bm, new Point(0, 0));
		}
		catch (Exception ex)
		{
			string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
			MessageBox.Show(this, text);
		}
	}

	private void PasteContent(Rectangle rect)
	{
		try
		{
			Graphics graphics = CreateGraphics();
			graphics.DrawImage(bm, rect, rect, GraphicsUnit.Pixel);
		}
		catch (Exception ex)
		{
			string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
			MessageBox.Show(this, text);
		}
	}

	private bool CheckForObjectsInSelectedArea(PointD selectionFirstPoint, PointD selectionSecondPoint)
	{
		bool result = false;
		try
		{
			double num = 0.0;
			foreach (GraphicObject @object in objects)
			{
				if (@object.CheckIfIsInSelectionRectangle(selectionFirstPoint, selectionSecondPoint))
				{
					@object.isSelected = true;
					num += @object.length;
					result = true;
				}
			}
			RedrawAll();
			mainwindow.WriteCurrentObjectLength(num, unit);
		}
		catch (Exception ex)
		{
			string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
			MessageBox.Show(this, text);
		}
		return result;
	}

	private void CheckObjectsForFiletingAndFiletThemIfTrue(GraphicObject firstSelectedObj, GraphicObject secondSelectedObj)
	{
		GraphicsPath path = firstSelectedObj.graphicsPath(2);
		GraphicsPath path2 = secondSelectedObj.graphicsPath(2);
		Region region = new Region(path);
		region.Intersect(path2);
		if (!region.IsEmpty(CreateGraphics()))
		{
			try
			{
				line line2 = (line)firstSelectedObj;
				line line3 = (line)secondSelectedObj;
				double num8;
				double num9;
				if (line2.stopPoint.X - line2.startPoint.X != 0.0)
				{
					if (line3.stopPoint.X - line3.startPoint.X != 0.0)
					{
						double num = (line2.startPoint.Y - line2.stopPoint.Y) / (line2.stopPoint.X - line2.startPoint.X);
						double num2 = (line3.startPoint.Y - line3.stopPoint.Y) / (line3.stopPoint.X - line3.startPoint.X);
						double num3 = line2.startPoint.Y + (line2.startPoint.Y - line2.stopPoint.Y) * line2.startPoint.X / (line2.stopPoint.X - line2.startPoint.X);
						double num4 = line3.startPoint.Y + (line3.startPoint.Y - line3.stopPoint.Y) * line3.startPoint.X / (line3.stopPoint.X - line3.startPoint.X);
						double num5 = num - num2;
						double num6 = num3 - num4;
						double num7 = num * num4 - num2 * num3;
						num8 = num6 / num5;
						num9 = num7 / num5;
					}
					else
					{
						num8 = line3.stopPoint.X;
						num9 = (line2.stopPoint.Y - line2.startPoint.Y) * (num8 - line2.startPoint.X) / (line2.stopPoint.X - line2.startPoint.X) + line2.startPoint.Y;
					}
				}
				else
				{
					num8 = line2.stopPoint.X;
					num9 = (line3.stopPoint.Y - line3.startPoint.Y) * (num8 - line3.startPoint.X) / (line3.stopPoint.X - line3.startPoint.X) + line3.startPoint.Y;
				}
				if (line2.startPoint.X < line2.stopPoint.X)
				{
					if (mouse1.X <= num8)
					{
						line2.stopPoint.X = num8;
					}
					else
					{
						line2.startPoint.X = num8;
					}
				}
				else if (line2.startPoint.X > line2.stopPoint.X)
				{
					if (mouse1.X <= num8)
					{
						line2.startPoint.X = num8;
					}
					else
					{
						line2.stopPoint.X = num8;
					}
				}
				if (line2.startPoint.Y < line2.stopPoint.Y)
				{
					if (mouse1.Y <= num9)
					{
						line2.stopPoint.Y = num9;
					}
					else
					{
						line2.startPoint.Y = num9;
					}
				}
				else if (line2.startPoint.Y > line2.stopPoint.Y)
				{
					if (mouse1.Y <= num9)
					{
						line2.startPoint.Y = num9;
					}
					else
					{
						line2.stopPoint.Y = num9;
					}
				}
				if (line3.startPoint.X < line3.stopPoint.X)
				{
					if (mouse2.X <= num8)
					{
						line3.stopPoint.X = num8;
					}
					else
					{
						line3.startPoint.X = num8;
					}
				}
				else if (line3.startPoint.X > line3.stopPoint.X)
				{
					if (mouse2.X <= num8)
					{
						line3.startPoint.X = num8;
					}
					else
					{
						line3.stopPoint.X = num8;
					}
				}
				if (line3.startPoint.Y < line3.stopPoint.Y)
				{
					if (mouse2.Y <= num9)
					{
						line3.stopPoint.Y = num9;
					}
					else
					{
						line3.startPoint.Y = num9;
					}
				}
				else if (line3.startPoint.Y > line3.stopPoint.Y)
				{
					if (mouse2.Y <= num9)
					{
						line3.startPoint.Y = num9;
					}
					else
					{
						line3.stopPoint.Y = num9;
					}
				}
			}
			catch (Exception ex)
			{
				string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
				MessageBox.Show(this, text);
			}
		}
		else
		{
			mainwindow.consoleWindow.AppendTextToHistory(dictionar.dictionar[199]);
		}
		firstSelectedObj.isSelected = false;
		secondSelectedObj.isSelected = false;
		((Document)base.ParentForm).enableCommand = false;
		((Document)base.ParentForm).lastCommand = ((Document)base.ParentForm).activeCommand;
		((Document)base.ParentForm).activeCommand = 0;
		RedrawAll();
	}

	private bool CheckHittingTestofAnObject(Point mouse)
	{
		bool result = false;
		try
		{
			foreach (GraphicObject @object in objects)
			{
				@object.UpdateDrawingCoords(scale, base_point_x, base_point_y);
				GraphicsPath graphicsPath = @object.graphicsPath(15);
				if (!graphicsPath.IsVisible(mouse))
				{
					continue;
				}
				if (@object.isSelected)
				{
					@object.isSelected = false;
				}
				else
				{
					@object.isSelected = true;
					int num = ((Document)base.ParentForm).layers.IndexOf(@object.layer);
					if (num < 0)
					{
						num = ((Document)base.ParentForm).layers.Count - 1;
					}
					((MainWindow)((Document)base.ParentForm).ParentForm).statusBarType2.Text = ((Document)base.ParentForm).layersList1.Items[num].ToString();
					((Document)base.ParentForm).layersList1.SelectedIndexChanged -= ((Document)base.ParentForm).layersList1_DropDownClosed;
					((Document)base.ParentForm).layersList1.SelectedIndex = num;
					((Document)base.ParentForm).layersList1.SelectedIndexChanged += ((Document)base.ParentForm).layersList1_DropDownClosed;
				}
				result = true;
				RedrawAll();
				break;
			}
			double num2 = 0.0;
			foreach (GraphicObject object2 in objects)
			{
				if (object2.isSelected)
				{
					num2 += object2.length;
				}
			}
			mainwindow.WriteCurrentObjectLength(num2, unit);
		}
		catch (Exception ex)
		{
			string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
			MessageBox.Show(this, text);
		}
		return result;
	}

	private GraphicObject CheckHittingTestofAnObjectAndReturnTheObject(Point mouse)
	{
		GraphicObject result = null;
		try
		{
			foreach (GraphicObject @object in objects)
			{
				GraphicsPath graphicsPath = @object.graphicsPath(15);
				if (graphicsPath.IsVisible(mouse))
				{
					@object.isSelected = true;
					result = @object;
					break;
				}
			}
		}
		catch (Exception ex)
		{
			string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
			MessageBox.Show(this, text);
		}
		return result;
	}

	public void CheckCommandForEnding(bool escapeCase)
	{
		if (((Document)base.ParentForm).enableCommand)
		{
			mainwindow.WriteCurrentObjectLength(lastLength, unit);
			((Document)base.ParentForm).lastCommand = ((Document)base.ParentForm).activeCommand;
			switch (((Document)base.ParentForm).activeCommand)
			{
			case 1:
				mainwindow.consoleWindow.AppendTextToHistory(dictionar.dictionar[193]);
				break;
			case 4:
				mainwindow.consoleWindow.AppendTextToHistory(dictionar.dictionar[194]);
				break;
			case 9:
				mainwindow.consoleWindow.AppendTextToHistory(dictionar.dictionar[195]);
				if (firstSelectedObject != null)
				{
					firstSelectedObject.isSelected = false;
				}
				Cursor = Cursors.Cross;
				break;
			case 11:
				if (!startCommand)
				{
					panels.Add(currentPanel);
					panelNo++;
					currentPanel = new Panel(currentLayer, dictionar.dictionar[109] + panelNo);
					mainwindow.consoleWindow.AppendTextToHistory(dictionar.dictionar[196]);
				}
				else
				{
					currentPanel = new Panel(currentLayer, dictionar.dictionar[109] + panelNo);
					mainwindow.consoleWindow.AppendTextToHistory(dictionar.dictionar[197]);
				}
				Cursor = Cursors.Cross;
				break;
			case 13:
				mainwindow.consoleWindow.AppendTextToHistory(dictionar.dictionar[198]);
				break;
			}
			((Document)base.ParentForm).enableCommand = false;
			startCommand = true;
			((Document)base.ParentForm).activeCommand = 0;
		}
		foreach (GraphicObject @object in objects)
		{
			@object.isSelected = false;
		}
		iAmSelecting = false;
		RedrawAll();
	}

	public void CheckCommandForEnding()
	{
		if (((Document)base.ParentForm).enableCommand)
		{
			mainwindow.WriteCurrentObjectLength(lastLength, unit);
			((Document)base.ParentForm).lastCommand = ((Document)base.ParentForm).activeCommand;
			switch (((Document)base.ParentForm).activeCommand)
			{
			case 1:
				mainwindow.consoleWindow.AppendTextToHistory(dictionar.dictionar[193]);
				break;
			case 4:
				mainwindow.consoleWindow.AppendTextToHistory(dictionar.dictionar[194]);
				break;
			case 9:
				mainwindow.consoleWindow.AppendTextToHistory(dictionar.dictionar[195]);
				if (firstSelectedObject != null)
				{
					firstSelectedObject.isSelected = false;
				}
				Cursor = Cursors.Cross;
				break;
			case 11:
				if (!startCommand)
				{
					AddPanel(currentPanel);
					panelNo++;
					currentPanel = new Panel(currentLayer, dictionar.dictionar[109] + panelNo);
					mainwindow.consoleWindow.AppendTextToHistory(dictionar.dictionar[196]);
				}
				else
				{
					currentPanel = new Panel(currentLayer, dictionar.dictionar[109] + panelNo);
					mainwindow.consoleWindow.AppendTextToHistory(dictionar.dictionar[197]);
				}
				Cursor = Cursors.Cross;
				break;
			case 13:
				mainwindow.consoleWindow.AppendTextToHistory(dictionar.dictionar[198]);
				break;
			}
			foreach (GraphicObject @object in objects)
			{
				@object.isSelected = false;
			}
			PasteContent();
			((Document)base.ParentForm).enableCommand = false;
			startCommand = true;
			((Document)base.ParentForm).activeCommand = 0;
		}
		else
		{
			((Document)base.ParentForm).enableCommand = true;
			((Document)base.ParentForm).activeCommand = ((Document)base.ParentForm).lastCommand;
			mainwindow.consoleWindow.CheckDictionary(((Document)base.ParentForm).activeCommand);
		}
	}

	private void CheckSnapPoint(PointD mouse)
	{
		if (objects == null)
		{
			return;
		}
		radiusD = (double)radius * scale;
		minimumLength = radiusD * 1.001;
		checkMinimumIfChanged = minimumLength;
		hasThePoint = false;
		try
		{
			if (mainwindow.statusBarGrid.BorderStyle.ToString() == "Sunken" && mainwindow.statusBarSnap.BorderStyle.ToString() == "Sunken")
			{
				PointD pointD = new PointD(Math.Round(mouse.X / gridDistance, 0) * gridDistance, Math.Round(mouse.Y / gridDistance, 0) * gridDistance);
				PointD value = new PointD(pointD.X + gridDistance, pointD.Y);
				PointD value2 = new PointD(pointD.X + gridDistance, pointD.Y + gridDistance);
				PointD value3 = new PointD(pointD.X, pointD.Y + gridDistance);
				ArrayList arrayList = new ArrayList(4);
				arrayList.Add(pointD);
				arrayList.Add(value);
				arrayList.Add(value2);
				arrayList.Add(value3);
				foreach (PointD item in arrayList)
				{
					double num = CalculateLengthBetween2PointsD(mouse, item);
					if (num <= radiusD && num <= minimumLength)
					{
						minimumLength = num;
						theSnapPointD = item;
						hasThePoint = true;
					}
				}
			}
			if (!(mainwindow.statusBarOsnap.BorderStyle.ToString() == "Sunken"))
			{
				return;
			}
			foreach (GraphicObject @object in objects)
			{
				double num2 = CalculateLengthBetween2PointsD(mouse, @object.startPoint);
				if (num2 <= radiusD && num2 <= minimumLength)
				{
					minimumLength = num2;
					theSnapPointD = @object.startPoint;
					hasThePoint = true;
				}
				num2 = CalculateLengthBetween2PointsD(mouse, @object.stopPoint);
				if (num2 <= radiusD && num2 <= minimumLength)
				{
					minimumLength = num2;
					theSnapPointD = @object.stopPoint;
					hasThePoint = true;
				}
			}
		}
		catch (Exception ex)
		{
			string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
			MessageBox.Show(this, text);
		}
	}

	public void AddPanel(Panel pan)
	{
		if (pan.startPoint.X != pan.stopPoint.X || pan.startPoint.Y != pan.stopPoint.Y)
		{
			pan.Add(new line(pan.stopPoint, pan.startPoint, currentLayer, objectIndex));
		}
		panels.Add(pan);
		Panel value = (Panel)pan.Clone();
		panelsForDeveloping.Add(value);
		RedrawAll();
	}

	public void AddObject(GraphicObject obj)
	{
		objects.Add(obj);
		PushUndoStack(obj, "add");
		objectIndex++;
		Graphics gt = Graphics.FromImage(bm);
		obj.Draw(gt, scale, base_point_x, base_point_y);
		PasteContent();
	}

	public void PushUndoStack(GraphicObject obj, string op)
	{
		undo_redo_object obj2 = new undo_redo_object(op, obj);
		undo_stack.Push(obj2);
		redo_stack.Clear();
	}

	public void PopUndoStack()
	{
		if (undo_stack.Count != 0)
		{
			undo_redo_object undo_redo_object2 = (undo_redo_object)undo_stack.Pop();
			switch (undo_redo_object2.operation)
			{
			case "add":
				objects.Remove(undo_redo_object2.obj);
				break;
			case "remove":
				objects.Add(undo_redo_object2.obj);
				break;
			}
			undo_redo_object2.obj.isSelected = false;
			RedrawAll();
			redo_stack.Push(undo_redo_object2);
		}
	}

	public void PopRedoStack()
	{
		if (redo_stack.Count != 0)
		{
			undo_redo_object undo_redo_object2 = (undo_redo_object)redo_stack.Pop();
			switch (undo_redo_object2.operation)
			{
			case "add":
				objects.Add(undo_redo_object2.obj);
				break;
			case "remove":
				objects.Remove(undo_redo_object2.obj);
				break;
			}
			RedrawAll();
			undo_stack.Push(undo_redo_object2);
		}
	}

	public void AddObjectWithoutRedraw(GraphicObject obj)
	{
		objects.Add(obj);
		PushUndoStack(obj, "add");
		objectIndex++;
	}

	public void RemoveSelectedObjects()
	{
		if (objects == null)
		{
			return;
		}
		foreach (GraphicObject @object in objects)
		{
			if (@object.isSelected)
			{
				objects.Remove(@object);
				PushUndoStack(@object, "remove");
				RemoveSelectedObjects();
				break;
			}
		}
	}

	public void CastingAPointOfThePolyline(Point e)
	{
		stopPoint = TransformFromScreen(new Point(e.X, e.Y));
		if (mainwindow.statusBarOrtho.BorderStyle.ToString() == "Sunken")
		{
			double num = Math.Abs(stopPoint.X - startPoint.X);
			double num2 = Math.Abs(stopPoint.Y - startPoint.Y);
			if (num >= num2)
			{
				stopPoint = new PointD(stopPoint.X, startPoint.Y);
			}
			else
			{
				stopPoint = new PointD(startPoint.X, stopPoint.Y);
			}
		}
		line line2 = new line(startPoint, stopPoint, currentLayer, objectIndex);
		AddObject(line2);
		lastLength = line2.length;
		currentLength = lastLength;
		mainwindow.WriteCurrentObjectLength(currentLength, unit);
		startPoint = stopPoint;
		((Document)base.ParentForm).lastCommand = ((Document)base.ParentForm).activeCommand;
	}

	public void CastingAPointOfThePolyline(PointD e)
	{
		stopPoint = e;
		line line2 = new line(startPoint, stopPoint, currentLayer, objectIndex);
		AddObject(line2);
		lastLength = line2.length;
		currentLength = lastLength;
		mainwindow.WriteCurrentObjectLength(currentLength, unit);
		startPoint = stopPoint;
		((Document)base.ParentForm).lastCommand = ((Document)base.ParentForm).activeCommand;
	}

	public void CastingStopPointOfTheLine(Point e)
	{
		stopPoint = TransformFromScreen(new Point(e.X, e.Y));
		if (mainwindow.statusBarOrtho.BorderStyle.ToString() == "Sunken")
		{
			double num = Math.Abs(stopPoint.X - startPoint.X);
			double num2 = Math.Abs(stopPoint.Y - startPoint.Y);
			if (num >= num2)
			{
				stopPoint = new PointD(stopPoint.X, startPoint.Y);
			}
			else
			{
				stopPoint = new PointD(startPoint.X, stopPoint.Y);
			}
		}
		line line2 = new line(startPoint, stopPoint, currentLayer, objectIndex);
		AddObject(line2);
		lastLength = line2.length;
		currentLength = lastLength;
		mainwindow.WriteCurrentObjectLength(currentLength, unit);
		startCommand = true;
		CheckCommandForEnding();
	}

	public void CastingStopPointOfTheLine(PointD e)
	{
		stopPoint = e;
		line line2 = new line(startPoint, stopPoint, currentLayer, objectIndex);
		AddObject(line2);
		lastLength = line2.length;
		currentLength = lastLength;
		mainwindow.WriteCurrentObjectLength(currentLength, unit);
		startCommand = true;
		CheckCommandForEnding();
	}

	public void CastingStopPointOfTheRectangle(Point e)
	{
		stopPoint = TransformFromScreen(new Point(e.X, e.Y));
		if (mainwindow.statusBarOrtho.BorderStyle.ToString() == "Sunken")
		{
			double num = Math.Abs(stopPoint.X - startPoint.X);
			double num2 = Math.Abs(stopPoint.Y - startPoint.Y);
			if (num >= num2)
			{
				stopPoint = new PointD(stopPoint.X, startPoint.Y);
			}
			else
			{
				stopPoint = new PointD(startPoint.X, stopPoint.Y);
			}
		}
		line line2 = new line(startPoint, new PointD(startPoint.X, stopPoint.Y), currentLayer, objectIndex);
		AddObjectWithoutRedraw(line2);
		line line3 = new line(new PointD(startPoint.X, stopPoint.Y), stopPoint, currentLayer, objectIndex);
		AddObjectWithoutRedraw(line3);
		line line4 = new line(stopPoint, new PointD(stopPoint.X, startPoint.Y), currentLayer, objectIndex);
		AddObjectWithoutRedraw(line4);
		line line5 = new line(new PointD(stopPoint.X, startPoint.Y), startPoint, currentLayer, objectIndex);
		AddObjectWithoutRedraw(line5);
		temporary.Clear();
		temporary.Add(line2);
		temporary.Add(line3);
		temporary.Add(line4);
		temporary.Add(line5);
		DrawThoseObjects(temporary);
		lastLength = line2.length + line3.length + line4.length + line5.length;
		currentLength = lastLength;
		mainwindow.WriteCurrentObjectLength(currentLength, unit);
		startCommand = true;
		CheckCommandForEnding();
	}

	public void CastingStopPointOfTheRectangle(PointD e)
	{
		stopPoint = e;
		line line2 = new line(startPoint, new PointD(startPoint.X, stopPoint.Y), currentLayer, objectIndex);
		AddObjectWithoutRedraw(line2);
		line line3 = new line(new PointD(startPoint.X, stopPoint.Y), stopPoint, currentLayer, objectIndex);
		AddObjectWithoutRedraw(line3);
		line line4 = new line(stopPoint, new PointD(stopPoint.X, startPoint.Y), currentLayer, objectIndex);
		AddObjectWithoutRedraw(line4);
		line line5 = new line(new PointD(stopPoint.X, startPoint.Y), startPoint, currentLayer, objectIndex);
		AddObjectWithoutRedraw(line5);
		temporary.Clear();
		temporary.Add(line2);
		temporary.Add(line3);
		temporary.Add(line4);
		temporary.Add(line5);
		DrawThoseObjects(temporary);
		lastLength = line2.length + line3.length + line4.length + line5.length;
		currentLength = lastLength;
		mainwindow.WriteCurrentObjectLength(currentLength, unit);
		startCommand = true;
		CheckCommandForEnding();
	}

	public PointD TransformFromScreen(Point p)
	{
		return new PointD((double)p.X * scale + base_point_x, base_point_y - (double)p.Y * scale);
	}

	public PointF TransformFromScreenF(Point p)
	{
		return new PointF((float)((double)p.X * scale + base_point_x), (float)(base_point_y - (double)p.Y * scale));
	}

	public Point TransformFromDrawing(PointD p)
	{
		return new Point((int)((p.X - base_point_x) / scale), (int)((base_point_y - p.Y) / scale));
	}

	private double CalculateLengthBetween2Points(Point first, Point second, double sc)
	{
		int num = second.X - first.X;
		int num2 = second.Y - first.Y;
		return Math.Sqrt(num * num + num2 * num2) * scale;
	}

	public double CalculateLengthBetween2PointsD(PointD first, PointD second)
	{
		double num = second.X - first.X;
		double num2 = second.Y - first.Y;
		return Math.Sqrt(num * num + num2 * num2);
	}

	public void showarea()
	{
		foreach (Panel panel in panels)
		{
			_ = panel;
		}
	}

	public void ZoomExtents()
	{
		try
		{
			if (objects == null || objects.Count == 0)
			{
				return;
			}
			PointD pointD = new PointD();
			PointD pointD2 = new PointD();
			bool flag = true;
			foreach (GraphicObject @object in objects)
			{
				if (flag)
				{
					pointD.X = @object.startPoint.X;
					pointD2.X = @object.startPoint.X;
					pointD.Y = @object.startPoint.Y;
					pointD2.Y = @object.startPoint.Y;
					flag = false;
				}
				if (pointD.X > @object.startPoint.X)
				{
					pointD.X = @object.startPoint.X;
				}
				if (pointD2.X < @object.startPoint.X)
				{
					pointD2.X = @object.startPoint.X;
				}
				if (pointD.X > @object.stopPoint.X)
				{
					pointD.X = @object.stopPoint.X;
				}
				if (pointD2.X < @object.stopPoint.X)
				{
					pointD2.X = @object.stopPoint.X;
				}
				if (pointD.Y > @object.startPoint.Y)
				{
					pointD.Y = @object.startPoint.Y;
				}
				if (pointD2.Y < @object.startPoint.Y)
				{
					pointD2.Y = @object.startPoint.Y;
				}
				if (pointD.Y > @object.stopPoint.Y)
				{
					pointD.Y = @object.stopPoint.Y;
				}
				if (pointD2.Y < @object.stopPoint.Y)
				{
					pointD2.Y = @object.stopPoint.Y;
				}
			}
			if ((double)(base.Size.Width - 30) * scale / (pointD2.X - pointD.X) <= (double)(base.Size.Height - 30) * scale / (pointD2.Y - pointD.Y))
			{
				scale /= (double)(base.Size.Width - 30) * scale / (pointD2.X - pointD.X);
				base_point_x = pointD.X - 15.0 * scale;
				base_point_y = pointD2.Y + ((double)(base.Size.Height - 30) * scale - (pointD2.Y - pointD.Y)) / 2.0 + 15.0 * scale;
			}
			else
			{
				scale /= (double)(base.Size.Height - 30) * scale / (pointD2.Y - pointD.Y);
				base_point_x = pointD.X - ((double)(base.Size.Width - 30) * scale - (pointD2.X - pointD.X)) / 2.0 - 15.0 * scale;
				base_point_y = pointD2.Y + 15.0 * scale;
			}
			RedrawAll();
		}
		catch (Exception ex)
		{
			string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
			MessageBox.Show(this, text);
		}
	}
}
