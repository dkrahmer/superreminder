using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using SuperReminder.Models;
using SuperReminder.ViewModels;

namespace SuperReminder
{
	/// <summary>
	///   Interaction logic for ReminderWindow.xaml
	/// </summary>
	public partial class ReminderWindow
	{
		private readonly ReminderWindowViewModel _viewModel;
		private readonly ReminderDispatcher _reminderDispatcher;

		public ReminderWindow(ReminderDispatcher reminderDispatcher)
		{
			InitializeComponent();
			_reminderDispatcher = reminderDispatcher;
			_viewModel = (ReminderWindowViewModel)DataContext;
			CloseAll += ReminderWindowCloseAll;
		}

		private static event ClossAllHandler CloseAll;
		private volatile static bool _isClossing;

		public static void ResetIsClosing()
		{
			_isClossing = false;
		}

		private void ReminderWindowCloseAll(object sender, EventArgs args)
		{
			if (sender != this)
			{
				Dispatcher.Invoke(DispatcherPriority.Normal, new Action(Close));
			}
		}

		private void ReminderWindowFormClosing(object sender, CancelEventArgs e)
		{
			if (CloseAll != null && !_isClossing)
			{
				_isClossing = true;
				CloseAll(this, null);
			}
		}

		public void AddReminder(ReminderInfo reminder)
		{
			Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => _viewModel.Reminders.Add(reminder)));
		}

		public void RemoveReminder(ReminderInfo reminder)
		{
			Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => _viewModel.Reminders.Remove(reminder)));
		}

		private void Snooze0Click(object sender, RoutedEventArgs e)
		{
			var reminder = ((Button)sender).DataContext as ReminderInfo;
			if (reminder == null)
				throw new Exception("Bad data context");

			// TODO: Snooze the reminder and remove it from all the windows
			_reminderDispatcher.SnoozeReminder(reminder, 0);
		}

		private void Snooze1Click(object sender, RoutedEventArgs e)
		{
			var reminder = ((Button)sender).DataContext as ReminderInfo;
			if (reminder == null)
				throw new Exception("Bad data context");

			// TODO: Snooze the reminder and remove it from all the windows
			_reminderDispatcher.SnoozeReminder(reminder, 1);
		}

		private void Snooze3Click(object sender, RoutedEventArgs e)
		{
			var reminder = ((Button)sender).DataContext as ReminderInfo;
			if (reminder == null)
				throw new Exception("Bad data context");

			// TODO: Snooze the reminder and remove it from all the windows
			_reminderDispatcher.SnoozeReminder(reminder, 3);
		}

		private void Snooze5Click(object sender, RoutedEventArgs e)
		{
			var reminder = ((Button)sender).DataContext as ReminderInfo;
			if (reminder == null)
				throw new Exception("Bad data context");

			// TODO: Snooze the reminder and remove it from all the windows
			_reminderDispatcher.SnoozeReminder(reminder, 5);
		}

		private void Snooze10Click(object sender, RoutedEventArgs e)
		{
			var reminder = ((Button)sender).DataContext as ReminderInfo;
			if (reminder == null)
				throw new Exception("Bad data context");

			// TODO: Snooze the reminder and remove it from all the windows
			_reminderDispatcher.SnoozeReminder(reminder, 10);
		}

		private void Snooze15Click(object sender, RoutedEventArgs e)
		{
			var reminder = ((Button)sender).DataContext as ReminderInfo;
			if (reminder == null)
				throw new Exception("Bad data context");

			// TODO: Snooze the reminder and remove it from all the windows
			_reminderDispatcher.SnoozeReminder(reminder, 15);
		}

		private void DismissClick(object sender, RoutedEventArgs e)
		{
			var reminder = ((Button)sender).DataContext as ReminderInfo;
			if (reminder == null)
				throw new Exception("Bad data context");

			// TODO: Dismiss the reminder and remove it from all the windows
			_reminderDispatcher.DismissReminder(reminder);
		}
	}

	internal delegate void ClossAllHandler(object sender, EventArgs args);
}