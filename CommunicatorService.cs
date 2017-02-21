/* -------------------------------------------------------------------------------------------------
   Restricted. Copyright (C) Siemens Healthcare GmbH, 2017. All rights reserved.
   ------------------------------------------------------------------------------------------------- */

using System;

using Microsoft.Win32;


namespace Meesta
{
    internal class CommunicatorService
    {
        public static Status GetCurrentStatus()
        {
            if (!IsRunning())
            {
                return Status.NotInMeeting;
            }
            using (var communicator = new OfficCommunicatorWrapper())
            {
                return communicator.State;
            }
        }

        public static bool IsRunning()
        {
            return Convert.ToInt32(Registry.CurrentUser
                       .OpenSubKey("Software")
                       .OpenSubKey("IM Providers")
                       .OpenSubKey("Communicator")
                       .GetValue("UpAndRunning", 1)) == 2;
        }

        public static void SetStatus(Status status)
        {
            if (!IsRunning())
            {
                return;
            }
            using (var communicator = new OfficCommunicatorWrapper())
            {
                communicator.State = status;
            }
        }
    }
}
