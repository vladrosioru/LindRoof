using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using GpcWrapper;
using LindRoof.FormsEnvironment;
using LindRoof.TextualEnvironment;

namespace LindRoof;

public class GraphicTableForPanels : UserControl
{
	private const uint SRCCOPY = 13369376u;

	private OleDbConnection objConnection = new OleDbConnection();

	private OleDbCommand oleCommand = new OleDbCommand();

	private OleDbDataAdapter objCommand = new OleDbDataAdapter();

	private string strConnect;

	private DataTable dtab2;

	private DataTable dtab3;

	private DataTable dtab4;

	private ArrayList tipuriDeInchidere;

	private PanelObject newPanelObjectGen;

	private double y_max;

	private double y_min;

	private double panelLength;

	private double deltaXp;

	private double deltaYp;

	private double deltaLp;

	private double int1;

	private double int2;

	private DataRow ddd;

	private int idPanou;

	private int nrOndulePerReper;

	private bool doarPentruOptimizare;

	private bool voidSelected;

	private bool startCommand;

	private Point firstP;

	private Point secondP;

	public bool trapezoidal;

	private AdjustableArrowCap ec = new AdjustableArrowCap(10f, 10f, isFilled: false);

	private double offsetX = 1000000.0;

	private double offsetY = 1000000.0;

	private StringFormat sf = new StringFormat();

	private StringFormat sf1 = new StringFormat();

	private Font font = new Font("Arial", 10f);

	private float[] penPattern = new float[2] { 5f, 5f };

	private Pen pen1 = new Pen(Color.Gray, 1f);

	private Pen pen2 = new Pen(Color.LightGray, 1f);

	private Point mouse = default(Point);

	private PointD mouseD = new PointD();

	private voids currentVoid = new voids("currentVoid", new PointD(), 0.0, 0.0, 0.0, trapez: false);

	public int activeCommand;

	public Point[] voidPoints = new Point[4]
	{
		new Point(0, 0),
		new Point(0, 0),
		new Point(0, 0),
		new Point(0, 0)
	};

	public int voidLength = 100;

	public int voidLength2;

	public int voidHeigh = 100;

	private Container components;

	private Bitmap bm;

	private Bitmap cuttedBm;

	public double base_point_x;

	public double base_point_y;

	public double scale;

	private int start_translate_x;

	private int start_translate_y;

	private int origin_x;

	private int origin_y;

	private int deltaX;

	private int deltaY;

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

	public PointD startPoint;

	public PointD stopPoint;

	public Layer currentLayer = new Layer("Default", Color.Gray);

	public Panel currentPanel = new Panel();

	public double lastLength;

	public double currentLength;

	public Point firstPoint;

	public Point secondPoint;

	public bool paning;

	public bool needRedraw;

	public bool iAmSelecting;

	private string unit = "mm";

	private PointD selectionFirstPoint;

	private PointD selectionSecondPoint;

	private ArrayList intersectingLines = new ArrayList();

	public Color caretLineColor = Color.Red;

	private float[] caretLinePattern = new float[6] { 8f, 8f, 8f, 8f, 8f, 8f };

	private Pen caretLinePen;

	private Point zeroPoint = new Point(0, 0);

	public string reportThis = "";

	private PrintDocument pd = new PrintDocument();

	private MainWindow mainwindow;

	private Document parentDocument;

	private dictionar_cuvinte dictionar;

	private PrivateFontCollection FC = new PrivateFontCollection();

	private Rectangle rec;

	private Rectangle newRec;

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

	private ArrayList ReadAccessDataBase
	{
		get
		{
			oleCommand.CommandText = "SELECT * FROM TipuriPanouri";
			objCommand.SelectCommand = oleCommand;
			dtab2.Reset();
			try
			{
				objConnection.Open();
				objCommand.Fill(dtab2);
				objConnection.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Eroare la umplerea variabilei dtab2 cu valori:\r\n" + ex.ToString(), "LindRoof", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			ArrayList arrayList = new ArrayList();
			foreach (DataRow row in dtab2.Rows)
			{
				if (!row.IsNull(2))
				{
					try
					{
						arrayList.Add((string)row[2]);
					}
					catch
					{
						MessageBox.Show("Eroare la adaugarea de valori din DataGrid in ArrayList", "LindRoof", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						Environment.Exit(0);
					}
				}
			}
			try
			{
				return arrayList;
			}
			catch
			{
				MessageBox.Show("Eroare la returnarea valorilor arl", "LindRoof", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Environment.Exit(0);
				return arrayList;
			}
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

	public GraphicTableForPanels(Document pD)
	{
		parentDocument = pD;
		mainwindow = pD.mainwindow;
		dictionar = mainwindow.dictionar;
		InitializeComponent();
		pen1.DashPattern = penPattern;
		pen2.CustomEndCap = ec;
		pen2.CustomEndCap.WidthScale = 1f;
		pen2.CustomStartCap = ec;
		pen2.CustomStartCap.WidthScale = 1f;
		tipuriDeInchidere = ReadAccessDataBase;
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
		mouse = new Point(e.X, e.Y);
		switch (e.Button.ToString())
		{
		case "Left":
			switch (activeCommand)
			{
			case 0:
				if (((Document)base.ParentForm).enableCommand)
				{
					switch (((Document)base.ParentForm).activeCommand)
					{
					case 15:
					{
						GraphicObject graphicObject = CheckHittingTestofAnObjectAndReturnTheObject(new Point(e.X, e.Y));
						if (graphicObject != null)
						{
							((Document)base.ParentForm).bendingObject = graphicObject;
							Cursor = Cursors.Cross;
							((Document)base.ParentForm).enableCommand = false;
							startCommand = true;
							((Document)base.ParentForm).activeCommand = 0;
							currentPanel.evolved = true;
							currentPanel.bendingObject = graphicObject;
							base.Enabled = false;
							currentPanel.rotateObjects(graphicObject);
							PointD[] array = GiveMeTheBoundingBox();
							if (Math.Abs(currentPanel.bendingObject.startPoint.Y - array[0].Y) > Math.Abs(currentPanel.bendingObject.startPoint.Y - array[1].Y))
							{
								currentPanel.rotateObjects(currentPanel.bendingObject.startPoint, 0.0, -1.0);
							}
							ZoomExtents();
							((Document)base.ParentForm).angleDeclaration.Enabled = true;
							((Document)base.ParentForm).angleDeclaration.SelectAll();
							((Document)base.ParentForm).inputSlope.Visible = true;
							((Document)base.ParentForm).panelList.Enabled = false;
						}
						break;
					}
					case 16:
					{
						PointD pointD = TransformFromScreen(new Point(e.X, e.Y));
						{
							foreach (PanelObject panelingObject in currentPanel.panelingObjects)
							{
								if (!panelingObject.CheckIfHitted(pointD))
								{
									continue;
								}
								if (pointD.Y - panelingObject.startPoint.Y > panelingObject.petrecereFoi && panelingObject.stopPoint.Y - pointD.Y > panelingObject.petrecereFoi)
								{
									if (currentPanel.tipPanou.nrMinimOndule != 0)
									{
										if (pointD.Y - panelingObject.startPoint.Y <= Math.Floor((panelingObject.stopPoint.Y - panelingObject.startPoint.Y - panelingObject.petrecereFoi - 0.001) / panelingObject.pasOndula) * panelingObject.pasOndula)
										{
											PointD pointD6 = (PointD)panelingObject.stopPoint.Clone();
											panelingObject.stopPoint.Y = panelingObject.startPoint.Y + Math.Ceiling((pointD.Y - panelingObject.startPoint.Y) / panelingObject.pasOndula) * panelingObject.pasOndula + panelingObject.petrecereFoi;
											panelingObject.UpdateLines();
											PointD pointD7 = new PointD(panelingObject.startPoint.X, panelingObject.stopPoint.Y - panelingObject.petrecereFoi);
											if (pointD6.Y - pointD7.Y < panelingObject.lungimeMinimaFoaieModulRandom && !panelingObject.tipTigla.Contains("LTP"))
											{
												pointD6.Y = panelingObject.lungimeMinimaFoaieModulRandom + pointD7.Y;
											}
											PanelObject value3 = (PanelObject)panelingObject.Clone(pointD7, pointD6);
											currentPanel.panelingObjects.Add(value3);
											((Document)base.ParentForm).enableCommand = false;
											((Document)base.ParentForm).activeCommand = 0;
											RedrawAll();
										}
									}
									else
									{
										PointD pointD8 = (PointD)panelingObject.stopPoint.Clone();
										panelingObject.stopPoint.Y = Math.Ceiling((pointD.Y - panelingObject.startPoint.Y) / 10.0) * 10.0 + panelingObject.startPoint.Y + panelingObject.petrecereFoi;
										panelingObject.UpdateLines();
										PointD pointD9 = new PointD(panelingObject.startPoint.X, panelingObject.stopPoint.Y - panelingObject.petrecereFoi);
										if (pointD8.Y - pointD9.Y < panelingObject.lungimeMinimaFoaieModulRandom && !panelingObject.tipTigla.Contains("LTP"))
										{
											pointD8.Y = panelingObject.lungimeMinimaFoaieModulRandom + pointD9.Y;
										}
										PanelObject value4 = (PanelObject)panelingObject.Clone(pointD9, pointD8);
										currentPanel.panelingObjects.Add(value4);
										((Document)base.ParentForm).enableCommand = false;
										((Document)base.ParentForm).activeCommand = 0;
										RedrawAll();
									}
								}
								else
								{
									((Document)base.ParentForm).enableCommand = false;
									((Document)base.ParentForm).activeCommand = 0;
									RedrawAll();
								}
								break;
							}
							break;
						}
					}
					case 17:
					{
						PointD pointD = TransformFromScreen(new Point(e.X, e.Y));
						ArrayList arrayList = (ArrayList)currentPanel.panelingObjects.Clone();
						foreach (PanelObject item in arrayList)
						{
							if (((!(item.startPoint.Y <= pointD.Y) || !(item.stopPoint.Y > pointD.Y)) && (!(item.startPoint.Y >= pointD.Y) || !(item.stopPoint.Y < pointD.Y))) || !(pointD.Y - item.startPoint.Y > item.petrecereFoi) || !(item.stopPoint.Y - pointD.Y > item.petrecereFoi))
							{
								continue;
							}
							if (currentPanel.tipPanou.nrMinimOndule != 0)
							{
								if (pointD.Y - item.startPoint.Y <= Math.Floor((item.stopPoint.Y - item.startPoint.Y - item.petrecereFoi - 0.001) / item.pasOndula) * item.pasOndula)
								{
									PointD pointD2 = (PointD)item.stopPoint.Clone();
									item.stopPoint.Y = item.startPoint.Y + Math.Ceiling((pointD.Y - item.startPoint.Y) / item.pasOndula) * item.pasOndula + item.petrecereFoi;
									item.UpdateLines();
									PointD pointD3 = new PointD(item.startPoint.X, item.stopPoint.Y - item.petrecereFoi);
									if (pointD2.Y - pointD3.Y < item.lungimeMinimaFoaieModulRandom && !item.tipTigla.Contains("LTP"))
									{
										pointD2.Y = item.lungimeMinimaFoaieModulRandom + pointD3.Y;
									}
									PanelObject value = (PanelObject)item.Clone(pointD3, pointD2);
									currentPanel.panelingObjects.Add(value);
								}
							}
							else
							{
								PointD pointD4 = (PointD)item.stopPoint.Clone();
								item.stopPoint.Y = Math.Ceiling((pointD.Y - item.startPoint.Y) / 10.0) * 10.0 + item.startPoint.Y + item.petrecereFoi;
								item.UpdateLines();
								PointD pointD5 = new PointD(item.startPoint.X, item.stopPoint.Y - item.petrecereFoi);
								if (pointD4.Y - pointD5.Y < item.lungimeMinimaFoaieModulRandom && !item.tipTigla.Contains("LTP"))
								{
									pointD4.Y = item.lungimeMinimaFoaieModulRandom + pointD5.Y;
								}
								PanelObject value2 = (PanelObject)item.Clone(pointD5, pointD4);
								currentPanel.panelingObjects.Add(value2);
							}
						}
						((Document)base.ParentForm).enableCommand = false;
						((Document)base.ParentForm).activeCommand = 0;
						RedrawAll();
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
						if (!CheckHittingTestofASheet(new Point(e.X, e.Y)) && !CheckHittingTestofAnObject(new Point(e.X, e.Y)) && !CheckHittingTestofAnVoidAndReturnTheVoid(new Point(e.X, e.Y)))
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
			case 3:
				PutVoid(mouse);
				activeCommand = 0;
				break;
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
				deltaX = 0;
				deltaY = 0;
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
			base_point_x += (double)(e.X - start_translate_x) * scale * -1.0;
			base_point_y += (double)(e.Y - start_translate_y) * scale;
			deltaX += e.X - start_translate_x;
			deltaY += e.Y - start_translate_y;
			start_translate_x = e.X;
			start_translate_y = e.Y;
			paning = true;
			PasteContent(deltaX, deltaY);
			paning = false;
			return;
		}
		switch (activeCommand)
		{
		case 0:
		{
			if (((Document)base.ParentForm).enableCommand)
			{
				PasteContent();
				switch (((Document)base.ParentForm).activeCommand)
				{
				case 15:
					toolTip(insertPoint: new Point(e.X, e.Y), theString: dictionar.dictionar[190]);
					break;
				case 16:
				case 17:
					toolTip(insertPoint: new Point(e.X, e.Y), theString: dictionar.dictionar[191]);
					break;
				}
			}
			else if (iAmSelecting)
			{
				PasteContent(rec);
				Point point2 = TransformFromDrawing(selectionFirstPoint);
				Point[] points = new Point[5]
				{
					point2,
					new Point(e.X, point2.Y),
					new Point(e.X, e.Y),
					new Point(point2.X, e.Y),
					point2
				};
				CreateGraphics().DrawLines(caretLinePen, points);
			}
			double x_drawing_in_cm = (double)e.X * scale + base_point_x;
			double y_drawing_in_cm = base_point_y - (double)e.Y * scale;
			mainwindow.UpdateCoords(x_drawing_in_cm, y_drawing_in_cm, unit);
			break;
		}
		case 3:
		{
			mouseD = TransformFromScreen(new Point(e.X, e.Y));
			ref Point reference = ref voidPoints[0];
			reference = TransformFromDrawing(new PointD(mouseD.X - (double)(voidLength / 2), mouseD.Y - (double)(voidHeigh / 2)));
			ref Point reference2 = ref voidPoints[1];
			reference2 = TransformFromDrawing(new PointD(mouseD.X + (double)(voidLength / 2), mouseD.Y - (double)(voidHeigh / 2)));
			if (!trapezoidal)
			{
				ref Point reference3 = ref voidPoints[2];
				reference3 = TransformFromDrawing(new PointD(mouseD.X + (double)(voidLength / 2), mouseD.Y + (double)(voidHeigh / 2)));
				ref Point reference4 = ref voidPoints[3];
				reference4 = TransformFromDrawing(new PointD(mouseD.X - (double)(voidLength / 2), mouseD.Y + (double)(voidHeigh / 2)));
			}
			else
			{
				ref Point reference5 = ref voidPoints[2];
				reference5 = TransformFromDrawing(new PointD(mouseD.X + (double)(voidLength2 / 2), mouseD.Y + (double)(voidHeigh / 2)));
				ref Point reference6 = ref voidPoints[3];
				reference6 = TransformFromDrawing(new PointD(mouseD.X - (double)(voidLength2 / 2), mouseD.Y + (double)(voidHeigh / 2)));
			}
			Region region = new Region();
			region.MakeEmpty();
			region.Union(currentPanel.graphicsPath(100, base_point_x, base_point_y, scale, bm));
			bool flag = true;
			Point[] array = voidPoints;
			foreach (Point point in array)
			{
				if (!region.IsVisible(point))
				{
					flag = false;
				}
			}
			if (flag)
			{
				DrawCurrentVoid();
				break;
			}
			PasteContent(rec);
			Graphics graphics = CreateGraphics();
			graphics.DrawPolygon(pen1, voidPoints);
			break;
		}
		}
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
		if (base.Width != 0 && base.Height != 0)
		{
			bm?.Dispose();
			cuttedBm?.Dispose();
			bm = new Bitmap(Screen.PrimaryScreen.Bounds.Width * 3, Screen.PrimaryScreen.Bounds.Height * 3);
			rec = new Rectangle(bm.Width / 3, bm.Height / 3, bm.Width / 3, bm.Height / 3);
			cuttedBm = new Bitmap(rec.Width, rec.Height);
		}
		base_point_x -= (double)(base.Size.Width - sizeOfTable_x) * scale / 2.0;
		base_point_y += (double)(base.Size.Height - sizeOfTable_y) * scale / 2.0;
		sizeOfTable_x = base.Size.Width;
		sizeOfTable_y = base.Size.Height;
		RedrawAll();
	}

	public void GraphicTable_MouseWheel(object sender, MouseEventArgs e)
	{
		double num = e.Delta / 120;
		double num2 = scale;
		scale = Math.Round(scale * (1.0 - num / 10.0), 3);
		if (scale <= 0.01)
		{
			scale = 0.01;
		}
		base_point_x += (double)(bm.Width / 3 + e.X) * (scale - num2) * -1.0;
		base_point_y += (double)(bm.Height / 3 + e.Y) * (scale - num2);
		RedrawAll();
		int num3 = activeCommand;
		if (num3 == 3)
		{
			DrawCurrentVoid();
		}
	}

	public bool CheckKeys(Keys keyData)
	{
		bool result = true;
		switch (keyData)
		{
		case Keys.F10:
			Clipboard.SetImage(cuttedBm);
			break;
		case Keys.Escape:
			CheckCommandForEnding(escapeCase: true);
			activeCommand = 0;
			RedrawAll();
			break;
		case Keys.Left:
			switch (activeCommand)
			{
			case 0:
				voidSelected = false;
				foreach (voids @void in currentPanel.voidList)
				{
					if (@void.isSelected)
					{
						voidSelected = true;
						@void.Reinit(-1.0, 0.0);
						@void.UpdateDrawingCoords(scale, base_point_x, base_point_y, bm);
					}
				}
				if (!voidSelected)
				{
					{
						IEnumerator enumerator8 = currentPanel.panelingObjects.GetEnumerator();
						try
						{
							if (enumerator8.MoveNext())
							{
								PanelObject panelObject6 = (PanelObject)enumerator8.Current;
								panelObject6.startPoint.X -= 1.0;
								ManualCuttingOptimization(panelObject6.startPoint);
								parentDocument.ActualizeazaAriaTablei(currentPanel);
							}
						}
						finally
						{
							IDisposable disposable = enumerator8 as IDisposable;
							if (disposable != null)
							{
								disposable.Dispose();
							}
						}
					}
				}
				else
				{
					RedrawAll();
				}
				break;
			case 3:
				if (voidLength > 1)
				{
					voidLength--;
				}
				DrawCurrentVoid();
				break;
			}
			break;
		case Keys.Right:
			switch (activeCommand)
			{
			case 0:
				voidSelected = false;
				foreach (voids void2 in currentPanel.voidList)
				{
					if (void2.isSelected)
					{
						voidSelected = true;
						void2.Reinit(1.0, 0.0);
						void2.UpdateDrawingCoords(scale, base_point_x, base_point_y, bm);
					}
				}
				if (!voidSelected)
				{
					{
						IEnumerator enumerator6 = currentPanel.panelingObjects.GetEnumerator();
						try
						{
							if (enumerator6.MoveNext())
							{
								PanelObject panelObject4 = (PanelObject)enumerator6.Current;
								panelObject4.startPoint.X += 1.0;
								ManualCuttingOptimization(panelObject4.startPoint);
								parentDocument.ActualizeazaAriaTablei(currentPanel);
							}
						}
						finally
						{
							IDisposable disposable2 = enumerator6 as IDisposable;
							if (disposable2 != null)
							{
								disposable2.Dispose();
							}
						}
					}
				}
				else
				{
					RedrawAll();
				}
				break;
			case 3:
				voidLength++;
				DrawCurrentVoid();
				break;
			}
			break;
		case Keys.Left | Keys.Control:
			voidSelected = false;
			foreach (voids void3 in currentPanel.voidList)
			{
				if (void3.isSelected)
				{
					voidSelected = true;
					void3.Reinit(-10.0, 0.0);
					void3.UpdateDrawingCoords(scale, base_point_x, base_point_y, bm);
				}
			}
			if (!voidSelected)
			{
				{
					IEnumerator enumerator12 = currentPanel.panelingObjects.GetEnumerator();
					try
					{
						if (enumerator12.MoveNext())
						{
							PanelObject panelObject8 = (PanelObject)enumerator12.Current;
							panelObject8.startPoint.X -= 10.0;
							ManualCuttingOptimization(panelObject8.startPoint);
						}
					}
					finally
					{
						IDisposable disposable3 = enumerator12 as IDisposable;
						if (disposable3 != null)
						{
							disposable3.Dispose();
						}
					}
				}
			}
			else
			{
				RedrawAll();
			}
			break;
		case Keys.Right | Keys.Control:
			voidSelected = false;
			foreach (voids void4 in currentPanel.voidList)
			{
				if (void4.isSelected)
				{
					voidSelected = true;
					void4.Reinit(10.0, 0.0);
					void4.UpdateDrawingCoords(scale, base_point_x, base_point_y, bm);
				}
			}
			if (!voidSelected)
			{
				{
					IEnumerator enumerator10 = currentPanel.panelingObjects.GetEnumerator();
					try
					{
						if (enumerator10.MoveNext())
						{
							PanelObject panelObject7 = (PanelObject)enumerator10.Current;
							panelObject7.startPoint.X += 10.0;
							ManualCuttingOptimization(panelObject7.startPoint);
						}
					}
					finally
					{
						IDisposable disposable4 = enumerator10 as IDisposable;
						if (disposable4 != null)
						{
							disposable4.Dispose();
						}
					}
				}
			}
			else
			{
				RedrawAll();
			}
			break;
		case Keys.Left | Keys.Shift | Keys.Control:
			voidSelected = false;
			foreach (voids void5 in currentPanel.voidList)
			{
				if (void5.isSelected)
				{
					voidSelected = true;
					void5.Reinit(-100.0, 0.0);
					void5.UpdateDrawingCoords(scale, base_point_x, base_point_y, bm);
				}
			}
			if (voidSelected)
			{
				RedrawAll();
			}
			break;
		case Keys.Right | Keys.Shift | Keys.Control:
			voidSelected = false;
			foreach (voids void6 in currentPanel.voidList)
			{
				if (void6.isSelected)
				{
					voidSelected = true;
					void6.Reinit(100.0, 0.0);
					void6.UpdateDrawingCoords(scale, base_point_x, base_point_y, bm);
				}
			}
			if (voidSelected)
			{
				RedrawAll();
			}
			break;
		case Keys.Up:
			switch (activeCommand)
			{
			case 0:
				voidSelected = false;
				foreach (voids void7 in currentPanel.voidList)
				{
					if (void7.isSelected)
					{
						voidSelected = true;
						void7.Reinit(0.0, 1.0);
						void7.UpdateDrawingCoords(scale, base_point_x, base_point_y, bm);
					}
				}
				if (!voidSelected)
				{
					foreach (PanelObject panelingObject in currentPanel.panelingObjects)
					{
						if (panelingObject.isSelected)
						{
							if (panelingObject.Change(goingUp: true, 1))
							{
								RedrawAll();
							}
							break;
						}
					}
				}
				else
				{
					RedrawAll();
				}
				break;
			case 3:
				voidHeigh++;
				DrawCurrentVoid();
				break;
			}
			break;
		case Keys.Right | Keys.Alt:
		{
			int num2 = activeCommand;
			if (num2 == 3)
			{
				voidLength2++;
				DrawCurrentVoid();
			}
			break;
		}
		case Keys.Left | Keys.Alt:
		{
			int num = activeCommand;
			if (num == 3)
			{
				if (voidLength2 > 1)
				{
					voidLength2--;
				}
				else
				{
					voidLength2 = 0;
				}
				DrawCurrentVoid();
			}
			break;
		}
		case Keys.Up | Keys.Control:
			voidSelected = false;
			foreach (voids void8 in currentPanel.voidList)
			{
				if (void8.isSelected)
				{
					voidSelected = true;
					void8.Reinit(0.0, 10.0);
					void8.UpdateDrawingCoords(scale, base_point_x, base_point_y, bm);
				}
			}
			if (!voidSelected)
			{
				foreach (PanelObject panelingObject2 in currentPanel.panelingObjects)
				{
					if (panelingObject2.isSelected)
					{
						if (panelingObject2.Change(goingUp: true, 10))
						{
							RedrawAll();
						}
						break;
					}
				}
			}
			else
			{
				RedrawAll();
			}
			break;
		case Keys.Up | Keys.Shift | Keys.Control:
			voidSelected = false;
			foreach (voids void9 in currentPanel.voidList)
			{
				if (void9.isSelected)
				{
					voidSelected = true;
					void9.Reinit(0.0, 100.0);
					void9.UpdateDrawingCoords(scale, base_point_x, base_point_y, bm);
				}
			}
			if (!voidSelected)
			{
				foreach (PanelObject panelingObject3 in currentPanel.panelingObjects)
				{
					if (panelingObject3.isSelected)
					{
						if (panelingObject3.Change(goingUp: true, 100))
						{
							RedrawAll();
						}
						break;
					}
				}
			}
			else
			{
				RedrawAll();
			}
			break;
		case Keys.Down:
			switch (activeCommand)
			{
			case 0:
				voidSelected = false;
				foreach (voids void10 in currentPanel.voidList)
				{
					if (void10.isSelected)
					{
						voidSelected = true;
						void10.Reinit(0.0, -1.0);
						void10.UpdateDrawingCoords(scale, base_point_x, base_point_y, bm);
					}
				}
				if (!voidSelected)
				{
					foreach (PanelObject panelingObject4 in currentPanel.panelingObjects)
					{
						if (panelingObject4.isSelected)
						{
							if (panelingObject4.Change(goingUp: false, 1))
							{
								RedrawAll();
							}
							break;
						}
					}
				}
				else
				{
					RedrawAll();
				}
				break;
			case 3:
				if (voidHeigh > 1)
				{
					voidHeigh--;
				}
				DrawCurrentVoid();
				break;
			}
			break;
		case Keys.Down | Keys.Control:
			voidSelected = false;
			foreach (voids void11 in currentPanel.voidList)
			{
				if (void11.isSelected)
				{
					voidSelected = true;
					void11.Reinit(0.0, -10.0);
					void11.UpdateDrawingCoords(scale, base_point_x, base_point_y, bm);
				}
			}
			if (!voidSelected)
			{
				foreach (PanelObject panelingObject5 in currentPanel.panelingObjects)
				{
					if (panelingObject5.isSelected)
					{
						if (panelingObject5.Change(goingUp: false, 10))
						{
							RedrawAll();
						}
						break;
					}
				}
			}
			else
			{
				RedrawAll();
			}
			break;
		case Keys.Down | Keys.Shift | Keys.Control:
			voidSelected = false;
			foreach (voids void12 in currentPanel.voidList)
			{
				if (void12.isSelected)
				{
					voidSelected = true;
					void12.Reinit(0.0, -100.0);
					void12.UpdateDrawingCoords(scale, base_point_x, base_point_y, bm);
				}
			}
			if (!voidSelected)
			{
				foreach (PanelObject panelingObject6 in currentPanel.panelingObjects)
				{
					if (panelingObject6.isSelected)
					{
						if (panelingObject6.Change(goingUp: false, 100))
						{
							RedrawAll();
						}
						break;
					}
				}
			}
			else
			{
				RedrawAll();
			}
			break;
		case Keys.Delete:
		{
			foreach (PanelObject panelingObject7 in currentPanel.panelingObjects)
			{
				if (panelingObject7.isSelected)
				{
					currentPanel.panelingObjects.Remove(panelingObject7);
					panelingObject7.Dispose();
					RedrawAll();
					parentDocument.ActualizeazaAriaTablei(currentPanel);
					break;
				}
			}
			for (byte b = 0; b < currentPanel.voidList.Count; b++)
			{
				if (((voids)currentPanel.voidList[b]).isSelected)
				{
					currentPanel.voidList.Remove((voids)currentPanel.voidList[b]);
					b--;
				}
			}
			RedrawAll();
			break;
		}
		default:
			result = false;
			break;
		}
		return result;
	}

	private void GraphicTableForPanels_KeyPress(object sender, KeyPressEventArgs e)
	{
	}

	private void InitializeComponent()
	{
		this.BackColor = System.Drawing.SystemColors.Desktop;
		this.Cursor = System.Windows.Forms.Cursors.Cross;
		this.ForeColor = System.Drawing.SystemColors.ControlText;
		base.Name = "GraphicTableForPanels";
		base.KeyPress += new System.Windows.Forms.KeyPressEventHandler(GraphicTableForPanels_KeyPress);
		base.SizeChanged += new System.EventHandler(GraphicTable_SizeChanged);
		base.MouseUp += new System.Windows.Forms.MouseEventHandler(GraphicTable_MouseUp);
		base.MouseMove += new System.Windows.Forms.MouseEventHandler(GraphicTable_MouseMove);
		base.MouseWheel += new System.Windows.Forms.MouseEventHandler(GraphicTable_MouseWheel);
		base.MouseDown += new System.Windows.Forms.MouseEventHandler(GraphicTable_MouseDown);
		this.caretLinePen = new System.Drawing.Pen(this.caretLineColor, 1f);
		this.caretLinePen.DashPattern = this.caretLinePattern;
		this.pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(OnPrintPage);
		this.bm = new System.Drawing.Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width * 3, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height * 3, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
		this.rec = new System.Drawing.Rectangle(this.bm.Width / 3, this.bm.Height / 3, this.bm.Width / 3, this.bm.Height / 3);
		this.cuttedBm = new System.Drawing.Bitmap(this.rec.Width, this.rec.Height);
		this.base_point_x = 0.0;
		this.base_point_y = 0.0;
		this.scale = 1.0;
		this.strConnect = "Provider=Microsoft.Jet.OLEDB.4.0;";
		this.strConnect = this.strConnect + "Data Source=" + System.Windows.Forms.Application.StartupPath + "/lindroof.dbf;";
		this.strConnect += "Persist Security Info=False";
		this.objConnection.ConnectionString = this.strConnect;
		this.oleCommand.Connection = this.objConnection;
		this.dtab2 = new System.Data.DataTable();
		this.dtab3 = new System.Data.DataTable();
		this.dtab4 = new System.Data.DataTable();
		this.pd.DefaultPageSettings.Landscape = true;
		this.pd.DefaultPageSettings.Margins.Bottom = 20;
		this.pd.DefaultPageSettings.Margins.Top = 100;
		this.pd.DefaultPageSettings.Margins.Left = 20;
		this.pd.DefaultPageSettings.Margins.Right = 20;
		base.SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, true);
	}

	[DllImport("Msimg32.dll")]
	public static extern bool TransparentBlt(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest, int hHeightDest, IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc, int crTransparent);

	[DllImport("gdi32.dll")]
	private static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);

	[DllImport("User32.dll")]
	public static extern IntPtr GetDC(IntPtr hWnd);

	[DllImport("User32.dll")]
	public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

	[DllImport("gdi32")]
	private static extern int AddFontResource(string lpszFilename);

	public static bool AddFont(string filename)
	{
		if (File.Exists(filename))
		{
			return AddFontResource(filename) != 0;
		}
		return false;
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
		string documentName = dictionar.dictionar[185] + ((Document)base.ParentForm).Text;
		pd.DocumentName = documentName;
		PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
		printPreviewDialog.Document = pd;
		printPreviewDialog.PrintPreviewControl.Zoom = 1.0;
		printPreviewDialog.WindowState = FormWindowState.Maximized;
		printPreviewDialog.ShowDialog(this);
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
		Bitmap bitmap = new Bitmap(bm.Width, bm.Height);
		PrintAll(Graphics.FromImage(bitmap));
		Bitmap image = bitmap.Clone(new Rectangle(rec.X, rec.Y, base.ClientSize.Width, base.ClientSize.Height), bitmap.PixelFormat);
		Clipboard.SetImage(image);
		int num = e.MarginBounds.X;
		int num2 = e.MarginBounds.Y;
		int num3 = base.ClientSize.Width;
		int num4 = base.ClientSize.Height;
		int srcWidth = num3;
		int srcHeight = num4;
		if ((float)num3 / (float)e.MarginBounds.Width > (float)num4 / (float)e.MarginBounds.Height)
		{
			num4 = (int)Math.Ceiling((float)num4 * (float)e.MarginBounds.Width / (float)num3);
			num3 = e.MarginBounds.Width;
		}
		else
		{
			num3 = (int)Math.Ceiling((float)num3 * (float)e.MarginBounds.Height / (float)num4);
			num4 = e.MarginBounds.Height;
		}
		num3 = (int)Math.Ceiling((double)(float)num3 * 0.98);
		num4 = (int)Math.Ceiling((double)(float)num4 * 0.98);
		Rectangle destRect = new Rectangle(num, num2, num3, num4);
		e.Graphics.DrawImage(image, destRect, 0, 0, srcWidth, srcHeight, GraphicsUnit.Pixel);
		sf.Alignment = StringAlignment.Near;
		sf.LineAlignment = StringAlignment.Near;
		e.Graphics.DrawString(base.ParentForm.Text + dictionar.dictionar[186], new Font("Arial", 11f), new SolidBrush(Color.Black), 80f, pd.DefaultPageSettings.Bounds.Height - 80, sf);
	}

	public void PrintAll(Graphics g)
	{
		g.Clear(Color.White);
		DrawPanels(g, print: true);
		DrawVoids(g, print: true);
		g.Dispose();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		PasteContent();
		base.OnPaint(e);
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
		catch
		{
		}
	}

	private void DrawUCS(Graphics gr)
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

	public void RedrawAll()
	{
		Graphics graphics = Graphics.FromImage(bm);
		graphics.Clear(background);
		DrawPanels(graphics);
		DrawVoids(graphics, print: false);
		graphics.Dispose();
		cuttedBm?.Dispose();
		cuttedBm = bm.Clone(rec, bm.PixelFormat);
		PasteContent();
	}

	private void DrawIntersectingLines(Graphics gr)
	{
		if (intersectingLines.Count != 0)
		{
			foreach (GraphicObject intersectingLine in intersectingLines)
			{
				intersectingLine.UpdateDrawingCoords(scale, base_point_x, base_point_y);
			}
		}
		if (intersectingLines.Count == 0)
		{
			return;
		}
		foreach (GraphicObject intersectingLine2 in intersectingLines)
		{
			intersectingLine2.Draw(gr, scale, base_point_x, base_point_y);
		}
	}

	private void DrawPanels(Graphics gr, bool print)
	{
		if (panels == null && panels.Count == 0)
		{
			return;
		}
		currentPanel.currentScale = scale;
		currentPanel.currentbpx = base_point_x;
		currentPanel.currentbpy = base_point_y;
		if (currentPanel.objects != null || currentPanel.objects.Count != 0)
		{
			foreach (GraphicObject @object in currentPanel.objects)
			{
				@object.UpdateDrawingCoords(scale, base_point_x, base_point_y);
			}
		}
		if (currentPanel.objects != null || currentPanel.objects.Count != 0)
		{
			foreach (GraphicObject object2 in currentPanel.objects)
			{
				object2.Draw(gr, scale, base_point_x, base_point_y, print);
			}
		}
		if (!currentPanel.evolved)
		{
			HatchBrush hatchBrush = (print ? new HatchBrush(HatchStyle.ZigZag, Color.Black, Color.Transparent) : new HatchBrush(HatchStyle.ZigZag, Color.Gray, Color.Transparent));
			gr.FillPath(hatchBrush, currentPanel.graphicsPath(100, base_point_x, base_point_y, scale, bm));
			hatchBrush.Dispose();
		}
		else if (currentPanel.panelingObjects.Count != 0)
		{
			currentPanel.DrawTableSheets(gr, this, scale, base_point_x, base_point_y, print);
		}
		PointD centerOfGravity = currentPanel.centerOfGravity;
		Point point = new Point((int)((centerOfGravity.X - base_point_x) / scale), (int)((base_point_y - centerOfGravity.Y) / scale));
		sf.Alignment = StringAlignment.Center;
		sf.LineAlignment = StringAlignment.Center;
		gr.DrawString(brush: new SolidBrush(print ? Color.Black : Color.FromArgb(255 - background.R, 255 - background.G, 255 - background.B)), s: currentPanel.panelName, font: new Font("ArialBold", 11f), point: point, format: sf);
	}

	private void DrawPanels(Graphics gr)
	{
		if (panels == null && panels.Count == 0)
		{
			return;
		}
		currentPanel.currentScale = scale;
		currentPanel.currentbpx = base_point_x;
		currentPanel.currentbpy = base_point_y;
		if (currentPanel.objects != null || currentPanel.objects.Count != 0)
		{
			foreach (GraphicObject @object in currentPanel.objects)
			{
				@object.UpdateDrawingCoords(scale, base_point_x, base_point_y);
			}
		}
		if (currentPanel.objects != null || currentPanel.objects.Count != 0)
		{
			foreach (GraphicObject object2 in currentPanel.objects)
			{
				object2.Draw(gr, scale, base_point_x, base_point_y);
			}
		}
		if (!currentPanel.evolved)
		{
			HatchBrush hatchBrush = new HatchBrush(HatchStyle.ZigZag, Color.Gray, Color.Transparent);
			gr.FillPath(hatchBrush, currentPanel.graphicsPath(100, base_point_x, base_point_y, scale, bm));
			hatchBrush.Dispose();
		}
		else if (currentPanel.panelingObjects.Count != 0)
		{
			currentPanel.DrawTableSheets(gr, this, scale, base_point_x, base_point_y, print: false);
		}
		PointD centerOfGravity = currentPanel.centerOfGravity;
		Point point = new Point((int)((centerOfGravity.X - base_point_x) / scale), (int)((base_point_y - centerOfGravity.Y) / scale));
		sf.Alignment = StringAlignment.Center;
		sf.LineAlignment = StringAlignment.Center;
		gr.DrawString(currentPanel.panelName, new Font("Arial", 11f), new SolidBrush(Color.FromArgb(255 - background.R, 255 - background.G, 255 - background.B)), point, sf);
	}

	private void DrawVoids(Graphics gr, bool print)
	{
		if (currentPanel.voidList.Count == 0)
		{
			return;
		}
		if (print)
		{
			pen2.Color = Color.Black;
		}
		else
		{
			pen2.Color = Color.LightGray;
		}
		double num = ((line)currentPanel.objects[0]).startPoint.X;
		double num2 = ((line)currentPanel.objects[0]).startPoint.Y;
		StringFormat stringFormat = (StringFormat)sf.Clone();
		stringFormat.FormatFlags = StringFormatFlags.DirectionVertical;
		stringFormat.Alignment = StringAlignment.Center;
		foreach (line @object in currentPanel.objects)
		{
			if (@object.startPoint.X < @object.stopPoint.X)
			{
				if (num > @object.startPoint.X)
				{
					num = @object.startPoint.X;
				}
			}
			else if (num > @object.stopPoint.X)
			{
				num = @object.stopPoint.X;
			}
			if (@object.startPoint.Y < @object.stopPoint.Y)
			{
				if (num2 > @object.startPoint.Y)
				{
					num2 = @object.startPoint.Y;
				}
			}
			else if (num2 > @object.stopPoint.Y)
			{
				num2 = @object.stopPoint.Y;
			}
		}
		foreach (voids @void in currentPanel.voidList)
		{
			@void.DrawEvolved(gr, scale, base_point_x, base_point_y, bm, print);
			offsetX = @void.offsetX;
			offsetY = @void.offsetY;
			PointD p = new PointD(@void.startPoint.X + (double)(bm.Width / 3) * scale, @void.startPoint.Y - @void.heigh - (double)(bm.Height / 3) * scale);
			PointD pointD = new PointD(num + (double)(bm.Width / 3) * scale, @void.startPoint.Y - @void.heigh - (double)(bm.Height / 3) * scale);
			firstP = TransformFromDrawing(p);
			secondP = TransformFromDrawing(pointD);
			gr.DrawLine(pen2, firstP, secondP);
			gr.DrawString(Math.Round(offsetX * 10.0, 0).ToString(), font, pen2.Brush, (float)(firstP.X + secondP.X) / 2f, (float)firstP.Y - 8f, sf);
			pointD.X = @void.startPoint.X + (double)(bm.Width / 3) * scale;
			pointD.Y = num2 - (double)(bm.Height / 3) * scale;
			secondP = TransformFromDrawing(pointD);
			gr.DrawLine(pen2, firstP, secondP);
			gr.DrawString(Math.Round(offsetY * 10.0, 0).ToString(), font, pen2.Brush, firstP.X + (int)(Font.GetHeight(gr) / 2f + 2f), (float)(firstP.Y + secondP.Y) / 2f, stringFormat);
		}
	}

	private void CopyContent()
	{
		try
		{
			if (base.Height > 0)
			{
				IntPtr dC = GetDC(base.Handle);
				Graphics graphics = Graphics.FromImage(cuttedBm);
				IntPtr hdc = graphics.GetHdc();
				BitBlt(hdc, 0, 0, bm.Width, bm.Height, dC, 0, 0, 13369376u);
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
			graphics.DrawImage(cuttedBm, 0, 0);
			graphics.Dispose();
		}
		catch (Exception ex)
		{
			string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
			MessageBox.Show(this, text);
		}
	}

	private void PasteContent(int xOff, int yOff)
	{
		try
		{
			Graphics graphics = CreateGraphics();
			newRec.X = rec.X - xOff;
			newRec.Y = rec.Y - yOff;
			newRec.Width = rec.Width;
			newRec.Height = rec.Height;
			graphics.DrawImage(bm, 0, 0, newRec, GraphicsUnit.Pixel);
			graphics.Dispose();
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
			graphics.DrawImage(cuttedBm, 0, 0);
			graphics.Dispose();
		}
		catch (Exception ex)
		{
			string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
			MessageBox.Show(this, text);
		}
	}

	private void PutVoid(Point mouseP)
	{
		Region region = new Region();
		region.MakeEmpty();
		region.Union(currentPanel.graphicsPath(100, base_point_x, base_point_y, scale, bm));
		bool flag = true;
		Point[] array = voidPoints;
		foreach (Point point in array)
		{
			if (!region.IsVisible(point))
			{
				flag = false;
			}
		}
		if (flag)
		{
			voids voids2 = new voids(dictionar.dictionar[192], new PointD(Math.Round(TransformFromScreen(mouseP).X, 0), Math.Round(TransformFromScreen(mouseP).Y, 0)), voidLength, voidLength2, voidHeigh, trapezoidal);
			voids2.offsetX = offsetX;
			voids2.offsetY = offsetY;
			currentPanel.voidList.Add(voids2);
			RedrawAll();
		}
	}

	private void DrawCurrentVoid()
	{
		Graphics graphics = CreateGraphics();
		currentVoid = new voids(dictionar.dictionar[192], new PointD(Math.Round(mouseD.X, 0), Math.Round(mouseD.Y, 0)), voidLength, voidLength2, voidHeigh, trapezoidal);
		PasteContent();
		currentVoid.DrawEvolved2(graphics, scale, base_point_x, base_point_y, bm, print: false);
		StringFormat stringFormat = (StringFormat)sf.Clone();
		stringFormat.FormatFlags = StringFormatFlags.DirectionVertical;
		stringFormat.Alignment = StringAlignment.Center;
		double num = ((line)currentPanel.objects[0]).startPoint.X;
		double num2 = ((line)currentPanel.objects[0]).startPoint.Y;
		foreach (line @object in currentPanel.objects)
		{
			if (@object.startPoint.X < @object.stopPoint.X)
			{
				if (num > @object.startPoint.X)
				{
					num = @object.startPoint.X;
				}
			}
			else if (num > @object.stopPoint.X)
			{
				num = @object.stopPoint.X;
			}
			if (@object.startPoint.Y < @object.stopPoint.Y)
			{
				if (num2 > @object.startPoint.Y)
				{
					num2 = @object.startPoint.Y;
				}
			}
			else if (num2 > @object.stopPoint.Y)
			{
				num2 = @object.stopPoint.Y;
			}
		}
		offsetX = currentVoid.startPoint.X - num;
		offsetY = currentVoid.startPoint.Y - currentVoid.heigh - num2;
		Point pt = TransformFromDrawing(new PointD(currentVoid.startPoint.X, currentVoid.startPoint.Y - currentVoid.heigh));
		Point pt2 = TransformFromDrawing(new PointD(num, currentVoid.startPoint.Y - currentVoid.heigh));
		graphics.DrawLine(pen2, pt, pt2);
		graphics.DrawString(Math.Round(offsetX * 10.0, 0).ToString(), font, pen.Brush, (float)(pt.X + pt2.X) / 2f, (float)pt.Y - 8f, sf);
		pt = TransformFromDrawing(new PointD(currentVoid.startPoint.X, currentVoid.startPoint.Y - currentVoid.heigh));
		pt2 = TransformFromDrawing(new PointD(currentVoid.startPoint.X, num2));
		graphics.DrawLine(pen2, pt, pt2);
		graphics.DrawString(Math.Round(offsetY * 10.0, 0).ToString(), font, pen.Brush, pt.X + (int)(Font.GetHeight(graphics) / 2f + 2f), (float)(pt.Y + pt2.Y) / 2f, stringFormat);
		pt = TransformFromDrawing(new PointD(currentVoid.startPoint.X, num2));
		pt2 = TransformFromDrawing(new PointD(num, num2));
		graphics.DrawLine(caretLinePen, pt, pt2);
		pt = TransformFromDrawing(new PointD(num, currentVoid.startPoint.Y - currentVoid.heigh));
		graphics.DrawLine(caretLinePen, pt, pt2);
		graphics.Dispose();
	}

	private bool CheckForObjectsInSelectedArea(PointD selectionFirstPoint, PointD selectionSecondPoint)
	{
		bool result = false;
		try
		{
			double num = 0.0;
			foreach (GraphicObject @object in currentPanel.objects)
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

	private bool CheckHittingTestofASheet(Point mouse)
	{
		bool flag = false;
		PointD pointD = TransformFromScreen(mouse);
		try
		{
			foreach (PanelObject panelingObject in currentPanel.panelingObjects)
			{
				if (!panelingObject.CheckIfHitted(pointD))
				{
					continue;
				}
				bool isSelected = panelingObject.isSelected;
				foreach (PanelObject panelingObject2 in currentPanel.panelingObjects)
				{
					panelingObject2.isSelected = false;
				}
				panelingObject.isSelected = isSelected;
				flag = true;
				break;
			}
		}
		catch (Exception ex)
		{
			string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
			MessageBox.Show(this, text);
		}
		if (flag)
		{
			RedrawAll();
		}
		return flag;
	}

	private bool CheckHittingTestofAnObject(Point mouse)
	{
		bool result = false;
		try
		{
			foreach (GraphicObject @object in currentPanel.objects)
			{
				GraphicsPath graphicsPath = @object.graphicsPath(15);
				if (graphicsPath.IsVisible(mouse))
				{
					if (@object.isSelected)
					{
						@object.isSelected = false;
					}
					else
					{
						@object.isSelected = true;
					}
					mainwindow.WriteCurrentObjectLength(@object.length, unit);
					RedrawAll();
					result = true;
					break;
				}
			}
			double num = 0.0;
			foreach (GraphicObject object2 in currentPanel.objects)
			{
				if (object2.isSelected)
				{
					num += object2.length;
				}
			}
			mainwindow.WriteCurrentObjectLength(num, unit);
		}
		catch
		{
		}
		return result;
	}

	private bool CheckHittingTestofAnVoidAndReturnTheVoid(Point mouse)
	{
		foreach (voids @void in currentPanel.voidList)
		{
			if (@void.Hitted(mouse))
			{
				@void.isSelected = !@void.isSelected;
				RedrawAll();
				return true;
			}
		}
		return false;
	}

	private GraphicObject CheckHittingTestofAnObjectAndReturnTheObject(Point mouse)
	{
		GraphicObject result = null;
		try
		{
			foreach (GraphicObject @object in currentPanel.objects)
			{
				GraphicsPath graphicsPath = @object.graphicsPath(15, base_point_x, base_point_y, scale, bm);
				if (graphicsPath.IsVisible(mouse))
				{
					@object.isSelected = true;
					result = @object;
					break;
				}
			}
		}
		catch
		{
		}
		return result;
	}

	public void CheckCommandForEnding(bool escapeCase)
	{
		foreach (GraphicObject @object in currentPanel.objects)
		{
			@object.isSelected = false;
		}
		foreach (PanelObject panelingObject in currentPanel.panelingObjects)
		{
			panelingObject.isSelected = false;
		}
		foreach (voids @void in currentPanel.voidList)
		{
			@void.isSelected = false;
		}
		iAmSelecting = false;
		RedrawAll();
	}

	public void CheckCommandForEnding()
	{
		foreach (GraphicObject @object in currentPanel.objects)
		{
			@object.isSelected = false;
		}
		iAmSelecting = false;
		RedrawAll();
	}

	public PointD TransformFromScreen(Point p)
	{
		return new PointD((double)(p.X + bm.Width / 3) * scale + base_point_x, base_point_y - (double)(p.Y + bm.Height / 3) * scale);
	}

	public PointF TransformFromScreenF(Point p)
	{
		return new PointF((float)((double)(p.X + bm.Width / 3) * scale + base_point_x), (float)(base_point_y - (double)(p.Y + bm.Height / 3) * scale));
	}

	public Point TransformFromDrawing(PointD p)
	{
		return new Point((int)((p.X - base_point_x) / scale - (double)(bm.Width / 3)), (int)((base_point_y - p.Y) / scale - (double)(bm.Height / 3)));
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

	public void Cutt(PointD[] thePoints)
	{
		intersectingLines.Clear();
		currentPanel.panelingObjects.Clear();
		ArrayList arrayList = new ArrayList();
		ArrayList arrayList2 = new ArrayList();
		int num = (int)currentPanel.tipPanou.latimeFoaie;
		if (Math.Round(thePoints[1].X - thePoints[0].X, 3) % currentPanel.tipPanou.latimeFoaie == 0.0)
		{
			num = 0;
		}
		for (int i = 0; i <= (int)Math.Round(thePoints[1].X - thePoints[0].X + (double)(num * 10), 3) * 100; i += (int)(currentPanel.tipPanou.latimeFoaie * 100.0))
		{
			line value = new line(new PointD(thePoints[0].X + (double)i / 100.0, thePoints[0].Y), new PointD(thePoints[0].X + (double)i / 100.0, thePoints[1].Y), new Layer("Linii Optimizare", Color.LightGray), 0);
			intersectingLines.Add(value);
		}
		for (int i = 0; i < intersectingLines.Count - 1; i++)
		{
			arrayList = arrayList2;
			arrayList2 = new ArrayList();
			bool flag = true;
			y_max = 5.0 * Math.Pow(10.0, 300.0);
			y_min = 5.0 * Math.Pow(10.0, 300.0);
			foreach (GraphicObject @object in currentPanel.objects)
			{
				if ((@object.startPoint.X >= ((line)intersectingLines[i]).stopPoint.X && ((line)intersectingLines[i]).stopPoint.X >= @object.stopPoint.X) || (@object.startPoint.X <= ((line)intersectingLines[i]).stopPoint.X && ((line)intersectingLines[i]).stopPoint.X <= @object.stopPoint.X))
				{
					deltaXp = @object.stopPoint.X - @object.startPoint.X;
					deltaYp = @object.stopPoint.Y - @object.startPoint.Y;
					deltaLp = @object.stopPoint.X - ((line)intersectingLines[i]).stopPoint.X;
					if (Math.Abs(deltaXp) >= 0.001)
					{
						if (flag)
						{
							y_max = @object.stopPoint.Y - deltaLp * deltaYp / deltaXp;
							y_min = y_max;
							flag = false;
						}
						else
						{
							if (@object.stopPoint.Y - deltaLp * deltaYp / deltaXp >= y_max)
							{
								y_max = @object.stopPoint.Y - deltaLp * deltaYp / deltaXp;
							}
							if (@object.stopPoint.Y - deltaLp * deltaYp / deltaXp <= y_min)
							{
								y_min = @object.stopPoint.Y - deltaLp * deltaYp / deltaXp;
							}
						}
					}
					else
					{
						if (@object.startPoint.Y >= y_max)
						{
							y_max = @object.startPoint.Y;
						}
						if (@object.startPoint.Y <= y_min)
						{
							y_min = @object.startPoint.Y;
						}
						if (@object.stopPoint.Y >= y_max)
						{
							y_max = @object.stopPoint.Y;
						}
						if (@object.stopPoint.Y <= y_min)
						{
							y_min = @object.stopPoint.Y;
						}
					}
				}
				if ((@object.startPoint.X >= ((line)intersectingLines[i + 1]).stopPoint.X && ((line)intersectingLines[i + 1]).stopPoint.X >= @object.stopPoint.X) || (@object.startPoint.X <= ((line)intersectingLines[i + 1]).stopPoint.X && ((line)intersectingLines[i + 1]).stopPoint.X <= @object.stopPoint.X))
				{
					deltaXp = @object.stopPoint.X - @object.startPoint.X;
					deltaYp = @object.stopPoint.Y - @object.startPoint.Y;
					deltaLp = @object.stopPoint.X - ((line)intersectingLines[i + 1]).stopPoint.X;
					if (Math.Abs(deltaXp) >= 0.001)
					{
						if (flag)
						{
							y_max = @object.stopPoint.Y - deltaLp * deltaYp / deltaXp;
							y_min = y_max;
							flag = false;
						}
						else
						{
							if (@object.stopPoint.Y - deltaLp * deltaYp / deltaXp >= y_max)
							{
								y_max = @object.stopPoint.Y - deltaLp * deltaYp / deltaXp;
							}
							if (@object.stopPoint.Y - deltaLp * deltaYp / deltaXp <= y_min)
							{
								y_min = @object.stopPoint.Y - deltaLp * deltaYp / deltaXp;
							}
						}
					}
					else
					{
						if (@object.startPoint.Y >= y_max)
						{
							y_max = @object.startPoint.Y;
						}
						if (@object.startPoint.Y <= y_min)
						{
							y_min = @object.startPoint.Y;
						}
						if (@object.stopPoint.Y >= y_max)
						{
							y_max = @object.stopPoint.Y;
						}
						if (@object.stopPoint.Y <= y_min)
						{
							y_min = @object.stopPoint.Y;
						}
					}
				}
				if (((line)intersectingLines[i]).stopPoint.X <= @object.startPoint.X && @object.startPoint.X <= ((line)intersectingLines[i + 1]).stopPoint.X)
				{
					if (@object.startPoint.Y >= y_max)
					{
						y_max = @object.startPoint.Y;
					}
					if (@object.startPoint.Y <= y_min)
					{
						y_min = @object.startPoint.Y;
					}
				}
				if (((line)intersectingLines[i]).stopPoint.X <= @object.stopPoint.X && @object.stopPoint.X <= ((line)intersectingLines[i + 1]).stopPoint.X)
				{
					if (@object.stopPoint.Y >= y_max)
					{
						y_max = @object.stopPoint.Y;
					}
					if (@object.stopPoint.Y <= y_min)
					{
						y_min = @object.stopPoint.Y;
					}
				}
			}
			try
			{
				if (y_max == 5.0 * Math.Pow(10.0, 300.0) || y_min == 5.0 * Math.Pow(10.0, 300.0))
				{
					continue;
				}
				y_min = Math.Round(y_min, 3);
				y_max = Math.Round(y_max, 3);
				int1 = Math.Floor((y_min - Math.Round(currentPanel.bendingObject.startPoint.Y - currentPanel.tipPanou.streasina, 3)) / currentPanel.tipPanou.pasOndula);
				int2 = Math.Ceiling((y_max - Math.Round(currentPanel.bendingObject.startPoint.Y - currentPanel.tipPanou.streasina, 3)) / currentPanel.tipPanou.pasOndula);
				y_min = ((currentPanel.tipPanou.nrMinimOndule == 0) ? y_min : (int1 * currentPanel.tipPanou.pasOndula + currentPanel.bendingObject.startPoint.Y));
				y_min -= currentPanel.tipPanou.streasina;
				if (mainwindow.menuSettingsStandardPanels.Checked && currentPanel.tipPanou.optimizareStandard)
				{
					currentPanel.standardPanels = true;
					panelLength = y_max - y_min;
					if (dtab4.Rows.Count == 0)
					{
						oleCommand.CommandText = "SELECT * FROM [" + currentPanel.tipPanou.tipTigla + "]";
						objCommand.SelectCommand = oleCommand;
						dtab4.Clear();
						objConnection.Open();
						objCommand.Fill(dtab4);
						objConnection.Close();
					}
					if (dtab3.Rows.Count == 0)
					{
						oleCommand.CommandText = "SELECT idPanou, ondulePerPanou FROM [" + currentPanel.tipPanou.tipTigla + "] WHERE (idPanou IS NOT NULL) AND (idPanou > 0)";
						objCommand.SelectCommand = oleCommand;
						dtab3.Reset();
						objConnection.Open();
						objCommand.Fill(dtab3);
						objConnection.Close();
					}
					IEnumerable<DataRow> source = (from r in dtab4.AsEnumerable()
						where r.Field<double>(0) >= panelLength * 10.0
						select r).Take(1);
					ddd = source.ElementAt(0);
					for (int num2 = 2; num2 < 13 && ddd[num2].ToString() != ""; num2++)
					{
						idPanou = int.Parse(ddd[num2].ToString());
						nrOndulePerReper = int.Parse(dtab3.Rows[idPanou - 1][1].ToString());
						newPanelObjectGen = new PanelObject(currentPanel.tipPanou, doarPentruOptimizare, new PointD(((line)intersectingLines[i]).startPoint.X, y_min), new PointD(((line)intersectingLines[i]).startPoint.X + currentPanel.tipPanou.latimeFoaie, y_min + (double)nrOndulePerReper * currentPanel.tipPanou.pasOndula + currentPanel.tipPanou.petrecereFoi), thePoints[0].Y, i);
						y_min += (double)nrOndulePerReper * currentPanel.tipPanou.pasOndula;
						currentPanel.panelingObjects.Add(newPanelObjectGen);
					}
					continue;
				}
				currentPanel.standardPanels = false;
				if (y_max - y_min < currentPanel.lungimeMinimaFoaieModulRandom)
				{
					y_max = y_min + currentPanel.lungimeMinimaFoaieModulRandom;
				}
				panelLength = Math.Ceiling((y_max - y_min) * 2.0) / 2.0;
				y_max = y_min + panelLength;
				if (currentPanel.tipPanou.nrMinimOndule != currentPanel.tipPanou.nrMaximOndule)
				{
					currentPanel.tipPanou.offsetMozaic = 0.0;
				}
				if (panelLength > currentPanel.lungimeMaximaFoaie)
				{
					int num3 = 0;
					if (arrayList.Count > 0 && currentPanel.tipPanou.nrMinimOndule == 0 && ((PanelObject)arrayList[0]).height == currentPanel.lungimeMaximaFoaie)
					{
						num3 = 1;
					}
					double num4 = (double)num3 * currentPanel.tipPanou.decalajVecin;
					int num5 = (int)Math.Ceiling((panelLength + (double)num3 * currentPanel.tipPanou.decalajVecin) / ((double)currentPanel.tipPanou.nrMaximOndule * currentPanel.tipPanou.pasOndula));
					PointD pointD;
					PointD pointD2;
					double num8;
					PanelObject panelObject;
					if (y_max - (y_min + (double)currentPanel.tipPanou.nrMaximOndule * currentPanel.tipPanou.pasOndula * (double)(num5 - 1)) > currentPanel.lungimeMinimaFoaieModulRandom)
					{
						for (int num6 = 0; num6 < num5 - 1; num6++)
						{
							double num7 = ((num6 == 0) ? 0.0 : num4);
							pointD = new PointD(((line)intersectingLines[i]).startPoint.X, y_min + (double)currentPanel.tipPanou.nrMaximOndule * currentPanel.tipPanou.pasOndula * (double)num6 - num7);
							pointD2 = new PointD(((line)intersectingLines[i]).startPoint.X + currentPanel.tipPanou.latimeFoaie, y_min + (double)currentPanel.tipPanou.nrMaximOndule * currentPanel.tipPanou.pasOndula * (double)(num6 + 1) + currentPanel.tipPanou.petrecereFoi - num4);
							num8 = (pointD.Y - thePoints[0].Y) / currentPanel.tipPanou.pasOndula * currentPanel.tipPanou.offsetMozaic % currentPanel.tipPanou.latimeFoaie;
							pointD.X += num8;
							pointD2.X += num8;
							panelObject = new PanelObject(currentPanel.tipPanou, doarPentruOptimizare, pointD, pointD2, thePoints[0].Y, i);
							panelObject.offsetMozaic = num8;
							currentPanel.panelingObjects.Add(panelObject);
							arrayList2.Add(panelObject);
						}
						pointD = new PointD(((line)intersectingLines[i]).startPoint.X, y_min + (double)currentPanel.tipPanou.nrMaximOndule * currentPanel.tipPanou.pasOndula * (double)(num5 - 1) - num4);
						pointD2 = new PointD(((line)intersectingLines[i]).startPoint.X + currentPanel.tipPanou.latimeFoaie, y_max);
						num8 = (pointD.Y - thePoints[0].Y) / currentPanel.tipPanou.pasOndula * currentPanel.tipPanou.offsetMozaic % currentPanel.tipPanou.latimeFoaie;
						pointD.X += num8;
						pointD2.X += num8;
						panelObject = new PanelObject(currentPanel.tipPanou, doarPentruOptimizare, pointD, pointD2, thePoints[0].Y, i);
						panelObject.offsetMozaic = num8;
						currentPanel.panelingObjects.Add(panelObject);
						arrayList2.Add(panelObject);
						continue;
					}
					for (int num6 = 0; num6 < num5 - 2; num6++)
					{
						pointD = new PointD(((line)intersectingLines[i]).startPoint.X, y_min + (double)currentPanel.tipPanou.nrMaximOndule * currentPanel.tipPanou.pasOndula * (double)num6);
						pointD2 = new PointD(((line)intersectingLines[i]).startPoint.X + currentPanel.tipPanou.latimeFoaie, y_min + (double)currentPanel.tipPanou.nrMaximOndule * currentPanel.tipPanou.pasOndula * (double)(num6 + 1) + currentPanel.tipPanou.petrecereFoi);
						num8 = (pointD.Y - thePoints[0].Y) / currentPanel.tipPanou.pasOndula * currentPanel.tipPanou.offsetMozaic % currentPanel.tipPanou.latimeFoaie;
						pointD.X += num8;
						pointD2.X += num8;
						panelObject = new PanelObject(currentPanel.tipPanou, doarPentruOptimizare, pointD, pointD2, thePoints[0].Y, i);
						panelObject.offsetMozaic = num8;
						currentPanel.panelingObjects.Add(panelObject);
						arrayList2.Add(panelObject);
					}
					pointD = new PointD(((line)intersectingLines[i]).startPoint.X, y_min + (double)currentPanel.tipPanou.nrMaximOndule * currentPanel.tipPanou.pasOndula * (double)(num5 - 2));
					pointD2 = new PointD(((line)intersectingLines[i]).startPoint.X + currentPanel.tipPanou.latimeFoaie, y_min + (double)currentPanel.tipPanou.nrMaximOndule * currentPanel.tipPanou.pasOndula * (double)(num5 - 1) + currentPanel.tipPanou.petrecereFoi - ((currentPanel.tipPanou.nrMaximOndule > 2) ? currentPanel.tipPanou.pasOndula : 0.0));
					num8 = (pointD.Y - thePoints[0].Y) / currentPanel.tipPanou.pasOndula * currentPanel.tipPanou.offsetMozaic % currentPanel.tipPanou.latimeFoaie;
					pointD.X += num8;
					pointD2.X += num8;
					panelObject = new PanelObject(currentPanel.tipPanou, doarPentruOptimizare, pointD, pointD2, thePoints[0].Y, i);
					panelObject.offsetMozaic = num8;
					currentPanel.panelingObjects.Add(panelObject);
					arrayList2.Add(panelObject);
					if (y_max - (y_min + (double)currentPanel.tipPanou.nrMaximOndule * currentPanel.tipPanou.pasOndula * (double)(num5 - 1) - ((currentPanel.tipPanou.nrMaximOndule > 2) ? currentPanel.tipPanou.pasOndula : 0.0)) < (double)currentPanel.tipPanou.nrMinimOndule * currentPanel.tipPanou.pasOndula + currentPanel.tipPanou.petrecereFoi)
					{
						y_max = y_min + (double)currentPanel.tipPanou.nrMaximOndule * currentPanel.tipPanou.pasOndula * (double)(num5 - 1) - ((currentPanel.tipPanou.nrMaximOndule > 2) ? currentPanel.tipPanou.pasOndula : 0.0) + (double)currentPanel.tipPanou.nrMinimOndule * currentPanel.tipPanou.pasOndula + currentPanel.tipPanou.petrecereFoi;
					}
					pointD = new PointD(((line)intersectingLines[i]).startPoint.X, y_min + (double)currentPanel.tipPanou.nrMaximOndule * currentPanel.tipPanou.pasOndula * (double)(num5 - 1) - ((currentPanel.tipPanou.nrMaximOndule > 2) ? currentPanel.tipPanou.pasOndula : 0.0));
					pointD2 = new PointD(((line)intersectingLines[i]).startPoint.X + currentPanel.tipPanou.latimeFoaie, y_max);
					num8 = (pointD.Y - thePoints[0].Y) / currentPanel.tipPanou.pasOndula * currentPanel.tipPanou.offsetMozaic % currentPanel.tipPanou.latimeFoaie;
					pointD.X += num8;
					pointD2.X += num8;
					panelObject = new PanelObject(currentPanel.tipPanou, doarPentruOptimizare, pointD, pointD2, thePoints[0].Y, i);
					panelObject.offsetMozaic = num8;
					currentPanel.panelingObjects.Add(panelObject);
					arrayList2.Add(panelObject);
				}
				else
				{
					PointD pointD = new PointD(((line)intersectingLines[i]).startPoint.X, y_min);
					PointD pointD2 = new PointD(((line)intersectingLines[i]).startPoint.X + currentPanel.tipPanou.latimeFoaie, y_max);
					double num8 = (pointD.Y - thePoints[0].Y) / currentPanel.tipPanou.pasOndula * currentPanel.tipPanou.offsetMozaic % currentPanel.tipPanou.latimeFoaie;
					pointD.X += num8;
					pointD2.X += num8;
					PanelObject panelObject = new PanelObject(currentPanel.tipPanou, doarPentruOptimizare, pointD, pointD2, thePoints[0].Y, i);
					panelObject.offsetMozaic = num8;
					currentPanel.panelingObjects.Add(panelObject);
					arrayList2.Add(panelObject);
				}
			}
			catch (Exception ex)
			{
				string text = dictionar.dictionar[187] + ex.TargetSite.DeclaringType.Name + dictionar.dictionar[188] + ex.TargetSite.Name + dictionar.dictionar[189];
				MessageBox.Show(this, text);
			}
		}
		if (currentPanel.tipPanou.offsetMozaic == 0.0 || mainwindow.menuSettingsStandardPanels.Checked)
		{
			return;
		}
		Polygon polygon = new Polygon(currentPanel.realGraphicsPath);
		if (currentPanel.panelingObjects.Count <= 0)
		{
			return;
		}
		bool flag2 = false;
		bool flag3 = false;
		int numarColoana = ((PanelObject)currentPanel.panelingObjects[currentPanel.panelingObjects.Count - 1]).numarColoana;
		ArrayList arrayList3 = (ArrayList)currentPanel.panelingObjects.Clone();
		foreach (PanelObject item in arrayList3)
		{
			if (item.numarOndulaStart <= 0 || !(item.offsetMozaic > 0.0))
			{
				continue;
			}
			if (item.numarColoana == 0)
			{
				flag2 = true;
				flag3 = false;
			}
			else if (item.numarColoana == numarColoana)
			{
				flag2 = false;
				flag3 = true;
			}
			else
			{
				flag2 = true;
				flag3 = true;
			}
			bool flag4 = false;
			bool flag5 = false;
			foreach (PanelObject item2 in arrayList3)
			{
				if (item.numarOndulaStart == item2.numarOndulaStart)
				{
					if (flag2 && item.numarColoana == item2.numarColoana + 1)
					{
						flag4 = true;
					}
					if (flag3 && item.numarColoana == item2.numarColoana - 1)
					{
						flag5 = true;
					}
					if (flag3 == flag5 && flag2 == flag4)
					{
						break;
					}
				}
			}
			if (flag2 && !flag4)
			{
				PointD pointD3 = (PointD)item.startPoint.Clone();
				PointD pointD4 = (PointD)item.stopPoint.Clone();
				pointD3.X -= currentPanel.tipPanou.latimeFoaie;
				pointD4.X -= currentPanel.tipPanou.latimeFoaie;
				PanelObject panelObject4 = (PanelObject)item.Clone(pointD3, pointD4);
				panelObject4.numarColoana = item.numarColoana - 1;
				Polygon polygon2 = new Polygon(panelObject4.realGraphicsPath);
				Polygon polygon3 = polygon2.Clip(GpcOperation.Intersection, polygon);
				if (polygon3.NofContours > 0)
				{
					currentPanel.panelingObjects.Add(panelObject4);
				}
			}
			if (flag3 && !flag5)
			{
				Polygon polygon4 = new Polygon(item.realGraphicsPath);
				Polygon polygon5 = polygon4.Clip(GpcOperation.Intersection, polygon);
				if (polygon5.NofContours == 0)
				{
					currentPanel.panelingObjects.Remove(item);
				}
			}
		}
	}

	public void CuttingOptimization()
	{
		SheetTypeChoice sheetTypeChoice = new SheetTypeChoice();
		sheetTypeChoice.Text = dictionar.dictionar[298];
		sheetTypeChoice.label1.Text = dictionar.dictionar[303];
		ArrayList arrayList = new ArrayList();
		int num = 0;
		foreach (string item in tipuriDeInchidere)
		{
			Button button = new Button();
			button.Text = item;
			button.Width = 195;
			button.Height = 20;
			button.Left = 10;
			button.Top = 30 + num * 20;
			button.Click += sheetTypeChoice.btn_Click;
			button.DialogResult = DialogResult.OK;
			arrayList.Add(button);
			sheetTypeChoice.Controls.Add(button);
			num++;
		}
		sheetTypeChoice.Height = 50 + num * 20 + 20;
		if (sheetTypeChoice.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		int index = arrayList.IndexOf(sheetTypeChoice.butonulApasat);
		DataRow dataRow = dtab2.Rows[index];
		currentPanel.tipPanou.variatorPasOndula = bool.Parse(dataRow["variatorPasOndula"].ToString());
		currentPanel.tipPanou.streasina = double.Parse(dataRow["offsetStreasina"].ToString()) / 10.0;
		currentPanel.tipPanou.pasOndula = double.Parse(dataRow["pasOndula"].ToString()) / 10.0;
		currentPanel.tipPanou.latimeFoaie = double.Parse(dataRow["latimeFoaie"].ToString()) / 10.0;
		currentPanel.tipPanou.nrMaximOndule = int.Parse(dataRow["nrMaximOndule"].ToString());
		currentPanel.tipPanou.petrecereFoi = double.Parse(dataRow["petrecereFoi"].ToString()) / 10.0;
		currentPanel.tipPanou.tipTigla = dataRow["tipPanou"].ToString();
		currentPanel.tipPanou.optimizareStandard = bool.Parse(dataRow["dimensiuniStandard"].ToString());
		currentPanel.tipPanou.optimizareAjustabile = bool.Parse(dataRow["dimensiuniAjustabile"].ToString());
		currentPanel.tipPanou.nrMinimOndule = int.Parse(dataRow["nrMinimOndule"].ToString());
		currentPanel.tipPanou.offsetMozaic = double.Parse(dataRow["offsetMozaic"].ToString()) / 10.0;
		currentPanel.tipPanou.observatii = dataRow["observatii"].ToString();
		currentPanel.lungimeMinimaFoaie = (double)currentPanel.tipPanou.nrMinimOndule * currentPanel.tipPanou.pasOndula + currentPanel.tipPanou.petrecereFoi;
		currentPanel.lungimeMinimaFoaieModulRandom = (double)currentPanel.tipPanou.nrMinimOndule * currentPanel.tipPanou.pasOndula + currentPanel.tipPanou.petrecereFoi;
		currentPanel.lungimeMaximaFoaie = (double)currentPanel.tipPanou.nrMaximOndule * currentPanel.tipPanou.pasOndula + currentPanel.tipPanou.petrecereFoi;
		if (!dataRow.IsNull("decalajVecin"))
		{
			currentPanel.tipPanou.decalajVecin = double.Parse(dataRow["decalajVecin"].ToString()) / 10.0;
		}
		if (currentPanel.tipPanou.optimizareStandard && !currentPanel.tipPanou.optimizareAjustabile && !mainwindow.menuSettingsStandardPanels.Checked)
		{
			string text2 = dictionar.dictionar[316] + dictionar.dictionar[317] + dictionar.dictionar[319] + dictionar.dictionar[317];
			MessageBox.Show(this, text2);
			mainwindow.menuSettingsStandardPanels.Checked = true;
		}
		else if (!currentPanel.tipPanou.optimizareStandard && currentPanel.tipPanou.optimizareAjustabile && mainwindow.menuSettingsStandardPanels.Checked)
		{
			if (currentPanel.tipPanou.nrMaximOndule - currentPanel.tipPanou.nrMinimOndule != 0)
			{
				string text3 = dictionar.dictionar[316] + dictionar.dictionar[318] + dictionar.dictionar[319] + dictionar.dictionar[318];
				MessageBox.Show(this, text3);
			}
			mainwindow.menuSettingsStandardPanels.Checked = false;
		}
		if (currentPanel.tipPanou.observatii != "")
		{
			string text4 = Wrap(currentPanel.tipPanou.observatii, 100);
			MessageBox.Show(this, text4);
		}
		if (!currentPanel.tipPanou.variatorPasOndula && currentPanel.tipPanou.optimizareStandard)
		{
			oleCommand.CommandText = "SELECT idPanou, ondulePerPanou FROM [" + currentPanel.tipPanou.tipTigla + "] WHERE (idPanou IS NOT NULL) AND (idPanou > 0)";
			objCommand.SelectCommand = oleCommand;
			dtab3.Reset();
			objConnection.Open();
			objCommand.Fill(dtab3);
			objConnection.Close();
			oleCommand.CommandText = "SELECT * FROM [" + currentPanel.tipPanou.tipTigla + "]";
			objCommand.SelectCommand = oleCommand;
			dtab4.Clear();
			objConnection.Open();
			objCommand.Fill(dtab4);
			objConnection.Close();
		}
		else if (currentPanel.tipPanou.variatorPasOndula)
		{
			PasOndula pasOndula = new PasOndula();
			pasOndula.Text = dictionar.dictionar[313];
			pasOndula.label1.Text = dictionar.dictionar[314];
			if (pasOndula.ShowDialog() == DialogResult.OK)
			{
				currentPanel.tipPanou.pasOndula = int.Parse(pasOndula.textBox1.Text) / 10;
				currentPanel.tipPanou.nrMaximOndule = (int)Math.Floor(currentPanel.lungimeMaximaFoaie / currentPanel.tipPanou.pasOndula);
			}
		}
		PointD[] array = GiveMeTheBoundingBox();
		ArrayList arrayList2 = new ArrayList();
		doarPentruOptimizare = true;
		int i;
		for (i = 0; i < (int)currentPanel.tipPanou.latimeFoaie; i++)
		{
			Cutt(array);
			if (i % 5 == 0)
			{
				RedrawAll();
			}
			double num2 = 0.0;
			foreach (PanelObject panelingObject in currentPanel.panelingObjects)
			{
				num2 += panelingObject.height;
			}
			arrayList2.Add(num2);
			array[0].X -= 1.0;
		}
		doarPentruOptimizare = false;
		array[0].X += (int)currentPanel.tipPanou.latimeFoaie;
		i = 0;
		int num3 = 0;
		double num4 = -1.0;
		foreach (double item2 in arrayList2)
		{
			if (num4 == -1.0)
			{
				num4 = item2;
			}
			else if (num4 >= item2)
			{
				num4 = item2;
				i = num3;
			}
			num3++;
		}
		array[0].X -= i;
		Cutt(array);
		if (currentPanel.panelingObjects.Count == 0)
		{
			((Document)base.ParentForm).currentSheetsArea.ResetText();
			((Document)base.ParentForm).currentSheetsArea.Enabled = false;
		}
		else
		{
			((Document)base.ParentForm).currentSheetsArea.Enabled = true;
			double num6 = 0.0;
			foreach (PanelObject panelingObject2 in currentPanel.panelingObjects)
			{
				num6 += panelingObject2.panelArea;
			}
			((Document)base.ParentForm).currentSheetsArea.Text = (num6 / 10000.0).ToString("N3") + dictionar.dictionar[173];
		}
		RedrawAll();
	}

	public void ManualCuttingOptimization(PointD aPoint)
	{
		PointD[] array = GiveMeTheBoundingBox();
		array[0].X = aPoint.X - Math.Ceiling((aPoint.X - array[0].X) / currentPanel.tipPanou.latimeFoaie) * currentPanel.tipPanou.latimeFoaie;
		Cutt(array);
		RedrawAll();
	}

	public PointD[] GiveMeTheBoundingBox()
	{
		PointD pointD = new PointD();
		PointD pointD2 = new PointD();
		PointD[] result = new PointD[2] { pointD, pointD2 };
		if (currentPanel != null && currentPanel.objects.Count != 0)
		{
			bool flag = true;
			foreach (GraphicObject @object in currentPanel.objects)
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
			if ((double)(base.Size.Width - 110) * scale / (pointD2.X - pointD.X) <= (double)(base.Size.Height - 50) * scale / (pointD2.Y - pointD.Y))
			{
				scale /= (double)(base.Size.Width - 110) * scale / (pointD2.X - pointD.X);
				base_point_x = pointD.X - 55.0 * scale + (double)(bm.Width / 3) * scale * -1.0;
				base_point_y = pointD2.Y + ((double)(base.Size.Height - 50) * scale - (pointD2.Y - pointD.Y)) / 2.0 + 25.0 * scale + (double)(bm.Height / 3) * scale;
			}
			else
			{
				scale /= (double)(base.Size.Height - 50) * scale / (pointD2.Y - pointD.Y);
				base_point_x = pointD.X - ((double)(base.Size.Width - 110) * scale - (pointD2.X - pointD.X)) / 2.0 - 55.0 * scale + (double)(bm.Width / 3) * scale * -1.0;
				base_point_y = pointD2.Y + 25.0 * scale + (double)(bm.Height / 3) * scale;
			}
			RedrawAll();
		}
		return result;
	}

	public void ZoomExtents()
	{
		if (currentPanel == null || currentPanel.objects.Count == 0)
		{
			return;
		}
		PointD pointD = new PointD();
		PointD pointD2 = new PointD();
		bool flag = true;
		foreach (GraphicObject @object in currentPanel.objects)
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
		if ((double)(base.Size.Width - 110) * scale / (pointD2.X - pointD.X) <= (double)(base.Size.Height - 50) * scale / (pointD2.Y - pointD.Y))
		{
			scale /= (double)(base.Size.Width - 110) * scale / (pointD2.X - pointD.X);
			base_point_x = pointD.X - 55.0 * scale + (double)(bm.Width / 3) * scale * -1.0;
			base_point_y = pointD2.Y + ((double)(base.Size.Height - 50) * scale - (pointD2.Y - pointD.Y)) / 2.0 + 25.0 * scale + (double)(bm.Height / 3) * scale;
		}
		else
		{
			scale /= (double)(base.Size.Height - 50) * scale / (pointD2.Y - pointD.Y);
			base_point_x = pointD.X - ((double)(base.Size.Width - 110) * scale - (pointD2.X - pointD.X)) / 2.0 - 55.0 * scale + (double)(bm.Width / 3) * scale * -1.0;
			base_point_y = pointD2.Y + 25.0 * scale + (double)(bm.Height / 3) * scale;
		}
		RedrawAll();
	}

	public void MakeReport()
	{
		ArrayList arrayList = new ArrayList();
		ArrayList arrayList2 = new ArrayList();
		ArrayList arrayList3 = new ArrayList();
		arrayList3.Add(new EdgeType("Streasina"));
		arrayList3.Add(new EdgeType("Coama"));
		arrayList3.Add(new EdgeType("Fronton"));
		arrayList3.Add(new EdgeType("Dolie"));
		arrayList3.Add(new EdgeType("BorduraCalcan"));
		arrayList3.Add(new EdgeType("CoamaInclinata"));
		arrayList3.Add(new EdgeType("SemiCoama"));
		arrayList3.Add(new EdgeType("FrontonInclinat"));
		arrayList3.Add(new EdgeType("RuperePantaCcv"));
		arrayList3.Add(new EdgeType("RuperePantaCvx"));
		arrayList3.Add(new EdgeType("RacordLateral"));
		arrayList3.Add(new EdgeType("FrontonEvazat"));
		arrayList3.Add(new EdgeType("Default"));
		int num = 0;
		if (panels.Count != 0)
		{
			foreach (Panel panel4 in panels)
			{
				if (panel4.panelingObjects.Count == 0)
				{
					continue;
				}
				PanelObject panelObject = null;
				foreach (PanelObject panelingObject in panel4.panelingObjects)
				{
					arrayList.Add(new PanelType(panelingObject, 1));
					if (panelObject != null && Math.Round(panelingObject.startPoint.X, 3) == Math.Round(panelObject.startPoint.X, 3))
					{
						double num2 = Math.Round(panelObject.stopPoint.Y - panelingObject.startPoint.Y, 3);
						if (num2 <= panelObject.petrecereFoi && num2 >= 0.0)
						{
							num++;
						}
					}
					panelObject = panelingObject;
				}
			}
		}
		for (int num3 = arrayList.Count - 1; num3 >= 0; num3--)
		{
			foreach (PanelType item in arrayList)
			{
				if (((PanelType)arrayList[num3]).po != item.po && ((PanelType)arrayList[num3]).po.height == item.po.height)
				{
					item.count++;
					arrayList.RemoveAt(num3);
					break;
				}
			}
		}
		foreach (Panel panel5 in panels)
		{
			if (!panel5.evolved)
			{
				continue;
			}
			foreach (GraphicObject @object in panel5.objects)
			{
				arrayList2.Add(@object);
			}
		}
		arrayList2.Sort();
		new ArrayList();
		for (int num3 = arrayList2.Count - 1; num3 >= 1; num3--)
		{
			if (((GraphicObject)arrayList2[num3]).objectIndex == ((GraphicObject)arrayList2[num3 - 1]).objectIndex)
			{
				if (((GraphicObject)arrayList2[num3]).layer.nameOfLayer == ((GraphicObject)arrayList2[num3 - 1]).layer.nameOfLayer)
				{
					if (((GraphicObject)arrayList2[num3]).length > ((GraphicObject)arrayList2[num3 - 1]).length)
					{
						arrayList2.RemoveAt(num3 - 1);
					}
					else
					{
						arrayList2.RemoveAt(num3);
					}
				}
				else if (((GraphicObject)arrayList2[num3]).length > ((GraphicObject)arrayList2[num3 - 1]).length)
				{
					arrayList2.RemoveAt(num3 - 1);
				}
				else
				{
					arrayList2.RemoveAt(num3);
				}
			}
		}
		foreach (GraphicObject item2 in arrayList2)
		{
			foreach (EdgeType item3 in arrayList3)
			{
				if (item3.name == item2.layer.nameOfLayer)
				{
					item3.count++;
					item3.length += item2.length;
					item3.elemente.Add(item2);
					break;
				}
			}
		}
		ArrayList arrayList4 = new ArrayList();
		ArrayList arrayList5 = new ArrayList();
		ArrayList arrayList6 = new ArrayList();
		ArrayList arrayList7 = new ArrayList();
		foreach (GraphicObject object2 in ((Document)base.ParentForm).graphicTable.objects)
		{
			foreach (GraphicObject item4 in arrayList2)
			{
				if (item4.objectIndex == object2.objectIndex)
				{
					if (object2.layer.nameOfLayer == "Coama" || object2.layer.nameOfLayer == "CoamaInclinata" || object2.layer.nameOfLayer == "SemiCoama")
					{
						arrayList4.Add(object2.startPoint);
						arrayList4.Add(object2.stopPoint);
					}
					else if (object2.layer.nameOfLayer != "Dolie")
					{
						arrayList7.Add(object2.startPoint);
						arrayList7.Add(object2.stopPoint);
					}
					if (object2.layer.nameOfLayer == "Streasina")
					{
						arrayList5.Add(object2.startPoint);
						arrayList5.Add(object2.stopPoint);
					}
					else if (object2.layer.nameOfLayer == "Dolie")
					{
						arrayList6.Add(object2.startPoint);
						arrayList6.Add(object2.stopPoint);
					}
					break;
				}
			}
		}
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		if (arrayList4.Count > 0 && arrayList7.Count > 0)
		{
			foreach (PointD item5 in arrayList4)
			{
				foreach (PointD item6 in arrayList7)
				{
					if (item5.X == item6.X && item5.Y == item6.Y)
					{
						num4++;
						break;
					}
				}
			}
		}
		foreach (PointD item7 in arrayList5)
		{
			foreach (PointD item8 in arrayList5)
			{
				if (arrayList5.IndexOf(item8) <= arrayList5.IndexOf(item7) || item7 == item8 || item7.X != item8.X || item7.Y != item8.Y)
				{
					continue;
				}
				sbyte b = -1;
				sbyte b2 = -1;
				if (Math.Round((float)arrayList5.IndexOf(item7) / 2f, 0) == (double)((float)arrayList5.IndexOf(item7) / 2f))
				{
					b = 1;
				}
				if (Math.Round((float)arrayList5.IndexOf(item8) / 2f, 0) == (double)((float)arrayList5.IndexOf(item8) / 2f))
				{
					b2 = 1;
				}
				PointD pointD5 = (PointD)arrayList5[arrayList5.IndexOf(item7) + b];
				PointD pointD6 = (PointD)arrayList5[arrayList5.IndexOf(item8) + b2];
				double num7 = Math.Pow(pointD5.X - pointD6.X, 2.0) + Math.Pow(pointD5.Y - pointD6.Y, 2.0);
				double num8 = Math.Pow(item7.X - pointD5.X, 2.0) + Math.Pow(item7.Y - pointD5.Y, 2.0) + Math.Pow(item7.X - pointD6.X, 2.0) + Math.Pow(item7.Y - pointD6.Y, 2.0);
				if (!(Math.Abs(num7 - num8) < 0.01))
				{
					continue;
				}
				bool flag = false;
				foreach (PointD item9 in arrayList6)
				{
					if (item9.X == item7.X && item9.Y == item7.Y)
					{
						num6++;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					num5++;
				}
				break;
			}
		}
		arrayList.Sort();
		reportThis = "";
		reportThis = dictionar.dictionar[200];
		double num9 = 0.0;
		foreach (Panel panel6 in panels)
		{
			string text = reportThis;
			reportThis = text + "\r\n             " + panel6.panelName + " Arie sarpanta: " + (panel6.panelArea / 10000.0).ToString("N2") + dictionar.dictionar[203];
			num9 += Math.Round(panel6.panelArea / 10000.0, 2) * 10000.0;
		}
		reportThis = reportThis + "\r\n             Arie totala sarpanta: " + (num9 / 10000.0).ToString("N2") + dictionar.dictionar[203];
		reportThis += "\r\n***************************";
		reportThis += "\r\n";
		reportThis += "\r\n";
		double num10 = 0.0;
		foreach (PanelType item10 in arrayList)
		{
			num10 += item10.po.panelArea * (double)item10.count;
		}
		reportThis = reportThis + dictionar.dictionar[202] + num10 / 10000.0 + dictionar.dictionar[203];
		reportThis += "\r\n";
		reportThis += dictionar.dictionar[204];
		reportThis = reportThis + dictionar.dictionar[205] + (((EdgeType)arrayList3[0]).length / 100.0).ToString("N") + dictionar.dictionar[206];
		foreach (GraphicObject item11 in ((EdgeType)arrayList3[0]).elemente)
		{
			string text = reportThis;
			reportThis = text + "\r\n                             " + ((EdgeType)arrayList3[0]).name + " #" + (((EdgeType)arrayList3[0]).elemente.IndexOf(item11) + 1) + " L=" + (item11.length / 100.0).ToString("N2") + dictionar.dictionar[206];
		}
		reportThis = reportThis + dictionar.dictionar[207] + (((EdgeType)arrayList3[1]).length / 100.0).ToString("N") + dictionar.dictionar[206];
		foreach (GraphicObject item12 in ((EdgeType)arrayList3[1]).elemente)
		{
			string text = reportThis;
			reportThis = text + "\r\n                             " + ((EdgeType)arrayList3[1]).name + " #" + (((EdgeType)arrayList3[1]).elemente.IndexOf(item12) + 1) + " L=" + (item12.length / 100.0).ToString("N2") + dictionar.dictionar[206];
		}
		reportThis = reportThis + dictionar.dictionar[208] + (((EdgeType)arrayList3[2]).length / 100.0).ToString("N") + dictionar.dictionar[206];
		foreach (GraphicObject item13 in ((EdgeType)arrayList3[2]).elemente)
		{
			string text = reportThis;
			reportThis = text + "\r\n                             " + ((EdgeType)arrayList3[2]).name + " #" + (((EdgeType)arrayList3[2]).elemente.IndexOf(item13) + 1) + " L=" + (item13.length / 100.0).ToString("N2") + dictionar.dictionar[206];
		}
		reportThis = reportThis + dictionar.dictionar[209] + (((EdgeType)arrayList3[7]).length / 100.0).ToString("N") + dictionar.dictionar[206];
		foreach (GraphicObject item14 in ((EdgeType)arrayList3[7]).elemente)
		{
			string text = reportThis;
			reportThis = text + "\r\n                             " + ((EdgeType)arrayList3[7]).name + " #" + (((EdgeType)arrayList3[7]).elemente.IndexOf(item14) + 1) + " L=" + (item14.length / 100.0).ToString("N2") + dictionar.dictionar[206];
		}
		reportThis = reportThis + dictionar.dictionar[210] + (((EdgeType)arrayList3[3]).length / 100.0).ToString("N") + dictionar.dictionar[206];
		foreach (GraphicObject item15 in ((EdgeType)arrayList3[3]).elemente)
		{
			string text = reportThis;
			reportThis = text + "\r\n                             " + ((EdgeType)arrayList3[3]).name + " #" + (((EdgeType)arrayList3[3]).elemente.IndexOf(item15) + 1) + " L=" + (item15.length / 100.0).ToString("N2") + dictionar.dictionar[206];
		}
		reportThis = reportThis + dictionar.dictionar[211] + (((EdgeType)arrayList3[4]).length / 100.0).ToString("N") + dictionar.dictionar[206];
		foreach (GraphicObject item16 in ((EdgeType)arrayList3[4]).elemente)
		{
			string text = reportThis;
			reportThis = text + "\r\n                             " + ((EdgeType)arrayList3[4]).name + " #" + (((EdgeType)arrayList3[4]).elemente.IndexOf(item16) + 1) + " L=" + (item16.length / 100.0).ToString("N2") + dictionar.dictionar[206];
		}
		reportThis = reportThis + dictionar.dictionar[212] + (((EdgeType)arrayList3[5]).length / 100.0).ToString("N") + dictionar.dictionar[206];
		foreach (GraphicObject item17 in ((EdgeType)arrayList3[5]).elemente)
		{
			string text = reportThis;
			reportThis = text + "\r\n                             " + ((EdgeType)arrayList3[5]).name + " #" + (((EdgeType)arrayList3[5]).elemente.IndexOf(item17) + 1) + " L=" + (item17.length / 100.0).ToString("N2") + dictionar.dictionar[206];
		}
		reportThis = reportThis + dictionar.dictionar[213] + (((EdgeType)arrayList3[6]).length / 100.0).ToString("N") + dictionar.dictionar[206];
		foreach (GraphicObject item18 in ((EdgeType)arrayList3[6]).elemente)
		{
			string text = reportThis;
			reportThis = text + "\r\n                             " + ((EdgeType)arrayList3[6]).name + " #" + (((EdgeType)arrayList3[6]).elemente.IndexOf(item18) + 1) + " L=" + (item18.length / 100.0).ToString("N2") + dictionar.dictionar[206];
		}
		reportThis = reportThis + dictionar.dictionar[214] + (((EdgeType)arrayList3[8]).length / 100.0).ToString("N") + dictionar.dictionar[206];
		foreach (GraphicObject item19 in ((EdgeType)arrayList3[8]).elemente)
		{
			string text = reportThis;
			reportThis = text + "\r\n                             " + ((EdgeType)arrayList3[8]).name + " #" + (((EdgeType)arrayList3[8]).elemente.IndexOf(item19) + 1) + " L=" + (item19.length / 100.0).ToString("N2") + dictionar.dictionar[206];
		}
		reportThis = reportThis + dictionar.dictionar[336] + (((EdgeType)arrayList3[9]).length / 100.0).ToString("N") + dictionar.dictionar[206];
		foreach (GraphicObject item20 in ((EdgeType)arrayList3[9]).elemente)
		{
			string text = reportThis;
			reportThis = text + "\r\n                             " + ((EdgeType)arrayList3[9]).name + " #" + (((EdgeType)arrayList3[9]).elemente.IndexOf(item20) + 1) + " L=" + (item20.length / 100.0).ToString("N2") + dictionar.dictionar[206];
		}
		reportThis = reportThis + dictionar.dictionar[337] + (((EdgeType)arrayList3[10]).length / 100.0).ToString("N") + dictionar.dictionar[206];
		foreach (GraphicObject item21 in ((EdgeType)arrayList3[10]).elemente)
		{
			string text = reportThis;
			reportThis = text + "\r\n                             " + ((EdgeType)arrayList3[10]).name + " #" + (((EdgeType)arrayList3[10]).elemente.IndexOf(item21) + 1) + " L=" + (item21.length / 100.0).ToString("N2") + dictionar.dictionar[206];
		}
		reportThis = reportThis + dictionar.dictionar[338] + (((EdgeType)arrayList3[11]).length / 100.0).ToString("N") + dictionar.dictionar[206];
		foreach (GraphicObject item22 in ((EdgeType)arrayList3[11]).elemente)
		{
			string text = reportThis;
			reportThis = text + "\r\n                             " + ((EdgeType)arrayList3[11]).name + " #" + (((EdgeType)arrayList3[11]).elemente.IndexOf(item22) + 1) + " L=" + (item22.length / 100.0).ToString("N2") + dictionar.dictionar[206];
		}
		reportThis = reportThis + dictionar.dictionar[215] + (((EdgeType)arrayList3[12]).length / 100.0).ToString("N") + dictionar.dictionar[206];
		foreach (GraphicObject item23 in ((EdgeType)arrayList3[12]).elemente)
		{
			string text = reportThis;
			reportThis = text + "\r\n                             " + ((EdgeType)arrayList3[12]).name + " #" + (((EdgeType)arrayList3[12]).elemente.IndexOf(item23) + 1) + " L=" + (item23.length / 100.0).ToString("N2") + dictionar.dictionar[206];
		}
		reportThis += "\r\n   Alte elemente:";
		reportThis = reportThis + "\r\n   Margini coama: " + arrayList4.Count + " buc";
		reportThis = reportThis + "\r\n   Margini streasina: " + arrayList5.Count + " buc";
		reportThis = reportThis + "\r\n   Margini dolie: " + arrayList6.Count + " buc";
		reportThis = reportThis + "\r\n   Colt exterior streasina-streasina: " + num5 + " buc";
		reportThis = reportThis + "\r\n   Colt interior streasina-streasina: " + num6 + " buc";
		reportThis = reportThis + "\r\n   Imbinari intre table: " + num + " buc";
		reportThis += "\r\n";
		reportThis += dictionar.dictionar[217];
		reportThis += "\r\n ----------------------";
		int num11 = (int)Math.Ceiling(((EdgeType)arrayList3[0]).length / 400.0);
		reportThis = reportThis + dictionar.dictionar[219] + num11 + dictionar.dictionar[218];
		int num12 = (int)Math.Ceiling(((EdgeType)arrayList3[0]).length / 80.0) + 2 * (num6 + num5) + ((EdgeType)arrayList3[0]).count;
		reportThis = reportThis + dictionar.dictionar[220] + num12 + dictionar.dictionar[218];
		int num13 = 0;
		if (num11 != 0)
		{
			num13 = num11 + 2 * (num6 + num5);
		}
		reportThis = reportThis + dictionar.dictionar[221] + num13 + dictionar.dictionar[218];
		int num14 = ((EdgeType)arrayList3[0]).count * 2 - 2 * (num6 + num5);
		reportThis = reportThis + dictionar.dictionar[222] + num14 + dictionar.dictionar[218];
		int num15 = (int)Math.Ceiling(((EdgeType)arrayList3[0]).length / 195.0);
		reportThis = reportThis + dictionar.dictionar[223] + num15 + dictionar.dictionar[218];
		int num16 = num6;
		reportThis = reportThis + dictionar.dictionar[225] + num16 + dictionar.dictionar[218];
		int num17 = num5;
		reportThis = reportThis + dictionar.dictionar[226] + num17 + dictionar.dictionar[218];
		int num18 = (int)Math.Ceiling(((EdgeType)arrayList3[0]).length / 100.0);
		reportThis += dictionar.dictionar[307];
		reportThis = reportThis + dictionar.dictionar[224] + num18 + dictionar.dictionar[218];
		reportThis += dictionar.dictionar[331];
		int num19 = (int)Math.Ceiling(((EdgeType)arrayList3[0]).length * 1.0 / 113.0);
		reportThis = reportThis + dictionar.dictionar[333] + num19 + dictionar.dictionar[218];
		reportThis += dictionar.dictionar[321];
		int num20 = (int)Math.Ceiling(((EdgeType)arrayList3[0]).length * 1.0 / 102.5);
		reportThis = reportThis + dictionar.dictionar[324] + num20 + dictionar.dictionar[218];
		int num21 = (int)Math.Ceiling(((EdgeType)arrayList3[0]).length * 1.0 / 90.0);
		reportThis = reportThis + dictionar.dictionar[325] + num21 + dictionar.dictionar[218];
		reportThis += "\r\n";
		reportThis += dictionar.dictionar[228];
		reportThis += "\r\n ------------------";
		int num22 = (int)Math.Ceiling(((EdgeType)arrayList3[1]).length / 200.0);
		reportThis = reportThis + dictionar.dictionar[229] + num22 + dictionar.dictionar[218];
		int num23 = 0;
		reportThis = reportThis + dictionar.dictionar[231] + num23 + dictionar.dictionar[218];
		reportThis += dictionar.dictionar[307];
		int num24 = (int)Math.Ceiling(((EdgeType)arrayList3[1]).length * 2.0 / 100.0);
		reportThis = reportThis + dictionar.dictionar[230] + num24 + dictionar.dictionar[218];
		reportThis += dictionar.dictionar[308];
		int num25 = (int)Math.Ceiling(((EdgeType)arrayList3[1]).length * 2.0 / 110.0);
		reportThis = reportThis + dictionar.dictionar[309] + num25 + dictionar.dictionar[218];
		reportThis += dictionar.dictionar[331];
		int num26 = (int)Math.Ceiling(((EdgeType)arrayList3[1]).length * 2.0 / 113.0);
		reportThis = reportThis + dictionar.dictionar[332] + num26 + dictionar.dictionar[218];
		reportThis += dictionar.dictionar[321];
		int num27 = (int)Math.Ceiling(((EdgeType)arrayList3[1]).length * 2.0 / 102.5);
		reportThis = reportThis + dictionar.dictionar[322] + num27 + dictionar.dictionar[218];
		int num28 = (int)Math.Ceiling(((EdgeType)arrayList3[1]).length * 2.0 / 90.0);
		reportThis = reportThis + dictionar.dictionar[323] + num28 + dictionar.dictionar[218];
		reportThis += "\r\n";
		reportThis += dictionar.dictionar[233];
		reportThis += "\r\n --------------------";
		int num29 = (int)Math.Ceiling(((EdgeType)arrayList3[2]).length / 195.0);
		reportThis = reportThis + dictionar.dictionar[234] + num29 + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[235] + num29 + dictionar.dictionar[218];
		reportThis += "\r\n";
		reportThis += dictionar.dictionar[237];
		reportThis += "\r\n --------------------";
		int num30 = (int)Math.Ceiling(((EdgeType)arrayList3[7]).length / 195.0);
		reportThis = reportThis + dictionar.dictionar[238] + num30 + dictionar.dictionar[218];
		reportThis += "\r\n";
		reportThis += dictionar.dictionar[240];
		reportThis += "\r\n ------------------";
		int num31 = (int)Math.Ceiling(((EdgeType)arrayList3[3]).length / 195.0);
		reportThis = reportThis + dictionar.dictionar[241] + num31 + dictionar.dictionar[218];
		reportThis += "\r\n";
		reportThis += "\r\n";
		reportThis += dictionar.dictionar[243];
		reportThis += "\r\n ------------------";
		int num32 = (int)Math.Ceiling(((EdgeType)arrayList3[4]).length / 195.0);
		reportThis = reportThis + dictionar.dictionar[244] + num32 + dictionar.dictionar[218];
		reportThis += dictionar.dictionar[307];
		int num33 = (int)Math.Ceiling(((EdgeType)arrayList3[4]).length / 100.0);
		reportThis = reportThis + dictionar.dictionar[245] + num33 + dictionar.dictionar[218];
		reportThis += dictionar.dictionar[308];
		int num34 = (int)Math.Ceiling(((EdgeType)arrayList3[4]).length / 110.0);
		reportThis = reportThis + dictionar.dictionar[309] + num34 + dictionar.dictionar[218];
		reportThis += dictionar.dictionar[331];
		int num35 = (int)Math.Ceiling(((EdgeType)arrayList3[4]).length * 1.0 / 113.0);
		reportThis = reportThis + dictionar.dictionar[332] + num35 + dictionar.dictionar[218];
		reportThis += dictionar.dictionar[321];
		int num36 = (int)Math.Ceiling(((EdgeType)arrayList3[4]).length * 1.0 / 102.5);
		reportThis = reportThis + dictionar.dictionar[322] + num36 + dictionar.dictionar[218];
		int num37 = (int)Math.Ceiling(((EdgeType)arrayList3[4]).length * 1.0 / 90.0);
		reportThis = reportThis + dictionar.dictionar[323] + num37 + dictionar.dictionar[218];
		reportThis += "\r\n";
		reportThis += "\r\n";
		reportThis += dictionar.dictionar[247];
		reportThis += "\r\n ------------------";
		int num38 = (int)Math.Ceiling(((EdgeType)arrayList3[5]).length / 200.0);
		reportThis = reportThis + dictionar.dictionar[248] + num38 + dictionar.dictionar[218];
		int num39 = (int)Math.Ceiling(((EdgeType)arrayList3[5]).length / 1000.0);
		reportThis = reportThis + dictionar.dictionar[249] + num39 + dictionar.dictionar[218];
		reportThis += "\r\n";
		reportThis += "\r\n";
		reportThis += dictionar.dictionar[251];
		reportThis += "\r\n ------------------";
		int num40 = (int)Math.Ceiling(((EdgeType)arrayList3[6]).length / 195.0);
		reportThis = reportThis + dictionar.dictionar[252] + num40 + dictionar.dictionar[218];
		reportThis += dictionar.dictionar[307];
		int num41 = (int)Math.Ceiling(((EdgeType)arrayList3[6]).length / 100.0);
		reportThis = reportThis + dictionar.dictionar[253] + num41 + dictionar.dictionar[218];
		reportThis += dictionar.dictionar[308];
		int num42 = (int)Math.Ceiling(((EdgeType)arrayList3[6]).length / 110.0);
		reportThis = reportThis + dictionar.dictionar[309] + num42 + dictionar.dictionar[218];
		reportThis += dictionar.dictionar[331];
		int num43 = (int)Math.Ceiling(((EdgeType)arrayList3[6]).length * 1.0 / 113.0);
		reportThis = reportThis + dictionar.dictionar[332] + num43 + dictionar.dictionar[218];
		reportThis += dictionar.dictionar[321];
		int num44 = (int)Math.Ceiling(((EdgeType)arrayList3[6]).length * 1.0 / 102.5);
		reportThis = reportThis + dictionar.dictionar[322] + num44 + dictionar.dictionar[218];
		int num45 = (int)Math.Ceiling(((EdgeType)arrayList3[6]).length * 1.0 / 90.0);
		reportThis = reportThis + dictionar.dictionar[323] + num45 + dictionar.dictionar[218];
		reportThis += "\r\n";
		reportThis += "\r\n";
		reportThis += dictionar.dictionar[255];
		reportThis += "\r\n ------------------";
		int num46 = (int)Math.Ceiling(((EdgeType)arrayList3[8]).length / 195.0);
		reportThis = reportThis + dictionar.dictionar[256] + num46 + dictionar.dictionar[218];
		reportThis += dictionar.dictionar[307];
		int num47 = (int)Math.Ceiling(((EdgeType)arrayList3[8]).length / 100.0);
		reportThis = reportThis + dictionar.dictionar[257] + num47 + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[258] + num47 + dictionar.dictionar[218];
		reportThis += dictionar.dictionar[308];
		int num48 = (int)Math.Ceiling(((EdgeType)arrayList3[8]).length * 1.0 / 110.0);
		reportThis = reportThis + dictionar.dictionar[309] + num48 + dictionar.dictionar[218];
		int num49 = (int)Math.Ceiling(((EdgeType)arrayList3[8]).length * 1.0 / 110.0);
		reportThis = reportThis + dictionar.dictionar[310] + num49 + dictionar.dictionar[218];
		reportThis += dictionar.dictionar[331];
		int num50 = (int)Math.Ceiling(((EdgeType)arrayList3[8]).length * 1.0 / 113.0);
		reportThis = reportThis + dictionar.dictionar[332] + num50 + dictionar.dictionar[218];
		reportThis += dictionar.dictionar[321];
		int num51 = (int)Math.Ceiling(((EdgeType)arrayList3[8]).length * 1.0 / 102.5);
		reportThis = reportThis + dictionar.dictionar[322] + num51 + dictionar.dictionar[218];
		int num52 = (int)Math.Ceiling(((EdgeType)arrayList3[8]).length * 1.0 / 90.0);
		reportThis = reportThis + dictionar.dictionar[323] + num52 + dictionar.dictionar[218];
		int num53 = (int)Math.Ceiling(((EdgeType)arrayList3[8]).length * 1.0 / 102.5);
		reportThis = reportThis + dictionar.dictionar[324] + num53 + dictionar.dictionar[218];
		int num54 = (int)Math.Ceiling(((EdgeType)arrayList3[8]).length * 1.0 / 90.0);
		reportThis = reportThis + dictionar.dictionar[325] + num54 + dictionar.dictionar[218];
		reportThis += "\r\n";
		reportThis += "\r\n";
		reportThis += dictionar.dictionar[260];
		reportThis += "\r\n ----------";
		int num55 = (int)Math.Ceiling(num10 / (((MainWindow)((Document)base.ParentForm).ParentForm).LAFarea * 10000.0));
		reportThis = reportThis + dictionar.dictionar[261] + num55 + dictionar.dictionar[218];
		reportThis += "\r\n";
		reportThis += dictionar.dictionar[263];
		reportThis += "\r\n--------------------------------------- ";
		reportThis += dictionar.dictionar[264];
		reportThis += "\r\n--------------------------------------- ";
		foreach (PanelType item24 in arrayList)
		{
			string text2 = "";
			string text3 = "";
			switch (item24.po.tipTigla)
			{
			case "Nordic400":
				text2 = dictionar.dictionar[265];
				text3 = dictionar.dictionar[266];
				break;
			case "Skane":
				text2 = dictionar.dictionar[301];
				text3 = dictionar.dictionar[302];
				break;
			default:
				text2 = "\r\n";
				text3 = "\r\n";
				text2 += item24.po.tipTigla;
				text3 += item24.po.tipTigla;
				while (text2.Length < 13)
				{
					text2 += " ";
					text3 += " ";
				}
				text2 += "L= ";
				text3 += "L=";
				break;
			}
			if (item24.po.height < 100.0)
			{
				string text = reportThis;
				reportThis = text + text2 + item24.po.height * 10.0 + dictionar.dictionar[267] + item24.count;
			}
			else
			{
				string text = reportThis;
				reportThis = text + text3 + item24.po.height * 10.0 + dictionar.dictionar[267] + item24.count;
			}
		}
		reportThis += "\r\n";
		reportThis += dictionar.dictionar[269];
		reportThis += "\r\n--------------------------------------- ";
		reportThis = reportThis + dictionar.dictionar[270] + num11 + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[271] + num12 + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[272] + num13 + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[273] + num14 + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[274] + num16 + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[275] + num17 + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[276] + num15 + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[278] + (int)Math.Ceiling((((EdgeType)arrayList3[5]).length + ((EdgeType)arrayList3[1]).length) / 195.0) + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[281] + num23 + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[282] + num29 + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[283] + num29 + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[284] + num30 + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[285] + num31 + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[286] + num32 + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[287] + num39 + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[288] + num40 + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[289] + num46 + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[290] + num55 + dictionar.dictionar[218];
		reportThis += dictionar.dictionar[305];
		reportThis = reportThis + dictionar.dictionar[279] + (num24 + num33 + num41 + num47) + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[280] + num47 + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[277] + num18 + dictionar.dictionar[218];
		reportThis += dictionar.dictionar[306];
		reportThis = reportThis + dictionar.dictionar[311] + (num25 + num34 + num42 + num48) + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[312] + num49 + dictionar.dictionar[218];
		reportThis += dictionar.dictionar[320];
		reportThis = reportThis + dictionar.dictionar[326] + (num27 + num36 + num44 + num51) + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[327] + (num28 + num37 + num45 + num52) + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[328] + (num20 + num53) + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[329] + (num21 + num54) + dictionar.dictionar[218];
		reportThis += dictionar.dictionar[330];
		reportThis = reportThis + dictionar.dictionar[334] + (num26 + num35 + num43 + num50) + dictionar.dictionar[218];
		reportThis = reportThis + dictionar.dictionar[335] + num19 + dictionar.dictionar[218];
		((Document)base.ParentForm).saveFileDialog1.DefaultExt = dictionar.dictionar[293];
		((Document)base.ParentForm).saveFileDialog1.FileName = dictionar.dictionar[294];
		((Document)base.ParentForm).saveFileDialog1.ShowDialog();
	}

	public static string Wrap(string text, int maxLength)
	{
		if (text.Length == 0)
		{
			return "";
		}
		string[] array = text.Split(' ');
		List<string> list = new List<string>();
		string text2 = "";
		string[] array2 = array;
		foreach (string text3 in array2)
		{
			if (text2.Length > maxLength || text2.Length + text3.Length > maxLength)
			{
				list.Add(text2 + "\r\n");
				text2 = "";
			}
			text2 = ((text2.Length <= 0) ? (text2 + text3) : (text2 + " " + text3));
		}
		if (text2.Length > 0)
		{
			list.Add(text2 + "\r\n");
		}
		return string.Join(string.Empty, list.ToArray());
	}
}
