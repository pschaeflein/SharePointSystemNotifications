using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Schaeflein.Community.SystemNotification.Data
{
	public class NotificationListItem
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Status { get; set; }
		public string Text { get; set; }
		public string InfoLink { get; set; }
		public string Scope { get; set; }

		public NotificationListItem() { }

		public NotificationListItem(SPListItem item)
		{
			this.Id      = Utility.NullSafeInt32(item["ID"]);
			this.Title   = Utility.NullSafeString(item["Title"]);
			this.Status  = Utility.NullSafeString(item[Constants.SystemNotificationList.Columns.MessageStatus]);
			this.Text = Utility.NullSafeString(item[Constants.SystemNotificationList.Columns.MessageText]);
			this.Scope   = Utility.NullSafeString(item[Constants.SystemNotificationList.Columns.MessageScope]);

			string urlRaw = Utility.NullSafeString(item[Constants.SystemNotificationList.Columns.MessageInfoLink]);
			if (!String.IsNullOrEmpty(urlRaw))
			{

				SPFieldUrl urlField = item.Fields[Constants.SystemNotificationList.Columns.MessageInfoLinkId] as SPFieldUrl;
				if (urlField != null)
				{
					this.InfoLink = urlField.GetFieldValueAsHtml(urlRaw);
				}
			}
		}
	}
}
