using System;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

namespace ProXmlEditor {
    internal partial class EditorUserControl : UserControl {
        public delegate void CaretChangeEventHandler(int line, int column);

        public event CaretChangeEventHandler CaretChangeEvent;

        private bool _textChanged;

        public EditorUserControl() {
            InitializeComponent();

            textEditorControl.TabIndent = 3;
            textEditorControl.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighter("XML");

            textEditorControl.ActiveTextAreaControl.Caret.PositionChanged += Caret_PositionChanged;
            textEditorControl.Document.DocumentChanged += Document_DocumentChanged;
        }

        private void Caret_PositionChanged(object sender, EventArgs e) {
            int line = textEditorControl.ActiveTextAreaControl.Caret.Line + 1;
            int column = textEditorControl.ActiveTextAreaControl.Caret.Column + 1;

            if (CaretChangeEvent != null) {
                CaretChangeEvent(line, column);
            }
        }

        private void Document_DocumentChanged(object sender, DocumentEventArgs e) {
            _textChanged = true;
        }

        public string GetText() {
            return textEditorControl.Text;
        }

        public int GetCountOfLines() {
            return textEditorControl.ActiveTextAreaControl.Document.TotalNumberOfLines;
        }

        public int GetLengthOfLine(int lineN) {
            return textEditorControl.ActiveTextAreaControl.Document.TextLength;
        }
        public void SetText(string text) {
            bool modified = ChangesInEditor;
            textEditorControl.Text = text;
            textEditorControl.Refresh();
            ChangesInEditor = modified;
        }

        public void SetFocus() {
            textEditorControl.Focus();
        }

        public int GetActiveLineNumber() {
            return textEditorControl.ActiveTextAreaControl.Caret.Line;
        }

        public int GetActiveColumnNumber() {
            return textEditorControl.ActiveTextAreaControl.Caret.Column;
        }

        public void GotoLine(int line, int column) {
            textEditorControl.ActiveTextAreaControl.Caret.Line = line - 1;
            textEditorControl.ActiveTextAreaControl.Caret.Column = column - 1;
            textEditorControl.ActiveTextAreaControl.CenterViewOn(line - 1, 0);
            textEditorControl.Focus();
        }

        public bool ChangesInEditor {
            get { return _textChanged; }
            set { _textChanged = value; }
        }

        private void toolStripMenuItemRedo_Click(object sender, EventArgs e) {
            textEditorControl.Redo();
        }

        private void toolStripMenuItemUndo_Click(object sender, EventArgs e) {
            textEditorControl.Undo();
        }

        private void contextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
            if (textEditorControl.Document.UndoStack.CanUndo) {
                toolStripMenuItemUndo.Enabled = true;
            }
            else {
                toolStripMenuItemUndo.Enabled = false;
            }

            if (textEditorControl.Document.UndoStack.CanRedo) {
                toolStripMenuItemRedo.Enabled = true;
            }
            else {
                toolStripMenuItemRedo.Enabled = false;
            }
        }
        
        private void toolStripMenuItemCut_Click(object sender, EventArgs e) {
            textEditorControl.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(sender, e);
        }

        private void toolStripMenuItemCopy_Click(object sender, EventArgs e) {
            textEditorControl.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(sender, e);
        }

        private void toolStripMenuItemPaste_Click(object sender, EventArgs e) {
            textEditorControl.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(sender, e);
        }

        private void toolStripMenuItemSelectAll_Click(object sender, EventArgs e) {
            SelectAll();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if (textEditorControl.ActiveTextAreaControl.TextArea.Focused) {
                if ((int) keyData == 131137) 
                {
                    SelectAll();
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SelectAll() {
            TextLocation startPosition = new TextLocation(0, 0);

            int textLength = textEditorControl.ActiveTextAreaControl.Document.TextLength;
            TextLocation endPosition = new TextLocation();
            endPosition.Column = textEditorControl.Document.OffsetToPosition(textLength).Column;
            endPosition.Line = textEditorControl.Document.OffsetToPosition(textLength).Line;

            textEditorControl.ActiveTextAreaControl.SelectionManager.SetSelection(startPosition, endPosition);
            textEditorControl.ActiveTextAreaControl.Caret.Position = endPosition;
        }

        private void toolStripMenuItemDelete_Click(object sender, EventArgs e) {
            textEditorControl.ActiveTextAreaControl.TextArea.ClipboardHandler.Delete(sender, e);
        }

        private void addNodeToolStripMenuItem_Click(object sender, EventArgs e) {
            var addNodeDialog = new AddNodeForm();
            addNodeDialog.ShowDialog();
            textEditorControl.ActiveTextAreaControl.TextArea.InsertString(addNodeDialog.XmlNodeStr);
        }

        
    }
}
