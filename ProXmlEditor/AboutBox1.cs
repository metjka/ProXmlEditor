using System;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;

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
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(GetDefaultWebBrowser());
            psi.Arguments = "https://github.com/metjka/ProXmlEditor";
            System.Diagnostics.Process.Start(psi);
            Close();
        }

        private void mailLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            System.Diagnostics.Process.Start("mailto:w50901@student.wsiz.rzeszow.pl");
            Close();
        }

        public static string GetDefaultWebBrowser() {
            RegistryKey rk = Registry.ClassesRoot;
            RegistryKey sk = rk.CreateSubKey(@"HTTP\shell\open\command");

            string returnValue = "";

            if (sk != null) {
                returnValue = sk.GetValue(null).ToString();

                int startPos = returnValue.IndexOf((char)34);
                int endPos = returnValue.IndexOf((char)34, startPos + 1);

                returnValue = returnValue.Substring(startPos + 1, endPos - 1);
            }

            if (returnValue == "") {
                returnValue = "iexplore.exe";
            }

            return returnValue;
        }
    }
}
