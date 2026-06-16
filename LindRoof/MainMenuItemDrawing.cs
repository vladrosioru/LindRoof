using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace LindRoof;

public class MainMenuItemDrawing
{
	public static void DrawMenuItem(DrawItemEventArgs e, MenuItem mi)
	{
		Rectangle rectangle = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height - 1);
		if ((e.State & DrawItemState.HotLight) == DrawItemState.HotLight)
		{
			DrawHoverRect(e, mi);
		}
		else if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
		{
			DrawSelectionRect(e, mi);
		}
		else
		{
			e.Graphics.FillRectangle(new SolidBrush(Globals.MainColor), rectangle);
			e.Graphics.DrawRectangle(new Pen(Globals.MainColor), rectangle);
		}
		StringFormat stringFormat = new StringFormat();
		stringFormat.Alignment = StringAlignment.Center;
		stringFormat.HotkeyPrefix = HotkeyPrefix.Show;
		SolidBrush brush = new SolidBrush(Globals.TextColor);
		e.Graphics.DrawString(mi.Text, Globals.menuFont, brush, rectangle, stringFormat);
	}

	private static void DrawHoverRect(DrawItemEventArgs e, MenuItem mi)
	{
		Rectangle rect = new Rectangle(e.Bounds.X, e.Bounds.Y + 1, e.Bounds.Width, e.Bounds.Height - 2);
		Brush brush = new LinearGradientBrush(rect, Color.White, Globals.CheckBoxColor, 90f, isAngleScaleable: false);
		e.Graphics.FillRectangle(brush, rect);
		e.Graphics.DrawRectangle(new Pen(Color.Black), rect);
	}

	private static void DrawSelectionRect(DrawItemEventArgs e, MenuItem mi)
	{
		Rectangle rect = new Rectangle(e.Bounds.X, e.Bounds.Y + 1, e.Bounds.Width, e.Bounds.Height - 2);
		Brush brush = new LinearGradientBrush(rect, Globals.MenuBgColor, Globals.MenuDarkColor2, 90f, isAngleScaleable: false);
		e.Graphics.FillRectangle(brush, rect);
		e.Graphics.DrawRectangle(new Pen(Color.Gray), rect);
	}
}
