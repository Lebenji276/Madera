using Madera.Classe;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Madera
{
    /// <summary>
    /// Logique d'interaction pour AgendaWindow.xaml
    /// </summary>
    public partial class AgendaWindow : Window
    {

        public AgendaWindow()
        {
            InitializeComponent();
        }

        private async void DateChanged(object sender, RoutedEventArgs e)
        {
            var clients = await Client.GetAllClient();
            var appointmentDay = await Appointment.GetAppointmentDay((DateTime)(sender as Calendar).SelectedDate);

            AppointmentWindow appointment = new AppointmentWindow((DateTime)(sender as Calendar).SelectedDate, clients, appointmentDay);
            appointment.ShowDialog();
        }

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menu = new MenuWindow();
            App.Current.MainWindow = menu;
            Close();
            menu.Show();
        }
    }
}
