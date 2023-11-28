//////////////////////////////////////////////////////////////////////////////////////
//																					//
//	cclog.cs																		//
//																					//
//	Purpose:																		//
//																					//
//																					//
//																					//
// ********************************************************************************	//
//																					//
//	Created By:	jim gaudette														//
//	email:		escuber@hotmail.com													//
//	completed:	winter 2002															//
//																					//
//																					//
//////////////////////////////////////////////////////////////////////////////////////

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;

//using Microsoft.Win32.Security;
//using System.Threading;

namespace CardCells.utility
{
	public class cclog
	{
		private static cclog LockObj = new cclog();

		public cclog()
		{
			//
			//
			//
		}

		// Request permission to create and write files to C:\TestTest.


		//Random r=new Random();
		// /////////////////////////////////////////////////////////////////////////////////////////
		//
		public static bool WriteToFile(string s)
		{
			//
			//
			//
			try
			{
				WaitLog(s);
			}
			catch //(Exception e)
			{
				//string em=e.Message;
				return false;
			}
			return true;
		}

		public static int myInstinx = 0;
		// /////////////////////////////////////////////////////////////////////////////////////////
		//
		public static void WaitSuccessLog(string s)
		{
			//
			//
			//


			///int cnt=0;

			WaitLog(s);
		}

		// ////////////////////////////////////////////////
		//
		public static void xmlwaitLog(string x)
		{
			x = cclog.MakeXMLPretty(x);
			WaitLog(x);
		}

		//[assembly: FileIOPermissionAttribute(SecurityAction.RequestMinimum, All = @"C:\EsLog.txt")]
		// /////////////////////////////////////////////////////////////////////////////////////////
		//
		public static void WaitLog(string s)
		{

			string xinst = "" + "";
			xinst = string.Format("[{0,-5}]", xinst);

			string xouts = string.Format("[{0,-22}][{2,-8}] {1}", (DateTime.Now).ToString(DateTimeFormatInfo.CurrentInfo), s,
										xinst);
			Debug.WriteLine(xouts);
			Console.WriteLine(xouts);
            return;
			//
			// does not log huge messages it is just silly
			//
			string logoption = "" + //CCConfig.getMachVal
			                   CCConfig.getMachVal("cclog");
			if (logoption.Length != 0)
			{
				if (logoption == "nolog") return;
			}

			string sloglimit = "0" + CCConfig.getMachVal("log_message_limit");
			int loglimit = 500000;
			if (sloglimit != "0")
			{
				loglimit = int.Parse(sloglimit);
			}


			if (s.Length > loglimit)
			{
				loglimit = ((loglimit > s.Length) ? s.Length : loglimit);


				string len = "" + s.Length;
				string first = s.Substring(0, loglimit);
				s = string.Format("Attempt to log message with size={0}\r\n{1}\r\n", len, first);
			}
			lock (typeof (cclog))
			{
				bool bHitFile = false;
				int retry = 100;

				string inst = "" + "";
				inst = string.Format("[{0,-5}]", inst);

				string outs = string.Format("[{0,-22}][{2,-8}] {1}", (DateTime.Now).ToString(DateTimeFormatInfo.CurrentInfo), s,
				                            inst);
				Debug.WriteLine(outs);
				Console.WriteLine(outs);
				string fname = "";
				while (!bHitFile)
				{
					if (retry <= 0) break;
					retry--;

					try
					{
						FileStream fs = null;
						try
						{
							fname = "" + getLogFileName();
							if (fname.Length == 0) fname = @"c:\cclog.txt";

							FileInfo fi = new FileInfo(fname);
							long maxsize = 2000000000000;

							StreamWriter ostreamWriter = null;

							if (fi.Length > maxsize)
							{
								int binx = 0;
								string[] prts = fname.Split('.');
								prts[0] += binx;
								string bname = string.Join(".", prts);


								FileInfo fi2 = new FileInfo(bname);
								//FileInfo fiold = new FileInfo(fname);
								//fi2.Name+=""+binx;
								while (fi2.Exists)
								{
									//fi2.Name+=binx;
									binx++;

									prts = fname.Split('.');
									prts[0] += binx;
									bname = string.Join(".", prts);
									fi2 = new FileInfo(bname);
								}
								fi.CopyTo(fi2.FullName);
								fs = new FileStream(fname, FileMode.Truncate, FileAccess.Write);
								ostreamWriter = new StreamWriter(fs);


								//ostreamWriter.BaseStream.Seek(0, SeekOrigin.End);
							}
							else
							{
								fs = new FileStream(fname, FileMode.Append, FileAccess.Write);
								ostreamWriter = new StreamWriter(fs);
								ostreamWriter.BaseStream.Seek(0, SeekOrigin.End);
							}

							inst = "" + myInstinx.ToString();
							inst = string.Format("[{0,-5}]", inst);
							outs = string.Format("[{0,-22}][{2,-8}] {1}", (DateTime.Now).ToString(DateTimeFormatInfo.CurrentInfo), s, inst);

							outs = outs.Replace("" + '\r' + '\n', "" + '\r' + '\n'
							                                      + "                                ");
							Debug.WriteLine(outs);
							//System.Console.WriteLine(outs);
							ostreamWriter.WriteLine(outs);
							ostreamWriter.Flush();

							ostreamWriter.Close();
							bHitFile = true;
						}
						catch (Exception e)
						{
							Debug.WriteLine("Logerror : " + e.Message);

							using (var mfs = File.OpenWrite(fname))
							{
								mfs.Write(new byte[] {}, 0, 1);
							}
							WriteEventLog("cclog error: " + e.Message);

							//Thread.Sleep(200);
						}
						finally
						{
							try
							{
								fs.Close();
							}
							catch
							{
							}
						}
					}
					catch
					{
					}
				}
			}
		}


		/*
				static int logfilehold=0;
		*/
		// /////////////////////////////////////////////////////////////////////////////////////////
		//
		public static void WaitLog(string s, string FileName)
		{
			//
			//
			//
			//object o = new Object();


			string logoption = "" + CCConfig.getMachVal("cclog");
			if (logoption.Length != 0)
			{
				if (logoption == "nolog") return;
			}


			lock (typeof (cclog))
			{
				bool bHitFile = false;
				int retry = 100;

				string inst = "" + "";
				inst = string.Format("[{0,-5}]", inst);

				string outs = string.Format("[{0,-22}][{2,-8}] {1}", (DateTime.Now).ToString(DateTimeFormatInfo.CurrentInfo), s,
				                            inst);
				Debug.WriteLine(outs);
				Console.WriteLine(outs);
				while (!bHitFile)
				{
					if (retry <= 0) break;
					retry--;

					try
					{
						FileStream fs = null;
						try
						{
							string fname = getLogFileName();

							fs = new FileStream(fname, FileMode.Append, FileAccess.Write);
							StreamWriter ostreamWriter = new StreamWriter(fs);
							ostreamWriter.BaseStream.Seek(0, SeekOrigin.End);

							inst = "" + myInstinx.ToString();
							inst = string.Format("[{0,-5}]", inst);
							outs = string.Format("[{0,-22}][{2,-8}] {1}", (DateTime.Now).ToString(DateTimeFormatInfo.CurrentInfo), s, inst);
							Debug.WriteLine(outs);
							Console.WriteLine(outs);
							ostreamWriter.WriteLine(outs);
							ostreamWriter.Flush();

							ostreamWriter.Close();
							bHitFile = true;
						}
						catch (Exception e)
						{
							Debug.WriteLine("Logerror : " + e.Message);
							Thread.Sleep(200);
						}
						finally
						{
							try
							{
								fs.Close();
							}
							catch
							{
							}
						}
					}
					catch
					{
					}
				}
			}
		}


		public static string getLogFileName()
		{
			string fname = "";
			try
			{
				if (mLogFile != null)
				{
					return mLogFile;
				}


				fname = ""+CCConfig.getMachVal("logfilename");
				if (fname.Length <= 0) fname = @"c:\xdlog.txt";

				FileInfo fi = new FileInfo(fname);

				if (!fi.Exists)
				{
					CreateLogFile(fname);
					//System_n_Security.SetfileEveryone(fname);
				}
			}
			catch (Exception e)
			{
				cclog.WriteEventLog("Error creating logfile:" + e.ToString());
			}

			mLogFile = fname;
			return mLogFile;
		}

		public static string mLogFile;

		// /////////////////////////////////////////////////////////////////////////////////////////
		//
		public static void CreateLogFile(string fname)
		{
			//
			//
			//


			if (fname.Length == 0)
			{
				fname = @"c:\xdlog.txt";
			}

			FileStream fs = null;
			try
			{
				FileInfo fi = new FileInfo(fname);
				StreamWriter ostreamWriter = null;


				fs = new FileStream(fname, FileMode.OpenOrCreate, FileAccess.Write);
				ostreamWriter = new StreamWriter(fs);
				ostreamWriter.BaseStream.Seek(0, SeekOrigin.End);

				ostreamWriter.Write("");
				ostreamWriter.Flush();

				ostreamWriter.Close();
			}
			catch (Exception e)
			{
				Debug.WriteLine("Logerror : " + e.Message);
				WriteEventLog("cclog error: " + e.Message);

				//Thread.Sleep(200);
			}
			finally
			{
				try
				{
					fs.Close();
				}
				catch
				{
				}
			}

			//System_n_Security.SetfileEveryone(fname);
		}


		// /////////////////////////////////////////////////////////////////////////////////////////
		//
		public static void ClearLog()
		{
			//
			//
			//
			lock (typeof (cclog))
			{
				bool bHitFile = false;
				int retry = 100;

				string inst = "" + "";
				inst = string.Format("[{0,-5}]", inst);

				string outs = "";
				Debug.WriteLine(outs);
				Console.WriteLine(outs);
				while (!bHitFile)
				{
					if (retry <= 0) break;
					retry--;

					try
					{
						FileStream fs = null;
						try
						{
							string fname = getLogFileName();

							FileInfo fi = new FileInfo(fname);
							//long maxsize = 2000000000000;

							StreamWriter ostreamWriter = null;


							fs = new FileStream(fname, FileMode.Create, FileAccess.Write);
							ostreamWriter = new StreamWriter(fs);
							ostreamWriter.BaseStream.Seek(0, SeekOrigin.End);


							outs = "";


							ostreamWriter.WriteLine(outs);
							ostreamWriter.Flush();

							ostreamWriter.Close();
						}
						catch (Exception e)
						{
							Debug.WriteLine("Logerror : " + e.Message);
							WriteEventLog("cclog error: " + e.Message);

							//Thread.Sleep(200);
						}
						finally
						{
							try
							{
								fs.Close();
							}
							catch
							{
							}
						}
					}
					catch
					{
					}
				}
			}
		}


		/*
				static int filehold=0;
		*/
		// /////////////////////////////////////////////////////////////////////////////////////////
		//
		public static void WaitLog_notime(string s, string FileName)
		{
			//
			//
			//
			object o = new object();
			lock (o)
			{
				bool bHitFile = false;
				int retry = 100;

				string inst = "" + "";
				inst = string.Format("[{0,-5}]", inst);

				//string outs = string.Format("[{0,-22}][{2,-8}] {1}",(DateTime.Now).ToString( DateTimeFormatInfo.CurrentInfo),s,inst);
				//System.Diagnostics.Debug.WriteLine(outs);
				//System.Console.WriteLine(outs);
				while (!bHitFile)
				{
					if (retry <= 0) break;
					retry--;

					try
					{
						FileStream fs = null;
						try
						{
							string fname;
							try
							{
								fname = FileName;
								if (fname.Length <= 0) fname = @"c:\xdlog.txt";
							}
							catch
							{
								fname = @"c:\xdlog.txt";
							}

							fs = new FileStream(fname, FileMode.Append, FileAccess.Write);
							StreamWriter ostreamWriter = new StreamWriter(fs);
							ostreamWriter.BaseStream.Seek(0, SeekOrigin.End);

							inst = "" + myInstinx.ToString();
							//inst= string.Format("[{0,-5}]",inst);
							//outs = string.Format("[{0,-22}][{2,-8}] {1}",(DateTime.Now).ToString( DateTimeFormatInfo.CurrentInfo),s,inst);
							Debug.WriteLine(s);
							Console.WriteLine(s);
							ostreamWriter.WriteLine(s);
							ostreamWriter.Flush();

							ostreamWriter.Close();
							bHitFile = true;
						}
						catch (Exception e)
						{
							Debug.WriteLine("Logerror : " + e.Message);
							Thread.Sleep(200);
						}
						finally
						{
							try
							{
								fs.Close();
							}
							catch
							{
							}
						}
					}
					catch
					{
					}
				}
			}
		}

		// ////////////////////////////////////////////////
		//
		public static void xpwl(string x)
		{
			x = cclog.MakeXMLPretty(x);
			wl(x);
		}


		// /////////////////////////////////////////////////////////////////////////////////////////
		//
		public static bool wl(string s)
		{
			string outs = string.Format("[{0,-22}][{2,-8}] {1}", (DateTime.Now).ToString(DateTimeFormatInfo.CurrentInfo), s,
			                            Thread.CurrentThread.GetHashCode());
			Debug.WriteLine(outs);
			Console.WriteLine(outs);

			return true;
		}


		// /////////////////////////////////////////////////////////////////////////////////////////
		//
		public static bool WriteToIncommingFile(string s)
		{
			//
			//	this funciton is added just to tract the document that came into the retry queue
			//
			//
			//
			//


			WaitLog(s);
			return true;
		}

		// /////////////////////////////////////////////////////////////////////////////////////////
		//
		public static void WriteToLog(string st)
		{
		}

		// /////////////////////////////////////////////////////////////////////////////////////////
		//
		public static bool WriteTooL(string s)
		{
			//
			//
			//
			try

			{
			}

			catch
			{
				return false;
			}

			return true;
		}

		public static void WriteException(Exception e, string location)
		{
			lock (LockObj)
			{
				cclog.WaitLog(" *******************************Exception ");
				cclog.WaitLog(" ");
				cclog.WaitLog("	Location: " + e.Source);
				cclog.WaitLog(" ");
				cclog.WaitLog("   Message:  " + e.Message);
				cclog.WaitLog(" ");
				cclog.WaitLog("***  Stack start:    ");
				cclog.WaitLog(e.StackTrace);
				cclog.WaitLog("***  Stack end:    ");
				cclog.WaitLog(" ");
				cclog.WaitLog(" *******************************EndException");
			}
		}

		public static void WriteException(Exception e)
		{
			lock (LockObj)
			{
				cclog.WaitLog(" *******************************Exception ");
				cclog.WaitLog(" ");
				cclog.WaitLog("	Location: " + e.Source);
				cclog.WaitLog(" ");
				cclog.WaitLog("   Message:  " + e.Message);
				cclog.WaitLog(" ");
				cclog.WaitLog("***  Stack start:    ");
				cclog.WaitLog(e.StackTrace);
				cclog.WaitLog("***  Stack end:    ");
				cclog.WaitLog(" ");
				cclog.WaitLog(" *******************************EndException");
			}
		}

		// /////////////////////////////////////////////////////////////////////////////////////////
		//
		public static void WriteToLog2(string st)
		{
			//
			//	Create the source, if it does not already exist.
			//
			if (EventLog.Exists("CIDSQueServer"))
			{
				EventLog.Delete("CIDSQueServer");
			}

			if (!EventLog.SourceExists("CIDSQueServer"))
			{
				EventLog.CreateEventSource("CIDSQueServer", "Application");
			}

			//
			// Create an EventLog instance and assign its source.
			//
			EventLog myLog = new EventLog();
			myLog.Log = "Application";
			myLog.Source = "CIDSQueServer";

			//
			//	Write an informational entry to the event log.
			//
			myLog.WriteEntry(st);
		}


		// /////////////////////////////////////////////////////////////////////////////////////////
		//
		public static void WriteEventLog(string st)
		{
			//
			//	Create the source, if it does not already exist.
			//

			if (!EventLog.SourceExists("XDA Error"))
			{
				EventLog.CreateEventSource("XDALog", "Application");
			}

			//
			// Create an EventLog instance and assign its source.
			//
			EventLog myLog = new EventLog();
			myLog.Log = "Application";


			myLog.Source = "XDA Error";

			//
			//	Write an informational entry to the event log.
			//
			myLog.WriteEntry(st, EventLogEntryType.Error);
		}

		// ////////////////////////////////////////////////////
		//
		public static string ReplaceString(string original,
		                                   string oldval,
		                                   string newval)
		{
			//
			//
			//
			original = original.Replace(oldval, newval);

			return original;
		}

		// /////////////////////////////////////////////////////////////////////////////////////////
		//
		public static string MakeXMLPretty(string doc)
		{
			//
			//
			//
			try
			{
				XmlDocument xdoc = new XmlDocument();
				xdoc.LoadXml(doc);

				StringBuilder sb = new StringBuilder();
				IndentedTextWriter itw = new IndentedTextWriter(new StringWriter(sb));

				XmlTextWriter xtw = new XmlTextWriter(itw.InnerWriter);

				xtw.Formatting = Formatting.Indented;
				xtw.Indentation = 4;

				xdoc.WriteTo(xtw);

				itw.Close();
				return sb.ToString();
			}
			catch (Exception e)
			{
				WriteException(e);
				cclog.WaitLog("cannot make pretty: " + doc);

				return doc;
			}
		}
	}
}