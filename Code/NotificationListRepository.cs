using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.SharePoint;

namespace Schaeflein.Community.SystemNotification.Data
{
	public class NotificationListRepository : IDisposable
	{
		private string webUrl;
		private SPSite site;
		private SPWeb web;

		public NotificationListRepository()
		{
			this.webUrl = SPContext.Current.Site.Url;
			Initialize();
		}

		public NotificationListRepository(string webUrl)
		{
			// TODO: validate that the url is an absolute url
			this.webUrl = webUrl;
			Initialize();
		}

		private void Initialize()
		{
			site = new SPSite(this.webUrl);
			web = site.RootWeb;
		}

		public List<NotificationListItem> GetActiveNotificationMessages()
		{
			List<NotificationListItem> results = new List<NotificationListItem>();

			SPList list = null;
			SPView view = null;

			try
			{
				list = web.Lists.TryGetList(Constants.SystemNotificationList.Name);
				view = list.Views["Active Notifications"];

				if (list != null)
				{
					foreach (SPListItem item in list.GetItems(view))
					{
						results.Add(new NotificationListItem(item));
					}
				}

			}
			catch (Exception ex)
			{
				RepositoryException repoEx = new RepositoryException(ex.Message, ex);
				repoEx.DataContextUrl = this.webUrl;
				repoEx.GeneratedCAML = (view == null) ? String.Empty : view.Query;
				throw repoEx;
			}
			return results;
		}

		private static SPQuery BuildNotificationMessageQuery()
		{
			SPQuery query = new SPQuery();
			query.Query = @"<Where>
												<Ne>
													<FieldRef Name='MessageStatus'/>
													<Value Type='Choice'>Closed</Value>
												</Ne>
											</Where>
											<OrderBy>
												<FieldRef Name='Modified' Ascending='FALSE'/>
											</OrderBy>";

			query.ViewFields = @"<FieldRef Name='ID' />
													 <FieldRef Name='Title' />
													 <FieldRef Name='MessageStatus' />
													 <FieldRef Name='MessageText' />
													 <FieldRef Name='MessageScope' />";
			return query;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (web != null)
				{
					web.Dispose();
					web = null;
				}
				if (site != null)
				{
					site.Dispose();
					site = null;
				}
			}
		}
	}
}
