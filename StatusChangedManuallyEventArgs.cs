/* -------------------------------------------------------------------------------------------------
   Restricted - Copyright (C) Siemens Healthcare GmbH/Siemens Medical Solutions USA, Inc., 2017. All rights reserved
   ------------------------------------------------------------------------------------------------- */
   
using System;


namespace Meesta
{
    public class StatusChangedManuallyEventArgs : EventArgs
    {
        public Status Status { get; set; }
        public bool Manually { get; set; }

        public StatusChangedManuallyEventArgs(Status status, bool manually)
        {
            Status = status;
            Manually = manually;
        }
    }
}