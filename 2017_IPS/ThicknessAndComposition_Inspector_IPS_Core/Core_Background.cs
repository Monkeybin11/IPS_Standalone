using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SpeedyCoding;
using ThicknessAndComposition_Inspector_IPS_Data;
using System.Windows.Media.Imaging;

namespace ThicknessAndComposition_Inspector_IPS_Core
{
	public partial class IPSCore
	{
		public enum ScanReadyMode { Ref , Dark , WaveLen , Refelct , All};

		public event Action<bool,bool> evtConnection;
		public event Action<IEnumerable<double>, IEnumerable<double>> evtSpectrum;
		public event Action<IEnumerable<double>, IEnumerable<double>> evtRefleectivity;
		public event Action<IEnumerable<double>, IEnumerable<double> , IEnumerable<double> , double , int > evtSngSignal;

		#region Status

		public bool FlgAutoUpdate;
		public bool FlgDarkReady;
		public bool FlgRefReady;
		public bool FlgHomeDone;
		public bool FlgCoreSingleScan = false;

		object keySingle = new object();
		#endregion


		#region result Data
		public IPSResult ResultData;

		public List<int> PickedIdx = new List<int>();
		public List<double> Wave = new List<double>();
		public List<double> Refs = new List<double>();
		public List<double> Darks = new List<double>();
		public List<double> SDWaves = new List<double>();
		public List<double> SelectedWaves = new List<double>();
		public List<int> PickedFactorIdx = new List<int>();
		public List<double> ReflctFactors = new List<double>();
		public List<double> SelectedReflctFactors = new List<double>();

		public int SpectrometerDelayTime { get { return Config.SpectrumWaitTime; } set { } }
		#endregion

		#region Static Data
		public string ModelPath = AppDomain.CurrentDomain.BaseDirectory + "Model";
		public string ModelName = "CurrentModel.model";
		public string ConfigBasePath = AppDomain.CurrentDomain.BaseDirectory + "config";
		public string ConfigName = "SettedConfig.xml";
		public string PrcConfigBasePath = AppDomain.CurrentDomain.BaseDirectory + "Prcconfig";
		public string PrcConfigName = "PrcSettedConfig.xml";

		public string ModelFullPath { get { return Path.Combine( ModelPath.CheckAndCreateDir() , ModelName ); } }
		public string ConfigFullPath { get { return Path.Combine( ConfigBasePath , ConfigName ); } }
		public string PrcConfigFullPath { get { return Path.Combine( PrcConfigBasePath , PrcConfigName ); } }
		string LogTime = "yyyy-MM-dd_HH-mm-ss";
		string CurrentSaveTime { get { return DateTime.Now.ToString( LogTime ); } }
		string LogDirPath = AppDomain.CurrentDomain.BaseDirectory + "log";
		string LogName = "";

		public double[] BkD_Spctrm = new double[] { }; // Background Data = Bkd_
		double[] Bkd_WaveLen = new double[] { }; 
		double [ ] SpctrmDeciles {
			get { return BkD_Spctrm
					.Map( x =>
							200.xRange( 40 , 20 )
							.Select( i => Bkd_WaveLen [ i ] ) )
					.ToArray();} }
		double [ ] WaveLenDeciles {
			get { return Bkd_WaveLen
					.Map( x =>
							200.xRange( 40 , 20 )
							.Select( i => Bkd_WaveLen [ i ] ))
					.ToArray(); } }
		#endregion



		Action AutoUpdateSpctrm =>
			() => BkD_Spctrm = GetSpectrum();

		Action GetPos =>
			() =>
			{
				var res = Stg.Query( Stg.Pos );
				Console.WriteLine( res );
			}; 
	}
}
