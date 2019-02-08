using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
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


        private void BtnCheck_OnClick(object sender, RoutedEventArgs e)
        {
            if (Convert.ToByte(adultsCombobox.SelectedIndex.ToString()) == 0 && Convert.ToByte(childrenCombobox.SelectedIndex.ToString()) == 0)
            {
                MessageBox.Show("Nie możesz zarezerwować pokoju 0 osobowego.");
            }
            else
            {
            string currentValueFromAdultsCombobox = adultsCombobox.SelectedIndex.ToString();
            string currentValueFromChildrenCombobox = childrenCombobox.SelectedIndex.ToString();

            int currentValue = (Convert.ToInt16(currentValueFromAdultsCombobox) + Convert.ToInt16(currentValueFromChildrenCombobox));
            MessageBox.Show(currentValue + " osób");
            //RoomsWindow rooms = new RoomsWindow();
            //rooms.Show();
            //this.Close();
            goBack();
            }
            
        }

        private void BtnBook_OnClick(object sender, RoutedEventArgs e)
        {

            
        }
    }
}
