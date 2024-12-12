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
        string polku = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\k2101834\\Documents\\testitietokanta.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True";
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
                

                PaivitaComboBoxKaikkihuollot(autokohtainenlista_cb);

                PaivitaComboBox(omistajalista_cb);

                PaivitaComboBox(omistajalistahaku_cb);

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

        // Tässä lisätään uusi auto datagridiin merkin, mallin ja rekisterinumeron  perusteella. 
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
            PaivitaComboBox(autokohtainenlista_cb);
            PaivitaComboBox(omistajalista_cb);

            // Tämä tyhjentää kentät niiden tietojen lähettämisen jälkeen.
            autonmerkki.Text = "";
            autonmalli.Text = "";
            autonrekno.Text = "";

        }


        // Päivittää comboboxin.
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


        // Mahdollistaa auton poistamisen järjestelmästä rekisterinumeron avulla.
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
            PaivitaDataGrid("SELECT * FROM huoltokuvat", "huoltokuvat", huoltokuvalista);
            PaivitaComboBox(autokohtainenlista_cb);
            PaivitaComboBox(omistajalista_cb);
            PaivitaDataGrid("SELECT * FROM omistaja", "omistaja", omistajalista);
            PaivitaComboBox(omistajalistahaku_cb);
            PaivitaComboBox(huoltolista_cb);
        }

        
        // Voidaan lisätä huoltotapahtuma huoltotyyppi, kilometrit, huollon päivämäärä ja auton rekisterinumero. 
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
            PaivitaDataGrid("SELECT * FROM huoltokuvat", "huoltokuvat", huoltokuvalista);

            // Tämä tyhjentää kentät niiden tietojen lähettämisen jälkeen.
            huoltotyyppi.Text = "";
            kilometrit.Text = "";
            paivamaara.Text = "";
        }


        /*
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
        }*/

        
        // Mahdollistaa kuvan tuomisen tietokoneelta.
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


        // Lisätään huoltokuitti kuva, kun tiedetään auton rekisterinumero ja huoltotapahtuman päivämäärä.
        private void lisaahuoltokuva(object sender, RoutedEventArgs e)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();


            int huolto_ID = 0;
            string rek = rekisterinumero_t.Text;
            string pvm = paivamaara_t.Text;

            string huolto_ID_kysely = "SELECT huolto_id from huollot where rekisteri_nro = '"+rek+"' and paivamaara = '"+pvm+"'";
            SqlCommand komento = new SqlCommand(huolto_ID_kysely, yhteys);
            SqlDataReader lukija = komento.ExecuteReader();

            while (lukija.Read())
            {
                huolto_ID = lukija.GetInt32(0);
            }
            lukija.Close();


            string kysely = "INSERT INTO huoltokuvat (huolto_id, kuva_nimi, kuva) VALUES ('" + huolto_ID + "','" + kuvanimi.Text + "','"+ kuvaPolku.Text + "');";
            komento = new SqlCommand(kysely, yhteys);
            komento.ExecuteNonQuery();
            yhteys.Close();

            PaivitaDataGrid("SELECT * FROM huoltokuvat", "huoltokuvat", huoltokuvalista);

            // Tämä tyhjentää kentät niiden tietojen lähettämisen jälkeen.
            kuvanimi.Text = "";
            kuvaPolku.Text = "";
            rekisterinumero_t.Text = "";
            paivamaara_t.Text = "";
        }

 

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = e.Uri.AbsoluteUri,  
                UseShellExecute = true  
            });
        }


        // Päivittää comboboxin.
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


        // Tämä mahdollistaa, että rekisterinumerolla pystyy hakemaan kaikki autoon merkityt huoltotapahtumat.
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


        //Mahdollistaa omistajan lisäämisen autolle. Lisätään nimi, puhelinnumero ja osoite.
        private void omistajanlisäys(object sender, RoutedEventArgs e)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string id = omistajalista_cb.SelectedValue.ToString();

            string kysely = "INSERT INTO omistaja (rekisteri_nro, nimi, puhelinnumero, osoite) VALUES ('" + id + "','" + Nimi.Text + "','" + Puh_numero.Text + "','" + Osoite.Text + "');";
            SqlCommand komento = new SqlCommand(kysely, yhteys);
            komento.ExecuteNonQuery();
            yhteys.Close();

            PaivitaDataGrid("SELECT * FROM omistaja", "omistaja", omistajalista);
            PaivitaComboBox(omistajalista_cb);
            PaivitaComboBox(omistajalistahaku_cb);

            // Tämä tyhjentää kentät niiden tietojen lähettämisen jälkeen.
            Nimi.Text = "";
            Puh_numero.Text = "";
            Osoite.Text = "";
        }


        // Voi hakea omistajan auton rekisterinumeron perusteella.
        private void omistajanhaku(object sender, RoutedEventArgs e)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string id = omistajalistahaku_cb.SelectedValue.ToString();


            PaivitaDataGrid("SELECT nimi, puhelinnumero, osoite from omistaja where rekisteri_nro='"+id+"' ", "omistaja", omistajalista);
            
        }


        // Voit poistaa omistajan nimen ja puhelinnumeron perusteella. 
        private void poistoomistajanhaku(object sender, RoutedEventArgs e)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string id = Nimipoisto.Text;
            string id2 = Puhelinpoisto.Text;

            PaivitaDataGrid("Delete from omistaja where nimi='"+id+"' and puhelinnumero='"+id2+"'", "omistaja", omistajalista);
            MessageBox.Show("Omistaja " + id + " poistettu.");

            // Tämä tyhjentää kentät niiden tietojen lähettämisen jälkeen.
            Nimipoisto.Text = "";
            Puhelinpoisto.Text = "";
            Nimi.Text = "";        
            Puh_numero.Text = "";  
            Osoite.Text = "";
        }






        /*

        private void Button_Click_3(object sender, RoutedEventArgs e)
            {

            }

            private void Button_Click_4(object sender, RoutedEventArgs e)
            {

            }

            private void tilauslista_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {

            }*/

       

    }
    }