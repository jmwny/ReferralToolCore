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
    }
}
