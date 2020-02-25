using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace WowSuite.Utils.ProgressBar
{
	/// <summary>
	/// Interaction logic for ProgressBar.xaml
	/// </summary>
	public partial class ProgressBar : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		
		public int Percentage { get; set; }
		public double BarWidth { get; set; }
		public double BorderLeft{ get; set; }
		public double NewBarWidth { get; set; }
		public double NewBorderLeft{ get; set; }
		
		public ProgressBar()
		{
			//Initialize components + bind us as context
			this.InitializeComponent();
			DataContext = this;
			
			//Set to initial width
			this.BarWidth = 1;
			this.BorderLeft = -79.25;
			this.NewBarWidth = this.BarWidth;
			this.NewBorderLeft = this.BorderLeft;
			NotifyPropertyChanged("BarWidth");
			NotifyPropertyChanged("BorderLeft");
			NotifyPropertyChanged("NewBarWidth");
			NotifyPropertyChanged("NewBorderLeft");
			
			//Initialize storyboard, animation bugs otherwise on first call
			Storyboard sb = this.FindResource("SetProgress") as Storyboard;
			sb.Stop();
		}
		
		private void NotifyPropertyChanged(string propertyName)
		{
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
		
		public void SetPercentage(int percent)
		{
			Storyboard sb = this.FindResource("SetProgress") as Storyboard;
			
			double width = ( this.ActualWidth / 100 ) * percent;
			this.NewBarWidth = width;
			this.NewBorderLeft = width - 80.25;
			this.Percentage = percent;
			
			NotifyPropertyChanged("NewBarWidth");
			NotifyPropertyChanged("NewBorderLeft");
			
			sb.Completed += sb_Completed;
			sb.Begin(this, HandoffBehavior.Compose);
		}
		
		public void Complete()
		{
			SetPercentage(100);
			Storyboard sb = this.FindResource("Complete") as Storyboard;
			sb.Begin();
		}

        public void NotNeeded()
        {
            SetPercentage(0);
        }

		private void sb_Completed(object sender, System.EventArgs e)
		{
			this.BarWidth = this.NewBarWidth;
			this.BorderLeft = this.NewBorderLeft;
			NotifyPropertyChanged("BarWidth");
			NotifyPropertyChanged("BorderLeft");
			Storyboard sb = this.FindResource("SetProgress") as Storyboard;
			sb.Completed -= sb_Completed;
			sb.Stop();
		} 
	}
}