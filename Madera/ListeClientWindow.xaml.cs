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
    /// Logique d'interaction pour ListeClientWindow.xaml
    /// </summary>
    public partial class ListeClientWindow : Window
    {
        private Client[] _clients { get; set; }

        public ListeClientWindow(Client[] clients)
        {
            this._clients = clients;
            InitializeComponent();

            lvUsers.ItemsSource = _clients;
        }

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menu = new MenuWindow();
            App.Current.MainWindow = menu;
            Close();
            menu.Show();
        }
        private void listViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;
            Client obj = (Client)item.Content;
            new Details_Update_Client(obj);
        }
    }
}
