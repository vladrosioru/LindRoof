using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using LindRoof.FormsEnvironment;
using LindRoof.TextualEnvironment;
using LindRoof.toolbar;
using Microsoft.Win32;

namespace LindRoof;

public class MainWindow : Form
{
	public Cursor selectionCursor;

	private MainMenu mainMenu1;

	private OpenFileDialog openFileDialog1;

	private StatusBar statusBarEx;

	private MenuItem menuFile;

	private MenuItem menuFileNew;

	private MenuItem menuFileOpen;

	private MenuItem menuFileClose;

	private MenuItem menuFileSave;

	private MenuItem menuFileDXF;

	private MenuItem menuFilePrint;

	private MenuItem menuFilePrintPreview;

	private MenuItem menuFilePrinterSettings;

	private MenuItem menuEdit;

	private MenuItem menuSettings;

	private MenuItem menuWindow;

	private MenuItem menuHelp;

	private MenuItem menuFileExit;

	private MenuItem menuFileSeparator1;

	private MenuItem menuFileSeparator2;

	private MenuItem menuFileSeparator3;

	private MenuItem menuEditUndo;

	private MenuItem menuEditRedo;

	private MenuItem menuEditSeparator1;

	private MenuItem menuEditCut;

	private MenuItem menuEditCopy;

	private MenuItem menuEditPaste;

	private MenuItem menuItem7;

	private MenuItem menuEditFind;

	private IContainer components;

	private MenuItem menuSettingsUCS;

	private MenuItem menuWindowClose;

	private MenuItem menuWindowCloseAll;

	private MenuItem menuItem3;

	private MenuItem menuWindowArrangeWindows;

	private MenuItem menuWindowArrangeWindowsCascade;

	private MenuItem menuWindowArrangeWindowsHorizontally;

	private MenuItem menuWindowArrangeWindowsVertically;

	public bool draw_Origin = true;

	private StatusBarPanel statusBarPosition;

	public StatusBarPanel statusBarSnap;

	public StatusBarPanel statusBarGrid;

	public StatusBarPanel statusBarOrtho;

	public StatusBarPanel statusBarOsnap;

	private StatusBarPanel statusBarEmpty1;

	private StatusBarPanel statusBarEmpty2;

	private StatusBarPanel statusBarEmpty3;

	private StatusBarPanel statusBarEmpty0;

	private OfficeMenus officeMenus1;

	private ImageList imageList1;

	private StatusBarPanel statusBarLength;

	private StatusBarPanel statusBarEmpty_1;

	private StatusBarPanel statusBarLength2;

	private CultureInfo culture = new CultureInfo("");

	private NumberFormatInfo numInfo;

	private ToolBar toolBarMenu;

	private ToolBar toolBarDrawing;

	private ToolBarManager _toolBarManager;

	private MenuItem menuItem5;

	private MenuItem menuItem11;

	private MenuItem menuItem17;

	private MenuItem menuSettingsColor;

	private MenuItem menuSettingsOptions;

	private MenuItem menuEditDelete;

	private MenuItem menuWindowToolbar;

	private MenuItem menuWindowToolbarMenu;

	private MenuItem menuHelpInstructions;

	private MenuItem menuHelpAbout;

	private MenuItem menuHelpContact;

	private MenuItem menuWindowToolbarPaint;

	private ToolBarButton toolBarFileNew;

	private ToolBarButton toolBarFileOpen;

	private ToolBarButton toolBarFileClose;

	private ToolBarButton toolBarSeparator5;

	private ToolBarButton toolBarFileSave;

	private ToolBarButton toolBarFilePrintPreview;

	private ToolBarButton toolBarFilePrinterSettings;

	private ToolBarButton toolBarFilePrint;

	private ToolBarButton toolBarSeparator6;

	private ToolBarButton toolBarSeparator7;

	private ToolBarButton toolBarSettingsOptions;

	private ToolBarButton toolBarCancel;

	private ToolBarButton toolBarRedo;

	private ToolBarButton toolBarSeparator1;

	private ToolBarButton toolBarDrawLine;

	private ToolBarButton toolBarDrawPolyLine;

	private ToolBarButton toolBarSeparator3;

	private ToolBarButton toolBarShowUCS;

	private ToolBarButton toolBarSeparator4;

	private ToolBarButton toolBarSeparator8;

	private SaveFileDialog saveFileDialog1;

	private ToolBarButton toolBarAssignPanel;

	private ToolBarButton toolBarDrawRectangle;

	private ToolBarButton toolBarSeparator10;

	private ToolBarButton toolBarFillet;

	private ToolBarButton toolBarZoomExtents;

	private ToolBarButton toolBarOptimizePanel;

	private ToolBarButton toolBarExcelReport;

	private ColorDialog colorDialog1;

	public bool standardPanels;

	public MenuItem menuSettingsStandardPanels;

	private ToolBarButton toolBarBreakPanel;

	private ToolBarButton toolBarBreakAllPanels;

	private MenuItem menuFileSaveAs;

	private StatusBarPanel statusBarGridSize1;

	private StatusBarPanel statusBarGridSize2;

	private ToolBarButton toolBarDeletePanel;

	private StatusBarEx statusBar;

	private ToolBar toolBarObjects;

	private ToolBarButton toolBarButton1;

	private ToolBarButton toolBarButton2;

	private ToolBarButton toolBarButton3;

	private ToolBarButton toolBarButton4;

	private ToolBarButton toolBarButton5;

	private ToolBarButton toolBarButton6;

	private ToolBarButton toolBarButton7;

	private ToolBarButton toolBarButton8;

	private ToolBarButton toolBarButton10;

	private ToolBarButton toolBarAssignVoidRectangle;

	private ToolBarButton toolBarButton9;

	private ToolBarButton toolBarAssignVoidTriangle;

	public StatusBarPanelEx statusBarType;

	public StatusBarPanelEx statusBarType2;

	private MenuItem menuSettingsLAFarea;

	private MenuItem menuSettingsColorBgrnd;

	private MenuItem menuSettingsColorSlopes;

	private MenuItem menuSettingsLanguage;

	private MenuItem menuSettingsLanguage1;

	private MenuItem menuSettingsLanguage2;

	private MenuItem menuSettingsLanguage3;

	private MenuItem menuSettingsLanguage4;

	private MenuItem menuSettingsLanguage5;

	private MenuItem menuSettingsLanguage6;

	private MenuItem menuSettingsLanguage7;

	private MenuItem menuSettingsLanguage8;

	private MenuItem menuSettingsLanguage9;

	private MenuItem menuSettingsLanguage10;

	private MenuItem menuSettingsLanguage11;

	private MenuItem menuSettingsLanguage12;

	private MenuItem menuSettingsLanguage13;

	private MenuItem menuSettingsLanguage14;

	private MenuItem menuSettingsLanguage15;

	public ConsoleWindow consoleWindow;

	public dictionar_cuvinte dictionar;

	private string showucs;

	private string hideucs;

	private string snap;

	private string grid;

	private string ortho;

	private string osnap;

	public double LAFarea;

	public MainWindow()
	{
		InitializeComponent();
	}

	public MainWindow(string[] args, string language, double lfArea)
	{
		LAFarea = lfArea;
		Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
		dictionar = new dictionar_cuvinte(language);
		try
		{
			InitComp(variantaPersonala: true);
		}
		catch
		{
			MessageBox.Show("Eroare 1", "LindRoof", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			Environment.Exit(0);
		}
		if (menuSettingsLanguage1.Text == "xxx")
		{
			menuSettingsLanguage1.Visible = false;
		}
		if (menuSettingsLanguage2.Text == "xxx")
		{
			menuSettingsLanguage2.Visible = false;
		}
		if (menuSettingsLanguage3.Text == "xxx")
		{
			menuSettingsLanguage3.Visible = false;
		}
		if (menuSettingsLanguage4.Text == "xxx")
		{
			menuSettingsLanguage4.Visible = false;
		}
		if (menuSettingsLanguage5.Text == "xxx")
		{
			menuSettingsLanguage5.Visible = false;
		}
		if (menuSettingsLanguage6.Text == "xxx")
		{
			menuSettingsLanguage6.Visible = false;
		}
		if (menuSettingsLanguage7.Text == "xxx")
		{
			menuSettingsLanguage7.Visible = false;
		}
		if (menuSettingsLanguage8.Text == "xxx")
		{
			menuSettingsLanguage8.Visible = false;
		}
		if (menuSettingsLanguage9.Text == "xxx")
		{
			menuSettingsLanguage9.Visible = false;
		}
		if (menuSettingsLanguage10.Text == "xxx")
		{
			menuSettingsLanguage10.Visible = false;
		}
		if (menuSettingsLanguage11.Text == "xxx")
		{
			menuSettingsLanguage11.Visible = false;
		}
		if (menuSettingsLanguage12.Text == "xxx")
		{
			menuSettingsLanguage12.Visible = false;
		}
		if (menuSettingsLanguage13.Text == "xxx")
		{
			menuSettingsLanguage13.Visible = false;
		}
		if (menuSettingsLanguage14.Text == "xxx")
		{
			menuSettingsLanguage14.Visible = false;
		}
		if (menuSettingsLanguage15.Text == "xxx")
		{
			menuSettingsLanguage15.Visible = false;
		}
		snap = dictionar.dictionar[72];
		grid = dictionar.dictionar[73];
		ortho = dictionar.dictionar[74];
		osnap = dictionar.dictionar[75];
		showucs = dictionar.dictionar[87];
		hideucs = dictionar.dictionar[88];
		CenterToScreen();
		LaunchTheMenuThatLooksLikeOffice2003Menu();
		LoadCustomCursors();
		_toolBarManager = new ToolBarManager(this, this);
		toolBarMenu.Text = dictionar.dictionar[0];
		toolBarDrawing.Text = dictionar.dictionar[1];
		_toolBarManager.AddControl(toolBarMenu);
		_toolBarManager.AddControl(toolBarDrawing);
		string text = dictionar.dictionar[2] + (base.MdiChildren.Length + 1);
		Document document = new Document(this);
		document.Text = text;
		document.MdiParent = this;
		document.Show();
		document.graphicTable.base_point_x = -1 * document.Width / 2;
		document.graphicTable.base_point_y = document.Height / 2;
		ActivateMdiChild(document);
		FileAssociation fileAssociation = new FileAssociation();
		fileAssociation.Extension = dictionar.dictionar[3];
		fileAssociation.ContentType = dictionar.dictionar[4];
		fileAssociation.FullName = dictionar.dictionar[5];
		fileAssociation.ProperName = dictionar.dictionar[6];
		fileAssociation.AddCommand(dictionar.dictionar[7], Application.ExecutablePath + dictionar.dictionar[8]);
		fileAssociation.IconPath = Application.ExecutablePath;
		fileAssociation.Create();
		if (args.GetLength(0) != 0)
		{
			string text2 = "";
			foreach (string text3 in args)
			{
				text2 = text2 + " " + text3;
			}
			try
			{
				StreamReader sw = new StreamReader(text2);
				OpenFile(sw, text2);
			}
			catch
			{
				MessageBox.Show(dictionar.dictionar[9] + text2 + dictionar.dictionar[10]);
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LindRoof.MainWindow));
		this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
		this.menuFile = new System.Windows.Forms.MenuItem();
		this.menuFileNew = new System.Windows.Forms.MenuItem();
		this.menuFileOpen = new System.Windows.Forms.MenuItem();
		this.menuFileClose = new System.Windows.Forms.MenuItem();
		this.menuFileSeparator1 = new System.Windows.Forms.MenuItem();
		this.menuFileSave = new System.Windows.Forms.MenuItem();
		this.menuFileSaveAs = new System.Windows.Forms.MenuItem();
		this.menuItem17 = new System.Windows.Forms.MenuItem();
		this.menuFileDXF = new System.Windows.Forms.MenuItem();
		this.menuFileSeparator2 = new System.Windows.Forms.MenuItem();
		this.menuFilePrint = new System.Windows.Forms.MenuItem();
		this.menuFilePrintPreview = new System.Windows.Forms.MenuItem();
		this.menuFilePrinterSettings = new System.Windows.Forms.MenuItem();
		this.menuFileSeparator3 = new System.Windows.Forms.MenuItem();
		this.menuFileExit = new System.Windows.Forms.MenuItem();
		this.menuEdit = new System.Windows.Forms.MenuItem();
		this.menuEditUndo = new System.Windows.Forms.MenuItem();
		this.menuEditRedo = new System.Windows.Forms.MenuItem();
		this.menuEditSeparator1 = new System.Windows.Forms.MenuItem();
		this.menuEditCut = new System.Windows.Forms.MenuItem();
		this.menuEditDelete = new System.Windows.Forms.MenuItem();
		this.menuEditCopy = new System.Windows.Forms.MenuItem();
		this.menuEditPaste = new System.Windows.Forms.MenuItem();
		this.menuItem7 = new System.Windows.Forms.MenuItem();
		this.menuEditFind = new System.Windows.Forms.MenuItem();
		this.menuSettings = new System.Windows.Forms.MenuItem();
		this.menuSettingsOptions = new System.Windows.Forms.MenuItem();
		this.menuSettingsStandardPanels = new System.Windows.Forms.MenuItem();
		this.menuSettingsLAFarea = new System.Windows.Forms.MenuItem();
		this.menuItem5 = new System.Windows.Forms.MenuItem();
		this.menuSettingsColor = new System.Windows.Forms.MenuItem();
		this.menuSettingsColorBgrnd = new System.Windows.Forms.MenuItem();
		this.menuSettingsColorSlopes = new System.Windows.Forms.MenuItem();
		this.menuSettingsLanguage = new System.Windows.Forms.MenuItem();
		this.menuSettingsLanguage1 = new System.Windows.Forms.MenuItem();
		this.menuSettingsLanguage2 = new System.Windows.Forms.MenuItem();
		this.menuSettingsLanguage3 = new System.Windows.Forms.MenuItem();
		this.menuSettingsLanguage4 = new System.Windows.Forms.MenuItem();
		this.menuSettingsLanguage5 = new System.Windows.Forms.MenuItem();
		this.menuSettingsLanguage6 = new System.Windows.Forms.MenuItem();
		this.menuSettingsLanguage7 = new System.Windows.Forms.MenuItem();
		this.menuSettingsLanguage8 = new System.Windows.Forms.MenuItem();
		this.menuSettingsLanguage9 = new System.Windows.Forms.MenuItem();
		this.menuSettingsLanguage10 = new System.Windows.Forms.MenuItem();
		this.menuSettingsLanguage11 = new System.Windows.Forms.MenuItem();
		this.menuSettingsLanguage12 = new System.Windows.Forms.MenuItem();
		this.menuSettingsLanguage13 = new System.Windows.Forms.MenuItem();
		this.menuSettingsLanguage14 = new System.Windows.Forms.MenuItem();
		this.menuSettingsLanguage15 = new System.Windows.Forms.MenuItem();
		this.menuSettingsUCS = new System.Windows.Forms.MenuItem();
		this.menuWindow = new System.Windows.Forms.MenuItem();
		this.menuWindowToolbar = new System.Windows.Forms.MenuItem();
		this.menuWindowToolbarMenu = new System.Windows.Forms.MenuItem();
		this.menuWindowToolbarPaint = new System.Windows.Forms.MenuItem();
		this.menuItem11 = new System.Windows.Forms.MenuItem();
		this.menuWindowClose = new System.Windows.Forms.MenuItem();
		this.menuWindowCloseAll = new System.Windows.Forms.MenuItem();
		this.menuItem3 = new System.Windows.Forms.MenuItem();
		this.menuWindowArrangeWindows = new System.Windows.Forms.MenuItem();
		this.menuWindowArrangeWindowsCascade = new System.Windows.Forms.MenuItem();
		this.menuWindowArrangeWindowsHorizontally = new System.Windows.Forms.MenuItem();
		this.menuWindowArrangeWindowsVertically = new System.Windows.Forms.MenuItem();
		this.menuHelp = new System.Windows.Forms.MenuItem();
		this.menuHelpInstructions = new System.Windows.Forms.MenuItem();
		this.menuHelpAbout = new System.Windows.Forms.MenuItem();
		this.menuHelpContact = new System.Windows.Forms.MenuItem();
		this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
		this.toolBarMenu = new System.Windows.Forms.ToolBar();
		this.toolBarFileNew = new System.Windows.Forms.ToolBarButton();
		this.toolBarFileOpen = new System.Windows.Forms.ToolBarButton();
		this.toolBarFileClose = new System.Windows.Forms.ToolBarButton();
		this.toolBarSeparator5 = new System.Windows.Forms.ToolBarButton();
		this.toolBarFileSave = new System.Windows.Forms.ToolBarButton();
		this.toolBarFilePrintPreview = new System.Windows.Forms.ToolBarButton();
		this.toolBarFilePrint = new System.Windows.Forms.ToolBarButton();
		this.toolBarFilePrinterSettings = new System.Windows.Forms.ToolBarButton();
		this.toolBarSeparator6 = new System.Windows.Forms.ToolBarButton();
		this.toolBarButton10 = new System.Windows.Forms.ToolBarButton();
		this.toolBarSeparator7 = new System.Windows.Forms.ToolBarButton();
		this.toolBarSettingsOptions = new System.Windows.Forms.ToolBarButton();
		this.toolBarSeparator8 = new System.Windows.Forms.ToolBarButton();
		this.toolBarZoomExtents = new System.Windows.Forms.ToolBarButton();
		this.imageList1 = new System.Windows.Forms.ImageList(this.components);
		this.toolBarDrawing = new System.Windows.Forms.ToolBar();
		this.toolBarCancel = new System.Windows.Forms.ToolBarButton();
		this.toolBarRedo = new System.Windows.Forms.ToolBarButton();
		this.toolBarSeparator1 = new System.Windows.Forms.ToolBarButton();
		this.toolBarButton4 = new System.Windows.Forms.ToolBarButton();
		this.toolBarButton5 = new System.Windows.Forms.ToolBarButton();
		this.toolBarButton6 = new System.Windows.Forms.ToolBarButton();
		this.toolBarButton7 = new System.Windows.Forms.ToolBarButton();
		this.toolBarButton8 = new System.Windows.Forms.ToolBarButton();
		this.toolBarDrawLine = new System.Windows.Forms.ToolBarButton();
		this.toolBarDrawPolyLine = new System.Windows.Forms.ToolBarButton();
		this.toolBarDrawRectangle = new System.Windows.Forms.ToolBarButton();
		this.toolBarButton9 = new System.Windows.Forms.ToolBarButton();
		this.toolBarDeletePanel = new System.Windows.Forms.ToolBarButton();
		this.toolBarSeparator10 = new System.Windows.Forms.ToolBarButton();
		this.toolBarFillet = new System.Windows.Forms.ToolBarButton();
		this.toolBarAssignPanel = new System.Windows.Forms.ToolBarButton();
		this.toolBarSeparator3 = new System.Windows.Forms.ToolBarButton();
		this.toolBarShowUCS = new System.Windows.Forms.ToolBarButton();
		this.toolBarSeparator4 = new System.Windows.Forms.ToolBarButton();
		this.toolBarAssignVoidRectangle = new System.Windows.Forms.ToolBarButton();
		this.toolBarAssignVoidTriangle = new System.Windows.Forms.ToolBarButton();
		this.toolBarBreakPanel = new System.Windows.Forms.ToolBarButton();
		this.toolBarBreakAllPanels = new System.Windows.Forms.ToolBarButton();
		this.toolBarOptimizePanel = new System.Windows.Forms.ToolBarButton();
		this.toolBarExcelReport = new System.Windows.Forms.ToolBarButton();
		this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
		this.colorDialog1 = new System.Windows.Forms.ColorDialog();
		this.consoleWindow = new LindRoof.ConsoleWindow();
		this.statusBar = new LindRoof.StatusBarEx();
		this.statusBarPosition = new System.Windows.Forms.StatusBarPanel();
		this.statusBarEmpty0 = new System.Windows.Forms.StatusBarPanel();
		this.statusBarSnap = new System.Windows.Forms.StatusBarPanel();
		this.statusBarEmpty1 = new System.Windows.Forms.StatusBarPanel();
		this.statusBarGrid = new System.Windows.Forms.StatusBarPanel();
		this.statusBarEmpty2 = new System.Windows.Forms.StatusBarPanel();
		this.statusBarOrtho = new System.Windows.Forms.StatusBarPanel();
		this.statusBarEmpty3 = new System.Windows.Forms.StatusBarPanel();
		this.statusBarOsnap = new System.Windows.Forms.StatusBarPanel();
		this.statusBarEmpty_1 = new System.Windows.Forms.StatusBarPanel();
		this.statusBarLength = new System.Windows.Forms.StatusBarPanel();
		this.statusBarLength2 = new System.Windows.Forms.StatusBarPanel();
		this.statusBarType = new LindRoof.StatusBarPanelEx();
		this.statusBarType2 = new LindRoof.StatusBarPanelEx();
		this.statusBarGridSize1 = new System.Windows.Forms.StatusBarPanel();
		this.statusBarGridSize2 = new System.Windows.Forms.StatusBarPanel();
		this.officeMenus1 = new LindRoof.OfficeMenus(this.components);
		((System.ComponentModel.ISupportInitialize)this.statusBarPosition).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarEmpty0).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarSnap).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarEmpty1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarGrid).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarEmpty2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarOrtho).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarEmpty3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarOsnap).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarEmpty_1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarLength).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarLength2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarType2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarGridSize1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarGridSize2).BeginInit();
		base.SuspendLayout();
		this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[5] { this.menuFile, this.menuEdit, this.menuSettings, this.menuWindow, this.menuHelp });
		this.menuFile.Index = 0;
		this.menuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[14]
		{
			this.menuFileNew, this.menuFileOpen, this.menuFileClose, this.menuFileSeparator1, this.menuFileSave, this.menuFileSaveAs, this.menuItem17, this.menuFileDXF, this.menuFileSeparator2, this.menuFilePrint,
			this.menuFilePrintPreview, this.menuFilePrinterSettings, this.menuFileSeparator3, this.menuFileExit
		});
		this.menuFile.Text = "";
		this.menuFileNew.Index = 0;
		this.menuFileNew.RadioCheck = true;
		this.menuFileNew.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
		this.menuFileNew.Text = "";
		this.menuFileNew.Click += new System.EventHandler(menuFileNew_Click);
		this.menuFileOpen.Index = 1;
		this.menuFileOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
		this.menuFileOpen.Text = "";
		this.menuFileOpen.Click += new System.EventHandler(menuItem3_Click);
		this.menuFileClose.Index = 2;
		this.menuFileClose.Shortcut = System.Windows.Forms.Shortcut.CtrlF4;
		this.menuFileClose.Text = "";
		this.menuFileClose.Click += new System.EventHandler(menuFileClose_Click);
		this.menuFileSeparator1.Index = 3;
		this.menuFileSeparator1.Text = "-";
		this.menuFileSave.Index = 4;
		this.menuFileSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
		this.menuFileSave.Text = "";
		this.menuFileSave.Click += new System.EventHandler(menuFileSave_Click);
		this.menuFileSaveAs.Index = 5;
		this.menuFileSaveAs.Text = "";
		this.menuFileSaveAs.Click += new System.EventHandler(menuFileSaveAs_Click);
		this.menuItem17.Index = 6;
		this.menuItem17.Text = "-";
		this.menuFileDXF.Enabled = false;
		this.menuFileDXF.Index = 7;
		this.menuFileDXF.Text = "";
		this.menuFileSeparator2.Index = 8;
		this.menuFileSeparator2.Text = "-";
		this.menuFilePrint.Index = 9;
		this.menuFilePrint.Shortcut = System.Windows.Forms.Shortcut.CtrlP;
		this.menuFilePrint.Text = "";
		this.menuFilePrint.Click += new System.EventHandler(menuFilePrint_Click);
		this.menuFilePrintPreview.Index = 10;
		this.menuFilePrintPreview.Text = "";
		this.menuFilePrintPreview.Click += new System.EventHandler(menuFilePrintPreview_Click);
		this.menuFilePrinterSettings.Index = 11;
		this.menuFilePrinterSettings.Text = "Printer Settings";
		this.menuFilePrinterSettings.Click += new System.EventHandler(menuFilePrinterSettings_Click);
		this.menuFileSeparator3.Index = 12;
		this.menuFileSeparator3.Text = "-";
		this.menuFileExit.Index = 13;
		this.menuFileExit.Shortcut = System.Windows.Forms.Shortcut.AltF4;
		this.menuFileExit.Text = "";
		this.menuFileExit.Click += new System.EventHandler(menuFileExit_Click);
		this.menuEdit.Index = 1;
		this.menuEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[9] { this.menuEditUndo, this.menuEditRedo, this.menuEditSeparator1, this.menuEditCut, this.menuEditDelete, this.menuEditCopy, this.menuEditPaste, this.menuItem7, this.menuEditFind });
		this.menuEdit.Text = "";
		this.menuEditUndo.Index = 0;
		this.menuEditUndo.Text = "";
		this.menuEditRedo.Index = 1;
		this.menuEditRedo.Text = "";
		this.menuEditSeparator1.Index = 2;
		this.menuEditSeparator1.Text = "-";
		this.menuEditCut.Enabled = false;
		this.menuEditCut.Index = 3;
		this.menuEditCut.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
		this.menuEditCut.Text = "";
		this.menuEditDelete.Enabled = false;
		this.menuEditDelete.Index = 4;
		this.menuEditDelete.Text = "";
		this.menuEditCopy.Enabled = false;
		this.menuEditCopy.Index = 5;
		this.menuEditCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
		this.menuEditCopy.Text = "";
		this.menuEditPaste.Enabled = false;
		this.menuEditPaste.Index = 6;
		this.menuEditPaste.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
		this.menuEditPaste.Text = "";
		this.menuItem7.Index = 7;
		this.menuItem7.Text = "-";
		this.menuEditFind.Enabled = false;
		this.menuEditFind.Index = 8;
		this.menuEditFind.Shortcut = System.Windows.Forms.Shortcut.CtrlF;
		this.menuEditFind.Text = "";
		this.menuSettings.Index = 2;
		this.menuSettings.MenuItems.AddRange(new System.Windows.Forms.MenuItem[5] { this.menuSettingsOptions, this.menuItem5, this.menuSettingsColor, this.menuSettingsLanguage, this.menuSettingsUCS });
		this.menuSettings.Text = "";
		this.menuSettingsOptions.Index = 0;
		this.menuSettingsOptions.MenuItems.AddRange(new System.Windows.Forms.MenuItem[2] { this.menuSettingsStandardPanels, this.menuSettingsLAFarea });
		this.menuSettingsOptions.Text = "";
		this.menuSettingsStandardPanels.Checked = true;
		this.menuSettingsStandardPanels.Index = 0;
		this.menuSettingsStandardPanels.Text = "";
		this.menuSettingsStandardPanels.Click += new System.EventHandler(menuSettingsStandardPanels_Click);
		this.menuSettingsLAFarea.Index = 1;
		this.menuSettingsLAFarea.Text = "";
		this.menuSettingsLAFarea.Click += new System.EventHandler(menuSettingsLAFarea_Click);
		this.menuItem5.Index = 1;
		this.menuItem5.Text = "-";
		this.menuSettingsColor.Index = 2;
		this.menuSettingsColor.MenuItems.AddRange(new System.Windows.Forms.MenuItem[2] { this.menuSettingsColorBgrnd, this.menuSettingsColorSlopes });
		this.menuSettingsColor.Text = "";
		this.menuSettingsColorBgrnd.Index = 0;
		this.menuSettingsColorBgrnd.Text = "";
		this.menuSettingsColorBgrnd.Click += new System.EventHandler(menuSettingsColorBgrnd_Click);
		this.menuSettingsColorSlopes.Index = 1;
		this.menuSettingsColorSlopes.Text = "";
		this.menuSettingsColorSlopes.Click += new System.EventHandler(menuSettingsColorSlopes_Click);
		this.menuSettingsLanguage.Index = 3;
		this.menuSettingsLanguage.MenuItems.AddRange(new System.Windows.Forms.MenuItem[15]
		{
			this.menuSettingsLanguage1, this.menuSettingsLanguage2, this.menuSettingsLanguage3, this.menuSettingsLanguage4, this.menuSettingsLanguage5, this.menuSettingsLanguage6, this.menuSettingsLanguage7, this.menuSettingsLanguage8, this.menuSettingsLanguage9, this.menuSettingsLanguage10,
			this.menuSettingsLanguage11, this.menuSettingsLanguage12, this.menuSettingsLanguage13, this.menuSettingsLanguage14, this.menuSettingsLanguage15
		});
		this.menuSettingsLanguage.Text = "";
		this.menuSettingsLanguage1.Index = 0;
		this.menuSettingsLanguage1.Text = "";
		this.menuSettingsLanguage1.Click += new System.EventHandler(menuSettingsLanguage_Click);
		this.menuSettingsLanguage2.Index = 1;
		this.menuSettingsLanguage2.Text = "";
		this.menuSettingsLanguage2.Click += new System.EventHandler(menuSettingsLanguage_Click);
		this.menuSettingsLanguage3.Index = 2;
		this.menuSettingsLanguage3.Text = "";
		this.menuSettingsLanguage3.Click += new System.EventHandler(menuSettingsLanguage_Click);
		this.menuSettingsLanguage4.Index = 3;
		this.menuSettingsLanguage4.Text = "";
		this.menuSettingsLanguage4.Click += new System.EventHandler(menuSettingsLanguage_Click);
		this.menuSettingsLanguage5.Index = 4;
		this.menuSettingsLanguage5.Text = "";
		this.menuSettingsLanguage5.Click += new System.EventHandler(menuSettingsLanguage_Click);
		this.menuSettingsLanguage6.Index = 5;
		this.menuSettingsLanguage6.Text = "";
		this.menuSettingsLanguage6.Click += new System.EventHandler(menuSettingsLanguage_Click);
		this.menuSettingsLanguage7.Index = 6;
		this.menuSettingsLanguage7.Text = "";
		this.menuSettingsLanguage7.Click += new System.EventHandler(menuSettingsLanguage_Click);
		this.menuSettingsLanguage8.Index = 7;
		this.menuSettingsLanguage8.Text = "";
		this.menuSettingsLanguage8.Click += new System.EventHandler(menuSettingsLanguage_Click);
		this.menuSettingsLanguage9.Index = 8;
		this.menuSettingsLanguage9.Text = "";
		this.menuSettingsLanguage9.Click += new System.EventHandler(menuSettingsLanguage_Click);
		this.menuSettingsLanguage10.Index = 9;
		this.menuSettingsLanguage10.Text = "";
		this.menuSettingsLanguage10.Click += new System.EventHandler(menuSettingsLanguage_Click);
		this.menuSettingsLanguage11.Index = 10;
		this.menuSettingsLanguage11.Text = "";
		this.menuSettingsLanguage11.Click += new System.EventHandler(menuSettingsLanguage_Click);
		this.menuSettingsLanguage12.Index = 11;
		this.menuSettingsLanguage12.Text = "";
		this.menuSettingsLanguage12.Click += new System.EventHandler(menuSettingsLanguage_Click);
		this.menuSettingsLanguage13.Index = 12;
		this.menuSettingsLanguage13.Text = "";
		this.menuSettingsLanguage13.Click += new System.EventHandler(menuSettingsLanguage_Click);
		this.menuSettingsLanguage14.Index = 13;
		this.menuSettingsLanguage14.Text = "";
		this.menuSettingsLanguage14.Click += new System.EventHandler(menuSettingsLanguage_Click);
		this.menuSettingsLanguage15.Index = 14;
		this.menuSettingsLanguage15.Text = "";
		this.menuSettingsLanguage15.Click += new System.EventHandler(menuSettingsLanguage_Click);
		this.menuSettingsUCS.Index = 4;
		this.menuSettingsUCS.Text = "";
		this.menuSettingsUCS.Click += new System.EventHandler(menuSettingsUCS_Click);
		this.menuWindow.Index = 3;
		this.menuWindow.MenuItems.AddRange(new System.Windows.Forms.MenuItem[6] { this.menuWindowToolbar, this.menuItem11, this.menuWindowClose, this.menuWindowCloseAll, this.menuItem3, this.menuWindowArrangeWindows });
		this.menuWindow.Text = "";
		this.menuWindowToolbar.Enabled = false;
		this.menuWindowToolbar.Index = 0;
		this.menuWindowToolbar.MenuItems.AddRange(new System.Windows.Forms.MenuItem[2] { this.menuWindowToolbarMenu, this.menuWindowToolbarPaint });
		this.menuWindowToolbar.Text = "";
		this.menuWindowToolbarMenu.Index = 0;
		this.menuWindowToolbarMenu.Text = "";
		this.menuWindowToolbarPaint.Index = 1;
		this.menuWindowToolbarPaint.Text = "";
		this.menuItem11.Index = 1;
		this.menuItem11.Text = "-";
		this.menuWindowClose.Index = 2;
		this.menuWindowClose.Text = "";
		this.menuWindowClose.Click += new System.EventHandler(menuWindowClose_Click);
		this.menuWindowCloseAll.Index = 3;
		this.menuWindowCloseAll.Text = "";
		this.menuWindowCloseAll.Click += new System.EventHandler(menuWindowCloseAll_Click);
		this.menuItem3.Index = 4;
		this.menuItem3.Text = "-";
		this.menuWindowArrangeWindows.Index = 5;
		this.menuWindowArrangeWindows.MenuItems.AddRange(new System.Windows.Forms.MenuItem[3] { this.menuWindowArrangeWindowsCascade, this.menuWindowArrangeWindowsHorizontally, this.menuWindowArrangeWindowsVertically });
		this.menuWindowArrangeWindows.Text = "";
		this.menuWindowArrangeWindowsCascade.Index = 0;
		this.menuWindowArrangeWindowsCascade.Text = "";
		this.menuWindowArrangeWindowsCascade.Click += new System.EventHandler(menuWindowArrangeWindowsCascade_Click);
		this.menuWindowArrangeWindowsHorizontally.Index = 1;
		this.menuWindowArrangeWindowsHorizontally.Text = "";
		this.menuWindowArrangeWindowsHorizontally.Click += new System.EventHandler(menuWindowArrangeWindowsHorizontally_Click);
		this.menuWindowArrangeWindowsVertically.Index = 2;
		this.menuWindowArrangeWindowsVertically.Text = "";
		this.menuWindowArrangeWindowsVertically.Click += new System.EventHandler(menuWindowArrangeWindowsVertically_Click);
		this.menuHelp.Index = 4;
		this.menuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[3] { this.menuHelpInstructions, this.menuHelpAbout, this.menuHelpContact });
		this.menuHelp.Text = "";
		this.menuHelpInstructions.Index = 0;
		this.menuHelpInstructions.Text = "";
		this.menuHelpInstructions.Click += new System.EventHandler(menuHelpInstructions_Click);
		this.menuHelpAbout.Enabled = false;
		this.menuHelpAbout.Index = 1;
		this.menuHelpAbout.Text = "";
		this.menuHelpContact.Enabled = false;
		this.menuHelpContact.Index = 2;
		this.menuHelpContact.Text = "";
		this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(openFileDialog1_FileOk);
		this.toolBarMenu.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
		this.toolBarMenu.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[14]
		{
			this.toolBarFileNew, this.toolBarFileOpen, this.toolBarFileClose, this.toolBarSeparator5, this.toolBarFileSave, this.toolBarFilePrintPreview, this.toolBarFilePrint, this.toolBarFilePrinterSettings, this.toolBarSeparator6, this.toolBarButton10,
			this.toolBarSeparator7, this.toolBarSettingsOptions, this.toolBarSeparator8, this.toolBarZoomExtents
		});
		this.toolBarMenu.Divider = false;
		this.toolBarMenu.Dock = System.Windows.Forms.DockStyle.None;
		this.toolBarMenu.DropDownArrows = true;
		this.toolBarMenu.ImageList = this.imageList1;
		this.toolBarMenu.Location = new System.Drawing.Point(0, 32);
		this.toolBarMenu.Name = "toolBarMenu";
		this.toolBarMenu.ShowToolTips = true;
		this.toolBarMenu.Size = new System.Drawing.Size(176, 70);
		this.toolBarMenu.TabIndex = 0;
		this.toolBarMenu.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(toolBarMenu_ButtonClick);
		this.toolBarFileNew.ImageIndex = 0;
		this.toolBarFileNew.Name = "toolBarFileNew";
		this.toolBarFileOpen.ImageIndex = 1;
		this.toolBarFileOpen.Name = "toolBarFileOpen";
		this.toolBarFileClose.ImageIndex = 2;
		this.toolBarFileClose.Name = "toolBarFileClose";
		this.toolBarSeparator5.Name = "toolBarSeparator5";
		this.toolBarSeparator5.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
		this.toolBarFileSave.ImageIndex = 3;
		this.toolBarFileSave.Name = "toolBarFileSave";
		this.toolBarFilePrintPreview.ImageIndex = 7;
		this.toolBarFilePrintPreview.Name = "toolBarFilePrintPreview";
		this.toolBarFilePrint.ImageIndex = 6;
		this.toolBarFilePrint.Name = "toolBarFilePrint";
		this.toolBarFilePrinterSettings.ImageIndex = 7;
		this.toolBarFilePrinterSettings.Name = "toolBarFilePrinterSettings";
		this.toolBarFilePrinterSettings.ToolTipText = "Printer settings";
		this.toolBarSeparator6.Name = "toolBarSeparator6";
		this.toolBarSeparator6.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
		this.toolBarButton10.Name = "toolBarButton10";
		this.toolBarButton10.Visible = false;
		this.toolBarSeparator7.Name = "toolBarSeparator7";
		this.toolBarSeparator7.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
		this.toolBarSeparator7.Visible = false;
		this.toolBarSettingsOptions.ImageIndex = 19;
		this.toolBarSettingsOptions.Name = "toolBarSettingsOptions";
		this.toolBarSettingsOptions.Visible = false;
		this.toolBarSeparator8.Name = "toolBarSeparator8";
		this.toolBarSeparator8.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
		this.toolBarSeparator8.Visible = false;
		this.toolBarZoomExtents.ImageIndex = 15;
		this.toolBarZoomExtents.Name = "toolBarZoomExtents";
		this.imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
		this.imageList1.TransparentColor = System.Drawing.Color.Black;
		this.imageList1.Images.SetKeyName(0, "");
		this.imageList1.Images.SetKeyName(1, "");
		this.imageList1.Images.SetKeyName(2, "");
		this.imageList1.Images.SetKeyName(3, "");
		this.imageList1.Images.SetKeyName(4, "");
		this.imageList1.Images.SetKeyName(5, "");
		this.imageList1.Images.SetKeyName(6, "");
		this.imageList1.Images.SetKeyName(7, "");
		this.imageList1.Images.SetKeyName(8, "");
		this.imageList1.Images.SetKeyName(9, "");
		this.imageList1.Images.SetKeyName(10, "");
		this.imageList1.Images.SetKeyName(11, "");
		this.imageList1.Images.SetKeyName(12, "");
		this.imageList1.Images.SetKeyName(13, "");
		this.imageList1.Images.SetKeyName(14, "");
		this.imageList1.Images.SetKeyName(15, "");
		this.imageList1.Images.SetKeyName(16, "");
		this.imageList1.Images.SetKeyName(17, "");
		this.imageList1.Images.SetKeyName(18, "");
		this.imageList1.Images.SetKeyName(19, "");
		this.imageList1.Images.SetKeyName(20, "");
		this.imageList1.Images.SetKeyName(21, "");
		this.imageList1.Images.SetKeyName(22, "");
		this.imageList1.Images.SetKeyName(23, "");
		this.imageList1.Images.SetKeyName(24, "");
		this.imageList1.Images.SetKeyName(25, "");
		this.imageList1.Images.SetKeyName(26, "");
		this.imageList1.Images.SetKeyName(27, "");
		this.imageList1.Images.SetKeyName(28, "");
		this.imageList1.Images.SetKeyName(29, "");
		this.imageList1.Images.SetKeyName(30, "");
		this.imageList1.Images.SetKeyName(31, "");
		this.imageList1.Images.SetKeyName(32, "");
		this.imageList1.Images.SetKeyName(33, "");
		this.imageList1.Images.SetKeyName(34, "");
		this.imageList1.Images.SetKeyName(35, "");
		this.imageList1.Images.SetKeyName(36, "");
		this.imageList1.Images.SetKeyName(37, "");
		this.imageList1.Images.SetKeyName(38, "");
		this.imageList1.Images.SetKeyName(39, "");
		this.imageList1.Images.SetKeyName(40, "");
		this.imageList1.Images.SetKeyName(41, "");
		this.imageList1.Images.SetKeyName(42, "");
		this.imageList1.Images.SetKeyName(43, "");
		this.imageList1.Images.SetKeyName(44, "");
		this.imageList1.Images.SetKeyName(45, "");
		this.imageList1.Images.SetKeyName(46, "");
		this.imageList1.Images.SetKeyName(47, "");
		this.imageList1.Images.SetKeyName(48, "");
		this.imageList1.Images.SetKeyName(49, "");
		this.imageList1.Images.SetKeyName(50, "");
		this.imageList1.Images.SetKeyName(51, "");
		this.imageList1.Images.SetKeyName(52, "");
		this.toolBarDrawing.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
		this.toolBarDrawing.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[25]
		{
			this.toolBarCancel, this.toolBarRedo, this.toolBarSeparator1, this.toolBarButton4, this.toolBarButton5, this.toolBarButton6, this.toolBarButton7, this.toolBarButton8, this.toolBarDrawLine, this.toolBarDrawPolyLine,
			this.toolBarDrawRectangle, this.toolBarButton9, this.toolBarDeletePanel, this.toolBarSeparator10, this.toolBarFillet, this.toolBarAssignPanel, this.toolBarSeparator3, this.toolBarShowUCS, this.toolBarSeparator4, this.toolBarAssignVoidRectangle,
			this.toolBarAssignVoidTriangle, this.toolBarBreakPanel, this.toolBarBreakAllPanels, this.toolBarOptimizePanel, this.toolBarExcelReport
		});
		this.toolBarDrawing.Divider = false;
		this.toolBarDrawing.DropDownArrows = true;
		this.toolBarDrawing.ImageList = this.imageList1;
		this.toolBarDrawing.Location = new System.Drawing.Point(0, 0);
		this.toolBarDrawing.Name = "toolBarDrawing";
		this.toolBarDrawing.ShowToolTips = true;
		this.toolBarDrawing.Size = new System.Drawing.Size(712, 26);
		this.toolBarDrawing.TabIndex = 0;
		this.toolBarDrawing.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(toolBarDrawing_ButtonClick);
		this.toolBarCancel.ImageIndex = 31;
		this.toolBarCancel.Name = "toolBarCancel";
		this.toolBarRedo.ImageIndex = 30;
		this.toolBarRedo.Name = "toolBarRedo";
		this.toolBarSeparator1.Name = "toolBarSeparator1";
		this.toolBarSeparator1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
		this.toolBarButton4.Name = "toolBarButton4";
		this.toolBarButton4.Visible = false;
		this.toolBarButton5.Name = "toolBarButton5";
		this.toolBarButton5.Visible = false;
		this.toolBarButton6.Name = "toolBarButton6";
		this.toolBarButton6.Visible = false;
		this.toolBarButton7.Name = "toolBarButton7";
		this.toolBarButton7.Visible = false;
		this.toolBarButton8.Name = "toolBarButton8";
		this.toolBarButton8.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
		this.toolBarButton8.Visible = false;
		this.toolBarDrawLine.ImageIndex = 33;
		this.toolBarDrawLine.Name = "toolBarDrawLine";
		this.toolBarDrawPolyLine.ImageIndex = 35;
		this.toolBarDrawPolyLine.Name = "toolBarDrawPolyLine";
		this.toolBarDrawRectangle.ImageIndex = 5;
		this.toolBarDrawRectangle.Name = "toolBarDrawRectangle";
		this.toolBarButton9.Name = "toolBarButton9";
		this.toolBarButton9.Visible = false;
		this.toolBarDeletePanel.ImageIndex = 48;
		this.toolBarDeletePanel.Name = "toolBarDeletePanel";
		this.toolBarSeparator10.Name = "toolBarSeparator10";
		this.toolBarSeparator10.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
		this.toolBarFillet.ImageIndex = 10;
		this.toolBarFillet.Name = "toolBarFillet";
		this.toolBarAssignPanel.ImageIndex = 11;
		this.toolBarAssignPanel.Name = "toolBarAssignPanel";
		this.toolBarSeparator3.Name = "toolBarSeparator3";
		this.toolBarSeparator3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
		this.toolBarShowUCS.ImageIndex = 40;
		this.toolBarShowUCS.Name = "toolBarShowUCS";
		this.toolBarSeparator4.Name = "toolBarSeparator4";
		this.toolBarSeparator4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
		this.toolBarAssignVoidRectangle.ImageIndex = 49;
		this.toolBarAssignVoidRectangle.Name = "toolBarAssignVoidRectangle";
		this.toolBarAssignVoidTriangle.ImageIndex = 50;
		this.toolBarAssignVoidTriangle.Name = "toolBarAssignVoidTriangle";
		this.toolBarBreakPanel.ImageIndex = 46;
		this.toolBarBreakPanel.Name = "toolBarBreakPanel";
		this.toolBarBreakAllPanels.ImageIndex = 52;
		this.toolBarBreakAllPanels.Name = "toolBarBreakAllPanels";
		this.toolBarOptimizePanel.ImageIndex = 45;
		this.toolBarOptimizePanel.Name = "toolBarOptimizePanel";
		this.toolBarExcelReport.ImageIndex = 47;
		this.toolBarExcelReport.Name = "toolBarExcelReport";
		this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(saveFileDialog1_FileOk);
		this.colorDialog1.AllowFullOpen = false;
		this.consoleWindow.BackColor = System.Drawing.SystemColors.Control;
		this.consoleWindow.Cursor = System.Windows.Forms.Cursors.IBeam;
		this.consoleWindow.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.consoleWindow.Location = new System.Drawing.Point(0, 274);
		this.consoleWindow.Name = "consoleWindow";
		this.consoleWindow.Size = new System.Drawing.Size(712, 96);
		this.consoleWindow.TabIndex = 1;
		this.statusBar.Location = new System.Drawing.Point(0, 370);
		this.statusBar.Name = "statusBar";
		this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[16]
		{
			this.statusBarPosition, this.statusBarEmpty0, this.statusBarSnap, this.statusBarEmpty1, this.statusBarGrid, this.statusBarEmpty2, this.statusBarOrtho, this.statusBarEmpty3, this.statusBarOsnap, this.statusBarEmpty_1,
			this.statusBarLength, this.statusBarLength2, this.statusBarType, this.statusBarType2, this.statusBarGridSize1, this.statusBarGridSize2
		});
		this.statusBar.ShowPanels = true;
		this.statusBar.Size = new System.Drawing.Size(712, 22);
		this.statusBar.SizingGrip = false;
		this.statusBar.TabIndex = 0;
		this.statusBar.PanelClick += new System.Windows.Forms.StatusBarPanelClickEventHandler(statusBar_PanelClick);
		this.statusBarPosition.Name = "statusBarPosition";
		this.statusBarPosition.Width = 150;
		this.statusBarEmpty0.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.None;
		this.statusBarEmpty0.MinWidth = 2;
		this.statusBarEmpty0.Name = "statusBarEmpty0";
		this.statusBarEmpty0.Width = 2;
		this.statusBarSnap.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.Raised;
		this.statusBarSnap.Name = "statusBarSnap";
		this.statusBarSnap.Width = 50;
		this.statusBarEmpty1.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.None;
		this.statusBarEmpty1.MinWidth = 2;
		this.statusBarEmpty1.Name = "statusBarEmpty1";
		this.statusBarEmpty1.Width = 2;
		this.statusBarGrid.Name = "statusBarGrid";
		this.statusBarGrid.Width = 50;
		this.statusBarEmpty2.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.None;
		this.statusBarEmpty2.MinWidth = 2;
		this.statusBarEmpty2.Name = "statusBarEmpty2";
		this.statusBarEmpty2.Width = 2;
		this.statusBarOrtho.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.Raised;
		this.statusBarOrtho.Name = "statusBarOrtho";
		this.statusBarOrtho.Width = 60;
		this.statusBarEmpty3.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.None;
		this.statusBarEmpty3.MinWidth = 2;
		this.statusBarEmpty3.Name = "statusBarEmpty3";
		this.statusBarEmpty3.Width = 2;
		this.statusBarOsnap.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.Raised;
		this.statusBarOsnap.Name = "statusBarOsnap";
		this.statusBarOsnap.Width = 60;
		this.statusBarEmpty_1.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.None;
		this.statusBarEmpty_1.MinWidth = 20;
		this.statusBarEmpty_1.Name = "statusBarEmpty_1";
		this.statusBarEmpty_1.Width = 20;
		this.statusBarLength.Name = "statusBarLength";
		this.statusBarLength.Width = 120;
		this.statusBarLength2.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
		this.statusBarLength2.Name = "statusBarLength2";
		this.statusBarType.ForeColor = System.Drawing.Color.Empty;
		this.statusBarType.HatchedProgressBarStyle = System.Drawing.Drawing2D.HatchStyle.Horizontal;
		this.statusBarType.Maximum = 100;
		this.statusBarType.Minimum = 1;
		this.statusBarType.Name = "statusBarType";
		this.statusBarType.Style = LindRoof.StatusBarPanelStyleEx.Text;
		this.statusBarType.Value = 0;
		this.statusBarType2.ForeColor = System.Drawing.Color.Empty;
		this.statusBarType2.HatchedProgressBarStyle = System.Drawing.Drawing2D.HatchStyle.Horizontal;
		this.statusBarType2.Maximum = 100;
		this.statusBarType2.Minimum = 1;
		this.statusBarType2.Name = "statusBarType2";
		this.statusBarType2.Style = LindRoof.StatusBarPanelStyleEx.Text;
		this.statusBarType2.Value = 0;
		this.statusBarType2.Width = 80;
		this.statusBarGridSize1.MinWidth = 70;
		this.statusBarGridSize1.Name = "statusBarGridSize1";
		this.statusBarGridSize1.Width = 70;
		this.statusBarGridSize2.MinWidth = 40;
		this.statusBarGridSize2.Name = "statusBarGridSize2";
		this.statusBarGridSize2.Width = 70;
		this.officeMenus1.ImageList = null;
		this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
		this.BackColor = System.Drawing.SystemColors.Control;
		base.ClientSize = new System.Drawing.Size(712, 392);
		base.Controls.Add(this.toolBarMenu);
		base.Controls.Add(this.consoleWindow);
		base.Controls.Add(this.statusBar);
		base.Controls.Add(this.toolBarDrawing);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.IsMdiContainer = true;
		base.Menu = this.mainMenu1;
		this.MinimumSize = new System.Drawing.Size(200, 400);
		base.Name = "MainWindow";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Deactivate += new System.EventHandler(MainWindow_Deactivate);
		base.Load += new System.EventHandler(MainWindow_Load);
		base.Activated += new System.EventHandler(MainWindow_Activated);
		((System.ComponentModel.ISupportInitialize)this.statusBarPosition).EndInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarEmpty0).EndInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarSnap).EndInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarEmpty1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarGrid).EndInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarEmpty2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarOrtho).EndInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarEmpty3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarOsnap).EndInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarEmpty_1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarLength).EndInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarLength2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarType2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarGridSize1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.statusBarGridSize2).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	private void InitComp(bool variantaPersonala)
	{
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MainWindow));
		mainMenu1 = new MainMenu(components);
		menuFile = new MenuItem();
		menuFileNew = new MenuItem();
		menuFileOpen = new MenuItem();
		menuFileClose = new MenuItem();
		menuFileSeparator1 = new MenuItem();
		menuFileSave = new MenuItem();
		menuFileSaveAs = new MenuItem();
		menuItem17 = new MenuItem();
		menuFileDXF = new MenuItem();
		menuFileSeparator2 = new MenuItem();
		menuFilePrint = new MenuItem();
		menuFilePrintPreview = new MenuItem();
		menuFilePrinterSettings = new MenuItem();
		menuFileSeparator3 = new MenuItem();
		menuFileExit = new MenuItem();
		menuEdit = new MenuItem();
		menuEditUndo = new MenuItem();
		menuEditRedo = new MenuItem();
		menuEditSeparator1 = new MenuItem();
		menuEditCut = new MenuItem();
		menuEditDelete = new MenuItem();
		menuEditCopy = new MenuItem();
		menuEditPaste = new MenuItem();
		menuItem7 = new MenuItem();
		menuEditFind = new MenuItem();
		menuSettings = new MenuItem();
		menuSettingsOptions = new MenuItem();
		menuSettingsStandardPanels = new MenuItem();
		menuSettingsLAFarea = new MenuItem();
		menuItem5 = new MenuItem();
		menuSettingsColor = new MenuItem();
		menuSettingsColorBgrnd = new MenuItem();
		menuSettingsColorSlopes = new MenuItem();
		menuSettingsLanguage = new MenuItem();
		menuSettingsLanguage1 = new MenuItem();
		menuSettingsLanguage2 = new MenuItem();
		menuSettingsLanguage3 = new MenuItem();
		menuSettingsLanguage4 = new MenuItem();
		menuSettingsLanguage5 = new MenuItem();
		menuSettingsLanguage6 = new MenuItem();
		menuSettingsLanguage7 = new MenuItem();
		menuSettingsLanguage8 = new MenuItem();
		menuSettingsLanguage9 = new MenuItem();
		menuSettingsLanguage10 = new MenuItem();
		menuSettingsLanguage11 = new MenuItem();
		menuSettingsLanguage12 = new MenuItem();
		menuSettingsLanguage13 = new MenuItem();
		menuSettingsLanguage14 = new MenuItem();
		menuSettingsLanguage15 = new MenuItem();
		menuSettingsUCS = new MenuItem();
		menuWindow = new MenuItem();
		menuWindowToolbar = new MenuItem();
		menuWindowToolbarMenu = new MenuItem();
		menuWindowToolbarPaint = new MenuItem();
		menuItem11 = new MenuItem();
		menuWindowClose = new MenuItem();
		menuWindowCloseAll = new MenuItem();
		menuItem3 = new MenuItem();
		menuWindowArrangeWindows = new MenuItem();
		menuWindowArrangeWindowsCascade = new MenuItem();
		menuWindowArrangeWindowsHorizontally = new MenuItem();
		menuWindowArrangeWindowsVertically = new MenuItem();
		menuHelp = new MenuItem();
		menuHelpInstructions = new MenuItem();
		menuHelpAbout = new MenuItem();
		menuHelpContact = new MenuItem();
		openFileDialog1 = new OpenFileDialog();
		toolBarMenu = new ToolBar();
		toolBarFileNew = new ToolBarButton();
		toolBarFileOpen = new ToolBarButton();
		toolBarFileClose = new ToolBarButton();
		toolBarSeparator5 = new ToolBarButton();
		toolBarFileSave = new ToolBarButton();
		toolBarFilePrintPreview = new ToolBarButton();
		toolBarFilePrinterSettings = new ToolBarButton();
		toolBarFilePrint = new ToolBarButton();
		toolBarSeparator6 = new ToolBarButton();
		toolBarButton10 = new ToolBarButton();
		toolBarSeparator7 = new ToolBarButton();
		toolBarSettingsOptions = new ToolBarButton();
		toolBarSeparator8 = new ToolBarButton();
		toolBarZoomExtents = new ToolBarButton();
		imageList1 = new ImageList(components);
		toolBarDrawing = new ToolBar();
		toolBarCancel = new ToolBarButton();
		toolBarRedo = new ToolBarButton();
		toolBarSeparator1 = new ToolBarButton();
		toolBarButton4 = new ToolBarButton();
		toolBarButton5 = new ToolBarButton();
		toolBarButton6 = new ToolBarButton();
		toolBarButton7 = new ToolBarButton();
		toolBarButton8 = new ToolBarButton();
		toolBarDrawLine = new ToolBarButton();
		toolBarDrawPolyLine = new ToolBarButton();
		toolBarDrawRectangle = new ToolBarButton();
		toolBarButton9 = new ToolBarButton();
		toolBarDeletePanel = new ToolBarButton();
		toolBarSeparator10 = new ToolBarButton();
		toolBarFillet = new ToolBarButton();
		toolBarAssignPanel = new ToolBarButton();
		toolBarSeparator3 = new ToolBarButton();
		toolBarShowUCS = new ToolBarButton();
		toolBarSeparator4 = new ToolBarButton();
		toolBarAssignVoidRectangle = new ToolBarButton();
		toolBarAssignVoidTriangle = new ToolBarButton();
		toolBarBreakPanel = new ToolBarButton();
		toolBarBreakAllPanels = new ToolBarButton();
		toolBarOptimizePanel = new ToolBarButton();
		toolBarExcelReport = new ToolBarButton();
		saveFileDialog1 = new SaveFileDialog();
		colorDialog1 = new ColorDialog();
		consoleWindow = new ConsoleWindow(this);
		statusBar = new StatusBarEx();
		statusBarPosition = new StatusBarPanel();
		statusBarEmpty0 = new StatusBarPanel();
		statusBarSnap = new StatusBarPanel();
		statusBarEmpty1 = new StatusBarPanel();
		statusBarGrid = new StatusBarPanel();
		statusBarEmpty2 = new StatusBarPanel();
		statusBarOrtho = new StatusBarPanel();
		statusBarEmpty3 = new StatusBarPanel();
		statusBarOsnap = new StatusBarPanel();
		statusBarEmpty_1 = new StatusBarPanel();
		statusBarLength = new StatusBarPanel();
		statusBarLength2 = new StatusBarPanel();
		statusBarType = new StatusBarPanelEx();
		statusBarType2 = new StatusBarPanelEx();
		statusBarGridSize1 = new StatusBarPanel();
		statusBarGridSize2 = new StatusBarPanel();
		officeMenus1 = new OfficeMenus(components);
		((ISupportInitialize)statusBarPosition).BeginInit();
		((ISupportInitialize)statusBarEmpty0).BeginInit();
		((ISupportInitialize)statusBarSnap).BeginInit();
		((ISupportInitialize)statusBarEmpty1).BeginInit();
		((ISupportInitialize)statusBarGrid).BeginInit();
		((ISupportInitialize)statusBarEmpty2).BeginInit();
		((ISupportInitialize)statusBarOrtho).BeginInit();
		((ISupportInitialize)statusBarEmpty3).BeginInit();
		((ISupportInitialize)statusBarOsnap).BeginInit();
		((ISupportInitialize)statusBarEmpty_1).BeginInit();
		((ISupportInitialize)statusBarLength).BeginInit();
		((ISupportInitialize)statusBarLength2).BeginInit();
		((ISupportInitialize)statusBarType).BeginInit();
		((ISupportInitialize)statusBarType2).BeginInit();
		((ISupportInitialize)statusBarGridSize1).BeginInit();
		((ISupportInitialize)statusBarGridSize2).BeginInit();
		SuspendLayout();
		mainMenu1.MenuItems.AddRange(new MenuItem[5] { menuFile, menuEdit, menuSettings, menuWindow, menuHelp });
		menuFile.Index = 0;
		menuFile.MenuItems.AddRange(new MenuItem[14]
		{
			menuFileNew, menuFileOpen, menuFileClose, menuFileSeparator1, menuFileSave, menuFileSaveAs, menuItem17, menuFileDXF, menuFileSeparator2, menuFilePrint,
			menuFilePrintPreview, menuFilePrinterSettings, menuFileSeparator3, menuFileExit
		});
		menuFile.Text = dictionar.dictionar[11];
		menuFileNew.Index = 0;
		menuFileNew.RadioCheck = true;
		menuFileNew.Shortcut = Shortcut.CtrlN;
		menuFileNew.Text = dictionar.dictionar[12];
		menuFileNew.Click += menuFileNew_Click;
		menuFileOpen.Index = 1;
		menuFileOpen.Shortcut = Shortcut.CtrlO;
		menuFileOpen.Text = dictionar.dictionar[13];
		menuFileOpen.Click += menuItem3_Click;
		menuFileClose.Index = 2;
		menuFileClose.Shortcut = Shortcut.CtrlF4;
		menuFileClose.Text = dictionar.dictionar[14];
		menuFileClose.Click += menuFileClose_Click;
		menuFileSeparator1.Index = 3;
		menuFileSeparator1.Text = "-";
		menuFileSave.Index = 4;
		menuFileSave.Shortcut = Shortcut.CtrlS;
		menuFileSave.Text = dictionar.dictionar[15];
		menuFileSave.Click += menuFileSave_Click;
		menuFileSaveAs.Index = 5;
		menuFileSaveAs.Text = dictionar.dictionar[16];
		menuFileSaveAs.Click += menuFileSaveAs_Click;
		menuItem17.Index = 6;
		menuItem17.Text = "-";
		menuFileDXF.Index = 7;
		menuFileDXF.Text = dictionar.dictionar[17];
		menuFileDXF.Enabled = false;
		menuFileSeparator2.Index = 8;
		menuFileSeparator2.Text = "-";
		menuFilePrint.Index = 9;
		menuFilePrint.Shortcut = Shortcut.CtrlP;
		menuFilePrint.Text = dictionar.dictionar[18];
		menuFilePrint.Click += menuFilePrint_Click;
		menuFilePrintPreview.Index = 10;
		menuFilePrintPreview.Text = dictionar.dictionar[19];
		menuFilePrintPreview.Click += menuFilePrintPreview_Click;
		menuFilePrinterSettings.Index = 11;
		menuFilePrinterSettings.Text = "Printer Settings";
		menuFilePrinterSettings.Click += menuFilePrinterSettings_Click;
		menuFileSeparator3.Index = 12;
		menuFileSeparator3.Text = "-";
		menuFileExit.Index = 13;
		menuFileExit.Shortcut = Shortcut.AltF4;
		menuFileExit.Text = dictionar.dictionar[20];
		menuFileExit.Click += menuFileExit_Click;
		menuEdit.Visible = false;
		menuEdit.Index = 1;
		menuEdit.MenuItems.AddRange(new MenuItem[9] { menuEditUndo, menuEditRedo, menuEditSeparator1, menuEditCut, menuEditDelete, menuEditCopy, menuEditPaste, menuItem7, menuEditFind });
		menuEdit.Text = dictionar.dictionar[21];
		menuEditUndo.Index = 0;
		menuEditUndo.Text = dictionar.dictionar[22];
		menuEditRedo.Index = 1;
		menuEditRedo.Text = dictionar.dictionar[23];
		menuEditSeparator1.Index = 2;
		menuEditSeparator1.Text = "-";
		menuEditCut.Index = 3;
		menuEditCut.Shortcut = Shortcut.CtrlX;
		menuEditCut.Text = dictionar.dictionar[24];
		menuEditCut.Enabled = false;
		menuEditDelete.Index = 4;
		menuEditDelete.Text = dictionar.dictionar[25];
		menuEditDelete.Enabled = false;
		menuEditCopy.Index = 5;
		menuEditCopy.Shortcut = Shortcut.CtrlC;
		menuEditCopy.Text = dictionar.dictionar[26];
		menuEditCopy.Enabled = false;
		menuEditPaste.Index = 6;
		menuEditPaste.Shortcut = Shortcut.CtrlV;
		menuEditPaste.Text = dictionar.dictionar[27];
		menuEditPaste.Enabled = false;
		menuItem7.Index = 7;
		menuItem7.Text = "-";
		menuEditFind.Index = 8;
		menuEditFind.Shortcut = Shortcut.CtrlF;
		menuEditFind.Text = dictionar.dictionar[28];
		menuEditFind.Enabled = false;
		menuSettings.Index = 2;
		menuSettings.MenuItems.AddRange(new MenuItem[5] { menuSettingsOptions, menuItem5, menuSettingsColor, menuSettingsLanguage, menuSettingsUCS });
		menuSettings.Text = dictionar.dictionar[29];
		menuSettingsOptions.Index = 0;
		menuSettingsOptions.MenuItems.AddRange(new MenuItem[2] { menuSettingsStandardPanels, menuSettingsLAFarea });
		menuSettingsOptions.Text = dictionar.dictionar[30];
		menuSettingsStandardPanels.Checked = true;
		menuSettingsStandardPanels.Index = 0;
		menuSettingsStandardPanels.Text = dictionar.dictionar[31];
		menuSettingsStandardPanels.Click += menuSettingsStandardPanels_Click;
		menuSettingsLAFarea.Index = 1;
		menuSettingsLAFarea.Text = dictionar.dictionar[296];
		menuSettingsLAFarea.Click += menuSettingsLAFarea_Click;
		menuItem5.Index = 1;
		menuItem5.Text = "-";
		menuSettingsColor.Index = 2;
		menuSettingsColor.MenuItems.AddRange(new MenuItem[2] { menuSettingsColorBgrnd, menuSettingsColorSlopes });
		menuSettingsColor.Text = dictionar.dictionar[32];
		menuSettingsColorBgrnd.Index = 0;
		menuSettingsColorBgrnd.Text = dictionar.dictionar[33];
		menuSettingsColorBgrnd.Click += menuSettingsColorBgrnd_Click;
		menuSettingsColorSlopes.Index = 1;
		menuSettingsColorSlopes.Text = dictionar.dictionar[34];
		menuSettingsColorSlopes.Click += menuSettingsColorSlopes_Click;
		menuSettingsLanguage.Index = 3;
		menuSettingsLanguage.MenuItems.AddRange(new MenuItem[15]
		{
			menuSettingsLanguage1, menuSettingsLanguage2, menuSettingsLanguage3, menuSettingsLanguage4, menuSettingsLanguage5, menuSettingsLanguage6, menuSettingsLanguage7, menuSettingsLanguage8, menuSettingsLanguage9, menuSettingsLanguage10,
			menuSettingsLanguage11, menuSettingsLanguage12, menuSettingsLanguage13, menuSettingsLanguage14, menuSettingsLanguage15
		});
		menuSettingsLanguage.Text = dictionar.dictionar[110];
		menuSettingsLanguage1.Checked = false;
		menuSettingsLanguage1.Index = 0;
		menuSettingsLanguage1.Text = dictionar.dictionar[111];
		menuSettingsLanguage1.Click += menuSettingsLanguage_Click;
		menuSettingsLanguage2.Checked = false;
		menuSettingsLanguage2.Index = 1;
		menuSettingsLanguage2.Text = dictionar.dictionar[112];
		menuSettingsLanguage2.Click += menuSettingsLanguage_Click;
		menuSettingsLanguage3.Checked = false;
		menuSettingsLanguage3.Index = 2;
		menuSettingsLanguage3.Text = dictionar.dictionar[113];
		menuSettingsLanguage3.Click += menuSettingsLanguage_Click;
		menuSettingsLanguage4.Checked = false;
		menuSettingsLanguage4.Index = 3;
		menuSettingsLanguage4.Text = dictionar.dictionar[114];
		menuSettingsLanguage4.Click += menuSettingsLanguage_Click;
		menuSettingsLanguage5.Checked = false;
		menuSettingsLanguage5.Index = 4;
		menuSettingsLanguage5.Text = dictionar.dictionar[115];
		menuSettingsLanguage5.Click += menuSettingsLanguage_Click;
		menuSettingsLanguage6.Checked = false;
		menuSettingsLanguage6.Index = 5;
		menuSettingsLanguage6.Text = dictionar.dictionar[116];
		menuSettingsLanguage6.Click += menuSettingsLanguage_Click;
		menuSettingsLanguage7.Checked = false;
		menuSettingsLanguage7.Index = 6;
		menuSettingsLanguage7.Text = dictionar.dictionar[117];
		menuSettingsLanguage7.Click += menuSettingsLanguage_Click;
		menuSettingsLanguage8.Checked = false;
		menuSettingsLanguage8.Index = 7;
		menuSettingsLanguage8.Text = dictionar.dictionar[118];
		menuSettingsLanguage8.Click += menuSettingsLanguage_Click;
		menuSettingsLanguage9.Checked = false;
		menuSettingsLanguage9.Index = 8;
		menuSettingsLanguage9.Text = dictionar.dictionar[119];
		menuSettingsLanguage9.Click += menuSettingsLanguage_Click;
		menuSettingsLanguage10.Checked = false;
		menuSettingsLanguage10.Index = 9;
		menuSettingsLanguage10.Text = dictionar.dictionar[120];
		menuSettingsLanguage10.Click += menuSettingsLanguage_Click;
		menuSettingsLanguage11.Checked = false;
		menuSettingsLanguage11.Index = 10;
		menuSettingsLanguage11.Text = dictionar.dictionar[121];
		menuSettingsLanguage11.Click += menuSettingsLanguage_Click;
		menuSettingsLanguage12.Checked = false;
		menuSettingsLanguage12.Index = 11;
		menuSettingsLanguage12.Text = dictionar.dictionar[122];
		menuSettingsLanguage12.Click += menuSettingsLanguage_Click;
		menuSettingsLanguage13.Checked = false;
		menuSettingsLanguage13.Index = 12;
		menuSettingsLanguage13.Text = dictionar.dictionar[123];
		menuSettingsLanguage13.Click += menuSettingsLanguage_Click;
		menuSettingsLanguage14.Checked = false;
		menuSettingsLanguage14.Index = 13;
		menuSettingsLanguage14.Text = dictionar.dictionar[124];
		menuSettingsLanguage14.Click += menuSettingsLanguage_Click;
		menuSettingsLanguage15.Checked = false;
		menuSettingsLanguage15.Index = 14;
		menuSettingsLanguage15.Text = dictionar.dictionar[125];
		menuSettingsLanguage15.Click += menuSettingsLanguage_Click;
		menuSettingsUCS.Index = 4;
		menuSettingsUCS.Text = dictionar.dictionar[35];
		menuSettingsUCS.Click += menuSettingsUCS_Click;
		menuWindow.Index = 3;
		menuWindow.MenuItems.AddRange(new MenuItem[6] { menuWindowToolbar, menuItem11, menuWindowClose, menuWindowCloseAll, menuItem3, menuWindowArrangeWindows });
		menuWindow.Text = dictionar.dictionar[36];
		menuWindowToolbar.Index = 0;
		menuWindowToolbar.MenuItems.AddRange(new MenuItem[2] { menuWindowToolbarMenu, menuWindowToolbarPaint });
		menuWindowToolbar.Text = dictionar.dictionar[37];
		menuWindowToolbar.Enabled = false;
		menuWindowToolbarMenu.Index = 0;
		menuWindowToolbarMenu.Text = dictionar.dictionar[38];
		menuWindowToolbarPaint.Index = 1;
		menuWindowToolbarPaint.Text = dictionar.dictionar[39];
		menuItem11.Index = 1;
		menuItem11.Text = "-";
		menuWindowClose.Index = 2;
		menuWindowClose.Text = dictionar.dictionar[40];
		menuWindowClose.Click += menuWindowClose_Click;
		menuWindowCloseAll.Index = 3;
		menuWindowCloseAll.Text = dictionar.dictionar[41];
		menuWindowCloseAll.Click += menuWindowCloseAll_Click;
		menuItem3.Index = 4;
		menuItem3.Text = "-";
		menuWindowArrangeWindows.Index = 5;
		menuWindowArrangeWindows.MenuItems.AddRange(new MenuItem[3] { menuWindowArrangeWindowsCascade, menuWindowArrangeWindowsHorizontally, menuWindowArrangeWindowsVertically });
		menuWindowArrangeWindows.Text = dictionar.dictionar[42];
		menuWindowArrangeWindowsCascade.Index = 0;
		menuWindowArrangeWindowsCascade.Text = dictionar.dictionar[43];
		menuWindowArrangeWindowsCascade.Click += menuWindowArrangeWindowsCascade_Click;
		menuWindowArrangeWindowsHorizontally.Index = 1;
		menuWindowArrangeWindowsHorizontally.Text = dictionar.dictionar[44];
		menuWindowArrangeWindowsHorizontally.Click += menuWindowArrangeWindowsHorizontally_Click;
		menuWindowArrangeWindowsVertically.Index = 2;
		menuWindowArrangeWindowsVertically.Text = dictionar.dictionar[45];
		menuWindowArrangeWindowsVertically.Click += menuWindowArrangeWindowsVertically_Click;
		menuHelp.Visible = false;
		menuHelp.Index = 4;
		menuHelp.MenuItems.AddRange(new MenuItem[3] { menuHelpInstructions, menuHelpAbout, menuHelpContact });
		menuHelp.Text = dictionar.dictionar[46];
		menuHelpInstructions.Index = 0;
		menuHelpInstructions.Text = dictionar.dictionar[47];
		menuHelpInstructions.Click += menuHelpInstructions_Click;
		menuHelpAbout.Index = 1;
		menuHelpAbout.Text = dictionar.dictionar[48];
		menuHelpAbout.Enabled = false;
		menuHelpContact.Index = 2;
		menuHelpContact.Text = dictionar.dictionar[49];
		menuHelpContact.Enabled = false;
		openFileDialog1.FileOk += openFileDialog1_FileOk;
		toolBarMenu.Appearance = ToolBarAppearance.Flat;
		toolBarMenu.Buttons.AddRange(new ToolBarButton[14]
		{
			toolBarFileNew, toolBarFileOpen, toolBarFileClose, toolBarSeparator5, toolBarFileSave, toolBarFilePrintPreview, toolBarFilePrint, toolBarFilePrinterSettings, toolBarSeparator6, toolBarButton10,
			toolBarSeparator7, toolBarSettingsOptions, toolBarSeparator8, toolBarZoomExtents
		});
		toolBarMenu.Divider = false;
		toolBarMenu.Dock = DockStyle.None;
		toolBarMenu.DropDownArrows = true;
		toolBarMenu.ImageList = imageList1;
		toolBarMenu.Location = new Point(0, 32);
		toolBarMenu.Name = "toolBarMenu";
		toolBarMenu.ShowToolTips = true;
		toolBarMenu.Size = new Size(176, 26);
		toolBarMenu.TabIndex = 0;
		toolBarMenu.ButtonClick += toolBarMenu_ButtonClick;
		toolBarFileNew.ImageIndex = 0;
		toolBarFileNew.Name = "toolBarFileNew";
		toolBarFileNew.ToolTipText = dictionar.dictionar[50];
		toolBarFileOpen.ImageIndex = 1;
		toolBarFileOpen.Name = "toolBarFileOpen";
		toolBarFileOpen.ToolTipText = dictionar.dictionar[51];
		toolBarFileClose.ImageIndex = 2;
		toolBarFileClose.Name = "toolBarFileClose";
		toolBarFileClose.ToolTipText = dictionar.dictionar[52];
		toolBarSeparator5.Name = "toolBarSeparator5";
		toolBarSeparator5.Style = ToolBarButtonStyle.Separator;
		toolBarFileSave.ImageIndex = 3;
		toolBarFileSave.Name = "toolBarFileSave";
		toolBarFileSave.ToolTipText = dictionar.dictionar[53];
		toolBarFilePrintPreview.ImageIndex = 7;
		toolBarFilePrintPreview.Name = "toolBarFilePrintPreview";
		toolBarFilePrintPreview.ToolTipText = dictionar.dictionar[54];
		toolBarFilePrinterSettings.ImageIndex = 7;
		toolBarFilePrinterSettings.Name = "toolBarFilePrinterSettings";
		toolBarFilePrinterSettings.ToolTipText = "Printer settings";
		toolBarFilePrint.ImageIndex = 6;
		toolBarFilePrint.Name = "toolBarFilePrint";
		toolBarFilePrint.ToolTipText = dictionar.dictionar[55];
		toolBarSeparator6.Name = "toolBarSeparator6";
		toolBarSeparator6.Style = ToolBarButtonStyle.Separator;
		toolBarButton10.Name = "toolBarButton10";
		toolBarButton10.Visible = false;
		toolBarSeparator7.Name = "toolBarSeparator7";
		toolBarSeparator7.Style = ToolBarButtonStyle.Separator;
		toolBarSeparator7.Visible = false;
		toolBarSettingsOptions.ImageIndex = 19;
		toolBarSettingsOptions.Name = "toolBarSettingsOptions";
		toolBarSettingsOptions.ToolTipText = dictionar.dictionar[56];
		toolBarSettingsOptions.Visible = false;
		toolBarSeparator8.Name = "toolBarSeparator8";
		toolBarSeparator8.Style = ToolBarButtonStyle.Separator;
		toolBarSeparator8.Visible = false;
		toolBarZoomExtents.ImageIndex = 15;
		toolBarZoomExtents.Name = "toolBarZoomExtents";
		toolBarZoomExtents.ToolTipText = dictionar.dictionar[57];
		imageList1.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageList1.ImageStream");
		imageList1.TransparentColor = Color.Black;
		imageList1.Images.SetKeyName(0, "");
		imageList1.Images.SetKeyName(1, "");
		imageList1.Images.SetKeyName(2, "");
		imageList1.Images.SetKeyName(3, "");
		imageList1.Images.SetKeyName(4, "");
		imageList1.Images.SetKeyName(5, "");
		imageList1.Images.SetKeyName(6, "");
		imageList1.Images.SetKeyName(7, "");
		imageList1.Images.SetKeyName(8, "");
		imageList1.Images.SetKeyName(9, "");
		imageList1.Images.SetKeyName(10, "");
		imageList1.Images.SetKeyName(11, "");
		imageList1.Images.SetKeyName(12, "");
		imageList1.Images.SetKeyName(13, "");
		imageList1.Images.SetKeyName(14, "");
		imageList1.Images.SetKeyName(15, "");
		imageList1.Images.SetKeyName(16, "");
		imageList1.Images.SetKeyName(17, "");
		imageList1.Images.SetKeyName(18, "");
		imageList1.Images.SetKeyName(19, "");
		imageList1.Images.SetKeyName(20, "");
		imageList1.Images.SetKeyName(21, "");
		imageList1.Images.SetKeyName(22, "");
		imageList1.Images.SetKeyName(23, "");
		imageList1.Images.SetKeyName(24, "");
		imageList1.Images.SetKeyName(25, "");
		imageList1.Images.SetKeyName(26, "");
		imageList1.Images.SetKeyName(27, "");
		imageList1.Images.SetKeyName(28, "");
		imageList1.Images.SetKeyName(29, "");
		imageList1.Images.SetKeyName(30, "");
		imageList1.Images.SetKeyName(31, "");
		imageList1.Images.SetKeyName(32, "");
		imageList1.Images.SetKeyName(33, "");
		imageList1.Images.SetKeyName(34, "");
		imageList1.Images.SetKeyName(35, "");
		imageList1.Images.SetKeyName(36, "");
		imageList1.Images.SetKeyName(37, "");
		imageList1.Images.SetKeyName(38, "");
		imageList1.Images.SetKeyName(39, "");
		imageList1.Images.SetKeyName(40, "");
		imageList1.Images.SetKeyName(41, "");
		imageList1.Images.SetKeyName(42, "");
		imageList1.Images.SetKeyName(43, "");
		imageList1.Images.SetKeyName(44, "");
		imageList1.Images.SetKeyName(45, "");
		imageList1.Images.SetKeyName(46, "");
		imageList1.Images.SetKeyName(47, "");
		imageList1.Images.SetKeyName(48, "");
		imageList1.Images.SetKeyName(49, "");
		imageList1.Images.SetKeyName(50, "");
		imageList1.Images.SetKeyName(51, "");
		toolBarDrawing.Appearance = ToolBarAppearance.Flat;
		toolBarDrawing.Buttons.AddRange(new ToolBarButton[25]
		{
			toolBarCancel, toolBarRedo, toolBarSeparator1, toolBarButton4, toolBarButton5, toolBarButton6, toolBarButton7, toolBarButton8, toolBarDrawLine, toolBarDrawPolyLine,
			toolBarDrawRectangle, toolBarButton9, toolBarDeletePanel, toolBarSeparator10, toolBarFillet, toolBarAssignPanel, toolBarSeparator3, toolBarShowUCS, toolBarSeparator4, toolBarAssignVoidRectangle,
			toolBarAssignVoidTriangle, toolBarBreakPanel, toolBarBreakAllPanels, toolBarOptimizePanel, toolBarExcelReport
		});
		toolBarDrawing.Divider = false;
		toolBarDrawing.DropDownArrows = true;
		toolBarDrawing.ImageList = imageList1;
		toolBarDrawing.Location = new Point(0, 0);
		toolBarDrawing.Name = "toolBarDrawing";
		toolBarDrawing.ShowToolTips = true;
		toolBarDrawing.Size = new Size(712, 26);
		toolBarDrawing.TabIndex = 0;
		toolBarDrawing.ButtonClick += toolBarDrawing_ButtonClick;
		toolBarCancel.ImageIndex = 31;
		toolBarCancel.Name = "toolBarCancel";
		toolBarCancel.ToolTipText = dictionar.dictionar[58];
		toolBarRedo.ImageIndex = 30;
		toolBarRedo.Name = "toolBarRedo";
		toolBarRedo.ToolTipText = dictionar.dictionar[59];
		toolBarSeparator1.Name = "toolBarSeparator1";
		toolBarSeparator1.Style = ToolBarButtonStyle.Separator;
		toolBarButton4.Name = "toolBarButton4";
		toolBarButton4.Visible = false;
		toolBarButton5.Name = "toolBarButton5";
		toolBarButton5.Visible = false;
		toolBarButton6.Name = "toolBarButton6";
		toolBarButton6.Visible = false;
		toolBarButton7.Name = "toolBarButton7";
		toolBarButton7.Visible = false;
		toolBarButton8.Name = "toolBarButton8";
		toolBarButton8.Style = ToolBarButtonStyle.Separator;
		toolBarButton8.Visible = false;
		toolBarDrawLine.ImageIndex = 33;
		toolBarDrawLine.Name = "toolBarDrawLine";
		toolBarDrawLine.ToolTipText = dictionar.dictionar[60];
		toolBarDrawPolyLine.ImageIndex = 35;
		toolBarDrawPolyLine.Name = "toolBarDrawPolyLine";
		toolBarDrawPolyLine.ToolTipText = dictionar.dictionar[61];
		toolBarDrawRectangle.ImageIndex = 5;
		toolBarDrawRectangle.Name = "toolBarDrawRectangle";
		toolBarDrawRectangle.ToolTipText = dictionar.dictionar[62];
		toolBarButton9.Name = "toolBarButton9";
		toolBarButton9.Visible = false;
		toolBarDeletePanel.ImageIndex = 48;
		toolBarDeletePanel.Name = "toolBarDeletePanel";
		toolBarDeletePanel.ToolTipText = dictionar.dictionar[63];
		toolBarSeparator10.Name = "toolBarSeparator10";
		toolBarSeparator10.Style = ToolBarButtonStyle.Separator;
		toolBarFillet.ImageIndex = 10;
		toolBarFillet.Name = "toolBarFillet";
		toolBarFillet.ToolTipText = dictionar.dictionar[64];
		toolBarAssignPanel.ImageIndex = 11;
		toolBarAssignPanel.Name = "toolBarAssignPanel";
		toolBarAssignPanel.ToolTipText = dictionar.dictionar[65];
		toolBarSeparator3.Name = "toolBarSeparator3";
		toolBarSeparator3.Style = ToolBarButtonStyle.Separator;
		toolBarShowUCS.ImageIndex = 40;
		toolBarShowUCS.Name = "toolBarShowUCS";
		toolBarShowUCS.ToolTipText = dictionar.dictionar[66];
		toolBarSeparator4.Name = "toolBarSeparator4";
		toolBarSeparator4.Style = ToolBarButtonStyle.Separator;
		toolBarAssignVoidRectangle.ImageIndex = 49;
		toolBarAssignVoidRectangle.Name = "toolBarAssignVoidRectangle";
		toolBarAssignVoidRectangle.ToolTipText = dictionar.dictionar[67];
		toolBarAssignVoidTriangle.ImageIndex = 50;
		toolBarAssignVoidTriangle.Name = "toolBarAssignVoidTriangle";
		toolBarAssignVoidTriangle.ToolTipText = dictionar.dictionar[68];
		toolBarBreakPanel.ImageIndex = 46;
		toolBarBreakPanel.Name = "toolBarBreakPanel";
		toolBarBreakPanel.ToolTipText = dictionar.dictionar[69];
		toolBarBreakAllPanels.ImageIndex = 52;
		toolBarBreakAllPanels.Name = "toolBarBreakAllPanels";
		toolBarBreakAllPanels.ToolTipText = dictionar.dictionar[315];
		toolBarOptimizePanel.ImageIndex = 45;
		toolBarOptimizePanel.Name = "toolBarOptimizePanel";
		toolBarOptimizePanel.ToolTipText = dictionar.dictionar[70];
		toolBarExcelReport.ImageIndex = 47;
		toolBarExcelReport.Name = "toolBarExcelReport";
		toolBarExcelReport.ToolTipText = dictionar.dictionar[71];
		saveFileDialog1.FileOk += saveFileDialog1_FileOk;
		colorDialog1.AllowFullOpen = false;
		consoleWindow.BackColor = SystemColors.Control;
		consoleWindow.Cursor = Cursors.IBeam;
		consoleWindow.Dock = DockStyle.Bottom;
		consoleWindow.Location = new Point(0, 354);
		consoleWindow.Name = "consoleWindow";
		consoleWindow.Size = new Size(712, 96);
		consoleWindow.TabIndex = 1;
		statusBar.Location = new Point(0, 450);
		statusBar.Name = "statusBar";
		statusBar.Panels.AddRange(new StatusBarPanel[16]
		{
			statusBarPosition, statusBarEmpty0, statusBarSnap, statusBarEmpty1, statusBarGrid, statusBarEmpty2, statusBarOrtho, statusBarEmpty3, statusBarOsnap, statusBarEmpty_1,
			statusBarLength, statusBarLength2, statusBarType, statusBarType2, statusBarGridSize1, statusBarGridSize2
		});
		statusBar.ShowPanels = true;
		statusBar.Size = new Size(712, 22);
		statusBar.SizingGrip = false;
		statusBar.TabIndex = 0;
		statusBar.PanelClick += statusBar_PanelClick;
		statusBarPosition.Name = "statusBarPosition";
		statusBarPosition.Width = 150;
		statusBarEmpty0.BorderStyle = StatusBarPanelBorderStyle.None;
		statusBarEmpty0.MinWidth = 2;
		statusBarEmpty0.Name = "statusBarEmpty0";
		statusBarEmpty0.Width = 2;
		statusBarSnap.BorderStyle = StatusBarPanelBorderStyle.Raised;
		statusBarSnap.Name = "statusBarSnap";
		statusBarSnap.Text = dictionar.dictionar[72];
		statusBarSnap.Width = 50;
		statusBarEmpty1.BorderStyle = StatusBarPanelBorderStyle.None;
		statusBarEmpty1.MinWidth = 2;
		statusBarEmpty1.Name = "statusBarEmpty1";
		statusBarEmpty1.Width = 2;
		statusBarGrid.Name = "statusBarGrid";
		statusBarGrid.Text = dictionar.dictionar[73];
		statusBarGrid.Width = 50;
		statusBarEmpty2.BorderStyle = StatusBarPanelBorderStyle.None;
		statusBarEmpty2.MinWidth = 2;
		statusBarEmpty2.Name = "statusBarEmpty2";
		statusBarEmpty2.Width = 2;
		statusBarOrtho.BorderStyle = StatusBarPanelBorderStyle.Raised;
		statusBarOrtho.Name = "statusBarOrtho";
		statusBarOrtho.Text = dictionar.dictionar[74];
		statusBarOrtho.Width = 60;
		statusBarEmpty3.BorderStyle = StatusBarPanelBorderStyle.None;
		statusBarEmpty3.MinWidth = 2;
		statusBarEmpty3.Name = "statusBarEmpty3";
		statusBarEmpty3.Width = 2;
		statusBarOsnap.BorderStyle = StatusBarPanelBorderStyle.Raised;
		statusBarOsnap.Name = "statusBarOsnap";
		statusBarOsnap.Text = dictionar.dictionar[75];
		statusBarOsnap.Width = 60;
		statusBarEmpty_1.BorderStyle = StatusBarPanelBorderStyle.None;
		statusBarEmpty_1.MinWidth = 20;
		statusBarEmpty_1.Name = "statusBarEmpty_1";
		statusBarEmpty_1.Width = 20;
		statusBarLength.Name = "statusBarLength";
		statusBarLength.Text = dictionar.dictionar[76];
		statusBarLength.Width = 120;
		statusBarLength2.Alignment = HorizontalAlignment.Right;
		statusBarLength2.Name = "statusBarLength2";
		statusBarLength2.Text = dictionar.dictionar[77];
		statusBarType.ForeColor = Color.Empty;
		statusBarType.HatchedProgressBarStyle = HatchStyle.Horizontal;
		statusBarType.Maximum = 100;
		statusBarType.Minimum = 1;
		statusBarType.Name = "statusBarType";
		statusBarType.Style = StatusBarPanelStyleEx.Text;
		statusBarType.Text = dictionar.dictionar[78];
		statusBarType.Value = 0;
		statusBarType2.ForeColor = Color.Empty;
		statusBarType2.HatchedProgressBarStyle = HatchStyle.Horizontal;
		statusBarType2.Maximum = 100;
		statusBarType2.Minimum = 1;
		statusBarType2.Name = "statusBarType2";
		statusBarType2.Style = StatusBarPanelStyleEx.Text;
		statusBarType2.Text = dictionar.dictionar[79];
		statusBarType2.Value = 0;
		statusBarType2.Width = 80;
		statusBarGridSize1.MinWidth = 70;
		statusBarGridSize1.Name = "statusBarGridSize1";
		statusBarGridSize1.Text = dictionar.dictionar[80];
		statusBarGridSize1.Width = 70;
		statusBarGridSize2.MinWidth = 40;
		statusBarGridSize2.Name = "statusBarGridSize2";
		statusBarGridSize2.Width = 70;
		officeMenus1.ImageList = null;
		AutoScaleBaseSize = new Size(5, 13);
		BackColor = SystemColors.Control;
		base.ClientSize = new Size(712, 472);
		base.Controls.Add(toolBarMenu);
		base.Controls.Add(consoleWindow);
		base.Controls.Add(statusBar);
		base.Controls.Add(toolBarDrawing);
		base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		base.IsMdiContainer = true;
		base.Menu = mainMenu1;
		MinimumSize = new Size(200, 400);
		base.Name = "MainWindow";
		base.StartPosition = FormStartPosition.CenterScreen;
		Text = dictionar.dictionar[81];
		base.WindowState = FormWindowState.Maximized;
		base.Deactivate += MainWindow_Deactivate;
		base.Activated += MainWindow_Activated;
		base.Load += MainWindow_Load;
		((ISupportInitialize)statusBarPosition).EndInit();
		((ISupportInitialize)statusBarEmpty0).EndInit();
		((ISupportInitialize)statusBarSnap).EndInit();
		((ISupportInitialize)statusBarEmpty1).EndInit();
		((ISupportInitialize)statusBarGrid).EndInit();
		((ISupportInitialize)statusBarEmpty2).EndInit();
		((ISupportInitialize)statusBarOrtho).EndInit();
		((ISupportInitialize)statusBarEmpty3).EndInit();
		((ISupportInitialize)statusBarOsnap).EndInit();
		((ISupportInitialize)statusBarEmpty_1).EndInit();
		((ISupportInitialize)statusBarLength).EndInit();
		((ISupportInitialize)statusBarLength2).EndInit();
		((ISupportInitialize)statusBarType).EndInit();
		((ISupportInitialize)statusBarType2).EndInit();
		((ISupportInitialize)statusBarGridSize1).EndInit();
		((ISupportInitialize)statusBarGridSize2).EndInit();
		ResumeLayout(performLayout: false);
		PerformLayout();
	}

	[STAThread]
	private static void Main(string[] args)
	{
		double lfArea = 150.0;
		new CultureInfo("en-US", useUserOverride: false);
		string text = "English";
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\LindSoft\\LINDROOF", writable: true);
		if (registryKey == null)
		{
			registryKey = Registry.CurrentUser.CreateSubKey("Software\\LindSoft\\LINDROOF");
			registryKey.SetValue("Language", text, RegistryValueKind.String);
			registryKey.SetValue("LAFarea", lfArea.ToString(), RegistryValueKind.String);
		}
		else
		{
			try
			{
				text = (string)registryKey.GetValue("Language");
				lfArea = double.Parse((string)registryKey.GetValue("LAFarea"), CultureInfo.CurrentUICulture);
			}
			catch
			{
				registryKey.SetValue("Language", text, RegistryValueKind.String);
				registryKey.SetValue("LAFarea", lfArea.ToString(), RegistryValueKind.String);
			}
		}
		dictionar_cuvinte dictionar_cuvinte2 = new dictionar_cuvinte(text);
		DateTime dateTime = DateTime.Today.ToUniversalTime();
		DateTime dateTime2 = DateTime.Now;
		try
		{
			StreamReader streamReader = new StreamReader(Application.StartupPath + "/date.ver");
			string text2 = "";
			char[] array = streamReader.ReadToEnd().ToCharArray();
			for (byte b = 0; b < array.Length; b++)
			{
				ref char reference = ref array[b];
				reference = (char)((int)reference >> 1 + b % 8);
				text2 += array[b];
			}
			dateTime2 = new DateTime(long.Parse(text2));
		}
		catch
		{
			MessageBox.Show("Eroare la citirea fisierelor componente (date.ver)", "LindRoof", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			Environment.Exit(0);
		}
		long ticks = dateTime.ToUniversalTime().Ticks;
		try
		{
			registryKey = Registry.CurrentUser.OpenSubKey("Software\\XLNDRF\\X-Key", writable: true);
		}
		catch
		{
			MessageBox.Show("Eroare la citirea din Registry", "LindRoof", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			Environment.Exit(0);
		}
		bool flag = false;
		if (registryKey != null)
		{
			string[] valueNames = registryKey.GetValueNames();
			foreach (string text3 in valueNames)
			{
				if (text3 == "v4")
				{
					flag = true;
					break;
				}
			}
		}
		if (registryKey == null)
		{
			try
			{
				registryKey = Registry.CurrentUser.CreateSubKey("Software\\XLNDRF\\X-Key");
				registryKey.SetValue("v4", ticks, RegistryValueKind.QWord);
				try
				{
					Application.Run(new MainWindow(args, text, lfArea));
					return;
				}
				catch
				{
					MessageBox.Show("Eroare la lansare aplicatie (prima lansare)", "LindRoof", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					Environment.Exit(0);
					return;
				}
			}
			catch
			{
				MessageBox.Show(dictionar_cuvinte2.dictionar[295], "LindRoof", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Environment.Exit(0);
				return;
			}
		}
		if (!flag)
		{
			registryKey.SetValue("v4", ticks, RegistryValueKind.QWord);
			try
			{
				Application.Run(new MainWindow(args, text, lfArea));
				return;
			}
			catch
			{
				MessageBox.Show("Eroare la lansare aplicatie (prima lansare)", "LindRoof", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Environment.Exit(0);
				return;
			}
		}
		long ticks2 = 0L;
		DateTime now = DateTime.Now;
		try
		{
			ticks2 = (long)registryKey.GetValue("v4");
		}
		catch
		{
			MessageBox.Show("Eroare la scriere in Registry (string)key.GetValue(st)", "LindRoof", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			Environment.Exit(0);
		}
		try
		{
			now = new DateTime(ticks2);
		}
		catch
		{
			now = dateTime;
			registryKey.SetValue("v4", ticks, RegistryValueKind.QWord);
		}
		if (now <= dateTime)
		{
			if (dateTime <= dateTime2)
			{
				try
				{
					registryKey.SetValue("v4", ticks, RegistryValueKind.QWord);
				}
				catch
				{
					MessageBox.Show("Eroare la scriere in Registry key.SetValue(v4, td, RegistryValueKind.QWord)", "LindRoof", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					Environment.Exit(0);
				}
				Application.Run(new MainWindow(args, text, lfArea));
			}
			else
			{
				MessageBox.Show(dictionar_cuvinte2.dictionar[83] + dictionar_cuvinte2.dictionar[84], "LindRoof", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Environment.Exit(0);
			}
		}
		else
		{
			MessageBox.Show(dictionar_cuvinte2.dictionar[85] + dictionar_cuvinte2.dictionar[86], "LindRoof", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			Environment.Exit(0);
		}
	}

	private void LaunchTheMenuThatLooksLikeOffice2003Menu()
	{
		officeMenus1.Start(this);
		officeMenus1.ImageList = imageList1;
		officeMenus1.AddPicture(menuFileNew, 0);
		officeMenus1.AddPicture(menuFileOpen, 1);
		officeMenus1.AddPicture(menuFileClose, 2);
		officeMenus1.AddPicture(menuFileSave, 3);
		officeMenus1.AddPicture(menuFileDXF, 4);
		officeMenus1.AddPicture(menuFilePrint, 6);
		officeMenus1.AddPicture(menuFilePrintPreview, 7);
		officeMenus1.AddPicture(menuFilePrinterSettings, 7);
		officeMenus1.AddPicture(menuFileExit, 8);
		officeMenus1.AddPicture(menuEditUndo, 31);
		officeMenus1.AddPicture(menuEditRedo, 30);
		officeMenus1.AddPicture(menuEditCut, 27);
		officeMenus1.AddPicture(menuEditCopy, 26);
		officeMenus1.AddPicture(menuEditDelete, 28);
		officeMenus1.AddPicture(menuEditPaste, 29);
		officeMenus1.AddPicture(menuEditFind, 15);
		officeMenus1.AddPicture(menuSettingsOptions, 19);
		officeMenus1.AddPicture(menuSettingsColor, 51);
		officeMenus1.AddPicture(menuSettingsUCS, 40);
		officeMenus1.AddPicture(menuWindowArrangeWindows, 25);
		officeMenus1.AddPicture(menuWindowToolbar, 45);
		officeMenus1.AddPicture(menuHelpInstructions, 42);
		officeMenus1.AddPicture(menuHelpAbout, 43);
		officeMenus1.AddPicture(menuHelpContact, 44);
		officeMenus1.AddPicture(menuWindowToolbarMenu, 46);
		officeMenus1.AddPicture(menuWindowToolbarPaint, 47);
	}

	private void MainWindow_Load(object sender, EventArgs e)
	{
		SplashForm splashForm = new SplashForm();
		splashForm.ShowDialog();
	}

	private void menuFileNew_Click(object sender, EventArgs e)
	{
		Document document = new Document(this);
		SettingUCSvisibility(document);
		string text = dictionar.dictionar[2] + (base.MdiChildren.Length + 1);
		document.Text = text;
		document.MdiParent = this;
		AllowDocumentOperations(val: true);
		document.Show();
		document.graphicTable.base_point_x = -1 * document.Width / 2;
		document.graphicTable.base_point_y = document.Height / 2;
	}

	private void menuFileExit_Click(object sender, EventArgs e)
	{
		Dispose();
	}

	private void menuFileClose_Click(object sender, EventArgs e)
	{
		if (base.MdiChildren.Length != 0)
		{
			((Document)base.ActiveMdiChild).CloseDocument();
		}
	}

	private void menuSettingsUCS_Click(object sender, EventArgs e)
	{
		MenuItem menuItem = (MenuItem)sender;
		menuItem.Checked = !menuItem.Checked;
		if (menuSettingsUCS.Text == showucs)
		{
			menuSettingsUCS.Text = hideucs;
			for (int i = 0; i < base.MdiChildren.Length; i++)
			{
				Document document = (Document)base.MdiChildren[i];
				document.DrawOrigin(b: true);
			}
		}
		else if (menuSettingsUCS.Text == hideucs)
		{
			menuSettingsUCS.Text = showucs;
			for (int j = 0; j < base.MdiChildren.Length; j++)
			{
				Document document2 = (Document)base.MdiChildren[j];
				document2.DrawOrigin(b: false);
			}
		}
	}

	private void menuWindowClose_Click(object sender, EventArgs e)
	{
		((Document)base.ActiveMdiChild).CloseDocument();
	}

	private void menuWindowCloseAll_Click(object sender, EventArgs e)
	{
		int i = 0;
		for (int num = base.MdiChildren.Length; i < num; i++)
		{
			((Document)base.ActiveMdiChild).CloseDocument();
		}
	}

	private void menuWindowArrangeWindowsCascade_Click(object sender, EventArgs e)
	{
		LayoutMdi(MdiLayout.Cascade);
	}

	private void menuWindowArrangeWindowsHorizontally_Click(object sender, EventArgs e)
	{
		LayoutMdi(MdiLayout.TileHorizontal);
	}

	private void menuWindowArrangeWindowsVertically_Click(object sender, EventArgs e)
	{
		LayoutMdi(MdiLayout.TileVertical);
	}

	private void menuSettingsStandardPanels_Click(object sender, EventArgs e)
	{
		if (menuSettingsStandardPanels.Checked)
		{
			menuSettingsStandardPanels.Checked = false;
			standardPanels = false;
		}
		else
		{
			menuSettingsStandardPanels.Checked = true;
			standardPanels = true;
		}
	}

	private void menuItem2_Click(object sender, EventArgs e)
	{
	}

	private void statusBar_PanelClick(object sender, StatusBarPanelClickEventArgs e)
	{
		if (base.MdiChildren.Length == 0 || !(e.Button.ToString() == "Left") || e.Clicks != 1)
		{
			return;
		}
		if (e.StatusBarPanel.Text == snap)
		{
			if (statusBarSnap.BorderStyle == StatusBarPanelBorderStyle.Raised)
			{
				statusBarSnap.BorderStyle = StatusBarPanelBorderStyle.Sunken;
			}
			else
			{
				statusBarSnap.BorderStyle = StatusBarPanelBorderStyle.Raised;
			}
		}
		else if (e.StatusBarPanel.Text == grid)
		{
			if (statusBarGrid.BorderStyle == StatusBarPanelBorderStyle.Raised)
			{
				statusBarGrid.BorderStyle = StatusBarPanelBorderStyle.Sunken;
			}
			else
			{
				statusBarGrid.BorderStyle = StatusBarPanelBorderStyle.Raised;
			}
			((Document)base.ActiveMdiChild).graphicTable.RedrawAll();
		}
		else if (e.StatusBarPanel.Text == ortho)
		{
			if (statusBarOrtho.BorderStyle == StatusBarPanelBorderStyle.Raised)
			{
				statusBarOrtho.BorderStyle = StatusBarPanelBorderStyle.Sunken;
			}
			else
			{
				statusBarOrtho.BorderStyle = StatusBarPanelBorderStyle.Raised;
			}
		}
		else if (e.StatusBarPanel.Text == osnap)
		{
			if (statusBarOsnap.BorderStyle == StatusBarPanelBorderStyle.Raised)
			{
				statusBarOsnap.BorderStyle = StatusBarPanelBorderStyle.Sunken;
			}
			else
			{
				statusBarOsnap.BorderStyle = StatusBarPanelBorderStyle.Raised;
			}
		}
	}

	private void MainWindow_Deactivate(object sender, EventArgs e)
	{
		try
		{
			((Document)base.ActiveMdiChild).graphicTable.Enabled = false;
		}
		catch
		{
		}
	}

	private void MainWindow_Activated(object sender, EventArgs e)
	{
		try
		{
			((Document)base.ActiveMdiChild).graphicTable.Enabled = true;
		}
		catch
		{
		}
	}

	private void toolBarDrawing_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
	{
		if (base.MdiChildren.Length == 0)
		{
			return;
		}
		switch (toolBarDrawing.Buttons.IndexOf(e.Button))
		{
		case 0:
			if (base.MdiChildren.Length != 0 && ((Document)base.ActiveMdiChild).graphicTable.Visible)
			{
				((Document)base.ActiveMdiChild).graphicTable.PopUndoStack();
			}
			break;
		case 1:
			if (base.MdiChildren.Length != 0 && ((Document)base.ActiveMdiChild).graphicTable.Visible)
			{
				((Document)base.ActiveMdiChild).graphicTable.PopRedoStack();
			}
			break;
		case 8:
			consoleWindow.CommandLine.Focus();
			SendKeys.SendWait(dictionar.dictionar[128] + " ");
			break;
		case 9:
			consoleWindow.CommandLine.Focus();
			SendKeys.SendWait(dictionar.dictionar[129] + " ");
			break;
		case 10:
			consoleWindow.CommandLine.Focus();
			SendKeys.SendWait(dictionar.dictionar[136] + " ");
			break;
		case 11:
			if (((Document)base.ActiveMdiChild).graphicTableForPanels.Visible && ((Document)base.ActiveMdiChild).graphicTable.panels.Count != 0)
			{
				((Document)base.ActiveMdiChild).graphicTableForPanels.activeCommand = 3;
			}
			break;
		case 12:
			if (base.MdiChildren.Length != 0 && ((Document)base.ActiveMdiChild).graphicTable.Visible && ((Document)base.ActiveMdiChild).graphicTable.panels.Count != 0)
			{
				ListOfPanels listOfPanels = new ListOfPanels(((Document)base.ActiveMdiChild).graphicTable.panels);
				listOfPanels.ShowInTaskbar = false;
				if (listOfPanels.ShowDialog() == DialogResult.OK)
				{
					DeletePanel((string)listOfPanels.listBox1.SelectedItem);
				}
			}
			break;
		case 14:
			consoleWindow.CommandLine.Focus();
			SendKeys.SendWait(dictionar.dictionar[132] + " ");
			break;
		case 15:
			consoleWindow.CommandLine.Focus();
			SendKeys.SendWait(dictionar.dictionar[134] + " ");
			break;
		case 17:
		{
			MenuItem menuItem = menuSettingsUCS;
			menuItem.Checked = !menuItem.Checked;
			if (menuSettingsUCS.Text == showucs)
			{
				menuSettingsUCS.Text = hideucs;
				for (int i = 0; i < base.MdiChildren.Length; i++)
				{
					Document document = (Document)base.MdiChildren[i];
					document.DrawOrigin(b: true);
				}
			}
			else if (menuSettingsUCS.Text == hideucs)
			{
				menuSettingsUCS.Text = showucs;
				for (int j = 0; j < base.MdiChildren.Length; j++)
				{
					Document document2 = (Document)base.MdiChildren[j];
					document2.DrawOrigin(b: false);
				}
			}
			break;
		}
		case 19:
			if (((Document)base.ActiveMdiChild).graphicTableForPanels.Visible && !((Document)base.ActiveMdiChild).angleDeclaration.Enabled && ((Document)base.ActiveMdiChild).angleDeclaration.Text != "" && !((Document)base.ActiveMdiChild).enableCommand && !((Document)base.ActiveMdiChild).graphicTableForPanels.iAmSelecting && ((Document)base.ActiveMdiChild).graphicTable.panels.Count != 0)
			{
				((Document)base.ActiveMdiChild).graphicTableForPanels.activeCommand = 3;
				((Document)base.ActiveMdiChild).graphicTableForPanels.trapezoidal = false;
			}
			break;
		case 20:
			if (((Document)base.ActiveMdiChild).graphicTableForPanels.Visible && !((Document)base.ActiveMdiChild).angleDeclaration.Enabled && ((Document)base.ActiveMdiChild).angleDeclaration.Text != "" && !((Document)base.ActiveMdiChild).enableCommand && !((Document)base.ActiveMdiChild).graphicTableForPanels.iAmSelecting && ((Document)base.ActiveMdiChild).graphicTable.panels.Count != 0)
			{
				((Document)base.ActiveMdiChild).graphicTableForPanels.activeCommand = 3;
				((Document)base.ActiveMdiChild).graphicTableForPanels.trapezoidal = true;
			}
			break;
		case 21:
			if (base.MdiChildren.Length != 0 && ((Document)base.ActiveMdiChild).graphicTableForPanels.Visible && !((Document)base.ActiveMdiChild).angleDeclaration.Enabled && ((Document)base.ActiveMdiChild).angleDeclaration.Text != "" && !((Document)base.ActiveMdiChild).enableCommand && !((Document)base.ActiveMdiChild).graphicTableForPanels.iAmSelecting)
			{
				((Document)base.ActiveMdiChild).BreakPanel();
			}
			break;
		case 22:
			if (base.MdiChildren.Length != 0 && ((Document)base.ActiveMdiChild).graphicTableForPanels.Visible && !((Document)base.ActiveMdiChild).angleDeclaration.Enabled && ((Document)base.ActiveMdiChild).angleDeclaration.Text != "" && !((Document)base.ActiveMdiChild).enableCommand && !((Document)base.ActiveMdiChild).graphicTableForPanels.iAmSelecting)
			{
				((Document)base.ActiveMdiChild).BreakAllPanels();
			}
			break;
		case 23:
			if (base.MdiChildren.Length != 0 && ((Document)base.ActiveMdiChild).graphicTableForPanels.Visible && !((Document)base.ActiveMdiChild).angleDeclaration.Enabled && ((Document)base.ActiveMdiChild).angleDeclaration.Text != "" && !((Document)base.ActiveMdiChild).enableCommand && !((Document)base.ActiveMdiChild).graphicTableForPanels.iAmSelecting)
			{
				((Document)base.ActiveMdiChild).graphicTableForPanels.CuttingOptimization();
			}
			break;
		case 24:
			if (base.MdiChildren.Length != 0 && ((Document)base.ActiveMdiChild).graphicTableForPanels.Visible)
			{
				((Document)base.ActiveMdiChild).graphicTableForPanels.MakeReport();
			}
			break;
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 13:
		case 16:
		case 18:
			break;
		}
	}

	private void toolBarMenu_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
	{
		switch (toolBarMenu.Buttons.IndexOf(e.Button))
		{
		case 0:
			menuFileNew.PerformClick();
			break;
		case 1:
			menuFileOpen.PerformClick();
			break;
		case 2:
			menuFileClose.PerformClick();
			break;
		case 4:
			menuFileSave.PerformClick();
			break;
		case 5:
			if (base.MdiChildren.Length != 0)
			{
				if (((Document)base.ActiveMdiChild).graphicTable.Visible)
				{
					((Document)base.ActiveMdiChild).graphicTable.PrintPreview();
				}
				else if (((Document)base.ActiveMdiChild).graphicTableForPanels.Visible)
				{
					((Document)base.ActiveMdiChild).graphicTableForPanels.PrintPreview();
				}
			}
			break;
		case 6:
			menuFilePrint.PerformClick();
			break;
		case 7:
			menuFilePrinterSettings.PerformClick();
			break;
		case 9:
			menuFileDXF.PerformClick();
			break;
		case 11:
			menuSettingsOptions.PerformClick();
			break;
		case 13:
			if (base.MdiChildren.Length != 0)
			{
				if (((Document)base.ActiveMdiChild).graphicTable.Visible)
				{
					((Document)base.ActiveMdiChild).graphicTable.ZoomExtents();
				}
				else
				{
					((Document)base.ActiveMdiChild).graphicTableForPanels.ZoomExtents();
				}
			}
			break;
		case 3:
		case 8:
		case 10:
		case 12:
			break;
		}
	}

	private void menuFileSave_Click(object sender, EventArgs e)
	{
		if (base.MdiChildren.Length == 0 || ((Document)base.ActiveMdiChild).graphicTable.objects == null || ((Document)base.ActiveMdiChild).graphicTable.objects.Count == 0)
		{
			return;
		}
		if (((Document)base.ActiveMdiChild).saveAs == null)
		{
			saveFileDialog1.Filter = "LindRoof Files (*.lnd) |*.lnd";
			saveFileDialog1.Title = "Save LindRoof File";
			saveFileDialog1.ShowDialog();
			return;
		}
		try
		{
			StreamWriter sw = new StreamWriter(((Document)base.ActiveMdiChild).saveAs);
			SaveFile(sw);
		}
		catch
		{
			MessageBox.Show(dictionar.dictionar[95], dictionar.dictionar[96], MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
	{
		StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);
		((Document)base.ActiveMdiChild).saveAs = saveFileDialog1.FileName;
		SaveFile(sw);
	}

	private void SaveFile(StreamWriter sw)
	{
		sw.WriteLine("Fisier date LindRoof");
		sw.WriteLine("GraphicObjects:");
		foreach (GraphicObject @object in ((Document)base.ActiveMdiChild).graphicTable.objects)
		{
			sw.WriteLine(@object.objectIndex + " " + @object.ToString() + " " + @object.layer.nameOfLayer + @object.startPoint.ToString(CultureInfo.CurrentUICulture) + @object.stopPoint.ToString(CultureInfo.CurrentUICulture));
		}
		sw.WriteLine("Panels in GraphicTable:");
		foreach (Panel panel3 in ((Document)base.ActiveMdiChild).graphicTable.panels)
		{
			sw.WriteLine("RoofSlope no." + (((Document)base.ActiveMdiChild).graphicTable.panels.IndexOf(panel3) + 1) + " ");
			foreach (GraphicObject object2 in panel3.objects)
			{
				sw.WriteLine(object2.objectIndex + " " + object2.ToString() + " " + object2.layer.nameOfLayer + object2.startPoint.ToString(CultureInfo.CurrentUICulture) + object2.stopPoint.ToString(CultureInfo.CurrentUICulture));
			}
		}
		sw.WriteLine("Panels in GraphicTableForPanels:");
		foreach (Panel item in ((Document)base.ActiveMdiChild).graphicTable.panelsForDeveloping)
		{
			if (item.evolved)
			{
				sw.WriteLine("RoofSlope no." + (((Document)base.ActiveMdiChild).graphicTable.panelsForDeveloping.IndexOf(item) + 1).ToString() + " " + item.evolved.ToString() + " " + item.bendingObject.objectIndex);
			}
			else
			{
				sw.WriteLine("RoofSlope no." + (((Document)base.ActiveMdiChild).graphicTable.panelsForDeveloping.IndexOf(item) + 1) + " " + item.evolved);
			}
			foreach (GraphicObject object3 in item.objects)
			{
				sw.WriteLine(object3.objectIndex + " " + object3.ToString() + " " + object3.layer.nameOfLayer + object3.startPoint.ToString(CultureInfo.CurrentUICulture) + object3.stopPoint.ToString(CultureInfo.CurrentUICulture));
			}
			if (!item.evolved)
			{
				continue;
			}
			int num = 0;
			string text = "";
			if (item.panelingObjects.Count != 0)
			{
				text = ":SheetType=" + item.tipPanou.tipTigla + ":" + item.tipPanou.pasOndula + ":" + item.tipPanou.latimeFoaie + ":" + item.tipPanou.petrecereFoi + ":" + item.tipPanou.streasina + ":" + item.tipPanou.nrMaximOndule + ":" + item.tipPanou.offsetMozaic + ":" + item.tipPanou.optimizareStandard + ":" + item.tipPanou.optimizareAjustabile + ":" + item.tipPanou.variatorPasOndula + ":" + item.tipPanou.nrMinimOndule + ":" + item.tipPanou.nrMaximOndule;
			}
			sw.WriteLine("No.of sheets:" + item.panelingObjects.Count + text);
			foreach (PanelObject panelingObject in item.panelingObjects)
			{
				num++;
				sw.WriteLine("Sheet " + num + panelingObject.startPoint.ToString(CultureInfo.CurrentUICulture) + " " + panelingObject.width.ToString(CultureInfo.CurrentUICulture) + " x " + panelingObject.height.ToString(CultureInfo.CurrentUICulture));
			}
			num = 0;
			sw.WriteLine("No.of voids:" + item.voidList.Count);
			foreach (voids @void in item.voidList)
			{
				num++;
				sw.WriteLine("Void " + num + " " + @void.name + @void.startPoint.ToString(CultureInfo.CurrentUICulture) + " " + @void.width.ToString(CultureInfo.CurrentUICulture) + " cu " + @void.width2.ToString(CultureInfo.CurrentUICulture) + " x " + @void.heigh.ToString(CultureInfo.CurrentUICulture) + " Triangle: " + @void.trapezoidal);
			}
		}
		sw.WriteLine("EndOfFile");
		sw.Close();
		MessageBox.Show(((Document)base.ActiveMdiChild).saveAs, dictionar.dictionar[97], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		((Document)base.ActiveMdiChild).Text = ((Document)base.ActiveMdiChild).saveAs;
	}

	private void menuItem3_Click(object sender, EventArgs e)
	{
		openFileDialog1.Filter = "LindRoof Files (*.lnd) |*.lnd";
		openFileDialog1.Title = "Open LindRoof File";
		openFileDialog1.ShowDialog();
	}

	private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
	{
		StreamReader streamReader = new StreamReader(openFileDialog1.FileName);
		OpenFile(streamReader, openFileDialog1.FileName);
		streamReader.Close();
		((Document)base.ActiveMdiChild).graphicTable.ZoomExtents();
		((Document)base.ActiveMdiChild).graphicTable.RedrawAll();
	}

	private void OpenFile(StreamReader sw, string fileName)
	{
		if (sw.ReadLine() == "Fisier date LindRoof")
		{
			try
			{
				Document document = ((base.MdiChildren.Length == 0) ? new Document(this) : (((Document)base.ActiveMdiChild == null) ? new Document(this) : ((((Document)base.ActiveMdiChild).graphicTable.objects.Count == 0 || ((Document)base.ActiveMdiChild).graphicTable.panels.Count == 0) ? ((Document)base.ActiveMdiChild) : new Document(this))));
				SettingUCSvisibility(document);
				document.Text = fileName;
				document.MdiParent = this;
				AllowDocumentOperations(val: true);
				document.Show();
				document.graphicTable.base_point_x = -1 * document.Width / 2;
				document.graphicTable.base_point_y = document.Height / 2;
				string text = "";
				text = sw.ReadLine();
				if (text == "GraphicObjects:")
				{
					text = sw.ReadLine();
					while (text != "Panels in GraphicTable:" && text != "EndOfFile")
					{
						Layer lay = document.graphicTable.currentLayer;
						string[] array = text.Split(new char[1] { ' ' }, 7);
						int index = int.Parse(array[0]);
						string text2 = array[1];
						string text3 = array[2];
						array[3] = array[3].Substring(3);
						double num = double.Parse(array[3], NumberStyles.Float, CultureInfo.CurrentUICulture);
						array[4] = array[4].Substring(2, array[4].Length - 3);
						double num2 = double.Parse(array[4], NumberStyles.Float, CultureInfo.CurrentUICulture);
						array[5] = array[5].Substring(3);
						double num3 = double.Parse(array[5], NumberStyles.Float, CultureInfo.CurrentUICulture);
						array[6] = array[6].Substring(2, array[6].Length - 3);
						double num4 = double.Parse(array[6], NumberStyles.Float, CultureInfo.CurrentUICulture);
						foreach (Layer layer4 in document.layers)
						{
							if (layer4.nameOfLayer == text3)
							{
								lay = layer4;
								break;
							}
						}
						if (text2 == "LindRoof.line")
						{
							document.graphicTable.AddObject(new line(new PointD(num, num2), new PointD(num3, num4), lay, index));
						}
						text = sw.ReadLine();
					}
					text = sw.ReadLine();
					while (text != "Panels in GraphicTableForPanels:" && text != "EndOfFile")
					{
						while (text.Substring(0, 6) == "RoofSl")
						{
							Panel panel = new Panel(document.graphicTable.currentLayer, dictionar.dictionar[109] + text.Substring(13));
							document.graphicTable.AddPanel(panel);
							document.graphicTable.panelNo++;
							int panelNo = document.graphicTable.panelNo;
							document.graphicTable.currentPanel = new Panel(document.graphicTable.currentLayer, dictionar.dictionar[109] + panelNo);
							text = sw.ReadLine();
							while (text.Substring(0, 6) != "RoofSl" && text != "Panels in GraphicTableForPanels:" && text != "EndOfFile")
							{
								Layer lay2 = document.graphicTable.currentLayer;
								string[] array2 = text.Split(new char[1] { ' ' }, 7);
								int index2 = int.Parse(array2[0]);
								string text4 = array2[1];
								string text5 = array2[2];
								array2[3] = array2[3].Substring(3);
								double num5 = double.Parse(array2[3], NumberStyles.Float, CultureInfo.CurrentUICulture);
								array2[4] = array2[4].Substring(2, array2[4].Length - 3);
								double num6 = double.Parse(array2[4], NumberStyles.Float, CultureInfo.CurrentUICulture);
								array2[5] = array2[5].Substring(3);
								double num7 = double.Parse(array2[5], NumberStyles.Float, CultureInfo.CurrentUICulture);
								array2[6] = array2[6].Substring(2, array2[6].Length - 3);
								double num8 = double.Parse(array2[6], NumberStyles.Float, CultureInfo.CurrentUICulture);
								foreach (Layer layer5 in document.layers)
								{
									if (layer5.nameOfLayer == text5)
									{
										lay2 = layer5;
										break;
									}
								}
								if (text4 == "LindRoof.line")
								{
									panel.Add(new line(new PointD(num5, num6), new PointD(num7, num8), lay2, index2));
								}
								text = sw.ReadLine();
							}
						}
					}
					text = sw.ReadLine();
					document.graphicTable.panelsForDeveloping.Clear();
					document.graphicTableForPanels.panels.Clear();
					while (text != "EndOfFile")
					{
						while (text.Substring(0, 6) == "RoofSl")
						{
							string[] array3 = text.Split(new char[1] { ' ' }, 4);
							bool flag = bool.Parse(array3[2]);
							int num9 = -1;
							if (flag)
							{
								num9 = int.Parse(array3[3]);
							}
							Panel panel2 = new Panel(document.graphicTableForPanels.currentLayer, dictionar.dictionar[109] + array3[1].Substring(3));
							panel2.evolved = flag;
							text = sw.ReadLine();
							while (text.Substring(0, 5) != "No.of" && text.Substring(0, 6) != "RoofSl" && text != "EndOfFile")
							{
								Layer lay3 = document.graphicTableForPanels.currentLayer;
								string[] array4 = text.Split(new char[1] { ' ' }, 8);
								int num10 = int.Parse(array4[0]);
								string text6 = array4[1];
								string text7 = array4[2];
								array4[3] = array4[3].Substring(3);
								double num11 = double.Parse(array4[3], NumberStyles.Float, CultureInfo.CurrentUICulture);
								array4[4] = array4[4].Substring(2, array4[4].Length - 3);
								double num12 = double.Parse(array4[4], NumberStyles.Float, CultureInfo.CurrentUICulture);
								array4[5] = array4[5].Substring(3);
								double num13 = double.Parse(array4[5], NumberStyles.Float, CultureInfo.CurrentUICulture);
								array4[6] = array4[6].Substring(2, array4[6].Length - 3);
								double num14 = double.Parse(array4[6], NumberStyles.Float, CultureInfo.CurrentUICulture);
								foreach (Layer layer6 in document.layers)
								{
									if (layer6.nameOfLayer == text7)
									{
										lay3 = layer6;
										break;
									}
								}
								if (text6 == "LindRoof.line")
								{
									line line2 = new line(new PointD(num11, num12), new PointD(num13, num14), lay3, num10);
									panel2.Add(line2);
									if (flag && num10 == num9)
									{
										panel2.bendingObject = line2;
									}
								}
								text = sw.ReadLine();
							}
							while (text.Substring(0, 9) != "No.of voi" && text != "EndOfFile" && text.Substring(0, 6) != "RoofSl")
							{
								string[] array5 = text.Split(':');
								if (int.Parse(array5[1]) == 0)
								{
									text = sw.ReadLine();
									break;
								}
								CaracteristiciPanou caracteristiciPanou = new CaracteristiciPanou(nimic: true);
								if (array5.Length > 1)
								{
									try
									{
										caracteristiciPanou.tipTigla = array5[2].Split('=')[1];
										caracteristiciPanou.pasOndula = double.Parse(array5[3]);
										caracteristiciPanou.latimeFoaie = double.Parse(array5[4]);
										caracteristiciPanou.petrecereFoi = double.Parse(array5[5]);
										caracteristiciPanou.streasina = double.Parse(array5[6]);
										caracteristiciPanou.nrMaximOndule = int.Parse(array5[7]);
										caracteristiciPanou.offsetMozaic = double.Parse(array5[8]);
										caracteristiciPanou.optimizareStandard = bool.Parse(array5[9]);
										caracteristiciPanou.optimizareAjustabile = bool.Parse(array5[10]);
										caracteristiciPanou.variatorPasOndula = bool.Parse(array5[11]);
										caracteristiciPanou.nrMinimOndule = int.Parse(array5[12]);
										caracteristiciPanou.nrMaximOndule = int.Parse(array5[13]);
										caracteristiciPanou.observatii = "";
									}
									catch
									{
									}
								}
								text = sw.ReadLine();
								while (text.Substring(0, 5) == "Sheet")
								{
									PointD pointD = new PointD();
									PointD pointD2 = new PointD();
									string[] array6 = text.Split(new char[1] { ' ' }, 7);
									double num15 = double.Parse(array6[2].Substring(3), CultureInfo.CurrentUICulture);
									double num16 = double.Parse(array6[3].Substring(2, array6[3].Length - 3), CultureInfo.CurrentUICulture);
									double num17 = double.Parse(array6[4], CultureInfo.CurrentUICulture);
									double num18 = double.Parse(array6[6], CultureInfo.CurrentUICulture);
									pointD.X = num15;
									pointD.Y = num16;
									pointD2.X = num15 + num17;
									pointD2.Y = num16 + num18;
									PanelObject panelObject = new PanelObject(caracteristiciPanou, ptOptimizare: false, pointD, pointD2, 0.0, 0);
									panel2.panelingObjects.Add(panelObject);
									if (panel2.tipPanou.latimeFoaie == 0.0)
									{
										panel2.tipPanou = caracteristiciPanou;
										panel2.lungimeMinimaFoaie = (double)panel2.tipPanou.nrMinimOndule * panel2.tipPanou.pasOndula + panel2.tipPanou.petrecereFoi;
										panel2.lungimeMinimaFoaieModulRandom = (double)panel2.tipPanou.nrMinimOndule * panel2.tipPanou.pasOndula + panel2.tipPanou.petrecereFoi;
										panel2.lungimeMaximaFoaie = (double)panel2.tipPanou.nrMaximOndule * panel2.tipPanou.pasOndula + panel2.tipPanou.petrecereFoi;
									}
									panelObject.UpdateLines();
									text = sw.ReadLine();
								}
							}
							while (text.Substring(0, 6) != "RoofSl" && text != "EndOfFile")
							{
								if (int.Parse(text.Split(':')[1]) != 0)
								{
									text = sw.ReadLine();
									bool flag2 = false;
									while (text.Substring(0, 4) == "Void")
									{
										PointD pointD3 = new PointD();
										string[] array7 = text.Split(new char[1] { ' ' }, 12);
										string nam = array7[2];
										double num19 = double.Parse(array7[3].Substring(3), CultureInfo.CurrentUICulture);
										double num20 = double.Parse(array7[4].Substring(2, array7[4].Length - 3), CultureInfo.CurrentUICulture);
										double num21 = double.Parse(array7[5], CultureInfo.CurrentUICulture);
										double wid = double.Parse(array7[7], CultureInfo.CurrentUICulture);
										double num22 = double.Parse(array7[9], CultureInfo.CurrentUICulture);
										pointD3.X = num19 + num21 / 2.0;
										pointD3.Y = num20 - num22 / 2.0;
										flag2 = array7[11] == "True";
										voids value = new voids(nam, pointD3, num21, wid, num22, flag2);
										panel2.voidList.Add(value);
										text = sw.ReadLine();
									}
								}
								else if (text == "No.of voids:0")
								{
									text = sw.ReadLine();
								}
							}
							document.graphicTable.panelsForDeveloping.Add(panel2);
							document.graphicTableForPanels.panels.Add(panel2);
						}
					}
				}
				foreach (GraphicObject @object in document.graphicTable.objects)
				{
					if (document.graphicTable.objectIndex <= @object.objectIndex)
					{
						document.graphicTable.objectIndex = @object.objectIndex + 1;
					}
				}
				document.saveAs = openFileDialog1.FileName;
				return;
			}
			catch
			{
				MessageBox.Show(this, dictionar.dictionar[98] + openFileDialog1.FileName + dictionar.dictionar[99], dictionar.dictionar[100], MessageBoxButtons.OK, MessageBoxIcon.Hand);
				((Document)base.ActiveMdiChild).Dispose();
				return;
			}
		}
		MessageBox.Show(this, dictionar.dictionar[101], dictionar.dictionar[102], MessageBoxButtons.OK, MessageBoxIcon.Hand);
	}

	private void menuFileSaveAs_Click(object sender, EventArgs e)
	{
		if (base.MdiChildren.Length != 0 && ((Document)base.ActiveMdiChild).graphicTable.objects != null && ((Document)base.ActiveMdiChild).graphicTable.objects.Count != 0)
		{
			saveFileDialog1.Filter = "LindRoof Files (*.lnd) |*.lnd";
			saveFileDialog1.Title = "Save LindRoof File";
			saveFileDialog1.ShowDialog();
		}
	}

	private void menuHelpInstructions_Click(object sender, EventArgs e)
	{
		Help help = new Help();
		help.Visible = true;
	}

	private void menuFilePrinterSettings_Click(object sender, EventArgs e)
	{
		if (base.MdiChildren.Length != 0)
		{
			if (((Document)base.ActiveMdiChild).graphicTable.Visible)
			{
				((Document)base.ActiveMdiChild).graphicTable.PrinterSettings();
			}
			else if (((Document)base.ActiveMdiChild).graphicTableForPanels.Visible)
			{
				((Document)base.ActiveMdiChild).graphicTableForPanels.PrinterSettings();
			}
		}
	}

	private void menuFilePrintPreview_Click(object sender, EventArgs e)
	{
		if (base.MdiChildren.Length != 0)
		{
			if (((Document)base.ActiveMdiChild).graphicTable.Visible)
			{
				((Document)base.ActiveMdiChild).graphicTable.PrintPreview();
			}
			else if (((Document)base.ActiveMdiChild).graphicTableForPanels.Visible)
			{
				((Document)base.ActiveMdiChild).graphicTableForPanels.PrintPreview();
			}
		}
	}

	private void menuFilePrint_Click(object sender, EventArgs e)
	{
		if (base.MdiChildren.Length != 0)
		{
			if (((Document)base.ActiveMdiChild).graphicTable.Visible)
			{
				((Document)base.ActiveMdiChild).graphicTable.Print();
			}
			else if (((Document)base.ActiveMdiChild).graphicTableForPanels.Visible)
			{
				((Document)base.ActiveMdiChild).graphicTableForPanels.Print();
			}
		}
	}

	private void menuSettingsColorBgrnd_Click(object sender, EventArgs e)
	{
		colorDialog1.ShowDialog();
		((Document)base.ActiveMdiChild).graphicTable.background = colorDialog1.Color;
		((Document)base.ActiveMdiChild).graphicTable.RedrawAll();
		((Document)base.ActiveMdiChild).graphicTableForPanels.background = colorDialog1.Color;
		((Document)base.ActiveMdiChild).graphicTableForPanels.RedrawAll();
	}

	private void menuSettingsColorSlopes_Click(object sender, EventArgs e)
	{
		colorDialog1.ShowDialog();
		((Document)base.ActiveMdiChild).graphicTable.culoarePanouri = new SolidBrush(colorDialog1.Color);
		((Document)base.ActiveMdiChild).graphicTable.RedrawAll();
	}

	private void menuSettingsLanguage_Click(object sender, EventArgs e)
	{
		string text = ((MenuItem)sender).Text;
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\LindSoft\\LINDROOF", writable: true);
		if (registryKey == null)
		{
			registryKey = Registry.CurrentUser.CreateSubKey("Software\\LindSoft\\LINDROOF");
		}
		registryKey.SetValue("Language", text, RegistryValueKind.String);
		dictionar_cuvinte dictionar_cuvinte2 = new dictionar_cuvinte(text);
		MessageBox.Show(dictionar_cuvinte2.dictionar[126]);
	}

	private void menuSettingsLAFarea_Click(object sender, EventArgs e)
	{
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\LindSoft\\LINDROOF", writable: true);
		if (registryKey == null)
		{
			registryKey = Registry.CurrentUser.CreateSubKey("Software\\LindSoft\\LINDROOF");
		}
		VariousWindows1 variousWindows = new VariousWindows1();
		variousWindows.Text = dictionar.dictionar[296];
		variousWindows.textBox1.Text = LAFarea.ToString();
		variousWindows.label1.Text = dictionar.dictionar[173];
		if (variousWindows.ShowDialog() != DialogResult.Cancel)
		{
			return;
		}
		try
		{
			double num = double.Parse(variousWindows.textBox1.Text, CultureInfo.CurrentUICulture);
			if (num >= 0.0)
			{
				LAFarea = num;
				registryKey.SetValue("LAFarea", LAFarea.ToString(), RegistryValueKind.String);
			}
			else
			{
				MessageBox.Show(dictionar.dictionar[297]);
			}
		}
		catch
		{
			MessageBox.Show(dictionar.dictionar[297]);
		}
	}

	public void SaveChild()
	{
		if (base.MdiChildren.Length == 0 || ((Document)base.ActiveMdiChild).graphicTable.objects == null || ((Document)base.ActiveMdiChild).graphicTable.objects.Count == 0)
		{
			return;
		}
		if (((Document)base.ActiveMdiChild).saveAs == null)
		{
			saveFileDialog1.Filter = "LindRoof Files (*.lnd) |*.lnd";
			saveFileDialog1.Title = "Save LindRoof File";
			saveFileDialog1.ShowDialog();
			return;
		}
		try
		{
			StreamWriter sw = new StreamWriter(((Document)base.ActiveMdiChild).saveAs);
			SaveFile(sw);
		}
		catch
		{
			MessageBox.Show(dictionar.dictionar[103], dictionar.dictionar[96], MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void LoadCustomCursors()
	{
		string resource = "GraphicEnvironment.Rectangle.cur";
		selectionCursor = new Cursor(GetType(), resource);
	}

	public void UpdateGridSize(double gridDistance)
	{
		numInfo = culture.NumberFormat;
		numInfo.NumberDecimalDigits = 3;
		statusBarGridSize2.Text = gridDistance.ToString("N", numInfo) + dictionar.dictionar[104];
	}

	public void UpdateCoords(double x_drawing_in_cm, double y_drawing_in_cm, string unit)
	{
		numInfo = culture.NumberFormat;
		numInfo.NumberDecimalDigits = 3;
		if (!(unit == dictionar.dictionar[104]))
		{
			if (unit == dictionar.dictionar[105])
			{
				x_drawing_in_cm /= 100.0;
				y_drawing_in_cm /= 100.0;
			}
			else if (unit == dictionar.dictionar[106])
			{
				x_drawing_in_cm *= 10.0;
				y_drawing_in_cm *= 10.0;
			}
		}
		statusBarPosition.Text = x_drawing_in_cm.ToString("N", numInfo) + "; " + y_drawing_in_cm.ToString("N", numInfo) + " " + unit;
	}

	public void WriteCurrentObjectLength(double length_in_cm, string unit)
	{
		if (!(unit == dictionar.dictionar[104]))
		{
			if (unit == dictionar.dictionar[105])
			{
				length_in_cm /= 100.0;
			}
			else if (unit == dictionar.dictionar[106])
			{
				length_in_cm *= 10.0;
			}
		}
		statusBarLength2.Text = length_in_cm.ToString("N") + " " + unit;
	}

	public void AllowDocumentOperations(bool val)
	{
		menuFileSave.Enabled = val;
		menuFilePrint.Enabled = val;
		menuFilePrintPreview.Enabled = val;
		menuFilePrinterSettings.Enabled = val;
		menuFileClose.Enabled = val;
		menuEditCut.Enabled = val;
		menuEditCopy.Enabled = val;
		menuEditPaste.Enabled = val;
		menuEditUndo.Enabled = val;
		menuEditRedo.Enabled = val;
		menuEditFind.Enabled = val;
		menuWindowClose.Enabled = val;
		menuWindowCloseAll.Enabled = val;
		menuWindowArrangeWindows.Enabled = val;
		consoleWindow.Visible = val;
	}

	public void OnChildClosing(Document doc)
	{
		AllowDocumentOperations(base.MdiChildren.Length > 1);
	}

	private void SettingUCSvisibility(Document doc)
	{
		if (menuSettingsUCS.Text == showucs)
		{
			doc.DrawOrigin(b: false);
		}
		else if (menuSettingsUCS.Text == hideucs)
		{
			doc.DrawOrigin(b: true);
		}
	}

	public void DeletePanel(string selectedPanel)
	{
		foreach (Panel panel4 in ((Document)base.ActiveMdiChild).graphicTable.panels)
		{
			if (selectedPanel == panel4.panelName)
			{
				((Document)base.ActiveMdiChild).graphicTable.panels.Remove(panel4);
				panel4.Dispose();
				((Document)base.ActiveMdiChild).graphicTable.RedrawAll();
				break;
			}
		}
		foreach (Panel item in ((Document)base.ActiveMdiChild).graphicTable.panelsForDeveloping)
		{
			if (selectedPanel.Trim() == item.panelName)
			{
				((Document)base.ActiveMdiChild).graphicTable.panelsForDeveloping.Remove(item);
				item.Dispose();
				((Document)base.ActiveMdiChild).graphicTableForPanels.RedrawAll();
				break;
			}
		}
		foreach (Panel panel5 in ((Document)base.ActiveMdiChild).graphicTableForPanels.panels)
		{
			if (selectedPanel.Trim() == panel5.panelName)
			{
				((Document)base.ActiveMdiChild).graphicTableForPanels.panels.Remove(panel5);
				panel5.Dispose();
				((Document)base.ActiveMdiChild).graphicTableForPanels.RedrawAll();
				break;
			}
		}
	}
}
