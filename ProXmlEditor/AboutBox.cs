using System;
using System.Reflection;
using System.Windows.Forms;

namespace ProXmlEditor {
    internal partial class AboutForm : Form {
        public AboutForm() {
            InitializeComponent();
            SetAboutFormProperties();
        }

        private void SetAboutFormProperties() {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();

            object[] attributes = thisAssembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

            string title = "";

            if (attributes.Length == 1) {
                title = ((AssemblyTitleAttribute)attributes[0]).Title;
            }

            Text = title;
        }

        public string ProgramVersion {
            set { versionLabel.Text = versionLabel.Text + " " + value; }
        }

        public string Title {
            set { titleLabel.Text = value; }
        }

        private void button1_Click(object sender, EventArgs e) {
            Close();
        }

        private void urlLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            System.Diagnostics.Process.Start("https://github.com/metjka/ProXmlEditor/");
            Close();
        }

        private void mailLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            System.Diagnostics.Process.Start("mailto:w50901@student.wsiz.rzeszow.pl");
            Close();
        }
    }
}
