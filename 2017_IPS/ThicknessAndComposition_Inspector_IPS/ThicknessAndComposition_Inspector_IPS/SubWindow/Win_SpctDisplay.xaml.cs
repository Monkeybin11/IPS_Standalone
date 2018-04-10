using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ThicknessAndComposition_Inspector_IPS
{
	/// <summary>
	/// Interaction logic for Win_SpctDisplay.xaml
	/// </summary>
	public partial class Win_SpctDisplay : Window
	{
		public event Action evtCloseWin;
		public Win_SpctDisplay()
		{
			InitializeComponent();


			ucSpctShart.lblTitle.Content = "Intensity";
			ucSpctShart.axisY.Title = "Intensity";
			ucSpctShart.Ysprtor.Step = 5000;
			ucSpctShart.axisY.MaxValue = 61000;
			ucSpctShart.axisY.MinValue = 0;

			ucSpctShart.srsMain.DataLabels = false;
			ucSpctShart.srsMain.PointGeometrySize = 0;
			ucSpctShart.srsMain.Fill = Brushes.Transparent;




		}

		private void Window_Closing( object sender , System.ComponentModel.CancelEventArgs e )
		{
			e.Cancel = true;
			this.Visibility = Visibility.Hidden;
			evtCloseWin();
		}
	}
}
