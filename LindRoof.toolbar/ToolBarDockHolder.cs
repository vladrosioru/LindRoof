using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LindRoof.toolbar;

public class ToolBarDockHolder : UserControl
{
	private Container components;

	private AllowedBorders _allowedBorders = AllowedBorders.All;

	private Control _control;

	private Point _preferredDockedLocation = new Point(0, 0);

	private ToolBarDockArea _preferredDockedArea;

	private Form _form = new Form();

	private string _toolbarTitle = string.Empty;

	private DockStyle _style = DockStyle.Top;

	private System.Windows.Forms.Panel _panel;

	private ToolBarManager _dockManager;

	private static int _mininumStrSize;

	public AllowedBorders AllowedBorders
	{
		get
		{
			return _allowedBorders;
		}
		set
		{
			_allowedBorders = value;
		}
	}

	public Control Control => _control;

	public Point PreferredDockedLocation
	{
		get
		{
			return _preferredDockedLocation;
		}
		set
		{
			_preferredDockedLocation = value;
		}
	}

	public ToolBarDockArea PreferredDockedArea
	{
		get
		{
			return _preferredDockedArea;
		}
		set
		{
			_preferredDockedArea = value;
		}
	}

	public Form FloatForm => _form;

	public string ToolbarTitle
	{
		get
		{
			return _toolbarTitle;
		}
		set
		{
			if (_toolbarTitle != value)
			{
				_toolbarTitle = value;
				TitleTextChanged();
			}
		}
	}

	public DockStyle DockStyle
	{
		get
		{
			return _style;
		}
		set
		{
			_style = value;
			Create();
		}
	}

	public ToolBarManager DockManager
	{
		get
		{
			return _dockManager;
		}
		set
		{
			_dockManager = value;
		}
	}

	public bool IsAllowed(DockStyle dock)
	{
		return dock switch
		{
			DockStyle.Fill => false, 
			DockStyle.Top => (AllowedBorders & AllowedBorders.Top) == AllowedBorders.Top, 
			DockStyle.Left => (AllowedBorders & AllowedBorders.Left) == AllowedBorders.Left, 
			DockStyle.Bottom => (AllowedBorders & AllowedBorders.Bottom) == AllowedBorders.Bottom, 
			DockStyle.Right => (AllowedBorders & AllowedBorders.Right) == AllowedBorders.Right, 
			DockStyle.None => true, 
			_ => false, 
		};
	}

	public ToolBarDockHolder(ToolBarManager dm, Control c, DockStyle style)
	{
		InitializeComponent();
		SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, value: true);
		_panel.Controls.AddRange(new Control[1] { c });
		DockManager = dm;
		switch (style)
		{
		case DockStyle.Left:
			_preferredDockedArea = dm.Left;
			break;
		case DockStyle.Right:
			_preferredDockedArea = dm.Right;
			break;
		case DockStyle.Bottom:
			_preferredDockedArea = dm.Bottom;
			break;
		default:
			_preferredDockedArea = dm.Top;
			break;
		}
		_control = c;
		FloatForm.Visible = false;
		FloatForm.FormBorderStyle = FormBorderStyle.None;
		FloatForm.MaximizeBox = false;
		FloatForm.MinimizeBox = false;
		FloatForm.ShowInTaskbar = false;
		FloatForm.ClientSize = new Size(10, 10);
		DockManager.MainForm.AddOwnedForm(FloatForm);
		DockStyle = style;
		ToolbarTitle = c.Text;
	}

	private void TitleTextChanged()
	{
		if (FloatForm.Visible)
		{
			Invalidate(invalidateChildren: false);
		}
	}

	private void Create()
	{
		Control control = _control;
		Size size = new Size(0, 0);
		if (typeof(ToolBar).IsAssignableFrom(control.GetType()))
		{
			ToolBar toolBar = (ToolBar)control;
			int num = 0;
			int num2 = 0;
			if (DockStyle != DockStyle.Right && DockStyle != DockStyle.Left)
			{
				control.Dock = DockStyle.Top;
				foreach (ToolBarButton button in toolBar.Buttons)
				{
					num += button.Rectangle.Width;
				}
				num2 = toolBar.ButtonSize.Height;
				size = new Size(num, num2);
			}
			else
			{
				control.Dock = DockStyle.Left;
				foreach (ToolBarButton button2 in toolBar.Buttons)
				{
					num2 = ((button2.Style != ToolBarButtonStyle.Separator) ? (num2 + button2.Rectangle.Height) : (num2 + 2 * button2.Rectangle.Width));
				}
				num = toolBar.ButtonSize.Width + 2;
				size = new Size(num, num2);
			}
		}
		else
		{
			size = control.Size;
			control.Dock = DockStyle.Fill;
		}
		base.DockPadding.All = 0;
		if (DockStyle == DockStyle.None)
		{
			base.DockPadding.Left = 2;
			base.DockPadding.Bottom = 2;
			base.DockPadding.Right = 2;
			base.DockPadding.Top = 15;
			size = new Size(size.Width + 4, size.Height + 18);
		}
		else if (DockStyle != DockStyle.Right && DockStyle != DockStyle.Left)
		{
			base.DockPadding.Left = 8;
			size = new Size(size.Width + 8, size.Height);
		}
		else
		{
			base.DockPadding.Top = 8;
			size = new Size(size.Width, size.Height + 8);
		}
		base.Size = size;
	}

	public bool CanDrag(Point p)
	{
		if (DockStyle == DockStyle.None)
		{
			if (p.Y < 16)
			{
				return p.X < base.Width - 16;
			}
			return false;
		}
		if (DockStyle != DockStyle.Right && DockStyle != DockStyle.Left)
		{
			if (p.X < 8)
			{
				return base.ClientRectangle.Contains(p);
			}
			return false;
		}
		if (p.Y < 8)
		{
			return base.ClientRectangle.Contains(p);
		}
		return false;
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
		this._panel = new System.Windows.Forms.Panel();
		base.SuspendLayout();
		this._panel.Dock = System.Windows.Forms.DockStyle.Fill;
		this._panel.Name = "_panel";
		this._panel.Size = new System.Drawing.Size(384, 40);
		this._panel.TabIndex = 0;
		this.BackColor = System.Drawing.SystemColors.ControlLight;
		base.Controls.AddRange(new System.Windows.Forms.Control[1] { this._panel });
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Name = "ToolBarDockHolder";
		base.Size = new System.Drawing.Size(384, 40);
		base.MouseUp += new System.Windows.Forms.MouseEventHandler(ToolBarDockHolder_MouseUp);
		base.Paint += new System.Windows.Forms.PaintEventHandler(ToolBarDockHolder_Paint);
		base.MouseEnter += new System.EventHandler(ToolBarDockHolder_MouseEnter);
		base.MouseMove += new System.Windows.Forms.MouseEventHandler(ToolBarDockHolder_MouseMove);
		base.MouseLeave += new System.EventHandler(ToolBarDockHolder_MouseLeave);
		base.ResumeLayout(false);
	}

	private void ToolBarDockHolder_Paint(object sender, PaintEventArgs e)
	{
		if (DockStyle == DockStyle.None)
		{
			e.Graphics.FillRectangle(SystemBrushes.ControlDark, base.ClientRectangle);
			DrawString(e.Graphics, ToolbarTitle, new Rectangle(0, 0, base.Width - 16, 14), SystemBrushes.ControlText);
			Rectangle rectangle = new Rectangle(base.Width - 15, 2, 10, 10);
			Pen pen = new Pen(SystemColors.ControlText);
			DrawCloseButton(e.Graphics, rectangle, pen);
			if (rectangle.Contains(PointToClient(Control.MousePosition)))
			{
				e.Graphics.DrawRectangle(pen, rectangle);
			}
			Rectangle clientRectangle = base.ClientRectangle;
			clientRectangle.Width--;
			clientRectangle.Height--;
			e.Graphics.DrawRectangle(new Pen(SystemColors.ControlDarkDark), clientRectangle);
			return;
		}
		e.Graphics.FillRectangle(SystemBrushes.ControlLight, base.ClientRectangle);
		int num = 2;
		Pen pen2 = new Pen(SystemColors.ControlDark);
		if (DockStyle != DockStyle.Right && DockStyle != DockStyle.Left)
		{
			for (int i = 3; i < base.Size.Height - 3; i += num)
			{
				e.Graphics.DrawLine(pen2, new Point(num, i), new Point(num + num, i));
			}
		}
		else
		{
			for (int j = 3; j < base.Size.Width - 3; j += num)
			{
				e.Graphics.DrawLine(pen2, new Point(j, num), new Point(j, num + num));
			}
		}
	}

	private void ToolBarDockHolder_MouseEnter(object sender, EventArgs e)
	{
		if (DockStyle != DockStyle.None && CanDrag(PointToClient(Control.MousePosition)))
		{
			Cursor = Cursors.SizeAll;
		}
		else
		{
			Cursor = Cursors.Default;
		}
		Invalidate(invalidateChildren: false);
	}

	private void ToolBarDockHolder_MouseLeave(object sender, EventArgs e)
	{
		Cursor = Cursors.Default;
		Invalidate(invalidateChildren: false);
	}

	private void ToolBarDockHolder_MouseMove(object sender, MouseEventArgs e)
	{
		if (DockStyle != DockStyle.None && CanDrag(new Point(e.X, e.Y)))
		{
			Cursor = Cursors.SizeAll;
		}
		else
		{
			Cursor = Cursors.Default;
		}
		Invalidate(invalidateChildren: false);
	}

	private void ToolBarDockHolder_MouseUp(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Right && CanDrag(new Point(e.X, e.Y)))
		{
			DockManager.ShowContextMenu(PointToScreen(new Point(e.X, e.Y)));
		}
		if (e.Button == MouseButtons.Left && DockStyle == DockStyle.None && e.Y < 16 && e.X > base.Width - 16)
		{
			FloatForm.Visible = false;
		}
	}

	private void DrawString(Graphics g, string s, Rectangle area, Brush brush)
	{
		if (_mininumStrSize == 0)
		{
			_mininumStrSize = (int)g.MeasureString("....", Font).Width;
		}
		if (area.Width >= _mininumStrSize)
		{
			StringFormat stringFormat = new StringFormat();
			stringFormat.FormatFlags = StringFormatFlags.FitBlackBox | StringFormatFlags.NoWrap;
			stringFormat.Trimming = StringTrimming.EllipsisCharacter;
			SizeF sizeF = g.MeasureString(s, Font);
			if (sizeF.Height < (float)area.Height)
			{
				int num = (int)((float)area.Height - sizeF.Height) / 2;
				area.Y += num;
				area.Height -= num;
			}
			g.DrawString(s, Font, brush, area, stringFormat);
		}
	}

	private void DrawCloseButton(Graphics g, Rectangle cross, Pen pen)
	{
		cross.Inflate(-2, -2);
		g.DrawLine(pen, cross.X, cross.Y, cross.Right, cross.Bottom);
		g.DrawLine(pen, cross.X + 1, cross.Y, cross.Right, cross.Bottom - 1);
		g.DrawLine(pen, cross.X, cross.Y + 1, cross.Right - 1, cross.Bottom);
		g.DrawLine(pen, cross.Right, cross.Y, cross.Left, cross.Bottom);
		g.DrawLine(pen, cross.Right - 1, cross.Y, cross.Left, cross.Bottom - 1);
		g.DrawLine(pen, cross.Right, cross.Y + 1, cross.Left + 1, cross.Bottom);
	}
}
