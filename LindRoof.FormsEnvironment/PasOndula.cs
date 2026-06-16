using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LindRoof.FormsEnvironment;

public class PasOndula : Form
{
	private IContainer components;

	public Label label1;

	public MaskedTextBox textBox1;

	private Button button1;

	public PasOndula()
	{
		InitializeComponent();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		if (textBox1.Text != "")
		{
			try
			{
				if (int.Parse(textBox1.Text) >= 100)
				{
					if (button1.DialogResult != DialogResult.OK)
					{
						button1.DialogResult = DialogResult.OK;
						button1.PerformClick();
					}
				}
				else
				{
					MessageBox.Show(this, "Value too low (min 100mm)");
					button1.DialogResult = DialogResult.None;
				}
				return;
			}
			catch
			{
				MessageBox.Show(this, "Invalid value");
				button1.DialogResult = DialogResult.None;
				return;
			}
		}
		MessageBox.Show(this, "Invalid value");
		button1.DialogResult = DialogResult.None;
	}

	private void PasOndula_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar == '\r')
		{
			button1.PerformClick();
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
		this.label1 = new System.Windows.Forms.Label();
		this.textBox1 = new System.Windows.Forms.MaskedTextBox();
		this.button1 = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(99, 15);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(45, 16);
		this.label1.TabIndex = 1;
		this.label1.Text = "label1";
		this.textBox1.AsciiOnly = true;
		this.textBox1.BeepOnError = true;
		this.textBox1.Location = new System.Drawing.Point(12, 11);
		this.textBox1.Mask = "00000";
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(81, 20);
		this.textBox1.TabIndex = 2;
		this.textBox1.Text = "100";
		this.textBox1.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
		this.textBox1.ValidatingType = typeof(int);
		this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(PasOndula_KeyPress);
		this.button1.Location = new System.Drawing.Point(12, 40);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(120, 28);
		this.button1.TabIndex = 3;
		this.button1.Text = "OK";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.button1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(PasOndula_KeyPress);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(144, 72);
		base.ControlBox = false;
		base.Controls.Add(this.button1);
		base.Controls.Add(this.textBox1);
		base.Controls.Add(this.label1);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "PasOndula";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "VariousWindows1";
		base.KeyPress += new System.Windows.Forms.KeyPressEventHandler(PasOndula_KeyPress);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
