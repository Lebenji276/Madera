using System.Windows;

namespace Madera
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public class Product
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Category { get; set; }
        }
        void OnClick1(object sender, RoutedEventArgs e)
        {
            label_1.Content = "User : " + textBox.Text;
            label_2.Content = "MDP : " + passwordBox.Password;

            var a = 1 + 1;

            MenuWindow main = new MenuWindow();
            App.Current.MainWindow = main;
            this.Close();
            main.Show();
        }

    }
}
