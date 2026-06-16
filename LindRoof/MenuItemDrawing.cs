using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;

namespace LindRoof;

public class MenuItemDrawing
{
	public static void DrawMenuItem(DrawItemEventArgs e, MenuItem mi)
	{
		if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
		{
			DrawSelectionRect(e, mi);
		}
		else
		{
			e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
			DrawPictureArea(e, mi);
		}
		if ((e.State & DrawItemState.Checked) == DrawItemState.Checked)
		{
			DrawCheckBox(e, mi);
		}
		DrawMenuText(e, mi);
		DrawItemPicture(e, mi);
	}

	private static void DrawMenuText(DrawItemEventArgs e, MenuItem mi)
	{
		Brush brush = new SolidBrush(Globals.TextColor);
		if (mi.Text == "-")
		{
			e.Graphics.DrawLine(new Pen(Globals.MenuLightColor), e.Bounds.X + Globals.PIC_AREA_SIZE + 3, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Y + 2);
			return;
		}
		StringFormat stringFormat = new StringFormat();
		stringFormat.LineAlignment = StringAlignment.Center;
		stringFormat.HotkeyPrefix = HotkeyPrefix.Show;
		RectangleF layoutRectangle = new Rectangle(Globals.PIC_AREA_SIZE + 2, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
		string text = mi.Text;
		brush = ((!mi.Enabled) ? new SolidBrush(Globals.TextDisabledColor) : new SolidBrush(Globals.TextColor));
		e.Graphics.DrawString(text, Globals.menuFont, brush, layoutRectangle, stringFormat);
		DrawShortCutText(e, mi);
	}

	private static void DrawShortCutText(DrawItemEventArgs e, MenuItem mi)
	{
		if (mi.Shortcut != Shortcut.None && mi.ShowShortcut)
		{
			string text = TypeDescriptor.GetConverter(typeof(Keys)).ConvertToString((Keys)mi.Shortcut);
			SizeF sizeF = e.Graphics.MeasureString(text, Globals.menuFont);
			Rectangle rectangle = new Rectangle(e.Bounds.Width - Convert.ToInt32(sizeF.Width) - Globals.PIC_AREA_SIZE, e.Bounds.Y, Convert.ToInt32(sizeF.Width) + 5, e.Bounds.Height);
			StringFormat stringFormat = new StringFormat();
			stringFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
			stringFormat.LineAlignment = StringAlignment.Center;
			stringFormat.HotkeyPrefix = HotkeyPrefix.Show;
			if (mi.Enabled)
			{
				e.Graphics.DrawString(text, Globals.menuFont, new SolidBrush(Globals.TextColor), rectangle, stringFormat);
			}
			else
			{
				e.Graphics.DrawString(text, Globals.menuFont, new SolidBrush(Globals.TextDisabledColor), rectangle, stringFormat);
			}
		}
	}

	private static void DrawPictureArea(DrawItemEventArgs e, MenuItem mi)
	{
		Rectangle rect = new Rectangle(e.Bounds.X - 1, e.Bounds.Y, Globals.PIC_AREA_SIZE, e.Bounds.Height);
		Brush brush = new LinearGradientBrush(rect, Globals.MenuDarkColor2, Globals.MenuLightColor2, 180f, isAngleScaleable: false);
		e.Graphics.FillRectangle(brush, rect);
	}

	private static void DrawItemPicture(DrawItemEventArgs e, MenuItem mi)
	{
		Image itemPicture = OfficeMenus.GetItemPicture(mi);
		if (itemPicture != null)
		{
			int num = ((itemPicture.Width > 16) ? 16 : itemPicture.Width);
			int num2 = ((itemPicture.Height > 16) ? 16 : itemPicture.Height);
			int x = e.Bounds.X + 2;
			int y = e.Bounds.Y + (e.Bounds.Height - num2) / 2;
			Rectangle destRect = new Rectangle(x, y, num, num2);
			if (mi.Enabled)
			{
				e.Graphics.DrawImage(itemPicture, x, y, num, num2);
				return;
			}
			ColorMatrix colorMatrix = new ColorMatrix();
			colorMatrix.Matrix00 = 0f;
			colorMatrix.Matrix11 = 0f;
			colorMatrix.Matrix22 = 0f;
			colorMatrix.Matrix33 = 1.3f;
			colorMatrix.Matrix44 = 0.5f;
			ImageAttributes imageAttributes = new ImageAttributes();
			imageAttributes.SetColorMatrix(colorMatrix);
			e.Graphics.DrawImage(itemPicture, destRect, 0, 0, num, num2, GraphicsUnit.Pixel, imageAttributes);
		}
	}

	private static void DrawSelectionRect(DrawItemEventArgs e, MenuItem mi)
	{
		if (mi.Enabled)
		{
			e.Graphics.FillRectangle(new SolidBrush(Globals.SelectionColor), e.Bounds);
			e.Graphics.DrawRectangle(new Pen(Color.Black), e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
		}
	}

	private static void DrawCheckBox(DrawItemEventArgs e, MenuItem mi)
	{
		int num = Globals.PIC_AREA_SIZE - 5;
		e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
		Rectangle rect = new Rectangle(e.Bounds.X + 1, e.Bounds.Y + (e.Bounds.Height - num) / 2, num, num);
		Pen pen = new Pen(Color.Black, 1.7f);
		if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
		{
			e.Graphics.FillRectangle(new SolidBrush(Globals.DarkCheckBoxColor), rect);
		}
		else
		{
			e.Graphics.FillRectangle(new SolidBrush(Globals.CheckBoxColor), rect);
		}
		e.Graphics.DrawRectangle(new Pen(Globals.MenuDarkColor), rect);
		Bitmap itemPicture = OfficeMenus.GetItemPicture(mi);
		if (itemPicture == null)
		{
			e.Graphics.DrawLine(pen, e.Bounds.X + 7, e.Bounds.Y + 10, e.Bounds.X + 10, e.Bounds.Y + 14);
			e.Graphics.DrawLine(pen, e.Bounds.X + 10, e.Bounds.Y + 14, e.Bounds.X + 15, e.Bounds.Y + 9);
		}
	}
}
