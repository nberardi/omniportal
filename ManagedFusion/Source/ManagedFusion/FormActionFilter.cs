#region Copyright © 2004, Nicholas Berardi
/*
 * ManagedFusion (www.ManagedFusion.net) Copyright © 2004, Nicholas Berardi
 * All rights reserved.
 * 
 * This code is protected under the Common Public License Version 1.0
 * The license in its entirety at <http://opensource.org/licenses/cpl.php>
 * 
 * ManagedFusion is freely available from <http://www.ManagedFusion.net/>
 */
#endregion

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;

// ManagedFusion Classes
using ManagedFusion;

namespace ManagedFusion
{
	/// <summary>
	/// This class modifies the action attribute of the form tag inorder to correct the POST location.
	/// </summary>
	class FormActionFilter : Stream
    {
		private Stream responseStream;
		private long position;
		private StringBuilder responseHtml;

		public FormActionFilter (Stream inputStream)
        {
            responseStream = inputStream;
            responseHtml = new StringBuilder ();
        }

        #region Filter

        public override bool CanRead { get { return true; } }

        public override bool CanSeek { get { return true; } }

        public override bool CanWrite { get { return true; } }

        public override void Close()
        {
            responseStream.Close ();
        }

        public override void Flush()
        {
            responseStream.Flush ();
        }

		public override long Length { get { return 0L; } }

		public override long Position 
		{ 
			get { return position; }
			set { position = value; }
		}

        public override long Seek(long offset, SeekOrigin origin)
        {
            return responseStream.Seek (offset, origin);
        }

        public override void SetLength(long length)
        {
            responseStream.SetLength (length);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return responseStream.Read (buffer, offset, count);
        }

        #endregion

        #region Dirty work

        public override void Write(byte[] buffer, int offset, int count)
        {
            string strBuffer = System.Text.UTF8Encoding.UTF8.GetString (buffer, offset, count);

            // ---------------------------------
            // Wait for the closing </html> tag
            // ---------------------------------
            Regex eof = new Regex ("</html>", RegexOptions.IgnoreCase);

			if (!eof.IsMatch (strBuffer))
			{
				responseHtml.Append (strBuffer);
			}
			else
			{
				responseHtml.Append (strBuffer);
				
				string finalHtml = responseHtml.ToString ();

				// Replace the form tag with one that is complient with ManagedFusion
				Regex re = new Regex("<form name=\"aspnetForm\" method=\"post\" action=\"(?<action>.*)\" id=\"aspnetForm\">", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
				finalHtml = re.Replace(finalHtml, new MatchEvaluator(FormMatch));

				// Write the formatted HTML back
				byte[] data = System.Text.UTF8Encoding.UTF8.GetBytes (finalHtml);

				responseStream.Write (data, 0, data.Length);            
			}
        }

		//---------------------------------------------------------------------------
		private string FormMatch (Match m)
		{
			string oldAction = m.Groups["action"].Value;
			string newAction = Common.Context.Request.RawUrl;

			// replace the old action with the new action
			return m.ToString().Replace(oldAction, newAction);
		}

        #endregion
    }
}