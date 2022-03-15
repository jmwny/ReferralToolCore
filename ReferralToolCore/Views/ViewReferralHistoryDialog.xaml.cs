using System.Windows;

namespace ReferralToolCore.Views
{
    /// <summary>
    /// Interaction logic for ViewReferralHistoryDialog.xaml
    /// </summary>
    public partial class ViewReferralHistoryDialog : Window
    {
        public bool duplicateReferral = false;

        public ViewReferralHistoryDialog()
        {
            InitializeComponent();
        }

        private void BtnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void BtnDupe_Click(object sender, RoutedEventArgs e)
        {
            this.duplicateReferral = true;
            this.DialogResult = false;
        }
    }
}
