/*
 * 	Template:		This code was generated by the ManagedFusion [http://www.managedfusion.com] Data Layer Template.
 * 	Created On :	11/22/2006
 * 	Remarks:		Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Security.Permissions;
using System.Xml;
using System.Xml.Serialization;

namespace ManagedFusion.Data.SqlServer2000
{
	[DataObject(true)]
	public partial class Site : ITable<int>
	{
		#region Static Methods
		
		#region Common Methods

		protected static SiteCollection FillCollection (SqlCommand command)
		{
			SiteCollection list = new SiteCollection();
			
			try
			{
				command.Connection.Open();
				using(SqlDataReader reader = command.ExecuteReader())
				{
					if (reader.HasRows)
					{
						int[] order = new int[9];
						order[0] = reader.GetOrdinal("SiteID");
						order[1] = reader.GetOrdinal("SectionID");
						order[2] = reader.GetOrdinal("Name");
						order[3] = reader.GetOrdinal("Description");
						order[4] = reader.GetOrdinal("Touched");
						order[5] = reader.GetOrdinal("SubDomain");
						order[6] = reader.GetOrdinal("Domain");
						order[7] = reader.GetOrdinal("Theme");
						order[8] = reader.GetOrdinal("Style");

						while (reader.Read()) 
						{
							Site entity = new Site();
							entity._siteID = reader.IsDBNull(0) ? 0 :  reader.GetInt32(order[0]); // SiteID
							entity._sectionID = reader.IsDBNull(1) ? (int?)null :  reader.GetInt32(order[1]); // SectionID
							entity._name = reader.IsDBNull(2) ? String.Empty :  reader.GetString(order[2]); // Name
							entity._description = reader.IsDBNull(3) ? (string)null :  reader.GetString(order[3]); // Description
							entity._touched = reader.IsDBNull(4) ? (DateTime)SqlDateTime.MinValue :  reader.GetDateTime(order[4]); // Touched
							entity._subDomain = reader.IsDBNull(5) ? String.Empty :  reader.GetString(order[5]); // SubDomain
							entity._domain = reader.IsDBNull(6) ? String.Empty :  reader.GetString(order[6]); // Domain
							entity._theme = reader.IsDBNull(7) ? String.Empty :  reader.GetString(order[7]); // Theme
							entity._style = reader.IsDBNull(8) ? String.Empty :  reader.GetString(order[8]); // Style

							// add to list
							list.Add(entity);
						}
					}
				}
			} catch (Exception exc) {
				Debug.WriteLine(exc);
			} finally {
				command.Connection.Close();
			}
					
			return list;
		}
	
		protected static Site FillEntity (SqlCommand command)
		{
			Site entity = null;
			
			try
			{
				command.Connection.Open();
				using(SqlDataReader reader = command.ExecuteReader())
				{
					if (reader.HasRows)
					{
						reader.Read();
						entity = new Site();
						entity._siteID = reader.IsDBNull(0) ? 0 :  reader.GetInt32(reader.GetOrdinal("SiteID"));
						entity._sectionID = reader.IsDBNull(1) ? (int?)null :  reader.GetInt32(reader.GetOrdinal("SectionID"));
						entity._name = reader.IsDBNull(2) ? String.Empty :  reader.GetString(reader.GetOrdinal("Name"));
						entity._description = reader.IsDBNull(3) ? (string)null :  reader.GetString(reader.GetOrdinal("Description"));
						entity._touched = reader.IsDBNull(4) ? (DateTime)SqlDateTime.MinValue :  reader.GetDateTime(reader.GetOrdinal("Touched"));
						entity._subDomain = reader.IsDBNull(5) ? String.Empty :  reader.GetString(reader.GetOrdinal("SubDomain"));
						entity._domain = reader.IsDBNull(6) ? String.Empty :  reader.GetString(reader.GetOrdinal("Domain"));
						entity._theme = reader.IsDBNull(7) ? String.Empty :  reader.GetString(reader.GetOrdinal("Theme"));
						entity._style = reader.IsDBNull(8) ? String.Empty :  reader.GetString(reader.GetOrdinal("Style"));
					}
				}
			} catch (Exception exc) {
				Debug.WriteLine(exc);
			} finally {
				if (entity == null) 
					entity = new Site();
					
				command.Connection.Close();
			}
					
			return entity;
		}
		
		#endregion
		
		#region Get List
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static SiteCollection GetList (string where, string orderBy)
		{
			StringBuilder sb = new StringBuilder(10);
			
			sb.Append(@"select * from [Site] ");
			
			if (String.IsNullOrEmpty(where) == false)
			{
				sb.Append(" where ");
				sb.Append("(");
				sb.Append(where);
				sb.Append(")");
			}
			
			if (String.IsNullOrEmpty(orderBy) == false)
			{
				sb.Append(" order by ");
				sb.Append(orderBy);
			}
			
			using(SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ManagedFusion"].ConnectionString))
			{
				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = sb.ToString();
					command.CommandType = CommandType.Text;
					
					return FillCollection(command);
				}
			}
		}
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static SiteCollection GetList (string where)
		{
			return GetList(where, String.Empty);
		}
		
		[DataObjectMethod(DataObjectMethodType.Select, true)]
		public static SiteCollection GetList ()
		{
			return GetList(String.Empty, String.Empty);
		}
		
		#endregion
		
		#region Get First
	
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static Site GetFirst (string where, string orderBy)
		{
			StringBuilder sb = new StringBuilder(10);
			
			sb.Append(@"select top 1 * from [Site] ");
			
			if (String.IsNullOrEmpty(where) == false)
			{
				sb.Append(" where ");
				sb.Append("(");
				sb.Append(where);
				sb.Append(")");
			}
			
			if (String.IsNullOrEmpty(orderBy) == false)
			{
				sb.Append(" order by ");
				sb.Append(orderBy);
			}
			
			using(SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ManagedFusion"].ConnectionString))
			{
				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = sb.ToString();
					command.CommandType = CommandType.Text;
					
					return FillEntity(command);
				}
			}
		}
	
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static Site GetFirst (string where)
		{
			return GetFirst(where, String.Empty);
		}
	
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static Site GetFirst ()
		{
			return GetFirst(String.Empty, String.Empty);
		}
		
		#endregion
		
		#region Get Latest
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static Site GetLatest (string where)
		{
			return GetFirst(where, "ModifiedDT desc");
		}
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static Site GetLatest ()
		{
			return GetLatest(String.Empty);
		}
		
		#endregion
		
		#region Get By Foreign Key
		
		#endregion
		
		#region Get By Index
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static Site GetBySiteID(int siteID, string orderBy)
		{
			return GetFirst("SiteID = " + siteID + "", orderBy);
		}
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static Site GetBySiteID(int siteID)
		{
			return GetBySiteID(siteID, String.Empty);
		}
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static Site GetByDomainAndSubDomain(string domain, string subDomain, string orderBy)
		{
			return GetFirst("Domain = '" + domain + "' and SubDomain = '" + subDomain + "'", orderBy);
		}
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static Site GetByDomainAndSubDomain(string domain, string subDomain)
		{
			return GetByDomainAndSubDomain(domain, subDomain, String.Empty);
		}
		
		#endregion

		#region Insert
		
		protected static bool InsertOrUpdate (int siteID, int? sectionID, string name, string description, DateTime touched, string subDomain, string domain, string theme, string style)		
		{
			using(SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ManagedFusion"].ConnectionString))
			{
				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = "ManagedFusion_Site";
					command.CommandType = CommandType.StoredProcedure;
					
					command.Parameters.AddWithValue("@SiteID", siteID);
					command.Parameters.AddWithValue("@SectionID", sectionID);
					command.Parameters.AddWithValue("@Name", name);
					command.Parameters.AddWithValue("@Description", description);
					command.Parameters.AddWithValue("@Touched", touched);
					command.Parameters.AddWithValue("@SubDomain", subDomain);
					command.Parameters.AddWithValue("@Domain", domain);
					command.Parameters.AddWithValue("@Theme", theme);
					command.Parameters.AddWithValue("@Style", style);
					
					bool success = false;
					
					try
					{
						connection.Open();
						command.ExecuteNonQuery();
					
						success = true;
					} catch (Exception exc) {
						Debug.WriteLine(exc);
						
						success = false;
					} finally {
						connection.Close();
					}
					
					return success;
				}
			}
		}
		
		[DataObjectMethod(DataObjectMethodType.Insert, false)]
		public static bool Insert (int siteID, int? sectionID, string name, string description, DateTime touched, string subDomain, string domain, string theme, string style)
		{
			return InsertOrUpdate(
				siteID,
				sectionID,
				name,
				description,
				touched,
				subDomain,
				domain,
				theme,
				style
			);
		}
		
		[DataObjectMethod(DataObjectMethodType.Insert, true)]
		public static bool Insert (Site entity)
		{
			entity.AcceptChanges();
			return InsertOrUpdate(
				entity.SiteID, 
				entity.SectionID, 
				entity.Name, 
				entity.Description, 
				entity.Touched, 
				entity.SubDomain, 
				entity.Domain, 
				entity.Theme, 
				entity.Style
			);
		}
		
		#endregion
		
		#region Update
		
		[DataObjectMethod(DataObjectMethodType.Update, false)]
		public static bool Update (int siteID, int? sectionID, string name, string description, DateTime touched, string subDomain, string domain, string theme, string style)
		{
			return InsertOrUpdate(
				siteID,
				sectionID,
				name,
				description,
				touched,
				subDomain,
				domain,
				theme,
				style
			);
		}
		
		[DataObjectMethod(DataObjectMethodType.Update, true)]
		public static bool Update (Site entity)
		{
			entity.AcceptChanges();
			return InsertOrUpdate(
				entity.SiteID, 
				entity.SectionID, 
				entity.Name, 
				entity.Description, 
				entity.Touched, 
				entity.SubDomain, 
				entity.Domain, 
				entity.Theme, 
				entity.Style
				);
		}
		
		#endregion
		
		#region Delete
		
		[DataObjectMethod(DataObjectMethodType.Delete, true)]
		public static bool Delete (Site entity)
		{
			entity.AcceptChanges();
			return Delete(
				entity.SiteID
			);
		}

		[DataObjectMethod(DataObjectMethodType.Delete, true)]
		public static bool Delete (int siteID)
		{
			using(SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ManagedFusion"].ConnectionString))
			{
				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = "ManagedFusion_Site_Delete";
					command.CommandType = CommandType.StoredProcedure;
					
					command.Parameters.AddWithValue("@SiteID", siteID);
					
					bool success = false;
					
					try
					{
						connection.Open();
						command.ExecuteNonQuery();
					
						success = true;
					} catch (Exception exc) {
						Debug.WriteLine(exc);
						
						success = false;
					} finally {
						connection.Close();
					}
					
					return success;
				}
			}
		}
		
		#endregion

		#endregion
		
		#region Column Variables
		
		#region Primary key(s)
		
		/// <summary>			
		/// Column SiteID : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "Site"</remarks>
		private int _siteID = 0;

		#endregion
		
		#region Non Primary key(s)
		
		/// <summary>
		/// Column SectionID : 
		/// </summary>
		private int? _sectionID = (int?)null;

		/// <summary>
		/// Column Name : 
		/// </summary>
		private string _name = String.Empty;

		/// <summary>
		/// Column Description : 
		/// </summary>
		private string _description = (string)null;

		/// <summary>
		/// Column Touched : 
		/// </summary>
		private DateTime _touched = (DateTime)SqlDateTime.MinValue;

		/// <summary>
		/// Column SubDomain : 
		/// </summary>
		private string _subDomain = String.Empty;

		/// <summary>
		/// Column Domain : 
		/// </summary>
		private string _domain = String.Empty;

		/// <summary>
		/// Column Theme : 
		/// </summary>
		private string _theme = String.Empty;

		/// <summary>
		/// Column Style : 
		/// </summary>
		private string _style = String.Empty;

		#endregion
		
		#endregion
		
		#region Constructor
		
		///<summary>
		/// Creates a new <see cref="Site"/> instance.
		///</summary>
		///<param name="SiteID"></param>
		///<param name="SectionID"></param>
		///<param name="Name"></param>
		///<param name="Description"></param>
		///<param name="Touched"></param>
		///<param name="SubDomain"></param>
		///<param name="Domain"></param>
		///<param name="Theme"></param>
		///<param name="Style"></param>
		public Site (int? sectionID, string name, string description, DateTime touched, string subDomain, string domain, string theme, string style)
		{
			this._isMarkedForDeletion = false;
			this._isDirty = true;
			this._isNew = true;
			this._autoUpdate = true;
				
			this._sectionID = sectionID;
			this._name = name;
			this._description = description;
			this._touched = touched;
			this._subDomain = subDomain;
			this._domain = domain;
			this._theme = theme;
			this._style = style;
		}
		
		public Site ()
		{
			this._isMarkedForDeletion = false;
			this._isDirty = false;
			this._isNew = true;
			this._autoUpdate = true;
		}
		
		#endregion
		
		#region Properties
		
		#region Foreign Keys
		
		#endregion
		
		/// <summary>Gets the SiteID value for the column.</summary>
		/// <remarks></remarks>
		/// <value>This type is int</value>
		[ReadOnly(true)]
		[Description("")]
		[DataObjectField(true, true, false, 4)]
		public int SiteID
		{
			get { return this._siteID; }
		}
		
		/// <summary>Gets or sets the SectionID value for the column.</summary>
		/// <remarks></remarks>
		/// <value>This type is int</value>
		[Description("")]
		[DataObjectField(false, false, true, 4)]
		public int? SectionID
		{
			get { return this._sectionID; }
			set
			{
				if (_sectionID == value)
					return;
					
				_sectionID = value;
				this._isDirty = true;
				
				// if auto update is turned on update this
				if (AllowAutoUpdate) Update(this);
			}
		}
		
		/// <summary>Gets or sets the Name value for the column.</summary>
		/// <remarks></remarks>
		/// <value>This type is nvarchar</value>
		[Description("")]
		[DataObjectField(false, false, false, 32)]
		public string Name
		{
			get { return this._name; }
			set
			{
				if (_name == value)
					return;
					
				_name = value;
				this._isDirty = true;
				
				// if auto update is turned on update this
				if (AllowAutoUpdate) Update(this);
			}
		}
		
		/// <summary>Gets or sets the Description value for the column.</summary>
		/// <remarks></remarks>
		/// <value>This type is nvarchar</value>
		[Description("")]
		[DataObjectField(false, false, true, 128)]
		public string Description
		{
			get { return this._description; }
			set
			{
				if (_description == value)
					return;
					
				_description = value;
				this._isDirty = true;
				
				// if auto update is turned on update this
				if (AllowAutoUpdate) Update(this);
			}
		}
		
		/// <summary>Gets or sets the Touched value for the column.</summary>
		/// <remarks></remarks>
		/// <value>This type is datetime</value>
		[Description("")]
		[DataObjectField(false, false, false, 8)]
		public DateTime Touched
		{
			get { return this._touched; }
			set
			{
				if (_touched == value)
					return;
					
				_touched = value;
				this._isDirty = true;
				
				// if auto update is turned on update this
				if (AllowAutoUpdate) Update(this);
			}
		}
		
		/// <summary>Gets or sets the SubDomain value for the column.</summary>
		/// <remarks></remarks>
		/// <value>This type is nvarchar</value>
		[Description("")]
		[DataObjectField(false, false, false, 128)]
		public string SubDomain
		{
			get { return this._subDomain; }
			set
			{
				if (_subDomain == value)
					return;
					
				_subDomain = value;
				this._isDirty = true;
				
				// if auto update is turned on update this
				if (AllowAutoUpdate) Update(this);
			}
		}
		
		/// <summary>Gets or sets the Domain value for the column.</summary>
		/// <remarks></remarks>
		/// <value>This type is nvarchar</value>
		[Description("")]
		[DataObjectField(false, false, false, 128)]
		public string Domain
		{
			get { return this._domain; }
			set
			{
				if (_domain == value)
					return;
					
				_domain = value;
				this._isDirty = true;
				
				// if auto update is turned on update this
				if (AllowAutoUpdate) Update(this);
			}
		}
		
		/// <summary>Gets or sets the Theme value for the column.</summary>
		/// <remarks></remarks>
		/// <value>This type is nvarchar</value>
		[Description("")]
		[DataObjectField(false, false, false, 64)]
		public string Theme
		{
			get { return this._theme; }
			set
			{
				if (_theme == value)
					return;
					
				_theme = value;
				this._isDirty = true;
				
				// if auto update is turned on update this
				if (AllowAutoUpdate) Update(this);
			}
		}
		
		/// <summary>Gets or sets the Style value for the column.</summary>
		/// <remarks></remarks>
		/// <value>This type is nvarchar</value>
		[Description("")]
		[DataObjectField(false, false, false, 64)]
		public string Style
		{
			get { return this._style; }
			set
			{
				if (_style == value)
					return;
					
				_style = value;
				this._isDirty = true;
				
				// if auto update is turned on update this
				if (AllowAutoUpdate) Update(this);
			}
		}
		

		private bool _autoUpdate = true;
		/// <summary>True if the entity should commit changes as soon as they are made.</summary>
		[Browsable(false)]
		public bool AutoUpdate
		{
			get { return this._autoUpdate; }
			set { _autoUpdate = value; }
		}

		private bool _isMarkedForDeletion = false;
		/// <summary>Gets if the object has been <see cref="MarkToDelete"/>.</summary>
		[Browsable(false)]
		public bool IsMarkedForDeletion
		{
			get { return this._isMarkedForDeletion; }
		}

		private bool _isDirty = false;
		/// <summary>Indicates if the object has been modified from its original state.</summary>
		///<value>True if object has been modified from its original state; otherwise False;</value>
		[Browsable(false)]
		public bool IsDirty
		{
			get { return this._isDirty; }
		}

		private bool _isNew = false;
		/// <summary>Indicates if the object is new.</summary>
		///<value>True if objectis new; otherwise False;</value>
		[Browsable(false)]
		public bool IsNew
		{
			get { return this._isNew; }
		}

		/// <summary>Gets a value indicating if AutoUpdate is allowed on this entity.</summary>
		private bool AllowAutoUpdate 
		{
			get { return (!IsNew && !IsMarkedForDeletion) && AutoUpdate; }
		}

		#endregion
		
		#region Methods
		
		internal void Merge (Site entity)
		{
			this._siteID = entity._siteID;
			this._sectionID = entity._sectionID;
			this._name = entity._name;
			this._description = entity._description;
			this._touched = entity._touched;
			this._subDomain = entity._subDomain;
			this._domain = entity._domain;
			this._theme = entity._theme;
			this._style = entity._style;
		}

		/// <summary>Begin the update process.</summary>
		public void BeginUpdate()
		{
			this.AutoUpdate = false;
		}
		
		/// <summary>End the update process and commit changes.</summary>
		public void EndUpdate()
		{
			this.EndUpdate(true);
		}
		
		/// <summary>End the update process</summary>
		public void EndUpdate(bool commit)
		{
			this.AutoUpdate = true;
			
			if (commit)
				this.CommitChanges();
		}
		
		/// <summary>Accepts the changes made to this object by setting each flags to false.</summary>
		internal void AcceptChanges()
		{
			this._isMarkedForDeletion = false;
			this._isDirty = false;
			this._isNew = false;
		}
		
		///<summary>Currently not supported.</summary>
		public void CancelChanges()
		{
			throw new NotSupportedException("Cancel changes is not currently supported.");
		}
		
		///<summary>Delete this entity.</summary>
		public void Delete()
		{
			this._isMarkedForDeletion = true;
			
			if (!IsNew && AutoUpdate) Delete(this);
		}
		
		#endregion
		
		#region ITable<int> Members

		[DataObjectField(true, true, false)]
		int ITable<int>.PrimaryKey
		{
			get { return _siteID; }
		}

		/// <summary>Commit the changes to the database.</summary>
		public void CommitChanges()
		{
			if (this.IsNew)
				Insert(this);
			
			else if (this.IsMarkedForDeletion)
				Delete(this);
				
			else if (this.IsDirty)
				Update(this);
		}

		#endregion
	}
	
	#region Site Columns
	
	public enum SiteColumn
	{
		/// <summary></summary>
		SiteID,
 
		/// <summary></summary>
		SectionID,
 
		/// <summary></summary>
		Name,
 
		/// <summary></summary>
		Description,
 
		/// <summary></summary>
		Touched,
 
		/// <summary></summary>
		SubDomain,
 
		/// <summary></summary>
		Domain,
 
		/// <summary></summary>
		Theme,
 
		/// <summary></summary>
		Style 
	}
	
	#endregion
	
	#region Site Collection
	
	public class SiteCollection : TableCollection<int, Site>
	{
	}
	
	#endregion
}