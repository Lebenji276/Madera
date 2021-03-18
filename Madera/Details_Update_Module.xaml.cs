using Madera.Classe;
using Newtonsoft.Json;
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
    /// Logique d'interaction pour Details_Update_Module.xaml
    /// </summary>
    public partial class Details_Update_Module : Window
    {
        ViewModelModule Data { get; set; }
        Module _obj;
        public Details_Update_Module(Module obj)
        {
            InitializeComponent();
            _obj = obj;
            SetValue(obj);
            SetComboBox(obj);
            this.Show();
        }
        public void SetValue(Module obj)
        {
            Detail_module_value_name.Text = obj.nomModule;
        }
        public async void SetComboBox(Module obj)
        {
            try
            {
                var gammes = await Gamme.GetAllGammes();
                var allCompo = await Composant.GetAllComposant();
                Detail_module_value_gamme.ItemsSource = gammes;
                Data = new ViewModelModule(gammes, obj.composantsArray, allCompo);
                this.DataContext = Data;
                for (int i = 0; i < Detail_module_value_gamme.Items.Count; i++)
                {
                    Gamme item = (Gamme)Detail_module_value_gamme.Items.GetItemAt(i);
                    if (item.nomGamme == obj.nomGamme)
                    {
                        Detail_module_value_gamme.SelectedItem = item;
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
            List<String> listestr = new List<String>();
            Module module = new Module();
            if (Detail_module_value_composant.Items.Count > 0)
            {
                foreach (Composant composant in Detail_module_value_composant.Items)
                {
                    listestr.Add(composant._id);
                }
            }
            
            string newJson = JsonConvert.SerializeObject(listestr, Formatting.None);
            _obj.composant = newJson;
            _obj.nomModule = Detail_module_value_name.Text;
            if (Detail_module_value_gamme.Text != "")
            {
            _obj.nomGamme = Detail_module_value_gamme.SelectedItem.ToString();

            }

            Module.UpdateModule(_obj);
            Close();
        }
        private void ClickDelete(object sender, RoutedEventArgs e)
        {
            Data.CollecCompo.Remove((Composant)Detail_module_value_composant.SelectedItem);
            this.DataContext = Data;
        }
        private void ClickAdd(object sender, RoutedEventArgs e)
        {
            Data.CollecCompo.Add((Composant)Add_selected_compo.SelectedItem);
            this.Data = Data;
        }
    }

    public class ViewModelModule
    {
        public ObservableCollection<Gamme> CollecGamme { get; set; }
        public ObservableCollection<Composant> CollecCompo { get; set; }
        public ObservableCollection<Composant> CollecAllCompo { get; set; }

        public ViewModelModule(Gamme[] Og, List<Composant> Oc, Composant[] Oac)
        {
            //this.CollecGamme = new ObservableCollection<Gamme>();
            //foreach (Gamme gamme in Og)
            //{
            //    this.CollecGamme.Add(gamme);
            //}

            this.CollecCompo = new ObservableCollection<Composant>();
            if (Oc != null && Oc.Count > 0)
            {
                foreach (Composant composant in Oc)
                {
                    this.CollecCompo.Add(composant);
                }
            }

            this.CollecAllCompo = new ObservableCollection<Composant>();
            foreach (Composant composant in Oac)
            {
                this.CollecAllCompo.Add(composant);
            }

        }
    }
}
