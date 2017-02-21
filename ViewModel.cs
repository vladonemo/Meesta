/* -------------------------------------------------------------------------------------------------
   Restricted - Copyright (C) Siemens Healthcare GmbH/Siemens Medical Solutions USA, Inc., 2017. All rights reserved
   ------------------------------------------------------------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;


namespace Meesta
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ICommand ChangeStatusManuallyCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<StatusChangedManuallyEventArgs> StatusChangedManually;

        private StatusView myStatus;

        public StatusView Status
        {
            get { return myStatus; }
            set
            {
                SetField(ref myStatus, value);
            }
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }
            field = value;
            FirePropertyChange(propertyName);
            return true;
        }

        public ViewModel()
        {
            Status = StatusView.NotInMeeting;
            ChangeStatusManuallyCommand = new RelayCommand<StatusView>(statusView =>
            {
                SetStatus(statusView, true);
            });
        }

        private void SetStatus(StatusView statusView, bool manually)
        {
            PropertyChangedEventHandler onPropertyChanged = (sender, args) =>
            {
                if (args.PropertyName == "Status")
                {
                    FireStatusChangedManually(manually);
                }
            };

            try
            {
                PropertyChanged += onPropertyChanged;
                Status = statusView;
            }
            finally
            {
                PropertyChanged -= onPropertyChanged;
            }
        }

        private void FireStatusChangedManually(bool manually)
        {
            var handler = StatusChangedManually;
            if (handler != null)
            {
                handler(this, new StatusChangedManuallyEventArgs(Status.Status, manually));
            }
        }

        private void FirePropertyChange(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void ChangeStatusAutomatically(Status status)
        {
            SetStatus(StatusView.Of(status), false);
        }
    }
}
