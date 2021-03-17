using Madera.Classe;
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
    /// Logique d'interaction pour FenCreationComposant.xaml
    /// </summary>
    public partial class FenCreationComposant : Window
    {
        public FenCreationComposant()
        {
            InitializeComponent();
            initialiseCombo();
        }

        public Boolean testSaisie()
        {
            if (TextBoxName.Text == "")
            {
                return false;
            }
            if (ComboUnités.SelectedItem == null)
            {
                return false;
            }
            if (TextBoxDescription.Text == "")
            {
                return false;
            }
            return true;
        }

        public void initialiseCombo()
        {
            var unités = Unité.GetAllUnités();
            ComboUnités.ItemsSource = unités.Result;
        }

        public void resetFields()
        {
            TextBoxName.Text = "";
            ComboUnités.SelectedIndex = -1;
            TextBoxDescription.Text = "";
            MessageBox.Show("Composant ajouté.");
        }

        private void submitComposant(object sender, RoutedEventArgs e)
        {
            if (testSaisie() == false)
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
            }
            else
            {
                Composant composant = new Composant();
                composant.nomComposant = TextBoxName.Text;
                composant.unité = ComboUnités.SelectedValue.ToString();
                composant.description = TextBoxDescription.Text;
                Composant.CreateComposant(composant);
                resetFields();
            }

        }
    }
}
