using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using LindRoof.TextualEnvironment;

namespace LindRoof;

public class ConsoleWindow : UserControl
{
	private Label label1;

	public ScrollableRichTextBox CommandHistory;

	public TextBox CommandLine;

	private Container components;

	private string[] dictionary;

	private string someText;

	private IEnumerator dictionaryEnumerator;

	private bool executeCommand;

	private MainWindow mainwindow;

	private dictionar_cuvinte dictionar;

	public ConsoleWindow()
	{
		InitializeComponent();
	}

	public ConsoleWindow(MainWindow mainwnd)
	{
		mainwindow = mainwnd;
		dictionar = mainwindow.dictionar;
		InitComp(celReal: true);
		FillDictionary();
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
		this.CommandHistory = new LindRoof.ScrollableRichTextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.CommandLine = new System.Windows.Forms.TextBox();
		base.SuspendLayout();
		this.CommandHistory.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.CommandHistory.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CommandHistory.Location = new System.Drawing.Point(8, 0);
		this.CommandHistory.Name = "CommandHistory";
		this.CommandHistory.ReadOnly = true;
		this.CommandHistory.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
		this.CommandHistory.Size = new System.Drawing.Size(784, 48);
		this.CommandHistory.TabIndex = 5;
		this.CommandHistory.TabStop = false;
		this.CommandHistory.Text = "";
		this.CommandHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CommandHistory_KeyPress);
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.label1.BackColor = System.Drawing.SystemColors.Control;
		this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label1.Font = new System.Drawing.Font("Arial", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(8, 48);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(88, 24);
		this.label1.TabIndex = 2;
		this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
		this.CommandLine.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.CommandLine.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CommandLine.Location = new System.Drawing.Point(96, 48);
		this.CommandLine.Name = "CommandLine";
		this.CommandLine.Size = new System.Drawing.Size(696, 22);
		this.CommandLine.TabIndex = 0;
		this.CommandLine.Text = "";
		this.CommandLine.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CommandLine_KeyPress);
		this.CommandLine.MouseWheel += new System.Windows.Forms.MouseEventHandler(CommandLine_MouseWheel);
		base.Controls.Add(this.CommandLine);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.CommandHistory);
		this.Cursor = System.Windows.Forms.Cursors.IBeam;
		base.Name = "ConsoleWindow";
		base.Size = new System.Drawing.Size(800, 72);
		base.ResumeLayout(false);
	}

	private void InitComp(bool celReal)
	{
		CommandHistory = new ScrollableRichTextBox();
		label1 = new Label();
		CommandLine = new TextBox();
		SuspendLayout();
		CommandHistory.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		CommandHistory.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
		CommandHistory.Location = new Point(8, 0);
		CommandHistory.Name = "CommandHistory";
		CommandHistory.ReadOnly = true;
		CommandHistory.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
		CommandHistory.Size = new Size(784, 48);
		CommandHistory.TabIndex = 5;
		CommandHistory.TabStop = false;
		CommandHistory.Text = "";
		CommandHistory.KeyPress += CommandHistory_KeyPress;
		label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
		label1.BackColor = SystemColors.Control;
		label1.BorderStyle = BorderStyle.Fixed3D;
		label1.Font = new Font("Arial", 11.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
		label1.Location = new Point(8, 48);
		label1.Name = "label1";
		label1.Size = new Size(88, 24);
		label1.TabIndex = 2;
		label1.Text = dictionar.dictionar[150];
		label1.TextAlign = ContentAlignment.BottomCenter;
		CommandLine.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		CommandLine.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
		CommandLine.Location = new Point(96, 48);
		CommandLine.Name = "CommandLine";
		CommandLine.Size = new Size(696, 22);
		CommandLine.TabIndex = 0;
		CommandLine.Text = "";
		CommandLine.KeyPress += CommandLine_KeyPress;
		CommandLine.MouseWheel += CommandLine_MouseWheel;
		base.Controls.Add(CommandLine);
		base.Controls.Add(label1);
		base.Controls.Add(CommandHistory);
		Cursor = Cursors.IBeam;
		base.Name = "ConsoleWindow";
		base.Size = new Size(800, 72);
		ResumeLayout(performLayout: false);
	}

	protected override bool ProcessDialogKey(Keys keyData)
	{
		CheckForSpecialKeys(keyData);
		return base.ProcessDialogKey(keyData);
	}

	public void CheckForSpecialKeys(Keys keyData)
	{
		switch (keyData)
		{
		case Keys.F3:
			if (mainwindow.statusBarOsnap.BorderStyle == StatusBarPanelBorderStyle.Raised)
			{
				mainwindow.statusBarOsnap.BorderStyle = StatusBarPanelBorderStyle.Sunken;
			}
			else
			{
				mainwindow.statusBarOsnap.BorderStyle = StatusBarPanelBorderStyle.Raised;
			}
			break;
		case Keys.F7:
			if (mainwindow.statusBarGrid.BorderStyle == StatusBarPanelBorderStyle.Raised)
			{
				mainwindow.statusBarGrid.BorderStyle = StatusBarPanelBorderStyle.Sunken;
			}
			else
			{
				mainwindow.statusBarGrid.BorderStyle = StatusBarPanelBorderStyle.Raised;
			}
			((Document)base.ParentForm.ActiveMdiChild).graphicTable.RedrawAll();
			break;
		case Keys.F8:
			if (mainwindow.statusBarOrtho.BorderStyle == StatusBarPanelBorderStyle.Raised)
			{
				mainwindow.statusBarOrtho.BorderStyle = StatusBarPanelBorderStyle.Sunken;
			}
			else
			{
				mainwindow.statusBarOrtho.BorderStyle = StatusBarPanelBorderStyle.Raised;
			}
			break;
		case Keys.F9:
			if (mainwindow.statusBarSnap.BorderStyle == StatusBarPanelBorderStyle.Raised)
			{
				mainwindow.statusBarSnap.BorderStyle = StatusBarPanelBorderStyle.Sunken;
			}
			else
			{
				mainwindow.statusBarSnap.BorderStyle = StatusBarPanelBorderStyle.Raised;
			}
			break;
		case Keys.Delete:
			if (!executeCommand)
			{
				((Document)base.ParentForm.ActiveMdiChild).graphicTable.RemoveSelectedObjects();
				((Document)base.ParentForm.ActiveMdiChild).graphicTable.RedrawAll();
			}
			break;
		case Keys.Left:
			if (((Document)base.ParentForm.ActiveMdiChild).graphicTableForPanels.Visible)
			{
				((Document)base.ParentForm.ActiveMdiChild).graphicTableForPanels.CheckKeys(keyData);
			}
			break;
		case Keys.Right:
			if (((Document)base.ParentForm.ActiveMdiChild).graphicTableForPanels.Visible)
			{
				((Document)base.ParentForm.ActiveMdiChild).graphicTableForPanels.CheckKeys(keyData);
			}
			break;
		}
	}

	public void PressedWithoutFocus(KeyPressEventArgs e)
	{
		SendKeys.SendWait(e.KeyChar.ToString());
	}

	private void CommandLine_MouseWheel(object sender, MouseEventArgs e)
	{
		((Document)base.ParentForm.ActiveMdiChild).graphicTable.Focus();
	}

	public void CommandLine_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (!executeCommand)
		{
			switch ((int)e.KeyChar)
			{
			case 13:
				CheckDictionary();
				CommandLine.ResetText();
				break;
			case 32:
				CheckDictionary();
				CommandLine.ResetText();
				SendKeys.Send("{BACKSPACE}");
				break;
			case 27:
			{
				CommandLine.ResetText();
				CommandLine.Text = dictionar.dictionar[127];
				SendKeys.Send("{ENTER}");
				bool escapeCase = true;
				((Document)base.ParentForm.ActiveMdiChild).graphicTable.CheckCommandForEnding(escapeCase);
				((Document)base.ParentForm.ActiveMdiChild).graphicTable.Cursor = Cursors.Cross;
				break;
			}
			}
		}
		else
		{
			int keyChar = e.KeyChar;
			if (keyChar == 27)
			{
				CommandLine.ResetText();
				SendKeys.Send("{ENTER}");
				bool escapeCase2 = true;
				((Document)base.ParentForm.ActiveMdiChild).graphicTable.CheckCommandForEnding(escapeCase2);
				((Document)base.ParentForm.ActiveMdiChild).graphicTable.Cursor = Cursors.Cross;
			}
		}
	}

	private void ConsoleWindow_Enter(object sender, EventArgs e)
	{
		CommandLine.Focus();
	}

	private void CommandHistory_Enter(object sender, EventArgs e)
	{
		CommandLine.Select();
	}

	private void CommandHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommandLine.Focus();
		PressedWithoutFocus(e);
	}

	private void FillDictionary()
	{
		dictionary = new string[14]
		{
			dictionar.dictionar[128],
			"cerc",
			"arc",
			dictionar.dictionar[129],
			dictionar.dictionar[130],
			"c",
			"a",
			dictionar.dictionar[131],
			dictionar.dictionar[132],
			dictionar.dictionar[133],
			dictionar.dictionar[134],
			dictionar.dictionar[135],
			dictionar.dictionar[136],
			dictionar.dictionar[137]
		};
		dictionaryEnumerator = dictionary.GetEnumerator();
	}

	public void CheckDictionary(int flag)
	{
		switch (flag)
		{
		case 1:
		case 5:
			someText = dictionar.dictionar[138];
			executeCommand = true;
			AppendTextToHistory("\r\n" + someText);
			if (executeCommand)
			{
				ExecuteCommand(1);
			}
			break;
		case 4:
		case 8:
			someText = dictionar.dictionar[139];
			executeCommand = true;
			AppendTextToHistory("\r\n" + someText);
			if (executeCommand)
			{
				ExecuteCommand(4);
			}
			break;
		case 9:
		case 10:
			someText = dictionar.dictionar[140];
			executeCommand = true;
			AppendTextToHistory("\r\n" + someText);
			if (executeCommand)
			{
				ExecuteCommand(9);
			}
			break;
		case 11:
		case 12:
			someText = dictionar.dictionar[141];
			executeCommand = true;
			AppendTextToHistory("\r\n" + someText);
			if (executeCommand)
			{
				ExecuteCommand(11);
			}
			break;
		case 13:
		case 14:
			someText = dictionar.dictionar[142];
			executeCommand = true;
			AppendTextToHistory("\r\n" + someText);
			if (executeCommand)
			{
				ExecuteCommand(13);
			}
			break;
		case 2:
		case 3:
		case 6:
		case 7:
			break;
		}
	}

	private void CheckDictionary()
	{
		if (CommandLine.Text != "")
		{
			string text = CommandLine.Text.Trim().ToLower();
			someText = dictionar.dictionar[143] + "'" + text + "'";
			int num = 1;
			dictionaryEnumerator.Reset();
			if (((Document)base.ParentForm.ActiveMdiChild).activeCommand > 0)
			{
				switch (text.Substring(0, 1))
				{
				case "@":
					switch (((Document)base.ParentForm.ActiveMdiChild).activeCommand)
					{
					case 1:
						try
						{
							string text4 = text.Substring(1);
							if (text4.Contains("<"))
							{
								string[] array5 = text4.Split(new char[1] { '<' }, 2);
								double num24 = double.Parse(array5[0], NumberStyles.Float, CultureInfo.CurrentUICulture) / (double)((Document)base.ParentForm.ActiveMdiChild).graphicTable.unitscale;
								double num25 = double.Parse(array5[1], NumberStyles.Float, CultureInfo.CurrentUICulture);
								double num26 = Math.Cos(Math.PI * 2.0 * num25 / 360.0);
								double num27 = Math.Sin(Math.PI * 2.0 * num25 / 360.0);
								PointD startPoint6 = ((Document)base.ParentForm.ActiveMdiChild).graphicTable.startPoint;
								_ = ((Document)base.ParentForm.ActiveMdiChild).graphicTable.scale;
								PointD e6 = new PointD(startPoint6.X + num24 * num26, startPoint6.Y + num24 * num27);
								((Document)base.ParentForm.ActiveMdiChild).graphicTable.CastingStopPointOfTheLine(e6);
							}
							else if (text4.Contains(","))
							{
								string[] array6 = text4.Split(new char[1] { ',' }, 2);
								double num28 = double.Parse(array6[0], NumberStyles.Float, CultureInfo.CurrentUICulture) / (double)((Document)base.ParentForm.ActiveMdiChild).graphicTable.unitscale;
								double num29 = double.Parse(array6[1], NumberStyles.Float, CultureInfo.CurrentUICulture) / (double)((Document)base.ParentForm.ActiveMdiChild).graphicTable.unitscale;
								PointD startPoint7 = ((Document)base.ParentForm.ActiveMdiChild).graphicTable.startPoint;
								_ = ((Document)base.ParentForm.ActiveMdiChild).graphicTable.scale;
								PointD e7 = new PointD(startPoint7.X + num28, startPoint7.Y + num29);
								((Document)base.ParentForm.ActiveMdiChild).graphicTable.CastingStopPointOfTheLine(e7);
							}
						}
						catch
						{
						}
						break;
					case 4:
					{
						string text3 = text.Substring(1);
						if (text3.Contains("<"))
						{
							string[] array3 = text3.Split(new char[1] { '<' }, 2);
							double num18 = double.Parse(array3[0], NumberStyles.Float, CultureInfo.CurrentUICulture) / (double)((Document)base.ParentForm.ActiveMdiChild).graphicTable.unitscale;
							double num19 = double.Parse(array3[1], NumberStyles.Float, CultureInfo.CurrentUICulture);
							double num20 = Math.Cos(Math.PI * 2.0 * num19 / 360.0);
							double num21 = Math.Sin(Math.PI * 2.0 * num19 / 360.0);
							PointD startPoint4 = ((Document)base.ParentForm.ActiveMdiChild).graphicTable.startPoint;
							_ = ((Document)base.ParentForm.ActiveMdiChild).graphicTable.scale;
							PointD e4 = new PointD(startPoint4.X + num18 * num20, startPoint4.Y + num18 * num21);
							((Document)base.ParentForm.ActiveMdiChild).graphicTable.CastingAPointOfThePolyline(e4);
						}
						else if (text3.Contains(","))
						{
							string[] array4 = text3.Split(new char[1] { ',' }, 2);
							double num22 = double.Parse(array4[0], NumberStyles.Float, CultureInfo.CurrentUICulture) / (double)((Document)base.ParentForm.ActiveMdiChild).graphicTable.unitscale;
							double num23 = double.Parse(array4[1], NumberStyles.Float, CultureInfo.CurrentUICulture) / (double)((Document)base.ParentForm.ActiveMdiChild).graphicTable.unitscale;
							PointD startPoint5 = ((Document)base.ParentForm.ActiveMdiChild).graphicTable.startPoint;
							_ = ((Document)base.ParentForm.ActiveMdiChild).graphicTable.scale;
							PointD e5 = new PointD(startPoint5.X + num22, startPoint5.Y + num23);
							((Document)base.ParentForm.ActiveMdiChild).graphicTable.CastingAPointOfThePolyline(e5);
						}
						break;
					}
					case 13:
						try
						{
							string text2 = text.Substring(1);
							string[] array2 = text2.Split(new char[1] { ',' }, 2);
							double num16 = double.Parse(array2[0], NumberStyles.Float, CultureInfo.CurrentUICulture) / (double)((Document)base.ParentForm.ActiveMdiChild).graphicTable.unitscale;
							double num17 = double.Parse(array2[1], NumberStyles.Float, CultureInfo.CurrentUICulture) / (double)((Document)base.ParentForm.ActiveMdiChild).graphicTable.unitscale;
							PointD startPoint3 = ((Document)base.ParentForm.ActiveMdiChild).graphicTable.startPoint;
							_ = ((Document)base.ParentForm.ActiveMdiChild).graphicTable.scale;
							PointD e3 = new PointD(startPoint3.X + num16, startPoint3.Y + num17);
							((Document)base.ParentForm.ActiveMdiChild).graphicTable.CastingStopPointOfTheRectangle(e3);
						}
						catch
						{
						}
						break;
					}
					break;
				case "0":
				case "1":
				case "2":
				case "3":
				case "4":
				case "5":
				case "6":
				case "7":
				case "8":
				case "9":
					try
					{
						double num2 = double.Parse(text, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentUICulture) / (double)((Document)base.ParentForm.ActiveMdiChild).graphicTable.unitscale;
						switch (((Document)base.ParentForm.ActiveMdiChild).activeCommand)
						{
						case 1:
						{
							PointD startPoint = ((Document)base.ParentForm.ActiveMdiChild).graphicTable.startPoint;
							PointD pointD = ((Document)base.ParentForm.ActiveMdiChild).graphicTable.TransformFromScreen(((Document)base.ParentForm.ActiveMdiChild).graphicTable.secondPoint);
							if (((MainWindow)base.ParentForm).statusBarOrtho.BorderStyle.ToString() == "Sunken")
							{
								double num10 = Math.Abs(pointD.X - startPoint.X);
								double num11 = Math.Abs(pointD.Y - startPoint.Y);
								pointD = ((!(num10 >= num11)) ? new PointD(startPoint.X, pointD.Y) : new PointD(pointD.X, startPoint.Y));
							}
							double num5 = ((Document)base.ParentForm.ActiveMdiChild).graphicTable.CalculateLengthBetween2PointsD(pointD, startPoint);
							double num6 = pointD.X - startPoint.X;
							double num7 = pointD.Y - startPoint.Y;
							double num8 = num6 * num2 / num5;
							double num9 = num7 * num2 / num5;
							PointD e = new PointD(startPoint.X + num8, startPoint.Y + num9);
							((Document)base.ParentForm.ActiveMdiChild).graphicTable.CastingStopPointOfTheLine(e);
							break;
						}
						case 4:
						{
							PointD startPoint = ((Document)base.ParentForm.ActiveMdiChild).graphicTable.startPoint;
							PointD pointD = ((Document)base.ParentForm.ActiveMdiChild).graphicTable.TransformFromScreen(((Document)base.ParentForm.ActiveMdiChild).graphicTable.secondPoint);
							if (((MainWindow)base.ParentForm).statusBarOrtho.BorderStyle.ToString() == "Sunken")
							{
								double num3 = Math.Abs(pointD.X - startPoint.X);
								double num4 = Math.Abs(pointD.Y - startPoint.Y);
								pointD = ((!(num3 >= num4)) ? new PointD(startPoint.X, pointD.Y) : new PointD(pointD.X, startPoint.Y));
							}
							double num5 = ((Document)base.ParentForm.ActiveMdiChild).graphicTable.CalculateLengthBetween2PointsD(pointD, startPoint);
							double num6 = pointD.X - startPoint.X;
							double num7 = pointD.Y - startPoint.Y;
							double num8 = num6 * num2 / num5;
							double num9 = num7 * num2 / num5;
							PointD e = new PointD(startPoint.X + num8, startPoint.Y + num9);
							((Document)base.ParentForm.ActiveMdiChild).graphicTable.CastingAPointOfThePolyline(e);
							break;
						}
						}
					}
					catch
					{
						try
						{
							string[] array = text.Split(new char[1] { '<' }, 2);
							double num12 = double.Parse(array[0], NumberStyles.Float, CultureInfo.CurrentUICulture) / (double)((Document)base.ParentForm.ActiveMdiChild).graphicTable.unitscale;
							double num13 = double.Parse(array[1], NumberStyles.Float, CultureInfo.CurrentUICulture);
							PointD startPoint2 = ((Document)base.ParentForm.ActiveMdiChild).graphicTable.startPoint;
							double num14 = num12 * Math.Cos(num13 * 2.0 * Math.PI / 360.0);
							double num15 = num12 * Math.Sin(num13 * 2.0 * Math.PI / 360.0);
							PointD e2 = new PointD(startPoint2.X + num14, startPoint2.Y + num15);
							switch (((Document)base.ParentForm.ActiveMdiChild).activeCommand)
							{
							case 1:
								((Document)base.ParentForm.ActiveMdiChild).graphicTable.CastingStopPointOfTheLine(e2);
								break;
							case 4:
								((Document)base.ParentForm.ActiveMdiChild).graphicTable.CastingAPointOfThePolyline(e2);
								break;
							}
						}
						catch
						{
						}
					}
					break;
				}
			}
			else
			{
				while (dictionaryEnumerator.MoveNext() && dictionaryEnumerator.Current != null)
				{
					if ((string)dictionaryEnumerator.Current == text)
					{
						someText = dictionar.dictionar[144] + "'" + text + "'";
						executeCommand = true;
						break;
					}
					num++;
				}
			}
			if (text == dictionar.dictionar[127])
			{
				someText = text;
			}
			if (executeCommand)
			{
				CheckDictionary(num);
			}
		}
		else
		{
			((Document)base.ParentForm.ActiveMdiChild).graphicTable.CheckCommandForEnding();
		}
	}

	private void ExecuteCommand(int i)
	{
		switch (i)
		{
		case 1:
		case 5:
			AppendTextToHistory(dictionar.dictionar[145]);
			((Document)mainwindow.ActiveMdiChild).Line();
			executeCommand = false;
			break;
		case 4:
		case 8:
			AppendTextToHistory(dictionar.dictionar[146]);
			((Document)mainwindow.ActiveMdiChild).PolyLine();
			executeCommand = false;
			break;
		case 9:
		case 10:
			AppendTextToHistory(dictionar.dictionar[147]);
			((Document)mainwindow.ActiveMdiChild).Filet();
			executeCommand = false;
			break;
		case 11:
		case 12:
			AppendTextToHistory(dictionar.dictionar[148]);
			((Document)mainwindow.ActiveMdiChild).Panel();
			executeCommand = false;
			break;
		case 13:
		case 14:
			AppendTextToHistory(dictionar.dictionar[149]);
			((Document)mainwindow.ActiveMdiChild).Rectangle1();
			executeCommand = false;
			break;
		default:
			executeCommand = false;
			break;
		}
	}

	public void AppendTextToHistory(string s)
	{
		CommandHistory.AppendText(s, scrollToEnd: true);
		((Document)mainwindow.ActiveMdiChild).commandHistoryText.AppendText(s);
		CommandLine.Focus();
	}
}
