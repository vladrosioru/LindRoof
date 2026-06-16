using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LindRoof;

public class ListOfPanels : Form
{
	private Button button1;

	private Button button2;

	public ListBox listBox1;

	private Container components;

	public ListOfPanels(ArrayList listOfPanels)
	{
		InitializeComponent();
		foreach (Panel listOfPanel in listOfPanels)
		{
			listBox1.Items.Add(listOfPanel.panelName);
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
		this.button1 = new System.Windows.Forms.Button();
		this.button2 = new System.Windows.Forms.Button();
		this.listBox1 = new System.Windows.Forms.ListBox();
		base.SuspendLayout();
		this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.button1.Location = new System.Drawing.Point(8, 328);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(80, 40);
		this.button1.TabIndex = 1;
		this.button1.Text = "Delete roof slope";
		this.button1.Click += new System.EventHandler(button1_Click);
		this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.button2.Location = new System.Drawing.Point(104, 328);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(80, 40);
		this.button2.TabIndex = 1;
		this.button2.Text = "Abort";
		this.listBox1.Location = new System.Drawing.Point(8, 8);
		this.listBox1.Name = "listBox1";
		this.listBox1.Size = new System.Drawing.Size(176, 316);
		this.listBox1.TabIndex = 2;
		this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
		base.ClientSize = new System.Drawing.Size(192, 374);
		base.Controls.Add(this.listBox1);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.button2);
		base.Name = "ListOfPanels";
		this.Text = "Roof slopes";
		base.ResumeLayout(false);
	}

	private void button1_Click(object sender, EventArgs e)
	{
	}
}
