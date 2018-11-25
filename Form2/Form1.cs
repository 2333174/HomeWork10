using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sourse;
using System.Text.RegularExpressions;
using Error;

namespace Form2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            string pattern1 = "^[0-9]{4}(0[1-9]|1[0-2])(0[1-9]|[1-2][0-9]|3[0-1])[0-9]{3}";
            string pattern2 = "[0-9]{11}";
            if (Regex.IsMatch(this.textBox1.Text, pattern1))
            {
                if( Regex.IsMatch(this.textBox7.Text,pattern2)) this.DialogResult = DialogResult.OK;
                else
                {
                    Error.Form1 formn = new Error.Form1();
                    formn.label1.Text = "电话格式有误，请重新输入！！！";
                    formn.ShowDialog();
                    if (formn.DialogResult == DialogResult.OK)
                    {
                        this.textBox7.Text = "";
                    }
                }
            }
            else
            {
                Error.Form1 formn = new Error.Form1();
                formn.label1.Text = "订单号格式有误，请重新输入！！！";
                formn.ShowDialog();
                if (formn.DialogResult == DialogResult.OK)
                {
                    this.textBox1.Text = "";
                }
            }
  
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = "";
            this.textBox2.Text = "";
            this.textBox3.Text = "";
            this.textBox4.Text = "";
            this.textBox5.Text = "";
            this.textBox6.Text = "";
            this.textBox7.Text = "";
        }
    }
}
