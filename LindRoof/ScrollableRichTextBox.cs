using System;
using System.Windows.Forms;

namespace LindRoof;

public class ScrollableRichTextBox : RichTextBox
{
	private const int WM_VSCROLL = 277;

	private const int WM_LBUTTONDOWN = 513;

	private const int WM_SETFOCUS = 7;

	private const int WM_KILLFOCUS = 8;

	private readonly IntPtr SB_ENDSCROLL = (IntPtr)8;

	private readonly IntPtr SB_BOTTOM = (IntPtr)7;

	private bool _scrollable = true;

	private object _scrollLock = new object();

	public void AppendText(string text, bool scrollToEnd)
	{
		lock (_scrollLock)
		{
			if (IntPtr.Zero != base.Handle)
			{
				decimal num = base.Text.Length + text.Length;
				if (num >= (decimal)base.MaxLength)
				{
					Clear();
				}
				base.Text += text;
				if (_scrollable && scrollToEnd && IntPtr.Zero != base.Handle)
				{
					base.SelectionStart = base.Text.Length;
					Message m = Message.Create(base.Handle, 277, SB_BOTTOM, IntPtr.Zero);
					WndProc(ref m);
				}
			}
		}
	}
}
