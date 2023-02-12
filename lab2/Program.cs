using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace lab2
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AutorizationController autorizationController = new AutorizationController();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Autorization(autorizationController));
            if (autorizationController.passed)
            {
                if (autorizationController.admin) { Application.Run(new MainProgAdm(autorizationController)); }
                else { Application.Run(new MainProg()); }
            }
            
        }
    }
    public class AutorizationController
    {
        public bool passed = false;
        public bool admin = false;
        public DataContainer dataContainer;
        public User loggedUser = null;
        public AutorizationController()
        {
            dataContainer = new DataContainer(@"..\..\DATA\1.xml");
        }
        public bool SignIn(string Name, string Password)
        {
            loggedUser = dataContainer.Users.FirstOrDefault(u => u.Name == Name && u.Password == Password);
            if(loggedUser == null) { return false; }
            if(loggedUser is Admin) { admin = true; }
            passed = true;
            return true;
        }
    }
    public class DataContainer
    {
        private string path;
        public List<User> Users = new List<User>();
        public DataContainer(string path)
        {
            this.path = path;
            if (File.Exists(path))
            {
                Load(path);
            }
            else
            {
                Create(path);
            }
        }
        private void Create(string Path)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<Users><User><Name>Admin</Name><Password></Password><Blocked>0</Blocked><Admin>1</Admin></User></Users>");
            Users.Add(new User("Admin"));
            doc.Save(Path);
        }
        private void Load(string Path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Path);
            LoadUsers(doc);
        }
        public void Save()
        {
            XmlDocument doc = new XmlDocument();

            File.Delete(path);

            doc.LoadXml("<Users></Users>");
            XmlElement root = doc.DocumentElement;
            foreach(var user in Users)
            {
                XmlElement userElem = doc.CreateElement("User");

                XmlElement nameElem = doc.CreateElement("Name");
                XmlElement passwordElem = doc.CreateElement("Password");
                XmlElement blockedElem = doc.CreateElement("Blocked");

                nameElem.AppendChild(doc.CreateTextNode(user.Name));
                passwordElem.AppendChild(doc.CreateTextNode(user.Password));
                if (user.Blocked) { blockedElem.AppendChild(doc.CreateTextNode("1")); }
                else { blockedElem.AppendChild(doc.CreateTextNode("0")); }
                
                userElem.AppendChild(nameElem);
                userElem.AppendChild(passwordElem);
                userElem.AppendChild(blockedElem);
                if (user is Admin)
                {
                    XmlElement adminElement = doc.CreateElement("Admin");
                    adminElement.AppendChild(doc.CreateTextNode("1"));
                    userElem.AppendChild(adminElement);
                }
                root.AppendChild(userElem);
            }
            doc.Save(path);
        }
        public void LoadUsers(XmlDocument doc)
        {
            XmlElement xRoot = doc.DocumentElement;
            foreach(XmlNode node in xRoot)
            {
                string Name = "";
                string Password = "";
                bool Blocked = false;
                bool Admin = false;
                foreach(XmlNode childNode in node.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case ("Name"):
                            Name = childNode.InnerText.Trim();
                            break;
                        case ("Password"):
                            Password = childNode.InnerText.Trim();
                            break;
                        case ("Blocked"):
                            if (childNode.InnerText == "1") { Blocked = true; }
                            else { Blocked = false; }
                            break;
                        case ("Admin"):
                            if (childNode.InnerText == "1") { Admin = true; }
                            break;
                    }
                }
                if (Admin)
                {
                    Users.Add(new Admin(Name, Password));
                }
                else
                {
                    Users.Add(new User(Name, Password, Blocked));
                }
            }
        }
    }
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; } = "";
        public bool Blocked { get; set; } = false;
        public User(string name)
        {
            Name = name;
        }
        public User(string name, string password, bool blocked)
        {
            Name = name;
            Password = password;
            Blocked = blocked;
        }
    }
    internal class Admin : User
    {
        public Admin(string name = "Admin") : base(name)
        {
        }
        public Admin(string name, string password) : base(name, password, false) { }
    }
}
