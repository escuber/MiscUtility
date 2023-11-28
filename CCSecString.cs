using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace CardCells.utility
{


	public unsafe class CCSecString
	{

		public static string get_original(string data)
		{
			var myss = new SecureString();
			myss.AppendProtectedData(Convert.FromBase64String(data));
			string val = myss.Unsecure();
			return val;
		}

		public static string get_encrypted(string data)
		{
			if (string.IsNullOrEmpty(data)) return "";
			SecureString myss = null;

			fixed (char* pChars = data.ToCharArray())
			{
				myss = new SecureString(pChars, data.Length);
			}
			return Convert.ToBase64String(myss.GetProtectedData());
		}

		public static string get(int digits)
		{

			string rtrn;

			string upc = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 11);

			var lng = long.Parse(upc, System.Globalization.NumberStyles.HexNumber); //, null, out result);
			upc = lng.ToString().PadLeft(11, '0');

			//upc = upc.Substring(0, 11);
			string upcFinal = upc;

			upc = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 11);

			lng = long.Parse(upc, System.Globalization.NumberStyles.HexNumber); //, null, out result);
			upc = lng.ToString(); //.PadLeft(11, '0');

			upcFinal += upc;

			upc = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 11);

			lng = long.Parse(upc, System.Globalization.NumberStyles.HexNumber); //, null, out result);
			upc = lng.ToString();
			upcFinal += upc;
			rtrn = upcFinal.Substring(0, digits);
			return rtrn;

		}
	}
}