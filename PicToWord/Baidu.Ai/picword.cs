using Baidu.Ai.Properties;
using System;
using System.Configuration;
using System.Threading;
using System.Windows.Forms;

namespace Baidu.Ai
{
    public partial class picword : Form
    {
        public picword()
        {
            InitializeComponent();
        }

        private static string path;

        [Obsolete]
        private void button1_Click(object sender, EventArgs e)
        {

            progressBar1.Value = 0;
            textBox1.Text = textBox2.Text = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "图片|*.bmp;*.jpg;*.jpeg;*.gif;*.png|所有文件|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                识别(openFileDialog.FileName);
            }
        }

        private void 识别(string spath)
        {
            path = textBox3.Text = spath;
            progressBar1.Value += progressBar1.Step;//让进度条增加一次
            label1.Text = System.IO.Path.GetFileNameWithoutExtension(path);
            progressBar1.Value += progressBar1.Step;
            pictureBox1.BackgroundImage = null;
            pictureBox1.ImageLocation = path;
            progressBar1.Value += progressBar1.Step;
            线程函数(path);
        }
        private void 线程函数(string sp)
        {
            Thread thread1 = new Thread(new ParameterizedThreadStart(sss1));
            thread1.Priority = ThreadPriority.BelowNormal;
            Thread thread2 = new Thread(new ParameterizedThreadStart(sss2));
            thread2.Priority = ThreadPriority.BelowNormal;
            thread1.Start();
            thread2.Start();
            progressBar1.Value += progressBar1.Step;
        }
        private string ss1;
        private string ss2;
        private void sss1(object str)
        {
            Console.WriteLine("线程1启动");
            ss1 = Bdai.onlygetword(path); 
            if (textBox1.InvokeRequired && progressBar1.InvokeRequired)
            {
                Action<string> actionDelegate = (x) =>
                {
                    progressBar1.Value += progressBar1.Step;
                    progressBar1.Value += progressBar1.Step;
                    progressBar1.Value += progressBar1.Step;
                    this.textBox1.Text = x.ToString();
                };
                this.textBox1.BeginInvoke(actionDelegate, ss1);
            }
            else
            {
                textBox1.Text = ss1;
            }
        }
        private void sss2(object str)
        {
            Console.WriteLine("线程2启动");
            ss2 = Bdai.getwordandfix(path);
            if (textBox2.InvokeRequired && progressBar1.InvokeRequired)
            {
                Action<string> actionDelegate = (x) =>
                {
                    progressBar1.Value += progressBar1.Step*3;
                    this.textBox2.Text = x.ToString();
                };
                this.textBox2.BeginInvoke(actionDelegate, ss2);
            }
            else
            {
                textBox2.Text = ss2;
            }
                
        }

        private void picword_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;  
            else
                e.Effect = DragDropEffects.None;
        }

        private void picword_DragEnter(object sender, DragEventArgs e)
        {
            textBox3.Text = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();//获得路径
            progressBar1.Value = 0;
            textBox1.Text = textBox2.Text = "";
            识别(textBox3.Text);
        }
    }
}

