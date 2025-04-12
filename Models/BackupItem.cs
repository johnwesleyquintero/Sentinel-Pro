using System;
using System.ComponentModel;

namespace SentinelPro.Models
{
    public class BackupItem : INotifyPropertyChanged
    {
        private string _id;
        private string _description;
        private DateTime _timestamp;
        private string _size;

        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public DateTime Timestamp
        {
            get => _timestamp;
            set
            {
                _timestamp = value;
                OnPropertyChanged(nameof(Timestamp));
            }
        }

        public string Size
        {
            get => _size;
            set
            {
                _size = value;
                OnPropertyChanged(nameof(Size));
            }
        }
        
        private string _originalPath;

        public string OriginalPath
        {
            get => _originalPath;
            set
            {
                _originalPath = value;
                OnPropertyChanged(nameof(OriginalPath));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}