using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Threading;

namespace ThicknessAndComposition_Inspector_IPS
{
	/// <summary>
	/// Interaction logic for WIn_SinglePointAnalysis.xaml
	/// </summary>
	
	public partial class WIn_SinglePointAnalysis : Window
	{
		public event Action<double[],int,int> evtScanStart;
		public event Action evtStopSingleScan;
		List<double[]> Spectruns = new List<double[]>();
		List<double[]> Reflectivitys = new List<double[]>();
		List<double> Thicknesses = new List<double>();
		double[] Waves = new double[] { } ;
		List<string> Time = new List<string>();
		bool waveSetted;
		string TempPathInten;
		string TempPathReflect;
		bool IsReady;

		public WIn_SinglePointAnalysis()
		{
			InitializeComponent();
			//ucIntensitiychart.chtLiveLine.
			ucReflectivityChart.axisY.MaxValue = 100;
			ucIntensitiychart.axisY.MaxValue = 60000;
			IsReady = true;
		}

		private void btnSinglePosStart_Click( object sender , RoutedEventArgs e )
		{
			if ( !IsReady ) return;
			IsReady = false;
			Spectruns = new List<double [ ]>();
			Reflectivitys = new List<double [ ]>();
			Thicknesses = new List<double>();
			Waves = new double [ ] { };
			Time = new List<string>();

			TempPathInten = txbTempBackupFile.Text;
			waveSetted = false;
			ucIntensitiychart.Counter = 0;
			ucReflectivityChart.Counter = 0;
			try
			{
				if ( ( bool )ckbTempBackup.IsChecked )
				{
					var path = System.IO.Path.GetFullPath( txbTempBackupFile.Text ).CheckAndCreateDir()
								+"\\"
								+ DateTime.Now.ToString( "yyMMdd__HH;mm;ss" );
					TempPathInten = path + "_Inten.csv";
					TempPathReflect = path + "_Refelct.csv";
					Thread.Sleep( 300 );
				}
				


				evtScanStart(
					new double [ ]
					{
					(double)nudXposSingle.Value,
					(double)nudYposSingle.Value
					} ,
					( int )nudIntervalSingle.Value ,
					( int )nudCountSingel.Value
					);
			}
			catch ( Exception )
			{
				MessageBox.Show( "Setted Temp Save Path is not Valid" );
				throw;
			}
		}

		
		public void ToReadyState()
		{
			IsReady = true;
		}


		public void DrawSignal( IEnumerable<double> spct , IEnumerable<double> reflect , IEnumerable<double> wave ,double thckn , int count)
		{
			Thicknesses.Add( thckn );
			string currentTime = DateTime.Now.ToString( "yyMMdd__HH_mm_ss" );

			Time.Add( currentTime );
			ucIntensitiychart.AddNewSeries( spct , wave , thckn.ToString("N2") );
			lblSingleScanStatus.Dispatcher.BeginInvoke( ( Action )( () => lblSingleScanStatus.Content = count.ToString() ) );
			Spectruns.Add( spct.ToArray() );
			if ( !waveSetted ) Waves = wave.ToArray();

			ucReflectivityChart.AddNewSeries( reflect , wave , thckn.ToString( "N2" )  );
			
			Reflectivitys.Add( reflect.ToArray() );


			bool needbackup = false;
			this.Dispatcher.Invoke( ( Action )( () => needbackup = ( bool )ckbTempBackup.IsChecked ) );
			if ( !waveSetted && needbackup )
			{
				var stb = new StringBuilder()
					.Act( x => x.Append("WaveLen," + wave.Select( y => y.ToString())
														.Aggregate((f,s) => f + ',' + s)
												 + Environment.NewLine));
				File.WriteAllText( TempPathInten , stb.ToString() );
				File.WriteAllText( TempPathReflect , stb.ToString() );
				waveSetted = true;
			}

			
			if ( needbackup )
			{
				var stbInten = new StringBuilder()
					.Act( x => x.Append( currentTime + ',' ));
				var stbReflc = new StringBuilder()
					.Act( x => x.Append( currentTime + ',' ) );

				File.AppendAllText( TempPathInten , 
					stbInten.Act( x => x.Append( 
						spct.Select( y => y.ToString() )
							.Aggregate( ( f , s ) => f + ',' + s ) ) ).ToString()
						+ Environment.NewLine );
				File.AppendAllText( TempPathReflect ,
					stbReflc.Act( x => x.Append(
						reflect.Select( y => y.ToString() )
							.Aggregate( ( f , s ) => f + ',' + s ) ) ).ToString() 
						+ Environment.NewLine );
			}

		}

		

		private void btnSeriesClear_Click( object sender , RoutedEventArgs e )
		{
			ucReflectivityChart.ClearSeries();
			ucIntensitiychart.ClearSeries();
		}

		private void Window_Closing( object sender , System.ComponentModel.CancelEventArgs e )
		{
			IsReady = true;
			e.Cancel = true;
			this.Visibility = Visibility.Hidden;
		}

		private void btnSaveSingleScan_Click( object sender , RoutedEventArgs e )
		{
			SaveFileDialog sfd = new SaveFileDialog();
			if (sfd.ShowDialog() == true)
			{
				var spctpath = sfd.FileName + "_Spectrum.csv";
				var rflctpath = sfd.FileName + "_Reflectivity.csv";

				StringBuilder stbspcts = new StringBuilder();
				StringBuilder stbrflct = new StringBuilder();
				stbspcts.Append( "WaveLen," + Time.Select( x => x.ToString() ).Aggregate( ( f , s ) => f + ',' + s ) + Environment.NewLine );
				stbrflct.Append( "WaveLen," + Time.Select( x => x.ToString() ).Aggregate( ( f , s ) => f + ',' + s ) + Environment.NewLine );

				stbspcts.Append( "Thickness," + Thicknesses.Select( x => x.ToString() ).Aggregate( ( f , s ) => f + ',' + s ) + Environment.NewLine );
				stbrflct.Append( "Thickness," + Thicknesses.Select( x => x.ToString() ).Aggregate( ( f , s ) => f + ',' + s ) + Environment.NewLine );

				var lines = Spectruns [ 0 ].GetLength( 0 );

				for ( int i = 0 ; i < lines ; i++ )
				{
					stbspcts.Append(
						Waves [ i ].ToString() + ',' +
						Spectruns.Select( x => x [ i ].ToString() ).Aggregate( ( f , s ) => f + "," + s ) +
						Environment.NewLine );

					stbrflct.Append(
						Waves [ i ].ToString() + ',' +
						Reflectivitys.Select( x => x [ i ].ToString() ).Aggregate( ( f , s ) => f + "," + s ) +
						Environment.NewLine );
				}
				File.WriteAllText( spctpath,stbspcts.ToString() );
				File.WriteAllText( rflctpath,stbrflct.ToString() );
			}
		}

		private void btnStop_Click( object sender , RoutedEventArgs e )
		{
			evtStopSingleScan();
		}
	}
}
