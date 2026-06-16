using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LindRoof.FormsEnvironment;

public class SheetTypeChoice : Form
{
	private IContainer components;

	public Button butonulApasat;

	public bool choice;

	public Label label1;

	public SheetTypeChoice()
	{
		InitializeComponent();
	}

	public void btn_Click(object sender, EventArgs e)
	{
		butonulApasat = (Button)sender;
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
		this.label1 = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(12, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(137, 16);
		this.label1.TabIndex = 1;
		this.label1.Text = "Sheet Type Selection";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(215, 309);
		base.Controls.Add(this.label1);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "SheetTypeChoice";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "SheetType";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
