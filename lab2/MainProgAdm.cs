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
    public partial class MainProgAdm : Form
    {
        AutorizationController autorizationController;
        public MainProgAdm(AutorizationController autorizationController)
        {
            this.autorizationController = autorizationController;
            InitializeComponent();
            RefreshListView();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            var current = listView1.FocusedItem;
            var cur = 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ConfirmAcition())
            {
                ChangePassword();
            }
            RefreshListView();
            autorizationController.dataContainer.Save();
        }
        private bool ConfirmAcition()
        {
            ConfirmAction confirm = new ConfirmAction(autorizationController.loggedUser.Password);
            confirm.ShowDialog();
            return confirm.pass;
        }
        private void ChangePassword()
        {
            PasswordChanger passwordChanger = new PasswordChanger(autorizationController.loggedUser);
            passwordChanger.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(!autorizationController.dataContainer.Users.Any(u => u.Name == textBox1.Text))
            {
                autorizationController.dataContainer.Users.Add(new User(textBox1.Text));
                RefreshListView();
            }
        }
        void RefreshListView()
        {
            listView1.Items.Clear();
            foreach (var user in autorizationController.dataContainer.Users)
            {
                ListViewItem item = new ListViewItem(user.Name);
                item.SubItems.Add(user.Password);
                item.SubItems.Add(user.Blocked.ToString());
                bool admin = false;
                if (user is Admin) { admin = true; }
                item.SubItems.Add(admin.ToString());
                listView1.Items.Add(item);
            }
        }
    }
}
