using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace ProXmlEditor {
    public partial class EditorXml : Form {
        public EditorXml() {
            InitializeComponent();
        }

        private int _tabCount;

        private void AddTab() {
            var body = new EditorUserControl {Name = "body", Dock = DockStyle.Fill};

            body.SetText("");
            var newPage = new TabPage();
            _tabCount += 1;

            string documentText = "Xml file " + _tabCount;
            newPage.Name = documentText;
            newPage.Text = documentText;
            newPage.Controls.Add(body);

            tabControl1.TabPages.Add(newPage);
        }

        private void Open() {
            AddTab();
            tabControl1.SelectedIndex += 1;
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog1.Filter = "XML|*.xml|Text Files|*.txt|All Files|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                var sr = new StreamReader(openFileDialog1.FileName);
                GetXmlEditor().SetText(sr.ReadToEnd());
                sr.Close();
            }
            tabControl1.SelectedTab.Text = openFileDialog1.SafeFileName;
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
            if (tabControl1.TabPages.Count > 1) {
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            }
            else if (tabControl1.TabPages.Count == 1) {
                AddTab();
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                
            }
        }

        private void ExpandTree() {
            treeView1.ExpandAll();
            if (treeView1.TopNode != null) {
                treeView1.Nodes[0].EnsureVisible();
            }
        }

        private void CollapseTree() {
            treeView1.CollapseAll();
            if (treeView1.TopNode != null) {
                treeView1.Nodes[0].Expand();
            }
        }

        private void XmlTreeMaker() {
            try {
                XmlDocument xmldoc = new XmlDocument();
                string xmlText = GetXmlEditor().GetText();
                xmldoc.LoadXml(xmlText);
                XmlNode xmlnode = xmldoc.ChildNodes[1];
                treeView1.Nodes.Clear();
                treeView1.Nodes.Add(new TreeNode(xmldoc.DocumentElement.Name));
                TreeNode tNode;
                tNode = treeView1.Nodes[0];
                AddNode(xmlnode, tNode);
                textBox1.Text = "Xml file is valid";
            }
            catch (XmlException e) {
                textBox1.Text = e.Message;
                treeView1.Nodes.Clear();
            }
        }


        private static void AddNode(XmlNode inXmlNode, TreeNode inTreeNode) {
            XmlNode xNode;
            XmlNodeList nodeList;

            if (inXmlNode.HasChildNodes) {
                nodeList = inXmlNode.ChildNodes;

                for (int i = 0; i <= nodeList.Count - 1; i++) {
                    xNode = inXmlNode.ChildNodes[i];

                    StringBuilder attributes = new StringBuilder();

                    if (xNode.Attributes != null) {
                        foreach (XmlAttribute attribute in xNode.Attributes) {

                            attributes.Append(string.Format(" {0}=\"{1}\"", attribute.Name, attribute.Value));
                        }
                    }
                    inTreeNode.Nodes.Add(new TreeNode(string.Format("{0}{1}", xNode.Name, attributes)));
                    TreeNode tNode = inTreeNode.Nodes[i];
                    AddNode(xNode, tNode);
                }
            }
            else {
                if (inXmlNode.InnerText == "") {
                    inTreeNode.BackColor = Color.FromArgb(50, 206,0,0);
                    inTreeNode.ForeColor = Color.Wheat;
                    inTreeNode.Text = (inXmlNode.Name).Trim();
                }
                else {
                    inTreeNode.Text = (inXmlNode.InnerText);
                }
        }
        }

        private EditorUserControl GetXmlEditor() {
            return (EditorUserControl)tabControl1.SelectedTab.Controls["body"];
        }


        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            AddTab();
            tabControl1.SelectedIndex += 1;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            Open();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveAs();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e) {
            XmlTreeMaker();
            ExpandTree();
        }



        private void newBTN_Click(object sender, EventArgs e) {
            AddTab();
            tabControl1.SelectedIndex += 1;
        }

        private void openBTN_Click(object sender, EventArgs e) {
            Open();
            XmlTreeMaker();
            ExpandTree();
        }

        private void refreshBTN_Click(object sender, EventArgs e) {
            XmlTreeMaker();
            ExpandTree();
        }

        private void removeBTN_Click(object sender, EventArgs e) {
            RemoveTab();
        }

        private void EditorXML_Load(object sender, EventArgs e) {
            AddTab();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            using (var af = new AboutForm()) {
                string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                af.ProgramVersion = version.Substring(0, version.Length - 4);

                Assembly thisAssembly = Assembly.GetExecutingAssembly();
                object[] attributes = thisAssembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                string title = "";

                if (attributes.Length == 1) {
                    title = ((AssemblyTitleAttribute)attributes[0]).Title;
                }

                
                af.Title = title;
                af.ShowDialog();
            }
        }

        private void expandBtn_Click(object sender, EventArgs e) {
            ExpandTree();
        }

        private void collapsBtn_Click(object sender, EventArgs e) {
            CollapseTree();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) {
            XmlTreeMaker();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e) {
            RemoveTab();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e) {
            var obj = e.Node.Tag as XmlNode;
            if (obj != null) {
                /*if ((obj.LineNumber != 0) && (obj.LineNumber < GetXmlEditor().GetCountOfLines())) {
                    int length = 0;
                    for (int i = 0; i < obj.LineNumber - 1; i++) {
                        length += GetXmlEditor().GetCountOfLines();

                        MessageBox.Show(obj.LineNumber.ToString());
                    }
                }
                */
            }
        }

        
        /*
        private void tvSchema_AfterSelect(object sender, TreeViewEventArgs e) {
            var obj = e.Node.Tag as XmlSchemaObject;
            // if this is a schema object...
            if (obj != null) {
                // find the corresponding line in the XSD source and highlight it
                if ((obj.LineNumber != 0) && (obj.LineNumber < edSchema.Lines.Length)) {
                    int length = 0;
                    for (int i = 0; i < obj.LineNumber - 1; i++) {
                        // get the length of the line including CRLF
                        length += edSchema.Lines[i].Length + 2;
                    }
                    // highlight the line
                    edSchema.Select(length, edSchema.Lines[obj.LineNumber - 1].Length);
                }

                // Update simple and global type combo boxes.
                // Get the name of the element type so that we can find it in the simple or global type list
                XmlQualifiedName name = null; // assume failure
                if (obj is XmlSchemaAttribute) {
                    // this is easy.
                    name = ((XmlSchemaAttribute)obj).SchemaTypeName;
                }
                else if (obj is XmlSchemaSimpleType) {
                    // if it's a simple type, get the restriction type, if it exists
                    var restriction = ((XmlSchemaSimpleType)obj).Content as XmlSchemaSimpleTypeRestriction;
                    if (restriction != null) {
                        name = restriction.BaseTypeName;
                    }
                }
                else if (obj is XmlSchemaElement) {
                    // if it's an element, determine if it's a simple type subnode of a complex type...
                    // and then get the restriction type, if it exists
                    var el = obj as XmlSchemaElement;
                    if (el.SchemaType is XmlSchemaSimpleType) {
                        var st = el.SchemaType as XmlSchemaSimpleType;
                        var rest = st.Content as XmlSchemaSimpleTypeRestriction;
                        if (rest != null) {
                            name = rest.BaseTypeName;
                        }
                    }
                    else {
                        // otherwise get the element name
                        name = el.SchemaTypeName;

                        // if the name is null, then there must be a reference instead
                        if (name.Name == "") {
                            name = el.RefName;
                        }
                    }
                }

                // select the name from either the simple type list or the global element type list
                if (name != null) {
                    // see if the name exists in the simple type list
                    int idx = cbSimpleTypes.FindStringExact(name.Name);
                    cbSimpleTypes.SelectedIndex = idx;
                    cbSimpleTypes.Enabled = true;

                    // see if the name exists in the complex type list
                    idx = cbGlobalTypes.FindStringExact(name.Name);
                    cbGlobalTypes.SelectedIndex = idx;
                    cbGlobalTypes.Enabled = true;
                }
                else {
                    // if there is no name, then disable the comboboxes
                    cbSimpleTypes.SelectedIndex = -1;
                    cbGlobalTypes.SelectedIndex = -1;
                    cbSimpleTypes.Enabled = false;
                    cbGlobalTypes.Enabled = false;
                }
            }
            else {
                // if this isn't a schema object, then disable the comboboxes
                cbSimpleTypes.SelectedIndex = -1;
                cbGlobalTypes.SelectedIndex = -1;
                cbSimpleTypes.Enabled = false;
                cbGlobalTypes.Enabled = false;
            }
        }
        */
        
    }
}
