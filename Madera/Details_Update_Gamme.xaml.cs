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
    /// Logique d'interaction pour Details_Update_Gamme.xaml
    /// </summary>
    public partial class Details_Update_Gamme : Window
    {
        public Details_Update_Gamme(Gamme obj)
        {
            InitializeComponent();
            SetValue(obj);
            this.Show();
        }
        public void SetValue(Gamme obj)
        {
            Detail_module_value_name.Text = obj.nomGamme;
            Detail_module_value_Description.Text = obj.description;
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
