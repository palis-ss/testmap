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
        private AxMap map;
        int MarkerLayer = -1;
        MapMode mode;
        bool editinglabel = false;
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
            mode = MapMode.Nothing;
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

            map.CursorMode = tkCursorMode.cmIdentify;
            map.MapCursor = tkCursor.crsrHelp;
            //map.CursorMode = tkCursorMode.cmNone;
            map.ChooseLayer += Map_ChooseLayer;
            map.ShapeIdentified += Map_ShapeIdentified;
            map.SendMouseDown = true;
            map.MouseDownEvent += Map_MouseDownEvent;
            map.AfterShapeEdit += Map_AfterShapeEdit;
            map.Focus();
            map.Identifier.IdentifierMode = tkIdentifierMode.imSingleLayer;

            InitLayers();

            cbIcon.Items.Add("car");
            cbIcon.Items.Add("house");
            if (cbIcon.Items.Count > 0)
                cbIcon.SelectedIndex = 0;


        }

        private void Map_AfterShapeEdit(object sender, _DMapEvents_AfterShapeEditEvent e)
        {
            switch (e.operation)
            {
                case tkUndoOperation.uoAddShape:
                    Shapefile sf = map.get_Shapefile(MarkerLayer);
                    _fieldIndex = sf.Table.FieldIndexByName["Icon"];
                    sf.EditCellValue(_fieldIndex, e.shapeIndex, cbIcon.Text);                    
                    sf.Categories.ApplyExpressions();  // render matching icon
                    map.Redraw();

                    break;
            }
        }

        private void Map_MouseDownEvent(object sender, _DMapEvents_MouseDownEvent e)
        {
            if (editinglabel)
                return;
            if (mode == MapMode.Add)
            {
                MarkPoint(e.x, e.y, cbIcon.Text);
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
            if (editinglabel)
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
            editinglabel = true;
            
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
                case MapMode.Nothing:
                    double px = 0, py = 0;
                    map.ProjToPixel(e.pointX, e.pointY, ref px, ref py);

                    contextMenuStrip1.Show(map, (int)px, (int)py);
                    map.IdentifiedShapes.Clear();

                    break;
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
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (editinglabel)
            {
                var sf = map.get_Shapefile(MarkerLayer);

                if (keyData == Keys.Escape)
                {
                    _fieldIndex = sf.Table.FieldIndexByName["ShowLabel"];
                    sf.EditCellValue(_fieldIndex, _shapeidx, false);
                    txtLabel.Visible = false;
                    editinglabel = false;
                    return true;
                }
                else if (keyData == Keys.Enter)
                {                
                    _fieldIndex = sf.Table.FieldIndexByName["Name"];
                    sf.EditCellValue(_fieldIndex, _shapeidx, txtLabel.Text);
                    _fieldIndex = sf.Table.FieldIndexByName["ShowLabel"];
                    sf.EditCellValue(_fieldIndex, _shapeidx, true);
                    txtLabel.Visible = false;
                    editinglabel = false;


                    sf.Labels.AddLabel(txtLabel.Text, _posx, _posy);                    
                    map.Redraw();
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
                    break;
                case Keys.D:
                    map.IdentifiedShapes.Clear();
                    map.Redraw();

                    map.CursorMode = tkCursorMode.cmIdentify;
                    map.MapCursor = tkCursor.crsrHand;
                    mode = MapMode.Delete;
                    lblMapMode.Text = "Mode: Remove marker";
                    break;
                case Keys.Escape:                    
                    map.IdentifiedShapes.Clear();
                    map.Redraw();

                    //map.CursorMode = tkCursorMode.cmIdentify;
                    map.MapCursor = tkCursor.crsrHelp;
                    map.CursorMode = tkCursorMode.cmIdentify;   // just to play around
                    /*
                    map.MapCursor = tkCursor.crsrUserDefined;
                    map.UDCursorHandle = Cursors.Help.Handle.ToInt32();
                    */
                    mode = MapMode.Nothing;
                    lblMapMode.Text = "Mode: Selection";
                    break;
                case Keys.Delete:                    
                    var shapes = map.IdentifiedShapes;
                    if (shapes.Count > 0)
                    {
                        var sf = map.get_Shapefile(shapes.LayerHandle[0]);
                        sf.EditDeleteShape(shapes.ShapeIndex[0]);
                        map.IdentifiedShapes.Clear();
                        map.Redraw();
                    }
                    break;                
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
            return true;
        }

        private enum MapMode
        {
            Nothing,
            Add,
            Delete
        };

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (editinglabel)
                return;
            var sf = map.get_Shapefile(MarkerLayer);
            sf.SaveAs("Markers.shp");
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (editinglabel)
                return;
            InitLayers();  // creating a new layer for loading data
            var sf = map.get_Shapefile(MarkerLayer);  // MarkerLayer is the newly created layer
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

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hello");
        }
    }
}