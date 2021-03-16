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
        private Client[] _clients { get; set; }

        public AppointmentWindow(DateTime date, Client[] clients)
        {
            InitializeComponent();
            this._clients = clients;

            lblDate.Content = date.ToLongDateString();
            comboBox.ItemsSource = _clients;
            _date = date;
            listBox.ItemsSource = Appointment.GetAppointmentDay(_date).Result;
        }

        private void btnValider_Click(object sender, RoutedEventArgs e)
        {
            _date = _date.Add(((DateTime)arrivalTimePicker.SelectedTime).TimeOfDay);
            var client = comboBox.SelectedItem.ToString();
            var titre = textBox.Text;
            var appointment = new Appointment() { name = titre, date = _date, client = client };
            Appointment.PostAppointment(appointment);
            MessageBox.Show(_date.ToString() + " " + client + " " + titre);
            Close();
        }

        private void listBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Voulez-vous supprimer ce rendez-vous", "Comfirmation de suppression", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Appointment.DeleteAppointment((listBox.SelectedItem as Appointment)._id);
            }
            Close();
        }


    }
}
