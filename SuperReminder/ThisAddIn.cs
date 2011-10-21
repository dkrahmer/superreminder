using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Office.Interop.Outlook;
using SuperReminder.Models;

namespace SuperReminder
{
    public partial class ThisAddIn
    {
        readonly ReminderDispatcher _dispatcher = new ReminderDispatcher(); 

        private void ThisAddInStartup(object sender, EventArgs e)
        {
            Application.Reminder += ApplicationReminder;
        }


        public void ApplicationReminder(object item)
        {
            var appointment = item as AppointmentItem;
            if(appointment == null)
                return;

            _dispatcher.DispatchReminder(new ReminderInfo(appointment));
        }


        private void ThisAddIn_Shutdown(object sender, EventArgs e)
        {
        }


        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            Startup += ThisAddInStartup;
            Shutdown += ThisAddIn_Shutdown;
        }
        
        #endregion
    }
}
