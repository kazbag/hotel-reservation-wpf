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
        public DateTime SelectedDayFrom { get; set; }
        public DateTime SelectedDayTo { get; set; }
        //public TimeSpan czasRezerwacji;
        public string s1 = "";
        public string s2 = "";
        public string s3 = "";
        public static string ostatecznyNrPokoju = "";

        public static string ClientOstatecznyNrPokoju
        {
            get => ostatecznyNrPokoju;
            set => ostatecznyNrPokoju = value;
        }

        public static int cenaPokoju = 100;

        public static int ClientCenaPokoju
        {
            get => cenaPokoju;
            set => cenaPokoju = value;
        }

        public MainWindow()
        {
            InitializeComponent();
            this.InitializeComponent();
            this.DataContext = this;
        }

        private void goBack()
        {
            RoomsWindow rooms = new RoomsWindow();
            rooms.Show();
            this.Close();
        }

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


            if (selectedDayFromString == "")
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

                int currentValue = (Convert.ToInt16(currentValueFromAdultsCombobox));
                int currentValue2 = currentValue + 1;

                int childrenValue = (Convert.ToInt16(currentValueFromChildrenCombobox));

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
                string[] checkboxArray = new string[9] { selectedDayToString, SelectedDayTo.ToString(), currentValue2.ToString(),
                    wakeUpCheckbox.ToString(), fridgeCheckbox.ToString(),safeCheckBox.ToString(),
                    childBedCheckBox.ToString(), coffeeMachineCheckBox.ToString(), breakfastToBedCheckBox.ToString() };

                //Wskazuje cenę zamówienia

                TimeSpan czasRezerwacji = SelectedDayTo - SelectedDayFrom;
                int dniRezerwacji = czasRezerwacji.Days;

                cenaPokoju = cenaPokoju * currentValue2;

                if (wakeUpCheckbox)
                {
                    cenaPokoju += 20;
                }
                if (fridgeCheckbox)
                {
                    cenaPokoju += 20;
                }
                if (safeCheckBox)
                {
                    cenaPokoju += 20;
                }
                if (childBedCheckBox && childrenValue != 0)
                {
                    cenaPokoju += 20 * childrenValue;
                }
                if (coffeeMachineCheckBox)
                {
                    cenaPokoju += 30;
                }
                if (breakfastToBedCheckBox)
                {
                    cenaPokoju = (currentValue2 * 50) + cenaPokoju;
                }
                cenaPokoju = cenaPokoju * dniRezerwacji;

                // wyswietlenie komunikatu potwierdzanjacego wybrane opcje
                MessageBox.Show(
                    $" Ilość osób: {currentValue2}" +
                    $"\r\n Ilość dzieci do lat 3: {childrenValue}" +
                    $"\r\n Początek rezerwacji: { DateTime.Parse(selectedDayFromString).ToString("dd.MM.yyyy")}" +
                    $"\r\n Koniec rezerwacji: {DateTime.Parse(selectedDayToString).ToString("dd.MM.yyyy")}" +
                    $"\r\n ilosc dni: {dniRezerwacji}" +
                    $"\r\n Pobudka: {MainWindow.CheckBoxNamesConverter(wakeUpCheckbox)}" +
                    $"\r\n Lodówka: {MainWindow.CheckBoxNamesConverter(fridgeCheckbox)}" +
                    $"\r\n Sejf: {MainWindow.CheckBoxNamesConverter(safeCheckBox)}" +
                    $"\r\n Łóżeczko dla dziecka: {MainWindow.CheckBoxNamesConverter(childBedCheckBox)} x{childrenValue}" +
                    $"\r\n Ekspres do kawy: {MainWindow.CheckBoxNamesConverter(coffeeMachineCheckBox)}" +
                    $"\r\n Śniadanie do łóżka: {MainWindow.CheckBoxNamesConverter(breakfastToBedCheckBox)}" +
                    $"\r\n Cena zamówienia: {cenaPokoju} zł"
                );

                string MSDEconn5 = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
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
                    if (s1 != "")
                        s1 += ',';

                    s1 += (thisreader1["idPokoje"].ToString());
                }

                thisreader1.Close();
                connection5.Close();
                string[] tablicaIdPokoje = s1.Split(',');
                int iloscZnalezionychPokoi = tablicaIdPokoje.Length;

                if (tablicaIdPokoje[0] != "")
                {
                    //Sprawdza dostępnosć pokoi w wybranym terminie za pomocą funkcji
                    int i = 0;
                    while (i < iloscZnalezionychPokoi)
                    {
                        //Pierwsze zapytanie SQL potrzebne do ustalenia czy pokój jest wolny        <-----
                        string query21 = "SELECT* FROM zamowienia WHERE idPokoje = @room" +
                                         " AND ReservedSince <= @dateFrom" +
                                         " AND @dateFrom < ReservedTo";

                        string conn10 = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
                        SqlConnection connection10 = new SqlConnection(conn10);
                        SqlCommand command10 = new SqlCommand(query21, connection10);
                        connection10.Open();

                        command10.Parameters.AddWithValue("@dateFrom", SelectedDayFrom);
                        command10.Parameters.AddWithValue("@room", tablicaIdPokoje[i]);

                        SqlDataReader thisreader10 = command10.ExecuteReader();
                        while (thisreader10.Read())
                        {
                            s2 += (thisreader10["idZamowienia"].ToString());
                        }

                        thisreader10.Close();
                        connection10.Close();

                        //Drugie zapytanie SQL potrzebne do ustalenia czy pokój jest wolny      <-----
                        string query22 = "SELECT * FROM zamowienia WHERE idPokoje = @room" +
                                         " AND ReservedSince < @dateTo" +
                                         " AND ReservedTo > @dateTo";

                        string conn11 = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
                        SqlConnection connection11 = new SqlConnection(conn11);
                        SqlCommand command11 = new SqlCommand(query22, connection11);
                        connection11.Open();

                        command11.Parameters.AddWithValue("@dateTo", SelectedDayTo);
                        command11.Parameters.AddWithValue("@room", tablicaIdPokoje[i]);

                        SqlDataReader thisreader11 = command11.ExecuteReader();
                        while (thisreader11.Read())
                        {

                            s3 += (thisreader11["idZamowienia"].ToString());
                        }

                        thisreader11.Close();
                        connection11.Close();

                        if (s2 != "" || s3 != "")
                        {
                            i++;
                        }
                        else
                        {
                            ostatecznyNrPokoju = tablicaIdPokoje[i];
                            break;
                        }
                    }
                }

                if (ostatecznyNrPokoju == "")
                {
                    //  Poniższy MessengeBox zawiera drobne "Oszustwo" ponieważ dostępność została już sprawdzona. Wydaję mi się jednak że jest
                    //  ono przydatne żeby poinformować użytkownika o tym czy wgl mamy pokój spełniający jego wymagania.

                    MessageBox.Show("Ilość pokoi z wybranymi przez ciebie dodatkami: " + iloscZnalezionychPokoi +
                                    $"\rKliknij OK żeby sprawdzić ich dostępność w wybranym przez ciebie terminie terminie");
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                    MessageBox.Show("Brak dostępnych pokoi.\r Proszę wybrać inne Dodatki lub zmienić datę rezerwacji");
                    cenaPokoju = 100;
                }
                else
                {
                    //  Poniższy MessengeBox także zawiera oszustwo. Tym razem jednak spowodowane chęcią utrzymania spujności interfejsu

                    MessageBox.Show("Dostępne pokoje: " + iloscZnalezionychPokoi + "\r Kliknij OK żeby sprawdzić dostępność w terminie");
                    MessageBox.Show("Pokój Dostępny. \rNumer pokoju: " + ostatecznyNrPokoju);
                }

                //"Usuwa" wszystkie błędne zamówienia

                string MSDEconn = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
                string query3 = "UPDATE zamowienia SET Imie = @Imie, Nazwisko = @Nazwisko, TELEFON = @TELEFON,Email=@Email WHERE Imie = @Tajemnica";
                SqlConnection connection3 = new SqlConnection(MSDEconn);
                SqlCommand command3 = new SqlCommand(query3, connection3);

                connection3.Open();

                command3.Parameters.AddWithValue("@Tajemnica", "");
                command3.Parameters.AddWithValue("@Imie", "X");
                command3.Parameters.AddWithValue("@Nazwisko", "X");
                command3.Parameters.AddWithValue("@TELEFON", "X");
                command3.Parameters.AddWithValue("@Email", "X");

                command3.ExecuteNonQuery();
                connection3.Close();


                if (ostatecznyNrPokoju != "")
                {
                    string conn = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

                    string query = "INSERT INTO zamowienia VALUES ( @idPokoje,@Imie,@Nazwisko,@TELEFON,@Email, @ReservedSince, @ReservedTo, @PeopleAmount, @WakeUp, @Fridge," +
                        "@Safe, @ChildBed, @CoffeeMachine, @BreakfastToBed, @Cena)";
                    SqlConnection connection = new SqlConnection(conn);
                    SqlCommand command = new SqlCommand(query, connection);

                    connection.Open();

                    command.Parameters.AddWithValue("@idPokoje", ostatecznyNrPokoju);
                    command.Parameters.AddWithValue("@ReservedSince", SelectedDayFrom);
                    command.Parameters.AddWithValue("@ReservedTo", SelectedDayTo);
                    command.Parameters.AddWithValue("@PeopleAmount", checkboxArray[2]);
                    command.Parameters.AddWithValue("@WakeUp", checkboxArray[3]);
                    command.Parameters.AddWithValue("@Fridge", checkboxArray[4]);
                    command.Parameters.AddWithValue("@Safe", checkboxArray[5]);
                    command.Parameters.AddWithValue("@ChildBed", checkboxArray[6]);
                    command.Parameters.AddWithValue("@CoffeeMachine", checkboxArray[7]);
                    command.Parameters.AddWithValue("@BreakfastToBed", checkboxArray[8]);
                    command.Parameters.AddWithValue("@Cena", cenaPokoju);
                    command.Parameters.AddWithValue("@Imie", "");
                    command.Parameters.AddWithValue("@Nazwisko", "");
                    command.Parameters.AddWithValue("@TELEFON", "");
                    command.Parameters.AddWithValue("@Email", "");

                    command.ExecuteNonQuery();

                    connection.Close();

                    goBack();
                }
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
    }
}

