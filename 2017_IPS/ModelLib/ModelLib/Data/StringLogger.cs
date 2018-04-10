using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib.Data
{
    public class StringLogger : ILogData
    {
        public List<string> Logs
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void AddError( string log )
        {
            throw new NotImplementedException();
        }

        public void AddNormal( string log )
        {
            throw new NotImplementedException();
        }
    }
    public class test<T>
    {
        ILogData temp = new StringLogger();

    }
}
