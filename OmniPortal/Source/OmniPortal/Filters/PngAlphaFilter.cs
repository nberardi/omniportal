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
using System.Web.UI.WebControls;

// ManagedFusion Classes
using ManagedFusion;

namespace OmniPortal.Filters
{
	/// <summary>
	/// Filter for adding PNG-24 Alpha Transparency for viewing in Internet Explorer.
	/// </summary>
	/// <remarks>
	/// <para>Modifies IMG and INPUT tags for MSIE5+ browsers to ensure that PNG-24
	/// transparencies are displayed correctly.  Replaces original SRC attribute
	/// with a transparent GIF file (spacer.png) that is located in the same
	/// directory as the orignal image, and adds the STYLE attribute needed to for
	/// the browser. (Matching is case-insensitive. However, the width attribute
	/// should come before height.</para>
	/// <para>Also replaces code for PNG images specified as backgrounds via:
	/// background-image: url('image.png'); When using PNG images in the background,
	/// there is no need to use a spacer.png image. (Only supports inline CSS at
	/// this point.)</para>
	/// <seealso href="http://www.koivi.com/ie-png-transparency/">Source Article</seealso>
	/// </remarks>
	/// <example>
	/// This is an example of what the original tag will look like.
	/// <code escaped="true"><img src="test.png" width="247" height="216" alt="" /></code>
	/// This is what the filtered tag will look like when Internet Explorer is used.
	/// <code escaped="true"><img src="spacer.png" width="247" height="216" style="filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src='test.png', sizingMethod='scale');" alt="" /></code>
	/// </example>
	public class PngAlphaFilter : Stream
	{
		private Stream _responseStream;
		private long _position;
		private StringBuilder _responseHtml;
		private bool _isMsie;

		public PngAlphaFilter(Stream stream)
		{
			// checks to see if browser is MSIE
			this._isMsie = (Common.Context.Request.Browser.Browser.ToUpper() == "IE");
			
			// and if browser is version 5.0 or greater
			this._isMsie = this._isMsie && (Common.Context.Request.Browser.MajorVersion >= 5);
			
			this._responseStream = stream;
			this._responseHtml = new StringBuilder();
		}

		#region Filter

		public override bool CanRead { get { return true; } }

		public override bool CanSeek { get { return true; } }

		public override bool CanWrite { get { return true; } }

		public override void Close()
		{
			this._responseStream.Close ();
		}

		public override void Flush()
		{
			this._responseStream.Flush ();
		}

		public override long Length { get { return 0L; } }

		public override long Position 
		{ 
			get { return this._position; }
			set { this._position = value; }
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._responseStream.Seek (offset, origin);
		}

		public override void SetLength(long length)
		{
			this._responseStream.SetLength (length);
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return this._responseStream.Read (buffer, offset, count);
		}

		#endregion

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._isMsie) 
			{
				string strBuffer = System.Text.UTF8Encoding.UTF8.GetString (buffer, offset, count);

				// Wait for the closing </html> tag
				Regex eof = new Regex ("</html>", RegexOptions.IgnoreCase);

				if (!eof.IsMatch (strBuffer))
				{
					_responseHtml.Append (strBuffer);
				}
				else
				{
					_responseHtml.Append (strBuffer);

					string finalHtml = _responseHtml.ToString ();
					Regex re = null;

					// find all the png images in backgrounds 
					re = new Regex(@"background-image\s*:\s*url\s*\(\s*\'(?<src>.*\.png)\'\s*\)\s*;", RegexOptions.IgnoreCase);
					finalHtml = re.Replace(finalHtml, new MatchEvaluator(BackgroundMatch));

					// find all img or input tags that have a src with a png image in it
					re = new Regex(@"<(input|img)[^>]*\.png[^>]*>", RegexOptions.IgnoreCase);
					finalHtml = re.Replace(finalHtml, new MatchEvaluator(ImageMatch));

					// Write the formatted HTML back
					byte[] data = System.Text.UTF8Encoding.UTF8.GetBytes (finalHtml);

					_responseStream.Write (data, 0, data.Length);            
				}
			}
			// if it is not MSIE5+
			else 
			{
				// write directly to the stream with out modifying the data
				this._responseStream.Write(buffer, offset, count);
			}
		}

		private static string BackgroundMatch (Match m)
		{
			string src = m.Groups["src"].Value;
			string format = "filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(enabled=true, sizingMethod=scale src=\'{0}\');";
			
			// return new formatted string
			return String.Format(format, src);
		}

		private const string StyleFormat = @"style=""width:{0}; height:{1}; filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(enabled=true, sizingMethod=scale src='{2}');""";
		private const string SrcSpacer = @"/Images/spacer.png";

		private static string ImageMatch (Match m)
		{
			// the original image
			string image = m.Value;
			
			// image variables
			Unit width = Unit.Empty;
			Unit height = Unit.Empty;
			string src = String.Empty;

			Regex re = null;

			// find width in image
			re = new Regex(@"width=['""](?<width>[0-9]*%?)['""]", RegexOptions.IgnoreCase);
			
			// check to see if width is found
			if (re.IsMatch(image))
			{
				Match widthMatch = re.Match(image);
				width = Unit.Parse(widthMatch.Groups["width"].Value);
				image = re.Replace(image, String.Empty, 1);
			}

			// find height in image
			re = new Regex(@"height=['""](?<height>[0-9]*%?)['""]", RegexOptions.IgnoreCase);
			
			// check to see if height is found
			if (re.IsMatch(image))
			{
				Match heightMatch = re.Match(image);
				height = Unit.Parse(heightMatch.Groups["height"].Value);
				image = re.Replace(image, String.Empty, 1);
			}

			// find src in image
			re = new Regex(@"src=['""](?<src>.*\.png)['""]", RegexOptions.IgnoreCase);
			
			// check to see if src is found
			if (re.IsMatch(image))
			{
				Match srcMatch = re.Match(image);
				src = srcMatch.Groups["src"].Value;

				string newSrc = String.Concat(String.Format("src=\"{0}\"", Common.Path.GetAbsoluteUrl(SrcSpacer)), " ", String.Format(StyleFormat, width, height, src));
				image = re.Replace(image, newSrc, 1);
			}

			return image;
		}
	}
}