using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LindRoof.toolbar;

public class ToolBarDockArea : UserControl
{
	private class LineHolder
	{
		public ArrayList Columns = new ArrayList();

		public int Index;

		public int Size;

		public LineHolder(int index)
		{
			Index = index;
		}

		public void AddColumn(ColumnHolder column)
		{
			int num = 0;
			foreach (ColumnHolder column2 in Columns)
			{
				if (column2.Position > column.Position)
				{
					Columns.Insert(num, column);
					break;
				}
				num++;
			}
			if (num == Columns.Count)
			{
				Columns.Add(column);
			}
		}

		public void Distribute()
		{
			int num = 0;
			foreach (ColumnHolder column in Columns)
			{
				if (column.Position < num)
				{
					column.Position = num;
				}
				num = column.Position + column.Size;
			}
		}
	}

	private class ColumnHolder
	{
		public int Position;

		public int Size;

		public ToolBarDockHolder Holder;

		public ColumnHolder(int pos, ToolBarDockHolder holder, int size)
		{
			Position = pos;
			Holder = holder;
			Size = size;
		}
	}

	private ToolBarManager _dockManager;

	private Container components;

	private int _lastLineCount = 1;

	public ToolBarManager DockManager => _dockManager;

	public bool Horizontal
	{
		get
		{
			if (Dock != DockStyle.Left)
			{
				return Dock != DockStyle.Right;
			}
			return false;
		}
	}

	public ToolBarDockArea(ToolBarManager dockManager, DockStyle dockStyle)
	{
		InitializeComponent();
		SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, value: true);
		_dockManager = dockManager;
		DockManager.DockStation.Controls.Add(this);
		if (dockStyle == DockStyle.Fill || dockStyle == DockStyle.None)
		{
			dockStyle = DockStyle.Top;
		}
		Dock = dockStyle;
		SendToBack();
		FitHolders();
		base.Layout += LayoutHandler;
	}

	public void LayoutHandler(object sender, LayoutEventArgs e)
	{
		SuspendLayout();
		int lineSz = 23;
		SortedList sortedList = new SortedList();
		foreach (ToolBarDockHolder control in base.Controls)
		{
			if (control.Visible)
			{
				int preferredLine = GetPreferredLine(lineSz, control);
				int preferredPosition = GetPreferredPosition(control);
				LineHolder lineHolder = (LineHolder)sortedList[preferredLine];
				if (lineHolder == null)
				{
					lineHolder = new LineHolder(preferredLine);
					sortedList.Add(preferredLine, lineHolder);
				}
				int holderWidth = GetHolderWidth(control);
				int holderLineSize = GetHolderLineSize(control);
				lineHolder.AddColumn(new ColumnHolder(preferredPosition, control, holderWidth + 1));
				if (lineHolder.Size - 1 < holderLineSize)
				{
					lineHolder.Size = holderLineSize + 1;
				}
			}
		}
		int num = 0;
		_lastLineCount = sortedList.Count;
		if (_lastLineCount == 0)
		{
			_lastLineCount = 1;
		}
		for (int i = 0; i < sortedList.Count; i++)
		{
			LineHolder lineHolder2 = (LineHolder)sortedList.GetByIndex(i);
			if (lineHolder2 == null)
			{
				continue;
			}
			lineHolder2.Distribute();
			foreach (ColumnHolder column in lineHolder2.Columns)
			{
				if (Horizontal)
				{
					column.Holder.Location = new Point(column.Position, num);
					column.Holder.PreferredDockedLocation = new Point(column.Holder.PreferredDockedLocation.X, num + column.Holder.Height / 2);
				}
				else
				{
					column.Holder.Location = new Point(num, column.Position);
					column.Holder.PreferredDockedLocation = new Point(num + column.Holder.Width / 2, column.Holder.PreferredDockedLocation.Y);
				}
			}
			num += lineHolder2.Size + 1;
		}
		FitHolders();
		ResumeLayout();
	}

	protected int GetPreferredLine(int lineSz, ToolBarDockHolder holder)
	{
		int num;
		if (Horizontal)
		{
			num = holder.PreferredDockedLocation.Y;
			_ = holder.Size.Height;
			if (num < 0)
			{
				return int.MinValue;
			}
			if (num > base.Height)
			{
				return int.MaxValue;
			}
		}
		else
		{
			num = holder.PreferredDockedLocation.X;
			_ = holder.Size.Width;
			if (num < 0)
			{
				return int.MinValue;
			}
			if (num > base.Width)
			{
				return int.MaxValue;
			}
		}
		int num2 = num / lineSz;
		int num3 = num2 * lineSz;
		if (num3 + 3 > num)
		{
			return num2 * 2;
		}
		if (num3 + lineSz - 3 < num)
		{
			return num2 * 2 + 2;
		}
		return num2 * 2 + 1;
	}

	protected int GetPreferredPosition(ToolBarDockHolder holder)
	{
		if (Horizontal)
		{
			return holder.PreferredDockedLocation.X;
		}
		return holder.PreferredDockedLocation.Y;
	}

	protected int GetHolderLineSize(ToolBarDockHolder holder)
	{
		if (Horizontal)
		{
			return holder.Height;
		}
		return holder.Width;
	}

	protected int GetMyLineSize()
	{
		if (Horizontal)
		{
			return base.Height;
		}
		return base.Width;
	}

	protected int GetHolderWidth(ToolBarDockHolder holder)
	{
		if (Horizontal)
		{
			return holder.Width;
		}
		return holder.Height;
	}

	protected void FitHolders()
	{
		Size size = new Size(0, 0);
		foreach (Control control in base.Controls)
		{
			if (control.Visible)
			{
				if (control.Right > size.Width)
				{
					size.Width = control.Right;
				}
				if (control.Bottom > size.Height)
				{
					size.Height = control.Bottom;
				}
			}
		}
		if (Horizontal)
		{
			base.Height = size.Height;
		}
		else
		{
			base.Width = size.Width;
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
		base.Name = "ToolBarDockArea";
		base.MouseUp += new System.Windows.Forms.MouseEventHandler(ToolBarDockArea_MouseUp);
	}

	private void ToolBarDockArea_MouseUp(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Right)
		{
			DockManager.ShowContextMenu(PointToScreen(new Point(e.X, e.Y)));
		}
	}
}
