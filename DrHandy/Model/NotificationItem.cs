
namespace DrHandy.Model {
    public class NotificationItem {

        public string Title { get; set; }
        public string Description { get; set; }
        public bool Audio { get; set; }

        public NotificationItem(string title, string description, bool audio) {
            Title = title;
            Description = description;
            Audio = audio;
        }
    }
}
