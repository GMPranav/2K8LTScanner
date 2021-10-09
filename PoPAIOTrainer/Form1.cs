using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Memory;

namespace PoPAIOTrainer
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		public Mem m = new Mem();

		private void Form1_Load(object sender, EventArgs e)
		{
		}

		private void numericUpDownChartMinx_ValueChanged(object sender, EventArgs e)
		{
			numericUpDown2.Minimum = numericUpDown1.Value + 1;
		}

		private void numericUpDownChartMaxx_ValueChanged(object sender, EventArgs e)
		{
			numericUpDown1.Maximum = numericUpDown2.Value - 1;
		}

		private void numericUpDownChartMiny_ValueChanged(object sender, EventArgs e)
		{
			numericUpDown3.Minimum = numericUpDown4.Value + 1;
		}

		private void numericUpDownChartMaxy_ValueChanged(object sender, EventArgs e)
		{
			numericUpDown4.Maximum = numericUpDown3.Value - 1;
		}

		private void numericUpDownChartMinz_ValueChanged(object sender, EventArgs e)
		{
			numericUpDown5.Minimum = numericUpDown6.Value + 1;
		}

		private void numericUpDownChartMaxz_ValueChanged(object sender, EventArgs e)
		{
			numericUpDown6.Maximum = numericUpDown5.Value - 1;
		}

		private void button1_Click(object sender, EventArgs e)
        {
			button1.Invoke((MethodInvoker)delegate
			{
				button1.Text = "Scanning...";
				button1.Enabled = false;
			});

			Thread.Sleep(100);
			int pid = 0;
			bool openproc = false;

			if (m.GetProcIdFromName("PrinceOfPersia_Launcher") > 0)
			{
				pid = m.GetProcIdFromName("PrinceOfPersia_Launcher");
				openproc = m.OpenProcess(pid);
				gameLabel.Invoke((MethodInvoker)delegate
				{
					gameLabel.Text = "YES";
				});
				pidLabel.Invoke((MethodInvoker)delegate
				{
					pidLabel.Text = pid.ToString();
				});

				int ymin = Convert.ToInt32(numericUpDown4.Value);
				int ymax = Convert.ToInt32(numericUpDown3.Value);
				int zmin = Convert.ToInt32(numericUpDown6.Value);
				int zmax = Convert.ToInt32(numericUpDown5.Value);
				int xmin = Convert.ToInt32(numericUpDown1.Value);
				int xmax = Convert.ToInt32(numericUpDown2.Value);
				int step = Convert.ToInt32(numericUpDown7.Value);


				for (int y = ymax; y >= ymin; y-=step)	//y is swept in reverse intentionally
				{
					for (int z = zmin; z <= zmax; z+=step)
					{
						for (int x = xmin; x <= xmax; x+=step)
						{
							int f = m.ReadInt("base+0xA44FC4");
							m.WriteMemory("base+0x00A45B04,0x7BC,0x40,0x0,0xB8,0x30", "float", x.ToString());
							m.WriteMemory("base+0x00A45B04,0x7BC,0x40,0x0,0xB8,0x34", "float", y.ToString());
							m.WriteMemory("base+0x00A45B04,0x7BC,0x40,0x0,0xB8,0x38", "float", z.ToString());
							while (f == m.ReadInt("base+0xA44FC4")) { continue; };	//Do not advance till the next frame
						}
					}
				}

			}
			else
			{
				gameLabel.Invoke((MethodInvoker)delegate
				{
					gameLabel.Text = "NO";
				});
				pidLabel.Invoke((MethodInvoker)delegate
				{
					pidLabel.Text = "?";
				});
			}
			button1.Invoke((MethodInvoker)delegate
			{
				button1.Enabled = true;
				button1.Text = "Start";
			});
		}
    }
}
