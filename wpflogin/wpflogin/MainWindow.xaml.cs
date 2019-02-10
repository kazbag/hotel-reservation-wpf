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

namespace wpflogin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DateTime? SelectedDayFrom { get; set; }
        public DateTime? SelectedDayTo { get; set; }

       

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

                if (Convert.ToByte(adultsCombobox.SelectedIndex.ToString()) == 0 
                && Convert.ToByte(childrenCombobox.SelectedIndex.ToString()) == 0)
                {
                MessageBox.Show("Nie możesz zarezerwować pokoju 0 osobowego.");
                }
                else
                {
            string currentValueFromAdultsCombobox = adultsCombobox.SelectedIndex.ToString();
            string currentValueFromChildrenCombobox = childrenCombobox.SelectedIndex.ToString();

            int currentValue = (Convert.ToInt16(currentValueFromAdultsCombobox) + 
                    Convert.ToInt16(currentValueFromChildrenCombobox));
                // tylko wyświetla liczbę osób, sprawdzenie czy jest ok. można wyrzucić.

                //przypisywanie wartosci checkboxow do zmiennych
            string selectedDayFromString = SelectedDayFrom.ToString();
            string selectedDayToString = SelectedDayTo.ToString();
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
                string[] checkboxArray = new string[9] { selectedDayFromString, selectedDayToString, currentValue.ToString(),
                    wakeUpCheckbox.ToString(), fridgeCheckbox.ToString(),safeCheckBox.ToString(),
                    childBedCheckBox.ToString(), coffeeMachineCheckBox.ToString(), breakfastToBed.ToString() };
             
                // wyswietlenie komunikatu potwierdzanjacego wybrane opcje
                MessageBox.Show($" ilość osób: {currentValue}\r\n początek rezerwacji: {selectedDayFromString}\r\n " +
                    $"koniec rezerwacji: {selectedDayToString}\r\n pobudka: {wakeUpCheckbox}\r\n " +
                    $"lodówka: {fridgeCheckbox}\r\n sejf: {safeCheckBox}\r\n łóżeczko dla dziecka: {childBedCheckBox}\r\n " +
                    $"ekspres do kawy: {coffeeMachineCheckBox}\r\n śniadanie do łóżka: {breakfastToBedCheckBox}");
                goBack();
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

