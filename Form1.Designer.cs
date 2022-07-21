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
            this.btnAddMarker = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbIcon = new System.Windows.Forms.ComboBox();
            this.lblIcon = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblMapMode = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.txtLabel = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnAddMarker
            // 
            this.btnAddMarker.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAddMarker.Location = new System.Drawing.Point(669, 1165);
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
            this.panel1.Size = new System.Drawing.Size(1433, 1112);
            this.panel1.TabIndex = 1;
            // 
            // cbIcon
            // 
            this.cbIcon.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cbIcon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIcon.FormattingEnabled = true;
            this.cbIcon.Location = new System.Drawing.Point(939, 1200);
            this.cbIcon.Name = "cbIcon";
            this.cbIcon.Size = new System.Drawing.Size(182, 33);
            this.cbIcon.TabIndex = 2;
            // 
            // lblIcon
            // 
            this.lblIcon.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblIcon.AutoSize = true;
            this.lblIcon.Location = new System.Drawing.Point(1000, 1172);
            this.lblIcon.Name = "lblIcon";
            this.lblIcon.Size = new System.Drawing.Size(46, 25);
            this.lblIcon.TabIndex = 3;
            this.lblIcon.Text = "Icon";
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(48, 1172);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(19, 25);
            this.lblInfo.TabIndex = 4;
            this.lblInfo.Text = "-";
            // 
            // lblMapMode
            // 
            this.lblMapMode.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblMapMode.AutoSize = true;
            this.lblMapMode.Location = new System.Drawing.Point(297, 1184);
            this.lblMapMode.Name = "lblMapMode";
            this.lblMapMode.Size = new System.Drawing.Size(139, 25);
            this.lblMapMode.TabIndex = 5;
            this.lblMapMode.Text = "Mode: Selection";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(1292, 1209);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(112, 34);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save .SHP";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(1292, 1163);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(112, 34);
            this.btnOpen.TabIndex = 7;
            this.btnOpen.Text = "Open .SHP";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // txtLabel
            // 
            this.txtLabel.Location = new System.Drawing.Point(502, 1167);
            this.txtLabel.Name = "txtLabel";
            this.txtLabel.Size = new System.Drawing.Size(100, 31);
            this.txtLabel.TabIndex = 8;
            this.txtLabel.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1463, 1280);
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
    }
}