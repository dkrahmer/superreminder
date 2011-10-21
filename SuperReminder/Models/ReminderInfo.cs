using System;
using Microsoft.Office.Interop.Outlook;

namespace SuperReminder.Models
{
    public class ReminderInfo
    {
        public AppointmentItem Appointment { get; set; }
        
        public ReminderInfo(AppointmentItem appointmentItem = null)
        {
            if(appointmentItem == null)
                return;
            Appointment = appointmentItem;
            StartTime = appointmentItem.StartInStartTimeZone;
            Subject = appointmentItem.Subject;
            Location = appointmentItem.Location;
            Body = appointmentItem.Body;
            Duration = TimeSpan.FromMinutes(appointmentItem.Duration);
        }

        public DateTime StartTime { get; set; }
        public string Subject { get; set; }
        public string Location { get; set; }
        public string Body { get; set; }
        public TimeSpan Duration { get; set; }

        public void SnoozeBefore(int minutes)
        {
            if (Appointment == null)
                return;
            if (Appointment.ReminderSet)
            {
                Appointment.ReminderMinutesBeforeStart = minutes;
                Appointment.Save();
            }

        }

        public void Dismiss()
        {
            if (Appointment == null)
                return;
            if (Appointment.ReminderSet)
            {
                Appointment.ReminderSet = false;
                Appointment.Save();
            }
        }
    }
}
