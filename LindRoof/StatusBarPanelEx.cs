using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace LindRoof;

[DesignerCategory("Component")]
[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class StatusBarPanelEx : StatusBarPanel, IStatusBarPanelExRefresh
{
	private int m_Minimum = 1;

	private int m_Maximum = 100;

	private int m_Value;

	private Color m_Color;

	private HatchStyle hatchStyle;

	private StatusBarPanelStyleEx style;

	[Category("ProgressBar Panel")]
	[Description("Minimum value the progress bar can have. (If this panel acts as a progress bar)")]
	public int Minimum
	{
		get
		{
			return m_Minimum;
		}
		set
		{
			if (value < 0)
			{
				m_Minimum = 0;
			}
			if (value > m_Minimum)
			{
				m_Minimum = value;
				m_Minimum = value;
			}
			if (m_Value < m_Minimum)
			{
				m_Value = m_Minimum;
			}
		}
	}

	[Category("ProgressBar Panel")]
	[Description("Maximum value the progress bar can have. (If this panel acts as a progress bar)")]
	public int Maximum
	{
		get
		{
			return m_Maximum;
		}
		set
		{
			if (value < m_Minimum)
			{
				m_Minimum = value;
			}
			m_Maximum = value;
			if (m_Value > m_Maximum)
			{
				m_Value = m_Maximum;
			}
		}
	}

	[Description("Value of the progress bar. (If this panel acts as a progress bar)")]
	[Category("ProgressBar Panel")]
	public int Value
	{
		get
		{
			return m_Value;
		}
		set
		{
			m_Value = value;
		}
	}

	[Category("ProgressBar Panel")]
	[Description("Progress bar color. (If this panel acts as a progress bar)")]
	public Color ForeColor
	{
		get
		{
			return m_Color;
		}
		set
		{
			m_Color = value;
		}
	}

	[Description("Style of the Hatched progress bar. Drawing2D.HatchStyles are available.")]
	[Category("ProgressBar Style")]
	public HatchStyle HatchedProgressBarStyle
	{
		get
		{
			return hatchStyle;
		}
		set
		{
			hatchStyle = value;
		}
	}

	[Category("Appearance")]
	public new StatusBarPanelStyleEx Style
	{
		get
		{
			return style;
		}
		set
		{
			style = value;
			if (style == StatusBarPanelStyleEx.OwnerDraw || style == StatusBarPanelStyleEx.SmoothProgressBar || style == StatusBarPanelStyleEx.HatchedProgressBar)
			{
				base.Style = StatusBarPanelStyle.OwnerDraw;
			}
			else
			{
				base.Style = StatusBarPanelStyle.Text;
			}
		}
	}

	public StatusBarPanelEx()
	{
		Style = StatusBarPanelStyleEx.Text;
	}

	public void Refresh()
	{
		DateTime now = DateTime.Now;
		if (style == StatusBarPanelStyleEx.Date)
		{
			base.Text = now.ToString("d", Thread.CurrentThread.CurrentUICulture);
		}
		if (style == StatusBarPanelStyleEx.Time)
		{
			base.Text = now.ToString("T", Thread.CurrentThread.CurrentUICulture);
		}
	}
}
