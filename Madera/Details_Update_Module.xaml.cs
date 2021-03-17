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
    /// Logique d'interaction pour Details_Update_Module.xaml
    /// </summary>
    public partial class Details_Update_Module : Window
    {
        public Details_Update_Module(Module obj)
        {
            InitializeComponent();
            SetValue(obj);
            this.Show();
        }
        public void SetValue(Module obj)
        {
            Detail_module_value_name.Text = obj.nomModule;
            Detail_module_value_name.Text = obj.nomModule;
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
