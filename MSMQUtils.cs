using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using CardCells.utility;

namespace CardCells.utility
{
	public class MSMQUtils
	{
		static void Main(string[] args)
		{
			//t();


			readmsgs(WriteMsg);
			System.Console.ReadLine();

			//readmsg();
			//sendMag();
		}
		public static bool IsQueueAvailable(string queueName)
		{
			var queue = new MessageQueue(queueName);
			try
			{
				queue.Peek(new TimeSpan(0, 0, 5));
				return true;
			}
			catch (MessageQueueException ex)
			{
				return ex.Message.StartsWith("Timeout");
			}
		}


		public static void ReadAllMsgs(Func<Message, bool> doWork)
		{

			var queuename = CCConfig.getMachVal("SyncInQueue");
			var q = new MessageQueue(queuename);

			while (q.CanRead)
			{ 
				readmsgs(doWork);
			}
		}

		public static void readmsgs(Func<Message,bool> doWork )
		{

			
			var queuename = CCConfig.getMachVal("SyncInQueue");

			var q = new MessageQueue(queuename);

			try
			{
				var msg=q.Peek();
				doWork(msg);
				msg=q.Receive();
			}
			catch (Exception)
			{
				System.Threading.Thread.Sleep(3000);					
			}			
		
		}


		public static bool WriteMsg(Message msg)
		{
			Console.WriteLine(msg.Label);
			return true;

		}

		private static MessageQueue _msgQueue = null;
		public static MessageQueue msgQueue
		{
			get
			{
				if (_msgQueue == null)
				{
					var queuename = CCConfig.getMachVal("SyncSendQueue");
					_msgQueue = new MessageQueue(queuename);					
				}
				return _msgQueue;
			}
		}

		public static bool queueBinMessage(byte[] body)
		{			
			var msg = new Message();
			var memstr = new MemoryStream(body);

			memstr.CopyTo(msg.BodyStream);
			
			msg.Recoverable = true;
			msgQueue.Send(msg);

			return true;
		}
		public static bool queueMessage(string body)
		{
			
			var msg = new Message();
			msg.Body = body;
			msg.Recoverable = true;

			msgQueue.Send(msg);
			return true;
		}		

		public static void sendMag()
		{
			string machineName = "ultra-lap";
			string qname = "x";

			//MessageQueue messageQueue = null;

			var fullname="FormatName:DIRECT=OS:" + machineName + @"\Private$\" + qname;


			if (!IsQueueAvailable(fullname))
			{
				Console.WriteLine("Got it");
			}

			var queue = new MessageQueue(fullname);
			queue.Send("first message");
			return;
			/*
			
			if (MessageQueue.Exists(fullname)) 
			{
				messageQueue = new MessageQueue("DIRECT=OS:"+machineName + @"\Private$\" + qname);
				messageQueue.Label = "Testing Queue";
			}
			else
			{
				// Create the Queue
				MessageQueue.Create("DIRECT=OS:" + machineName + @"\Private$\" + qname);
				messageQueue = new MessageQueue("DIRECT=OS:" + machineName + @"\Private$\" + qname);
				messageQueue.Label = "Newly Created Queue";
			}

			messageQueue.Send("First ever Message is sent to MSMQ", "Title");
		*/
		}
	}
}