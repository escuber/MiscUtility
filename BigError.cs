using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace CardCells.utility
{
	public static class BigError
	{

		public static void Notify(string errorMsg)
		{
			cclog.WaitLog("**** big error  *********************");



			GMail.SendEmail("escuber@gmail.com","*****  BIG ERROR  *****",errorMsg);




		}

	}
}