/* -------------------------------------------------------------------------------------------------
   Restricted - Copyright (C) Siemens Healthcare GmbH/Siemens Medical Solutions USA, Inc., 2017. All rights reserved
   ------------------------------------------------------------------------------------------------- */

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;


namespace Meesta
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ICommand ChangeStatusCommand { get; set; }

        private StatusView myStatus;

        public StatusView Status
        {
            get { return myStatus; }
            set
            {
                SetField(ref myStatus, value);
            }
        }

        protected void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return;
            }
            field = value;
            OnPropertyChange(propertyName);
        }

        public ViewModel()
        {
            Status = StatusView.NotInMeeting;
            ChangeStatusCommand = new RelayCommand<StatusView>(statusView =>
            {
                Status = statusView;
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChange(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
