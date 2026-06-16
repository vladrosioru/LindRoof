using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Windows.Forms;

namespace LindRoof;

public class SplashForm : Form
{
	private PictureBox pictureBox1;

	private Timer timer1;

	private IContainer components;

	public SplashForm()
	{
		InitializeComponent();
		base.FormBorderStyle = FormBorderStyle.None;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.StartPosition = FormStartPosition.CenterScreen;
		base.ControlBox = false;
		string name = "LindRoof.SplashScreen.sync.bmp";
		Stream stream = null;
		try
		{
			stream = GetType().Assembly.GetManifestResourceStream(name);
			pictureBox1.Image = new Bitmap(stream);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message.ToString());
		}
		finally
		{
			stream?.Close();
		}
		timer1.Start();
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
		this.components = new System.ComponentModel.Container();
		System.Resources.ResourceManager resourceManager = new System.Resources.ResourceManager(typeof(LindRoof.SplashForm));
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		base.SuspendLayout();
		this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pictureBox1.Location = new System.Drawing.Point(0, 0);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(712, 351);
		this.pictureBox1.TabIndex = 0;
		this.pictureBox1.TabStop = false;
		this.timer1.Interval = 800;
		this.timer1.Tick += new System.EventHandler(timer1_Tick);
		this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
		base.ClientSize = new System.Drawing.Size(712, 351);
		base.Controls.Add(this.pictureBox1);
		base.Icon = (System.Drawing.Icon)resourceManager.GetObject("$this.Icon");
		base.Name = "SplashForm";
		this.Text = "SplashForm";
		base.ResumeLayout(false);
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		timer1.Stop();
		Close();
	}
}
