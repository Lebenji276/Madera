using Madera.Classe;
using System;
using System.Windows;

namespace Madera
{
    /// <summary>
    /// Logique d'interaction pour FenCreationDevis.xaml
    /// </summary>
    public partial class FenCreationDevis : Window
    {
        public FenCreationDevis(Module[] listmodules)
        {
            InitializeComponent();
            getClients(listmodules);
            
        }
        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menu = new MenuWindow();
            App.Current.MainWindow = menu;
            Close();
            menu.Show();
        }

        private async void getClients(Module[] listmodules)
        {
            var toto = Client.GetAllClient();
            ComboClients.ItemsSource = toto;
            ListeModules.ItemsSource = listmodules;
            ComModules.ItemsSource = listmodules;
            ComboComposants.Items.Add("Angle entrant");
            ComboComposants.Items.Add("Angle sortant");
            var gammes = await Gamme.GetAllGammes();
            ComboGammes.ItemsSource = gammes;
        }

        private void btnPayer_Click(object sender, RoutedEventArgs e)
        {
            PaymentCreditWindow payment = new PaymentCreditWindow();
            App.Current.MainWindow = payment;
            payment.ShowDialog();
        }
    }
}
