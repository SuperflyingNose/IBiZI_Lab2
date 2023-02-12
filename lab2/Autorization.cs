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
    public partial class Autorization : Form
    {
        AutorizationController controller;
        short tries = 3;
        public Autorization(AutorizationController autrizationController)
        {
            controller = autrizationController;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(controller.SignIn(textName.Text, textPass.Text))
            {
                Close();
            }
            else
            {
                tries--;
            }
            if(tries == 0) { Close(); }
        }
    }
}
