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
	public partial class SectionProperty : ITable<int>
	{
		#region Static Methods
		
		#region Common Methods

		protected static SectionPropertyCollection FillCollection (SqlCommand command)
		{
			SectionPropertyCollection list = new SectionPropertyCollection();
			
			try
			{
				command.Connection.Open();
				using(SqlDataReader reader = command.ExecuteReader())
				{
					if (reader.HasRows)
					{
						int[] order = new int[4];
						order[0] = reader.GetOrdinal("SectionID");
						order[1] = reader.GetOrdinal("PropertyGroup");
						order[2] = reader.GetOrdinal("Name");
						order[3] = reader.GetOrdinal("Value");

						while (reader.Read()) 
						{
							SectionProperty entity = new SectionProperty();
							entity._sectionID = reader.IsDBNull(0) ? 0 :  reader.GetInt32(order[0]); // SectionID
							entity._propertyGroup = reader.IsDBNull(1) ? (string)null :  reader.GetString(order[1]); // PropertyGroup
							entity._name = reader.IsDBNull(2) ? String.Empty :  reader.GetString(order[2]); // Name
							entity._value = reader.IsDBNull(3) ? String.Empty :  reader.GetString(order[3]); // Value

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
	
		protected static SectionProperty FillEntity (SqlCommand command)
		{
			SectionProperty entity = null;
			
			try
			{
				command.Connection.Open();
				using(SqlDataReader reader = command.ExecuteReader())
				{
					if (reader.HasRows)
					{
						reader.Read();
						entity = new SectionProperty();
						entity._sectionID = reader.IsDBNull(0) ? 0 :  reader.GetInt32(reader.GetOrdinal("SectionID"));
						entity._propertyGroup = reader.IsDBNull(1) ? (string)null :  reader.GetString(reader.GetOrdinal("PropertyGroup"));
						entity._name = reader.IsDBNull(2) ? String.Empty :  reader.GetString(reader.GetOrdinal("Name"));
						entity._value = reader.IsDBNull(3) ? String.Empty :  reader.GetString(reader.GetOrdinal("Value"));
					}
				}
			} catch (Exception exc) {
				Debug.WriteLine(exc);
			} finally {
				if (entity == null) 
					entity = new SectionProperty();
					
				command.Connection.Close();
			}
					
			return entity;
		}
		
		#endregion
		
		#region Get List
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static SectionPropertyCollection GetList (string where, string orderBy)
		{
			StringBuilder sb = new StringBuilder(10);
			
			sb.Append(@"select * from [SectionProperty] ");
			
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
		public static SectionPropertyCollection GetList (string where)
		{
			return GetList(where, String.Empty);
		}
		
		[DataObjectMethod(DataObjectMethodType.Select, true)]
		public static SectionPropertyCollection GetList ()
		{
			return GetList(String.Empty, String.Empty);
		}
		
		#endregion
		
		#region Get First
	
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static SectionProperty GetFirst (string where, string orderBy)
		{
			StringBuilder sb = new StringBuilder(10);
			
			sb.Append(@"select top 1 * from [SectionProperty] ");
			
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
		public static SectionProperty GetFirst (string where)
		{
			return GetFirst(where, String.Empty);
		}
	
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static SectionProperty GetFirst ()
		{
			return GetFirst(String.Empty, String.Empty);
		}
		
		#endregion
		
		#region Get Latest
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static SectionProperty GetLatest (string where)
		{
			return GetFirst(where, "ModifiedDT desc");
		}
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static SectionProperty GetLatest ()
		{
			return GetLatest(String.Empty);
		}
		
		#endregion
		
		#region Get By Foreign Key
		
		#endregion
		
		#region Get By Index
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static SectionProperty GetBySectionIDAndPropertyGroupAndName(int sectionID, string propertyGroup, string name, string orderBy)
		{
			return GetFirst("SectionID = " + sectionID + " and PropertyGroup = '" + propertyGroup + "' and Name = '" + name + "'", orderBy);
		}
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static SectionProperty GetBySectionIDAndPropertyGroupAndName(int sectionID, string propertyGroup, string name)
		{
			return GetBySectionIDAndPropertyGroupAndName(sectionID, propertyGroup, name, String.Empty);
		}
		
		#endregion

		#region Insert
		
		protected static bool InsertOrUpdate (int sectionID, string propertyGroup, string name, string value)		
		{
			using(SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ManagedFusion"].ConnectionString))
			{
				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = "ManagedFusion_SectionProperty";
					command.CommandType = CommandType.StoredProcedure;
					
					command.Parameters.AddWithValue("@SectionID", sectionID);
					command.Parameters.AddWithValue("@PropertyGroup", propertyGroup);
					command.Parameters.AddWithValue("@Name", name);
					command.Parameters.AddWithValue("@Value", value);
					
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
		public static bool Insert (int sectionID, string propertyGroup, string name, string value)
		{
			return InsertOrUpdate(
				sectionID,
				propertyGroup,
				name,
				value
			);
		}
		
		[DataObjectMethod(DataObjectMethodType.Insert, true)]
		public static bool Insert (SectionProperty entity)
		{
			entity.AcceptChanges();
			return InsertOrUpdate(
				entity.SectionID, 
				entity.PropertyGroup, 
				entity.Name, 
				entity.Value
			);
		}
		
		#endregion
		
		#region Update
		
		[DataObjectMethod(DataObjectMethodType.Update, false)]
		public static bool Update (int sectionID, string propertyGroup, string name, string value)
		{
			return InsertOrUpdate(
				sectionID,
				propertyGroup,
				name,
				value
			);
		}
		
		[DataObjectMethod(DataObjectMethodType.Update, true)]
		public static bool Update (SectionProperty entity)
		{
			entity.AcceptChanges();
			return InsertOrUpdate(
				entity.SectionID, 
				entity.PropertyGroup, 
				entity.Name, 
				entity.Value
				);
		}
		
		#endregion
		
		#region Delete
		
		[DataObjectMethod(DataObjectMethodType.Delete, true)]
		public static bool Delete (SectionProperty entity)
		{
			entity.AcceptChanges();
			return Delete(
				entity.SectionID, 
				entity.PropertyGroup, 
				entity.Name
			);
		}

		[DataObjectMethod(DataObjectMethodType.Delete, true)]
		public static bool Delete (int sectionID, string propertyGroup, string name)
		{
			using(SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ManagedFusion"].ConnectionString))
			{
				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = "ManagedFusion_SectionProperty_Delete";
					command.CommandType = CommandType.StoredProcedure;
					
					command.Parameters.AddWithValue("@SectionID", sectionID);
					command.Parameters.AddWithValue("@PropertyGroup", propertyGroup);
					command.Parameters.AddWithValue("@Name", name);
					
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
		/// Column SectionID : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "SectionProperty"</remarks>
		private int _sectionID = 0;

		/// <summary>			
		/// Column PropertyGroup : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "SectionProperty"</remarks>
		private string _propertyGroup = (string)null;

		/// <summary>			
		/// Column Name : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "SectionProperty"</remarks>
		private string _name = String.Empty;

		#endregion
		
		#region Non Primary key(s)
		
		/// <summary>
		/// Column Value : 
		/// </summary>
		private string _value = String.Empty;

		#endregion
		
		#endregion
		
		#region Constructor
		
		///<summary>
		/// Creates a new <see cref="SectionProperty"/> instance.
		///</summary>
		///<param name="SectionID"></param>
		///<param name="PropertyGroup"></param>
		///<param name="Name"></param>
		///<param name="Value"></param>
		public SectionProperty (int sectionID, string propertyGroup, string name, string value)
		{
			this._isMarkedForDeletion = false;
			this._isDirty = true;
			this._isNew = true;
			this._autoUpdate = true;
				
			this._sectionID = sectionID;
			this._propertyGroup = propertyGroup;
			this._name = name;
			this._value = value;
		}
		
		public SectionProperty ()
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
		
		/// <summary>Gets the SectionID value for the column.</summary>
		/// <remarks></remarks>
		/// <value>This type is int</value>
		[ReadOnly(true)]
		[Description("")]
		[DataObjectField(true, false, false, 4)]
		public int SectionID
		{
			get { return this._sectionID; }
		}
		
		/// <summary>Gets the PropertyGroup value for the column.</summary>
		/// <remarks></remarks>
		/// <value>This type is nvarchar</value>
		[ReadOnly(true)]
		[Description("")]
		[DataObjectField(true, false, false, 32)]
		public string PropertyGroup
		{
			get { return this._propertyGroup; }
		}
		
		/// <summary>Gets the Name value for the column.</summary>
		/// <remarks></remarks>
		/// <value>This type is nvarchar</value>
		[ReadOnly(true)]
		[Description("")]
		[DataObjectField(true, false, false, 32)]
		public string Name
		{
			get { return this._name; }
		}
		
		/// <summary>Gets or sets the Value value for the column.</summary>
		/// <remarks></remarks>
		/// <value>This type is ntext</value>
		[Description("")]
		[DataObjectField(false, false, false, 16)]
		public string Value
		{
			get { return this._value; }
			set
			{
				if (_value == value)
					return;
					
				_value = value;
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
		
		internal void Merge (SectionProperty entity)
		{
			this._sectionID = entity._sectionID;
			this._propertyGroup = entity._propertyGroup;
			this._name = entity._name;
			this._value = entity._value;
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
			get { return _sectionID; }
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
	
	#region SectionProperty Columns
	
	public enum SectionPropertyColumn
	{
		/// <summary></summary>
		SectionID,
 
		/// <summary></summary>
		PropertyGroup,
 
		/// <summary></summary>
		Name,
 
		/// <summary></summary>
		Value 
	}
	
	#endregion
	
	#region SectionProperty Collection
	
	public class SectionPropertyCollection : TableCollection<int, SectionProperty>
	{
	}
	
	#endregion
}