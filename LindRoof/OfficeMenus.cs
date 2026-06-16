using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LindRoof;

[ToolboxBitmap(typeof(OfficeMenus), "LindRoof.OfficeMenus.bmp")]
public class OfficeMenus : Component
{
	private Container components;

	private static ImageList _imageList;

	private static NameValueCollection picDetails = new NameValueCollection();

	public ImageList ImageList
	{
		get
		{
			return _imageList;
		}
		set
		{
			_imageList = value;
		}
	}

	public OfficeMenus(IContainer container)
	{
		container.Add(this);
		InitializeComponent();
	}

	public OfficeMenus()
	{
		InitializeComponent();
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
		components = new Container();
	}

	public void Start(Form form)
	{
		try
		{
			MainMenu menu = form.Menu;
			foreach (MenuItem menuItem in menu.MenuItems)
			{
				menuItem.MeasureItem += mainMenuItem_MeasureItem;
				menuItem.DrawItem += mainMenuItem_DrawItem;
				menuItem.OwnerDraw = true;
				InitMenuItem(menuItem);
			}
			ContextMenu contextMenu = form.ContextMenu;
			if (contextMenu != null)
			{
				InitMenuItem(contextMenu);
			}
			foreach (Control control in form.Controls)
			{
				if (control.ContextMenu != null)
				{
					InitMenuItem(control.ContextMenu);
				}
			}
		}
		catch
		{
		}
	}

	public void End(Form form)
	{
		try
		{
			MainMenu menu = form.Menu;
			foreach (MenuItem menuItem in menu.MenuItems)
			{
				menuItem.MeasureItem -= mainMenuItem_MeasureItem;
				menuItem.DrawItem -= mainMenuItem_DrawItem;
				menuItem.OwnerDraw = false;
				UninitMenuItem(menuItem);
			}
			ContextMenu contextMenu = form.ContextMenu;
			if (contextMenu != null)
			{
				UninitMenuItem(contextMenu);
			}
			foreach (Control control in form.Controls)
			{
				if (control.ContextMenu != null)
				{
					UninitMenuItem(control.ContextMenu);
				}
			}
		}
		catch
		{
		}
	}

	private void InitMenuItem(Menu mi)
	{
		foreach (MenuItem menuItem in mi.MenuItems)
		{
			menuItem.MeasureItem += menuItem_MeasureItem;
			menuItem.DrawItem += menuItem_DrawItem;
			menuItem.OwnerDraw = true;
			InitMenuItem(menuItem);
		}
	}

	private void UninitMenuItem(Menu mi)
	{
		foreach (MenuItem menuItem in mi.MenuItems)
		{
			menuItem.MeasureItem -= menuItem_MeasureItem;
			menuItem.DrawItem -= menuItem_DrawItem;
			menuItem.OwnerDraw = false;
			UninitMenuItem(menuItem);
		}
	}

	private void menuItem_MeasureItem(object sender, MeasureItemEventArgs e)
	{
		MenuItem menuItem = (MenuItem)sender;
		if (menuItem.Text == "-")
		{
			e.ItemHeight = 7;
			return;
		}
		SizeF sizeF = e.Graphics.MeasureString(menuItem.Text, Globals.menuFont);
		int num = 0;
		if (menuItem.Shortcut != Shortcut.None)
		{
			num = Convert.ToInt32(e.Graphics.MeasureString(menuItem.Shortcut.ToString(), Globals.menuFont).Width);
		}
		int num2 = Convert.ToInt32(sizeF.Height) + 7;
		if (num2 < 25)
		{
			num2 = Globals.MIN_MENU_HEIGHT;
		}
		e.ItemHeight = num2;
		e.ItemWidth = Convert.ToInt32(sizeF.Width) + num + Globals.PIC_AREA_SIZE * 2;
	}

	private void menuItem_DrawItem(object sender, DrawItemEventArgs e)
	{
		MenuItemDrawing.DrawMenuItem(e, (MenuItem)sender);
	}

	private void mainMenuItem_MeasureItem(object sender, MeasureItemEventArgs e)
	{
		MenuItem menuItem = (MenuItem)sender;
		e.ItemWidth = Convert.ToInt32(e.Graphics.MeasureString(menuItem.Text, Globals.menuFont).Width);
	}

	private void mainMenuItem_DrawItem(object sender, DrawItemEventArgs e)
	{
		MainMenuItemDrawing.DrawMenuItem(e, (MenuItem)sender);
	}

	public void AddPicture(MenuItem mi, int index)
	{
		picDetails.Add(mi.Handle.ToString(), index.ToString());
	}

	public static Bitmap GetItemPicture(MenuItem mi)
	{
		if (_imageList == null)
		{
			return null;
		}
		string[] values = picDetails.GetValues(mi.Handle.ToString());
		if (values == null)
		{
			return null;
		}
		return (Bitmap)_imageList.Images[Convert.ToInt32(values[0])];
	}
}
