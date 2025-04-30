using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml;
using System.IO;

namespace dotnet_lab_13
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string filePath = "people.xml";
        private void btnCreate_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration decl = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(decl);

            XmlElement company = doc.CreateElement("Company");
            doc.AppendChild(company);

            doc.Save(filePath);
            MessageBox.Show("XML-файл создан.");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Сначала создайте XML-файл.");
                return;
            }

            string nameText = txtName.Text;
            string ageText = txtAge.Text;
            string positionText = txtPosition.Text;
            string departmentText = txtDepartment.Text;

            if (nameText == "" || ageText == "" || positionText == "" || departmentText == "")
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            string deptName = txtDepartment.Text.Trim();
            XmlElement company = doc.DocumentElement;

            XmlElement department = null;

            // Проверка: существует ли уже такой отдел
            foreach (XmlElement dept in company.GetElementsByTagName("Department"))
            {
                if (dept.GetAttribute("name") == deptName)
                {
                    department = dept;
                    break;
                }
            }

            // Если нет — создаем
            if (department == null)
            {
                department = doc.CreateElement("Department");
                department.SetAttribute("name", deptName);
                company.AppendChild(department);
            }

            XmlElement person = doc.CreateElement("Person");

            XmlElement name = doc.CreateElement("Name");
            name.InnerText = nameText;
            person.AppendChild(name);

            XmlElement age = doc.CreateElement("Age");
            age.InnerText = ageText;
            person.AppendChild(age);

            XmlElement position = doc.CreateElement("Position");
            position.InnerText = positionText;
            person.AppendChild(position);

            department.AppendChild(person);
            doc.Save(filePath);

            MessageBox.Show("Сотрудник добавлен.");
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Файл не найден.");
                return;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            listBox1.Items.Clear();

            foreach (XmlNode dept in doc.SelectNodes("//Department"))
            {
                string deptName = ((XmlElement)dept).GetAttribute("name");

                foreach (XmlNode person in dept.SelectNodes("Person"))
                {
                    string name = person["Name"]?.InnerText ?? "";
                    string age = person["Age"]?.InnerText ?? "";
                    string position = person["Position"]?.InnerText ?? "";

                    listBox1.Items.Add($"{deptName}: {name}, {age} лет, {position}");
                }
            }
        }
    }
}
