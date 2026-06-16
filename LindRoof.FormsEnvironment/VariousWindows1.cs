using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LindRoof.FormsEnvironment;

public class VariousWindows1 : Form
{
	private IContainer components;

	public TextBox textBox1;

	public Label label1;

	public VariousWindows1()
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
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox1.Location = new System.Drawing.Point(12, 12);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(81, 22);
		this.textBox1.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(99, 15);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(45, 16);
		this.label1.TabIndex = 1;
		this.label1.Text = "label1";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(144, 41);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.textBox1);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "VariousWindows1";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "VariousWindows1";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
