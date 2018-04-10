using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using ThicknessAndComposition_Inspector_IPS_Data;
using static System.Linq.Enumerable;
using Unit = System.ValueTuple;
using static System.ValueTuple;
using SpeedyCoding;


/* State Manipulation Part  */

namespace IPSAnalysis
{
	using ThicknessAndComposition_Inspector_IPS_Core;
	using static IPSAnalysis.Handler;
	using static AnalysisState;
	using AnalysisBase;

	public static partial class Handler
	{
		public static AnalysisState CreateState
			( Dictionary<int , IPSResultData> dict = null , 
			  double [ ] waveMinMax = null )
			=> new AnalysisState( dict , waveMinMax );
	}

	public class AnalysisState
	{
		public static Dictionary<int,IPSResultData> _State;
		public Dictionary<int , IPSResultData> State { get { return _State; } set { _State = value; } }
		private static double[] _WaveMinMax = new double[] { };
		public double [ ] WaveMinMax { get { return _WaveMinMax; } set { _WaveMinMax = value; } }
		public bool IsAdd;
		public int TargetIdx;

		public AnalysisState( Dictionary<int , IPSResultData> dict = null , double [ ] waveMinMax = null )
		{
			try
			{
				State = dict == null
							? new Dictionary<int , IPSResultData>()
							: dict;

				WaveMinMax = waveMinMax != null
							? waveMinMax
							: new double [ ]
								{
								AnalysisState._State.First().Value.WaveLegth[1],
								AnalysisState._State.First().Value.WaveLegth.Last()
								};
			}
			catch ( Exception ex )
			{
				ex.ToString().Print();
			}
		}

		public AnalysisState Defualt()
			=> new AnalysisState();
	}

	public static class AnalysisFunc
	{
		public static AnalysisState Add( AnalysisState state , IPSResultData data , int idx )
			=> state.Map( SetAction( true ) )
					.Map( SetTarget( idx ) );

		public static AnalysisState Pop( AnalysisState state , int idx )
			=> state.Map( SetAction( false ) )
					.Map( SetTarget( idx ) );

		public static AnalysisState ChangeWaveLen( AnalysisState state , double [ ] minmax )
			=> CreateState( state.State , minmax );

		public static AnalysisState Insert(
			this AnalysisState self , IPSResultData data , int idx )
		{
			self.State [ idx ] = data;
			return self;
		}

		public static AnalysisState Remove(
			this AnalysisState self , int idx )
		{
			self.State.Remove( idx );
			return self;
		}

		static Func<AnalysisState,AnalysisState> SetAction(  bool isAdd )
			=> state =>
			{
				state.IsAdd = isAdd;
				return state;
			};

		static Func<AnalysisState , AnalysisState> SetTarget( int idx )
			=> state =>
			{
				state.TargetIdx = idx;
				return state;
			};


		#region IO

		public static Intensity [ ] OfIntensity
			( this AnalysisState self , int idx )
			=> self.State.ContainsKey( idx )
				? self.State [ idx ].IntenList
				: null;

		public static Reflectivity [ ] OfRefelctivity
			( this AnalysisState self , int idx )
			=> self.State.ContainsKey( idx )
				? self.State [ idx ].Reflectivity
				: null;

		public static Thickness OfThickness
		( this AnalysisState self , int idx )
			=> self.State.ContainsKey( idx )
				? self.State [ idx ].Thickness
				: null;

		public static mCrtCrd OfPosition
		( this AnalysisState self , int idx )
			=> self.State.ContainsKey( idx )
				? self.State [ idx ].Position
				: null;

		#endregion

		#region Exractor
		public static IEnumerable<double [ ]> ExtractInten( AnalysisState state )
			=> state.State.Select( x => x.Value.DIntenList.ToArray() );

		public static IEnumerable<double [ ]> ExtractRflct( AnalysisState state )
			=> state.State.Select( x => x.Value.DReflectivity.ToArray() );

		public static IEnumerable<double> ExtractLabel( AnalysisState state )
			=> state.State.First().Value.DWaveLength;

		#endregion

	}
}

