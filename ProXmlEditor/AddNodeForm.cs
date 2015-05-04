using System;
using System.Windows.Forms;

namespace ProXmlEditor {
    public partial class AddNodeForm : Form {
        public AddNodeForm() {
            InitializeComponent();
        }
        public string XmlNodeStr;
        private void AddNodeForm_Load(object sender, EventArgs e) {
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            textBox4.Text = textBox1.Text;
            TextChanger();
        }

        private void button1_Click(object sender, EventArgs e) {
            Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e) {
            TextChanger();
        }

        private void TextChanger() {
            string nodeName = textBox1.Text;
            string nodeAtri = textBox2.Text;
            XmlNodeStr = string.Format("<{0}>{1}</{0}>", nodeName, nodeAtri);
            textBox3.Text = XmlNodeStr;
        }
    }
}
