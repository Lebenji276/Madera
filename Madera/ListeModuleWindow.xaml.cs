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
    /// Logique d'interaction pour ListeModuleWindow.xaml
    /// </summary>
    public partial class ListeModuleWindow : Window
    {
        private Module[] _modules { get; set; }
        public ListeModuleWindow(Module[] modules)
        {
            this._modules = modules;
            InitializeComponent();

            lvModule.ItemsSource = modules;
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
            Module obj = (Module)item.Content;
            new Details_Update_Module(obj);
        }
    }
}
