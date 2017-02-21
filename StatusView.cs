/* -------------------------------------------------------------------------------------------------
   Restricted - Copyright (C) Siemens Healthcare GmbH/Siemens Medical Solutions USA, Inc., 2017. All rights reserved
   ------------------------------------------------------------------------------------------------- */

using System.Collections.Generic;
using System.Windows.Media;


namespace Meesta
{
    public class StatusView
    {
        private static readonly IDictionary<Status, StatusView> myStatus = new Dictionary<Status, StatusView>
        {
            {Status.InMeeting, new StatusView(Brushes.Red, "In meeting", Status.InMeeting)},
            {Status.NotInMeeting, new StatusView(Brushes.Green, "Not in meeting", Status.NotInMeeting)}
        };

        public Brush Brush { get; private set; }
        public string Text { get; private set; }
        public Status Status { get; private set; }

        private StatusView(Brush brush, string text, Status status)
        {
            Brush = brush;
            Text = text;
            Status = status;
        }

        public static StatusView Of(Status status)
        {
            return myStatus[status];
        }

        public static StatusView InMeeting
        {
            get { return Of(Status.InMeeting); }
        }

        public static StatusView NotInMeeting
        {
            get { return Of(Status.NotInMeeting); }
        }
    }
}
