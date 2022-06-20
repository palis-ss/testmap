using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using AxMapWinGIS;
using MapWinGIS;

namespace testmap
{
    public partial class Form1 : Form
    {
        private AxMap map;

        public Form1()
        {
            InitializeComponent();
            map = new AxMap();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.Size = new Size(ClientSize.Width - 20, ClientSize.Height - 100);

            map.Location = new System.Drawing.Point(0, 0);            
            map.Size = panel1.Size;
            panel1.Controls.Add(map);

            
            TileProviders providers = map.Tiles.Providers;
            int providerId = (int)tkTileProvider.ProviderCustom + 10;            
            bool ret = providers.Add(providerId, "MapTilerOutdoor",
                @"https://api.maptiler.com/maps/topographique/256/{zoom}/{x}/{y}.png?key=ZnCLN1UxbkcF74yFeryt",
                tkTileProjection.SphericalMercator);
            /*
            int error = map.Tiles.LastErrorCode;
            Debug.WriteLine(ret.ToString() + error.ToString());
            Debug.WriteLine(map.Tiles.Providers.ErrorMsg[error]);
            */
            
            map.Projection = tkMapProjection.PROJECTION_GOOGLE_MERCATOR;            
            map.Tiles.ProviderId = providerId;            
            map.CursorMode = tkCursorMode.cmPan;

            map.KnownExtents = tkKnownExtents.keThailand;
            map.CurrentZoom = 8;

            map.SendMouseDown = true;
            map.MouseDownEvent += map_MouseDown;            
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            panel1.Size = new Size(control.ClientSize.Width-20, control.ClientSize.Height-100);
            if(map != null)
                map.Size = panel1.Size;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (map.CursorMode == MapWinGIS.tkCursorMode.cmPan)
            {
                map.CursorMode = tkCursorMode.cmNone;
                button1.Text = "Marking Off";
            }
            else
            {
                map.CursorMode = tkCursorMode.cmPan;
                button1.Text = "Marking On";
            }
        }

        public void map_MouseDown(object sender, _DMapEvents_MouseDownEvent e)
        {
            if (e.button == 1)          // left button
            {                                
                MapWinGIS.Point pnt = new MapWinGIS.Point();
                int handle = map.NewDrawing(tkDrawReferenceList.dlSpatiallyReferencedList);                

                double x = 0.0;
                double y = 0.0;
                map.PixelToProj(e.x, e.y, ref x, ref y);
                pnt.x = x;
                pnt.y = y;
                
                map.DrawCircleEx(handle, pnt.x, pnt.y, 5.0, 255, true);
                map.Redraw2(tkRedrawType.RedrawMinimal);
            }
        }
    }
}