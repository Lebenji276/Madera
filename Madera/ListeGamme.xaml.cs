using Madera.Classe;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Logique d'interaction pour ListeGamme.xaml
    /// </summary>
    public partial class ListeGamme : Window
    {
        Module[] _listmodules;
        public ListeGamme(Module[] listmodules)
        {
            _listmodules = listmodules;
            InitializeComponent();
            var listeGammes = Gamme.GetAllGammes().Result;
            var listModuleAdd = new List<Module>();
            foreach (var gamme in listeGammes)
            {
                listModuleAdd = new List<Module>();
                foreach (var module in listmodules)
                {
                    if (gamme.modules.Contains(module._id))
                    {
                        gamme.listmodulesString += module.nomModule +", ";
                        listModuleAdd.Add(module);
                    }
                    gamme.listmodules = listModuleAdd;
                }
            }
            lvUsers.ItemsSource = listeGammes;
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
            Gamme obj = (Gamme)item.Content;
            new Details_Update_Gamme(obj, _listmodules);
        }
    }
}
