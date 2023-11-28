using System;
using System.Net.Http;
using System.Web;
using System.Web.Http.WebHost;

namespace MiscUtility
{
	//public interface IHostBufferPolicySelector
	//{
	//	bool UseBufferedInputStream(object hostContext);
	//	bool UseBufferedOutputStream(HttpResponseMessage response);
	//} 


	public class PolicySelectorWithoutBuffer : WebHostBufferPolicySelector
	{
		public override bool UseBufferedInputStream(object hostContext)
		{
			var context = hostContext as HttpContextBase;
			if (context != null)
			{
				if (
					string.Compare(context.Request.RequestContext.RouteData.Values["controller"].ToString(), "uploading",
					               StringComparison.InvariantCultureIgnoreCase) == 0)
					return false;
			}
			return true;
		}

		public override bool UseBufferedOutputStream(HttpResponseMessage response)
		{
			return base.UseBufferedOutputStream(response);
		}
	}
}