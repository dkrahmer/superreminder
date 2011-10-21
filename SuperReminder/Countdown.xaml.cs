using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SuperReminder
{
    /// <summary>
    ///   Interaction logic for Countdown.xaml
    /// </summary>
    public partial class Countdown : UserControl
    {
        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof (DateTime),
                                                                                             typeof (Countdown));
        private DateTime _time;

        public Countdown()
        {
            InitializeComponent();
            var timer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 1)};
            timer.Tick += TimerTick;
            timer.Start();
            _time = DateTime.Now + TimeSpan.FromMinutes(64);
            UpdateTime();
        }

        public DateTime Time
        {
            get { return _time; }
            set
            {
                _time = value;
                UpdateTime();
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            UpdateTime();
        }

        private void UpdateTime()
        {
            var timeLeft = new TimeSpan(0);
            if (Time > DateTime.Now)
                timeLeft = Time - DateTime.Now;
            TimeLeftTextBlock.Text = String.Format("{0:00}:{1:00}", timeLeft.TotalHours, timeLeft.Seconds);
        }
    }
}