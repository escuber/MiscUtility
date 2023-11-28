using System;
using System.Configuration;

namespace CardCells.utility
{
	public class CCConfig
	{
		public static string _env = "";
	    public static bool _initialized = false;
	    
        
        public static bool _isDMT = false;
	    public static bool IsDMT
	    {
	        get
	        {
	            string x = "";
                if (!_initialized) 
                    x = env;
	            return _isDMT;
	        }
	    }
        public static bool _isSEC = false;
        public static bool  IsSec
        {
            get
            {
                string x = "";
                if (!_initialized)
                    x = env;
                return _isSEC;
            }
        }

        public static string rawEnv
        {
            get
            {

                var eprts = env.Split('_');
                var rawEnv= eprts[eprts.Length - 1];

                return rawEnv;
            }
        }
	    public static string env
		{
			get
			{
				if (_env == "")
				{

					_env = ConfigurationSettings.AppSettings[MachineName + "_env"];
                    
                    _isDMT = _env.ToLower().Contains("dmt");
				    _isSEC = !_isDMT;

				    _initialized = true;
				}
				return _env;
			}
		}

		public static string _machineName = "";

		public static string MachineName
		{
			get
			{
				if (_machineName == "")
				{
					_machineName = System.Environment.MachineName.ToLower();
				}
				return _machineName;
			}
		}

		public static string getMachVal(string key)
		{
			string rtrn= ConfigurationSettings.AppSettings[env + "_" + key];
		    if (string.IsNullOrEmpty(rtrn))
		    {

                Console.WriteLine("Config Value: "+key);
                Console.WriteLine("*************  Not Found*******************");

		    }
		    return rtrn;
		}

		public static string getVal(string key)
		{
			string rtrn=ConfigurationSettings.AppSettings[ key];
            if (string.IsNullOrEmpty(rtrn))
            {

                Console.WriteLine("Config Value: " + key);
                Console.WriteLine("*************  Not Found*******************");

            }
            return rtrn;
		}

	}
}