namespace testmap
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnAddMarker = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbIcon = new System.Windows.Forms.ComboBox();
            this.lblIcon = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblMapMode = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.txtLabel = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showLabelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.attributeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAddMarker
            // 
            this.btnAddMarker.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAddMarker.Location = new System.Drawing.Point(779, 795);
            this.btnAddMarker.Name = "btnAddMarker";
            this.btnAddMarker.Size = new System.Drawing.Size(146, 57);
            this.btnAddMarker.TabIndex = 0;
            this.btnAddMarker.Text = "Add Marker";
            this.btnAddMarker.UseVisualStyleBackColor = true;
            this.btnAddMarker.Click += new System.EventHandler(this.btnAddMarker_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(14, 17);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1231, 619);
            this.panel1.TabIndex = 1;
            // 
            // cbIcon
            // 
            this.cbIcon.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cbIcon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIcon.FormattingEnabled = true;
            this.cbIcon.Location = new System.Drawing.Point(539, 819);
            this.cbIcon.Name = "cbIcon";
            this.cbIcon.Size = new System.Drawing.Size(183, 33);
            this.cbIcon.TabIndex = 2;
            // 
            // lblIcon
            // 
            this.lblIcon.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblIcon.AutoSize = true;
            this.lblIcon.Location = new System.Drawing.Point(610, 775);
            this.lblIcon.Name = "lblIcon";
            this.lblIcon.Size = new System.Drawing.Size(46, 25);
            this.lblIcon.TabIndex = 3;
            this.lblIcon.Text = "Icon";
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(79, 850);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(19, 25);
            this.lblInfo.TabIndex = 4;
            this.lblInfo.Text = "-";
            // 
            // lblMapMode
            // 
            this.lblMapMode.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblMapMode.AutoSize = true;
            this.lblMapMode.Location = new System.Drawing.Point(79, 739);
            this.lblMapMode.Name = "lblMapMode";
            this.lblMapMode.Size = new System.Drawing.Size(98, 25);
            this.lblMapMode.TabIndex = 5;
            this.lblMapMode.Text = "Mode: Edit";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(1105, 819);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(111, 33);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save .SHP";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpen.Location = new System.Drawing.Point(1105, 774);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(111, 33);
            this.btnOpen.TabIndex = 7;
            this.btnOpen.Text = "Open .SHP";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // txtLabel
            // 
            this.txtLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtLabel.Location = new System.Drawing.Point(155, 657);
            this.txtLabel.Name = "txtLabel";
            this.txtLabel.Size = new System.Drawing.Size(100, 31);
            this.txtLabel.TabIndex = 8;
            this.txtLabel.Visible = false;
            this.txtLabel.Leave += new System.EventHandler(this.txtLabel_Leave);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showLabelToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.attributeToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(175, 132);
            this.contextMenuStrip1.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.contextMenuStrip1_Closed);
            // 
            // showLabelToolStripMenuItem
            // 
            this.showLabelToolStripMenuItem.Name = "showLabelToolStripMenuItem";
            this.showLabelToolStripMenuItem.Size = new System.Drawing.Size(174, 32);
            this.showLabelToolStripMenuItem.Text = "Show Label";
            this.showLabelToolStripMenuItem.Click += new System.EventHandler(this.showLabelToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(174, 32);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(174, 32);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // attributeToolStripMenuItem
            // 
            this.attributeToolStripMenuItem.Name = "attributeToolStripMenuItem";
            this.attributeToolStripMenuItem.Size = new System.Drawing.Size(174, 32);
            this.attributeToolStripMenuItem.Text = "Attribute";
            this.attributeToolStripMenuItem.Click += new System.EventHandler(this.attributeToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1258, 904);
            this.Controls.Add(this.txtLabel);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblMapMode);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lblIcon);
            this.Controls.Add(this.cbIcon);
            this.Controls.Add(this.btnAddMarker);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnAddMarker;
        private Panel panel1;
        private ComboBox cbIcon;
        private Label lblIcon;
        private Label lblInfo;
        private Label lblMapMode;
        private Button btnSave;
        private Button btnOpen;
        private TextBox txtLabel;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem attributeToolStripMenuItem;
        private ToolStripMenuItem renameToolStripMenuItem;
        private ToolStripMenuItem showLabelToolStripMenuItem;
    }
}