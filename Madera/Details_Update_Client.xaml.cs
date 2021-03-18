using Madera.Classe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private string clientId {get;set;}
        private ListeClientWindow lscWindow { get; set; }

        private Client client;

        public Details_Update_Client(Client obj, ListeClientWindow lscWindow)
        {
            this.client = obj;
            this.clientId = obj._id;
            this.lscWindow = lscWindow;
            InitializeComponent();
            SetValue(obj);
            setDevisClient(obj._id);
            this.Show();
        }

        private void setDevisClient(string idClient)
        {
            var devis = Devis.getDevisByClientId(idClient);

            this.DataContext = new ViewModelClient(devis);
        }

        private void listBoxItem_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            new PdfViewer((Devis)Detail_module_value_composant.SelectedItem, client);
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
            var client = Client.GetClientById(clientId);

            var dt = DateTime.Now;
            dt = dt.AddHours(-1);

            client.first_name = Detail_client_value_name.Text;
            client.phone = Detail_client_value_phone.Text;
            client.mail = Detail_client_value_mail.Text;
            client.address = Detail_client_value_address.Text;
            client.postal_code = Detail_client_value_postal_code.Text;
            client.city = Detail_client_value_city.Text;
            client.country = Detail_client_value_country.Text;
            client.isSynchronised = false;
            client.updatedAt = dt;

            Client.updateClient(client);
            this.lscWindow.lvUsers.ItemsSource = Client.GetAllClient();

            this.Close();
        }

        private void ClickDelete(object sender, RoutedEventArgs e)
        {
            var client = Client.GetClientById(clientId);

            var dt = DateTime.Now;
            dt = dt.AddHours(-1);

            client.first_name = Detail_client_value_name.Text;
            client.phone = Detail_client_value_phone.Text;
            client.mail = Detail_client_value_mail.Text;
            client.address = Detail_client_value_address.Text;
            client.postal_code = Detail_client_value_postal_code.Text;
            client.city = Detail_client_value_city.Text;
            client.country = Detail_client_value_country.Text;
            client.isDeleted = true;
            client.updatedAt = dt;

            Client.updateClient(client);
            this.lscWindow.lvUsers.ItemsSource = Client.GetAllClient();

            this.Close();
        }
    }

    public class ViewModelClient
    {
        public ObservableCollection<Devis> CollecDevis { get; set; }

        public ViewModelClient(Devis[] Od)
        {
            this.CollecDevis = new ObservableCollection<Devis>();
            foreach (Devis dev in Od)
            {
                this.CollecDevis.Add(dev);
            }
        }
    }
}
