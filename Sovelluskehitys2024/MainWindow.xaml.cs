using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ControlzEx.Theming;
using MahApps.Metro.Controls;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;

namespace Sovelluskehitys2024
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        string polku = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\onnis\\OneDrive\\Tiedostot\\testitietokanta.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True";
        public MainWindow()
        {
            InitializeComponent();

            ThemeManager.Current.ChangeTheme(this, "Light.Taupe");

            try
            {
                
                PaivitaDataGrid("SELECT * FROM autot", "autot", autolista);
                PaivitaComboBox(autolista_cb);

                PaivitaDataGrid("SELECT * FROM huollot", "huollot", huoltolista);
                PaivitaComboBox(huoltolista_cb);

                PaivitaDataGrid("SELECT * FROM huoltokuvat", "huoltokuvat", huoltokuvalista);
                PaivitaComboBoxKuva(huoltokuvalista_cb);

                PaivitaComboBoxKaikkihuollot(autokohtainenlista_cb);


                /*
                PaivitaDataGrid("SELECT * FROM asiakkaat", "asiakkaat", asiakaslista);
                PaivitaDataGrid("SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu WHERE a.id=ti.asiakas_id AND tu.id=ti.tuote_id AND ti.toimitettu='0'", "tilaukset", tilauslista);
                PaivitaDataGrid("SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu WHERE a.id=ti.asiakas_id AND tu.id=ti.tuote_id AND ti.toimitettu='1'", "tilaukset", toimitetutlista);
                PaivitaComboBox(tuotelista_cb);
                PaivitaAsiakasComboBox();
                */
            }
            catch
            {
                tilaviesti.Text = "Ei tietokantayhteyttä";
            }
        }

        
        
private void PaivitaDataGrid(string kysely, string taulu, DataGrid grid)
{
   SqlConnection yhteys = new SqlConnection(polku);
   yhteys.Open();

   SqlCommand komento = yhteys.CreateCommand();
   komento.CommandText = kysely;

   SqlDataAdapter adapteri = new SqlDataAdapter(komento);
   DataTable dt = new DataTable(taulu);
   adapteri.Fill(dt);

   grid.ItemsSource = dt.DefaultView;

   yhteys.Close();
}

        private void autonlisäys(object sender, RoutedEventArgs e)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string kysely = "INSERT INTO autot (merkki, malli, rekisteri_nro) VALUES ('" + autonmerkki.Text + "','" + autonmalli.Text + "','" + autonrekno.Text + "');";
            SqlCommand komento = new SqlCommand(kysely, yhteys);
            komento.ExecuteNonQuery();

            yhteys.Close();
            PaivitaDataGrid("SELECT * FROM autot", "autot", autolista);
            PaivitaComboBox(autolista_cb);
            PaivitaComboBox(huoltolista_cb);
            PaivitaComboBoxKuva(huoltokuvalista_cb);
            PaivitaComboBox(autokohtainenlista_cb);

        }

        private void PaivitaComboBox(ComboBox autolista_cb)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string kysely = "SELECT rekisteri_nro FROM autot";
            SqlCommand komento = new SqlCommand(kysely, yhteys);
            SqlDataReader lukija = komento.ExecuteReader();

            autolista_cb.Items.Clear(); // Tyhjennetään ComboBox ennen päivittämistä

            while (lukija.Read())
            {
                autolista_cb.Items.Add(lukija["rekisteri_nro"].ToString());
            }

            lukija.Close();
            yhteys.Close();
        }



        private void autonpoisto(object sender, RoutedEventArgs e)
            {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string id = autolista_cb.SelectedValue.ToString();

            string kysely = "DELETE FROM autot WHERE rekisteri_nro='" + id + "';";
            SqlCommand komento = new SqlCommand(kysely, yhteys);
            komento.ExecuteNonQuery();
            yhteys.Close();

            PaivitaDataGrid("SELECT * FROM autot", "autot", autolista);
            PaivitaComboBox(autolista_cb);
            PaivitaDataGrid("SELECT * FROM huollot", "huollot", huoltolista);
            PaivitaComboBoxKuva(huoltokuvalista_cb);
            PaivitaDataGrid("SELECT * FROM huoltokuvat", "huoltokuvat", huoltokuvalista);
            PaivitaComboBox(autokohtainenlista_cb);
        }

        

        private void huollonlisäys(object sender, RoutedEventArgs e)
        {

            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string id = huoltolista_cb.SelectedValue.ToString();

            string kysely = "INSERT INTO huollot (huoltotyyppi, kilometrit, paivamaara, rekisteri_nro ) VALUES ('" + huoltotyyppi.Text + "','" + kilometrit.Text + "','" + paivamaara.Text + "','" + id + "');";
            SqlCommand komento = new SqlCommand(kysely, yhteys);
            komento.ExecuteNonQuery();
            yhteys.Close();

            PaivitaDataGrid("SELECT * FROM huollot", "huollot", huoltolista);
            PaivitaComboBox(huoltolista_cb);
            PaivitaComboBoxKuva(huoltokuvalista_cb);
            PaivitaDataGrid("SELECT * FROM huoltokuvat", "huoltokuvat", huoltokuvalista);
        }

        private void PaivitaComboBoxKuva(ComboBox huoltokuvalista_cb)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            // Hae huolto_id huollot-taulusta
            string kysely = "SELECT huolto_id FROM huollot";
            SqlCommand komento = new SqlCommand(kysely, yhteys);
            SqlDataReader lukija = komento.ExecuteReader();

            huoltokuvalista_cb.Items.Clear(); // Tyhjennetään ComboBox ennen päivitystä

            while (lukija.Read())
            {
                huoltokuvalista_cb.Items.Add(lukija["huolto_id"].ToString()); // Lisää huolto_id
            }

            lukija.Close();
            yhteys.Close();
        }


   

        
        private void TuoKuvaButton_Click(object sender, RoutedEventArgs e)
        {
            // Luo OpenFileDialog-ikkuna
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "JPEG Files (*.jpg;*.jpeg)|*.jpg;*.jpeg|All Files (*.*)|*.*",
                Title = "Valitse JPEG-kuva"
            };

            // Näytä dialogi ja käsittele valinta
            if (openFileDialog.ShowDialog() == true)
            {
                // Aseta valittu tiedostopolku TextBoxiin
                kuvaPolku.Text = openFileDialog.FileName;
            }
        }


        
        private void lisaahuoltokuva(object sender, RoutedEventArgs e)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string id = huoltokuvalista_cb.SelectedValue.ToString();

            string kysely = "INSERT INTO huoltokuvat (huolto_id, kuva_nimi, kuva) VALUES ('" + id + "','" + kuvanimi.Text + "','"+ kuvaPolku.Text + "');";
            SqlCommand komento = new SqlCommand(kysely, yhteys);
            komento.ExecuteNonQuery();
            yhteys.Close();

            PaivitaDataGrid("SELECT * FROM huoltokuvat", "huoltokuvat", huoltokuvalista);
            PaivitaComboBox(huoltokuvalista_cb);
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // Open the image using the default viewer for the image file type
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = e.Uri.AbsoluteUri,  // the image path
                UseShellExecute = true  // Ensures that the image is opened with the default associated application
            });
        }






        private void PaivitaComboBoxKaikkihuollot(ComboBox autokohtainenlista_cb)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string kysely = "SELECT rekisteri_nro FROM autot";
            SqlCommand komento = new SqlCommand(kysely, yhteys);
            SqlDataReader lukija = komento.ExecuteReader();

            autokohtainenlista_cb.Items.Clear(); // Tyhjennetään ComboBox ennen päivittämistä

            while (lukija.Read())
            {
                autokohtainenlista_cb.Items.Add(lukija["rekisteri_nro"].ToString());
            }

            lukija.Close();
            yhteys.Close();
        }


        /*
        private void haekaikkihuoltotiedot(object sender, RoutedEventArgs e)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string id = autokohtainenlista_cb.SelectedValue.ToString();

            string kysely = "SELECT autot.rekisteri_nro, huollot.huoltotyyppi, huollot.kilometrit, huollot.paivamaara, huoltokuvat.kuva_nimi FROM huollot JOIN huoltokuvat ON huollot.huolto_id = huoltokuvat.huolto_id JOIN autot ON huollot.rekisteri_nro = autot.rekisteri_nro WHERE autot.rekisteri_nro = '"+id+"';";
            
            SqlCommand komento = new SqlCommand(kysely, yhteys);
            komento.ExecuteNonQuery();
            yhteys.Close();

            PaivitaDataGrid("SELECT * FROM auton_huoltohistoria", "auton_huoltohistoria", autokohtainen_lista);
            PaivitaComboBox(autokohtainenlista_cb);
        }*/

        private void haekaikkihuoltotiedot(object sender, RoutedEventArgs e)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            // Haetaan valittu auton rekisterinumero
            string id = autokohtainenlista_cb.SelectedValue.ToString();

            // SQL SELECT kysely
            string kysely = @"
        SELECT 
            autot.rekisteri_nro, 
            huollot.huoltotyyppi, 
            huollot.kilometrit, 
            huollot.paivamaara, 
            huoltokuvat.kuva_nimi 
        FROM huollot 
        JOIN huoltokuvat ON huollot.huolto_id = huoltokuvat.huolto_id 
        JOIN autot ON huollot.rekisteri_nro = autot.rekisteri_nro 
        WHERE autot.rekisteri_nro = @rekisteri_nro";

            // Luo SqlCommand ja lisää parametrin
            SqlCommand komento = new SqlCommand(kysely, yhteys);
            komento.Parameters.AddWithValue("@rekisteri_nro", id);

            // Ajetaan kysely ja täytetään DataTable
            SqlDataAdapter adapter = new SqlDataAdapter(komento);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            // Asetetaan DataTable DataGridin DataSourceksi
            autokohtainen_lista.ItemsSource = dt.DefaultView;

            // Suljetaan yhteys tietokantaan
            yhteys.Close();

            // Päivitä ComboBox (jos tarpeen)
            PaivitaComboBox(autokohtainenlista_cb);
        }




        private void Button_Click_3(object sender, RoutedEventArgs e)
            {

            }



            private void Button_Click_4(object sender, RoutedEventArgs e)
            {

            }

            private void tilauslista_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {

            }

        







        /*
private void PaivitaComboBox(ComboBox kombo1, ComboBox kombo2)
{
   //tuotelista_cb.Items.Clear();

   SqlConnection yhteys = new SqlConnection(polku);
   yhteys.Open();

   SqlCommand komento = new SqlCommand("SELECT * FROM tuotteet", yhteys);
   SqlDataReader lukija = komento.ExecuteReader();

   DataTable taulu = new DataTable();
   taulu.Columns.Add("ID",  typeof(string));
   taulu.Columns.Add("NIMI", typeof(string));
   */
        /* tehdään sidokset että comboboxissa näytetää datataulua*/
        /*
        kombo1.ItemsSource = taulu.DefaultView;
        kombo1.DisplayMemberPath = "NIMI";
        kombo1.SelectedValuePath = "ID";

        kombo2.ItemsSource = taulu.DefaultView;
        kombo2.DisplayMemberPath = "NIMI";
        kombo2.SelectedValuePath = "ID";

        while (lukija.Read()) // käsitellään kyselytulos rivi riviltä
        {
            int id = lukija.GetInt32(0); 
            string nimi = lukija.GetString(1);
            taulu.Rows.Add(id, nimi); // lisätään datatauluun rivi tietoineen
            //tuotelista_cb.Items.Add(lukija.GetString(1));
        }
        lukija.Close();

        yhteys.Close();
    }


    private void PaivitaAsiakasComboBox()
    {
        //tuotelista_cb.Items.Clear();

        SqlConnection yhteys = new SqlConnection(polku);
        yhteys.Open();

        SqlCommand komento = new SqlCommand("SELECT * FROM asiakkaat", yhteys);
        SqlDataReader lukija = komento.ExecuteReader();

        DataTable taulu = new DataTable();
        taulu.Columns.Add("ID", typeof(string));
        taulu.Columns.Add("NIMI", typeof(string));


        /* tehdään sidokset että comboboxissa näytetää datataulua*/
        /*
        asiakaslista_cb.ItemsSource = taulu.DefaultView;
        asiakaslista_cb.DisplayMemberPath = "NIMI";
        asiakaslista_cb.SelectedValuePath = "ID";

        while (lukija.Read()) // käsitellään kyselytulos rivi riviltä
        {
            int id = lukija.GetInt32(0);
            string nimi = lukija.GetString(1);
            taulu.Rows.Add(id, nimi); // lisätään datatauluun rivi tietoineen
            //tuotelista_cb.Items.Add(lukija.GetString(1));
        }
        lukija.Close();

        yhteys.Close();

    }


    /*
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        PaivitaDataGrid("SELECT * FROM tuotteet", "tuotteet", tuotelista);
        PaivitaComboBox(tuotelista_cb, tuotelista_cb_2);

    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        SqlConnection yhteys = new SqlConnection(polku);
        yhteys.Open();

        string kysely = "INSERT INTO tuotteet (nimi, hinta) VALUES ('" + tuotenimi.Text + "'," + tuotehinta.Text + ");";
        SqlCommand komento = new SqlCommand(kysely, yhteys);
        komento.ExecuteNonQuery();

        yhteys.Close();

        PaivitaDataGrid("SELECT * FROM tuotteet", "tuotteet", tuotelista);
        PaivitaComboBox(tuotelista_cb, tuotelista_cb_2);
    }
    */
        /*
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string id = tuotelista_cb.SelectedValue.ToString();
            string kysely = "DELETE FROM tuotteet WHERE id='" + id + "';";
            SqlCommand komento = new SqlCommand(kysely, yhteys);
            komento.ExecuteNonQuery();
            yhteys.Close();

            PaivitaDataGrid("SELECT * FROM tuotteet", "tuotteet", tuotelista);
            PaivitaComboBox(tuotelista_cb, tuotelista_cb_2);
        }
        */
        /*
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string kysely = "INSERT INTO asiakkaat (nimi, osoite, puhelin) VALUES ('" + asiakasnimi.Text + "','" + asiakasosoite.Text + "','" + asiakaspuhelin.Text + "');";
            SqlCommand komento = new SqlCommand(kysely, yhteys);
            komento.ExecuteNonQuery();

            yhteys.Close();

            PaivitaDataGrid("SELECT * FROM asiakkaat", "asiakkaat", asiakaslista);
            PaivitaAsiakasComboBox();
        }
        */
        /*
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string asiakasID = asiakaslista_cb.SelectedValue.ToString();
            string tuoteID = tuotelista_cb_2.SelectedValue.ToString();

            string sql = "INSERT INTO tilaukset (asiakas_id, tuote_id) VALUES ('" + asiakasID + "','" + tuoteID + "')";

            SqlCommand komento = new SqlCommand(sql, yhteys);
            komento.ExecuteNonQuery();

            yhteys.Close();

            PaivitaDataGrid("SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu WHERE a.id=ti.asiakas_id AND tu.id=ti.tuote_id AND ti.toimitettu='0'", "tilaukset", tilauslista);
        }
        */
        /*
        private void toimita_tilaus_Click(object sender, RoutedEventArgs e)
        {
            DataRowView rivinakyma = (DataRowView)((Button)e.Source).DataContext;
            String tilaus_id = rivinakyma[0].ToString();

            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string sql = "UPDATE tilaukset SET toimitettu=1 WHERE id='" + tilaus_id + "';";

            SqlCommand komento = new SqlCommand(sql, yhteys);
            komento.ExecuteNonQuery();

            yhteys.Close();

            PaivitaDataGrid("SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu WHERE a.id=ti.asiakas_id AND tu.id=ti.tuote_id AND ti.toimitettu='0'", "tilaukset", tilauslista);
            PaivitaDataGrid("SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu WHERE a.id=ti.asiakas_id AND tu.id=ti.tuote_id AND ti.toimitettu='1'", "tilaukset", toimitetutlista);
        }*/

    }
    }