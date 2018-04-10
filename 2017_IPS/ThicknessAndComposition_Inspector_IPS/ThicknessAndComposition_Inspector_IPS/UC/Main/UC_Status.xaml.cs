﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ThicknessAndComposition_Inspector_IPS
{
	/// <summary>
	/// Interaction logic for UC_Status.xaml
	/// </summary>
	public partial class UC_Status : UserControl
	{
		

		public UC_Status()
		{
			InitializeComponent();
		}

		public void DisplayConnection( bool stg , bool spct )
		{
			brdStgConnect.Background = stg ? Brushes.LimeGreen
											: Brushes.OrangeRed;

			brdSpctConnect.Background = spct ? Brushes.LimeGreen
											: Brushes.OrangeRed;
		}

		public void DisplayScanStatus( string msg )
		{
			lblProgress.Content = msg;
		}

	}
}
