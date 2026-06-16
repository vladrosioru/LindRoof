using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using AxSHDocVw;

namespace LindRoof;

public class Help : Form
{
	private AxWebBrowser axWebBrowser1;

	private object o;

	private Container components;

	public Help()
	{
		InitializeComponent();
		string uRL = Environment.CurrentDirectory.ToString() + "\\help.htm";
		axWebBrowser1.Navigate(uRL, ref o, ref o, ref o, ref o);
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
		System.Resources.ResourceManager resourceManager = new System.Resources.ResourceManager(typeof(LindRoof.Help));
		this.axWebBrowser1 = new AxSHDocVw.AxWebBrowser();
		((System.ComponentModel.ISupportInitialize)this.axWebBrowser1).BeginInit();
		base.SuspendLayout();
		this.axWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.axWebBrowser1.Enabled = true;
		this.axWebBrowser1.Location = new System.Drawing.Point(0, 0);
		this.axWebBrowser1.OcxState = (System.Windows.Forms.AxHost.State)resourceManager.GetObject("axWebBrowser1.OcxState");
		this.axWebBrowser1.Size = new System.Drawing.Size(784, 614);
		this.axWebBrowser1.TabIndex = 0;
		this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
		base.ClientSize = new System.Drawing.Size(784, 614);
		base.Controls.Add(this.axWebBrowser1);
		base.Name = "Help";
		this.Text = "Help";
		((System.ComponentModel.ISupportInitialize)this.axWebBrowser1).EndInit();
		base.ResumeLayout(false);
	}
}
