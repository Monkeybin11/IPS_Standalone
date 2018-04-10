using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib.Monad
{
    public interface TWriter { }
    public class TestWriter<T, L , LT> where L : ILogData<LT>
    {
        public Maybe<T> Value { get; set; }
        public L Log;

        public TestWriter(T val , L log)
        {
            if ( val == null ) 

            Value = val == null
                    ? (Maybe<T>) new Nothing<T>()
                    : (Maybe<T>) new Just<T>( val );
            Log = log;
        }
    }

    public static class TWriterExt
    {
        //public static Maybe<T> runWriter<T, L>(
        //    this TestWriter<T , L> src ,
        //    Func< T, TestWriter<T , L> > func )
        //{
        //    var obj = src.Value as Just<T>;
        //    if ( obj != null )
        //    {
        //        var result = func(obj.Value);
        //        var output = new Writer<T,l>
        //
        //    }
        //    else
        //    {
        //        
        //    }
        //}
        public static TestWriter<T , L , LT> Pass<T, L, LT>(
            this TestWriter<T , L , LT> src ,
            T val ,
            L log 
            ) where L : ILogData<LT>
        {
            return null;
        }

        public static TestWriter<T , L , LT> Bind<T, L , LT>(
            this TestWriter<T,L,LT> src,
            LT log,
            Func<T , TestWriter<T,L,LT>> func
            ) where L : ILogData<LT>
        {

            return null;
        }
    }
        


}
