using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Syndication
{
	public enum ChangeFrequency
	{
		/// <summary>
		/// Always changes.
		/// </summary>
		Always,

		/// <summary>
		/// Changes hourly.
		/// </summary>
		Hourly,

		/// <summary>
		/// Changes daily.
		/// </summary>
		Daily,

		/// <summary>
		/// Changes weekly.
		/// </summary>
		Weekly,

		/// <summary>
		/// Changes monthly.
		/// </summary>
		Monthly,

		/// <summary>
		/// Changes annually (yearly).
		/// </summary>
		Yearly,

		/// <summary>
		/// Never changes.
		/// </summary>
		Never,

		/// <summary>
		/// Change frequency is not defined.
		/// </summary>
		NotDefined
	}
}
