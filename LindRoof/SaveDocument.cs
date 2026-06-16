using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LindRoof;

public class SaveDocument : Form
{
	private PictureBox pictureBox1;

	private Label label1;

	private Button button1;

	private Button button2;

	private Document parentDocument;

	private Container components;

	public SaveDocument(string name, Document parentDocument1)
	{
		parentDocument = parentDocument1;
		InitializeComponent();
		label1.Text = name;
		Graphics graphics = CreateGraphics();
		base.ClientSize = new Size(50 + (int)graphics.MeasureString(name, new Font("Microsoft Sans Serif", 8.25f)).Width, 94);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LindRoof.SaveDocument));
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.label1 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.button2 = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
		this.pictureBox1.Location = new System.Drawing.Point(10, 18);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(38, 37);
		this.pictureBox1.TabIndex = 0;
		this.pictureBox1.TabStop = false;
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(58, 28);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(218, 17);
		this.label1.TabIndex = 1;
		this.label1.Text = "Save changes from _________ ?";
		this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.button1.Location = new System.Drawing.Point(38, 74);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(77, 28);
		this.button1.TabIndex = 2;
		this.button1.Text = "YES";
		this.button1.Click += new System.EventHandler(button1_Click);
		this.button2.DialogResult = System.Windows.Forms.DialogResult.No;
		this.button2.Location = new System.Drawing.Point(182, 74);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(77, 28);
		this.button2.TabIndex = 3;
		this.button2.Text = "NO";
		this.button2.Click += new System.EventHandler(button2_Click);
		base.AcceptButton = this.button1;
		this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
		base.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		base.CancelButton = this.button2;
		base.ClientSize = new System.Drawing.Size(280, 117);
		base.ControlBox = false;
		base.Controls.Add(this.button2);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.pictureBox1);
		this.MinimumSize = new System.Drawing.Size(298, 162);
		base.Name = "SaveDocument";
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Save file";
		base.TopMost = true;
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		parentDocument.saveItOrNot = "Yes";
	}

	private void button2_Click(object sender, EventArgs e)
	{
		parentDocument.saveItOrNot = "No";
	}

	private void button3_Click(object sender, EventArgs e)
	{
		parentDocument.saveItOrNot = "Abort";
	}
}
