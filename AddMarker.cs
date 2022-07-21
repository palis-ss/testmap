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
        public string? name { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public double height { get; set; }

        public AddMarker()
        {
            InitializeComponent();

            btnAddAccept.DialogResult = DialogResult.OK;
            btnAddCancel.DialogResult = DialogResult.Cancel;
            AcceptButton = btnAddAccept;
            CancelButton = btnAddCancel;
        }

        private void btnAddAccept_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAddLat.Text) || string.IsNullOrEmpty(txtAddLon.Text))
            {
                MessageBox.Show("Must fill in Latitude and Longitude");
                return;
            }
            name = txtName.Text;
            lat = Convert.ToDouble(txtAddLat.Text);
            lon = Convert.ToDouble(txtAddLon.Text);
            if (!string.IsNullOrEmpty(txtAddHeight.Text))
                height = Convert.ToDouble(txtAddHeight.Text);
        }
    }
}
