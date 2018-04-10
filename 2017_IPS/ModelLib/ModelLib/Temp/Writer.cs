using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib.Monad
{
    // writer 에서 구현할것 
    // writer 모나드는 바인딩 할때, nothing 값이면 로그에 추가, 아니면 함수적용후 새로운 Writer 리턴 하는 것으로.
    // 새로운  Writer는 기존의 Writer에 저장되 있던 로그를 그대로 가져온다. 
    //  이름을 Writer 가 아닌 새로운 이름으로 해줘야 겠다.  


    public class Writer<Tval, Tlog, Tlogger> : IEnumerable<Tval> where Tlogger : IWriterLog<Tlog>
    {
        public Tval Value { get; private set; }
        public Tlogger Logger;

        public Writer( Tval value , Tlogger logger )
        {
            Value = value;
            Logger = logger;
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public IEnumerable<Tval> ToEnumerable()
        {
            yield return Value;
        }

        public IEnumerator<Tval> GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return null;
        }
    }

    public static class WriterExt
    {
        //public static Writer<Tval, Tlog , Tlogger> ToWriter<Tval,Tlog, Tlogger>(
        //    this Tval value ,
        //    Tlogger logger)
        //{
        //    return new Writer<Tval , Tlog , Tlogger >(value,logger);
        //}
       
    }

}
