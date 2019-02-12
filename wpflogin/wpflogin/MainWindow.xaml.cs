using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Drawing;
using System.Configuration;

namespace wpflogin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DateTime? SelectedDayFrom { get; set; }
        public DateTime? SelectedDayTo { get; set; }
        public string s1;


        public MainWindow()
        {
            InitializeComponent();
            this.InitializeComponent();
            this.DataContext = this;
        }

        //
        private void goBack()
        {
            RoomsWindow rooms = new RoomsWindow();
            rooms.Show();
            this.Close();
        }
        //

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Run the program again
                Process.Start(Application.ResourceAssembly.Location);

                // Close the Old one
                Process.GetCurrentProcess().Kill();
            }
            catch
            { }
        }

        // zlicza liczbę osób i zrzuca ją do zmiennej currentValue
        private void BtnCheck_OnClick(object sender, RoutedEventArgs e)
        {

            //przypisuje wartości z daty do zmniennych

            string selectedDayFromString = SelectedDayFrom.ToString();
            string selectedDayToString = SelectedDayTo.ToString();

            //sprawdza logiczność wyborów

            if (Convert.ToByte(adultsCombobox.SelectedIndex.ToString()) == 0
                && Convert.ToByte(childrenCombobox.SelectedIndex.ToString()) == 0)
            {
                MessageBox.Show("Błąd: Nie możesz zarezerwować pokoju 0 osobowego.");
            }
            else if (selectedDayFromString == "")
            {
                MessageBox.Show("Błąd: Nie wybrano daty początku rezerwacji");
            }
            else if (selectedDayToString == "")
            {
                MessageBox.Show("Błąd: Nie wybrano daty końca rezerwacji");
            }
            else if (selectedDayToString == selectedDayFromString)
            {
                MessageBox.Show("Błąd: Nie możesz zarezerwować pokoju na 0 dni");
            }

            else if (SelectedDayTo < SelectedDayFrom)
            {
                MessageBox.Show("Błąd: Data końca rezerwacji jest wcześniej niż początku rezerwacji");
            }

            else if (SelectedDayFrom < DateTime.Today)
            {
                MessageBox.Show("Błąd: Nie możesz zarezerwować przeszłości");
            }

            else
            {
                string currentValueFromAdultsCombobox = adultsCombobox.SelectedIndex.ToString();
                string currentValueFromChildrenCombobox = childrenCombobox.SelectedIndex.ToString();

                int currentValue = (Convert.ToInt16(currentValueFromAdultsCombobox) +
                        Convert.ToInt16(currentValueFromChildrenCombobox));
                // tylko wyświetla liczbę osób, sprawdzenie czy jest ok. można wyrzucić.

                //przypisywanie wartosci checkboxow do zmiennych

                bool wakeUpCheckbox, fridgeCheckbox, safeCheckBox, childBedCheckBox,
                     coffeeMachineCheckBox, breakfastToBedCheckBox;
                if (wakeUp.IsChecked == true) wakeUpCheckbox = true;
                else wakeUpCheckbox = false;
                if (fridge.IsChecked == true) fridgeCheckbox = true;
                else fridgeCheckbox = false;
                if (safe.IsChecked == true) safeCheckBox = true;
                else safeCheckBox = false;
                if (childBed.IsChecked == true) childBedCheckBox = true;
                else childBedCheckBox = false;
                if (coffeeMachine.IsChecked == true) coffeeMachineCheckBox = true;
                else coffeeMachineCheckBox = false;
                if (breakfastToBed.IsChecked == true) breakfastToBedCheckBox = true;
                else breakfastToBedCheckBox = false;
                // zapisanie wartosci checkboxow w tablicy
                string[] checkboxArray = new string[9] { selectedDayToString, SelectedDayTo.ToString(), currentValue.ToString(),
                    wakeUpCheckbox.ToString(), fridgeCheckbox.ToString(),safeCheckBox.ToString(),
                    childBedCheckBox.ToString(), coffeeMachineCheckBox.ToString(), breakfastToBedCheckBox.ToString() };

                // wyswietlenie komunikatu potwierdzanjacego wybrane opcje
                MessageBox.Show(
                    $" Ilość osób: {currentValue}" +
                    $"\r\n Początek rezerwacji: {selectedDayFromString}" +
                    $"\r\n Koniec rezerwacji: {selectedDayToString}" +
                    $"\r\n Pobudka: {MainWindow.CheckBoxNamesConverter(wakeUpCheckbox)}" +
                    $"\r\n Lodówka: {MainWindow.CheckBoxNamesConverter(fridgeCheckbox)}" +
                    $"\r\n Sejf: {MainWindow.CheckBoxNamesConverter(safeCheckBox)}" +
                    $"\r\n Łóżeczko dla dziecka: {MainWindow.CheckBoxNamesConverter(childBedCheckBox)}" +
                    $"\r\n Ekspres do kawy: {MainWindow.CheckBoxNamesConverter(coffeeMachineCheckBox)}" +
                    $"\r\n Śniadanie do łóżka: {MainWindow.CheckBoxNamesConverter(breakfastToBedCheckBox)}"
                    );

                /*string MSDEconn5 = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
                string ABC = "SELECT idPokoje from pokoje where Liczba_osob >= @liczbaOsob AND Lodowka = @lodwka " +
                    "AND Sejf = @sejf AND Lozko_dzieciece = @lozeczkoDzieciece AND Ekspres_do_kawy = @ekspres " +
                    "AND Sniadanie_do_lozka = @sniadanie  AND Budzenie = @budzenie; ";
                SqlConnection connection5 = new SqlConnection(MSDEconn5);
                SqlCommand command5 = new SqlCommand(ABC, connection5);
                connection5.Open();
                command5.Parameters.AddWithValue("@budzenie", checkboxArray[3]);
                command5.Parameters.AddWithValue("@liczbaOsob", checkboxArray[2]);
                command5.Parameters.AddWithValue("@lodwka", checkboxArray[4]);
                command5.Parameters.AddWithValue("@sejf", checkboxArray[5]);
                command5.Parameters.AddWithValue("@lozeczkoDzieciece", checkboxArray[6]);
                command5.Parameters.AddWithValue("@ekspres", checkboxArray[7]);
                command5.Parameters.AddWithValue("@sniadanie", checkboxArray[8]);

                SqlDataReader thisreader1 = command5.ExecuteReader();
                while (thisreader1.Read())
                {

                    s1 += (thisreader1["idPokoje"].ToString());
                }

                thisreader1.Close();
                connection5.Close();
                MessageBox.Show(s1);*/


                //"Usuwa" wszystkie błędne zamówienia
                string MSDEconn = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
                string query3 = "UPDATE zamowienia SET Imie = @Imie, Nazwisko = @Nazwisko, TELEFON = @TELEFON,Email=@Email WHERE Imie = @Tajemnica";
                SqlConnection connection3 = new SqlConnection(MSDEconn);
                SqlCommand command3 = new SqlCommand(query3, connection3);

                connection3.Open();

                command3.Parameters.AddWithValue("@Tajemnica", "");
                command3.Parameters.AddWithValue("@Imie","X");
                command3.Parameters.AddWithValue("@Nazwisko", "X");
                command3.Parameters.AddWithValue("@TELEFON", "X");
                command3.Parameters.AddWithValue("@Email", "X");


                command3.ExecuteNonQuery();

                connection3.Close();


                string conn = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

                string query = "INSERT INTO zamowienia VALUES ( @idPokoje,@Imie,@Nazwisko,@TELEFON,@Email, @ReservedSince, @ReservedTo, @PeopleAmount, @WakeUp, @Fridge," +
                    "@Safe, @ChildBed, @CoffeeMachine, @BreakfastToBed)";
                SqlConnection connection = new SqlConnection(conn);
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();

                // trzeba ustawić autoinkrementację ID, jest ustawiony na sztywno i jeden jedyny raz się uda wstawić rekord, w innym przypadku wyrzuci błąd o duplikacie

                command.Parameters.AddWithValue("@idPokoje", 18);
                command.Parameters.AddWithValue("@ReservedSince", SelectedDayFrom);
                command.Parameters.AddWithValue("@ReservedTo", SelectedDayTo);
                command.Parameters.AddWithValue("@PeopleAmount", checkboxArray[2]);
                command.Parameters.AddWithValue("@WakeUp", checkboxArray[3]);
                command.Parameters.AddWithValue("@Fridge", checkboxArray[4]);
                command.Parameters.AddWithValue("@Safe", checkboxArray[5]);
                command.Parameters.AddWithValue("@ChildBed", checkboxArray[6]);
                command.Parameters.AddWithValue("@CoffeeMachine", checkboxArray[7]);
                command.Parameters.AddWithValue("@BreakfastToBed", checkboxArray[8]);
                command.Parameters.AddWithValue("@Imie", "");
                command.Parameters.AddWithValue("@Nazwisko", "");
                command.Parameters.AddWithValue("@TELEFON", "");
                command.Parameters.AddWithValue("@Email", "");

                command.ExecuteNonQuery();

                connection.Close();

                goBack();

            }


        }

        // Zamienia wartości boolowskie na stringi dla funkcji BtnCheck_OnClick

        private static string CheckBoxNamesConverter(bool TakCzyNie)
        {

            if (TakCzyNie == true)
            {
                return "Tak";
            }
            else
            {
                return "Nie";
            }

        }




        // todo
        // trzeba zrzucić do bazy -od i -do
        // BookFrom i BookTo wyświetlają messageboxa, którego finalnie nie będzie. Można powiedzieć, że są to funkcje "testujące"

        private void BookFrom(object sender, RoutedEventArgs e)
        {
            string msg;

            if (SelectedDayFrom == null)
                msg = "Nie rezerwuję";
            else if (SelectedDayFrom >= DateTime.Today)
                msg = string.Format("Rezerwuję {0:D}", SelectedDayFrom);
            else
                msg = "Nie możesz rezerwować przeszłości";

            MessageBox.Show(msg);
        }
        private void BookTo(object sender, RoutedEventArgs e)
        {
            string msg;

            if (SelectedDayTo == null)
                msg = "Nie wybrałeś dnia 'do'.";
            else if (SelectedDayTo > SelectedDayFrom)
                msg = string.Format("Zarezerwowano do {0:D}", SelectedDayTo);
            else
                msg = "Musisz zarezerwować pokój co najmniej na 1 dzień.";

            MessageBox.Show(msg);

        }


        // todo
        // checkboxy
        // sądzę że to może się przydać https://www.youtube.com/watch?v=VHSIAz-WVDA
        // trzeba zrobić pobieranie wartości z każdego checkboxa do zmiennej, a następnie do tablicy checkboxArray
        // i wysłać zapytanie do bazy o zwrócenie pokoju, lub podobnie

        // otworzenie połączenia z bazą, jeżeli nie wyrzuci żadnego błędu i wszystkie pola są wypełnione.

        // zapytanie - wszystkie zmienne (dataFrom, dataTo, peopleAmount (czyli suma adultsAmount + childrenAmount),

        // i każde poszczególne "extrasy". Można to będzie zrobić albo stringiem,
        // np string extrasValues = "000000" i później każdy poszczególny zaznaczony checkbox
        // by modyfikował(nadpisywał) poszczególny index "1" jeżeli true,
        // lub zostawiał "0" jeśli false, czyli np string by wyglądał później "010111" i  następnie splitował każdy index przecinkiem,
        // żeby powstało "0", "1", "0", "1", "1", "1"
        // jak wydaje się za bardzo pokręcone, można to pewnie rozkminić w inny sposób, ale taki wpadł mi do głowy
    }



}

