using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThicknessAndComposition_Inspector_IPS_Core;
using ThicknessAndComposition_Inspector_IPS_Data;
using SpeedyCoding;
using System.Threading;
using System.IO;
using static ModelLib.AmplifiedType.Handler; 
using ModelLib.AmplifiedType;

namespace ThicknessAndComposition_Inspector_IPS
{
	using static ModelLib.AmplifiedType.Handler;
	using static ThicknessAndComposition_Inspector_IPS_Core.Core_Helper;
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		
		IPSCore Core { get; set; }
		Win_Config WinConfig;
		Win_SpctDisplay WinSpct;
		WIn_SinglePointAnalysis WinSingleScan;
		Win_FittingControl WinFitting;
		bool CoreRunning;

		#region Load Close
		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;
		}
		private void Window_Loaded( object sender , RoutedEventArgs e )
		{
			ucSpectrum.lblTitle.Content = "Raw Spectrum";
			ucReflectivity.lblTitle.Content = "Reflectivity";

			WinConfig = new Win_Config();
			WinSpct = new Win_SpctDisplay();
			WinSingleScan = new WIn_SinglePointAnalysis();
			WinFitting = new Win_FittingControl();
			ucLSMenu.evtBtn += new BtnEvt( LeftSideBtn );

			Core = new IPSCore()
				.Act( x => x.evtConnection += ucLSStatus.DisplayConnection )
				.Act( x => x.evtSpectrum +=  ucSpectrum.UpdateSeries  )
				.Act( x => x.evtRefleectivity += ucReflectivity.UpdateSeries ) 
				.Act( x => x.evtSngSignal +=WinSingleScan.DrawSignal ) 
				.Act( x => x.evtSingleMeasureComplete += WinSingleScan.ToReadyState ) 
				.Act( x => x.Connect());

			WinConfig
				.Act( x => x.evtStgSpeedSetChange += Core.SetHWInternalParm )
				.Act( x => x.evtClose += () => this.IsEnabled = true );
			WinSpct 
				.Act( x => x.evtCloseWin += () => { Core.FlgAutoUpdate = false; FlgSpctDisplay = false; } ) ;

			WinSingleScan 
				.Act( x => x.evtScanStart += Core.StartManualRunEvent ) 
				.Act( x => x.evtStopSingleScan += () => Core.FlgCoreSingleScan = false);

			

			Config2UI( Core.Config );
			if ( !Core.OpLoadAbsReflecDatas()
				|| !Core.OpPickWaveIdx()
				|| !Core.OpPickReflectFactorIdx() )
				MessageBox.Show( "Spectrometer Problem. Transfer wavelength fail.  Please restart spectrometer " );
					
			Core.StartBackgroundTask();

			UI_Init();
		}

		private void UI_Init()
		{
			ucSpectrum.UpdateSeries( new double [ ] { 1 , 1100 , 110000 , 110000 } , new double [ ] { 1 , 2 , 3 , 4 } );
			ucReflectivity.UpdateSeries( new double [ ] { 1 , 1100 , 110000 , 110000 } , new double [ ] { 1 , 2 , 3 , 4 } );

			Action<UC_LiveLineChart> CommonSetting
				= src
				=> src.srsMain.Act( x => x.DataLabels = false)
								.Act( x=> x.PointGeometrySize = 0 )
								.Act( x=> x.Fill = Brushes.Transparent);

			ucSpectrum.lblTitle.Content = "Intensity";
			ucSpectrum.axisY.Title = "Intensity";
			ucSpectrum.Ysprtor.Step = 5000;
			ucSpectrum.axisY.MaxValue = 61000;
			ucSpectrum.axisY.MinValue = 0;
			CommonSetting( ucSpectrum );


			ucReflectivity.lblTitle.Content = "Reflectivity";
			ucReflectivity.axisY.Title = "Reflectivity";
			ucReflectivity.Ysprtor.Step = 10;
			ucReflectivity.axisY.MaxValue = 110;
			ucReflectivity.axisY.MinValue = 0;
			CommonSetting( ucReflectivity );

			ucMapDisplay.imgScale.ImageSource = CreateScalebar().ToBitmapSource();
		}




		private void Window_Closed( object sender , EventArgs e )
		{
			Core.Act( x => x.Config = UI2IpsConfig() )
				.Act( x => x.SaveConfig( x.ConfigFullPath ) )
				.Act( x => x.SavePrcConfig( x.PrcConfigFullPath ) );
			WinConfig.Close();
			Environment.Exit( Environment.ExitCode );
		}

		void UpDownLimit()
		{
			//ToDo : UpDown Limit Setting  
		}


		#endregion

		bool FlgSpctDisplay;
		public void FileMenuClick( object sender , RoutedEventArgs e )
		{
			SaveFileDialog sfd = new SaveFileDialog();
			if ( CoreRunning ) return;
			CoreRunning = true;
			var master = sender as MenuItem;
			switch ( master.Name )
			{
				case "menuSaveResultonly":
					if ( sfd.ShowDialog() == true )
						Core.SaveResult( sfd.FileName + "_Result.csv" , Core.ResultData );
						break;
				case "menuSaveRawonly":
					if ( sfd.ShowDialog() == true )
					{
						Core.SaveRaw(
							sfd.FileName + "_Raw.csv" ,
							Core.ResultData ,
							Core.SelectedWaves ,
							Core.Darks ,
							Core.Refs ,
							Core.SelectedReflctFactors );

						Core.SaveRawReflectivity(
								sfd.FileName + "_Reflectivity.csv" ,
								Core.ResultData ,
								Core.SelectedWaves );
					}
					break;
				case "menuSaveImageonly":
					if ( sfd.ShowDialog() == true )
						Core.SaveImage( sfd.FileName );
						break;
				
				case "menuSaveConfig":
					sfd.Filter = "Data Files (*.xml)|*.xml";
					sfd.DefaultExt = "xml";
					sfd.AddExtension = true;
					if ( sfd.ShowDialog() == true )
					{
						Core.SaveConfig(sfd .FileName);
					}
					break;

				case "menuExit":
					Environment.Exit( Environment.ExitCode );
					break;
			}
			CoreRunning = false;
		}

		public void AnalysisMenuClick( object sender , RoutedEventArgs e )
		{
			if ( CoreRunning ) return;
			CoreRunning = true;
			var master = sender as MenuItem;
			switch ( master.Name )
			{
				case "menuSinglePosScan":
					WinSingleScan.Visibility = Visibility.Visible;
					WinSingleScan.Topmost = true;
					break;

				case "menuMapAnalysis":
					new Win_ResultAnalysis( Core.ImgScanned ,
										    Core.ResultData == null ? 
												None : 
												Just( Core.ResultData) )
									.Act( x => x.evtClose += () => this.IsEnabled = true )
									.Act( x => x.Show()); 
					this.IsEnabled = false;
					break;
					
				case "menuModelUpdator":
					WinFitting.Visibility = Visibility.Visible;
					WinFitting.Topmost = true;
					break;

				default:
					break;
			}
			CoreRunning = false;
		}

		public void OptionMenuClick( object sender , RoutedEventArgs e )
		{
			
			if ( CoreRunning ) return;
			CoreRunning = true;
			var master = sender as MenuItem;
			switch ( master.Name )
			{
				case "menuViewSpct":
					Core.FlgAutoUpdate = true;
					FlgSpctDisplay = true;
					WinSpct.Visibility = Visibility.Visible;
					Task.Run( () =>
					 {
						 while ( FlgSpctDisplay )
						 {
							 WinSpct.ucSpctShart.UpdateSeries( Core.GetSpectrum() , Core.SelectedWaves );
							 Thread.Sleep( Core.SpectrometerDelayTime );
						 }
					 } );
					Console.WriteLine( "closed" );
					break;

				case "menuSetSpecStg":
					WinConfig.Show();
					this.IsEnabled = false;
					break;


				case "menuShowConfig":
						Core.ShowSetting();
					break;

				case "menuSetDefualtConfig":
					Core.Config = new IPSDefualtSetting().ToConfig();
					Config2UI(Core.Config);
					break;

				

				default:
					break;
			}
			CoreRunning = false;
		}


		public async void LeftSideBtn( string name )
		{
			if ( CoreRunning ) return;
			//CoreRunning = true;
			OpenFileDialog ofd = new OpenFileDialog();
			Mouse.OverrideCursor = Cursors.Wait;
			switch ( name )
			{
				case "btnSaveResult":
					SaveFileDialog sfd = new SaveFileDialog();
					if ( sfd.ShowDialog() == true )
					{
						Core.SaveResult( sfd.FileName + "_Result.csv" , Core.ResultData );
						Core.SaveRaw( 
							sfd.FileName + "_Raw.csv" , 
							Core.ResultData , 
							Core.SelectedWaves , 
							Core.Darks ,
							Core.Refs ,
							Core.SelectedReflctFactors );
						Core.SaveRawReflectivity(
							sfd.FileName + "_Reflectivity.csv" ,
							Core.ResultData ,
							Core.SelectedWaves );
						Core.SaveImage( sfd.FileName + "_MapImage.png" );
					}

					break;
				case "btnLoadConfig":
					if ( ofd.ShowDialog() == true )
					{
						Core.LoadConfig( ofd.FileName );
						Config2UI( Core.Config );
					}
				break;

				case "btnRefScan":
					Core.OpReady( IPSCore.ScanReadyMode.Ref );
					break;
				case "btnDarkScan":
					Core.OpReady( IPSCore.ScanReadyMode.Dark );
					break;
				case "btnSetWave":
					Core.OpReady( IPSCore.ScanReadyMode.WaveLen );
					break;
				case "btnLoadReflection":
					Core.OpReady( IPSCore.ScanReadyMode.Refelct );
					break;
				case "btnHome":
					Core.OpORGMaxSpeed();
					Core.OpHome();
					break;
				case "btnScanReady":
					Core.OpReady(IPSCore.ScanReadyMode.All);
					break;
				case "btnReconnect":
					Core.Connect();
					break;
				case "btnStart":
					Core.Config = UI2IpsConfig();
					ucLSStatus.lblProgress.Content = "InProgress";
					Core.ScanPos = new ScanPosData();
					Core.ScanPos.RhoList [ 3 ] = Core.Config.EdgeEnd;

					if ( await Task<bool>.Run( () => Core.ScanAutoRun() ) )
					{
						ucLSStatus.lblProgress.Content = "Ready";
						ucDataGrid.UpdateGrid( Core.ResultData.SpotDataList.Select( x =>
																x.ToGridResult() ).ToList() );
						ucMapDisplay.DrawImg( Core.ImgScanned );
					}
					else ucLSStatus.lblProgress.Content = ( "Ready (Interruped)" );
					
					break;

				case "btnSaveCurrentConfig":
					if ( ofd.ShowDialog() == true )
					{
						Core.Config = UI2IpsConfig();
						Core.SaveConfig( ofd.FileName );
					}
					break;
				default:
					break;
			}
			Mouse.OverrideCursor = null;
			CoreRunning = false;
		}
	}
	
}
