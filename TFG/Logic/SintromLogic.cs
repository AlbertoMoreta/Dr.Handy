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
            var sintromItems = DBHelper.Instance.GetSintromItemFromDate(DateTime.Now);
            if (sintromItems.Count > 0) {
                var sintromItem = sintromItems[0];
                var title = HealthModulesInfo.GetStringFromResourceName("sintrom_name");
                var description =
                    string.Format(HealthModulesInfo.GetStringFromResourceName("sintrom_notification_description"),
                        sintromItem.Fraction, sintromItem.Medicine);

                return new NotificationItem(title, description, true);
            }

            return null;
        }

    }
}
