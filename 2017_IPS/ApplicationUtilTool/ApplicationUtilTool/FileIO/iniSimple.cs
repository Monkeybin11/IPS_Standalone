using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUtilTool.FileIO
{
    public class iniSimple
    {
        public string Path = @"C:\Users\idiol\Desktop\T__tdi_4inch.ini";
        public string CommonSectionName;
        public Dictionary<string , string> inirawData { get; set; }


        public iniSimple(string path , string sectionname = "Stream Conditioning" )
        {
            if(Path == null) Path = path;
            CommonSectionName = sectionname;

            //inirawData = new Dictionary<string , string>();
            //inirawData.Add( "speed" , "" );
            //inirawData.Add( "zpos" , "" );
            //inirawData.Add( "linerate" , "" );
            //
            //if(!File.Exists( Path ) ) File.Create( Path );
        }

        public static class iniTool
        {
            [DllImport( "kernel32" )]
            public static extern long WritePrivateProfileString( string section , string key , string val , string filePath );
            [DllImport( "kernel32" )]
            public static extern int GetPrivateProfileString( string section , string key , string def , StringBuilder retVal ,
                                                            int size , string filePath );
        }

        public void Loadini()
        {
            List<string> stringvalList = new List<string>();
            var str = new StringBuilder(1024);

            iniTool.GetPrivateProfileString( CommonSectionName , "speed"     , "" , str , 1024 , Path );
            stringvalList.Add( str.ToString() );

            iniTool.GetPrivateProfileString( CommonSectionName , "zpos"      , "" , str , 1024 , Path );
            stringvalList.Add( str.ToString() );

            iniTool.GetPrivateProfileString( CommonSectionName , "linerate" , "" , str , 1024 , Path );
            stringvalList.Add( str.ToString() );
        }

        public void Writeini()
        {
            iniTool.WritePrivateProfileString( CommonSectionName , "Crop Height" , "33333" , Path );
            iniTool.WritePrivateProfileString( CommonSectionName , "Scale Vertical" , "33333" , Path );
            //iniTool.WritePrivateProfileString( CommonSectionName , "linerate" , "1070" , Path );
        }
    }
}
