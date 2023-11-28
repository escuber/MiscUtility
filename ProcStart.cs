
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;



namespace CardCells.utility
{

	public static class ProcStart
	{

		public static string runWait(string exeWithPath,string parms)
		{

	

			

			ProcessStartInfo start = new ProcessStartInfo();
			start.FileName = exeWithPath; // Specify exe name.
			start.UseShellExecute = false;
			start.RedirectStandardOutput = true;
			start.Arguments = parms;

			//
			// Start the process.
			//
			string output = "";

			using (Process process = Process.Start(start))
			{
				//
				// Read in all the text from the process with the StreamReader.
				//
				using (StreamReader reader = process.StandardOutput)
				{
					output = reader.ReadToEnd();
				}
			}
			return output;

		}
	}
}