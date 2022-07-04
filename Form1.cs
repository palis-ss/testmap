using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using AxMapWinGIS;
using MapWinGIS;
using Image = MapWinGIS.Image;

namespace testmap
{
    public partial class Form1 : Form
    {
        private AxMap map;
        int MarkerLayer = -1;
        
        modes mode;

        public Form1()
        {
            InitializeComponent();
            map = new AxMap();
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
            mode = modes.selection;

            map.KnownExtents = tkKnownExtents.keThailand;
            map.CurrentZoom = 8;

            map.SendMouseDown = true;
            map.SendMouseMove = true;
            map.MouseDownEvent += map_MouseDown;            
            map.ShapeIdentified += map_ShapeIdentified;
            map.CursorMode = tkCursorMode.cmNone;

            cbIcon.Items.Add("car");
            cbIcon.Items.Add("house");
            cbIcon.SelectedIndex = 0;

            InitLayers();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            panel1.Size = new Size(control.ClientSize.Width - 20, control.ClientSize.Height - 100);
            if (map != null)
                map.Size = panel1.Size;            
        }
        private void btnMode_Click(object sender, EventArgs e)
        {
            Shapefile sf = map.get_Shapefile(MarkerLayer);
            switch (mode)
            {
                case modes.selection:
                    // changing to marking
                    mode = modes.marking;
                    
                    lblMode.Text = "Current Mode: Add Marker";                    
                    sf.StartEditingShapes(true, null);
                    sf.InteractiveEditing = true;
                    map.CursorMode = tkCursorMode.cmAddShape;                    
                    map.ChooseLayer += map_ChooseLayer;
                    //map.MapCursor = tkCursor.;

                    lblIcon.Visible = true;
                    cbIcon.Visible = true;
                    break;
                case modes.marking:
                    mode = modes.delete;
                    lblMode.Text = "Current Mode: Delete Marker";
                    map.CursorMode = tkCursorMode.cmIdentify;
                    //map.MapCursor = tkCursor.crsrHand;

                    lblIcon.Visible = false;
                    cbIcon.Visible = false;
                    break;
                case modes.delete:
                    mode = modes.selection;
                    sf.StopEditingShapes();
                    lblMode.Text = "Current Mode: Selection";
                    map.CursorMode = tkCursorMode.cmNone;
                    //map.MapCursor = tkCursor.crsrMapDefault;

                    lblIcon.Visible = false;
                    cbIcon.Visible = false;
                    break;
            }
        }

        private void map_ChooseLayer(object sender, AxMapWinGIS._DMapEvents_ChooseLayerEvent e)
        {
            if (map.CursorMode == tkCursorMode.cmAddShape)  // cmMoveShapes, etc
            {
                e.layerHandle = MarkerLayer;
            }
        }

        public void map_ShapeIdentified(object sender, _DMapEvents_ShapeIdentifiedEvent e)
        {
            if (mode != modes.delete)
                return;

            Shapefile sf = map.get_Shapefile(MarkerLayer);
            if (sf != null)
            {                
                sf.EditDeleteShape(e.shapeIndex);                
                map.Redraw();                
            }
        }

        public void map_MouseDown(object sender, _DMapEvents_MouseDownEvent e)
        {
            if (e.button == 1)          // left button
            {
                if(mode == modes.marking)
                    Mark(e.x, e.y, cbIcon.Text);
            }
        }

        public void Mark(int ex, int ey, string icon)
        {
            Shapefile sf = map.get_Shapefile(MarkerLayer);
            Shape shp = new Shape();
            shp.Create(ShpfileType.SHP_POINT);

            MapWinGIS.Point pnt = new MapWinGIS.Point();
            //int handle = map.NewDrawing(tkDrawReferenceList.dlSpatiallyReferencedList);

            double x = 0.0;
            double y = 0.0;
            map.PixelToProj(ex, ey, ref x, ref y);
            pnt.x = x;
            pnt.y = y;

            shp.AddPoint(x, y);
            int idx = sf.EditAddShape(shp);
            sf.ShapeCategory2[idx] = icon;

            //int handle = map.NewDrawing(tkDrawReferenceList.dlSpatiallyReferencedList);
            //map.DrawCircleEx(handle, pnt.x, pnt.y, 5.0, 255, true);

            //map.Redraw2(tkRedrawType.RedrawMinimal);                
            map.Redraw();
        }

        // map layers
        // 1) base map
        // 2) markers
        // 3) tracking
        public void InitLayers()
        {
            string _iconPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\..\..\..\icons\";

            var sf = new Shapefile();
            sf.CreateNew("", ShpfileType.SHP_POINT);
            sf.DefaultDrawingOptions.AlignPictureByBottom = false;

            // loading icons
            var ct = sf.Categories.Add("car");            
            ct.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
            var img = new Image();
            if(img.Open(_iconPath+@"car-32.png"))
                ct.DrawingOptions.Picture = img;

            // loading icons
            ct = sf.Categories.Add("house");
            ct.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
            img = new Image();
            if (img.Open(_iconPath + @"house-32.png"))
                ct.DrawingOptions.Picture = img;

            sf.DefaultDrawingOptions.Visible = false;
            MarkerLayer = map.AddLayer(sf, true);
        }
    }

    enum modes
    {
        selection,
        marking,
        delete
    };

}