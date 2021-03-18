using Madera.Classe;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Logique d'interaction pour PaymentCreditWindow.xaml
    /// </summary>
    public partial class PaymentCreditWindow : Window
    {
        public PaymentCreditWindow()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            // Get the parameters.
            bool err = false;
            decimal balance;
            decimal interest_rate;
            decimal payment_percent;
            decimal min_payment;
            if (!decimal.TryParse(txtInitialBalance.Text, out balance))
            {
                lbl_error_Total.Content = "Erreur de saisie sur le montant total";
                err = true;
            }
            if (!decimal.TryParse(txtInterestRate.Text.Replace("%", ""), out interest_rate))
            {
                lbl_error_Interet.Content = "Erreur de saisie sur le taux d'intérêt";
                err = true;
            }
            if (!decimal.TryParse(txtPaymentPercent.Text.Replace("%", ""), out payment_percent))
            {
                lbl_error_TauxPaiement.Content = "Erreur de saisie sur le taux de paiement";
                err = true;
            }
            if (!decimal.TryParse(txtMinPayment.Text, out min_payment))
            {
                lbl_error_PaiementMini.Content = "Erreur de saisie sur le paiement minimum";
                err = true;
            }
            interest_rate /= 100;
            payment_percent /= 100;
            interest_rate /= 12;

            lblTotalPayments.Content = null;
            decimal total_payments = 0;

            // Display the initial balance.
            lvwPayments.Items.Clear();
            if (!err)
            {

                PaymentData data = new PaymentData()
                {
                    Period = "0",
                    Payment = null,
                    Interest = null,
                    Balance = balance.ToString("c"),
                };
                lvwPayments.Items.Add(data);

                // Loop until balance == 0.
                for (int i = 1; balance > 0; i++)
                {
                    // Calculate the payment.
                    decimal payment = balance * payment_percent;
                    if (payment < min_payment) { payment = min_payment; }

                    // Calculate interest.
                    decimal interest = balance * interest_rate;
                    balance += interest;

                    // See if we can pay off the balance.
                    if (payment > balance) payment = balance;
                    total_payments += payment;
                    balance -= payment;

                    // Display results.
                    data = new PaymentData()
                    {
                        Period = i.ToString(),
                        Payment = payment.ToString("c"),
                        Interest = interest.ToString("c"),
                        Balance = balance.ToString("c"),
                        Sum = (payment + interest).ToString("c")
                    };
                    lvwPayments.Items.Add(data);
                }
                radiobtnCB.Visibility = Visibility.Visible;
                radiobtnCheque.Visibility = Visibility.Visible;
                radiobtnVirement.Visibility = Visibility.Visible;
                // Display the total payments.
                lblTotalPayments.Content = total_payments.ToString("c");
            }
        }

        private void btnPayer_Click(object sender, RoutedEventArgs e)
        {
            if (radiobtnCheque.IsChecked == true)
            {
                MessageBox.Show("Réceptionez les chèques");
            }
            if (radiobtnCB.IsChecked == true)
            {
                MessageBox.Show("Utilisez votre lecteur");
            }
            if (radiobtnVirement.IsChecked == true)
            {
                MessageBox.Show("IBAN : FR7630001007941234567890185 \nCode banque : 30001 \nCode guichet : 00794 \nNuméro de compte: 12345678901 \nClé du RIB: 85");
            }

            PaymentData paymentData = new PaymentData() { Balance = txtInitialBalance.Text, Interest = txtInterestRate.Text, Payment = txtPaymentPercent.Text, Period = txtMinPayment.Text, Sum = lblTotalPayments.Content.ToString() };
            btnCreate.Visibility = Visibility.Visible;
        }

        private void radiobtnCB_Checked(object sender, RoutedEventArgs e)
        {
            btnPayer.Visibility = Visibility.Visible;
        }

        private void radiobtnCheque_Checked(object sender, RoutedEventArgs e)
        {
            btnPayer.Visibility = Visibility.Visible;
        }

        private void radiobtnVirement_Checked(object sender, RoutedEventArgs e)
        {
            btnPayer.Visibility = Visibility.Visible;
        }

        private void txtInitialBalance_TextChanged(object sender, TextChangedEventArgs e)
        {
            lbl_error_Total.Content = "";
        }

        private void txtInterestRate_TextChanged(object sender, TextChangedEventArgs e)
        {
            lbl_error_Interet.Content = "";
        }

        private void txtPaymentPercent_TextChanged(object sender, TextChangedEventArgs e)
        {
            lbl_error_TauxPaiement.Content = "";
        }

        private void txtMinPayment_TextChanged(object sender, TextChangedEventArgs e)
        {
            lbl_error_PaiementMini.Content = "";
        }
    }
}
