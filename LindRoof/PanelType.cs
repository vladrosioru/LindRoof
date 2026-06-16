using System;

namespace LindRoof;

public class PanelType : IComparable
{
	public PanelObject po;

	public int count;

	public PanelType(PanelObject p, int c)
	{
		po = p;
		count = c;
	}

	public int CompareTo(object obj)
	{
		PanelType panelType = (PanelType)obj;
		return po.height.CompareTo(panelType.po.height);
	}
}
