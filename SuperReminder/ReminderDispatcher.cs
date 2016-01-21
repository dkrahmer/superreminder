using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;
using Microsoft.Office.Interop.Outlook;
using SuperReminder.Models;
using SuperReminder.ViewModels;
using Action = System.Action;
using Exception = System.Exception;

namespace SuperReminder
{
    public class ReminderDispatcher
    {
        private class ThreadContext
        {
            public Thread Thread;
            public ReminderWindow Window;
            public ManualResetEvent IsReady;
            public Screen Screen;
        }

        // Handle changes to SystemEvents.DisplaySettingsChanged 

        private readonly List<ThreadContext> _contexts = new List<ThreadContext>();
        private readonly ReminderWindowViewModel _viewModel = new ReminderWindowViewModel();

        public void DispatchReminder(ReminderInfo reminder)
        {
            if (reminder == null)
                return;

            if (_contexts.Count == 0)
            {
                CreateContexts();
                foreach (var isReady in _contexts.Select(x => x.IsReady))
                {
                    isReady.WaitOne(new TimeSpan(0, 0, 5));
                }
            }
            
            _viewModel.Reminders.Add(reminder);
        }

        private void CreateContexts()
        {
            ReminderWindow.ResetIsClosing();
            Screen[] screens = Screen.AllScreens;
            foreach (var screen in screens)
            {
                var context = new ThreadContext
                                  {
                                      Thread = new Thread(RunWindow),
                                      IsReady = new ManualResetEvent(false),
                                      Screen = screen
                                  };
                context.Thread.SetApartmentState(ApartmentState.STA);
                context.Thread.Start(context);
                _contexts.Add(context);
            }
        }

        public void SnoozeReminder(ReminderInfo reminder)
        {
            reminder.SnoozeBefore(1);
            _viewModel.Reminders.Remove(reminder);
            CloseWindowsIfThereAreNoReminders();
        }

        public void DismissReminder(ReminderInfo reminder)
        {
            reminder.Dismiss();
            _viewModel.Reminders.Remove(reminder);
            CloseWindowsIfThereAreNoReminders();
        }

        private void CloseWindowsIfThereAreNoReminders()
        {
            if (_viewModel.Reminders.Count() == 0)
            {
                var window = _contexts.First().Window;
                if (window != null)
                    window.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(window.Close));
            }
        }

        private object WindowCreationLock = new object();
        private void RunWindow(object contextObj)
        {
            try
            {
                var context = (ThreadContext)contextObj;
                var screen = context.Screen;
                lock (WindowCreationLock) { 
                context.Window = new ReminderWindow(this)
                {
                    Top = screen.Bounds.Top,
                    Left = screen.Bounds.Left,
                    Width = screen.Bounds.Width,
                    Height = screen.Bounds.Height
                };
                }
                context.Window.DataContext = _viewModel;
                context.Window.Show();
                context.Window.Closed += (sender2, e2) =>
                                             {
                                                 context.Window.Dispatcher.InvokeShutdown();
                                                 _contexts.Remove(context);
                                             };
                context.IsReady.Set();
                Dispatcher.Run();
            }
            catch (Exception e)
            {
                   // TODO: Log exception
            }
            
        }

        public void Wait()
        {
            foreach (var thread in _contexts.Select(x => x.Thread))
            {
                thread.Join();
            }
        }
    }
}
