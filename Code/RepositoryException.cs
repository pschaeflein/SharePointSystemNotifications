using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schaeflein.Community.SystemNotification.Data
{
	public class RepositoryException : Exception
	{
		public string DataContextUrl { get; set; }
		public string GeneratedCAML { get; set; }

		public RepositoryException() { }

		public RepositoryException(string message, Exception ex)
			: base(message, ex)
		{
		}
	}
}
