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
            txtAddLat = new TextBox();
            txtAddLon = new TextBox();
            txtAddHeight = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            btnAddAccept = new Button();
            btnAddCancel = new Button();
            txtName = new TextBox();
            label4 = new Label();
            chkShowLabel = new CheckBox();
            cbIcon2 = new ComboBox();
            label5 = new Label();
            SuspendLayout();
            // 
            // txtAddLat
            // 
            txtAddLat.Location = new Point(194, 63);
            txtAddLat.Margin = new Padding(2);
            txtAddLat.Name = "txtAddLat";
            txtAddLat.Size = new Size(106, 23);
            txtAddLat.TabIndex = 1;
            // 
            // txtAddLon
            // 
            txtAddLon.Location = new Point(194, 98);
            txtAddLon.Margin = new Padding(2);
            txtAddLon.Name = "txtAddLon";
            txtAddLon.Size = new Size(106, 23);
            txtAddLon.TabIndex = 2;
            // 
            // txtAddHeight
            // 
            txtAddHeight.Location = new Point(194, 136);
            txtAddHeight.Margin = new Padding(2);
            txtAddHeight.Name = "txtAddHeight";
            txtAddHeight.Size = new Size(106, 23);
            txtAddHeight.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(80, 65);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(50, 15);
            label1.TabIndex = 7;
            label1.Text = "Latitude";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(80, 100);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(58, 15);
            label2.TabIndex = 8;
            label2.Text = "Lontitude";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(80, 137);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(43, 15);
            label3.TabIndex = 9;
            label3.Text = "Height";
            // 
            // btnAddAccept
            // 
            btnAddAccept.Location = new Point(172, 206);
            btnAddAccept.Margin = new Padding(2);
            btnAddAccept.Name = "btnAddAccept";
            btnAddAccept.Size = new Size(78, 20);
            btnAddAccept.TabIndex = 6;
            btnAddAccept.Text = "Accept";
            btnAddAccept.UseVisualStyleBackColor = true;
            btnAddAccept.Click += btnAddAccept_Click;
            // 
            // btnAddCancel
            // 
            btnAddCancel.Location = new Point(302, 206);
            btnAddCancel.Margin = new Padding(2);
            btnAddCancel.Name = "btnAddCancel";
            btnAddCancel.Size = new Size(78, 20);
            btnAddCancel.TabIndex = 7;
            btnAddCancel.Text = "Cancel";
            btnAddCancel.UseVisualStyleBackColor = true;
            // 
            // txtName
            // 
            txtName.Location = new Point(194, 26);
            txtName.Margin = new Padding(2);
            txtName.Name = "txtName";
            txtName.Size = new Size(106, 23);
            txtName.TabIndex = 0;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(80, 28);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(39, 15);
            label4.TabIndex = 6;
            label4.Text = "Name";
            // 
            // chkShowLabel
            // 
            chkShowLabel.AutoSize = true;
            chkShowLabel.Checked = true;
            chkShowLabel.CheckState = CheckState.Checked;
            chkShowLabel.Location = new Point(372, 28);
            chkShowLabel.Margin = new Padding(2);
            chkShowLabel.Name = "chkShowLabel";
            chkShowLabel.Size = new Size(100, 19);
            chkShowLabel.TabIndex = 4;
            chkShowLabel.Text = "Show as Label";
            chkShowLabel.UseVisualStyleBackColor = true;
            // 
            // cbIcon2
            // 
            cbIcon2.DropDownStyle = ComboBoxStyle.DropDownList;
            cbIcon2.FormattingEnabled = true;
            cbIcon2.Location = new Point(365, 89);
            cbIcon2.Margin = new Padding(2);
            cbIcon2.Name = "cbIcon2";
            cbIcon2.Size = new Size(129, 23);
            cbIcon2.TabIndex = 5;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(403, 72);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(30, 15);
            label5.TabIndex = 12;
            label5.Text = "Icon";
            // 
            // AddMarker
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(545, 266);
            Controls.Add(label5);
            Controls.Add(cbIcon2);
            Controls.Add(chkShowLabel);
            Controls.Add(label4);
            Controls.Add(txtName);
            Controls.Add(btnAddCancel);
            Controls.Add(btnAddAccept);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtAddHeight);
            Controls.Add(txtAddLon);
            Controls.Add(txtAddLat);
            Margin = new Padding(2);
            Name = "AddMarker";
            Text = "Adding Marker";
            ResumeLayout(false);
            PerformLayout();
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