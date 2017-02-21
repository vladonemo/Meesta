/* -------------------------------------------------------------------------------------------------
   Restricted. Copyright (C) Siemens Healthcare GmbH, 2017. All rights reserved.
   ------------------------------------------------------------------------------------------------- */

using System;
using System.Runtime.InteropServices;

using CommunicatorAPI;

using Microsoft.Win32;


namespace Meesta
{
    internal class CommunicatorService
    {
        public static Status GetCurrentStatus()
        {
            Messenger communicator = new Messenger();
            if (Convert.ToInt32(Registry.CurrentUser
                .OpenSubKey("Software")
                .OpenSubKey("IM Providers")
                .OpenSubKey("Communicator")
                .GetValue("UpAndRunning", 1)) != 2)
            {
                Console.WriteLine("The communicator is not running.");
                return Status.NotInMeeting;
            }
            communicator.AutoSignin();

            var status = Translate(communicator.MyStatus);

            Marshal.ReleaseComObject(communicator);
            return status;
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
