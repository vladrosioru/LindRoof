using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LindRoof;

[ToolboxBitmap(typeof(StatusBarEx))]
public class StatusBarEx : StatusBar
{
	public class StatusBarPanelExCollection : StatusBarPanelCollection, IEnumerable
	{
		public new StatusBarPanelEx this[int index]
		{
			get
			{
				return (StatusBarPanelEx)base[index];
			}
			set
			{
				base[index] = value;
			}
		}

		public StatusBarPanelExCollection(StatusBarEx owner)
			: base(owner)
		{
		}
	}

	private Container components;

	private StatusBarPanelExCollection panels;

	private Timer timer;

	[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Editor(typeof(StatusBarPanelExCollectionEditor), typeof(UITypeEditor))]
	public new StatusBarPanelExCollection Panels => panels;

	private void InitializeComponent()
	{
		base.Name = "StatusBarEx";
	}

	public StatusBarEx()
	{
		components = new Container();
		panels = new StatusBarPanelExCollection(this);
		base.SizingGrip = false;
		base.ShowPanels = true;
		timer = new Timer(components);
		timer.Interval = 1000;
		timer.Enabled = true;
	}

	[Description("Update any progress bar panel(s) within this statusbar.")]
	public void UpdateValue()
	{
		IEnumerator enumerator = Panels.GetEnumerator();
		while (enumerator.MoveNext())
		{
			StatusBarPanelEx statusBarPanelEx = (StatusBarPanelEx)enumerator.Current;
			if (statusBarPanelEx.Style.ToString().EndsWith("ProgressBar"))
			{
				statusBarPanelEx.Value++;
				Invalidate(invalidateChildren: true);
			}
		}
	}

	[Description("Update given progress bar panels value to the new value.")]
	public void UpdateValue(StatusBarPanelEx panel, int NewValue)
	{
		panel.Value = NewValue;
		Invalidate(invalidateChildren: true);
	}

	[Description("Update given progress bar panels value by one.")]
	public void UpdateValue(StatusBarPanelEx panel)
	{
		panel.Value++;
		Invalidate(invalidateChildren: true);
	}

	protected override void OnDrawItem(StatusBarDrawItemEventArgs e)
	{
		if (e.Panel.GetType().ToString().EndsWith("StatusBarPanelEx"))
		{
			StatusBarPanelEx statusBarPanelEx = (StatusBarPanelEx)e.Panel;
			if (statusBarPanelEx.Style.ToString().EndsWith("ProgressBar"))
			{
				if (statusBarPanelEx.Value > statusBarPanelEx.Minimum)
				{
					int num = (int)((double)statusBarPanelEx.Value / (double)statusBarPanelEx.Maximum * (double)statusBarPanelEx.Width);
					Rectangle bounds = e.Bounds;
					Brush brush = ((statusBarPanelEx.Style != StatusBarPanelStyleEx.SmoothProgressBar) ? ((Brush)new HatchBrush(statusBarPanelEx.HatchedProgressBarStyle, statusBarPanelEx.ForeColor, base.Parent.BackColor)) : ((Brush)new SolidBrush(statusBarPanelEx.ForeColor)));
					bounds.Width = num;
					e.Graphics.FillRegion(brush, new Region(bounds));
					brush.Dispose();
				}
				else
				{
					base.OnDrawItem(e);
				}
			}
		}
		else
		{
			base.OnDrawItem(e);
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

	private void TimerEventProcessor(object myObject, EventArgs myEventArgs)
	{
		IEnumerator enumerator = Panels.GetEnumerator();
		while (enumerator.MoveNext())
		{
			((IStatusBarPanelExRefresh)enumerator.Current).Refresh();
		}
	}
}
