using ReferralToolCore.Models;
using ReferralToolCore.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ReferralToolCore.Views
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

        // Enter new referral dialog
        private void TabControl_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            (DataContext as ViewModel).NewReferralButtonClick();
        }

        // Active tabs (MAS and LGTC) view referral
        private void ListViewActive_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (DataContext as ViewModel).ViewItemClick();
        }

        // History tab button
        private void HistoryRefreshButton_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as ViewModel).HistoryRefreshButtonClick();
        }

        // History tab - view referral dialog
        private void ListViewHistory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (DataContext as ViewModel).HistoryListViewClick();
        }

        private void ComboBox_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            ReferralData rdata = (ReferralData)(sender as ComboBox).DataContext;
            (DataContext as ViewModel).UpdateReferralCallStatus(rdata);
        }
    }
}