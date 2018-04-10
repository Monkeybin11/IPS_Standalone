using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationUtilTool.FileIO;
using System.IO.Ports;

namespace MachineLib.DeviceLib.DalsaTDICamera
{
    public class DalsaTDICam_Addon
    {
        public Func<string , Action<int>> SetBufferHeight
            => fullpath
            => value
            =>
            {
                ccfTool ccftool = new ccfTool(fullpath);
                var changer = ccftool.AppendChangeList("Stream Conditioning");
                changer( @"Crop Height" )( value.ToString() );
                changer( @"Scale Vertical" )( value.ToString() );
                ccftool.RunChnage();
            };

        //public Func<CommandList , Action<string>> SetInnerParm
        //    => command
        //    => value
        //    =>
        //    {
        //        SerialPort port = new SerialPort();
        //
        //    };
    }
}
