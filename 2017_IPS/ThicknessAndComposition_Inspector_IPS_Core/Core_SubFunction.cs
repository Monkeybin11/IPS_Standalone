using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLib.DeviceLib;
using ThicknessAndComposition_Inspector_IPS_Data;
using ApplicationUtilTool.FileIO;
using System.IO;
using ModelLib.TypeClass;
using ModelLib.ClassInstance;
using ApplicationUtilTool.Log;
using System.Windows;
using SpeedyCoding;
using System.Threading;
using ModelLib.Data;
using System.Reflection;
using System.Windows.Input;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCvExtension;
using System.Diagnostics;

namespace ThicknessAndComposition_Inspector_IPS_Core
{
	using static ApplicationUtilTool.FileIO.CsvTool;
	public partial class IPSCore
	{
		#region OpFun

		public bool OpReady( ScanReadyMode mode )
		{

			OpMaxSpeed();
			LEither<bool> result = new LEither<bool>(true);
			switch ( mode )
			{
				case ScanReadyMode.Dark:
					result.Bind( x => OpSetdark() , "Dark Scan is Fail" );
					break;

				case ScanReadyMode.Ref:
					SetHWInternalParm(
							  Config.RStgSpeed ,
							  Config.XStgSpeed ,
							  Config.Scan2Avg ,
							  Config.IntegrationTime ,
							   Config.Boxcar ,
							   Config.EdgeEnd
							  );
					result.Bind( x => OpSetRef() , "Referance Scan is Fail" )
						  .Bind( x => x.Act( f => FlgRefReady = true));
					break;

				case ScanReadyMode.Refelct:
					result.Bind( x => OpLoadAbsReflecDatas() , "Refelct Scan is Fail" );
					break;

				case ScanReadyMode.WaveLen:
					result.Bind( x => OpPickWaveIdx() , "WaveLength Scan is Fail" );
					break;
			}
			return result.IsRight
				? true
				: false;
		}


		public bool OpSetdark() //sjw
		{
			Stg.SendAndReady( Stg.GoAbs + 0.ToPos( Axis.X ) );
			Stg.SendAndReady( Stg.Go );
			var darkraw = GetSpectrum();
			Task.Run(() => evtSpectrum( darkraw , SelectedWaves ));
			Darks = PickedIdx.Select( x => darkraw [ x ] ).ToList();
			return true;
		}

		public async Task<bool> OpSetRef() //sjw
		{
			double[] refraw = new double[] { };
			await Task.Run( () =>
			 {
				 Stg.SendAndReady( Stg.GoAbs + 0.ToOffPos( Axis.X ) );
				 Stg.SendAndReady( Stg.Go );
				 refraw = GetSpectrum();
				 evtSpectrum( refraw , SelectedWaves );
				 Refs = PickedIdx.Select( x => refraw [ x ] ).ToList();
				 Stg.SendAndReady( Stg.GoAbs + 0.ToPos( Axis.X ) );
				 Stg.SendAndReady( Stg.Go );
			 } );
		

			//Stopwatch stw = new Stopwatch();
			//bool flgout = false;
			//stw.Start();
			//while ( !flgout )
			//{
			//	if ( stw.ElapsedMilliseconds / 1000 > SpectrometerDelayTime )
			//		flgout = true;
			//}
			//stw.ElapsedMilliseconds.Print( "Waited Time" );
			//stw.Stop();

			//await Task.Delay( SpectrometerDelayTime );

			


			StringBuilder stb = new StringBuilder();
			refraw.ActLoop( x =>
			{
				stb.Append( x );
				stb.Append( Environment.NewLine );
			} );

			Clipboard.SetText( stb .ToString());
			return true;
		}

		public bool OpPickWaveIdx() //sjw
		{
			Bkd_WaveLen = Spctr.GetWaveLen().Select(x => (double)Math.Round(x , 11)).ToArray();
			PickedIdx = Enumerable.Range(0,Bkd_WaveLen.GetLength(0)).ToList();
			//SelectedWaves = PickedIdx.Select( x => (double)Bkd_WaveLen [ x ] ).ToList();
			SelectedWaves = Bkd_WaveLen.ToList();
			return true;
		}

		public bool OpPickReflectFactorIdx()
		{
			List<int> idxlist = new List<int>();
			foreach ( var item in SelectedWaves )
			{
				var idx = Array.IndexOf( SDWaves.ToArray() , item);
				if ( idx >= 0 ) idxlist.Add( idx );
			}
			PickedFactorIdx = idxlist;
			SelectedReflctFactors = ReflctFactors; // For Speed
			return true;
		}

		public bool OpHome()
		{
			var resHome = new TEither( Stg as IStgCtrl , 12)
						.Bind( x => x.Act( f =>
							f.SendAndReady( f.Home + Axis.W.ToIdx() ) ).ToTEither( 12 ) , "R or X Home Fail" );
			//.Bind( x => x.Act( f =>
			//			f.SendAndReady( f.GoAbs
			//							+ 0.ToOffPos(Axis.X) ) )
			//					 .ToTEither( 1 ) , "Opposit Movement Fail" )
			//.Bind( x => x.Act( f =>
			//	f.SendAndReady( f.Go ) ).ToTEither( 1 ) , "Stage Movement Fail" );
			if ( !resHome.IsRight ) return false.Act( x => FlgHomeDone = false );
			return true.Act( x => FlgHomeDone = true );
		}

		public bool OpLoadAbsReflecDatas() 
		{
			//string path = AppDomain.CurrentDomain.BaseDirectory + @"absreflect\Siref10.csv";
			string path = AppDomain.CurrentDomain.BaseDirectory + @"absreflect\10deg_siabsreflect.csv";
			if ( !File.Exists( path ) )
			{
				MessageBox.Show( @"Please absolute refelection data in program folder. path and file name is ""...\absreflect\10deg_siabsrefle.csv"" " );
				return false;
			}

			var relec = ReadCsv2String( path , order0Dirction: false );
			SDWaves = relec.Select( x => Convert.ToDouble( x [ 0 ] ) ).ToList();
			ReflctFactors = relec.Select( x => Convert.ToDouble( x [ 1 ] ) ).ToList();
			return true;
		}

		public bool OpMaxSpeed()
		{
			var str = Stg.SetSpeed
				+ Axis.W.ToIdx()
				+ ( 50000 ).ToSpeed()
				+ ( 100000 ).ToSpeed();
			Stg.SendAndReady( str );
			return true;
		}

		public bool OpORGMaxSpeed()
		{
			Stg.SendAndReady( "V:"
				+ Axis.W.ToIdx()
				+ ( 50000 ).ToSpeed()
				+ ( 100000 ).ToSpeed() );
			return true;

		}

		#endregion

		#region SubFuntion
		public void ShowSetting()
		{
			StringBuilder configlog = new StringBuilder();
			Type ty = Config.GetType();
			foreach ( PropertyInfo config in ty.GetProperties() ) // pick one property from class
			{
				var condition = config.GetValue( Config ) // get property value
									  .GetType() // type of value
									  .GetInterfaces() // interface of type
									  .Where( x => x.Name == "ICollection" )
									  .Count();
				"".Print();
				if ( condition > 0 ) continue;

				configlog.Append( " - " + config.Name + " : " + config.GetValue( Config ) );
				configlog.Append( Environment.NewLine );
			}
			MessageBox.Show( configlog.ToString() );
		}

		public void SetComPort( double port ) => Config.Port = ( int )port;

		public void SetHWInternalParm( double rspeed , double xspeed , double scan2avg , double intetime , double boxcar , double edgeend )
		{
			Config.RStgSpeed = ( int )rspeed;
			Config.XStgSpeed = ( int )xspeed;
			Config.Scan2Avg = ( int )scan2avg;
			Config.IntegrationTime = ( int )intetime;
			Config.Boxcar = ( int )boxcar;
			Config.EdgeEnd = edgeend;

			Stg.SendAndReady( "S:24" );
			Stg.SendAndReady( "S:15" );

			//Stg.SendAndReady( Stg.SetSpeed
			//				+ Axis.W.ToIdx()
			//				+ ( Config.RStgSpeed ).ToSpeed()
			//				+ ( Config.XStgSpeed ).ToSpeed() );

			Spctr.ScanAvg( Config.Scan2Avg );
			//Spctr.ScanAvg( 2 );
			Spctr.IntegrationTime( Config.IntegrationTime*1000 );
			//Spctr.IntegrationTime( 600000 );
			Spctr.BoxCar( Config.Boxcar );
		}

	
		public bool GoAbsPos( Axis axis , int pos1 , int pos2 = 99999 )
		{
			switch ( axis )
			{
				case Axis.R:
					Stg.SendAndReady( Stg.GoAbs + pos1.ToPos( Axis.R ) );
					break;

				case Axis.X:
					Stg.SendAndReady( Stg.GoAbs + pos1.ToPos( Axis.X ) );
					break;

				case Axis.W:
					Stg.SendAndReady( Stg.GoAbs + pos1.ToPos( Axis.R ) );
					Stg.SendAndReady( Stg.GoAbs + pos2.ToPos( Axis.X ) );
					break;
			}
			Stg.SendAndReady( Stg.Go );
			return true;
		}

		public double [ ] [ ] GetPosThetas( ScanPosData data )
			=> data.ThetaList.Select( ( x , i ) => i % 2 != 0
												? x
												: x)//.Reverse().ToArray() )
							 .ToArray();

		public Func<double [ ] , double [ ]> FnCalReflections(
			List<double> dark ,
			List<double> refer ,
			List<double> reffactor )
			=> src =>
			{
				var res =src.Select( ( x , i ) => ( (x - dark [ i ]) / (refer [ i ] - dark [ i ]) ) * reffactor [ i ] )
						 .ToArray();
				return res;
			};
			

		public Func<double [ ] , double [ ]> FnIdxDataPicker(
			List<int> pickedidx )
			=> src => pickedidx.Count == 0 ? src : pickedidx.Select( x => src [ x ] ).ToArray();


		public double [ ] SplitBound( double [ ] src , int divide )
		{
			var min = src.Min();
			var max = src.Max();

			var step = (max - min) / divide;

			var res = new double[divide];




			return new double [ ]
				{


				};

		}
		public double [ ] ImgDataTo5Group( double[] src ,double[] bound)
		{


			return null;
		}

		#endregion

		#region Test Helper

		public void test()
		{
			Mapping.LoadMapdata();
		}

		public List<double [ ]> LoadTestData( List<string> pathlist = null )
		{
			lock ( key )
			{
				try
				{
					List<string> filenamelist = new List<string>();
					List<double[]> output = new List<double[]>();
					if ( pathlist == null )
					{
						string path = @"D:\03JobPro\2017\011.ISP\CsvData\TLA talko _new Sample\1spct";
						filenamelist = Directory.GetFiles( path ).ToList();
					}

					
					foreach ( var item in filenamelist )
					{
						var strdata = ReadCsv2String( item , ',' );
						for ( int i = 1 ; i < strdata.Len( 1 ) ; i++ )
						{
							List<double> res = new List<double>();
							for ( int j = 0 ; j < strdata.Len( 0 ) ; j++ )
							{
								if ( j == 4 ) continue;
								res.Add( Convert.ToDouble( strdata [ j ] [ i ] ) );
							}
							output.Add( res.ToArray() );
						}
					}
					return output;
				}
				catch ( Exception ex )
				{
					Console.WriteLine( ex.ToString() );
					return null;
				}
			}
		}

		public double [ ] LoadWavelen()
		{
			lock ( key )
			{
				string path = @"D:\03JobPro\2017\011.ISP\CsvData\TLA talko _new Sample\1spct\01.csv";
				var res = ReadCsv2String(path , order0Dirction : false);
				return res [ 0 ].Select( x => Convert.ToDouble( x ) ).ToArray();
			}
		}


		public List<PlrCrd> CreatedefualtPos()
		{
			lock ( key )
			{
				var path = @"D:\03JobPro\2017\011.ISP\CsvData\TLA talko _new Sample\1pos\pos.csv";
				var datas = ReadCsv2String( path , ',' , order0Dirction: false);

				List<PlrCrd> crtnlist = new List<PlrCrd>();

				for ( int i = 0 ; i < datas.Len( 2 ) ; i++ )
				{

					for ( int j = 0 ; j < datas.Len( 0 ) / 2 ; j++ )
					{
						var crt = new CrtnCrd(Convert.ToDouble( datas[ j * 2 ][i]) ,Convert.ToDouble( datas[ j * 2 + 1 ][i] ));
						var plr = crt.ToPolar();
						crtnlist.Add( plr as PlrCrd );
					}
				}
				return crtnlist;
			}
		}

		#endregion
	}
}
 