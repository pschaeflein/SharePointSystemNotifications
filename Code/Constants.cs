using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schaeflein.Community.SystemNotification
{
	internal static class Constants
	{
		internal const string NotificationTextFormat = "{0} -- for more information: {1}";

		internal static class MessageScopes
		{
			internal const string HomePageOnly = "Home page only";
			internal const string AllPages     = "All pages";
		}

		internal static class SystemNotificationList
		{
			internal const string Name = "System Notifications";

			internal static class Columns
			{
				internal const string MessageStatus    = "MessageStatus";
				internal const string MessageText      = "MessageText";
				internal const string MessageScope     = "MessageScope";
				internal const string MessageInfoLink  = "MessageInfoLink";
				internal static Guid MessageInfoLinkId = new Guid("{6a01a72c-8ed8-49f9-a23b-0c84fb9d85db}");
			}
		}
	}
}
