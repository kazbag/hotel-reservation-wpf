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

namespace wpflogin
{
    /// <summary>
    /// Interaction logic for RoomsWindow.xaml
    /// </summary>
    public partial class RoomsWindow : Window
    {
        public string clientNameString;
        public string clientPhnoneString;
        public string clientSecondNameString;
        public string clientEmailString;

 

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
            
            MessageBox.Show("Zarezerwowano.");
            string[] dateArray = new string[4] { clientNameString, clientPhnoneString, clientSecondNameString, clientEmailString };

            
            SqlConnection MSDEconn = new SqlConnection(@"Data source=FILIP-PC\SQLEXPRESS; Initial Catalog=filip_database; Integrated Security=True;");
            MSDEconn.Open();
            SqlCommand MSDEcommand = new SqlCommand();
            MSDEcommand.Connection = MSDEconn;

            MSDEcommand.CommandText = "INSERT INTO [zamowienia] ( [[Imie],[Nazwisko],[TELEFON],[Email]) " +
                "VALUES (1,dateArray[0],dateArray[2],dateArray[1],dateArray[3] )";
            MSDEcommand.ExecuteNonQuery();
            MSDEconn.Close();

          
            
           
            goBack();
        }

        private void GoBackBtn_OnClick(object sender, RoutedEventArgs e)
        {
            goBack();
            //MainWindow main = new MainWindow();
            //main.Show();
            //this.Close();

        }
        //sprawdzanie poprawnosci wprowadzonych danych oraz zapisywanie ich do zmiennych publicznych
        private void ClientNameTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
             clientNameString = clientNameTxt.Text;
            if (clientNameString.Length > 50) throw new IndexOutOfRangeException("Imie nie moze przekraczac 50 znakow");   
        }

        private void ClientPhoneTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
             clientPhnoneString = clientPhoneTxt.Text;
            double j;
            if (Double.TryParse(clientPhnoneString, out j))
                clientPhnoneString = j.ToString();
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
