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
    public partial class PasswordChanger : Form
    {
        User user;
        public PasswordChanger(User user)
        {
            InitializeComponent();
            this.user = user;   
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox2.Text == textBox3.Text)
            {
                user.Password = textBox2.Text;
                Close();
            }
            else
            {
                label1.Text = "Passwords dont match";
            }
        }
    }
}
