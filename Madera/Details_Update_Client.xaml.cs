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
    /// Logique d'interaction pour Details_Update_Client.xaml
    /// </summary>
    public partial class Details_Update_Client : Window
    {
        public Details_Update_Client(Client obj)
        {
            InitializeComponent();
            SetValue(obj);
            this.Show();
        }

        public void SetValue(Client obj)
        {
           Detail_client_value_name.Text = obj.first_name;
            Detail_client_value_phone.Text = obj.phone;
            Detail_client_value_mail.Text = obj.mail;
            Detail_client_value_address.Text = obj.address;
            Detail_client_value_postal_code.Text = obj.postal_code;
            Detail_client_value_city.Text = obj.city;
            Detail_client_value_country.Text = obj.country;
        }

        private void ClickQuit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ClickModify(object sender, RoutedEventArgs e)
        {
            //TODO
        }
    }
}
