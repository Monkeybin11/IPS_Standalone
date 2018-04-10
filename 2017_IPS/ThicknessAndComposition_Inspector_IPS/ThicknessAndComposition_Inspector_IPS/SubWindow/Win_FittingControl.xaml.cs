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
using SpeedyCoding;
using ModelLib.AmplifiedType;
using static ModelLib.AmplifiedType.Handler;
using ThicknessAndComposition_Inspector_IPS_Core;
using Fitting_Core;

namespace ThicknessAndComposition_Inspector_IPS
{
	using System.Windows.Forms;
	using static Fitting_Core.DataLoader;
	using static Core_Fitting;

	/// <summary>
	/// Interaction logic for Win_FittingControl.xaml
	/// </summary>
	public partial class Win_FittingControl : Window
	{
		Maybe<string> Ipspath;
		Maybe<string> Klapath;
		Maybe<List<Fitting_Core.IpsDataSet>> LoadedDatas;

		public Win_FittingControl()
		{
			InitializeComponent();
			Reset();
		}

		private void btnLoad_Click( object sender , RoutedEventArgs e )
		{
			OpenFileDialog ofd = new OpenFileDialog();
			MessageBox.Show( "Select Ips Scan Result Data Folder" );
			if ( ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK )
			{
				Ipspath = Just( ofd.FileName );

				MessageBox.Show( "Select KLA Scan Result Data Folder" );
				if ( ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK )
				{
					Klapath = Just( ofd.FileName );

					var temp = Ipspath.MGetEachDataSetWith( Klapath );
					LoadedDatas = Ipspath.MGetEachDataSetWith( Klapath );
					LoadedDatas.Lift( CreateModel );
					return;
				}
				else Klapath = None;
			}
			else Ipspath = None;
			
		}

		private void btnStart_Click( object sender , RoutedEventArgs e )
		{
			lblError.Content = Ipspath.MGetEachDataSetWith( Klapath )
									 .Lift( UpdateModel )
									 .Lift(CalcMSE)
									 .Match(
										() => "Error" ,
										x => x.ToString());

		}

		private void btnLoadModel_Click( object sender , RoutedEventArgs e )
		{
			MessageBox.Show( "Select Model File" );
			OpenFileDialog ofd = new OpenFileDialog();
			if ( ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK )
				LoadModel( ofd.FileName );
		}

		private void btnSaveModel_Click( object sender , RoutedEventArgs e )
		{
			SaveFileDialog ofd = new SaveFileDialog();
			if ( ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK )
				SaveModel(ofd.FileName);
		}

		private void btnEvaluate_Click( object sender , RoutedEventArgs e )
		{
			lblError.Content = CalcMSE( Regr );
		}

		private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e )
		{
			e.Cancel = true;
			this.Visibility = Visibility.Hidden;
		}
	}
}
