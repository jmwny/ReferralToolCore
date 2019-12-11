﻿using ReferralToolCore.Models;
using ReferralToolCore.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace ReferralToolCore.ViewModels
{
    class ViewModel : Notifier
    {
        #region Properties - Main Collection Classes

        /*
         * Referral Collection Classes
         */

        public ObservableCollection<ReferralData> ReferralCollectionMAS { get; } = new ObservableCollection<ReferralData>();
        public ObservableCollection<ReferralData> ReferralCollectionLogisticare { get; } = new ObservableCollection<ReferralData>();
        public ObservableCollection<ReferralData> ReferralCollectionHistory { get; } = new ObservableCollection<ReferralData>();
        public ObservableCollection<HistoryData> ReferralHistory { get; } = new ObservableCollection<HistoryData>();

        #endregion
        #region Properties - Tab Counts

        /*
         * Counts at the top of each open work tab
         */

        private string _collectionElementsActiveMAS;
        public string CollectionElementsActiveMAS
        {
            get { return _collectionElementsActiveMAS; }
            set
            {
                if (value != this._collectionElementsActiveMAS)
                {
                    _collectionElementsActiveMAS = value;
                    OnPropertyChanged("CollectionElementsActiveMAS");
                }
            }
        }

        private string _collectionElementsActiveLogisticare;
        public string CollectionElementsActiveLogisticare
        {
            get { return _collectionElementsActiveLogisticare; }
            set
            {
                if (value != this._collectionElementsActiveLogisticare)
                {
                    _collectionElementsActiveLogisticare = value;
                    OnPropertyChanged("CollectionElementsActiveLogisticare");
                }
            }
        }

        #endregion
        #region Properties - Other Display Properties

        /*
         * Various other display properties
         */

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                if (value != this._userName)
                {
                    _userName = value;
                    OnPropertyChanged("UserName");
                }
            }
        }

        private string _lastScanTime;
        public string LastScanTime
        {
            get { return _lastScanTime; }
            set
            {
                if (value != this._lastScanTime)
                {
                    _lastScanTime = value;
                    OnPropertyChanged("LastScanTime");
                }
            }
        }

        private string _statusMessageAPIStatus;
        public string StatusMessageAPIStatus
        {
            get { return _statusMessageAPIStatus; }
            set
            {
                if (value != this._statusMessageAPIStatus)
                {
                    _statusMessageAPIStatus = value;
                    OnPropertyChanged("StatusMessageAPIStatus");
                }
            }
        }

        #endregion
        #region Properties - Other Helper Properties

        /*
         * Various other properties
         */

        private ReferralData _selectedItem;
        public ReferralData SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value != this._selectedItem)
                {
                    _selectedItem = value;
                    OnPropertyChanged("SelectedItem");
                }
            }
        }

        private ReferralData _editReferralItem;
        public ReferralData EditReferralItem
        {
            get { return _editReferralItem; }
            set
            {
                if (value != this._editReferralItem)
                {
                    _editReferralItem = value;
                    OnPropertyChanged("EditReferralItem");
                }
            }
        }

        #endregion
        #region Properties - History and archived history related properties

        /*
         * History and archived history related properties
         */

        private string _collectionElementsClosed;
        public string CollectionElementsClosed
        {
            get { return _collectionElementsClosed; }
            set
            {
                if (value != this._collectionElementsClosed)
                {
                    _collectionElementsClosed = value;
                    OnPropertyChanged("CollectionElementsClosed");
                }
            }
        }

        private string _historyDateSelection;
        public string HistoryDateSelection
        {
            get { return _historyDateSelection; }
            set
            {
                value = value.Split(' ')[0].Replace('/', '-');

                if (value != this._historyDateSelection)
                {
                    ReferralCollectionHistory.Clear();
                    _historyDateSelection = value;
                    OnPropertyChanged("HistoryDateSelection");
                }
            }
        }

        private bool _autoRefresh;
        public bool AutoRefresh
        {
            get { return _autoRefresh; }
            set
            {
                if (value != this._autoRefresh)
                {
                    if (value == true)
                    {
                        HistoryDateSelection = DateTime.Now.ToString();
                        historyAutoRefreshTimer.Start();
                        IsEnabledHistoryDatePicker = false;
                        HistoryRefreshButtonClick();
                    }
                    else
                    {
                        IsEnabledHistoryDatePicker = true;
                        historyAutoRefreshTimer.Stop();
                    }

                    _autoRefresh = value;
                    OnPropertyChanged("AutoRefresh");
                }
            }
        }

        private bool _isEnabledHistoryDatePicker;
        public bool IsEnabledHistoryDatePicker
        {
            get { return _isEnabledHistoryDatePicker; }
            set
            {
                if (value != this._isEnabledHistoryDatePicker)
                {
                    _isEnabledHistoryDatePicker = value;
                    OnPropertyChanged("IsEnabledHistoryDatePicker");
                }
            }
        }

        #endregion

        private readonly DispatcherTimer dispatcherTimer;
        private readonly DispatcherTimer historyAutoRefreshTimer;
        private readonly Database db;

        public ViewModel()
        {
            // Get username (API)
            db = new Database();
            UserName = $"{Environment.UserName.ToUpper()}";

            // Set timers
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(HandleTimer);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            dispatcherTimer.Start();

            historyAutoRefreshTimer = new DispatcherTimer();
            historyAutoRefreshTimer.Tick += new EventHandler(HandleAutoRefreshTimer);
            historyAutoRefreshTimer.Interval = new TimeSpan(0, 1, 0);

            HistoryDateSelection = DateTime.Now.ToString("M-d-yyyy");
            CollectionElementsClosed = ReferralCollectionHistory.Count.ToString();
            AutoRefresh = false;
            IsEnabledHistoryDatePicker = true;

            _ = RebuildCollection();
        }

        /*
         * UI Command Implementations
         */

        public async void NewReferralButtonClick()
        {
            EditReferralItem = new ReferralData
            {
                ID = 0,
                PatientName = string.Empty,
                CAD = string.Empty,
                CallStatus = "Active",
                DateOfDischarge = DateTime.Now.ToString("M-d-yyyy"),
                RequestedTime = string.Empty,
                CallTaker = Environment.UserName.ToLower(),
                Nature = string.Empty,
                Provider = string.Empty,
                CreatedDate = DateTime.Now.ToString("M-d-yyyy"),
                CreatedTime = DateTime.Now.ToString("HH:mm:ss")
            };

            NewReferralDialog newReferralDialog = new NewReferralDialog()
            {
                Owner = Application.Current.MainWindow,
                DataContext = this
            };
            newReferralDialog.RadioButton_MAS.IsChecked = true;

            if (newReferralDialog.ShowDialog() == true)
            {
                if (EditReferralItem.PatientName == string.Empty)
                    EditReferralItem.PatientName = "<none>";
                if (EditReferralItem.CAD == string.Empty)
                    EditReferralItem.CAD = "<none>";
                if (EditReferralItem.RequestedTime == string.Empty)
                    EditReferralItem.RequestedTime = "<none>";
                if (EditReferralItem.Nature == string.Empty)
                    EditReferralItem.Nature = "<none>";
                if (newReferralDialog.RadioButton_MAS.IsChecked == true)
                    EditReferralItem.Provider = "MAS";
                else
                    EditReferralItem.Provider = "Logisticare";

                // Cleanup the referral object
                EditReferralItem.PatientName = EditReferralItem.PatientName.Trim();
                EditReferralItem.CAD = EditReferralItem.CAD.Trim();
                EditReferralItem.RequestedTime = EditReferralItem.RequestedTime.Trim();
                EditReferralItem.Nature = EditReferralItem.Nature.Trim();

                await Task.Run(async () =>
                {
                    var result = await db.InsertIntoCollection(EditReferralItem);
                    StatusMessageAPIStatus = result;
                });
            }
        }

        public async void ViewItemClick()
        {
            if (SelectedItem == null)
                return;

            // Populate edit history from database
            var history = await db.GetHistory(SelectedItem.ID.ToString());
            foreach (KeyValuePair<string, List<object>> entry in history)
            {
                HistoryData historyDataItem = new HistoryData
                {
                    EditTime = entry.Value[1].ToString(),
                    Name = entry.Value[2].ToString(),
                    CAD = entry.Value[3].ToString(),
                    Status = entry.Value[4].ToString(),
                    DOD = entry.Value[5].ToString(),
                    ReqTime = entry.Value[6].ToString(),
                    Nature = entry.Value[7].ToString(),
                    Provider = entry.Value[8].ToString()
                };
                ReferralHistory.Add(historyDataItem);
            }

            EditReferralItem = SelectedItem;
            string oldProvider = EditReferralItem.Provider;

            NewReferralDialog newReferralDialog = new NewReferralDialog()
            {
                Owner = Application.Current.MainWindow,
                Title = "Edit Referral Item Data",
                DataContext = this
            };

            if (EditReferralItem.Provider == "MAS")
                newReferralDialog.RadioButton_MAS.IsChecked = true;
            else
                newReferralDialog.RadioButton_Logisticare.IsChecked = true;

            if (newReferralDialog.ShowDialog() == true)
            {
                // Cleanup the referral object
                EditReferralItem.PatientName = EditReferralItem.PatientName.Trim();
                EditReferralItem.CAD = EditReferralItem.CAD.Trim();
                EditReferralItem.RequestedTime = EditReferralItem.RequestedTime.Trim();
                EditReferralItem.Nature = EditReferralItem.Nature.Trim();
                if (newReferralDialog.RadioButton_MAS.IsChecked == true)
                    EditReferralItem.Provider = "MAS";
                else
                    EditReferralItem.Provider = "Logisticare";

                await Task.Run(async () =>
                {
                    // Moved referral?  Drop event and reset CallStatus to an active state
                    if (oldProvider != EditReferralItem.Provider)
                        EditReferralItem.CallStatus = "Active";

                    var result = await db.UpdateReferral(EditReferralItem);
                    StatusMessageAPIStatus = $"UpdateReferral: ID = {EditReferralItem.ID.ToString()}, {result.Trim()}";
                    if (result.Trim() == "false")
                        MessageBox.Show("Referenced object is no longer flagged as active.  Edits cancelled",
                            "Informational", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                });

                // pop referral from observable collection if provider has changed
                if (oldProvider != EditReferralItem.Provider)
                {
                    ObservableCollection<ReferralData> obsCol;

                    if (EditReferralItem.Provider == "MAS")
                        obsCol = ReferralCollectionLogisticare;
                    else
                        obsCol = ReferralCollectionMAS;

                    var obj = obsCol.SingleOrDefault(a => a.ID == EditReferralItem.ID);
                    if (obj != null)
                        obsCol.Remove(obj);
                }
            }
            SelectedItem = null;
            ReferralHistory.Clear();
        }

        public async void HistoryRefreshButtonClick()
        {
            var result = await db.GetHistoryCollection(HistoryDateSelection);
            if (result.ContainsKey("-1"))
            {
                StatusMessageAPIStatus = $"GetHistoryCollection: API Failure...";
                return;
            }
            else
                StatusMessageAPIStatus = $"GetHistoryCollection: Returns {result.Count} Elements";

            foreach (KeyValuePair<string, List<object>> entry in result)
            {
                ReferralData newReferralData = new ReferralData
                {
                    ID = Convert.ToInt64(entry.Value[0].ToString()),
                    PatientName = entry.Value[1].ToString(),
                    CAD = entry.Value[2].ToString(),
                    CallStatus = entry.Value[3].ToString(),
                    DateOfDischarge = entry.Value[4].ToString(),
                    RequestedTime = entry.Value[5].ToString(),
                    CallTaker = entry.Value[6].ToString(),
                    Nature = entry.Value[7].ToString(),
                    Provider = entry.Value[8].ToString(),
                    CreatedDate = entry.Value[9].ToString(),
                    CreatedTime = entry.Value[10].ToString()
                };
                if (ReferralCollectionHistory.SingleOrDefault(a => a.ID == newReferralData.ID) == null)
                {
                    ReferralCollectionHistory.Add(newReferralData);
                    System.Diagnostics.Trace.WriteLine($"Added ID: {newReferralData.ID}");
                }
            }
            CollectionElementsClosed = ReferralCollectionHistory.Count.ToString();
        }

        public async void HistoryListViewClick()
        {
            if (SelectedItem == null)
                return;

            // Populate edit history from database
            var history = await db.GetHistory(SelectedItem.ID.ToString());
            foreach (KeyValuePair<string, List<object>> entry in history)
            {
                HistoryData historyDataItem = new HistoryData
                {
                    EditTime = entry.Value[1].ToString(),
                    Name = entry.Value[2].ToString(),
                    CAD = entry.Value[3].ToString(),
                    Status = entry.Value[4].ToString(),
                    DOD = entry.Value[5].ToString(),
                    ReqTime = entry.Value[6].ToString(),
                    Nature = entry.Value[7].ToString(),
                    Provider = entry.Value[8].ToString()
                };
                ReferralHistory.Add(historyDataItem);
            }

            EditReferralItem = SelectedItem;

            ViewReferralHistoryDialog viewHistoryDialog = new ViewReferralHistoryDialog()
            {
                Owner = Application.Current.MainWindow,
                DataContext = this
            };
            viewHistoryDialog.ShowDialog();

            SelectedItem = null;
            ReferralHistory.Clear();
        }


        /*
         * RebuildCollection and support methods
         */

        private async void HandleTimer(object source, EventArgs e)
        {
            await RebuildCollection();
        }

        private void HandleAutoRefreshTimer(object source, EventArgs e)
        {
            HistoryRefreshButtonClick();
        }

        private async Task RebuildCollection()
        {
            ObservableCollection<ReferralData> tmpCollectionMAS = new ObservableCollection<ReferralData>();
            ObservableCollection<ReferralData> tmpCollectionLogisticare = new ObservableCollection<ReferralData>();

            if (dispatcherTimer.IsEnabled == false)
                return;
            else
                dispatcherTimer.Stop();

            // Check for new magick (referrals and updates)
            if (await db.HasMagick())
            {
                var result = await db.GetOpenCollection();
                if (result.ContainsKey("-1"))
                    StatusMessageAPIStatus = $"GetOpenCollection: API Failure...";
                else
                    StatusMessageAPIStatus = $"GetOpenCollection: Returns {result.Count} Elements";

                foreach (KeyValuePair<string, List<object>> entry in result)
                {
                    ReferralData newReferralData = new ReferralData
                    {
                        ID = Convert.ToInt64(entry.Value[0].ToString()),
                        PatientName = entry.Value[1].ToString(),
                        CAD = entry.Value[2].ToString(),
                        CallStatus = entry.Value[3].ToString(),
                        DateOfDischarge = entry.Value[4].ToString(),
                        RequestedTime = entry.Value[5].ToString(),
                        CallTaker = entry.Value[6].ToString(),
                        Nature = entry.Value[7].ToString(),
                        Provider = entry.Value[8].ToString(),
                        CreatedDate = entry.Value[9].ToString(),
                        CreatedTime = entry.Value[10].ToString()
                    };

                    // 1) Sort returned referrals and add to temporary collection/s
                    ObservableCollection<ReferralData> collectionPointer;
                    if (newReferralData.Provider == "MAS")
                    {
                        collectionPointer = ReferralCollectionMAS;
                        tmpCollectionMAS.Add(newReferralData);
                    }
                    else
                    {
                        collectionPointer = ReferralCollectionLogisticare;
                        tmpCollectionLogisticare.Add(newReferralData);
                    }

                    // 2) If referral doesn't exist in primary collection, simply add
                    if (collectionPointer.SingleOrDefault(a => a.ID == newReferralData.ID) == null)
                        collectionPointer.Add(newReferralData);
                }
                // Compare and update collections
                UpdateActiveReferrals("MAS", tmpCollectionMAS);
                UpdateActiveReferrals("Logisticare", tmpCollectionLogisticare);
            }
            else
                StatusMessageAPIStatus = "No database updates detected...";

            //Update a few UI elements for Status
            LastScanTime = DateTime.Now.ToString("HH:mm:ss");
            CollectionElementsActiveMAS = $"{ReferralCollectionMAS.Count}";
            CollectionElementsActiveLogisticare = $"{ReferralCollectionLogisticare.Count}";
            dispatcherTimer.Start();
        }

        private void UpdateActiveReferrals(string primaryCollection, ObservableCollection<ReferralData> tmpReferralCollection)
        {
            ObservableCollection<ReferralData> activeCollection;

            if (primaryCollection == "MAS")
                activeCollection = ReferralCollectionMAS;
            else
                activeCollection = ReferralCollectionLogisticare;

            for (int i = 0; i <= activeCollection.Count() - 1; i++)
            {
                // If item doesn't exist in temp collectin, pop from active collection
                ReferralData tmpReferral = tmpReferralCollection.SingleOrDefault(a => a.ID == activeCollection[i].ID);
                if (tmpReferral == null)
                {
                    activeCollection.RemoveAt(i);
                    i--;
                }
                // item does exist, update
                else
                {
                    activeCollection[i].PatientName = tmpReferral.PatientName;
                    activeCollection[i].CAD = tmpReferral.CAD;
                    activeCollection[i].DateOfDischarge = tmpReferral.DateOfDischarge;
                    activeCollection[i].RequestedTime = tmpReferral.RequestedTime;
                    activeCollection[i].Nature = tmpReferral.Nature;

                    if (activeCollection[i].CallStatus != tmpReferral.CallStatus)
                        activeCollection[i].CallStatus = tmpReferral.CallStatus;
                }
            }
        }

        public async void UpdateReferralCallStatus(ReferralData referral)
        {
            await Task.Run(async () =>
            {
                var result = await db.UpdateReferral(referral);
                StatusMessageAPIStatus = $"UpdateReferralCallStatus: ID = {referral.ID.ToString()}, {result.Trim()}";
            });

            // Remove from collection if referral is no longer active
            if (referral.CallStatus != "Active" && referral.CallStatus != "Calling")
            {
                if (referral.Provider == "MAS")
                    ReferralCollectionMAS.Remove(referral);
                else
                    ReferralCollectionLogisticare.Remove(referral);
            }
        }
    }

    [ValueConversion(typeof(string), typeof(int))]
    public class DateTimeToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime referralDate = DateTime.Parse((string)value);
            DateTime todayDate = DateTime.Now;

            if (referralDate.Date > todayDate.Date)
                return 1;
            else if (referralDate.Date < todayDate.Date)
                return -1;
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}