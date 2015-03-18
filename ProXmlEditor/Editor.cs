using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;


namespace ProXmlEditor {
    public partial class Editor : Form {
        private int TabCount;

        public Editor() {
            InitializeComponent();
        }


        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            AddTab();
            tabControl1.SelectedIndex += 1;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            Open();
        }

        private void Editor_Load(object sender, EventArgs e) {
            AddTab();
        }

        private void AddTab() {
            var Body = new EditorUserControl();

            Body.Name = "Body";
            Body.Dock = DockStyle.Fill;

            Body.SetText("");
            var NewPage = new TabPage();
            TabCount += 1;

            string DocumentText = "Document " + TabCount;
            NewPage.Name = DocumentText;
            NewPage.Text = DocumentText;
            NewPage.Controls.Add(Body);

            tabControl1.TabPages.Add(NewPage);
        }

        private void Open() {
            AddTab();
            tabControl1.SelectedIndex += 1;
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog1.Filter = "XML|*.xml|Text Files|*.txt|All Files|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                var SR = new StreamReader(openFileDialog1.FileName);
                GetXmlEditor().SetText(SR.ReadToEnd());
                SR.Close();
            }
        }

        private void AddNode(XmlNode inXmlNode, TreeNode inTreeNode) {
            XmlNode xNode;
            TreeNode tNode;
            XmlNodeList nodeList;
            int i = 0;
            if (inXmlNode.HasChildNodes) {
                nodeList = inXmlNode.ChildNodes;
                for (i = 0; i <= nodeList.Count - 1; i++) {
                    xNode = inXmlNode.ChildNodes[i];
                    inTreeNode.Nodes.Add(new TreeNode(xNode.Name));
                    tNode = inTreeNode.Nodes[i];
                    AddNode(xNode, tNode);
                }
            }
            else {
                inTreeNode.Text = inXmlNode.InnerText;
            }
        }

        private EditorUserControl GetXmlEditor() {
            if (tabControl1.TabPages.Count != 0) {
                return (EditorUserControl) tabControl1.SelectedTab.Controls["Body"];
            }
            MessageBox.Show("You donn`t have any tab");
            AddTab();
            return (EditorUserControl) tabControl1.SelectedTab.Controls["Body"];
            ;
        }

        private void refreshBtn_Click(object sender, EventArgs e) {
            try {
                var xmldoc = new XmlDocument();
                XmlNode xmlnode;
                string xmlText = GetXmlEditor().GetText();
                if (xmlText != null) {
                    xmldoc.LoadXml(xmlText);
                    xmlnode = xmldoc.ChildNodes[1];
                    treeView1.Nodes.Clear();
                    treeView1.Nodes.Add(new TreeNode(xmldoc.DocumentElement.Name));
                    TreeNode tNode;
                    tNode = treeView1.Nodes[0];
                    AddNode(xmlnode, tNode);
                }
            }
            catch (XmlException exception) {
                MessageBox.Show(exception.Message);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveAs();
        }

        private void Save() {
            //saveFileDialog1.FileName = tabControl1.SelectedTab.Name;
            //saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //saveFileDialog1.Filter = "XML|*.xml|Text Files|*.txt|All Files|*.*";
            //saveFileDialog1.Title = "Save";
            if (openFileDialog1.FileName == "") {
                SaveAs();
            }
            else {
                var SW = new StreamWriter(openFileDialog1.FileName, false, Encoding.UTF8);
                SW.WriteLine(GetXmlEditor().GetText());
                SW.Close();
            }
        }

        private void SaveAs() {
            saveFileDialog1.FileName = tabControl1.SelectedTab.Name;
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog1.Filter = "XML|*.xml|Text Files|*.txt|All Files|*.*";
            saveFileDialog1.Title = "Save As";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
                var SW = new StreamWriter(saveFileDialog1.FileName);
                SW.WriteLine(GetXmlEditor().GetText());
                SW.Close();
            }
        }

        private void RemoveTab() {
            if (tabControl1.TabPages.Count != 1) {
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            }
            else {
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                AddTab();
            }
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e) {
        }

        private void newBtn_Click(object sender, EventArgs e) {
            AddTab();
        }

        private void openBtn_Click(object sender, EventArgs e) {
            Open();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e) {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
        }

        private void closeBtn_Click(object sender, EventArgs e) {
            RemoveTab();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            using (AboutForm af = new AboutForm()) {
                string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                af.ProgramVersion = version.Substring(0, version.Length - 4);
                af.Title = Util.GetTitle();
                af.ShowDialog();
            }
        }
    }
}