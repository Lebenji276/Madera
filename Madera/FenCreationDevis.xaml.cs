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
        private Client[] _clients { get; set; }
        private Gamme[] _gammes { get; set; }
        private Module[] _modules { get; set; }
        public FenCreationDevis(Client[] clients, Gamme[] gammes, Module[] modules)
        {
            InitializeComponent();
            this._clients = clients;
            this._gammes = gammes;
            this._modules = modules;


            ComboClients.ItemsSource = _clients;
            ListeModules.ItemsSource = _modules;
            ComModules.ItemsSource = _modules;
            ComboComposants.Items.Add("Angle entrant");
            ComboComposants.Items.Add("Angle sortant");
            ComboGammes.ItemsSource = _gammes
                ;

        }
    }
}
