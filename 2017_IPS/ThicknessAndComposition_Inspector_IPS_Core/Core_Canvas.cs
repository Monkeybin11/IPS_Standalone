using SpeedyCoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ModelLib.Data;
using MachineLib.DeviceLib;
using ModelLib.ClassInstance;
using System.Threading;
using System.IO;

namespace ThicknessAndComposition_Inspector_IPS_Core
{
	public partial class IPSCore
	{

		public event Action evtSingleMeasureComplete;
		public void StartManualRunEvent( double [ ] TargetPosTR , int intervalsec , int count )
		{
			if ( FlgCoreSingleScan ) return;
			FlgCoreSingleScan = true;
			Task.Run( () => ScanManualRun( TargetPosTR , intervalsec , count ) );
		}

		public  bool ScanManualRun( double [ ] TargetPosTR , int intervalsec , int count )
		{
			OpMaxSpeed();
			OpORGMaxSpeed();
			// Ref Check --
			if ( !FlgRefReady ) return false.Act( x => MessageBox.Show( "Set Referance Please" ) );
			// Home And Check --
			//if ( !FlgHomeDone ) if ( !OpHome() ) return false;
			///OpHome();
			FlgHomeDone = true;

			// Scan Ready And Check -- 
			if ( FlgDarkReady == false ) if ( !OpReady( ScanReadyMode.Dark ) ) return false;
			FlgDarkReady = true;

			var toReflect = FnCalReflections(Darks,Refs,SelectedReflctFactors);

			var plrpos = new CrtnCrd(TargetPosTR[0] ,
									 TargetPosTR[1])
							.ToPolar() as PlrCrd;

			var stgMoveRes = new TEither( Stg as IStgCtrl , 12)
									.Bind( x => x.Act( f =>
			{
				f.SendAndReady( f.GoAbs
								+ plrpos.Theta.Degree2Pulse().ToPos( Axis.W )
								+ plrpos.Rho.mmToPulse().ToOffPos() );
				f.SendAndReady( f.Go );

			}).ToTEither() ,  "R Stage Move Command Fail" );
			var moveResLog = stgMoveRes.ToLEither(new double[]{ });
			
			lock ( keySingle )
			{
				int curcount = 0;
				while ( FlgCoreSingleScan )
				{
					Console.WriteLine( "Current Count : {0} , Limit Count : {1}" , curcount , count );

					if ( curcount == count ) break;

					var currentInten = GetSpectrum();
					var reflet =  toReflect(currentInten);
					var thckn = ToThickness(
												reflet.ToLEither() ,
												SelectedWaves,
												plrpos )
											.Item2.Right;

					evtSngSignal( currentInten , reflet , SelectedWaves , thckn , curcount );
					Thread.Sleep( intervalsec * 1000 );
					curcount++;
				}
			}
			Console.WriteLine( "Complete" );
			evtSingleMeasureComplete();
			FlgCoreSingleScan = false;
			return true;
		}

		

	}
}
