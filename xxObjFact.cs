using System;
using System.Collections.Generic;
using System.Reflection;


namespace CardCells.utility
{
	public class ObjFact
	{
		// ////////////////////////////////////////////////////////////////////////////
		//
		public static object FireObjectMethod(string FullMethodString,
		                                      object[] MethodArgs)
		{
			//
			//
			//

			string[] parts = FullMethodString.Split('|');
			string assemblyname = parts[0];
			string objName = parts[1];
			string meth = parts[2];

			object xo = getObject(assemblyname, objName);
			object ret = fireMethod(xo, meth, MethodArgs);

			return ret;
		}

		// ////////////////////////////////////////////////////////////////////////////
		//
		public static object getObjectfromString(string objectString)
		{
			//
			//
			//
			var objData = ObjFact.splitValues(objectString);
			var serObject = ObjFact.getObjectfromString(objData["assembly"], objData["type"]);

			return serObject;

			//
		}


		// ////////////////////////////////////////////////////////////////////////////
		//
		public static object getObjectfromString(string assName,
		                                         string objectName)
		{
			//
			//
			//
			Assembly ass;
			ass = Assembly.Load(assName);
			object xobj = ass.CreateInstance(objectName, true);
			return xobj;

			//
		}

		// ////////////////////////////////////////////////////////////////////////////
		//
		public static Type getTypeFromString(string objString)
		{
			//
			//
			//
			var objData = ObjFact.splitValues(objString);
			var serObject = ObjFact.getTypeFromString(objData["assembly"], objData["type"]);
			return serObject;
		}

		// ////////////////////////////////////////////////////////////////////////////
		//
		public static Type getTypeFromString(string assName,
		                                     string objectName)
		{
			//
			//
			//
			Assembly ass;
			assName = assName.Trim();
			ass = Assembly.LoadWithPartialName(assName);

			objectName = objectName.Trim();
			Type ty = ass.GetType(objectName);

			return ty;
		}

		// ////////////////////////////////////////////////////////////////////////////
		//
		public static Type getClass(string assName,
		                            string objectName)
		{
			//
			//
			//


			Assembly ass;
			assName = assName.Trim();
			ass = Assembly.LoadWithPartialName(assName);

			objectName = objectName.Trim();
			Type ty = ass.GetType(objectName);

			//object xobj = ass.CreateInstance(objectName,true);


			return ty;
			//
			//
			//
			//
		}

		// ////////////////////////////////////////////////////////////////////////////
		//
		public static object getObject(string assName,
		                               string objectName)
		{
			Assembly ass = Assembly.Load(assName);
			Type ty = ass.GetType(objectName);
			object xobj = ass.CreateInstance(objectName, true);

			return xobj;
		}


		// ////////////////////////////////////////////////////////////////////////////
		//
		public static object getPropValue(object xobj,
		                                  string PropName)
		{
			//
			//
			//

			PropertyInfo pi = null;
			object ret = null;


			try
			{
				Type t = xobj.GetType();
				pi = t.GetProperty(PropName);

				ret = pi.GetValue(xobj, null);
			}
			catch
			{
				return null;
			}


			return ret;
		}

		// ////////////////////////////////////////////////////////////////////////////
		//
		public static void setProp(object xobj,
		                           string PropName,
		                           object[] methArgs)
		{
			//
			//
			//


			if (methArgs.Length == 0)
			{
				//JLogging.WaitLog("No args, fake call");
				return;
			}

			PropertyInfo pi = null;
			string ret = "";

			//JLogging.WaitLog("Setting prop:"+PropName);

			Type t = xobj.GetType();
			pi = t.GetProperty(PropName);


			pi.SetValue(xobj, methArgs[0], null);


			return;
		}

		// ////////////////////////////////////////////////////////////////////////////
		//
		public static object fireMethod(object xobj,
		                                string MethodName)
		{
			//
			//
			//

			MethodInfo mi = null;
			string ret = "";


			Type t = xobj.GetType();
			mi = t.GetMethod(MethodName);


			ret = (string) mi.Invoke(xobj, new object[] {});


			return ret;
		}

		// ////////////////////////////////////////////////////////////////////////////
		//
		public static object fireMethod(object xobj,
		                                string MethodName,
		                                object[] methArgs)
		{
			//
			//
			//
			
			if (methArgs.Length == 0)
			{
				//JLogging.WaitLog("No args, fake call");
				return "";
			}

			MethodInfo mi = null;
			string ret = "";


			Type t = xobj.GetType();
			mi = t.GetMethod(MethodName);


			ret = (string) mi.Invoke(xobj, methArgs);


			return ret;
		}

		private static int mInstanceCount = 0;
		// /////////////////////////////////////////////////////////////////////////////////////////
		//
		public string StartHold(string doc)
		{
			//
			//
			//

			mInstanceCount += 1;
			int myinst = mInstanceCount;

			//JLogging.WaitLog("Holding: " +mInstanceCount);


			//while(x>0)
			//{
			System.Threading.Thread.Sleep(20000);

			//}

			//JLogging.WaitLog("ending: " +mInstanceCount);


			return "";
		}

		public static Dictionary<string, string> splitValues(string txt)
		{
			string temp = txt;
			var valDic = new Dictionary<string, string>();
			while (temp.Contains("="))
			{
				int pos1 = temp.IndexOf("=");
				//var key = temp.Substring(0, pos1).Trim().ToLower();
				var key = temp.Substring(0, pos1).Trim();

				temp = temp.Remove(0, pos1 + 1);
				temp = temp.TrimStart('"');

				pos1 = temp.IndexOf("\"");
				//var value = temp.Substring(0, pos1).Trim().ToLower();
				var value = temp.Substring(0, pos1).Trim();
				temp = temp.Remove(0, pos1);
				temp = temp.TrimStart('"');
				valDic.Add(key, value);
			}

			return valDic;
		}
	}
}