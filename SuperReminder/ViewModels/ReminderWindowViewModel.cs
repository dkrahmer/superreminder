using System.Collections.ObjectModel;
using SuperReminder.Models;

namespace SuperReminder.ViewModels
{
    public class ReminderWindowViewModel
    {
        public ReminderWindowViewModel()
        {
            Reminders = new ThreadSafeDispatchedObservableCollection<ReminderInfo>();
        }
        public ThreadSafeDispatchedObservableCollection<ReminderInfo> Reminders { get; set; }
    }
}
