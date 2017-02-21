/* -------------------------------------------------------------------------------------------------
   Restricted - Copyright (C) Siemens Healthcare GmbH/Siemens Medical Solutions USA, Inc., 2017. All rights reserved
   ------------------------------------------------------------------------------------------------- */
   
using System;
using System.Runtime.InteropServices;

using CommunicatorAPI;


namespace Meesta
{
    internal class OfficCommunicatorWrapper : IDisposable
    {
        private Messenger myCommunicator;

        public OfficCommunicatorWrapper()
        {
            myCommunicator = new Messenger();
            myCommunicator.AutoSignin();
        }

        public void Dispose()
        {
            Marshal.ReleaseComObject(myCommunicator);
            myCommunicator = null;
        }

        public Status State
        {
            get { return Translate(myCommunicator.MyStatus); }
            set { myCommunicator.MyStatus = Translate(value); }
        }

        private static MISTATUS Translate(Status status)
        {
            return status == Status.InMeeting ? MISTATUS.MISTATUS_BUSY : MISTATUS.MISTATUS_ONLINE;
        }

        private static Status Translate(MISTATUS status)
        {
            return status == MISTATUS.MISTATUS_IN_A_CONFERENCE ||
                   status == MISTATUS.MISTATUS_BUSY ||
                   status == MISTATUS.MISTATUS_DO_NOT_DISTURB ||
                   status == MISTATUS.MISTATUS_IN_A_MEETING ||
                   status == MISTATUS.MISTATUS_ON_THE_PHONE
                       ? Status.InMeeting
                       : Status.NotInMeeting;
        }
    }
}