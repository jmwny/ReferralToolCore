using System.Collections.Generic;

namespace ReferralToolCore.Models
{
    public class ReferralData : Notifier
    {
        private List<string> _statusItems = new List<string>
        {
            "Active",
            "Calling",
            "Completed",
            "Managed",
            "Cancelled"
        };
        public List<string> StatusItems
        {
            get { return _statusItems; }
            set
            {
                if (value != this._statusItems)
                {
                    _statusItems = value;
                    OnPropertyChanged("StatusItems");
                }
            }
        }

        private long _id;
        public long ID
        {
            get { return _id; }
            set
            {
                if (value != this._id)
                {
                    _id = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private string _patientName;
        public string PatientName
        {
            get { return _patientName; }
            set
            {
                if (value != this._patientName)
                {
                    _patientName = value;
                    OnPropertyChanged("PatientName");
                }
            }
        }

        private string _cad;
        public string CAD
        {
            get { return _cad; }
            set
            {
                if (value != this._cad)
                {
                    _cad = value;
                    OnPropertyChanged("CAD");
                }
            }
        }

        private string _callStatus;
        public string CallStatus
        {
            get { return _callStatus; }
            set
            {
                if (value != this._callStatus)
                {
                    _callStatus = value;
                    OnPropertyChanged("CallStatus");
                }
            }
        }

        private string _dateOfDischarge;
        public string DateOfDischarge
        {
            get { return _dateOfDischarge; }
            set
            {
                if (value != this._dateOfDischarge)
                {
                    value = value.Split(' ')[0].Replace('/', '-');
                    _dateOfDischarge = value;
                    OnPropertyChanged("DateOfDischarge");
                }
            }
        }

        private string _requestedTime;
        public string RequestedTime
        {
            get { return _requestedTime; }
            set
            {
                if (value != this._requestedTime)
                {
                    _requestedTime = value;
                    OnPropertyChanged("RequestedTime");
                }
            }
        }

        private string _callTaker;
        public string CallTaker
        {
            get { return _callTaker; }
            set
            {
                if (value != this._callTaker)
                {
                    _callTaker = value;
                    OnPropertyChanged("CallTaker");
                }
            }
        }

        private string _nature;
        public string Nature
        {
            get { return _nature; }
            set
            {
                if (value != this._nature)
                {
                    _nature = value;
                    OnPropertyChanged("Nature");
                }
            }
        }

        private string _provider;
        public string Provider
        {
            get { return _provider; }
            set
            {
                if (value != this._provider)
                {
                    _provider = value;
                    OnPropertyChanged("Provider");
                }
            }
        }

        private string _createdDate;
        public string CreatedDate
        {
            get { return _createdDate; }
            set
            {
                if (value != this._createdDate)
                {
                    _createdDate = value;
                    OnPropertyChanged("CreatedDate");
                }
            }
        }

        private string _createdTime;
        public string CreatedTime
        {
            get { return _createdTime; }
            set
            {
                if (value != this._createdTime)
                {
                    _createdTime = value;
                    OnPropertyChanged("CreatedTime");
                }
            }
        }
    }
}