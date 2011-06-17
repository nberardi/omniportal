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
using System.Drawing.Imaging;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

// ManagedFusion Classes
using ManagedFusion;

namespace OmniPortal.Controls
{
	/// <summary>
	/// Summary description for WebCustomControl1.
	/// </summary>
	[ToolboxData("<{0}:DynamicImage runat=server></{0}:DynamicImage>")]
	public class DynamicImage : System.Web.UI.WebControls.Image
	{
		public string ImageGeneratorType
		{
			get { return (string)ViewState["ImageGenerator"]; }
			set 
			{
				// check to see if the set type inherits from IDynamicImage
				try 
				{
					Type type = Type.GetType(value, false, true);
					Type typeInterface = type.GetInterface("IDynamicImage");

					if (typeInterface == null)
						throw new InvalidCastException("ImageGeneratorType must have a type that inherits from IDynamicImage.");
				} 
				catch (NullReferenceException exc) 
				{
					throw new TypeInitializationException(value, exc);
				}

				ViewState["ImageGenerator"] = value; 
			}
		}

		public ImageFormat ImageFormat 
		{
			get { return (ImageFormat)ViewState["ImageFormat"]; }
			set { ViewState["ImageFormat"] = value; }
		}

		public new string ImageUrl 
		{
			get { return ConstructImageUrl(this.ImageGeneratorType, this.ImageFormat.ToString(), this.ImageFormat); }
		}

		#region Image Serving

		/*****************************************************************************
		 * Image Serving
		 */
		/// <summary>
		/// Renders the image in the context.
		/// </summary>
		/// <param name="image">The image to render.</param>
		/// <param name="contentType">The content to display.</param>
		/// <param name="format">The type of image.</param>
		protected void RenderImage (System.Drawing.Image image, string contentType, ImageFormat format) 
		{
			Context.Response.ContentType = contentType;
			image.Save(Context.Response.OutputStream, format);
		}

		/// <summary>
		/// Outputs a string to the Dynamic URL.
		/// </summary>
		/// <remarks>This method returns a url to use in the <c>src</c> attribute in the <c>img</c> tag for the image.</remarks>
		/// <param name="type">The type of the class that uses the <see cref="OmniPortal.IServerImage">IServerImage</see>.</param>
		/// <param name="contentType">The content of the image if any.</param>
		/// <param name="format">The image type.</param>
		/// <returns>Returns a valid URL string to the image on the server.</returns>
		protected string ConstructImageUrl (string type, string contentType, ImageFormat format)
		{
			// returns an absolute path for the image source - <img src={0}>
			return Common.Path.GetPortalUrl(String.Format("~/Image.ashx?assembly={0}&content={1}&format={2}", 
				type, 
				contentType, 
				Enum.GetName(typeof(ImageFormat), format)
				)).ToString(); 
		}
		/*
		 *****************************************************************************/
		
		#endregion
	}
}