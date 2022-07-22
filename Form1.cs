using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Input;
using System.Runtime.InteropServices;
using AxMapWinGIS;
using MapWinGIS;
using Image = MapWinGIS.Image;

namespace testmap
{
    public partial class Form1 : Form
    {
        const string EDIT_INFOTXT = "Click on marker, then\n- Shift+del to delete\n- Click and drag to move.\n- Click anywhere else to finish";
        const string ADD_INFOTXT = "Click a location to add";

        private AxMap map;
        int MarkerLayer = -1;
        MapMode mode;
        bool addinglabel = false;
        bool renaminglabel = false;
        bool deleting = false;
        int _fieldIndex = -1;
        int _shapeidx = -1;
        double _posx = 0;
        double _posy = 0;

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern short GetKeyState(int keyCode);

        public Form1()
        {
            InitializeComponent();
            map = new AxMap();
            mode = MapMode.Edit;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.Size = new Size(ClientSize.Width - 20, ClientSize.Height - 200);

            map.Location = new System.Drawing.Point(0, 0);
            map.Size = panel1.Size;
            panel1.Controls.Add(map);


            TileProviders providers = map.Tiles.Providers;
            int providerId = (int)tkTileProvider.ProviderCustom + 10;
            bool ret = providers.Add(providerId, "MapTilerOutdoor",
                @"https://api.maptiler.com/maps/topographique/256/{zoom}/{x}/{y}.png?key=ZnCLN1UxbkcF74yFeryt",
                tkTileProjection.SphericalMercator);

            map.Projection = tkMapProjection.PROJECTION_GOOGLE_MERCATOR;
            map.Tiles.ProviderId = providerId;

            map.KnownExtents = tkKnownExtents.keThailand;
            map.CurrentZoom = 8;

            map.ShapeEditor.EditorBehavior = tkEditorBehavior.ebVertexEditor;
            map.ShapeEditor.IndicesVisible = false;
            //map.CursorMode = tkCursorMode.cmNone;
            //map.CursorMode = tkCursorMode.cmIdentify;
            map.CursorMode = tkCursorMode.cmEditShape;
            //map.MapCursor = tkCursor.crsrHelp;

            map.ChooseLayer += Map_ChooseLayer;
            //map.ShapeIdentified += Map_ShapeIdentified;
            map.SendMouseDown = true;
            map.MouseDownEvent += Map_MouseDownEvent;
            map.AfterShapeEdit += Map_AfterShapeEdit;
            map.BeforeShapeEdit += Map_BeforeShapeEdit;
            map.Focus();
            map.Identifier.IdentifierMode = tkIdentifierMode.imSingleLayer;

            InitLayers();

            cbIcon.Items.Add("car");
            cbIcon.Items.Add("house");
            if (cbIcon.Items.Count > 0)
                cbIcon.SelectedIndex = 0;

            lblMapMode.Text = "Mode: Edit marker";
            lblInfo.Text = EDIT_INFOTXT;
        }

        private void Map_BeforeShapeEdit(object sender, _DMapEvents_BeforeShapeEditEvent e)
        {
            _shapeidx = e.shapeIndex;
        }

        private void Map_AfterShapeEdit(object sender, _DMapEvents_AfterShapeEditEvent e)
        {
            Shapefile sf = map.get_Shapefile(MarkerLayer);
            switch (e.operation)
            {
                case tkUndoOperation.uoAddShape:
                    double lat = 0, lon = 0;
                    map.ProjToDegrees(sf.Shape[e.shapeIndex].Center.x, sf.Shape[e.shapeIndex].Center.y, ref lon, ref lat);
                    _fieldIndex = sf.Table.FieldIndexByName["Icon"];
                    sf.EditCellValue(_fieldIndex, e.shapeIndex, cbIcon.Text);
                    _fieldIndex = sf.Table.FieldIndexByName["Lat"];
                    sf.EditCellValue(_fieldIndex, e.shapeIndex, lat);
                    _fieldIndex = sf.Table.FieldIndexByName["Lon"];
                    sf.EditCellValue(_fieldIndex, e.shapeIndex, lon);
                    _fieldIndex = sf.Table.FieldIndexByName["Height"];
                    sf.EditCellValue(_fieldIndex, e.shapeIndex, 0);

                    sf.Categories.ApplyExpressions();  // render matching icon
                    map.Redraw();

                    // adding textbox for label input
                    double ex = 0, ey = 0;
                    map.ProjToPixel(sf.Shape[e.shapeIndex].Center.x, sf.Shape[e.shapeIndex].Center.y, ref ex, ref ey);
                    txtLabel.Location = new System.Drawing.Point((int)ex - 40, (int)ey + 35);
                    txtLabel.Text = "[Add Name]";
                    txtLabel.SelectAll();
                    txtLabel.BringToFront();
                    txtLabel.Visible = true;
                    txtLabel.Focus();
                    addinglabel = true;

                    _posx = sf.Shape[e.shapeIndex].Center.x;
                    _posy = sf.Shape[e.shapeIndex].Center.y;
                    _shapeidx = e.shapeIndex;

                    mode = MapMode.Edit;
                    map.CursorMode = tkCursorMode.cmEditShape;
                    lblMapMode.Text = "Mode: Edit marker";
                    lblInfo.Text = EDIT_INFOTXT;
                    break;
                case tkUndoOperation.uoRemoveShape:
                    // removing associated label
                    sf.Labels.RemoveLabel(e.shapeIndex);
                    break;
                case tkUndoOperation.uoEditShape:  // this is supposed to be move shape
                    if (deleting)
                    {
                        deleting = false;
                        break;
                    }
                    if (addinglabel)
                    {
                        addinglabel = false;
                        break;
                    }
                    if (renaminglabel)
                    {
                        renaminglabel = false;
                        break;
                    }

                    lat = 0; lon = 0;
                    map.ProjToDegrees(sf.Shape[e.shapeIndex].Center.x, sf.Shape[e.shapeIndex].Center.y, ref lon, ref lat);
                    _fieldIndex = sf.Table.FieldIndexByName["Lat"];
                    sf.EditCellValue(_fieldIndex, e.shapeIndex, lat);
                    _fieldIndex = sf.Table.FieldIndexByName["Lon"];
                    sf.EditCellValue(_fieldIndex, e.shapeIndex, lon);

                    // moving label along
                    _posx = sf.Shape[e.shapeIndex].Center.x;
                    _posy = sf.Shape[e.shapeIndex].Center.y;
                    if (sf.Labels.Count > 0)
                    {
                        sf.Labels.Label[e.shapeIndex, 0].x = _posx;
                        sf.Labels.Label[e.shapeIndex, 0].y = _posy;
                    }
                    map.Redraw();
                    break;
            }
        }

        private void Map_MouseDownEvent(object sender, _DMapEvents_MouseDownEvent e)
        {
            if (addinglabel || renaminglabel || deleting)
                return;

            if (map.ShapeEditor.EditorState == tkEditorState.esEdit)
            {
                Shapefile sf = map.get_Shapefile(MarkerLayer);
                _fieldIndex = sf.Table.FieldIndexByName["Showlabel"];
                var val = sf.CellValue[_fieldIndex, _shapeidx];
                if (val != null && (bool)val == false)
                    contextMenuStrip1.Items[0].Text = "Show Label";
                else if (val != null && (bool)val == true)
                    contextMenuStrip1.Items[0].Text = "Hide Label";

                contextMenuStrip1.Show(map, e.x, e.y);
            }
        }

        private void Map_ChooseLayer(object sender, _DMapEvents_ChooseLayerEvent e)
        {
            e.layerHandle = MarkerLayer;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            panel1.Size = new Size(control.ClientSize.Width - 20, control.ClientSize.Height - 200);
            if (map != null)
                map.Size = panel1.Size;
        }
        private void btnAddMarker_Click(object sender, EventArgs e)
        {
            if (addinglabel || renaminglabel)
                return;
            /*
            Shapefile sf = map.get_Shapefile(MarkerLayer);
            MarkLoc(13.5, 100.5 + 0.5 * sf.NumShapes, cbIcon.Text);
            return;
            */
            AddMarker addMarker = new AddMarker();
            addMarker.StartPosition = FormStartPosition.CenterParent;
            addMarker.ShowDialog();
            if (addMarker.DialogResult == DialogResult.OK)
            {
                MarkLoc(addMarker.name, addMarker.lat, addMarker.lon, addMarker.height, cbIcon.Text);
            }
        }

        public void MarkLoc(string? name, double lat, double lon, double height, string icon)
        {
            Shapefile sf = map.get_Shapefile(MarkerLayer);
            Shape shp = new Shape();
            shp.Create(ShpfileType.SHP_POINT);

            double x = 0.0;
            double y = 0.0;
            map.DegreesToProj(lon, lat, ref x, ref y);
            shp.AddPoint(x, y);
            _shapeidx = sf.EditAddShape(shp);
            //sf.ShapeCategory2[idx] = icon;
            _fieldIndex = sf.Table.FieldIndexByName["Icon"];
            sf.EditCellValue(_fieldIndex, _shapeidx, icon);
            _fieldIndex = sf.Table.FieldIndexByName["Lat"];
            sf.EditCellValue(_fieldIndex, _shapeidx, lat);
            _fieldIndex = sf.Table.FieldIndexByName["Lon"];
            sf.EditCellValue(_fieldIndex, _shapeidx, lon);
            _fieldIndex = sf.Table.FieldIndexByName["Height"];
            sf.EditCellValue(_fieldIndex, _shapeidx, height);

            if (string.IsNullOrEmpty(name))
            {
                _fieldIndex = sf.Table.FieldIndexByName["ShowLabel"];
                sf.EditCellValue(_fieldIndex, _shapeidx, false);
            }
            else
            {
                _fieldIndex = sf.Table.FieldIndexByName["Name"];
                sf.EditCellValue(_fieldIndex, _shapeidx, name);
                _fieldIndex = sf.Table.FieldIndexByName["ShowLabel"];
                sf.EditCellValue(_fieldIndex, _shapeidx, true);

                sf.Labels.AddLabel(name, x, y);
            }

            sf.Categories.ApplyExpressions();  // render matching icon


            map.Redraw();
        }

        public void MarkPoint(int ex, int ey, string icon)
        {
            Shapefile sf = map.get_Shapefile(MarkerLayer);
            Shape shp = new Shape();
            shp.Create(ShpfileType.SHP_POINT);

            double x = 0.0;
            double y = 0.0;
            map.PixelToProj(ex, ey, ref x, ref y);

            shp.AddPoint(x, y);
            _shapeidx = sf.EditAddShape(shp);
            _fieldIndex = sf.Table.FieldIndexByName["Icon"];
            sf.EditCellValue(_fieldIndex, _shapeidx, icon);

            double lat = 0.0;
            double lon = 0.0;
            map.ProjToDegrees(x, y, ref lon, ref lat);
            _fieldIndex = sf.Table.FieldIndexByName["Lat"];
            sf.EditCellValue(_fieldIndex, _shapeidx, lat);
            _fieldIndex = sf.Table.FieldIndexByName["Lon"];
            sf.EditCellValue(_fieldIndex, _shapeidx, lon);

            //int handle = map.NewDrawing(tkDrawReferenceList.dlSpatiallyReferencedList);
            //map.DrawCircleEx(handle, x, y, 1000.0, 0xFFFF, false);  // test example grid

            sf.Categories.ApplyExpressions();  // render matching icon
            map.Redraw();

            // adding textbox for label input
            txtLabel.Location = new System.Drawing.Point(ex, ey + 35);
            txtLabel.Text = "[Add Name]";
            txtLabel.SelectAll();
            txtLabel.BringToFront();
            txtLabel.Visible = true;
            txtLabel.Focus();
            addinglabel = true;

            _posx = x;
            _posy = y;
        }

        // map layers
        // 1) base map
        // 2) markers
        // 3) tracking
        public void InitLayers()
        {
            var sf = new Shapefile();
            sf.CreateNew("", ShpfileType.SHP_POINT);
            sf.DefaultDrawingOptions.AlignPictureByBottom = false;
            sf.Identifiable = true;
            MarkerLayer = map.AddLayer(sf, true);
            if (!sf.StartEditingTable())
            {
                MessageBox.Show(@"Failed to open editing mode.");
                return;
            }
            sf.EditAddField("Name", FieldType.STRING_FIELD, 15, 18);
            sf.EditAddField("ShowLabel", FieldType.BOOLEAN_FIELD, 1, 1);
            sf.EditAddField("Icon", FieldType.STRING_FIELD, 15, 18);
            sf.EditAddField("Lat", FieldType.DOUBLE_FIELD, 15, 18);
            sf.EditAddField("Lon", FieldType.DOUBLE_FIELD, 15, 18);
            sf.EditAddField("Height", FieldType.DOUBLE_FIELD, 15, 18);


            loadicons();
            sf.DefaultDrawingOptions.Visible = true;
            sf.InteractiveEditing = true;            
            //sf.Labels.Synchronized = true;
        }

        public void loadicons()
        {
            string _iconPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\..\..\..\icons\";

            Shapefile sf = map.get_Shapefile(MarkerLayer);
            var ct = sf.Categories.Add("car");
            ct.Expression = "[Icon]=\"car\"";
            ct.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
            var img = new Image();
            if (img.Open(_iconPath + @"car-32.png"))
                ct.DrawingOptions.Picture = img;

            ct = sf.Categories.Add("house");
            ct.Expression = "[Icon]=\"house\"";
            ct.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
            img = new Image();
            if (img.Open(_iconPath + @"house-32.png"))
                ct.DrawingOptions.Picture = img;

            var labels = sf.Labels;
            labels.FrameVisible = true;
            labels.FrameType = tkLabelFrameType.lfRectangle;
            labels.Alignment = tkLabelAlignment.laBottomCenter;
            labels.OffsetY = 16;
        }

        public void Map_ShapeIdentified(object sender, _DMapEvents_ShapeIdentifiedEvent e)
        {
            var shapes = map.IdentifiedShapes;
            Shapefile sf = map.get_Shapefile(shapes.LayerHandle[0]);

            switch (mode)
            {
                case MapMode.Edit:
                    double px = 0, py = 0;
                    map.ProjToPixel(e.pointX, e.pointY, ref px, ref py);

                    contextMenuStrip1.Show(map, (int)px, (int)py);
                    map.IdentifiedShapes.Clear();

                    break;
                    /*
                case MapMode.Delete:
                    var name = sf.CellValue[sf.FieldIndexByName["Name"], shapes.ShapeIndex[0]];
                    sf.EditDeleteShape(shapes.ShapeIndex[0]);

                    for (int i = 0; i < sf.Labels.Count; i++)
                    {
                        if (sf.Labels.Label[i, 0].Text == name as string)
                        {
                            sf.Labels.RemoveLabel(i);
                        }
                    }
                    map.Redraw();

                    break;
                    */
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (addinglabel || renaminglabel)
            {
                var sf = map.get_Shapefile(MarkerLayer);
                string label = "";
                bool showlabel = false;

                if (keyData == Keys.Escape || keyData == Keys.Enter)
                {
                    if (keyData == Keys.Escape)
                    {
                        label = "";
                        showlabel = false;
                    }
                    else
                    {
                        label = txtLabel.Text;
                        showlabel = true;
                    }

                    _fieldIndex = sf.Table.FieldIndexByName["Name"];
                    sf.EditCellValue(_fieldIndex, _shapeidx, label);
                    _fieldIndex = sf.Table.FieldIndexByName["ShowLabel"];
                    sf.EditCellValue(_fieldIndex, _shapeidx, showlabel);
                    txtLabel.Visible = false;

                    if (addinglabel)
                        sf.Labels.InsertLabel(_shapeidx, label, _posx, _posy);
                    if (renaminglabel)
                        sf.Labels.Label[_shapeidx, 0].Text = label;
                    sf.Labels.Label[_shapeidx, 0].Visible = showlabel;

                    map.ShapeEditor.SaveChanges();
                    map.Redraw();

                    addinglabel = false;
                    renaminglabel = false;
                    return true;
                }

                return base.ProcessCmdKey(ref msg, keyData);
            }

            switch (keyData)
            {
                case Keys.A:
                    map.IdentifiedShapes.Clear();
                    map.Redraw();

                    map.CursorMode = tkCursorMode.cmAddShape;
                    map.MapCursor = tkCursor.crsrMapDefault;
                    mode = MapMode.Add;
                    lblMapMode.Text = "Mode: Add marker";
                    lblInfo.Text = ADD_INFOTXT;
                    break;
                case Keys.E:
                case Keys.Escape:
                    map.IdentifiedShapes.Clear();
                    map.Redraw();

                    //map.CursorMode = tkCursorMode.cmIdentify;
                    //map.MapCursor = tkCursor.crsrHelp;
                    map.CursorMode = tkCursorMode.cmEditShape;   // just to play around
                    /*
                    map.MapCursor = tkCursor.crsrUserDefined;
                    map.UDCursorHandle = Cursors.Help.Handle.ToInt32();
                    */
                    mode = MapMode.Edit;
                    lblMapMode.Text = "Mode: Edit marker";
                    lblInfo.Text = EDIT_INFOTXT;
                    break;
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
            return true;
        }

        private enum MapMode
        {
            Edit,
            Add
        };

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (addinglabel || renaminglabel)
                return;
            Shapefile sf = map.get_Shapefile(MarkerLayer);
            sf.SaveAs("Markers.shp");
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (addinglabel || renaminglabel)
                return;
            InitLayers();  // creating a new layer for loading data
            Shapefile sf = map.get_Shapefile(MarkerLayer);  // MarkerLayer is the newly created layer
            if (sf.LoadDataFrom("Markers.shp") == false)
            {
                MessageBox.Show(sf.ErrorMsg[sf.LastErrorCode]);
            }
            else
            {
                sf.Categories.ApplyExpressions();
                map.Redraw();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Shapefile sf = map.get_Shapefile(MarkerLayer);

            deleting = true;
            sf.EditDeleteShape(_shapeidx);
            sf.Labels.RemoveLabel(_shapeidx);
        }

        private void attributeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Shapefile sf = map.get_Shapefile(MarkerLayer);
            Form form = new Form();
            for (int i = 0; i < sf.NumFields; i++)
            {
                System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                label.Left = 15;
                label.Top = i * 30 + 5;
                label.Text = sf.Field[i].Name;
                label.Width = 60;
                form.Controls.Add(label);

                TextBox box = new TextBox();
                box.ReadOnly = true;
                box.Left = 80;
                box.Top = label.Top;
                box.Width = 100;
                box.TabStop = false;

                box.Text = sf.CellValue[i, _shapeidx].ToString();
                box.Name = sf.Field[i].Name;
                form.Controls.Add(box);
            }

            form.Width = 300;
            form.Height = sf.NumFields * 30 + 70;

            form.Text = "Shape: " + _shapeidx;
            form.ShowInTaskbar = false;
            form.StartPosition = FormStartPosition.CenterParent;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.ShowDialog(map.Parent);
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Shapefile sf = map.get_Shapefile(MarkerLayer);

            sf.Labels.Label[_shapeidx, 0].Visible = false;
            map.Redraw();

            double ex = 0, ey = 0;
            map.ProjToPixel(sf.Shape[_shapeidx].Center.x, sf.Shape[_shapeidx].Center.y, ref ex, ref ey);
            txtLabel.Location = new System.Drawing.Point((int)ex - 40, (int)ey + 35);
            txtLabel.Text = sf.CellValue[0, _shapeidx].ToString();
            txtLabel.SelectAll();
            txtLabel.BringToFront();
            txtLabel.Visible = true;
            txtLabel.Focus();
            renaminglabel = true;

            _posx = sf.Shape[_shapeidx].Center.x;
            _posy = sf.Shape[_shapeidx].Center.y;
        }

        private void txtLabel_Leave(object sender, EventArgs e)
        {
            // prevent focus from leaving the edit box
            txtLabel.Focus();
        }

        private void contextMenuStrip1_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            map.ShapeEditor.SaveChanges();
        }

        private void showLabelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sf = map.get_Shapefile(MarkerLayer);

            ToolStripMenuItem m = sender as ToolStripMenuItem;
            if (m.Text == "Hide Label")
            {
                _fieldIndex = sf.Table.FieldIndexByName["ShowLabel"];
                sf.EditCellValue(_fieldIndex, _shapeidx, false);
                sf.Labels.Label[_shapeidx, 0].Visible = false;
                
            }
            else if(m.Text == "Show Label")
            {
                _fieldIndex = sf.Table.FieldIndexByName["ShowLabel"];
                sf.EditCellValue(_fieldIndex, _shapeidx, true);
                sf.Labels.Label[_shapeidx, 0].Visible = true;
            }
            map.Redraw();

        }
    }
}