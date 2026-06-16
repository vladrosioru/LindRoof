using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace LindRoof.toolbar;

public class ToolBarManager : IMessageFilter
{
	private class MyMenuItem : MenuItem
	{
		public Control Control;
	}

	private class HolderSorter : IComparer
	{
		public int Compare(object x, object y)
		{
			ToolBarDockHolder toolBarDockHolder = x as ToolBarDockHolder;
			ToolBarDockHolder toolBarDockHolder2 = y as ToolBarDockHolder;
			return toolBarDockHolder.ToolbarTitle.CompareTo(toolBarDockHolder2.ToolbarTitle);
		}
	}

	private const int WM_KEYDOWN = 256;

	private const int WM_KEYUP = 257;

	private ScrollableControl _dockStation;

	private Form _mainForm;

	private ToolBarDockArea _left;

	private ToolBarDockArea _right;

	private ToolBarDockArea _top;

	private ToolBarDockArea _bottom;

	private ArrayList _holders = new ArrayList();

	private ToolBarDockHolder _dragged;

	private Point _ptStart;

	private Point _ptOffset;

	private bool _ctrlDown;

	public ScrollableControl DockStation
	{
		get
		{
			return _dockStation;
		}
		set
		{
			_dockStation = value;
		}
	}

	public Form MainForm
	{
		get
		{
			return _mainForm;
		}
		set
		{
			_mainForm = value;
		}
	}

	public ToolBarDockArea Left => _left;

	public ToolBarDockArea Right => _right;

	public ToolBarDockArea Top => _top;

	public ToolBarDockArea Bottom => _bottom;

	public ToolBarManager(ScrollableControl dockStation, Form mainForm)
	{
		DockStation = dockStation;
		MainForm = mainForm;
		_left = new ToolBarDockArea(this, DockStyle.Left);
		_right = new ToolBarDockArea(this, DockStyle.Right);
		_top = new ToolBarDockArea(this, DockStyle.Top);
		_bottom = new ToolBarDockArea(this, DockStyle.Bottom);
		Application.AddMessageFilter(this);
	}

	protected ToolBarDockArea GetClosestArea(Point ptScreen, ToolBarDockArea preferred)
	{
		if (preferred != null)
		{
			Rectangle rectangle = preferred.RectangleToScreen(preferred.ClientRectangle);
			rectangle.Inflate(8, 8);
			if (rectangle.Contains(ptScreen))
			{
				return preferred;
			}
		}
		Rectangle rectangle2 = _left.RectangleToScreen(_left.ClientRectangle);
		rectangle2.Inflate(8, 8);
		Rectangle rectangle3 = _right.RectangleToScreen(_right.ClientRectangle);
		rectangle3.Inflate(8, 8);
		Rectangle rectangle4 = _top.RectangleToScreen(_top.ClientRectangle);
		rectangle4.Inflate(8, 8);
		Rectangle rectangle5 = _bottom.RectangleToScreen(_bottom.ClientRectangle);
		rectangle5.Inflate(8, 8);
		if (rectangle4.Contains(ptScreen))
		{
			return _top;
		}
		if (rectangle5.Contains(ptScreen))
		{
			return _bottom;
		}
		if (rectangle2.Contains(ptScreen))
		{
			return _left;
		}
		if (rectangle3.Contains(ptScreen))
		{
			return _right;
		}
		return null;
	}

	public ToolBarDockHolder GetHolder(Control c)
	{
		foreach (ToolBarDockHolder holder in _holders)
		{
			if (holder.Control == c)
			{
				return holder;
			}
		}
		return null;
	}

	public ToolBarDockHolder GetHolder(string title)
	{
		foreach (ToolBarDockHolder holder in _holders)
		{
			if (holder.ToolbarTitle == title)
			{
				return holder;
			}
		}
		return null;
	}

	public ArrayList GetControls()
	{
		ArrayList arrayList = new ArrayList();
		foreach (ToolBarDockHolder holder in _holders)
		{
			arrayList.Add(holder.Control);
		}
		return arrayList;
	}

	public bool ContainsControl(Control c)
	{
		return GetControls().Contains(c);
	}

	public void ShowControl(Control c, bool show)
	{
		ToolBarDockHolder holder = GetHolder(c);
		if (holder != null && holder.Visible != show)
		{
			if (IsDocked(holder))
			{
				holder.Visible = show;
			}
			else
			{
				holder.FloatForm.Visible = show;
			}
		}
	}

	public ToolBarDockHolder AddControl(Control c, DockStyle site)
	{
		return AddControl(c, site, null, DockStyle.Right);
	}

	public ToolBarDockHolder AddControl(Control c)
	{
		return AddControl(c, DockStyle.Top, null, DockStyle.Right);
	}

	public ToolBarDockHolder AddControl(Control c, DockStyle site, Control refControl, DockStyle refSite)
	{
		if (site == DockStyle.Fill)
		{
			site = DockStyle.Top;
		}
		ToolBarDockHolder toolBarDockHolder = new ToolBarDockHolder(this, c, site);
		if (refControl != null)
		{
			ToolBarDockHolder holder = GetHolder(refControl);
			if (holder != null)
			{
				Point preferredDockedLocation = holder.PreferredDockedLocation;
				switch (refSite)
				{
				case DockStyle.Left:
					preferredDockedLocation.X--;
					break;
				case DockStyle.Right:
					preferredDockedLocation.X += holder.Width + 1;
					break;
				case DockStyle.Bottom:
					preferredDockedLocation.Y += holder.Height + 1;
					break;
				default:
					preferredDockedLocation.Y--;
					break;
				}
				toolBarDockHolder.PreferredDockedLocation = preferredDockedLocation;
			}
		}
		_holders.Add(toolBarDockHolder);
		if (site != DockStyle.None)
		{
			toolBarDockHolder.DockStyle = site;
			toolBarDockHolder.Parent = toolBarDockHolder.PreferredDockedArea;
		}
		else
		{
			toolBarDockHolder.Parent = toolBarDockHolder.FloatForm;
			toolBarDockHolder.Location = new Point(0, 0);
			toolBarDockHolder.DockStyle = DockStyle.None;
			toolBarDockHolder.FloatForm.Size = toolBarDockHolder.Size;
			toolBarDockHolder.FloatForm.Visible = true;
		}
		toolBarDockHolder.MouseUp += ToolBarMouseUp;
		toolBarDockHolder.DoubleClick += ToolBarDoubleClick;
		toolBarDockHolder.MouseMove += ToolBarMouseMove;
		toolBarDockHolder.MouseDown += ToolBarMouseDown;
		return toolBarDockHolder;
	}

	public void RemoveControl(Control c)
	{
		ToolBarDockHolder holder = GetHolder(c);
		if (holder != null)
		{
			holder.MouseUp -= ToolBarMouseUp;
			holder.DoubleClick -= ToolBarDoubleClick;
			holder.MouseMove -= ToolBarMouseMove;
			holder.MouseDown -= ToolBarMouseDown;
			_holders.Remove(holder);
			holder.Parent = null;
			holder.FloatForm.Close();
		}
	}

	private void ToolBarMouseDown(object sender, MouseEventArgs e)
	{
		ToolBarDockHolder toolBarDockHolder = (ToolBarDockHolder)sender;
		if (_dragged == null && e.Button.Equals(MouseButtons.Left) && e.Clicks == 1 && toolBarDockHolder.CanDrag(new Point(e.X, e.Y)))
		{
			_ptStart = Control.MousePosition;
			_dragged = (ToolBarDockHolder)sender;
			_ptOffset = new Point(e.X, e.Y);
		}
	}

	private bool IsDocked(ToolBarDockHolder holder)
	{
		if (holder.Parent != Top && holder.Parent != Left && holder.Parent != Right)
		{
			return holder.Parent == Bottom;
		}
		return true;
	}

	private ToolBarDockArea GetDockedArea(ToolBarDockHolder holder)
	{
		if (holder.Parent == Top)
		{
			return Top;
		}
		if (holder.Parent == Left)
		{
			return Left;
		}
		if (holder.Parent == Right)
		{
			return Right;
		}
		if (holder.Parent == Bottom)
		{
			return Bottom;
		}
		return null;
	}

	private void ToolBarMouseMove(object sender, MouseEventArgs e)
	{
		new Point(e.X, e.Y);
		if (_dragged == null)
		{
			return;
		}
		Point point = new Point(_ptStart.X - Control.MousePosition.X, _ptStart.Y - Control.MousePosition.Y);
		Point point2 = _dragged.PointToScreen(new Point(0, 0));
		point2 = new Point(point2.X - point.X, point2.Y - point.Y);
		ToolBarDockArea toolBarDockArea = GetClosestArea(Control.MousePosition, _dragged.PreferredDockedArea);
		if (toolBarDockArea != null && !_dragged.IsAllowed(toolBarDockArea.Dock))
		{
			toolBarDockArea = null;
		}
		ToolBarDockArea dockedArea = GetDockedArea(_dragged);
		if (_ctrlDown)
		{
			toolBarDockArea = null;
		}
		if (dockedArea != null)
		{
			if (toolBarDockArea == null)
			{
				dockedArea.SuspendLayout();
				_dragged.Parent = _dragged.FloatForm;
				_dragged.Location = new Point(0, 0);
				_dragged.DockStyle = DockStyle.None;
				_dragged.FloatForm.Visible = true;
				_dragged.FloatForm.Location = new Point(Control.MousePosition.X - _ptOffset.X, Control.MousePosition.Y - 8);
				_dragged.FloatForm.Size = _dragged.Size;
				dockedArea.ResumeLayout();
				dockedArea.PerformLayout();
			}
			else if (toolBarDockArea != dockedArea)
			{
				toolBarDockArea.SuspendLayout();
				point2 = toolBarDockArea.PointToClient(Control.MousePosition);
				_dragged.DockStyle = toolBarDockArea.Dock;
				_dragged.Parent = toolBarDockArea;
				_dragged.PreferredDockedLocation = point2;
				_dragged.FloatForm.Visible = false;
				_dragged.PreferredDockedArea = toolBarDockArea;
				toolBarDockArea.ResumeLayout();
				toolBarDockArea.PerformLayout();
			}
			else
			{
				toolBarDockArea.SuspendLayout();
				point2 = toolBarDockArea.PointToClient(Control.MousePosition);
				_dragged.PreferredDockedLocation = point2;
				toolBarDockArea.ResumeLayout();
				toolBarDockArea.PerformLayout();
			}
		}
		else if (toolBarDockArea == null)
		{
			_dragged.FloatForm.Location = point2;
		}
		else
		{
			toolBarDockArea.SuspendLayout();
			point2 = toolBarDockArea.PointToClient(Control.MousePosition);
			_dragged.DockStyle = toolBarDockArea.Dock;
			_dragged.Parent = toolBarDockArea;
			_dragged.PreferredDockedLocation = point2;
			_dragged.FloatForm.Visible = false;
			_dragged.PreferredDockedArea = toolBarDockArea;
			toolBarDockArea.ResumeLayout();
			toolBarDockArea.PerformLayout();
		}
		_ptStart = Control.MousePosition;
	}

	private void ToolBarMouseUp(object sender, MouseEventArgs e)
	{
		if (_dragged != null)
		{
			_dragged = null;
			_ptOffset.X = 8;
			_ptOffset.Y = 8;
		}
	}

	private void ToolBarDoubleClick(object sender, EventArgs e)
	{
		ToolBarDockHolder toolBarDockHolder = (ToolBarDockHolder)sender;
		if (IsDocked(toolBarDockHolder))
		{
			ToolBarDockArea dockedArea = GetDockedArea(toolBarDockHolder);
			dockedArea.SuspendLayout();
			toolBarDockHolder.Parent = toolBarDockHolder.FloatForm;
			toolBarDockHolder.Location = new Point(0, 0);
			toolBarDockHolder.DockStyle = DockStyle.None;
			toolBarDockHolder.FloatForm.Visible = true;
			toolBarDockHolder.FloatForm.Size = toolBarDockHolder.Size;
			dockedArea.ResumeLayout();
			dockedArea.PerformLayout();
		}
		else
		{
			ToolBarDockArea preferredDockedArea = toolBarDockHolder.PreferredDockedArea;
			preferredDockedArea.SuspendLayout();
			Point preferredDockedLocation = toolBarDockHolder.PreferredDockedLocation;
			toolBarDockHolder.DockStyle = preferredDockedArea.Dock;
			toolBarDockHolder.Parent = preferredDockedArea;
			toolBarDockHolder.PreferredDockedLocation = preferredDockedLocation;
			toolBarDockHolder.FloatForm.Visible = false;
			toolBarDockHolder.PreferredDockedArea = preferredDockedArea;
			preferredDockedArea.ResumeLayout();
			preferredDockedArea.PerformLayout();
		}
	}

	public bool PreFilterMessage(ref Message m)
	{
		if (m.Msg == 256)
		{
			Keys keys = (Keys)((int)m.WParam & 0xFFFF);
			if (keys == Keys.ControlKey)
			{
				if (!_ctrlDown && _dragged != null && IsDocked(_dragged))
				{
					ToolBarDockArea dockedArea = GetDockedArea(_dragged);
					dockedArea.SuspendLayout();
					_dragged.Parent = _dragged.FloatForm;
					_dragged.Location = new Point(0, 0);
					_dragged.DockStyle = DockStyle.None;
					_dragged.FloatForm.Visible = true;
					_dragged.FloatForm.Location = new Point(Control.MousePosition.X - _ptOffset.X, Control.MousePosition.Y - 8);
					_dragged.FloatForm.Size = _dragged.Size;
					dockedArea.ResumeLayout();
					dockedArea.PerformLayout();
				}
				_ctrlDown = true;
			}
		}
		else if (m.Msg == 257)
		{
			Keys keys2 = (Keys)((int)m.WParam & 0xFFFF);
			if (keys2 == Keys.ControlKey)
			{
				if (_ctrlDown && _dragged != null && !IsDocked(_dragged))
				{
					ToolBarDockArea closestArea = GetClosestArea(Control.MousePosition, _dragged.PreferredDockedArea);
					if (closestArea != null)
					{
						closestArea.SuspendLayout();
						Point preferredDockedLocation = closestArea.PointToClient(Control.MousePosition);
						_dragged.DockStyle = closestArea.Dock;
						_dragged.Parent = closestArea;
						_dragged.PreferredDockedLocation = preferredDockedLocation;
						_dragged.FloatForm.Visible = false;
						_dragged.PreferredDockedArea = closestArea;
						closestArea.ResumeLayout();
						closestArea.PerformLayout();
					}
				}
				_ctrlDown = false;
			}
		}
		return false;
	}

	public virtual void ShowContextMenu(Point ptScreen)
	{
		ContextMenu contextMenu = new ContextMenu();
		ArrayList arrayList = new ArrayList();
		arrayList.AddRange(_holders);
		arrayList.Sort(new HolderSorter());
		MyMenuItem[] array = new MyMenuItem[arrayList.Count];
		for (int i = 0; i < arrayList.Count; i++)
		{
			ToolBarDockHolder toolBarDockHolder = arrayList[i] as ToolBarDockHolder;
			Control control = toolBarDockHolder.Control;
			array[i] = new MyMenuItem();
			array[i].Checked = control.Visible;
			array[i].Text = toolBarDockHolder.ToolbarTitle;
			array[i].Click += MenuClickEventHandler;
			array[i].Control = control;
			contextMenu.MenuItems.Add(array[i]);
		}
		contextMenu.Show(DockStation, DockStation.PointToClient(ptScreen));
	}

	protected void MenuClickEventHandler(object sender, EventArgs e)
	{
		MyMenuItem myMenuItem = (MyMenuItem)sender;
		ShowControl(myMenuItem.Control, !myMenuItem.Control.Visible);
	}
}
