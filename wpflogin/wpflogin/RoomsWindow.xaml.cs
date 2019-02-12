using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace wpflogin
{
    /// <summary>
    /// Interaction logic for RoomsWindow.xaml
    /// </summary>
    public partial class RoomsWindow : Window
    {
        string[,] tablica = new string[9,100];
         string clientNameString;
         string clientSecondNameString;
         string clientPhoneString;
         string clientEmailString;
        public string s;
        public string a = "";

        public string ClientNameString
        {
            get => clientNameString;
            
        }
     
        public string ClientSecondNameString
        {
            get => clientSecondNameString;
            
        }
        public string ClientPhnoneString
        {
            get => clientPhoneString;
            
        }
        public string ClientEmailString
        {
            get => clientEmailString;
            
        }


        private void goBack()
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
        public RoomsWindow()
        {
            InitializeComponent();
        }

        private void BookBtn_OnClick(object sender, RoutedEventArgs e)
        {
            string[] dataArray = new string[4] { clientNameString, clientSecondNameString, clientPhoneString, clientEmailString };
            string MSDEconn = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            //Dodaje Danę do Ostatniego zamówienia
            string query = "UPDATE zamowienia SET Imie = @Imie, Nazwisko = @Nazwisko, TELEFON = @TELEFON,Email=@Email WHERE Imie = @Tajemnica";
            SqlConnection connection = new SqlConnection(MSDEconn);          
            SqlCommand command = new SqlCommand(query, connection);
 
            connection.Open();
           
            command.Parameters.AddWithValue("@Tajemnica", a);
            command.Parameters.AddWithValue("@Imie", dataArray[0]);
            command.Parameters.AddWithValue("@Nazwisko", dataArray[1]);
            command.Parameters.AddWithValue("@TELEFON", dataArray[2]);
            command.Parameters.AddWithValue("@Email", dataArray[3]);


            command.ExecuteNonQuery();
           
            connection.Close();
            /*
            string MSDEconn1 = (@"Data source=FILIP-PC\SQLEXPRESS; Initial Catalog=filip_database; Integrated Security=True;");
            string query1 = "SELECT * FROM pokoje ";
            SqlConnection connection1 = new SqlConnection(MSDEconn1);
            connection1.Open();
            SqlCommand command1 = new SqlCommand(query1, connection1);
            
            SqlDataReader thisReader = command1.ExecuteReader();
            
            while (thisReader.Read())
            {

                s+=(thisReader["Number_pokoju"].ToString() + " " + thisReader["Pietro"].ToString()
                 + " " + thisReader["Liczba_osob"] + " " + thisReader["Budzenie"] + " " + thisReader["Lodowka"]
                 + " " + thisReader["Sejf"] + " " + thisReader["Lozko_dzieciece"] + " " + thisReader["Ekspres_do_kawy"]
                 + " " + thisReader["Sniadanie_do_lozka"]);
            }
             
            thisReader.Close();
            connection1.Close();

    */



          
    
            MessageBox.Show("Zarezerwowano.");
            goBack();
        }

        private void GoBackBtn_OnClick(object sender, RoutedEventArgs e)
        {  
            goBack();
        }
        //sprawdzanie poprawnosci wprowadzonych danych oraz zapisywanie ich do zmiennych publicznych
        
        private void ClientNameTxt_TextChanged(object sender, TextChangedEventArgs e)
        {

            clientNameString = clientNameTxt.Text;
            if (clientNameString.Length > 50) throw new IndexOutOfRangeException("Imie nie moze przekraczac 50 znakow");   
        }

        private void ClientPhoneTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
             clientPhoneString = clientPhoneTxt.Text;
            double j;
            if (Double.TryParse(clientPhoneString, out j))
                clientPhoneString = j.ToString();
            else
                MessageBox.Show("Niepoprawny format wprowadzonych danych");
            
        }

        private void ClientSecondNameTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            clientSecondNameString = clientSecondNameTxt.Text;
            if (clientSecondNameString.Length > 50) throw new IndexOutOfRangeException("Nazwisko nie moze przekraczac 50 znakow");
        }

        private void ClientEmailTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
             clientEmailString = clientEmailTxt.Text;
            if (clientEmailString.Length > 50) throw new IndexOutOfRangeException("Email nie moze przekraczac 50 znakow");
        }
        
    }
}
