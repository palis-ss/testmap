namespace testmap
{
    partial class AddMarker
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtAddLat = new System.Windows.Forms.TextBox();
            this.txtAddLon = new System.Windows.Forms.TextBox();
            this.txtAddHeight = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAddAccept = new System.Windows.Forms.Button();
            this.btnAddCancel = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkShowLabel = new System.Windows.Forms.CheckBox();
            this.cbIcon2 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtAddLat
            // 
            this.txtAddLat.Location = new System.Drawing.Point(277, 105);
            this.txtAddLat.Name = "txtAddLat";
            this.txtAddLat.Size = new System.Drawing.Size(150, 31);
            this.txtAddLat.TabIndex = 1;
            // 
            // txtAddLon
            // 
            this.txtAddLon.Location = new System.Drawing.Point(277, 163);
            this.txtAddLon.Name = "txtAddLon";
            this.txtAddLon.Size = new System.Drawing.Size(150, 31);
            this.txtAddLon.TabIndex = 2;
            // 
            // txtAddHeight
            // 
            this.txtAddHeight.Location = new System.Drawing.Point(277, 226);
            this.txtAddHeight.Name = "txtAddHeight";
            this.txtAddHeight.Size = new System.Drawing.Size(150, 31);
            this.txtAddHeight.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(114, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 25);
            this.label1.TabIndex = 7;
            this.label1.Text = "Latitude";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(114, 166);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 25);
            this.label2.TabIndex = 8;
            this.label2.Text = "Lontitude";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(114, 229);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 25);
            this.label3.TabIndex = 9;
            this.label3.Text = "Height";
            // 
            // btnAddAccept
            // 
            this.btnAddAccept.Location = new System.Drawing.Point(246, 343);
            this.btnAddAccept.Name = "btnAddAccept";
            this.btnAddAccept.Size = new System.Drawing.Size(112, 34);
            this.btnAddAccept.TabIndex = 6;
            this.btnAddAccept.Text = "Accept";
            this.btnAddAccept.UseVisualStyleBackColor = true;
            this.btnAddAccept.Click += new System.EventHandler(this.btnAddAccept_Click);
            // 
            // btnAddCancel
            // 
            this.btnAddCancel.Location = new System.Drawing.Point(432, 343);
            this.btnAddCancel.Name = "btnAddCancel";
            this.btnAddCancel.Size = new System.Drawing.Size(112, 34);
            this.btnAddCancel.TabIndex = 7;
            this.btnAddCancel.Text = "Cancel";
            this.btnAddCancel.UseVisualStyleBackColor = true;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(277, 44);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(150, 31);
            this.txtName.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(114, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 25);
            this.label4.TabIndex = 6;
            this.label4.Text = "Name";
            // 
            // chkShowLabel
            // 
            this.chkShowLabel.AutoSize = true;
            this.chkShowLabel.Checked = true;
            this.chkShowLabel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowLabel.Location = new System.Drawing.Point(532, 47);
            this.chkShowLabel.Name = "chkShowLabel";
            this.chkShowLabel.Size = new System.Drawing.Size(150, 29);
            this.chkShowLabel.TabIndex = 4;
            this.chkShowLabel.Text = "Show as Label";
            this.chkShowLabel.UseVisualStyleBackColor = true;
            // 
            // cbIcon2
            // 
            this.cbIcon2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIcon2.FormattingEnabled = true;
            this.cbIcon2.Location = new System.Drawing.Point(521, 148);
            this.cbIcon2.Name = "cbIcon2";
            this.cbIcon2.Size = new System.Drawing.Size(182, 33);
            this.cbIcon2.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(576, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 25);
            this.label5.TabIndex = 12;
            this.label5.Text = "Icon";
            // 
            // AddMarker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 444);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbIcon2);
            this.Controls.Add(this.chkShowLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.btnAddCancel);
            this.Controls.Add(this.btnAddAccept);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtAddHeight);
            this.Controls.Add(this.txtAddLon);
            this.Controls.Add(this.txtAddLat);
            this.Name = "AddMarker";
            this.Text = "AddMarker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txtAddLat;
        private TextBox txtAddLon;
        private TextBox txtAddHeight;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button btnAddAccept;
        private Button btnAddCancel;
        private TextBox txtName;
        private Label label4;
        private CheckBox chkShowLabel;
        private ComboBox cbIcon2;
        private Label label5;
    }
}