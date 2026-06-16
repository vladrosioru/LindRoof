using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Windows.Forms;
using LindRoof.TextualEnvironment;

namespace LindRoof;

public class Document : Form
{
	public string saveItOrNot = "No";

	private Container components;

	public bool enableCommand;

	public RichTextBox commandHistoryText;

	public string commandLineText = "";

	public GraphicTable graphicTable;

	public GraphicTableForPanels graphicTableForPanels;

	public int activeCommand;

	private TabControl tabControl;

	private TabPage tabSketch2;

	private TabPage tabPanels3;

	public ComboBox panelList;

	public ComboBox layersList;

	public int lastCommand;

	public ArrayList layers = new ArrayList();

	private int tabControlButton;

	public TextBox angleDeclaration;

	private TextBox currentPanelArea;

	private Label label1;

	public TextBox currentSheetsArea;

	private Label label2;

	public GraphicObject bendingObject;

	private Layer StreasinaLayer;

	private Layer CoamaLayer;

	private Layer FrontonLayer;

	private Layer DolieLayer;

	private Layer BorduraCalcanLayer;

	private Layer CoamaInclinataLayer;

	private Layer SemiCoamaLayer;

	private Layer FrontonInclinatLayer;

	private Layer RuperePantaConcavaLayer;

	private Layer RuperePantaConvexaLayer;

	private Layer RacordLateralLayer;

	private Layer FrontonEvazatLayer;

	private Layer DefaultLayer;

	private string[] arr;

	private Image[] imageArr;

	public ComboBox layersList1;

	public Font myFont;

	private RadioButton inputBentBySlope;

	private RadioButton inputBentByAngle;

	public GroupBox inputSlope;

	private IEnumerator panelsEnumerator;

	public string saveAs;

	public SaveFileDialog saveFileDialog1;

	public bool needToBeSaved;

	private dictionar_cuvinte dictionar;

	public MainWindow mainwindow;

	public Document(MainWindow mainwnd)
	{
		mainwindow = mainwnd;
		dictionar = mainwindow.dictionar;
		InitializeComponent(mainwindow);
		myFont = new Font("Arial", 10f);
		arr = new string[13];
		arr[0] = dictionar.dictionar[151];
		arr[1] = dictionar.dictionar[152];
		arr[2] = dictionar.dictionar[153];
		arr[3] = dictionar.dictionar[154];
		arr[4] = dictionar.dictionar[155];
		arr[5] = dictionar.dictionar[156];
		arr[6] = dictionar.dictionar[157];
		arr[7] = dictionar.dictionar[158];
		arr[8] = dictionar.dictionar[159];
		arr[9] = dictionar.dictionar[339];
		arr[10] = dictionar.dictionar[340];
		arr[11] = dictionar.dictionar[341];
		arr[12] = dictionar.dictionar[160];
		layersList.DataSource = arr;
		layersList1.DataSource = arr;
		imageArr = new Image[13];
		string name = "LindRoof.GraphicEnvironment.green.bmp";
		Stream stream = null;
		stream = GetType().Assembly.GetManifestResourceStream(name);
		imageArr[0] = new Bitmap(stream);
		name = "LindRoof.GraphicEnvironment.yellow.bmp";
		stream = GetType().Assembly.GetManifestResourceStream(name);
		imageArr[1] = new Bitmap(stream);
		name = "LindRoof.GraphicEnvironment.red.bmp";
		stream = GetType().Assembly.GetManifestResourceStream(name);
		imageArr[2] = new Bitmap(stream);
		name = "LindRoof.GraphicEnvironment.blue.bmp";
		stream = GetType().Assembly.GetManifestResourceStream(name);
		imageArr[3] = new Bitmap(stream);
		name = "LindRoof.GraphicEnvironment.orange.bmp";
		stream = GetType().Assembly.GetManifestResourceStream(name);
		imageArr[4] = new Bitmap(stream);
		name = "LindRoof.GraphicEnvironment.pink.bmp";
		stream = GetType().Assembly.GetManifestResourceStream(name);
		imageArr[5] = new Bitmap(stream);
		name = "LindRoof.GraphicEnvironment.brown.bmp";
		stream = GetType().Assembly.GetManifestResourceStream(name);
		imageArr[6] = new Bitmap(stream);
		name = "LindRoof.GraphicEnvironment.magenta.bmp";
		stream = GetType().Assembly.GetManifestResourceStream(name);
		imageArr[7] = new Bitmap(stream);
		name = "LindRoof.GraphicEnvironment.bbb.bmp";
		stream = GetType().Assembly.GetManifestResourceStream(name);
		imageArr[8] = new Bitmap(stream);
		name = "LindRoof.GraphicEnvironment.lightorange.bmp";
		stream = GetType().Assembly.GetManifestResourceStream(name);
		imageArr[9] = new Bitmap(stream);
		name = "LindRoof.GraphicEnvironment.violet.bmp";
		stream = GetType().Assembly.GetManifestResourceStream(name);
		imageArr[10] = new Bitmap(stream);
		name = "LindRoof.GraphicEnvironment.darkred.bmp";
		stream = GetType().Assembly.GetManifestResourceStream(name);
		imageArr[11] = new Bitmap(stream);
		name = "LindRoof.GraphicEnvironment.gray.bmp";
		stream = GetType().Assembly.GetManifestResourceStream(name);
		imageArr[12] = new Bitmap(stream);
		StreasinaLayer = new Layer("Streasina", Color.Green);
		CoamaLayer = new Layer("Coama", Color.Yellow);
		FrontonLayer = new Layer("Fronton", Color.Red);
		DolieLayer = new Layer("Dolie", Color.Blue);
		BorduraCalcanLayer = new Layer("BorduraCalcan", Color.Orange);
		CoamaInclinataLayer = new Layer("CoamaInclinata", Color.DeepPink);
		SemiCoamaLayer = new Layer("SemiCoama", Color.Brown);
		FrontonInclinatLayer = new Layer("FrontonInclinat", Color.Magenta);
		RuperePantaConcavaLayer = new Layer("RuperePantaCcv", Color.DarkBlue);
		RuperePantaConvexaLayer = new Layer("RuperePantaCvx", Color.OrangeRed);
		RacordLateralLayer = new Layer("RacordLateral", Color.Violet);
		FrontonEvazatLayer = new Layer("FrontonEvazat", Color.DarkRed);
		DefaultLayer = new Layer("Default", Color.DarkGray);
		layers.Add(StreasinaLayer);
		layers.Add(CoamaLayer);
		layers.Add(FrontonLayer);
		layers.Add(DolieLayer);
		layers.Add(BorduraCalcanLayer);
		layers.Add(CoamaInclinataLayer);
		layers.Add(SemiCoamaLayer);
		layers.Add(FrontonInclinatLayer);
		layers.Add(RuperePantaConcavaLayer);
		layers.Add(RuperePantaConvexaLayer);
		layers.Add(RacordLateralLayer);
		layers.Add(FrontonEvazatLayer);
		layers.Add(DefaultLayer);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			base.Dispose(true);
		}
	}

	public void CloseDocument()
	{
		if (graphicTable.objects.Count != 0)
		{
			SaveDocument saveDocument = new SaveDocument(dictionar.dictionar[161] + Text + " ?", this);
			saveDocument.ShowDialog(this);
			switch (saveItOrNot)
			{
			case "Yes":
				mainwindow.SaveChild();
				GC.SuppressFinalize(this);
				Dispose(disposing: true);
				break;
			case "No":
				GC.SuppressFinalize(this);
				Dispose(disposing: true);
				break;
			}
		}
		else
		{
			Dispose(disposing: true);
		}
	}

	private void InitializeComponent(LindRoof.MainWindow mainwindow)
	{
		System.Resources.ResourceManager resourceManager = new System.Resources.ResourceManager(typeof(LindRoof.Document));
		this.commandHistoryText = new System.Windows.Forms.RichTextBox();
		this.graphicTable = new LindRoof.GraphicTable(mainwindow);
		this.tabControl = new System.Windows.Forms.TabControl();
		this.tabSketch2 = new System.Windows.Forms.TabPage();
		this.layersList1 = new System.Windows.Forms.ComboBox();
		this.tabPanels3 = new System.Windows.Forms.TabPage();
		this.layersList = new System.Windows.Forms.ComboBox();
		this.label1 = new System.Windows.Forms.Label();
		this.currentPanelArea = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.currentSheetsArea = new System.Windows.Forms.TextBox();
		this.angleDeclaration = new System.Windows.Forms.TextBox();
		this.panelList = new System.Windows.Forms.ComboBox();
		this.graphicTableForPanels = new LindRoof.GraphicTableForPanels(this);
		this.inputBentBySlope = new System.Windows.Forms.RadioButton();
		this.inputBentByAngle = new System.Windows.Forms.RadioButton();
		this.inputSlope = new System.Windows.Forms.GroupBox();
		this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
		this.tabControl.SuspendLayout();
		this.tabSketch2.SuspendLayout();
		this.tabPanels3.SuspendLayout();
		this.inputSlope.SuspendLayout();
		base.SuspendLayout();
		this.commandHistoryText.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.commandHistoryText.CausesValidation = false;
		this.commandHistoryText.Enabled = false;
		this.commandHistoryText.Location = new System.Drawing.Point(0, 288);
		this.commandHistoryText.Name = "commandHistoryText";
		this.commandHistoryText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
		this.commandHistoryText.Size = new System.Drawing.Size(80, 24);
		this.commandHistoryText.TabIndex = 3;
		this.commandHistoryText.TabStop = false;
		this.commandHistoryText.Text = this.dictionar.dictionar[162];
		this.commandHistoryText.Visible = false;
		this.graphicTable.SuspendLayout();
		this.graphicTable.AmIdrawingTheOrigin = true;
		this.graphicTable.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.graphicTable.BackColor = System.Drawing.SystemColors.Desktop;
		this.graphicTable.Cursor = System.Windows.Forms.Cursors.Cross;
		this.graphicTable.ForeColor = System.Drawing.Color.Black;
		this.graphicTable.Location = new System.Drawing.Point(0, 0);
		this.graphicTable.Name = "graphicTable";
		this.graphicTable.Size = new System.Drawing.Size(576, 304);
		this.graphicTable.TabIndex = 1;
		this.graphicTable.TabStop = false;
		this.graphicTable.KeyPress += new System.Windows.Forms.KeyPressEventHandler(graphicTable_KeyPress);
		this.graphicTable.ResumeLayout();
		this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl.Controls.Add(this.tabSketch2);
		this.tabControl.Controls.Add(this.tabPanels3);
		this.tabControl.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
		this.tabControl.ItemSize = new System.Drawing.Size(110, 25);
		this.tabControl.Location = new System.Drawing.Point(0, 306);
		this.tabControl.Name = "tabControl";
		this.tabControl.SelectedIndex = 0;
		this.tabControl.Size = new System.Drawing.Size(576, 56);
		this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
		this.tabControl.TabIndex = 99;
		this.tabControl.TabStop = false;
		this.tabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(tabControl_DrawItem);
		this.tabControl.Enter += new System.EventHandler(tabControl_Enter);
		this.tabControl.KeyPress += new System.Windows.Forms.KeyPressEventHandler(tabControl_KeyPress);
		this.tabControl.KeyDown += new System.Windows.Forms.KeyEventHandler(tabControl_KeyDown);
		this.tabControl.Leave += new System.EventHandler(tabControl_Leave);
		this.tabControl.MouseWheel += new System.Windows.Forms.MouseEventHandler(tabControl_MouseWheel);
		this.tabControl.SelectedIndexChanged += new System.EventHandler(tabControl_SelectedIndexChanged);
		this.tabSketch2.Controls.Add(this.layersList1);
		this.tabSketch2.Location = new System.Drawing.Point(4, 4);
		this.tabSketch2.Name = "tabSketch2";
		this.tabSketch2.Size = new System.Drawing.Size(568, 23);
		this.tabSketch2.TabIndex = 0;
		this.tabSketch2.Text = this.dictionar.dictionar[163];
		this.layersList1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.layersList1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
		this.layersList1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.layersList1.Location = new System.Drawing.Point(0, 1);
		this.layersList1.Name = "layersList1";
		this.layersList1.Size = new System.Drawing.Size(220, 21);
		this.layersList1.TabIndex = 104;
		this.layersList1.TabStop = false;
		this.layersList1.DropDownClosed += new System.EventHandler(layersList1_DropDownClosed);
		this.layersList1.SelectedIndexChanged += new System.EventHandler(layersList1_DropDownClosed);
		this.layersList1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(layersList1_DrawItem);
		this.tabPanels3.Controls.Add(this.layersList);
		this.tabPanels3.Controls.Add(this.label1);
		this.tabPanels3.Controls.Add(this.currentPanelArea);
		this.tabPanels3.Controls.Add(this.label2);
		this.tabPanels3.Controls.Add(this.currentSheetsArea);
		this.tabPanels3.Controls.Add(this.angleDeclaration);
		this.tabPanels3.Controls.Add(this.panelList);
		this.tabPanels3.Location = new System.Drawing.Point(4, 4);
		this.tabPanels3.Name = "tabPanels3";
		this.tabPanels3.Size = new System.Drawing.Size(568, 23);
		this.tabPanels3.TabIndex = 1;
		this.tabPanels3.Text = this.dictionar.dictionar[164];
		this.layersList.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.layersList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
		this.layersList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.layersList.Location = new System.Drawing.Point(232, 1);
		this.layersList.Name = "layersList";
		this.layersList.Size = new System.Drawing.Size(220, 21);
		this.layersList.TabIndex = 103;
		this.layersList.TabStop = false;
		this.layersList.Visible = false;
		this.layersList.SelectedIndexChanged += new System.EventHandler(layersList_SelectedIndexChanged);
		this.layersList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(layersList_DrawItem);
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.label1.BackColor = System.Drawing.Color.Transparent;
		this.label1.ForeColor = System.Drawing.Color.Black;
		this.label1.Location = new System.Drawing.Point(432, 1);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(80, 24);
		this.label1.TabIndex = 102;
		this.label1.Text = this.dictionar.dictionar[165];
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.currentPanelArea.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.currentPanelArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.currentPanelArea.Location = new System.Drawing.Point(512, 1);
		this.currentPanelArea.Name = "currentPanelArea";
		this.currentPanelArea.ReadOnly = true;
		this.currentPanelArea.Size = new System.Drawing.Size(56, 20);
		this.currentPanelArea.TabIndex = 101;
		this.currentPanelArea.TabStop = false;
		this.currentPanelArea.Text = "";
		this.currentPanelArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.label2.BackColor = System.Drawing.Color.Transparent;
		this.label2.ForeColor = System.Drawing.Color.Black;
		this.label2.Location = new System.Drawing.Point(289, 1);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(80, 24);
		this.label2.TabIndex = 120;
		this.label2.Text = this.dictionar.dictionar[166];
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.currentSheetsArea.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.currentSheetsArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.currentSheetsArea.Location = new System.Drawing.Point(369, 1);
		this.currentSheetsArea.Name = "currentSheetsArea";
		this.currentSheetsArea.ReadOnly = true;
		this.currentSheetsArea.Size = new System.Drawing.Size(56, 20);
		this.currentSheetsArea.TabIndex = 101;
		this.currentSheetsArea.TabStop = false;
		this.currentSheetsArea.Text = "";
		this.currentSheetsArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.angleDeclaration.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.angleDeclaration.Enabled = false;
		this.angleDeclaration.Location = new System.Drawing.Point(152, 1);
		this.angleDeclaration.MaxLength = 5;
		this.angleDeclaration.Name = "angleDeclaration";
		this.angleDeclaration.Size = new System.Drawing.Size(72, 20);
		this.angleDeclaration.TabIndex = 100;
		this.angleDeclaration.TabStop = false;
		this.angleDeclaration.Text = "";
		this.angleDeclaration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.angleDeclaration.KeyPress += new System.Windows.Forms.KeyPressEventHandler(angleDeclaration_KeyPress);
		this.panelList.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.panelList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.panelList.ItemHeight = 13;
		this.panelList.Location = new System.Drawing.Point(0, 1);
		this.panelList.MaxDropDownItems = 20;
		this.panelList.Name = "panelList";
		this.panelList.Size = new System.Drawing.Size(152, 21);
		this.panelList.Sorted = true;
		this.panelList.TabIndex = 1;
		this.panelList.TabStop = false;
		this.panelList.SelectedValueChanged += new System.EventHandler(panelList_SelectedValueChanged);
		this.graphicTableForPanels.AmIdrawingTheOrigin = true;
		this.graphicTableForPanels.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.graphicTableForPanels.BackColor = System.Drawing.SystemColors.Desktop;
		this.graphicTableForPanels.Cursor = System.Windows.Forms.Cursors.Cross;
		this.graphicTableForPanels.Enabled = false;
		this.graphicTableForPanels.ForeColor = System.Drawing.SystemColors.ControlText;
		this.graphicTableForPanels.Location = new System.Drawing.Point(0, 0);
		this.graphicTableForPanels.Name = "graphicTableForPanels";
		this.graphicTableForPanels.Size = new System.Drawing.Size(576, 304);
		this.graphicTableForPanels.TabIndex = 1;
		this.graphicTableForPanels.TabStop = false;
		this.graphicTableForPanels.Visible = false;
		this.inputBentBySlope.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.inputBentBySlope.BackColor = System.Drawing.Color.Transparent;
		this.inputBentBySlope.Checked = true;
		this.inputBentBySlope.Location = new System.Drawing.Point(16, 32);
		this.inputBentBySlope.Name = "inputBentBySlope";
		this.inputBentBySlope.Size = new System.Drawing.Size(104, 16);
		this.inputBentBySlope.TabIndex = 106;
		this.inputBentBySlope.TabStop = true;
		this.inputBentBySlope.Text = this.dictionar.dictionar[167];
		this.inputBentByAngle.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.inputBentByAngle.BackColor = System.Drawing.Color.Transparent;
		this.inputBentByAngle.Location = new System.Drawing.Point(16, 56);
		this.inputBentByAngle.Name = "inputBentByAngle";
		this.inputBentByAngle.Size = new System.Drawing.Size(104, 16);
		this.inputBentByAngle.TabIndex = 107;
		this.inputBentByAngle.Text = this.dictionar.dictionar[168];
		this.inputSlope.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.inputSlope.BackColor = System.Drawing.Color.Transparent;
		this.inputSlope.Controls.Add(this.inputBentBySlope);
		this.inputSlope.Controls.Add(this.inputBentByAngle);
		this.inputSlope.Location = new System.Drawing.Point(152, -40);
		this.inputSlope.Name = "inputSlope";
		this.inputSlope.Size = new System.Drawing.Size(120, 80);
		this.inputSlope.TabIndex = 109;
		this.inputSlope.TabStop = false;
		this.inputSlope.Text = this.dictionar.dictionar[169];
		this.inputSlope.Visible = false;
		this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(saveFileDialog1_FileOk);
		this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
		this.BackColor = System.Drawing.Color.Black;
		base.ClientSize = new System.Drawing.Size(576, 362);
		base.Controls.Add(this.inputSlope);
		base.Controls.Add(this.tabControl);
		base.Controls.Add(this.commandHistoryText);
		base.Controls.Add(this.graphicTableForPanels);
		base.Controls.Add(this.graphicTable);
		this.ForeColor = System.Drawing.SystemColors.ActiveBorder;
		try
		{
			base.Icon = (System.Drawing.Icon)resourceManager.GetObject("$this.Icon");
		}
		catch
		{
			System.Windows.Forms.MessageBox.Show("Eroare la initializare Icon", "LindRoof", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Hand);
			System.Environment.Exit(0);
		}
		this.MinimumSize = new System.Drawing.Size(160, 88);
		base.Name = this.dictionar.dictionar[2];
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
		this.Text = this.dictionar.dictionar[2];
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Resize += new System.EventHandler(Document_Resize);
		base.Closing += new System.ComponentModel.CancelEventHandler(Document_Closing);
		base.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Document_KeyPress);
		base.Layout += new System.Windows.Forms.LayoutEventHandler(Document_Layout);
		base.VisibleChanged += new System.EventHandler(Document_VisibleChanged);
		base.Activated += new System.EventHandler(Document_Activated);
		base.Leave += new System.EventHandler(Document_Leave);
		base.Enter += new System.EventHandler(Document_Enter);
		base.Deactivate += new System.EventHandler(Document_Deactivate);
		this.tabControl.ResumeLayout(false);
		this.tabSketch2.ResumeLayout(false);
		this.tabPanels3.ResumeLayout(false);
		this.inputSlope.ResumeLayout(false);
		base.ResumeLayout(false);
	}

	private void Document_MouseMove(object sender, MouseEventArgs e)
	{
	}

	private void Document_Closing(object sender, CancelEventArgs e)
	{
		CloseDocument();
	}

	private void Document_Deactivate(object sender, EventArgs e)
	{
		graphicTable.Enabled = false;
		graphicTableForPanels.Enabled = false;
		tabControlButton = tabControl.SelectedIndex;
	}

	private void Document_Activated(object sender, EventArgs e)
	{
		SetTabControlButton(tabControlButton);
	}

	private void graphicTable_KeyPress(object sender, KeyPressEventArgs e)
	{
		mainwindow.consoleWindow.CommandLine.Focus();
		mainwindow.consoleWindow.PressedWithoutFocus(e);
	}

	private void Document_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (graphicTable.Visible && mainwindow.consoleWindow.Enabled)
		{
			mainwindow.consoleWindow.CommandLine.Focus();
			mainwindow.consoleWindow.PressedWithoutFocus(e);
		}
	}

	private void Document_Enter(object sender, EventArgs e)
	{
		if (tabControl.SelectedIndex == 0)
		{
			mainwindow.consoleWindow.Enabled = true;
			mainwindow.consoleWindow.SuspendLayout();
			mainwindow.consoleWindow.CommandHistory.Text = commandHistoryText.Text;
			mainwindow.consoleWindow.CommandHistory.AppendText("", scrollToEnd: true);
			mainwindow.consoleWindow.ResumeLayout(performLayout: true);
			mainwindow.consoleWindow.CommandLine.Text = commandLineText;
		}
		else
		{
			mainwindow.consoleWindow.Enabled = false;
		}
	}

	private void Document_Leave(object sender, EventArgs e)
	{
		if (mainwindow.consoleWindow.Enabled)
		{
			commandLineText = mainwindow.consoleWindow.CommandLine.Text;
		}
	}

	private void tabControl_Enter(object sender, EventArgs e)
	{
	}

	private void tabControl_Leave(object sender, EventArgs e)
	{
	}

	protected override bool ProcessDialogKey(Keys keyData)
	{
		if (graphicTable.Visible)
		{
			if (mainwindow.consoleWindow.Enabled)
			{
				mainwindow.consoleWindow.CheckForSpecialKeys(keyData);
			}
			return base.ProcessDialogKey(keyData);
		}
		if (!graphicTableForPanels.CheckKeys(keyData))
		{
			return base.ProcessDialogKey(keyData);
		}
		return true;
	}

	private void tabControl_DrawItem(object sender, DrawItemEventArgs e)
	{
		try
		{
			Font font;
			Brush brush;
			Brush brush2;
			if (e.Index == tabControl.SelectedIndex)
			{
				font = new Font(e.Font, FontStyle.Bold);
				font = new Font(e.Font, FontStyle.Bold);
				brush = new LinearGradientBrush(e.Bounds, Color.Blue, Color.LightBlue, LinearGradientMode.Vertical);
				brush2 = Brushes.White;
			}
			else
			{
				font = new Font(e.Font, FontStyle.Regular);
				brush = new SolidBrush(e.BackColor);
				brush2 = new SolidBrush(e.ForeColor);
			}
			string s = tabControl.TabPages[e.Index].Text;
			StringFormat stringFormat = new StringFormat();
			stringFormat.Alignment = StringAlignment.Center;
			stringFormat.LineAlignment = StringAlignment.Center;
			e.Graphics.FillRectangle(brush, e.Bounds);
			Rectangle bounds = e.Bounds;
			bounds = new Rectangle(bounds.X, bounds.Y + 3, bounds.Width, bounds.Height - 3);
			e.Graphics.DrawString(s, font, brush2, bounds, stringFormat);
			stringFormat.Dispose();
			if (e.Index == tabControl.SelectedIndex)
			{
				font.Dispose();
				brush.Dispose();
			}
			else
			{
				brush.Dispose();
				brush2.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message.ToString(), dictionar.dictionar[170], MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
	}

	private void Document_Resize(object sender, EventArgs e)
	{
	}

	private void Document_Layout(object sender, LayoutEventArgs e)
	{
	}

	private void Document_VisibleChanged(object sender, EventArgs e)
	{
	}

	private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
	{
		TabControl tabControl = (TabControl)sender;
		int selectedIndex = tabControl.SelectedIndex;
		SetTabControlButton(selectedIndex);
	}

	private void tabControl_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (graphicTable.Visible)
		{
			if (mainwindow.consoleWindow.Enabled)
			{
				mainwindow.consoleWindow.CommandLine.Focus();
				mainwindow.consoleWindow.PressedWithoutFocus(e);
			}
		}
		else
		{
			graphicTableForPanels.Focus();
		}
	}

	private void tabControl_MouseWheel(object sender, MouseEventArgs e)
	{
		if (graphicTable.Visible)
		{
			graphicTable.Focus();
		}
		else
		{
			graphicTableForPanels.Focus();
		}
	}

	private void angleDeclaration_KeyPress(object sender, KeyPressEventArgs e)
	{
		switch ((int)e.KeyChar)
		{
		case 13:
		case 32:
			try
			{
				string s = angleDeclaration.Text.Trim().ToLower();
				double num = double.Parse(s, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentUICulture);
				double ratio;
				if (inputBentBySlope.Checked)
				{
					if (num < 0.0)
					{
						throw new Exception();
					}
					ratio = Math.Cos(Math.Atan(num / 100.0));
				}
				else
				{
					if (num < 0.0 || num > 89.9)
					{
						throw new Exception();
					}
					ratio = Math.Cos(num * Math.PI / 180.0);
				}
				EvolveThePanel(ratio);
				graphicTableForPanels.Enabled = true;
				panelList.Enabled = true;
				bendingObject.isSelected = false;
				angleDeclaration.Enabled = false;
				inputSlope.Visible = false;
				break;
			}
			catch
			{
				string text = dictionar.dictionar[171];
				MessageBox.Show(this, text, dictionar.dictionar[172], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				angleDeclaration.ResetText();
				break;
			}
		case 27:
			angleDeclaration.ResetText();
			break;
		}
	}

	private void layersList_DrawItem(object sender, DrawItemEventArgs e)
	{
		e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds);
		e.Graphics.DrawString(arr[e.Index], myFont, Brushes.Black, new Point(imageArr[e.Index].Width * 2, e.Bounds.Y));
		e.Graphics.DrawImage(imageArr[e.Index], new Point(e.Bounds.X, e.Bounds.Y));
		if ((e.State & DrawItemState.Focus) == 0)
		{
			e.Graphics.FillRectangle(Brushes.White, e.Bounds);
			e.Graphics.DrawString(arr[e.Index], myFont, Brushes.Black, new Point(imageArr[e.Index].Width * 2, e.Bounds.Y));
			e.Graphics.DrawImage(imageArr[e.Index], new Point(e.Bounds.X, e.Bounds.Y));
		}
	}

	private void layersList_SelectedIndexChanged(object sender, EventArgs e)
	{
		foreach (GraphicObject @object in graphicTableForPanels.currentPanel.objects)
		{
			if (@object.isSelected)
			{
				switch (layersList.SelectedIndex)
				{
				case 0:
					@object.layer = StreasinaLayer;
					break;
				case 1:
					@object.layer = CoamaLayer;
					break;
				case 2:
					@object.layer = FrontonLayer;
					break;
				case 3:
					@object.layer = DolieLayer;
					break;
				case 4:
					@object.layer = BorduraCalcanLayer;
					break;
				case 5:
					@object.layer = CoamaInclinataLayer;
					break;
				case 6:
					@object.layer = SemiCoamaLayer;
					break;
				case 7:
					@object.layer = FrontonInclinatLayer;
					break;
				case 8:
					@object.layer = RuperePantaConcavaLayer;
					break;
				case 9:
					@object.layer = RuperePantaConvexaLayer;
					break;
				case 10:
					@object.layer = RacordLateralLayer;
					break;
				case 11:
					@object.layer = FrontonEvazatLayer;
					break;
				case 12:
					@object.layer = DefaultLayer;
					break;
				}
				@object.isSelected = false;
				graphicTableForPanels.RedrawAll();
			}
		}
	}

	private void layersList1_DrawItem(object sender, DrawItemEventArgs e)
	{
		e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds);
		e.Graphics.DrawString(arr[e.Index], myFont, Brushes.Black, new Point(imageArr[e.Index].Width * 2, e.Bounds.Y));
		e.Graphics.DrawImage(imageArr[e.Index], new Point(e.Bounds.X, e.Bounds.Y));
		if ((e.State & DrawItemState.Focus) == 0)
		{
			e.Graphics.FillRectangle(Brushes.White, e.Bounds);
			e.Graphics.DrawString(arr[e.Index], myFont, Brushes.Black, new Point(imageArr[e.Index].Width * 2, e.Bounds.Y));
			e.Graphics.DrawImage(imageArr[e.Index], new Point(e.Bounds.X, e.Bounds.Y));
		}
	}

	public void layersList1_DropDownClosed(object sender, EventArgs e)
	{
		foreach (GraphicObject @object in graphicTable.objects)
		{
			if (!@object.isSelected)
			{
				continue;
			}
			switch (layersList1.SelectedIndex)
			{
			case 0:
				@object.layer = StreasinaLayer;
				break;
			case 1:
				@object.layer = CoamaLayer;
				break;
			case 2:
				@object.layer = FrontonLayer;
				break;
			case 3:
				@object.layer = DolieLayer;
				break;
			case 4:
				@object.layer = BorduraCalcanLayer;
				break;
			case 5:
				@object.layer = CoamaInclinataLayer;
				break;
			case 6:
				@object.layer = SemiCoamaLayer;
				break;
			case 7:
				@object.layer = FrontonInclinatLayer;
				break;
			case 8:
				@object.layer = RuperePantaConcavaLayer;
				break;
			case 9:
				@object.layer = RuperePantaConvexaLayer;
				break;
			case 10:
				@object.layer = RacordLateralLayer;
				break;
			case 11:
				@object.layer = FrontonEvazatLayer;
				break;
			case 12:
				@object.layer = DefaultLayer;
				break;
			}
			foreach (Panel item in graphicTable.panelsForDeveloping)
			{
				foreach (GraphicObject object2 in item.objects)
				{
					if (object2.objectIndex == @object.objectIndex)
					{
						object2.layer = @object.layer;
					}
				}
			}
			foreach (Panel panel in graphicTableForPanels.panels)
			{
				foreach (GraphicObject object3 in panel.objects)
				{
					if (object3.objectIndex == @object.objectIndex)
					{
						object3.layer = @object.layer;
					}
				}
			}
			@object.isSelected = false;
			graphicTable.RedrawAll();
		}
	}

	private void tabControl_KeyDown(object sender, KeyEventArgs e)
	{
		if (!graphicTable.Visible)
		{
			graphicTableForPanels.Focus();
			ProcessDialogKey(e.KeyData);
		}
	}

	private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
	{
		StreamWriter streamWriter = new StreamWriter(saveFileDialog1.FileName);
		streamWriter.Write(graphicTableForPanels.reportThis);
		streamWriter.Close();
	}

	public void DrawOrigin(bool b)
	{
		if (b)
		{
			graphicTable.AmIdrawingTheOrigin = true;
			graphicTableForPanels.AmIdrawingTheOrigin = true;
		}
		if (!b)
		{
			graphicTable.AmIdrawingTheOrigin = false;
			graphicTableForPanels.AmIdrawingTheOrigin = false;
		}
	}

	public void Line()
	{
		enableCommand = true;
		activeCommand = 1;
	}

	public void PolyLine()
	{
		enableCommand = true;
		activeCommand = 4;
	}

	public void Filet()
	{
		enableCommand = true;
		activeCommand = 9;
		graphicTable.Cursor = mainwindow.selectionCursor;
	}

	public void Panel()
	{
		enableCommand = true;
		activeCommand = 11;
		graphicTable.Cursor = mainwindow.selectionCursor;
	}

	public void SelectObject()
	{
		enableCommand = true;
		activeCommand = 15;
	}

	public void Rectangle1()
	{
		enableCommand = true;
		activeCommand = 13;
	}

	public void BreakPanel()
	{
		enableCommand = true;
		activeCommand = 16;
	}

	public void BreakAllPanels()
	{
		enableCommand = true;
		activeCommand = 17;
	}

	private void SetTabControlButton(int x)
	{
		switch (x)
		{
		case 0:
			graphicTable.Enabled = true;
			graphicTable.Visible = true;
			activeCommand = 0;
			enableCommand = false;
			graphicTableForPanels.Enabled = false;
			graphicTableForPanels.Visible = false;
			mainwindow.consoleWindow.Enabled = true;
			mainwindow.consoleWindow.SuspendLayout();
			mainwindow.consoleWindow.CommandHistory.Text = commandHistoryText.Text;
			mainwindow.consoleWindow.CommandHistory.AppendText("", scrollToEnd: true);
			mainwindow.consoleWindow.ResumeLayout(performLayout: true);
			mainwindow.consoleWindow.CommandLine.Text = commandLineText;
			break;
		case 1:
			graphicTable.Enabled = false;
			graphicTable.Visible = false;
			graphicTableForPanels.Enabled = true;
			graphicTableForPanels.Visible = true;
			graphicTableForPanels.panels = graphicTable.panelsForDeveloping;
			if (graphicTableForPanels.panels.Count != 0 && graphicTableForPanels.panels != null)
			{
				if (graphicTableForPanels.currentPanel == null || graphicTableForPanels.currentPanel.objects.Count == 0)
				{
					graphicTableForPanels.currentPanel = (Panel)graphicTableForPanels.panels[0];
				}
				panelList.Items.Clear();
				foreach (Panel panel in graphicTableForPanels.panels)
				{
					panelList.Items.Add(panel.panelName);
				}
				panelList.SelectedItem = graphicTableForPanels.currentPanel.panelName;
				CheckPanel(graphicTableForPanels.currentPanel);
			}
			else
			{
				graphicTableForPanels.Enabled = false;
			}
			commandLineText = mainwindow.consoleWindow.CommandLine.Text;
			mainwindow.consoleWindow.Enabled = false;
			graphicTableForPanels.Focus();
			break;
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		graphicTable.showarea();
	}

	private void panelList_SelectedValueChanged(object sender, EventArgs e)
	{
		if (graphicTableForPanels.panels == null)
		{
			return;
		}
		foreach (Panel panel in graphicTableForPanels.panels)
		{
			if (panel.panelName == panelList.Text)
			{
				CheckPanel(panel);
				graphicTableForPanels.Focus();
				break;
			}
		}
	}

	public void CheckPanel(Panel pan)
	{
		graphicTableForPanels.currentPanel = pan;
		foreach (GraphicObject @object in pan.objects)
		{
			@object.isSelected = false;
		}
		if (!pan.evolved)
		{
			layersList.Enabled = false;
			currentPanelArea.ResetText();
			currentPanelArea.Enabled = false;
			SelectObject();
		}
		else
		{
			activeCommand = 0;
			enableCommand = false;
			layersList.Enabled = true;
			angleDeclaration.Text = pan.panelAngle.ToString();
			currentPanelArea.Enabled = true;
			currentPanelArea.Text = (pan.panelArea / 10000.0).ToString("N3") + dictionar.dictionar[173];
		}
		ActualizeazaAriaTablei(pan);
		graphicTableForPanels.ZoomExtents();
	}

	public void ActualizeazaAriaTablei(Panel pan)
	{
		if (pan.panelingObjects.Count == 0)
		{
			currentSheetsArea.ResetText();
			currentSheetsArea.Enabled = false;
			return;
		}
		currentSheetsArea.Enabled = true;
		double num = 0.0;
		foreach (PanelObject panelingObject in pan.panelingObjects)
		{
			num += panelingObject.panelArea;
		}
		currentSheetsArea.Text = (num / 10000.0).ToString("N3") + dictionar.dictionar[173];
	}

	private void EvolveThePanel(double ratio)
	{
		graphicTableForPanels.currentPanel.panelAngle = Math.Acos(ratio) * 180.0 / Math.PI;
		foreach (GraphicObject @object in graphicTableForPanels.currentPanel.objects)
		{
			double num = bendingObject.startPoint.X;
			double num2 = bendingObject.startPoint.Y;
			double num3 = bendingObject.stopPoint.X;
			double num4 = bendingObject.stopPoint.Y;
			double num5 = @object.startPoint.X;
			double num6 = @object.startPoint.Y;
			double num7 = ((num5 - num) * (num3 - num) + (num6 - num2) * (num4 - num2)) / ((num3 - num) * (num3 - num) + (num4 - num2) * (num4 - num2));
			double num8 = num + num7 * (num3 - num);
			double num9 = num2 + num7 * (num4 - num2);
			double num10 = num8 - num5;
			double num11 = num9 - num6;
			double num12 = num10 / ratio;
			double num13 = num11 / ratio;
			@object.startPoint.X = num8 - num12;
			@object.startPoint.Y = num9 - num13;
			num5 = @object.stopPoint.X;
			num6 = @object.stopPoint.Y;
			num7 = ((num5 - num) * (num3 - num) + (num6 - num2) * (num4 - num2)) / ((num3 - num) * (num3 - num) + (num4 - num2) * (num4 - num2));
			num8 = num + num7 * (num3 - num);
			num9 = num2 + num7 * (num4 - num2);
			num10 = num8 - num5;
			num11 = num9 - num6;
			num12 = num10 / ratio;
			num13 = num11 / ratio;
			@object.stopPoint.X = num8 - num12;
			@object.stopPoint.Y = num9 - num13;
		}
		layersList.Enabled = true;
		currentPanelArea.Enabled = true;
		currentPanelArea.Text = (graphicTableForPanels.currentPanel.panelArea / 10000.0).ToString("N3") + dictionar.dictionar[173];
		graphicTableForPanels.ZoomExtents();
	}
}
