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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Madera
{
    /// <summary>
    /// Logique d'interaction pour ListeClient.xaml
    /// </summary>
    public partial class ListeClient : Page
    {
        public ListeClient()
        {
            InitializeComponent();
            var toto = Client.GetAllClient();
            textBox.Text = toto.Result[0].first_name;
        }


    }
}
