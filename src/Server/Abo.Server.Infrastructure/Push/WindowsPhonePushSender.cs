using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Abo.Server.Application;
using Abo.Server.Application.Contracts;

namespace Abo.Server.Infrastructure.Push
{
    public class WindowsPhonePushSender : IPushNotificator
    {
        public bool Send(string pushUri, string text)
        {
            string title = "Astral Battles";
            string subtitle = text;

            HttpWebRequest sendNotificationRequest = (HttpWebRequest)WebRequest.Create(pushUri);

            // Create an HTTPWebRequest that posts the toast notification to the Microsoft Push Notification Service.
            // HTTP POST is the only method allowed to send the notification.
            sendNotificationRequest.Method = "POST";

            // The optional custom header X-MessageID uniquely identifies a notification message. 
            // If it is present, the same value is returned in the notification response. It must be a string that contains a UUID.
            // sendNotificationRequest.Headers.Add("X-MessageID", "<UUID>");

            // Create the toast message.
            string toastMessage = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<wp:Notification xmlns:wp=\"WPNotification\">" +
               "<wp:Toast>" +
                    "<wp:Text1>" + title + "</wp:Text1>" +
                    "<wp:Text2>" + string.Format(subtitle) + "</wp:Text2>" +
                    "<wp:Param>?NavigatedFrom=ToastNotification</wp:Param>" +
                    //"<wp:Sound>ToastSound.wav</wp:Sound>" +
               "</wp:Toast> " +
            "</wp:Notification>";

            // Set the notification payload to send.
            byte[] notificationMessage = Encoding.Default.GetBytes(toastMessage);

            // Set the web request content length.
            sendNotificationRequest.ContentLength = notificationMessage.Length;
            sendNotificationRequest.ContentType = "text/xml";
            sendNotificationRequest.Headers.Add("X-WindowsPhone-Target", "toast");
            sendNotificationRequest.Headers.Add("X-NotificationClass", "2");


            using (Stream requestStream = sendNotificationRequest.GetRequestStream())
            {
                requestStream.Write(notificationMessage, 0, notificationMessage.Length);
            }

            // Send the notification and get the response.
            HttpWebResponse response = (HttpWebResponse)sendNotificationRequest.GetResponse();
            string notificationStatus = response.Headers["X-NotificationStatus"];//Received is Ok
            string notificationChannelStatus = response.Headers["X-SubscriptionStatus"];
            string deviceConnectionStatus = response.Headers["X-DeviceConnectionStatus"];//Connected is Ok

            return notificationStatus == "Received";
            // Display the response from the Microsoft Push Notification Service.  
            // Normally, error handling code would be here. In the real world, because data connections are not always available,
            // notifications may need to be throttled back if the device cannot be reached.
            //TextBoxResponse.Text = notificationStatus + " | " + deviceConnectionStatus + " | " + notificationChannelStatus;
        }
    }
}
