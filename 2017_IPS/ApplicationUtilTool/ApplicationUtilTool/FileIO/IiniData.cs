using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUtilTool.FileIO
{
    public interface IiniData<Tdata> : IEnumerable<string>
    {
        Dictionary<string , string> inirawData { get; set; }
        Tdata ToData( Dictionary<string , string> rawdata );
        Dictionary<string , string> ToDic( Tdata rawdata );
         
    }
}
