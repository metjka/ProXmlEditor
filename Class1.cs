// Decompiled with JetBrains decompiler
// Type: ICSharpCode.TextEditor.TextAreaClipboardHandler
// Assembly: ICSharpCode.TextEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=4d61825e8dd49f1a
// MVID: 4B5D72EB-DE12-48E6-9F0B-8DEB62E5DD1D
// Assembly location: D:\Dropbox\Development\c#\ProXmlEditor\ProXmlEditor\ICSharpCode.TextEditor.dll

using ICSharpCode.TextEditor.Actions;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor.Util;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ICSharpCode.TextEditor {
    public class TextAreaClipboardHandler {
        private const string LineSelectedType = "MSDEVLineSelect";
        private TextArea textArea;
        public static TextAreaClipboardHandler.ClipboardContainsTextDelegate GetClipboardContainsText;
        [ThreadStatic]
        private static int SafeSetClipboardDataVersion;

        public bool EnableCut {
            get {
                return this.textArea.EnableCutOrPaste;
            }
        }

        public bool EnableCopy {
            get {
                return true;
            }
        }

        public bool EnablePaste {
            get {
                if (!this.textArea.EnableCutOrPaste)
                    return false;
                TextAreaClipboardHandler.ClipboardContainsTextDelegate containsTextDelegate = TextAreaClipboardHandler.GetClipboardContainsText;
                if (containsTextDelegate != null)
                    return containsTextDelegate();
                try {
                    return Clipboard.ContainsText();
                }
                catch (ExternalException ex) {
                    return false;
                }
            }
        }

        public bool EnableDelete {
            get {
                if (this.textArea.SelectionManager.HasSomethingSelected)
                    return !this.textArea.SelectionManager.SelectionIsReadonly;
                return false;
            }
        }

        public bool EnableSelectAll {
            get {
                return true;
            }
        }

        public event CopyTextEventHandler CopyText;

        public TextAreaClipboardHandler(TextArea textArea) {
            this.textArea = textArea;
            textArea.SelectionManager.SelectionChanged += new EventHandler(this.DocumentSelectionChanged);
        }

        private void DocumentSelectionChanged(object sender, EventArgs e) {
        }

        private bool CopyTextToClipboard(string stringToCopy, bool asLine) {
            if (stringToCopy.Length <= 0)
                return false;
            DataObject dataObject = new DataObject();
            dataObject.SetData(DataFormats.UnicodeText, true, (object)stringToCopy);
            if (asLine) {
                MemoryStream memoryStream = new MemoryStream(1);
                memoryStream.WriteByte((byte)1);
                dataObject.SetData("MSDEVLineSelect", false, (object)memoryStream);
            }
            if (this.textArea.Document.HighlightingStrategy.Name != "Default")
                dataObject.SetData(DataFormats.Rtf, (object)RtfWriter.GenerateRtf(this.textArea));
            this.OnCopyText(new CopyTextEventArgs(stringToCopy));
            TextAreaClipboardHandler.SafeSetClipboard((object)dataObject);
            return true;
        }

        private static void SafeSetClipboard(object dataObject) {
            int version = ++TextAreaClipboardHandler.SafeSetClipboardDataVersion;
            try {
                Clipboard.SetDataObject(dataObject, true);
            }
            catch (ExternalException ex1) {
                Timer timer = new Timer();
                timer.Interval = 100;
                timer.Tick += (EventHandler)delegate {
                    timer.Stop();
                    timer.Dispose();
                    if (TextAreaClipboardHandler.SafeSetClipboardDataVersion != version)
                        return;
                    try {
                        Clipboard.SetDataObject(dataObject, true, 10, 50);
                    }
                    catch (ExternalException ex) {
                    }
                };
                timer.Start();
            }
        }

        private bool CopyTextToClipboard(string stringToCopy) {
            return this.CopyTextToClipboard(stringToCopy, false);
        }

        public void Cut(object sender, EventArgs e) {
            if (this.textArea.SelectionManager.HasSomethingSelected) {
                if (!this.CopyTextToClipboard(this.textArea.SelectionManager.SelectedText) || this.textArea.SelectionManager.SelectionIsReadonly)
                    return;
                this.textArea.BeginUpdate();
                this.textArea.Caret.Position = this.textArea.SelectionManager.SelectionCollection[0].StartPosition;
                this.textArea.SelectionManager.RemoveSelectedText();
                this.textArea.EndUpdate();
            }
            else {
                if (!this.textArea.Document.TextEditorProperties.CutCopyWholeLine)
                    return;
                int lineNumberForOffset = this.textArea.Document.GetLineNumberForOffset(this.textArea.Caret.Offset);
                LineSegment lineSegment = this.textArea.Document.GetLineSegment(lineNumberForOffset);
                string text = this.textArea.Document.GetText(lineSegment.Offset, lineSegment.TotalLength);
                this.textArea.SelectionManager.SetSelection(this.textArea.Document.OffsetToPosition(lineSegment.Offset), this.textArea.Document.OffsetToPosition(lineSegment.Offset + lineSegment.TotalLength));
                if (!this.CopyTextToClipboard(text, true) || this.textArea.SelectionManager.SelectionIsReadonly)
                    return;
                this.textArea.BeginUpdate();
                this.textArea.Caret.Position = this.textArea.Document.OffsetToPosition(lineSegment.Offset);
                this.textArea.SelectionManager.RemoveSelectedText();
                this.textArea.Document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.PositionToEnd, new TextLocation(0, lineNumberForOffset)));
                this.textArea.EndUpdate();
            }
        }

        public void Copy(object sender, EventArgs e) {
            if (this.CopyTextToClipboard(this.textArea.SelectionManager.SelectedText) || !this.textArea.Document.TextEditorProperties.CutCopyWholeLine)
                return;
            LineSegment lineSegment = this.textArea.Document.GetLineSegment(this.textArea.Document.GetLineNumberForOffset(this.textArea.Caret.Offset));
            this.CopyTextToClipboard(this.textArea.Document.GetText(lineSegment.Offset, lineSegment.TotalLength), true);
        }

        public void Paste(object sender, EventArgs e) {
            if (!this.textArea.EnableCutOrPaste)
                return;
            int num = 0;
            while (true) {
                try {
                    IDataObject dataObject = Clipboard.GetDataObject();
                    if (dataObject == null)
                        break;
                    bool dataPresent = dataObject.GetDataPresent("MSDEVLineSelect");
                    if (!dataObject.GetDataPresent(DataFormats.UnicodeText))break;
                    string str = (string)dataObject.GetData(DataFormats.UnicodeText);
                    if (string.IsNullOrEmpty(str))
                        break;
                    this.textArea.Document.UndoStack.StartUndoGroup();
                    try {
                        if (this.textArea.SelectionManager.HasSomethingSelected) {
                            this.textArea.Caret.Position = this.textArea.SelectionManager.SelectionCollection[0].StartPosition;
                            this.textArea.SelectionManager.RemoveSelectedText();
                        }
                        if (dataPresent) {
                            int column = this.textArea.Caret.Column;
                            this.textArea.Caret.Column = 0;
                            if (!this.textArea.IsReadOnly(this.textArea.Caret.Offset))
                                this.textArea.InsertString(str);
                            this.textArea.Caret.Column = column;
                            break;
                        }
                        this.textArea.InsertString(str);
                        break;
                    }
                    finally {
                        this.textArea.Document.UndoStack.EndUndoGroup();
                    }
                }
                catch (ExternalException ex) {
                    if (num > 5)
                        throw;
                }
                ++num;
            }
        }

        public void Delete(object sender, EventArgs e) {
            new Delete().Execute(this.textArea);
        }

        public void SelectAll(object sender, EventArgs e) {
            new SelectWholeDocument().Execute(this.textArea);
        }

        protected virtual void OnCopyText(CopyTextEventArgs e) {
            if (this.CopyText == null)
                return;
            this.CopyText((object)this, e);
        }

        public delegate bool ClipboardContainsTextDelegate();
    }
}
