using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CografiBilgiSistemi1
{
    public partial class Form1 : Form
    {
        GMapOverlay katman1;
        List<Arac> list;

        SqlConnection baglanti = new SqlConnection(@"Data Source = CIBI; Initial Catalog = projeVT; Integrated Security = True; Encrypt=False");
        public Form1()
        {
            InitializeComponent();
            initializeMap();
            aracListesiniOlustur();
        }


        private void araclariHaritadaGoster()
        {
            foreach (Arac arac in list)
            {
                GMarkerGoogle markerTmp = new GMarkerGoogle(arac.Konum, GMarkerGoogleType.green_dot);
                markerTmp.Tag = arac.Plaka;
                markerTmp.ToolTipText = arac.ToString();
                //kaatmanımıza aracları ekleyelim...
                katman1.Markers.Add(markerTmp);

                markerTmp.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                Console.WriteLine(arac.ToString());
            }
        }

        private void aracListesiniOlustur()
        {
            list = new List<Arac>();


            // VT den ADO.NET ile veri cekme
            try
            {
                baglanti.Open();
                string sqlCumlesi = "SELECT Plaka, AracTipi, Baslangic, Bitis, Enlem, Boylam FROM Araclar";

                SqlDataAdapter da = new SqlDataAdapter(sqlCumlesi, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                }

                //Listemizin içini çekilen verilerle doldurucaz

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(new Arac(dt.Rows[i][0].ToString(),
                                      dt.Rows[i][1].ToString(),
                                      dt.Rows[i][2].ToString(),
                                      dt.Rows[i][3].ToString(),
                                      new PointLatLng(Convert.ToDouble(dt.Rows[i][4].ToString()),
                                                      Convert.ToDouble(dt.Rows[i][5].ToString()))));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı Bağlantısı Sırasında Bir Hata Oluştu, Hata Kodu 101\n" + ex.Message);
            }

            finally
            {
                if (baglanti != null)
                    baglanti.Close();
            }

            araclariHaritadaGoster();

        }
        private void initializeMap()
        {
            map.DragButton = MouseButtons.Left;
            map.MapProvider = GMapProviders.OpenStreetMap;
            map.Position = new GMap.NET.PointLatLng(36.0, 42.0);
            map.Zoom = 4;
            map.MinZoom = 3;
            map.MaxZoom = 25;
            // bir overlay (katman)olusturmamız lazım
            katman1 = new GMapOverlay();
            //harita üzerinde görüntülenecek tüm compenentleri bu katmana eklememiz gerkmektedir.
            // ilk olarak olsuturdugumuz katmanı haritamıza eklemeliyiz
            map.Overlays.Add(katman1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PointLatLng lokasyon1 = new PointLatLng(Convert.ToDouble(textBoxEnlem.Text),
                                                   (Convert.ToDouble(textBoxBoylam.Text)));
            GMarkerGoogle marker = new GMarkerGoogle(lokasyon1, GMarkerGoogleType.red_dot);
            marker.ToolTipText = "\nLokasyon1\nTır\nForm: Antalya\nTo :İzmir";
            marker.ToolTip.Fill = Brushes.Black;
            marker.ToolTip.Foreground=Brushes.White;
            marker.ToolTip.Stroke = Pens.Black;
            marker.ToolTip.TextPadding = new Size(5, 5);
            marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
            marker.Tag = 101;

            // daha sonra  marker ları katmana ekle
            //DİKKAT
            //markerı önce eklersek yanlıs olbilir
            katman1.Markers.Add(marker);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            map.Dispose();
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PointLatLng lokasyon2 = new PointLatLng(Convert.ToDouble(textBox2.Text),
                                                   (Convert.ToDouble(textBox1.Text)));
            GMarkerGoogle marker2 = new GMarkerGoogle(lokasyon2, GMarkerGoogleType.blue_dot);
            marker2.Tag = 102;
            katman1.Markers.Add(marker2);
        }

        private void map_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            //string markerId = Convert.ToString(item.Tag);
            //Console.WriteLine("id:" + markerId + "olan markera tıklandı");  


            string secilenAracinPlakasi = Convert.ToString(item.Tag);

            foreach (Arac arac in list)
            {
                if(secilenAracinPlakasi.Equals(arac.Plaka))
                {
                    textBox3.Text = arac.Plaka;
                    textBox4.Text = arac.Tipi;
                    textBox5.Text = arac.Baslangic;
                    textBox6.Text = arac.Bitis;
                    break; 
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach(Arac arac in list)
            {
                araclariHaritadaGoster();
            }
        }
    }
}

