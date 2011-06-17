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
	public partial class PortletRole : ITable<int>
	{
		#region Static Methods
		
		#region Common Methods

		protected static PortletRoleCollection FillCollection (SqlCommand command)
		{
			PortletRoleCollection list = new PortletRoleCollection();
			
			try
			{
				command.Connection.Open();
				using(SqlDataReader reader = command.ExecuteReader())
				{
					if (reader.HasRows)
					{
						int[] order = new int[3];
						order[0] = reader.GetOrdinal("PortletID");
						order[1] = reader.GetOrdinal("Role");
						order[2] = reader.GetOrdinal("Permissions");

						while (reader.Read()) 
						{
							PortletRole entity = new PortletRole();
							entity._portletID = reader.IsDBNull(0) ? 0 :  reader.GetInt32(order[0]); // PortletID
							entity._role = reader.IsDBNull(1) ? String.Empty :  reader.GetString(order[1]); // Role
							entity._permissions = reader.IsDBNull(2) ? String.Empty :  reader.GetString(order[2]); // Permissions

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
	
		protected static PortletRole FillEntity (SqlCommand command)
		{
			PortletRole entity = null;
			
			try
			{
				command.Connection.Open();
				using(SqlDataReader reader = command.ExecuteReader())
				{
					if (reader.HasRows)
					{
						reader.Read();
						entity = new PortletRole();
						entity._portletID = reader.IsDBNull(0) ? 0 :  reader.GetInt32(reader.GetOrdinal("PortletID"));
						entity._role = reader.IsDBNull(1) ? String.Empty :  reader.GetString(reader.GetOrdinal("Role"));
						entity._permissions = reader.IsDBNull(2) ? String.Empty :  reader.GetString(reader.GetOrdinal("Permissions"));
					}
				}
			} catch (Exception exc) {
				Debug.WriteLine(exc);
			} finally {
				if (entity == null) 
					entity = new PortletRole();
					
				command.Connection.Close();
			}
					
			return entity;
		}
		
		#endregion
		
		#region Get List
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static PortletRoleCollection GetList (string where, string orderBy)
		{
			StringBuilder sb = new StringBuilder(10);
			
			sb.Append(@"select * from [PortletRole] ");
			
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
		public static PortletRoleCollection GetList (string where)
		{
			return GetList(where, String.Empty);
		}
		
		[DataObjectMethod(DataObjectMethodType.Select, true)]
		public static PortletRoleCollection GetList ()
		{
			return GetList(String.Empty, String.Empty);
		}
		
		#endregion
		
		#region Get First
	
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static PortletRole GetFirst (string where, string orderBy)
		{
			StringBuilder sb = new StringBuilder(10);
			
			sb.Append(@"select top 1 * from [PortletRole] ");
			
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
		public static PortletRole GetFirst (string where)
		{
			return GetFirst(where, String.Empty);
		}
	
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static PortletRole GetFirst ()
		{
			return GetFirst(String.Empty, String.Empty);
		}
		
		#endregion
		
		#region Get Latest
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static PortletRole GetLatest (string where)
		{
			return GetFirst(where, "ModifiedDT desc");
		}
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static PortletRole GetLatest ()
		{
			return GetLatest(String.Empty);
		}
		
		#endregion
		
		#region Get By Foreign Key
		
		#endregion
		
		#region Get By Index
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static PortletRole GetByPortletIDAndRole(int portletID, string role, string orderBy)
		{
			return GetFirst("PortletID = " + portletID + " and Role = '" + role + "'", orderBy);
		}
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static PortletRole GetByPortletIDAndRole(int portletID, string role)
		{
			return GetByPortletIDAndRole(portletID, role, String.Empty);
		}
		
		#endregion

		#region Insert
		
		protected static bool InsertOrUpdate (int portletID, string role, string permissions)		
		{
			using(SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ManagedFusion"].ConnectionString))
			{
				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = "ManagedFusion_PortletRole";
					command.CommandType = CommandType.StoredProcedure;
					
					command.Parameters.AddWithValue("@PortletID", portletID);
					command.Parameters.AddWithValue("@Role", role);
					command.Parameters.AddWithValue("@Permissions", permissions);
					
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
		public static bool Insert (int portletID, string role, string permissions)
		{
			return InsertOrUpdate(
				portletID,
				role,
				permissions
			);
		}
		
		[DataObjectMethod(DataObjectMethodType.Insert, true)]
		public static bool Insert (PortletRole entity)
		{
			entity.AcceptChanges();
			return InsertOrUpdate(
				entity.PortletID, 
				entity.Role, 
				entity.Permissions
			);
		}
		
		#endregion
		
		#region Update
		
		[DataObjectMethod(DataObjectMethodType.Update, false)]
		public static bool Update (int portletID, string role, string permissions)
		{
			return InsertOrUpdate(
				portletID,
				role,
				permissions
			);
		}
		
		[DataObjectMethod(DataObjectMethodType.Update, true)]
		public static bool Update (PortletRole entity)
		{
			entity.AcceptChanges();
			return InsertOrUpdate(
				entity.PortletID, 
				entity.Role, 
				entity.Permissions
				);
		}
		
		#endregion
		
		#region Delete
		
		[DataObjectMethod(DataObjectMethodType.Delete, true)]
		public static bool Delete (PortletRole entity)
		{
			entity.AcceptChanges();
			return Delete(
				entity.PortletID, 
				entity.Role
			);
		}

		[DataObjectMethod(DataObjectMethodType.Delete, true)]
		public static bool Delete (int portletID, string role)
		{
			using(SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ManagedFusion"].ConnectionString))
			{
				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = "ManagedFusion_PortletRole_Delete";
					command.CommandType = CommandType.StoredProcedure;
					
					command.Parameters.AddWithValue("@PortletID", portletID);
					command.Parameters.AddWithValue("@Role", role);
					
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
		/// Column PortletID : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "PortletRole"</remarks>
		private int _portletID = 0;

		/// <summary>			
		/// Column Role : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "PortletRole"</remarks>
		private string _role = String.Empty;

		#endregion
		
		#region Non Primary key(s)
		
		/// <summary>
		/// Column Permissions : 
		/// </summary>
		private string _permissions = String.Empty;

		#endregion
		
		#endregion
		
		#region Constructor
		
		///<summary>
		/// Creates a new <see cref="PortletRole"/> instance.
		///</summary>
		///<param name="PortletID"></param>
		///<param name="Role"></param>
		///<param name="Permissions"></param>
		public PortletRole (int portletID, string role, string permissions)
		{
			this._isMarkedForDeletion = false;
			this._isDirty = true;
			this._isNew = true;
			this._autoUpdate = true;
				
			this._portletID = portletID;
			this._role = role;
			this._permissions = permissions;
		}
		
		public PortletRole ()
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
		
		/// <summary>Gets the PortletID value for the column.</summary>
		/// <remarks></remarks>
		/// <value>This type is int</value>
		[ReadOnly(true)]
		[Description("")]
		[DataObjectField(true, false, false, 4)]
		public int PortletID
		{
			get { return this._portletID; }
		}
		
		/// <summary>Gets the Role value for the column.</summary>
		/// <remarks></remarks>
		/// <value>This type is nvarchar</value>
		[ReadOnly(true)]
		[Description("")]
		[DataObjectField(true, false, false, 32)]
		public string Role
		{
			get { return this._role; }
		}
		
		/// <summary>Gets or sets the Permissions value for the column.</summary>
		/// <remarks></remarks>
		/// <value>This type is ntext</value>
		[Description("")]
		[DataObjectField(false, false, false, 16)]
		public string Permissions
		{
			get { return this._permissions; }
			set
			{
				if (_permissions == value)
					return;
					
				_permissions = value;
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
		
		internal void Merge (PortletRole entity)
		{
			this._portletID = entity._portletID;
			this._role = entity._role;
			this._permissions = entity._permissions;
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
			get { return _portletID; }
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
	
	#region PortletRole Columns
	
	public enum PortletRoleColumn
	{
		/// <summary></summary>
		PortletID,
 
		/// <summary></summary>
		Role,
 
		/// <summary></summary>
		Permissions 
	}
	
	#endregion
	
	#region PortletRole Collection
	
	public class PortletRoleCollection : TableCollection<int, PortletRole>
	{
	}
	
	#endregion
}