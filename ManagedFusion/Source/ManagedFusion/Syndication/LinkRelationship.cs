using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Syndication
{
	public enum LinkRelationship
	{
		/// <summary>
		/// Identifies an alternate version of the resource described by the containing element.
		/// </summary>
		Alternate,

		/// <summary>
		/// Identifies a resource related to the resource described by the containing element.
		/// </summary>
		Related,

		/// <summary>
		/// Identifies a resource equivalent to the containing element.
		/// </summary>
		Self,

		/// <summary>
		/// Identifies a related resource that is potentially large in size and might require special handling.
		/// </summary>
		Enclosure,

		/// <summary>
		/// Identifies a resource that is the source of the information provided in the containing element.
		/// </summary>
		Via,

		/// <summary>
		/// The link relationship isn't defined.
		/// </summary>
		NotDefined
	}
}
