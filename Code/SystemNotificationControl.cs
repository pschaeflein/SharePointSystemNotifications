using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Web.UI;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;
using System.Web.Script.Serialization;


namespace Schaeflein.Community.SystemNotification.Controls
{
	[Guid("d94c3bb0-bd99-435a-a484-177e2490ee2d")]
	class SystemNotificationControl : System.Web.UI.Control
	{
		string messageListJSON = String.Empty;
		bool renderScript = false;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			try
			{
				List<Data.NotificationListItem> notifications = null;

				string homePageUrl = Utility.ConcatUrls(new string[] { 
																SPContext.Current.Web.Url,
																SPContext.Current.Site.RootWeb.RootFolder.WelcomePage});
				bool onHomePage = homePageUrl == Page.Request.Url.ToString();


				using (Data.NotificationListRepository repos = new Data.NotificationListRepository(SPContext.Current.Site.RootWeb.Url))
				{
					notifications = repos.GetActiveNotificationMessages();
				}

				if (notifications != null && notifications.Count > 0)
				{
					List<NotificationMessage> messages = new List<NotificationMessage>();

					foreach (Data.NotificationListItem item in notifications)
					{
						if (item.Scope == Constants.MessageScopes.HomePageOnly && !onHomePage)
						{
							continue;
						}

						NotificationMessage message = new NotificationMessage();
						message.Title = item.Title;

						if (!String.IsNullOrEmpty(item.InfoLink))
						{
							message.Text = String.Format(Constants.NotificationTextFormat, item.Text, item.InfoLink);
						}
						else
						{
							message.Text = item.Text;
						}

						switch (item.Status)
						{
							case "Success":
								message.Color = "green";
								break;
							case "Important":
								message.Color = "yellow";
								break;
							case "Very Important":
								message.Color = "red";
								break;
							default:
								message.Color = "blue";
								break;
						}

						messages.Add(message);
					}

					JavaScriptSerializer ser = new JavaScriptSerializer();
					messageListJSON = ser.Serialize(messages);
					renderScript = true;
				}
			}
			catch (Exception ex)
			{
				// TODO: Log this
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			try
			{

				if (renderScript)
				{
					this.Page.ClientScript.RegisterHiddenField(this.ClientID + "notificationList", messageListJSON);

					//page.ClientScript.RegisterStartupScript(type, scriptBlockId, Utility.BuildDelayedExecutionScript(strFileKey, strMethod, scriptSafeArgs), true);

					ScriptLink.Register(this.Page, "schaeflein.community.systemnotification/json2.js", false);
					ScriptLink.Register(this.Page, "schaeflein.community.systemnotification/systemnotifications.js", false);

					Utility.RegisterDelayedExecutionScript(this.Page, this.GetType(),
						"schaeflein.community.systemnotification/systemnotifications.js", "INIT_KEY",
						"Schaeflein.Community.SystemNotification.init",
						new string[] { "'" + this.ClientID + "'" });
				}
			}
			catch (Exception ex)
			{
				// TODO: Log this
			}

		}

	}
}
