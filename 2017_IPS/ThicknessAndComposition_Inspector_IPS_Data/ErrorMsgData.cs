using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ApplicationUtilTool.Log;
using ModelLib.TypeClass;

namespace ThicknessAndComposition_Inspector_IPS_Data
{
	public enum ErrorType { StgConnectErr , SpecConnectErr , CalcErr  }
	public class IPSErrorMsgData
	{
		Dictionary<ErrorType , string> ToErrMsg;
		Logger Loger;

		public IPSErrorMsgData( Logger loger )
		{
			Loger = loger;
			ToErrMsg = new Dictionary<ErrorType , string>();
			ToErrMsg.Add( ErrorType.StgConnectErr , "Stage Malfunction. Please Restart stage controller" );
			ToErrMsg.Add( ErrorType.SpecConnectErr , "Spectrometer Malfunction. Please reconnect spectrometer controller" );
			ToErrMsg.Add( ErrorType.CalcErr , "Internal Data Analysis Fail. Please check sample or other posibility" );
		}

		public void WriteShowErr( ErrorType ty )
		{
			MessageBox.Show( ToErrMsg [ ty ] );
			Loger.Log( ToErrMsg [ ty ] , true);
		}

	}

	public static class ErrExt
	{
		public static string Log(
			this string log ,
			Logger logger )
		{
			logger.Log( log , true );
			return log;
		}

	}
		
}
