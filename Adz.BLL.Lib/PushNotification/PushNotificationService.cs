using Adz.DAL.EF;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PushSharp;
using PushSharp.Android;
using PushSharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.Lib
{
    public class PushNotificationService
    {
        public const string GCM_API_SERVER_KEY = "AIzaSyDWanZVq5Xr_1tX3yKC8m1cJTay3VAyatQ";

        public static void PushNotification(string GCMDeviceId, PNobject pnobject)
        {
            //Create our push services broker
            var push = new PushBroker();

            //Wire up the events for all the services that the broker registers
            push.OnNotificationSent += NotificationSent;
            push.OnChannelException += ChannelException;
            push.OnServiceException += ServiceException;
            push.OnNotificationFailed += NotificationFailed;
            push.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
            push.OnDeviceSubscriptionChanged += DeviceSubscriptionChanged;
            push.OnChannelCreated += ChannelCreated;
            push.OnChannelDestroyed += ChannelDestroyed;


            push.RegisterGcmService(new GcmPushChannelSettings(GCM_API_SERVER_KEY));
            
            var pnobject_json = JsonConvert.SerializeObject(pnobject);

            push.QueueNotification(new GcmNotification().ForDeviceRegistrationId(GCMDeviceId)
                                  .WithJson(pnobject_json));


            //Stop and wait for the queues to drains before it dispose 
            push.StopAllServices(waitForQueuesToFinish: true);
        }

        #region pushbroker events
        //Currently it will raise only for android devices
        static void DeviceSubscriptionChanged(object sender,
        string oldSubscriptionId, string newSubscriptionId, INotification notification)
        {
            //Do something here
        }

        //this is raised when a notification is successfully sent
        static void NotificationSent(object sender, INotification notification)
        {
            //
        }

        //this is raised when a notification is failed due to some reason
        static void NotificationFailed(object sender,
        INotification notification, Exception notificationFailureException)
        {
            using (var DBContext = new TheAdzEntities())
            {
                int pn_log_id = 0;

                var gcmNotification = notification as GcmNotification;
                if (gcmNotification != null)
                {
                    var json = JObject.Parse(gcmNotification.JsonData);

                    pn_log_id = (int)json["pn_log_id"];
                }

                var PushNotificationLog = from d in DBContext.PushNotificationLogs
                                          where d.id == pn_log_id
                                          select d;

                if (PushNotificationLog.FirstOrDefault() != null)
                {
                    var v = PushNotificationLog.FirstOrDefault();
                    v.remarks += string.Join(",", gcmNotification.RegistrationIds) + " : " + notificationFailureException.Message + "\n";
                    DBContext.SaveChanges();
                }
            }            
        }

        //this is fired when there is exception is raised by the channel
        static void ChannelException
            (object sender, IPushChannel channel, Exception exception)
        {
            //Do something here
        }

        //this is fired when there is exception is raised by the service
        static void ServiceException(object sender, Exception exception)
        {
            //Do something here
        }

        //this is raised when the particular device subscription is expired
        static void DeviceSubscriptionExpired(object sender,
        string expiredDeviceSubscriptionId,
            DateTime timestamp, INotification notification)
        {
            //Do something here
        }

        //this is raised when the channel is destroyed
        static void ChannelDestroyed(object sender)
        {
            //Do something here
        }

        //this is raised when the channel is created
        static void ChannelCreated(object sender, IPushChannel pushChannel)
        {
            //Do something here
        }
        #endregion
    }

    public class PNobject
    {
        public PNobject()
        {
            badge = 7;
        }
        public string alert { get; set; }
        public int badge { get; set; }

        public int? pn_log_id { get; set; }

    }
}
