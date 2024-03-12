using ReferralToolCore.Models;
using ReferralToolCore.ViewModels;
using System;
using System.Net.NetworkInformation;
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
        string referralReceived = "Referral received. Please standby while we review this referral.";
        string authRequiredMAS = "Trip #TRIPNUM was released to (insert vendor name).  Please stand by while we contact insurance to set up transportation. Thank you.";
        string closedReferralLivery = "Trip #TRIPNUM on (insert date only) arranged with Medicaid insurance. They state they may take up to 3hrs to assign a vendor. All Livery request vendors will call the patient directly at the number you provided. **Referral is now closed and will no longer be viewed. PLEASE CALL IN ANY CHANGES. DO NOT EDIT. ** Thank you.";
        string closedReferralNextAvail = "Trip #TRIPNUM confirmed on (insert date) with (insert vendor name). The vendor is granted a 3hr window to arrive on scene. You can expect their arrival by (insert standard time) for this request. **Referral is now closed and will no longer be viewed. PLEASE CALL IN ANY CHANGES. DO NOT EDIT. ** Thank you.";
        string closedReferralPreSched = "Trip #TRIPNUM on DATETIME with (insert vendor name). **Referral is now closed and will no longer be viewed. PLEASE CALL IN ANY CHANGES. DO NOT EDIT. ** Thank you.";

        public MainWindow()
        {
            InitializeComponent();

            Btn_referralReceived.Content = referralReceived;
            Btn_authRequiredMAS.Content = authRequiredMAS;
            Btn_closedReferralLivery.Content = closedReferralLivery;
            Btn_closedReferralNextAvail.Content = closedReferralNextAvail;
            Btn_closedReferralPreSched.Content = closedReferralPreSched;
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

        private void Btn_Checked(object sender, RoutedEventArgs e)
        {
            string text = (sender as RadioButton).Content.ToString();

            string tmpTextMAS = authRequiredMAS.Replace("(insert vendor name)", text);
            Btn_authRequiredMAS.Content = tmpTextMAS;

            string tmpTextClosed = closedReferralNextAvail.Replace("(insert vendor name)", text);
            Btn_closedReferralNextAvail.Content = tmpTextClosed;

            string tmpTextClosedPreSched = closedReferralPreSched.Replace("(insert vendor name)", text);
            Btn_closedReferralPreSched.Content = tmpTextClosedPreSched;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Btn_Assist.IsChecked = true;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            string text = (sender as Button).Content.ToString();
            text = text.Replace("(insert date only)", DateTime.Now.ToShortDateString());
            text = text.Replace("(insert date)", DateTime.Now.ToString());
            text = text.Replace("(insert standard time)", DateTime.Now.AddHours(3).ToString());
            Clipboard.SetText(text);
        }
    }
}