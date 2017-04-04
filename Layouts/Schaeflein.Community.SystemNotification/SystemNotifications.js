/// <reference path="C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\TEMPLATE\LAYOUTS\SP.UI.Dialog.debug.js" />

"use strict";

var Schaeflein = window.Schaeflein || {};

Schaeflein.Community = function () {
	var systemNotification = function () {
		var notificationList = null,

		showStatus = function () {
			var title = null, text = null, color = null;

			for (var i = 0; i < notificationList.length; i++) {

				title = notificationList[i].Title;
				text = notificationList[i].Text;
				color = notificationList[i].Color;

				var statusId = SP.UI.Status.addStatus(title, text);
				SP.UI.Status.setStatusPriColor(statusId, color);
			}
		},

		init = function (id) {
			var hiddenDataControl = document.getElementById(id + "notificationList");
			notificationList = JSON.parse(hiddenDataControl.value);

			SP.SOD.executeOrDelayUntilScriptLoaded(showStatus, "SP.js");
		};

		return {
			notificationList: notificationList,
			init: init
		}

	}();

	return {
		SystemNotification: systemNotification
	}
}();
SP.SOD.notifyScriptLoadedAndExecuteWaitingJobs("schaeflein.community.systemnotification/systemnotifications.js");