using System;
using System.Net;
using System.Web;

namespace ManagedFusion
{
	public class PortalInitializationException : HttpException
	{
		public PortalInitializationException(HttpStatusCode status, string message) : base((int)status, message) { }

		public PortalInitializationException(string message) : this(HttpStatusCode.NotFound, message) { }
	}
}
