using System;
using System.Windows;

namespace ReferralToolCore.Views
{
    /// <summary>
    /// Interaction logic for NewReferralDialog.xaml
    /// </summary>
    public partial class NewReferralDialog : Window
    {
        public NewReferralDialog()
        {
            InitializeComponent();
        }

        private void BtnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void BtnAddDate_Click(object sender, RoutedEventArgs e)
        {
            DateTime selectedDT = (DateTime)datePickerDropdown.SelectedDate;
            datePickerDropdown.SelectedDate = selectedDT.AddDays(1);
        }
    }
}
