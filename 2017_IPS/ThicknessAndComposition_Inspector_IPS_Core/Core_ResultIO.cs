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
using System.Runtime.InteropServices;

namespace ThicknessAndComposition_Inspector_IPS_Core
{
	using static ThicknessAndComposition_Inspector_IPS_Core.Core_Helper;

	public partial class IPSCore
	{
		#region Trans
		public IPSResult ToResult( List<PlrCrd> pos , List<double> thckness , List<double [ ]> intens , List<double [ ]> reflectivities , List<double> wavelen )
		{
			var res = new IPSResult(wavelen );
			for ( int i = 0 ; i < pos.Count ; i++ )
			{
				res.SpotDataList.Add( new SpotData( pos [ i ] , thckness [ i ] , intens [ i ] , reflectivities [i]) );
			}
			return res;
		}
		#endregion

		#region IO

		public void SaveResult( string path , IPSResult result )
		{
			if ( !CheckResult( result ) ) return;
			StringBuilder stb = new StringBuilder();
			//stb.Append( "Theta" + "," + "Rho" + "," + "Thickness" );
			stb.Append( "X" + "," + "Y" + "," + "Thickness" );
			stb.Append( Environment.NewLine );
			result?.SpotDataList.ForEach( x =>
			{
				stb.Append( x.CrtPos.X.ToString("N2") + "," +  x.CrtPos.Y.ToString("N2") );
				//stb.Append( x.PlrPos.Theta.ToString() + "," +  x.PlrPos.Rho.ToString() );
				stb.Append( "," + x.Thickness );
				stb.Append( Environment.NewLine);
			} );
			File.WriteAllText( path , stb.ToString() );
		}

		public void SaveRaw( 
			string path , 
			IPSResult result , 
			List<double> wavelen ,
			List<double> dark ,
			List<double> refer , 
			List<double> absreflect )
		{
			if ( !CheckResult( result ) ) return;
			StringBuilder stb = new StringBuilder();
			
			stb.Append( "WaveLength,Dark,Referance,Abs_Reflectivity" );
			
			result.SpotDataList.ForEach( x =>
			{
				stb.Append( "," + x.CrtPos.X.ToString("N2") + " " + x.CrtPos.Y.ToString("N2") );
				
				//stb.Append( "," + x.PlrPos.Theta.ToString() + " " + x.PlrPos.Rho.ToString() + ",Reflectivity" );
			} );
			stb.Append(Environment.NewLine);

			for ( int i = 0 ; i < dark.Count ; i++ )
			{
				stb.Append(
					wavelen[i].ToString() + "," +
					dark[i].ToString() + "," +
					refer[i].ToString() + "," +
					absreflect[i].ToString()  
					);
				result.SpotDataList.ForEach( x =>
				{
					stb.Append( "," + x.IntenList[i] );
					
				} );
				stb.Append( Environment.NewLine );
			}

			File.WriteAllText( path , stb.ToString() );
		}

		public void SaveRawReflectivity(
		string path ,
		IPSResult result,
		List<double> wavelen )
		{
			if ( !CheckResult( result ) ) return;
			StringBuilder stb_raw = new StringBuilder();
			stb_raw.Append( "WaveLength" );
			result.SpotDataList.ForEach( x =>
			{
				stb_raw.Append( "," + x.CrtPos.X.ToString( "N2" ) + " " + x.CrtPos.Y.ToString( "N2" ) );
			} );
			stb_raw.Append( Environment.NewLine );

			for ( int i = 0 ; i < wavelen.Count ; i++ )
			{
				stb_raw.Append( wavelen [ i ].ToString() );
				result.SpotDataList.ForEach( x =>
				{
					stb_raw.Append( "," + x.Reflectivity [ i ] );
				} );
				stb_raw.Append( Environment.NewLine );
			}

			File.WriteAllText( path , stb_raw.ToString() );
		}

		public bool SaveImage( string path )
		{
			if ( ImgScanResult == null )
			{
				MessageBox.Show( "Scan result is not exist. Save is canceled" );
				return false;
			}
			ImgScanResult.Save( path );
			return true;
		}

		private bool CheckResult( IPSResult res )
		{
			if ( res == null )
			{
				MessageBox.Show( "Scan result is not exist. Save is canceled" );
				return false;
			}
			return true;
		}


		public void LoadConfig( string path )
		{
			Config = XmlTool.ReadXmlClas(
						Config ,
						Path.GetDirectoryName( path ) ,
						Path.GetFileName( path ) );
		}

		public void SaveConfig( string path )
		{
			XmlTool.WriteXmlClass(
			   Config ,
			   Path.GetDirectoryName( path ) ,
			   Path.GetFileName( path ) );
		}

		public void SavePrcConfig( string path )
		{
			XmlTool.WriteXmlClass(
					 Config ,
					 Path.GetDirectoryName( path ) ,
					 Path.GetFileName( path ) );
		}


		#endregion
	}

	public static class Core_Helper
	{
		public static Image<Bgr , byte>  CreateMapandBar( IPSResult src , int divide )
		{
			int dotSize = 5;
			var sizemultiflier = 8;
			var imgshiftoffset = 10;
			var offset = src.SpotDataList.Select( x => x.PlrPos.Rho/10.0).Max(); // Padding Size
			var thcklist = src.SpotDataList.Select( x => x.Thickness);

			// Choose Data Scale 
			// normalization -> In trust region 95% , fit outlier data to boundary -> rescale from  0 to 255   
			//var pinnedArr = GCHandle.Alloc( thcklist , GCHandleType.Pinned);

			//int size = Marshal.SizeOf(thcklist);
			//var pointer = Marshal.AllocHGlobal(size);
			//Marshal.StructureToPtr( thcklist , pointer , true );



			//var inputarr = new Mat(new int[] { 1, thcklist.Count() } , Emgu.CV.CvEnum.DepthType.Cv64F ,  pointer  );
			//CvInvoke.MeanStdDev( inputarr , ref mean , ref std );
			//Matrix<double> datas = new Matrix<double>(1, thcklist.Count() , pointer);
			//pinnedArr.Free();

			//var zscore = thcklist.Select( x => (x - mean.V0)/std.V0 )
			//							  .Select( x => x >  1.96 ? 1.96 :
			//											x < -1.96 ? -1.96
			//											: x).ToArray()
			//							  ;

			//var n = (double)thcklist.Count();
			//var mean = thcklist.Sum()/n;
			//var std = Math.Sqrt( thcklist.Select( x => Math.Pow(x - mean,2)).Sum() /  n );

			var min = thcklist.Min();
			var max = thcklist.Max();

			var cm = new ColorMap().Inferno_cm;

			// Interpolation


			var srcdatas = src.Result2TRThArr().Select( x =>x).ToList();
			
			/*
			#region 
			// Extact Color Position
			"".Print();
			"".Print();
			"".Print();
			"STart Point".Print();
			var xyCm2 = srcdatas.ToCartesianReslt()
							.OrderBy( x=> x[0])
							.ThenBy( x => x[1])
							.AsParallel()
							.AsOrdered()
							//.Select( x => new double[] { x[0] , x[1] , ( x[2] - mean ) / std } )
							//.Select( x => new double[] { x[0] , x[1] , x[2] >  1.96 ? 1.96 :
							//										   x[2] < -1.96 ? -1.96
							//										   : x[2] } )
							.Select( x => new
							{
								X  = offset + x[0] ,
								Y  = offset + x[1] ,
								Cm = (min - max) == 0
													? cm[127]  //color double[r,g,b]
													: cm[ (int)(( x[2] -min )/(max - min)*255) ] ,
								Gry = (int)(( x[2] -min )/(max - min)*255 + 1)
							}).ToList();

			var circleLst2 = xyCm2.Select((x,i) =>
									new
									{
										pos = new System.Drawing.Point(
																			(int)(x.X*sizemultiflier)+imgshiftoffset,
																			(int)(x.Y*sizemultiflier)+imgshiftoffset),

										color = new MCvScalar(x.Cm[2]*255 , x.Cm[1]*255 , x.Cm[0]*255).Act( test22 )
									}).ToArray();
			"End Point".Print();
			"".Print();
			"".Print();
			"".Print();
			#endregion
			var imgsize = Math.Max(
				 xyCm2.Select( x => x.X).Max(),
				 xyCm2.Select( x => x.Y).Max()
				);

			var imgData = new byte[(int)(imgsize*sizemultiflier + imgshiftoffset*2.0) ,(int)(imgsize*sizemultiflier+imgshiftoffset*2.0),3];
			Image<Bgr,byte> img = new Image<Bgr, byte>(imgData);
			circleLst2.ActLoop( x => CvInvoke.Circle( img, x.pos, dotSize, x.color, -1, Emgu.CV.CvEnum.LineType.EightConnected ) );
			*/
			
			for ( int i = 0 ; i < divide ; i++ )
			{
				srcdatas = srcdatas.Interpol_Theta( 1 ).Select( x => x ).ToList()
								   .Interpol_Rho( 1 ).Select( x => x ).ToList();
			}

			// Extact Color Position
			var xyCm = srcdatas.ToCartesianReslt()
							.OrderBy( x=> x[0])
							.ThenBy( x => x[1])
							.AsParallel()
							.AsOrdered()
							//.Select( x => new double[] { x[0] , x[1] , ( x[2] - mean ) / std } )
							//.Select( x => new double[] { x[0] , x[1] , x[2] >  1.96 ? 1.96 :
							//										   x[2] < -1.96 ? -1.96
							//										   : x[2] } )
							.Select( x => new
							{
								X  = offset + x[0] ,
								Y  = offset + x[1] ,
								Cm = (min - max) == 0
													? cm[127]  //color double[r,g,b]
													: cm[ (int)(( x[2] -min )/(max - min)*255) ] ,
								Gry = (int)(( x[2] -min )/(max - min)*255 + 1)
							}).ToList();

			var imgsize = Math.Max(
				 xyCm.Select( x => x.X).Max(),
				 xyCm.Select( x => x.Y).Max()
				);

			var imgData = new byte[(int)(imgsize*sizemultiflier + imgshiftoffset*2.0) ,(int)(imgsize*sizemultiflier+imgshiftoffset*2.0),3];
			Image<Bgr,byte> img = new Image<Bgr, byte>(imgData);
			var circleLst = xyCm.Select((x,i) =>
									new
									{
										pos = new System.Drawing.Point(
																			(int)(x.X*sizemultiflier)+imgshiftoffset,
																			(int)(x.Y*sizemultiflier)+imgshiftoffset),

										color = new MCvScalar(x.Cm[2]*255 , x.Cm[1]*255 , x.Cm[0]*255)
									});



			circleLst.ActLoop( x => CvInvoke.Circle( img , x.pos , dotSize , x.color , -1 , Emgu.CV.CvEnum.LineType.EightConnected ) );
			//circleLst.ActLoop( (x,i) => CvInvoke.Circle( grayimg , x.pos , dotSize , new MCvScalar( xyCm[i].Gry) , -1 , Emgu.CV.CvEnum.LineType.EightConnected ) );

			img = img.Median( 5 );
			img = img.SmoothGaussian( 3 );

			return img; 
		}

		public static Image<Bgr , byte> CreateScalebar()
		{
			var scalebar = new byte[255,2,3];
			var cm = new ColorMap().Inferno_cm;
			for ( int i = 0 ; i < 255 ; i++ )
			{
				scalebar [ i , 0 , 0 ] = ( byte )( cm [ i ] [ 2 ] * 255 );
				scalebar [ i , 0 , 1 ] = ( byte )( cm [ i ] [ 1 ] * 255 );
				scalebar [ i , 0 , 2 ] = ( byte )( cm [ i ] [ 0 ] * 255 );
				scalebar [ i , 1 , 0 ] = ( byte )( cm [ i ] [ 2 ] * 255 );
				scalebar [ i , 1 , 1 ] = ( byte )( cm [ i ] [ 1 ] * 255 );
				scalebar [ i , 1 , 2 ] = ( byte )( cm [ i ] [ 0 ] * 255 );
			}

			return new Image<Bgr , byte>( scalebar );
		}
	}

}
