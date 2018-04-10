using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib.Monad
{
    public interface ILogData<T>
    { 
        T Logs { get; set; }
        ILogData<T> AppendLog( T log ) ;
        ILogData<T> AppendError( T log ) ;
        ILogData<T> AppendEmpty() ;

    }

    public class TestLogData : ILogData<string>
    {
        public string Logs { get; set; }
        public ILogData<string> AppendLog( string log )
        {
            Logs = Logs + DateTime.Now.ToString( "HH_mm_ss" ) + log;
            return this;
        }

        public ILogData<string> AppendError( string log )
        {
            Logs = Logs + DateTime.Now.ToString( "HH_mm_ss" ) + "Error : " + log;
            return this;
        }

        public ILogData<string> AppendEmpty()
        {
            return this;
        }  
    }

}
