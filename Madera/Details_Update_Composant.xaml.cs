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
    /// Logique d'interaction pour Details_Update_Composant.xaml
    /// </summary>
    public partial class Details_Update_Composant : Window
    {

        //public ViewModel _DataUnite { get; set; }
        public Details_Update_Composant(Composant obj)
        {
            InitializeComponent();
            SetValue(obj);
            SetComboBox(obj);
            this.Show();
        }

        public void SetValue(Composant obj)
        {
            Detail_composant_value_name.Text = obj.nomComposant;
        }

        public async void SetComboBox(Composant obj)
        {
            try
            {
                var unites = await Unité.GetAllUnités();
                var caracs = await Caracteristique.GetAllCaracteristique();
                this.DataContext = new ViewModelComposant(unites, caracs);
                for (int i = 0; i < Detail_composant_value_unite.Items.Count; i++)
                {
                    Unité item = (Unité)Detail_composant_value_unite.Items.GetItemAt(i);
                    if(item.uniteMesure == obj.unité)
                    {
                        Detail_composant_value_unite.SelectedItem = item;
                    }
                }
                for (int i = 0; i < Detail_composant_value_carac.Items.Count; i++)
                {
                    Caracteristique item = (Caracteristique)Detail_composant_value_carac.Items.GetItemAt(i);
                    if (item.nomCaracteristique == obj.nomCaracteristique)
                    {
                        Detail_composant_value_carac.SelectedItem = item;
                    }
                }
            }
            catch (Exception error)
            {
            }
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
    public class ViewModelComposant
    {
        public ObservableCollection<Unité> CollecUnite { get; set; }
        public ObservableCollection<Caracteristique> CollecCaracteristique { get; set; }

        public ViewModelComposant(Unité[] Ou, Caracteristique[] Oc)
        {
            this.CollecUnite = new ObservableCollection<Unité>();
            foreach(Unité unite in Ou)
            {
                this.CollecUnite.Add(unite);
            }

            this.CollecCaracteristique = new ObservableCollection<Caracteristique>();
            foreach (Caracteristique carac in Oc)
            {
                this.CollecCaracteristique.Add(carac);
            }
        }
    }
}
