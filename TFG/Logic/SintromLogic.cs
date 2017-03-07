using System;
using System.Collections.Generic;
using System.Text;
using TFG.Model;

namespace TFG.Logic {
    public class SintromLogic {

        private static SintromLogic _instance;

        public static SintromLogic Instance() {
            if (_instance == null)  {
                _instance = new SintromLogic();
            }

            return _instance;
        }

        private SintromLogic() { }

        public NotificationItem GetNotificationitem() {
            var controlday = DBHelper.Instance.GetSintromINRItemFromDate(DateTime.Now);
            var title = HealthModulesInfo.GetStringFromResourceName("sintrom_name");
            //Control Day Notification
            if (controlday.Count > 0 && controlday[0].Control) {
                var description =
                    string.Format(HealthModulesInfo.GetStringFromResourceName("sintrom_notification_control_description"));

                return new NotificationItem(title, description, true);
            }

            //Treatment Day Notification
            var sintromItems = DBHelper.Instance.GetSintromItemFromDate(DateTime.Now);
            if (sintromItems.Count > 0) {
                var sintromItem = sintromItems[0];
                var description =
                    string.Format(HealthModulesInfo.GetStringFromResourceName("sintrom_notification_description"),
                        sintromItem.Fraction, sintromItem.Medicine);

                return new NotificationItem(title, description, true);
            } 

            //No Notification
            return null;
        }

    }
}
