// Xml Visualizer v.2
// by Lars Hove Christiansen (larshove@gmail.com)
// http://www.codeplex.com/XmlVisualizer

namespace ProXmlEditor {
    internal partial class AboutForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.okButton = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.urlLinkLabel = new System.Windows.Forms.LinkLabel();
            this.line = new System.Windows.Forms.Label();
            this.mailLinkLabel = new System.Windows.Forms.LinkLabel();
            this.mailLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.byLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okButton.Location = new System.Drawing.Point(180, 204);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(12, 9);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(0, 13);
            this.titleLabel.TabIndex = 1;
            // 
            // urlLinkLabel
            // 
            this.urlLinkLabel.AutoSize = true;
            this.urlLinkLabel.Location = new System.Drawing.Point(12, 51);
            this.urlLinkLabel.Name = "urlLinkLabel";
            this.urlLinkLabel.Size = new System.Drawing.Size(196, 13);
            this.urlLinkLabel.TabIndex = 2;
            this.urlLinkLabel.TabStop = true;
            this.urlLinkLabel.Text = "https://github.com/metjka/ProXmlEditor";
            this.urlLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.urlLinkLabel_LinkClicked);
            // 
            // line
            // 
            this.line.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.line.Location = new System.Drawing.Point(4, 190);
            this.line.Name = "line";
            this.line.Size = new System.Drawing.Size(260, 2);
            this.line.TabIndex = 14;
            // 
            // mailLinkLabel
            // 
            this.mailLinkLabel.AutoSize = true;
            this.mailLinkLabel.Location = new System.Drawing.Point(12, 114);
            this.mailLinkLabel.Name = "mailLinkLabel";
            this.mailLinkLabel.Size = new System.Drawing.Size(166, 13);
            this.mailLinkLabel.TabIndex = 16;
            this.mailLinkLabel.TabStop = true;
            this.mailLinkLabel.Text = "w50901@student.wsiz.rzeszow.pl";
            this.mailLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.mailLinkLabel_LinkClicked);
            // 
            // mailLabel
            // 
            this.mailLabel.Location = new System.Drawing.Point(0, 0);
            this.mailLabel.Name = "mailLabel";
            this.mailLabel.Size = new System.Drawing.Size(100, 23);
            this.mailLabel.TabIndex = 24;
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(11, 17);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(30, 13);
            this.versionLabel.TabIndex = 18;
            this.versionLabel.Text = "Build";
            // 
            // byLabel
            // 
            this.byLabel.AutoSize = true;
            this.byLabel.Location = new System.Drawing.Point(12, 78);
            this.byLabel.Name = "byLabel";
            this.byLabel.Size = new System.Drawing.Size(84, 26);
            this.byLabel.TabIndex = 19;
            this.byLabel.Text = "By Ihor Salnikov\nw50901";
            // 
            // AboutForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.okButton;
            this.ClientSize = new System.Drawing.Size(267, 239);
            this.ControlBox = false;
            this.Controls.Add(this.byLabel);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.mailLabel);
            this.Controls.Add(this.mailLinkLabel);
            this.Controls.Add(this.line);
            this.Controls.Add(this.urlLinkLabel);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.LinkLabel urlLinkLabel;
        private System.Windows.Forms.Label line;
        private System.Windows.Forms.LinkLabel mailLinkLabel;
        private System.Windows.Forms.Label mailLabel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label byLabel;
    }
}
