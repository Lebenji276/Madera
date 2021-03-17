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
    /// Logique d'interaction pour AppointmentWindow.xaml
    /// </summary>
    public partial class AppointmentWindow : Window
    {
        DateTime _date;

        public AppointmentWindow(DateTime date, Client[] clients, Appointment[] appointments)
        {
            InitializeComponent();
            lblDate.Content = date.ToLongDateString();
            comboBox.ItemsSource = clients;
            _date = date;
            listBox.ItemsSource = appointments;
        }

        private async void btnValider_Click(object sender, RoutedEventArgs e)
        {
            _date = _date.Add(((DateTime)arrivalTimePicker.SelectedTime).TimeOfDay);
            var client = comboBox.SelectedItem.ToString();
            var titre = textBox.Text;
            var appointment = new Appointment() { name = titre, date = _date, client = client };
            await Appointment.PostAppointment(appointment);
            Close();
        }

        private async void listBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Voulez-vous supprimer ce rendez-vous", "Comfirmation de suppression", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
               await Appointment.DeleteAppointment((listBox.SelectedItem as Appointment)._id);
            }
            Close();
        }


    }
}
