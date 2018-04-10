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
using Fitting_Core;

namespace ThicknessAndComposition_Inspector_IPS_Core
{
	using static Core_Helper;
	using static Core_Fitting;
	public partial class IPSCore
	{
		public ScanPosData ScanPos; 

		public IPSConfig Config { get; set; }
		//public IPSProcessingConfig Config { get; set; }
		IPSErrorMsgData    Err; // 에러 데이터 공통 메세지 클래스 
		Logger             Lggr; // 같은 로그 파일에 사용하기 위해서 전역 변수로 만들었다. 
		ISgmaStg_XR        Stg;
		IMaya_Spectrometer Spctr;
		//Maya_Spectrometer Spctr;
		Image<Bgr,byte> ImgScanResult;
		public System.Windows.Media.Imaging.BitmapSource ImgScanned { get { return ImgScanResult.ToBitmapSource(); } }

		#region Init
		public IPSCore()
		{
			InitCore(); // Field Initialize

		}

		public void Connect()
		{
			ConnectHW( "COM" + Config.Port.ToString() ).FailShow( "Virtual Mode is Activated" );
		}

		public string TodayLogPath()
		=> DateTime.Now.ToString( "yyyyMMdd" ) + "_IPSLog.txt";

		public void InitCore()
		{
			LogName = TodayLogPath();
			Lggr = new TextLogger( LogDirPath , LogName )
					.Act( x => Err = new IPSErrorMsgData( x ) );

			Config = XmlTool.ReadXmlClas(
						new IPSDefualtSetting().ToConfig() , // Defulat Setting
						ConfigBasePath.CheckAndCreateDir() ,
						ConfigName.CheckAndCreateFile() );
		}

		public bool ConnectHW( string comport )
		{
			bool stg = false;
			bool spt = false;
			try
			{
				
				Stg = new SgmaStg_XR( comport, false );
				Spctr = new Maya_Spectrometer();

				stg = Stg.Open() == true
							? true
							: false.Act( x => Err.WriteShowErr( ErrorType.StgConnectErr ) );

				spt = Spctr.Connect() == true
							? true
							: false.Act( x => Err.WriteShowErr( ErrorType.SpecConnectErr ) );
			}
			catch ( Exception ){}
			
			if ( !stg || !spt )
			{
				Stg.Close();
				Stg = new SgmaStg_XR_Virtual();
				Spctr = new Maya_Spectrometer_Virtual();

				// Dummy Data
				var rnd = new Random();
				Darks = new double[1068].ToList();
				ReflctFactors = Enumerable.Range( 0, 1068 ).Select( x => (double)rnd.Next( 1, 10 ) ).ToList();
				Refs = Enumerable.Range( 0, 1068 ).Select( x => (double)rnd.Next( 10, 100 ) ).ToList();
				SelectedWaves = 300.0.xRange( 1068, 1 ).ToList();
			}

			evtConnection( stg , spt );
			return stg && spt;
		}

		public void StartBackgroundTask()
		{
			Task.Run( () =>
			{
				while ( true )
				{
					if ( FlgAutoUpdate )
					{
						AutoUpdateSpctrm();
						
					}
					Thread.Sleep( SpectrometerDelayTime );
				}
			} );

		}

		public double[] GetSpectrum()
			=> Spctr.GetSpectrum();
			#endregion

		#region MainFunction
		int counter = 0;

		object key = new object();
		public void updatecounter()
		{
			lock ( key )
			{
				counter++;
			}
		}
		/*
		public Func<LEither<double[]> , IEnumerable<double> , PlrCrd ,
						Tuple<PlrCrd , LEither<double>>> CalcPorce1 =>
			( inten , wave , plrcrd ) =>
			{
				double[] intendata = new double[] { };
				double[] wav = new double[] { };

				PlrCrd crd = new PlrCrd(0,0);
				double res = 355.46848;

				try
				{
					if ( plrcrd.Rho != 0 )
					{
						var intentemp = LoadTestData();
						var postemp = CreatedefualtPos();

						intendata = intentemp [ counter ];
						crd = postemp [ counter ];
						wav = LoadWavelen();

						var integ = intendata.Integral(wav , Config.IntglStart , Config.IntglEnd);
						res = integ * Config.Weight + Config.Bias;

						updatecounter();
						"updated".Print( counter.ToString() );
					}
					else
					{
						"Not updated".Print( counter.ToString() );
						plrcrd.ToString().Print( "Current Pos" );
					}
				}
				catch ( Exception )
				{
				}
				return Tuple.Create( crd , res.ToLEither() );
			};
			*/

		public Func<LEither<double [ ]> , IEnumerable<double>  , PlrCrd ,
						Tuple<PlrCrd , LEither<double> , double [ ]>> ToThickness =>
			( reflections , wave , plrcrd ) =>
			{

				var temp =  reflections.Bind( x => x.Skip( 450 ).Take( 400 ).ToArray() , "Refine Refelectivity Fail" );
				var temp2 = Predict(temp.Right);

				List<double> templist = new List<double>();

				foreach ( var item in temp2 )
				{
					templist.Add( ( double )item );
				}

				return Tuple.Create( plrcrd,
									 reflections.Bind( x => x.Skip(450).Take(400).ToArray()	, "Refine Refelectivity Fail" )
											    .Bind( Predict , "Predict Fail" )
												.Bind( x => (double)x.First() ),
									 reflections.Right);
			};

		public async Task<bool> ScanAutoRun() // Use Internal Config , not get config from method parameter
		{
			LoadModel( ModelFullPath );
			OpMaxSpeed();
			OpORGMaxSpeed();
			counter = 0;

			SetHWInternalParm(
							  Config.RStgSpeed ,
							  Config.XStgSpeed ,
							  Config.Scan2Avg ,
							  Config.IntegrationTime ,
							  Config.Boxcar ,
							  Config.EdgeEnd
							  );
			// Ref Check --
			if ( !FlgRefReady ) return false.Act( x => MessageBox.Show("Set Referance Please"));

			// Home And Check --
			//if ( !FlgHomeDone ) if ( !OpHome() ) return false;
			OpHome();
			FlgHomeDone = true;

			// Scan Ready And Check -- 
			if ( FlgDarkReady == false ) if ( !OpReady( ScanReadyMode.Dark ) ) return false;
			FlgDarkReady = true;

			var toReflect = FnCalReflections(Darks,Refs,SelectedReflctFactors);
			var toSelected = FnIdxDataPicker(PickedIdx);
			var thetas = GetPosThetas(ScanPos);
			var calcTaskList =
					new Task< Tuple<PlrCrd , LEither<double> , double[]>>
					[ScanPos.ThetaList.Select( x => x.Length).Aggregate((f,s) => f+ s) ];
			var wavelength = SDWaves;
			var res = new TEither( Stg as IStgCtrl , 12); // Controller , Timeout 

			// Get Pos-Intensity || Task Calc Thickness --
			int taskcounter = 0;
			try
			{

			var posIntenlist = ScanPos.RhoList.Select( ( rho , i ) =>
			{
				var logs = res.Bind( x => x.Act( f =>
				{
					f.SendAndReady( f.GoAbs + rho.mmToPulse().ToOffPos(Axis.X) );
					f.SendAndReady( f.Go );
				} ).ToTEither() , " X Stage Move Command Fail" );

				return  thetas[i].Select( (theta,j) =>
					{
						var pos = new PlrCrd(theta,rho).Print();

						var logres = logs.Bind( x => x.Act( f =>
						{
							f.SendAndReady( f.GoAbs + ((ScanPos.MovePosList[i][j] + 360 * (i > 1 ? i-1 : 0 ) )).Degree2Pulse().ToPos(Axis.R) );
							f.SendAndReady( f.Go );
							Thread.Sleep(500);
						} ).ToTEither() , "R Stage Move Command Fail" )
													.ToLEither( new double [ ] { } );
						//Thread.Sleep(SpectrometerDelayTime);

						var intenlist = logres.IsRight // Todo : Change to Match Function
													//? logres.Bind( x => toSelected( BkD_Spctrm ) )
													? logres.Bind( x => toSelected( GetSpectrum() ) ) // Todo : Change to only toSelected( Spctr.GetSpectrum() ), not bind. cuz endpoint
													: logres.Act( x => Lggr.Log(x.Left , true )); // Logging Error

						evtSpectrum( intenlist.Right , SelectedWaves );

						var temp1 = ToThickness(
							toReflect(intenlist.Right)
								.Act( x => evtRefleectivity( x , SelectedWaves ))
								.ToLEither(intenlist.Left),
							wavelength,
							pos );  // Estimate Thickness

			calcTaskList [ taskcounter++ ] // Clac Thickness
										= Task.Run<Tuple<PlrCrd , LEither<double> ,double[] >>(
											() => logres.IsRight
													? ToThickness(
														toReflect(intenlist.Right)
															.Act( x => evtRefleectivity( x , SelectedWaves ))
															.ToLEither(intenlist.Left),
														wavelength,
														pos )  // Estimate Thickness
													: Tuple.Create( pos , new LEither<double>() , new double[] {} ));
						return Tuple.Create(pos,intenlist);
					}).ToList();
			}).SelectMany( x => x ).ToList(); // List [ Result , Result , Result  ] 

			// Thickness Result List --
			// ( You dont need to use try catch pattern for catch all exception from tasklist )
			//var posThicknesses = Task.WhenAll( calcTaskList ).Result.Duplicate0ToAllTheta().ToList();
			var posThickReflec = Task.WhenAll( calcTaskList ).Result.ToList();

			int nochiIdx = 0;
			for ( int i = 0 ; i < posThickReflec.Count ; i++ )
			{
				if ( posThickReflec [ i ].Item1.Rho == 148 )
					if ( posThickReflec [ i ].Item1.Theta == 0 )
						nochiIdx = i;
			}

			posThickReflec [ nochiIdx ] = Tuple.Create(
				posThickReflec [ nochiIdx ].Item1 ,
				( ( posThickReflec.Last().Item2.Right
					+ posThickReflec [ nochiIdx + 1 ].Item2.Right )
					/ 2.0 )
					.ToLEither( posThickReflec [ nochiIdx ].Item2.Left ) ,

				(  posThickReflec.Last().Item3.Zip( 
												posThickReflec [ nochiIdx + 1 ].Item3 ,
												( f , s )  => (f + s)/2.0 )
				).ToArray());

			// Reset Pos --
			await Task.Run( () =>
				res.Bind( x => x.Act( f => f.SendAndReady( f.GoAbs + 0.ToPos( Axis.W ) + 0.ToPos() ) ).ToTEither( 1 ) )
				   .Bind( x => x.Act( f => f.SendAndReady( f.Go ) ).ToTEither( 1 ) ));
			FlgAutoUpdate = false;
			
			//Zip ( Pos , Thickness , Intencity ) --
			var posThickIntenReflc = posThickReflec
									.Zip(posIntenlist ,
										(f,s) => new
										{
											Pos = f.Item1 ,
											Thickness = f.Item2 ,
											Intensity = s.Item2 ,
											Refelctivity = f.Item3
										} );

			// -- Integration Log Result -- // 
			bool isright = posThickReflec.Select( x => x.Item2.IsRight).Aggregate( (f,s) => f && s);

			if ( !isright ) // If Task Error
			{
				var tasklogs = posThickIntenReflc.Where( x => x.Thickness.IsRight == false)
											.Select( x => x.Pos.ToString()+" ||" + x.Thickness.Left )
											.ActLoop( x => Lggr.Log(x , true));
				ImgScanResult = new Image<Bgr , byte>( 100,100, new Bgr(100,100,100) );
				return false;
			}
			else
			{
				var poses     = posThickIntenReflc.Select( x => x.Pos ).ToList();
				var thckneses = posThickIntenReflc.Select( x => x.Thickness.Right ).ToList();
				var intens    = posThickIntenReflc.Select( x => x.Intensity.Right ).ToList();
				var reflec    = posThickIntenReflc.Select( x => x.Refelctivity ).ToList();


				ResultData = ToResult( poses , thckneses , intens , reflec , SelectedWaves );
				Stopwatch stw = new Stopwatch();
				stw.Start();
				CreateMapandBar( ResultData , 6 )
					.Act( x => ImgScanResult = x );
				stw.ElapsedMilliseconds.Print("Draw Time");
				return true;
			}

			}
			catch ( Exception e)
			{
				MessageBox.Show( e.ToString() );
				return false;
			}
		}
		#endregion
	}
}
