using System;
using System.Collections.Generic;
using System.Text;

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
           var sintromItem = DBHelper.Instance.GetSintromItemFromDate(DateTime.Now);
            sintromItem.
        }

    }
}
