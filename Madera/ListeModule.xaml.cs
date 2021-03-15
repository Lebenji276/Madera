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
    /// Logique d'interaction pour ListeModule.xaml
    /// </summary>
    public partial class ListeModule : Page
    {
        private Module[] _modules { get; set; }

        public ListeModule(Module[] modules)
        {
            this._modules = modules;
            InitializeComponent();

            lvModule.ItemsSource = modules;
        }
    }
}
