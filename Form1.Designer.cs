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
            this.btnMode = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbIcon = new System.Windows.Forms.ComboBox();
            this.lblIcon = new System.Windows.Forms.Label();
            this.lblMode = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnMode
            // 
            this.btnMode.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnMode.Location = new System.Drawing.Point(669, 1165);
            this.btnMode.Name = "btnMode";
            this.btnMode.Size = new System.Drawing.Size(146, 57);
            this.btnMode.TabIndex = 0;
            this.btnMode.Text = "Change mode";
            this.btnMode.UseVisualStyleBackColor = true;
            this.btnMode.Click += new System.EventHandler(this.btnMode_Click);
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
            this.cbIcon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIcon.FormattingEnabled = true;
            this.cbIcon.Location = new System.Drawing.Point(939, 1200);
            this.cbIcon.Name = "cbIcon";
            this.cbIcon.Size = new System.Drawing.Size(182, 33);
            this.cbIcon.TabIndex = 2;
            this.cbIcon.Visible = false;
            // 
            // lblIcon
            // 
            this.lblIcon.AutoSize = true;
            this.lblIcon.Location = new System.Drawing.Point(1000, 1172);
            this.lblIcon.Name = "lblIcon";
            this.lblIcon.Size = new System.Drawing.Size(46, 25);
            this.lblIcon.TabIndex = 3;
            this.lblIcon.Text = "Icon";
            this.lblIcon.Visible = false;
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Location = new System.Drawing.Point(669, 1237);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(202, 25);
            this.lblMode.TabIndex = 4;
            this.lblMode.Text = "Current Mode: Selection";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1463, 1280);
            this.Controls.Add(this.lblMode);
            this.Controls.Add(this.lblIcon);
            this.Controls.Add(this.cbIcon);
            this.Controls.Add(this.btnMode);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnMode;
        private Panel panel1;
        private ComboBox cbIcon;
        private Label lblIcon;
        private Label lblMode;
    }
}