using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LuckyOutes
{
    // 定义一个比较器
   
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // Set the form properties
            this.StartPosition = FormStartPosition.CenterScreen;
            
        }
        ThreadStart ts;
        Thread t;
        delegate void StringArgsDelegate(string s);
        
        private void Form1_Load(object sender, EventArgs e)
        {
            ts = new ThreadStart(UpdateUI);
            t = new Thread(ts);
            t.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (t.IsAlive)
            {
                t.Abort();
                button1.Text = "重新抽取";
            }
            else
            {
                t = new Thread(ts);
                t.Start();
                button1.Text = "抽取";
            }

           
        }
        private void UpdateLabel(string s)
        {
            label2.Text = s;
           
        }
        private void UpdateUI()
        {
            string path = "抽奖名单.txt";
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);
            List<string> names = sr.ReadToEnd().Split(new char[] { '\n' }).ToList();
            while (true)
            {
                names.Sort(new RandomComparer());
                foreach (string name in names)
                {
                    this.BeginInvoke(new StringArgsDelegate(UpdateLabel), name);
                    Thread.Sleep(20);
                }
            }
            
        }
    }
    class RandomComparer : IComparer<string>
    {
        Random random = new Random();
        public int Compare(string x, string y)
        {
            // 返回-1或1，让Sort()方法随机比较元素，从而实现随机排序
            return random.Next(0, 2) * 2 - 1;
        }
    }
}
