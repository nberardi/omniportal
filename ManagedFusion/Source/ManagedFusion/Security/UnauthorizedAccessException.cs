using System;
using System.Net;
using System.Web;

namespace ManagedFusion.Security
{
	public class UnauthorizedAccessException : HttpException
	{
		public UnauthorizedAccessException(string username, string urlPath)
			: base((int)HttpStatusCode.Unauthorized, String.Format("Access to {0} by {1} is denied.", urlPath, username)) { }
	}
}
