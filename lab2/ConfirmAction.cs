using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab2
{
    public partial class ConfirmAction : Form
    {
        string password;
        public bool pass = false;
        public ConfirmAction(string password)
        {
            this.password = password;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == password)
            {
                pass = true;
                Close();
            }
            else
            {
                WrongPasswordLabel.Text = "Wrong Password";
            }
        }
    }
}
