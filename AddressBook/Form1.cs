using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace AddressBook
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<Person> person = new List<Person>();

        private void Form1_Load(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (!Directory.Exists(path + "\\Address Book -Ani"))
                Directory.CreateDirectory(path + "\\Address Book -Ani");
            if (!File.Exists(path + "\\Address Book -Ani\\settings.xml"))
            {
                XmlTextWriter xw = new XmlTextWriter(path + "\\Address Book -Ani\\settings.xml", Encoding.UTF8);
                //File.Create(path + "\\Address Book -Ani\\settings.xml");
                xw.WriteStartElement("People");
                xw.WriteEndElement();
                xw.Close();
            }
            XmlDocument Doc = new XmlDocument();
            Doc.Load(path + "\\Address Book -Ani\\settings.xml");
            foreach (XmlNode node in Doc.SelectNodes("People/Person"))
            {
                Person p = new Person();
                p.Name = node.SelectSingleNode("Name").InnerText;
                p.Email = node.SelectSingleNode("Email").InnerText;
                p.StreetAddress = node.SelectSingleNode("Address").InnerText;
                p.Notes = node.SelectSingleNode("Notes").InnerText;
                p.Birthday = DateTime.FromFileTime(Convert.ToInt64(node.SelectSingleNode("Birthday").InnerText));
                person.Add(p);
                listView1.Items.Add(p.Name);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Person p = new Person();
            p.Name = textBox1.Text;
            p.Email = textBox2.Text;
            p.Birthday = dateTimePicker1.Value;
            p.Notes = textBox4.Text;
            person.Add(p);
            listView1.Items.Add(p.Name);
            textBox1.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
            dateTimePicker1.Value = DateTime.Now;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = person[listView1.SelectedItems[0].Index].Name;
            textBox2.Text = person[listView1.SelectedItems[0].Index].Email;
            textBox4.Text = person[listView1.SelectedItems[0].Index].Notes;
            dateTimePicker1.Value = person[listView1.SelectedItems[0].Index].Birthday;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Remove();
        }

        public void Remove()
        {
            try
            {
                listView1.Items.Remove(listView1.SelectedItems[0]);
                person.RemoveAt(listView1.SelectedItems[0].Index);
            }
            catch { }
        }
        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Remove();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            person[listView1.SelectedItems[0].Index].Name = textBox1.Text;
            person[listView1.SelectedItems[0].Index].Email = textBox2.Text;
            person[listView1.SelectedItems[0].Index].Notes = textBox4.Text;
            person[listView1.SelectedItems[0].Index].Birthday = dateTimePicker1.Value;
            listView1.SelectedItems[0].Text = textBox1.Text;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            XmlDocument Doc = new XmlDocument();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Doc.Load(path + "\\Address Book -Ani\\settings.xml");
            XmlNode node = Doc.SelectSingleNode("People");
            node.RemoveAll();
            foreach (Person p in person)
            {
                XmlNode top = Doc.CreateElement("Person");
                XmlNode name = Doc.CreateElement("Name");
                XmlNode email = Doc.CreateElement("Email");
                XmlNode notes = Doc.CreateElement("Notes");
                XmlNode birthday = Doc.CreateElement("Birthday");
                name.InnerText = p.Name;
                email.InnerText = p.Email;
                notes.InnerText = p.Notes;
                birthday.InnerText = p.Birthday.ToFileTime().ToString();
                top.AppendChild(name);
                top.AppendChild(email);
                top.AppendChild(notes);
                top.AppendChild(birthday);
                Doc.DocumentElement.AppendChild(top);
            }
            Doc.Save(path + "\\Address Book -Ani\\settings.xml");
        }
    }

    class Person
    {
        public string Name
        {
            get;
            set;
        }
        public string Email
        {
            get;
            set;
        }
        public string StreetAddress
        {
            get;
            set;
        }
        public string Notes
        {
            get;
            set;
        }
        public DateTime Birthday
        {
            get;
            set;
        }
    }
}
