using System;
using System.ComponentModel.Design;

namespace LindRoof;

public class StatusBarPanelExCollectionEditor : CollectionEditor
{
	private Type[] types;

	public StatusBarPanelExCollectionEditor(Type type)
		: base(type)
	{
		types = new Type[1] { typeof(StatusBarPanelEx) };
	}

	protected override Type[] CreateNewItemTypes()
	{
		return types;
	}
}
