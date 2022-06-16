namespace testmap
{
    public partial class Form1 : Form
    {
        private AxMapWinGIS.AxMap map = new AxMapWinGIS.AxMap();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.map.Location = new Point(10, 10);
            this.map.Size = new System.Drawing.Size(1024, 768);
            this.Controls.Add(map);

            map.CursorMode = MapWinGIS.tkCursorMode.cmPan;
            map.Projection = MapWinGIS.tkMapProjection.PROJECTION_WGS84;
            map.TileProvider = MapWinGIS.tkTileProvider.OpenStreetMap;
            map.SetLatitudeLongitude(13, 100);            
            map.ZoomToTileLevel(8);
        }
    }
}