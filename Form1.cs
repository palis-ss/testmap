using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Input;
using System.Runtime.InteropServices;
using AxMapWinGIS;
using MapWinGIS;
using System.IO.Ports;
using System.Threading;
using Geo.Gps.Serialization;
using Geo.Gps;

namespace testmap
{
    public partial class Form1 : Form
    {        
        #region Constants
        const string EDIT_INFOTXT = @"Click a marker to start editing, then
- DEL to delete
- Click and drag to move.
- Right click for more options.
- Click anywhere else to finish";
        const string ADD_INFOTXT = @"Click a location on map to add, or push this button ->
ESC to go back to Edit mode";
        const string PAN_INFOTXT = @"Click and drag to pan
ESC to go back to Edit mode";
        const string TILENAME = @"MapTilerOutdoor";
        const string TILEURL = @"https://api.maptiler.com/maps/topographique/256/{zoom}/{x}/{y}.png?key=ZnCLN1UxbkcF74yFeryt";
        //const string TILEURL = @"https://api.maptiler.com/maps/hybrid/{zoom}/{x}/{y}.jpg?key=ZnCLN1UxbkcF74yFeryt";
        //const string TILEURL = @"https://api.maptiler.com/tiles/satellite-v2/{zoom}/{x}/{y}.jpg?key=ZnCLN1UxbkcF74yFeryt";
        #endregion

        #region Locals
        AxMap map;
        int MarkerLayer = -1;
        bool deleting = false;
        int _shapeidx = -1;
        #endregion
        static SerialPort _serialPort = new SerialPort();
        Thread readThread = new Thread(ReadGPSData);
        static bool _continue;

        #region Initialization
        public Form1()
        {
            InitializeComponent();
            map = new AxMap();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new Size(1280, 960);
            this.CenterToScreen();
            panel1.Size = new Size(ClientSize.Width - 20, ClientSize.Height - 210);
            panel1.Location = new System.Drawing.Point(10, 10);

            map.Location = new System.Drawing.Point(0, 0);
            map.Size = panel1.Size;
            panel1.Controls.Add(map);

            InitMap();
            InitLayers();

            lblMapMode.Text = "Mode: Edit markers";
            lblInfo.Text = EDIT_INFOTXT;

            // these need to be setup 1 time only
            map.ChooseLayer += Map_ChooseLayer;
            map.MouseDownEvent += Map_MouseDownEvent;
            map.AfterShapeEdit += Map_AfterShapeEdit;
            map.BeforeShapeEdit += Map_BeforeShapeEdit;
            //map.ShapeIdentified += Map_ShapeIdentified;
            map.DblClick += Map_DblClick;

            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
                cbPort.Items.Add(port);
            if (cbPort.Items.Count > 0)
                cbPort.SelectedIndex = 0;

            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;
        }

        public void InitMap()
        {
            map.Clear();

            // loading custom tile provider
            int providerId = (int)tkTileProvider.ProviderCustom + 10;
            if (!map.Tiles.Providers.Add(providerId, TILENAME, TILEURL, tkTileProjection.SphericalMercator))
            {
                map.get_ErrorMsg(map.LastErrorCode);
            }

            map.Projection = tkMapProjection.PROJECTION_GOOGLE_MERCATOR;
            //map.TileProvider = tkTileProvider.OpenStreetMap;  // select a default tile provider
            map.Tiles.ProviderId = providerId;  // select custom tile provider

            map.KnownExtents = tkKnownExtents.keThailand;
            map.CurrentZoom = 8;

            map.ShapeEditor.EditorBehavior = tkEditorBehavior.ebVertexEditor;
            map.ShapeEditor.IndicesVisible = false;
            map.CursorMode = tkCursorMode.cmEditShape;
            map.MapCursor = tkCursor.crsrHand;
            btnAddMarker.Visible = false;

            map.SendMouseDown = true;
            map.Focus();
        }

        // map layers
        // 1) base map
        // 2) markers
        // 3) tracking
        public void InitLayers()
        {
            map.RemoveAllLayers();

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


            LoadIcons();
            sf.DefaultDrawingOptions.Visible = true;
            sf.InteractiveEditing = true;
            //sf.Labels.Synchronized = true;
        }

        public void LoadIcons()
        {
            string _iconPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\..\..\..\icons\";

            Shapefile sf = map.get_Shapefile(MarkerLayer);
            var ct = sf.Categories.Add("car");
            ct.Expression = "[Icon]=\"car\"";
            ct.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
            var img = new MapWinGIS.Image();
            if (img.Open(_iconPath + @"car-32.png"))
                ct.DrawingOptions.Picture = img;

            ct = sf.Categories.Add("house");
            ct.Expression = "[Icon]=\"house\"";
            ct.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
            img = new MapWinGIS.Image();
            if (img.Open(_iconPath + @"house-32.png"))
                ct.DrawingOptions.Picture = img;

            var labels = sf.Labels;
            labels.FrameVisible = true;
            labels.FrameType = tkLabelFrameType.lfRectangle;
            labels.Alignment = tkLabelAlignment.laBottomCenter;
            labels.OffsetY = 16;
        }
        #endregion

        #region Map event handlers
        private void Form1_Resize(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            panel1.Size = new Size(control.ClientSize.Width - 20, control.ClientSize.Height - 210);
            if (map != null)
                map.Size = panel1.Size;
        }

        private void Map_BeforeShapeEdit(object sender, _DMapEvents_BeforeShapeEditEvent e)
        {
            _shapeidx = e.shapeIndex;
        }

        private void Map_AfterShapeEdit(object sender, _DMapEvents_AfterShapeEditEvent e)
        {
            Shapefile sf = map.get_Shapefile(MarkerLayer);
            double projx = 0, projy = 0;
            double lat = 0, lon = 0;
            int fieldidx;

            if (e.operation != tkUndoOperation.uoRemoveShape)
            {
                projx = sf.Shape[e.shapeIndex].Center.x;
                projy = sf.Shape[e.shapeIndex].Center.y;
                map.ProjToDegrees(projx, projy, ref lon, ref lat);
            }

            switch (e.operation)
            {
                case tkUndoOperation.uoAddShape:
                    AddMarker dlg = new AddMarker();
                    dlg.PosLat = lat;
                    dlg.PosLon = lon;
                    dlg.ShowLabel = true;
                    dlg.UpdateData();
                    dlg.StartPosition = FormStartPosition.CenterParent;

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        sf.Labels.InsertLabel(e.shapeIndex, dlg.PosName, projx, projy);
                        UpdateMarker(e.shapeIndex, dlg.PosName, dlg.PosLat, dlg.PosLon, dlg.PosHeight, dlg.PosIcon, dlg.ShowLabel);

                        map.ShapeEditor.SaveChanges();
                        map.CursorMode = tkCursorMode.cmEditShape;
                        map.MapCursor = tkCursor.crsrHand;
                        lblMapMode.Text = "Mode: Edit markers";
                        lblInfo.Text = EDIT_INFOTXT;
                        btnAddMarker.Visible = false;
                    }
                    else
                    {
                        sf.EditDeleteShape(e.shapeIndex);
                        map.ShapeEditor.SaveChanges();
                    }

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

                    fieldidx = sf.Table.FieldIndexByName["Lat"];
                    sf.EditCellValue(fieldidx, e.shapeIndex, lat);
                    fieldidx = sf.Table.FieldIndexByName["Lon"];
                    sf.EditCellValue(fieldidx, e.shapeIndex, lon);

                    // moving label along                    
                    if (sf.Labels.Count > 0)
                    {
                        sf.Labels.Label[e.shapeIndex, 0].x = projx;
                        sf.Labels.Label[e.shapeIndex, 0].y = projy;
                    }
                    map.Redraw();
                    break;
            }
        }

        private void Map_MouseDownEvent(object sender, _DMapEvents_MouseDownEvent e)
        {
            if (deleting)
                return;

            if (map.ShapeEditor.EditorState == tkEditorState.esEdit)
            {
                contextMenuStrip1.Show(map, e.x, e.y);
            }

        }

        private void Map_ChooseLayer(object sender, _DMapEvents_ChooseLayerEvent e)
        {
            e.layerHandle = MarkerLayer;
        }

        private void Map_DblClick(object sender, EventArgs e)
        {
            btnAddMarker_Click(sender, e);
        }
        #endregion

        #region Button handlers
        private void btnAddMarker_Click(object sender, EventArgs e)
        {
            /*
            Shapefile sf = map.get_Shapefile(MarkerLayer);
            MarkLoc(13.5, 100.5 + 0.5 * sf.NumShapes, cbIcon.Text);
            return;
            */
            AddMarker dlg = new AddMarker();
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.ShowDialog();
            if (dlg.DialogResult == DialogResult.OK)
            {
                Shapefile sf = map.get_Shapefile(MarkerLayer);
                Shape shp = new Shape();
                shp.Create(ShpfileType.SHP_POINT);

                double projx = 0.0;
                double projy = 0.0;
                map.DegreesToProj(dlg.PosLon, dlg.PosLat, ref projx, ref projy);
                shp.AddPoint(projx, projy);
                int shapeidx = sf.EditAddShape(shp);
                sf.Labels.InsertLabel(shapeidx, dlg.PosName, projx, projy);

                UpdateMarker(shapeidx, dlg.PosName, dlg.PosLat, dlg.PosLon, dlg.PosHeight, dlg.PosIcon, dlg.ShowLabel);

                map.CursorMode = tkCursorMode.cmEditShape;
                map.MapCursor = tkCursor.crsrHand;
                lblMapMode.Text = "Mode: Edit markers";
                lblInfo.Text = EDIT_INFOTXT;
                btnAddMarker.Visible = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Shapefile sf = map.get_Shapefile(MarkerLayer);
            sf.SaveAs("Markers.shp");
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            InitMap();
            InitLayers();  // creating a new layer for loading data
            Shapefile sf = map.get_Shapefile(MarkerLayer);  // MarkerLayer is the newly created layer
            if (sf.LoadDataFrom("Markers.shp") == false)
            {
                MessageBox.Show(sf.ErrorMsg[sf.LastErrorCode]);
            }
            else
            {
                // update icons
                sf.Categories.ApplyExpressions();

                // update labels
                for (int i = 0; i < sf.NumShapes; i++)
                {
                    bool showlabel = (bool)sf.Table.CellValue[sf.FieldIndexByName["ShowLabel"], i];
                    string? labeltext;
                    if (!showlabel)
                        labeltext = "";
                    else
                        labeltext = sf.Table.CellValue[sf.FieldIndexByName["Name"], i].ToString();

                    sf.Labels.InsertLabel(i, labeltext, sf.Shape[i].Center.x, sf.Shape[i].Center.y);
                    sf.Labels.Label[i, 0].Visible = showlabel;
                }
                map.Redraw();
            }
        }
        #endregion

        #region Keyboard handler (ProcessCmdKey)
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Insert:
                    map.IdentifiedShapes.Clear();
                    map.Redraw();

                    map.MapCursor = tkCursor.crsrCross;
                    map.CursorMode = tkCursorMode.cmAddShape;
                    //map.MapCursor = tkCursor.crsrMapDefault;

                    lblMapMode.Text = "Adding a marker";
                    lblInfo.Text = ADD_INFOTXT;
                    btnAddMarker.Visible = true;
                    break;
                case Keys.Escape:
                    map.IdentifiedShapes.Clear();
                    map.Redraw();

                    //map.CursorMode = tkCursorMode.cmIdentify;
                    //map.MapCursor = tkCursor.crsrHelp;
                    map.MapCursor = tkCursor.crsrHand;
                    map.CursorMode = tkCursorMode.cmEditShape;   // just to play around                    
                    /*
                    map.MapCursor = tkCursor.crsrUserDefined;
                    map.UDCursorHandle = Cursors.Help.Handle.ToInt32();
                    */
                    lblMapMode.Text = "Mode: Edit markers";
                    lblInfo.Text = EDIT_INFOTXT;
                    btnAddMarker.Visible = false;
                    break;
                case Keys.F8:
                    map.MapCursor = tkCursor.crsrMapDefault;
                    map.CursorMode = tkCursorMode.cmPan;
                    lblMapMode.Text = "Mode: Pan map";
                    lblInfo.Text = PAN_INFOTXT;
                    btnAddMarker.Visible = false;
                    break;
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
            return true;
        }
        #endregion

        #region Helper functions
        public void UpdateMarker(int shapeidx, string? name, double lat, double lon, double height, string? icon, bool showlabel)
        {
            Shapefile sf = map.get_Shapefile(MarkerLayer);
            int fieldidx;
            double projx = 0.0;
            double projy = 0.0;
            map.DegreesToProj(lon, lat, ref projx, ref projy);

            // update table
            fieldidx = sf.FieldIndexByName["Name"];
            sf.EditCellValue(fieldidx, shapeidx, name);
            fieldidx = sf.FieldIndexByName["Lat"];
            sf.EditCellValue(fieldidx, shapeidx, lat);
            fieldidx = sf.FieldIndexByName["Lon"];
            sf.EditCellValue(fieldidx, shapeidx, lon);
            fieldidx = sf.FieldIndexByName["Height"];
            sf.EditCellValue(fieldidx, shapeidx, height);
            fieldidx = sf.FieldIndexByName["ShowLabel"];
            sf.EditCellValue(fieldidx, shapeidx, showlabel);
            fieldidx = sf.FieldIndexByName["Icon"];
            sf.EditCellValue(fieldidx, shapeidx, icon);

            // update icon
            sf.Categories.ApplyExpressions();  // render matching icon

            // update label
            string? labeltext;
            if (!showlabel)
                labeltext = "";
            else
                labeltext = name;

            sf.Labels.Label[shapeidx, 0].Text = labeltext;
            sf.Labels.Label[shapeidx, 0].Visible = showlabel;
            sf.Labels.Label[shapeidx, 0].x = projx;
            sf.Labels.Label[shapeidx, 0].y = projy;
            map.Redraw();
        }
        #endregion

        #region ToolStrip handlers
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
            int fieldidx;

            AddMarker dlg = new AddMarker();
            fieldidx = sf.FieldIndexByName["Name"];
            dlg.PosName = sf.CellValue[fieldidx, _shapeidx].ToString();
            fieldidx = sf.FieldIndexByName["Lat"];
            dlg.PosLat = (double)sf.CellValue[fieldidx, _shapeidx];
            fieldidx = sf.FieldIndexByName["Lon"];
            dlg.PosLon = (double)sf.CellValue[fieldidx, _shapeidx];
            fieldidx = sf.FieldIndexByName["Height"];
            dlg.PosHeight = (double)sf.CellValue[fieldidx, _shapeidx];
            fieldidx = sf.FieldIndexByName["ShowLabel"];
            dlg.ShowLabel = (bool)sf.CellValue[fieldidx, _shapeidx];
            fieldidx = sf.FieldIndexByName["Icon"];
            dlg.PosIcon = sf.CellValue[fieldidx, _shapeidx].ToString();

            dlg.UpdateData();
            dlg.StartPosition = FormStartPosition.CenterParent;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Shape shp = new Shape();
                shp.Create(ShpfileType.SHP_POINT);

                double projx = 0, projy = 0;
                map.DegreesToProj(dlg.PosLon, dlg.PosLat, ref projx, ref projy);
                shp.AddPoint(projx, projy);
                sf.EditUpdateShape(_shapeidx, shp);
                sf.ShapeCategory2[_shapeidx] = dlg.PosIcon;

                UpdateMarker(_shapeidx, dlg.PosName, dlg.PosLat, dlg.PosLon, dlg.PosHeight, dlg.PosIcon, dlg.ShowLabel);
            }
        }

        private void contextMenuStrip1_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            map.ShapeEditor.SaveChanges();
        }
        #endregion

        private void btnGPS_Click(object sender, EventArgs e)
        {
            if (_serialPort.IsOpen)
            {
                _continue = false;
                readThread.Join();
                _serialPort.Close();
                btnGPS.Text = "Link GPS";
                lblGPS.Text = "GPS not linked";
            }
            else
            {
                if (cbPort.Items.Count == 0)
                {
                    MessageBox.Show("No available port");
                    return;
                }
                _serialPort.PortName = cbPort.Text;
                //_serialPort.BaudRate = 115200;
                _serialPort.BaudRate = 4800;
                _serialPort.DataBits = 8;
                _serialPort.Parity = Parity.None;
                _serialPort.StopBits = StopBits.One;
                _serialPort.Handshake = Handshake.None;

                try
                {
                    _continue = true;
                    _serialPort.Open();

                    if (_serialPort.IsOpen)
                    {
                        btnGPS.Text = "Disconnect GPS";
                        lblGPS.Text = "GPS linked";
                        //readThread = new Thread(ReadGPSData);
                        readThread.Start();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public static void ReadGPSData()
        {            
            while (_continue)
            {
                try
                {
                    if (_serialPort.BytesToRead > 0)
                    {
                        string message = _serialPort.ReadLine();
                        Debug.WriteLine(message);                        
                    }
                }
                catch (TimeoutException) { }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (readThread!=null && readThread.IsAlive)
            {
                _continue = false;
                readThread.Join();
            }
        }
    }
}