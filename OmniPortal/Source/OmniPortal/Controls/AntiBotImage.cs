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
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace OmniPortal.Controls
{
//	[ToolboxData("<{0}:AntiBotImage runat=server></{0}:AntiBotImage>")]
//	public class AntiBotImage : System.Web.UI.WebControls.Image
//	{
//		private static string _key;			    
//
//		internal string Key { get { return (string)Context.Items["AntiBotKey"]; } }
//
//		//*********************************************************************
//		//
//		// OnInit Method
//		//
//		/// <summary>
//		/// Overriden from <see cref="Control.OnInit">Control.OnInit</see>.
//		/// </summary>
//		/// <param name="e">Event arguments passed to control.</param>
//		protected override void OnInit (EventArgs e) 
//		{
//			AlternateText = "Random Number Generated for Registration";
//
//			if (!Page.IsPostBack) 
//			{
//				// creates a random creator object
//				Random r = new Random(unchecked((int)DateTime.Now.Ticks));
//				
//				// gets a random number in base-16
//				_key = Convert.ToString(r.Next(0x8000000), 16);
//			}
//
//			Context.Items.Add("AntiBotKey", _key);
//			Context.Items.Add("AntiBotForeColor", this.ForeColor);
//			Context.Items.Add("AntiBotBackColor", this.BackColor);
//
//			ImageUrl = Common.ConstructImageUrl(typeof(AntiBotImageGenerator), "image/gif", ImageFormat.Gif);
//		}
//	}
//
//	[ToolboxData("<{0}:AntiBotImageValidator runat=server></{0}:AntiBotImageValidator>")]
//	public class AntiBotImageValidator : BaseCompareValidator  
//	{
//		private string _controlToCompare;
//
//		[Bindable(false)]
//		[Category("Behavior")]
//		public string ControlToCompare 
//		{
//			get { return _controlToCompare; }
//			set { _controlToCompare = value; }
//		}
//
//		protected override bool ControlPropertiesValid() 
//		{
//			Control c = FindControl(ControlToCompare);
//			Control v = FindControl(ControlToValidate);
//
//			// check to see if both controls have been initialized
//			if ((c != null) && (v != null)) 
//			{
//				// check to see if controls are of right type
//				return (c is AntiBotImage && v is TextBox);
//			} 
//
//			// controls have not been initialized
//			return false;
//		}
//
//		protected override bool EvaluateIsValid () 
//		{
//			AntiBotImage c = (AntiBotImage)FindControl(ControlToCompare);
//			TextBox v = (TextBox)FindControl(ControlToValidate);
//
//			// checks to see if the code is equal to the entered text
//			return (c.Key == v.Text);
//		}
//	}

	/// <summary>
	/// Summary description for RegisterAntiBot.
	/// </summary>
	public class AntiBotImageGenerator : IDynamicImage
	{
		#region IDynamicImage Members

		public System.Drawing.Image CreateImage(HttpContext context)
		{
			// settings
			int fontWidth = 16;
			int width = (8 * fontWidth) + 20;
			int height = 26;

			// create graphic objects
			Bitmap codeBitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
			Graphics g = Graphics.FromImage(codeBitmap);

			// set smooting to anti alias and the background color to clear
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.Clear((Color)context.Items["AntiBotBackColor"]);

			// writes the code to the display
			g.DrawString(
				(string)context.Items["AntiBotKey"],
				new Font(FontFamily.GenericSansSerif, fontWidth, FontStyle.Bold),
				new SolidBrush((Color)context.Items["AntiBotForeColor"]),
				new Point(10, 2));

			return codeBitmap;
		}

		#endregion
	}
}