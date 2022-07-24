using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testmap
{
    public partial class AddMarker : Form
    {
        public string? PosName { get; set; }
        public double PosLat { get; set; }
        public double PosLon { get; set; }
        public double PosHeight { get; set; }
        public bool ShowLabel { get; set; }
        public string? PosIcon { get; set; }

        public AddMarker()
        {
            InitializeComponent();

            btnAddAccept.DialogResult = DialogResult.OK;
            btnAddCancel.DialogResult = DialogResult.Cancel;
            AcceptButton = btnAddAccept;
            CancelButton = btnAddCancel;
            
            cbIcon2.Items.Add("car");
            cbIcon2.Items.Add("house");
            if (cbIcon2.Items.Count > 0)
                cbIcon2.SelectedIndex = 0;
        }

        public void UpdateData()
        {
            txtName.Text = PosName;
            txtAddLat.Text = PosLat.ToString();
            txtAddLon.Text = PosLon.ToString();
            txtAddHeight.Text = PosHeight.ToString();
            if (string.IsNullOrEmpty(PosIcon))
                cbIcon2.SelectedIndex = 0;
            else
                cbIcon2.Text = PosIcon;
            chkShowLabel.Checked = ShowLabel;

        }

        private void btnAddAccept_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAddLat.Text) || string.IsNullOrEmpty(txtAddLon.Text))
            {
                MessageBox.Show("Must fill in Latitude and Longitude");
                return;
            }
            PosName = txtName.Text;
            PosLat = Convert.ToDouble(txtAddLat.Text);
            PosLon = Convert.ToDouble(txtAddLon.Text);
            PosHeight = Convert.ToDouble(txtAddHeight.Text);
            if (!string.IsNullOrEmpty(txtAddHeight.Text))
                PosHeight = Convert.ToDouble(txtAddHeight.Text);
            else
                PosHeight = 0;

            ShowLabel = chkShowLabel.Checked;
            PosIcon = cbIcon2.Text;
        }
    }
}
