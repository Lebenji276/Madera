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
    /// Logique d'interaction pour ListeComposantWindow.xaml
    /// </summary>
    public partial class ListeComposantWindow : Window
    {
        private Composant[] _composants { get; set; }
        public ListeComposantWindow(Composant[] composants)
        {
            this._composants = composants;
            InitializeComponent();

            lvComposant.ItemsSource = _composants;
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
            Composant obj = (Composant)item.Content;
            new Details_Update_Composant(obj);
        }
    }
}
