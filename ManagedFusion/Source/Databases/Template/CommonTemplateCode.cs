using CodeSmith.Engine;
using SchemaExplorer;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Design;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

namespace ManagedFusion.Templates
{
	/// <summary>
	/// Common code-behind class used to simplify SQL Server based CodeSmith templates
	/// </summary>
	public class CommonTemplateCode : CodeTemplate
	{
		private string entityFormat 		= "{0}";
		private string collectionFormat 	= "{0}Collection";
		private string providerFormat 		= "{0}Provider";
		private string interfaceFormat	 	= "I{0}";
		private string baseClassFormat 		= "{0}Base";
		private string unitTestFormat		= "{0}Test";
		private string enumFormat 			= "{0}List";
		private string manyToManyFormat		= "{0}From{1}";
		
		private bool _generateEnums = false;
		private bool _generateTables = true;
		private string _connectionString = String.Empty;
		private string _outputDirectory = String.Empty;
		private string _sqlOutputDirectory = String.Empty;
		private string _insertUpdateSuffix = String.Empty;
		private string _deleteSuffix = String.Empty;
		private string _procedurePrefix = String.Empty;
		private string _namespace = String.Empty;
		private string _companyName = String.Empty;
		private string _companyUrl = String.Empty;

		private TableSchemaCollection _sourceTables;
		private ViewSchemaCollection _sourceViews;
		private TableSchemaCollection _enumTables;
		
		/// <summary>
		/// Return a specified number of tabs
		/// </summary>
		/// <param name="n">Number of tabs</param>
		/// <returns>n tabs</returns>
		public string Tab(int n)
		{
			return new String('\t', n);
		}
		
		#region Code style public properties

		[Category("Generate")]
		[Optional]
		[DefaultValue(true)]
		public bool GenerateTables
		{
			get { return _generateTables; }
			set { _generateTables = value; }
		}

		[Category("Generate")]
		[Optional]
		[DefaultValue(false)]
		public bool GenerateEnums
		{
			get { return _generateEnums; }
			set { _generateEnums = value; }
		}

		[Category("Configuration")]
		[Description("This is the connectionString property from the config file that the entities will use.")]
		public string ConnectionStringName
		{
			get { return _connectionString; }
			set { _connectionString = value; }
		}

		[Category("Configuration")]
		public string Namespace 
		{
			get { return _namespace; }
			set { _namespace = value; }
		}

		[Category("Documentation")]
		[Optional]
		public string CompanyName
		{
			get { return _companyName; }
			set { _companyName = value; }
		}

		[Category("Documentation")]
		[Optional]
		public string CompanyUrl
		{
			get { return _companyUrl; }
			set { _companyUrl = value; }
		}

		[Category("Stored Procedures")]
		[Optional]
		public string InsertUpdateSuffix
		{
			get { return _insertUpdateSuffix; }
			set { _insertUpdateSuffix = value; }
		}

		[Category("Stored Procedures")]
		[DefaultValue("_Delete")]
		[Optional]
		public string DeleteSuffix
		{
			get { return _deleteSuffix; }
			set { _deleteSuffix = value; }
		}

		[Category("Stored Procedures")]
		[Optional]
		public string ProcedurePrefix
		{
			get { return _procedurePrefix; }
			set { _procedurePrefix = value; }
		}

		[Editor(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(System.Drawing.Design.UITypeEditor))] 
		[Category("General")]
		[Description("The directory to output the results to.")]
		[DefaultValue("")]
		public string OutputDirectory 
		{ 
			get
			{
				if (_outputDirectory.Length == 0) return this.CodeTemplateInfo.DirectoryName + "output";
				return _outputDirectory;
			}
			set
			{
				if (value.EndsWith("\\")) value = value.Substring(0, value.Length - 1);
				_outputDirectory = value;
			} 
		}
		
		[Editor(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(System.Drawing.Design.UITypeEditor))] 
		[Category("General")]
		[Description("The directory to output the SQL results to.")]
		[DefaultValue("")]
		public string SqlOutputDirectory 
		{ 
			get
			{
				if (_sqlOutputDirectory.Length == 0) return this.CodeTemplateInfo.DirectoryName + "output";
				return _sqlOutputDirectory;
			}
			set
			{
				if (value.EndsWith("\\")) value = value.Substring(0, value.Length - 1);
				_sqlOutputDirectory = value;
			} 
		}

		[Category("DataSource")]
		[Description("The tables to generate.")]
		public TableSchemaCollection SourceTables
		{
			get
			{
				if (this._sourceTables != null && this._sourceTables.Count > 0 )
					return this._sourceTables;
				else
					return null;
			}
			set
			{
				this._sourceTables = value;
			}
		}

		[Category("DataSource")]
		[Description("The tables to generate as enums.")]
		[Optional]
		public TableSchemaCollection EnumTables
		{
			get
			{
				if (this._enumTables != null && this._enumTables.Count > 0)
					return this._enumTables;
				else
					return null;
			}
			set
			{
				this._enumTables = value;
			}
		}
		
		[Category("DataSource")]
		[Description("The view to generate.")]
		[Optional]
		public ViewSchemaCollection SourceViews
		{
			get
			{
				if (this._sourceViews != null && this._sourceViews.Count > 0)
					return this._sourceViews;
				else
					return null;
			}
			set
			{
				this._sourceViews = value;
			}
		}
//		
//		[Category("Code style")]
//		[Description("The format for entity class name. Parameter {0} is replaced by the trimed table name, in Pascal case.")]
//		public string EntityFormat
//		{
//			get {return this.entityFormat;}
//			set
//			{
//				if (value.IndexOf("{0}") == -1) 
//				{
//					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "EntityFormat");
//				}
//				this.entityFormat = value;
//			}
//		}
//		
//		[Category("Code style")]
//		[Description("The format for any collection class name. Parameter {0} is replaced by the collection item class name.")]
//		public string CollectionFormat
//		{
//			get {return this.collectionFormat;}
//			set
//			{
//				if (value.IndexOf("{0}") == -1) 
//				{
//					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "CollectionFormat");
//				}
//				this.collectionFormat = value;
//			}
//		}
//		
//		[Category("Code style")]
//		[Description("The format for any provider class name. Parameter {0} is replaced by the original class name.")]
//		public string ProviderFormat
//		{
//			get {return this.providerFormat;}
//			set
//			{
//				if (value.IndexOf("{0}") == -1) 
//				{
//					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "ProviderFormat");
//				}
//				this.providerFormat = value;
//			}
//		}
//		
//		[Category("Code style")]
//		[Description("The format for any interface name. Parameter {0} is replaced by the original class name.")]
//		public string InterfaceFormat
//		{
//			get {return this.interfaceFormat;}
//			set
//			{
//				if (value.IndexOf("{0}") == -1) 
//				{
//					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "InterfaceFormat");
//				}
//				this.interfaceFormat = value;
//			}
//		}
//		
//		[Category("Code style")]
//		[Description("The format for any base class name. Parameter {0} is replaced by the original class name.")]
//		public string BaseClassFormat
//		{
//			get {return this.baseClassFormat;}
//			set
//			{
//				if (value.IndexOf("{0}") == -1) 
//				{
//					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "BaseClassFormat");
//				}
//				this.baseClassFormat = value;
//			}
//		}
//		
//		[Category("Code style")]
//		[Description("The format for any enum. Parameter {0} is replaced by the original class name.")]
//		public string EnumFormat
//		{
//			get {return this.enumFormat;}
//			set
//			{
//				if (value.IndexOf("{0}") == -1) 
//				{
//					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "EnumFormat");
//				}
//				this.enumFormat = value;
//			}
//		}
//		
//		[Category("Code style")]
//		[Description("The format for many to many methods. Parameter {0} is replaced by the secondary class name.")]
//		public string ManyToManyFormat
//		{
//			get {return this.manyToManyFormat;}
//			set
//			{
//				if (value.IndexOf("{0}") == -1) 
//				{
//					throw new ArgumentException("This parameter must contains the pattern {0} to be valid.", "ManyToManyFormat");
//				}
//				this.manyToManyFormat = value;
//			}
//		}
		
		#endregion

		/// <summary>
		/// Get the safe name for a data object by determining if it contains spaces or other illegal
		/// characters - if so wrap with []
		/// </summary>
		/// <param name="schemaObject">Database schema object (e.g. a table, stored proc, etc)</param>
		/// <returns>The safe name of the object</returns>
		public string GetSafeName(SchemaObjectBase schemaObject)
		{
			return GetSafeName(schemaObject.Name);
		}

		/// <summary>
		/// Get the safe name for a data object by determining if it contains spaces or other illegal
		/// characters - if so wrap with []
		/// </summary>
		/// <param name="objectName">The name of the database schema object</param>
		/// <returns>The safe name of the object</returns>
		public string GetSafeName(string objectName)
		{
			return objectName.IndexOfAny(new char[]{' ', '@', '-', ',', '!'}) > -1 ? "[" + objectName + "]" : objectName;
		}

		/// <summary>
		/// Get the camel cased version of a name.  
		/// If the name is all upper case, change it to all lower case
		/// </summary>
		/// <param name="name">Name to be changed</param>
		/// <returns>CamelCased version of the name</returns>
		public string GetCamelCaseName(string name)
		{
			if (name.Equals(name.ToUpper()))
				return name.ToLower().Replace(" ","");
			else
				return name.Substring(0, 1).ToLower() + name.Substring(1).Replace(" ","");
		}
		
		/// <summary>
		/// Get the Pascal cased version of a name.  
		/// </summary>
		/// <param name="name">Name to be changed</param>
		/// <returns>PascalCased version of the name</returns>
		public string GetPascalCaseName(string name)
		{		
			return name.Substring(0, 1).ToUpper() + name.Substring(1);
		}
		
		/// <summary>
		/// Remove any non-word characters from a SchemaObject's name (word characters are a-z, A-Z, 0-9, _)
		/// so that it may be used in code
		/// </summary>
		/// <param name="schemaObject">DB Object whose name is to be cleaned</param>
		/// <returns>Cleaned up object name</returns>
		public string GetCleanName(SchemaObjectBase schemaObject)
		{
			return GetCleanName(schemaObject.Name);
		}
		
		
		private string ApplyBaseClassFormat(string className)
		{
			return string.Format(baseClassFormat, className);
		}
		
		#region Business object class name

		public string GetAbstractClassName(string tableName)
		{
			return ApplyBaseClassFormat(GetClassName(tableName));
		}

		public string GetPartialClassName(string tableName)
		{
			return string.Format("{0}.generated", GetClassName(tableName));
		}
		
		public string GetEnumName(string tableName)
		{
			return string.Format(this.enumFormat, GetClassName(tableName));
		}
				
		
		// Create a class name from a table name, for a business object
		public string GetClassName(string tableName)
		{
			// 1.remove space or bad characters
			string name = GetCleanName(string.Format(this.entityFormat, tableName));
			
			// 2. Set Pascal case
			name = GetPascalCaseName(name);
			
			// 3. Remove any plural - Experimental, need more grammar analysis//ref: http://www.gsu.edu/~wwwesl/egw/crump.htm
			ArrayList invariants = new ArrayList();
			invariants.Add("alias");
			
			
			if (invariants.Contains(name.ToLower()))
			{
				return name;
			}
			else if (name.EndsWith("ies"))
			{
				return name.Substring(0, name.Length-3) + "y";
			}
			else if (name.EndsWith("s") && !(name.EndsWith("ss") || name.EndsWith("us")))
			{
				return name.Substring(0, name.Length-1);
			}
			else
				return name;			
		}		
		#endregion
		
		#region collection class name
		public string GetAbstractCollectionClassName(string tableName)
		{
			return ApplyBaseClassFormat(GetCollectionClassName(tableName));
		}
		public string GetCollectionClassName(string tableName)
		{
			return string.Format(collectionFormat, GetClassName(tableName));
		}
		#endregion

		#region Provider class name
		public string GetProviderName(string tableName)
		{
			return string.Format(providerFormat, GetClassName(tableName));
		}
		
		public string GetProviderClassName(string tableName)
		{
			return GetProviderName(tableName);
		}
		
		public string GetIProviderName(string tableName)
		{
			return string.Format(interfaceFormat, GetProviderClassName(tableName));
		}
		public string GetProviderBaseName(string tableName)
		{
			return ApplyBaseClassFormat(GetProviderClassName(tableName));
		}
		
		public string GetProviderTestName(string tableName)
		{
			return string.Format(unitTestFormat, GetClassName(tableName));
		}
		#endregion
		
		#region Factory class name
				
		public string GetAbstractRepositoryClassName(string tableName)
		{
			return ApplyBaseClassFormat(GetRepositoryClassName(tableName));
		}
		
		public string GetRepositoryClassName(string tableName)
		{
			return GetProviderName(tableName);
		}		
		
		public string GetRepositoryTestClassName(string tableName)
		{
			return string.Format(unitTestFormat, GetClassName(tableName));
		}
		#endregion
		
		/// <summary>
		/// Remove any non-word characters from a name (word characters are a-z, A-Z, 0-9, _)
		/// so that it may be used in code
		/// </summary>
		/// <param name="name">name to be cleaned</param>
		/// <returns>Cleaned up object name</returns>
		public string GetCleanName(string name)
		{
			return Regex.Replace(name, @"[\W]", "");
		}
		
		public string GetPropertyName(string name)
		{		
		   	name = Regex.Replace(name, @"[\W]", "");
			name = GetPascalCaseName(name);
			return name;
		}
		
		public string GetObjectPropertyAccessor(ColumnSchema column, string objectName)
		{
			return objectName + "." + GetPropertyName(column.Name);
		}
		
		public string GetObjectPropertyAccessor(ViewColumnSchema column, string objectName)
		{
			return objectName + "." + GetPropertyName(column.Name);
		}
		
		public string GetPrivateName(ColumnSchema column)
		{
			return GetPrivateName(column.Name);
		}
						
		public string GetPrivateName(string name)
		{		
		   	name = Regex.Replace(name, @"[\W]", "");
			name = GetCamelCaseName(name);
			
			if (name == "internal" || name == "class" || name == "public" || name == "private")
			{
				name = "p" + name;
			}
			
			return name;
		}

		public string GetManyToManyName(TableKeySchema junctionTableKey, string junctionTableName)
		{			
			return GetManyToManyName(junctionTableKey.ForeignKeyMemberColumns, junctionTableName);
		}
		
		public string GetManyToManyName(ColumnSchemaCollection columns, string junctionTableName)
		{
			string result = string.Empty;
			foreach(ColumnSchema pCol in columns)
			{
				result += GetCleanName(pCol.Name);
			}
			return string.Format(this.manyToManyFormat, result, junctionTableName);
		}
		
		/// <summary>
		/// Check that a given key has all foreign's columns into the primary key.
		/// </summary>
		/// <param name="key">The key to check.</param>
		public bool IsJunctionKey(TableKeySchema key)
		{
			foreach(ColumnSchema col in key.ForeignKeyMemberColumns)
			{
				if (!col.IsPrimaryKeyMember)
					return false;
			}
			return true;
		}
		
		/// <summary>
		/// Check that a given index has all it's columns into the primary key.
		/// </summary>
		/// <param name="index">The index to check.</param>
		public bool IsPrimaryKey(IndexSchema index)
		{
			foreach(ColumnSchema col in index.MemberColumns)
			{
				if (!col.IsPrimaryKeyMember)
					return false;
			}
			return true;
		}

		/// <summary>
		/// Get the cleaned, camelcased name of a parameter
		/// </summary>
		/// <param name="par">Command Parameter</param>
		/// <returns>the cleaned, camelcased name </returns>
		public string GetCleanParName(ParameterSchema par)
		{
			return GetCleanParName(par.Name);
		}

		/// <summary>
		/// Get the cleaned, camelcased version of a name
		/// </summary>
		/// <param name="name">name to be cleaned</param>
		/// <returns>the cleaned, camelcased name </returns>
		public string GetCleanParName(string name)
		{
			return GetCamelCaseName(GetCleanName(name));
		}

		/// <summary>
		/// Get the member variable styled version of a name
		/// </summary>
		/// <param name="column">The ColumnSchema with the name to be cleaned</param>
		/// <returns>the cleaned, camelcased name with a _ prefix</returns>
		public string GetMemberVariableName(DataObjectBase column)
		{
			return "_" + GetCleanParName(column.Name);
		}

		/// <summary>
		/// Get the member variable styled version of a name
		/// </summary>
		/// <param name="name">name to be cleaned</param>
		/// <returns>the cleaned, camelcased name with a _ prefix</returns>
		public string GetMemberVariableName(string name)
		{
			return "_" + GetCleanParName(name);
		}
		
		/// <summary>
		/// Get the member variable styled version of a name
		/// </summary>
		/// <param name="column">The column with the name to be cleaned</param>
		/// <returns>the cleaned, pascal cases name with a _Original prefix</returns>
		public string GetOriginalMemberVariableName(ColumnSchema column)
		{
			return GetOriginalMemberVariableName(column.Name);
		}
		
		/// <summary>
		/// Get the member variable styled version of a name
		/// </summary>
		/// <param name="name">name to be cleaned</param>
		/// <returns>the cleaned, camelcased name with a _ prefix</returns>
		public string GetOriginalMemberVariableName(string name)
		{
			return "_Original" + GetPropertyName(name);
		}

		/// <summary>
		/// Get the description ext. property of a column and return as inline SQL comment
		/// </summary>
		/// <param name="schemaObject">Any database object, but typically a column</param>
		/// <returns>Object description, as inline SQL comment</returns>
		public string GetColumnSqlComment(SchemaObjectBase schemaObject)
		{
			return schemaObject.Description.Length > 0 ? "-- " + schemaObject.Description : "";
		}

		/// <summary>
		/// Check if a column is an identity column
		/// </summary>
		/// <param name="column">DB table column to be checked</param>
		/// <returns>Identity?</returns>
		public bool IsIdentityColumn(ColumnSchema column)
		{
			return (bool)column.ExtendedProperties["CS_IsIdentity"].Value;
		}

		public bool IsDefaultSet(ColumnSchema column)
		{
			return !String.IsNullOrEmpty(column.ExtendedProperties["CS_Default"].Value as string);
		}

		public string GetColumnDefault(ColumnSchema column)
		{
			return column.ExtendedProperties["CS_Default"].Value as string;
		}

		public bool IsReadOnlyColumn(ColumnSchema column)
		{
			if (column.ExtendedProperties["CS_ReadOnly"].Value != null)
				return (bool)column.ExtendedProperties["CS_ReadOnly"].Value;
			else 
				return false;
		}
		
		public bool IsComputed(ColumnSchema column)
		{
			return (bool)column.ExtendedProperties["CS_IsComputed"].Value == true || column.NativeType.ToLower() == "timestamp";
		}

		/// <summary>
		/// Get the owner of a table
		/// </summary>
		/// <param name="table">The table to check</param>
		/// <returns>The safe name of the owner of the table</returns>
		public string GetOwner(TableSchema table)
		{
			return (table.Owner.Length > 0) ? GetSafeName(table.Owner) + "." : "";
		}
		
		/// <summary>
		/// Get the owner of a view
		/// </summary>
		/// <param name="view">The view to check</param>
		/// <returns>The safe name of the owner of the view</returns>
		public string GetOwner(ViewSchema view)
		{
			return (view.Owner.Length > 0) ? GetSafeName(view.Owner) + "." : "";
		}

		/// <summary>
		/// Get the owner of a command
		/// </summary>
		/// <param name="command">The command to check</param>
		/// <returns>The safe name of the owner of the command</returns>
		public string GetOwner(CommandSchema command)
		{
			return (command.Owner.Length > 0) ? GetSafeName(command.Owner) + "." : "";
		}

		/// <summary>
		/// Does the command have a resultset?
		/// </summary>
		/// <param name="cmd">Command in question</param>
		/// <returns>Resultset?</returns>
		public bool HasResultset(CommandSchema cmd)
		{
			return cmd.CommandResults.Count > 0;
		}
		
		/// <summary>
		/// Get a SqlParameter statement for a column
		/// </summary>
		/// <param name="columns">Columns for which to get the Sql parameter statement</param>
		/// <returns>Sql Parameter statement</returns>
		public string GetSqlParameterStatement(ColumnSchemaCollection columns)
		{
			string result = string.Empty;
			
			for(int i=0; i<columns.Count; i++)
			{
				if (i>0) result += ", ";
				
				result += GetSqlParameterStatement(columns[i]) + Environment.NewLine;
				
			}	
			return result;
		}

		/// <summary>
		/// Get a SqlParameter statement for a column
		/// </summary>
		/// <param name="column">Column for which to get the Sql parameter statement</param>
		/// <returns>Sql Parameter statement</returns>
		public string GetSqlParameterStatement(ColumnSchema column)
		{
			return GetSqlParameterStatement(column, false);
		}
		
		public string GetSqlParameterStatement(ColumnSchema column, bool isOutput) 
		{
			return GetSqlParameterStatement(column, null, isOutput);
		}

		/// <summary>
		/// Get a SqlParameter statement for a column
		/// </summary>
		/// <param name="column">Column for which to get the Sql parameter statement</param>
		/// <param name="isOutput">Is this an output parameter?</param>
		/// <returns>Sql Parameter statement</returns>
		public string GetSqlParameterStatement(ColumnSchema column, string defaultValue, bool isOutput)
		{
			string param = "@" + GetPropertyName(column.Name) + " as " + column.NativeType.ToLower();
			
			switch (column.DataType)
			{
				case DbType.Decimal:
				{
					if (column.NativeType != "real")
						param += "(" + column.Precision + ", " + column.Scale + ")";
				
					break;
				}
				// [ab 022205] now handles xxxbinary data type
				case DbType.Binary:
				// 
				case DbType.AnsiString:
				case DbType.AnsiStringFixedLength:
				case DbType.String:
				case DbType.StringFixedLength:
				{
					if (column.NativeType != "text" && 
						column.NativeType != "ntext" && 
						column.NativeType != "timestamp" &&
						column.NativeType != "image"
						)

					{
						if (column.Size > 0)
						{
							param += "(" + column.Size + ")";
						}
					}
					break;
				}
			}
			
			if (defaultValue != null && defaultValue.Length != 0)
			{
				param += " = " + defaultValue;
			}

			if (isOutput)
			{
				param += " output";
			}
			
			return param;
		}
		/// <summary>
		/// Get a SqlParameter statement for a column
		/// </summary>
		/// <param name="column">Column for which to get the Sql parameter statement</param>
		/// <param name="Name">the name of the parameter?</param>
		/// <returns>Sql Parameter statement</returns>
		public string GetSqlParameterStatement(ColumnSchema column, string Name)
		{
			string param = "@" + GetPropertyName(Name) + " " + column.NativeType;
			
			switch (column.DataType)
			{
				case DbType.Decimal:
				{
					param += "(" + column.Precision + ", " + column.Scale + ")";
					break;
				}
				case DbType.AnsiString:
				case DbType.AnsiStringFixedLength:
				case DbType.String:
				case DbType.StringFixedLength:
				{
					if (column.NativeType != "text" && column.NativeType != "ntext")
					{
						if (column.Size > 0)
						{
							param += "(" + column.Size + ")";
						}
					}
					break;
				}
			}	
			return param;
		}
		
		/// <summary>
		/// Get a SqlParameter statement for a column
		/// </summary>
		/// <param name="column">Column for which to get the Sql parameter statement</param>
		/// <param name="isOutput">indicates the direction</param>
		/// <returns>Sql Parameter statement</returns>
		public string GetSqlParameterXmlNode(ColumnSchema column, bool isOutput)
		{
			return GetSqlParameterXmlNode(column, column.Name, isOutput);
			//string formater = "<parameter name=\"@{0}\" type=\"{1}\" direction=\"{2}\" size=\"{3}\" precision=\"{4}\" scale=\"{5}\" param=\"{6}\"/>";			
			//return string.Format(formater, GetPropertyName(column.Name), column.NativeType, isOutput ? "Output" : "Input", column.Size, column.Precision, column.Scale, GetSqlParameterParam(column));
		}
		
		/// <summary>
		/// Get a SqlParameter statement for a column
		/// </summary>
		/// <param name="column">Column for which to get the Sql parameter statement</param>
		/// <param name="parameterName">the name of the parameter?</param>
		/// <param name="isOutput">indicates the direction</param>
		/// <returns>the xml Sql Parameter statement</returns>
		public string GetSqlParameterXmlNode(ColumnSchema column, string parameterName, bool isOutput)
		{
			string formater = "<parameter name=\"@{0}\" type=\"{1}\" direction=\"{2}\" size=\"{3}\" precision=\"{4}\" scale=\"{5}\" param=\"{6}\"/>";			
			return string.Format(formater, GetPropertyName(parameterName), column.NativeType, isOutput ? "Output" : "Input", column.Size, column.Precision, column.Scale, GetSqlParameterParam(column));
		}
		
		public string GetSqlParameterParam(ColumnSchema column)
		{
			string param = string.Empty;
			
			switch (column.DataType)
			{
				case DbType.Decimal:
				{
					param = "(" + column.Precision + ", " + column.Scale + ")";
					break;
				}
				case DbType.AnsiString:
				case DbType.AnsiStringFixedLength:
				case DbType.String:
				case DbType.StringFixedLength:
				{
					if (column.NativeType != "text" && column.NativeType != "ntext")
					{
						if (column.Size > 0)
						{
							param = "(" + column.Size + ")";
						}
					}
					break;
				}
			}	
			return param;
		}

		/// <summary>
		/// Parse the text of a stored procedure to retrieve any comment prior to the CREATE PROC construct
		/// </summary>
		/// <param name="commandText">Command Text of the procedure</param>
		/// <returns>The procedure header comment</returns>
		public string GetSqlProcedureComment(string commandText)
		{
			string comment = "";
			// Find anything upto the CREATE PROC statement
			Regex regex = new Regex(@"CREATE[\s]*PROC", RegexOptions.IgnoreCase);	
			comment = regex.Split(commandText)[0];
			//remove comment characters
			regex = new Regex(@"(-{2,})|(/\*)|(\*/)");
			comment = regex.Replace(comment, string.Empty);
			//trim and return
			return comment.Trim();
		}

		/// <summary>
		/// Get any in-line SQL comments on the same lines as parameters
		/// </summary>
		/// <param name="commandText">Command Text of the procedure</param>
		/// <returns>Hashtable of parameter comments, with parameter names as keys</returns>
		public Hashtable GetSqlParameterComments(string commandText)
		{
			Hashtable paramComments = new Hashtable();
			//Get parameter names and comments
			string pattern = @"(?<param>@\w*)[^@]*--(?<comment>.*)";
			//loop through the matches and extract the parameter and the comment, ignoring duplicates
			foreach (Match match in Regex.Matches(commandText, pattern))
				if (!paramComments.ContainsKey(match.Groups["param"].Value))
					paramComments.Add(match.Groups["param"].Value, match.Groups["comment"].Value.Trim());
			//return the hashtable
			return paramComments;
		}
		
		public string GetFunctionHeaderParameters(ColumnSchemaCollection columns)
		{
			string output = "";
			for (int i = 0; i < columns.Count; i++)
			{
				output += GetCSType(columns[i]) + " ";
				output +=  GetPrivateName(columns[i].Name);
				if (i < columns.Count - 1)
				{
					output += ", ";
				}
			}
			return output;
		}
		
		
		public string GetFunctionCallParameters(ColumnSchemaCollection columns)
		{
			string output = "";
			for (int i = 0; i < columns.Count; i++)
			{
				output +=  GetPrivateName(columns[i].Name);
				if (i < columns.Count - 1)
				{
					output += ", ";
				}
			}
			return output;
		}
		
		public string GetFunctionEntityParameters(ColumnSchemaCollection columns)
		{
			string output = "";
			for (int i = 0; i < columns.Count; i++)
			{
				output +=  "entity." + GetPropertyName(columns[i].Name);
				if (i < columns.Count - 1)
				{
					output += ", ";
				}
			}
			return output;
		}

		/// <summary>
		/// Convert database types to C# types
		/// </summary>
		/// <param name="field">Column or parameter</param>
		/// <returns>The C# (rough) equivalent of the field's data type</returns>
		public string GetCSType(DataObjectBase column)
		{
			bool setNullable = column.AllowDBNull || (column is ColumnSchema && (column as ColumnSchema).IsPrimaryKeyMember && IsDefaultSet(column as ColumnSchema));

			if (column.NativeType.ToLower() == "real")
				return "decimal" + (column.AllowDBNull ? "?" : "");
			else
			{
				DbType dataType = column.DataType;
				switch (dataType)
				{
					case DbType.AnsiString: 
						return "string";

					case DbType.AnsiStringFixedLength: 
						return "string";
					
					case DbType.String: 
						return "string";
						
					case DbType.Boolean:
						return "bool" + (setNullable ? "?" : "");
					
					case DbType.StringFixedLength: 
						return "string";
						
					case DbType.Guid:
						return "Guid" + (setNullable ? "?" : "");
					
					case DbType.Binary: 
						return "byte[]";
					
					case DbType.Byte:
						return "byte" + (setNullable ? "?" : "");
					
					case DbType.Currency:
						return "decimal" + (setNullable ? "?" : "");
					
					case DbType.Date:
						return "DateTime" + (setNullable ? "?" : "");
					
					case DbType.DateTime:
						return "DateTime" + (setNullable ? "?" : "");
					
					case DbType.Decimal:
						return "decimal" + (setNullable ? "?" : "");
					
					case DbType.Double:
						return "double" + (setNullable ? "?" : "");
					
					case DbType.Int16:
						return "short" + (setNullable ? "?" : "");
						
					case DbType.Int32:
						return "int" + (setNullable ? "?" : "");
						
					case DbType.Int64:
						return "long" + (setNullable ? "?" : "");
					
					case DbType.Object: 
						return "object";
					
					case DbType.Single:
						return "float" + (setNullable ? "?" : "");
					
					case DbType.Time:
						return "DateTime" + (setNullable ? "?" : "");
					
					case DbType.VarNumeric:
						return "int" + (setNullable ? "?" : "");
					
					default: 
						return "object";
				}
			}
		}

		public string GetReaderMethodByType(DataObjectBase column)
		{
			if (column.NativeType.ToLower() == "real")
				return "GetDecimal";
			else
			{
				DbType dataType = column.DataType;
				switch (dataType)
				{
					case DbType.AnsiString: 
						return "GetString";
						
					case DbType.AnsiStringFixedLength: 
						return "GetString";
					
					case DbType.String: 
						return "GetString";
						
					case DbType.Boolean: 
						return "GetBoolean";
					
					case DbType.StringFixedLength: 
						return "GetString";
						
					case DbType.Guid: 
						return "GetGuid";
					
					case DbType.Binary: 
						return "__UNKNOWN__";
					
					case DbType.Byte:
						return "GetByte";
					
					case DbType.Currency: 
						return "GetDecimal";
					
					case DbType.Date: 
						return "GetDateTime";
					
					case DbType.DateTime: 
						return "GetDateTime";
					
					case DbType.Decimal: 
						return "GetDecimal";
					
					case DbType.Double: 
						return "GetDouble";
					
					case DbType.Int16: 
						return "GetInt16";
						
					case DbType.Int32: 
						return "GetInt32";
						
					case DbType.Int64: 
						return "GetInt64";
					
					case DbType.Object: 
						return "GetValue";
					
					case DbType.Single: 
						return "GetSingle";
					
					case DbType.Time: 
						return "GetDateTime";
					
					case DbType.VarNumeric: 
						return "GetInt32";
					
					default: 
						return "GetValue";
				}
			}
		}

		public string GetCSDefaultByType(DataObjectBase column)
		{
			if (column.AllowDBNull || (column is ColumnSchema && (column as ColumnSchema).IsPrimaryKeyMember && IsDefaultSet(column as ColumnSchema)))
				return "(" + GetCSType(column) + ")null";

			if (column.NativeType.ToLower() == "real")
				return "0F";
			else
			{
				DbType dataType = column.DataType;
				switch (dataType)
				{
					case DbType.AnsiString: 
						return "String.Empty";
						
					case DbType.AnsiStringFixedLength: 
						return "String.Empty";
					
					case DbType.String: 
						return "String.Empty";
						
					case DbType.Boolean: 
						return "false";
					
					case DbType.StringFixedLength: 
						return "String.Empty";
						
					case DbType.Guid: 
						return "Guid.Empty";
					
					case DbType.Binary: 
						return "new byte[] {}";
					
					case DbType.Byte:
						return "(byte)0";
					
					case DbType.Currency: 
						return "0M";
					
					case DbType.Date: 
						return "(DateTime)SqlDateTime.MinValue";
					
					case DbType.DateTime: 
						return "(DateTime)SqlDateTime.MinValue";
					
					case DbType.Decimal: 
						return "0M";
					
					case DbType.Double: 
						return "0D";
					
					case DbType.Int16: 
						return "(short)0";
						
					case DbType.Int32: 
						return "0";
						
					case DbType.Int64: 
						return "(long)0";
					
					case DbType.Object: 
						return "null";
					
					case DbType.Single: 
						return "0F";
					
					case DbType.Time: 
						return "(DateTime)SqlDateTime.MinValue";
					
					case DbType.VarNumeric: 
						return "0";

					default: 
						return "null";
				}
			}
		}
		
		/// <summary>
		/// Generates a random number between the given bounds.
		/// </summary>
		/// <param name="min">lowest bound</param>
		/// <param name="max">highest bound</param>
		public int RandomNumber(int min, int max)
		{
			Random random = new Random();
			return random.Next(min, max); 
		}

		public string RandomString(ColumnSchema column, bool lowerCase)
		{
			//Debugger.Break();
			int size = 2; // default size
			
			// calculate maximum size of the field
			switch (column.DataType)
			{				
				case DbType.AnsiString:
				case DbType.AnsiStringFixedLength:
				case DbType.String:
				case DbType.StringFixedLength:
				{
					if (column.NativeType != "text" && column.NativeType != "ntext")
					{
						if (column.Size > 0)
						{
							size = column.Size;
						}
					}
					break;
				}
			}
			
			return RandomString((size/2) -1, lowerCase);
		}
		
		/// <summary>
		/// Generates a random string with the given length
		/// </summary>
		/// <param name="size">Size of the string</param>
		/// <param name="lowerCase">If true, generate lowercase string</param>
		/// <returns>Random string</returns>
		/// <remarks>Mahesh Chand  - http://www.c-sharpcorner.com/Code/2004/Oct/RandomNumber.asp</remarks>
		public string RandomString(int size, bool lowerCase)
		{
			StringBuilder builder = new StringBuilder();
			Random random = new Random(size);
			char ch ;
			for(int i=0; i<size; i++)
			{
				ch = Convert.ToChar(Convert.ToInt32(26 * random.NextDouble() + 65)) ;
				builder.Append(ch); 
			}
			if(lowerCase)
			return builder.ToString().ToLower();
			return builder.ToString();
		}


		/// <summary>
		/// Get the Sql Data type of a column
		/// </summary>
		/// <param name="column">Column for which to get the type</param>
		/// <returns>String representing the SQL data type</returns>
		public string GetSqlDbType(DataObjectBase column)	
		{
			switch (column.NativeType)
			{
				case "bigint": return "BigInt";
				case "binary": return "Binary";
				case "bit": return "Bit";
				case "char": return "Char";
				case "datetime": return "DateTime";
				case "decimal": return "Decimal";
				case "float": return "Float";
				case "image": return "Image";
				case "int": return "Int";
				case "money": return "Money";
				case "nchar": return "NChar";
				case "ntext": return "NText";
				case "numeric": return "Decimal";
				case "nvarchar": return "NVarChar";
				case "real": return "Real";
				case "smalldatetime": return "SmallDateTime";
				case "smallint": return "SmallInt";
				case "smallmoney": return "SmallMoney";
				case "sql_variant": return "Variant";
				case "sysname": return "NChar";
				case "text": return "Text";
				case "timestamp": return "Timestamp";
				case "tinyint": return "TinyInt";
				case "uniqueidentifier": return "UniqueIdentifier";
				case "varbinary": return "VarBinary";
				case "varchar": return "VarChar";
				default: return "__UNKNOWN__" + column.NativeType;
			}
		}
		
		public string FKColumnName(TableKeySchema fkey)
		{
			string Name = String.Empty;
			for(int x=0;x < fkey.ForeignKeyMemberColumns.Count;x++)
			{
				Name += GetPropertyName(fkey.ForeignKeyMemberColumns[x].Name);
			}
			return Name;
		}
		
		public string PKColumnName(TableKeySchema key)
		{
			string Name = String.Empty;
			for(int x=0;x < key.ForeignKeyMemberColumns.Count;x++)
			{
				Name += GetPropertyName(key.PrimaryKeyMemberColumns[x].Name);
			}
			return Name;
		}
		
		public string IXColumnName(IndexSchema index)
		{
			string Name = String.Empty;
			for(int x=0;x < index.MemberColumns.Count;x++)
			{
				Name += GetPropertyName(index.MemberColumns[x].Name);
			}
			return Name;
		}
		
		public string IXColumnNames(IndexSchema index)
		{
			string Name = String.Empty;
			for(int x=0;x < index.MemberColumns.Count;x++)
			{
				Name += ", " + GetPrivateName(index.MemberColumns[x].Name);
			}
			return Name.Substring(2);
		}
		
		public string GetKeysName(ColumnSchemaCollection keys)
		{	
			string result = String.Empty;
			for(int x=0; x < keys.Count;x++)
			{
				result += GetPropertyName(keys[x].Name);
			}
			return result;
		}
	
		/// <summary>
		/// Indicates if the output rowset of the command is compliant with the table rowset.
		/// </summary>
		/// <param name="command">The stored procedure</param>
		/// <param name="table">The table</param>
		public bool IsMatching(CommandSchema command, TableSchema table)
		{
			if (command.CommandResults.Count != 1)
			{
				return false;
			}
			
			if (command.CommandResults[0].Columns.Count != table.Columns.Count)
			{
				return false;
			}
			
			for(int i=0; i<table.Columns.Count; i++) //  CommandResultSchema cmdResult in command.CommandResults)
			{
				if (command.CommandResults[0].Columns[i].Name != table.Columns[i].Name)
				{
					return false;
				}
				
				if (command.CommandResults[0].Columns[i].NativeType != table.Columns[i].NativeType)
				{
					return false;
				}
			}
			return true;
		}
		
		/// <summary>
		/// Indicates if the output rowset of the command is compliant with the view rowset.
		/// </summary>
		/// <param name="command">The stored procedure</param>
		/// <param name="view">The view</param>
		public bool IsMatching(CommandSchema command, ViewSchema view)
		{
			if (command.CommandResults.Count != 1)
			{
				return false;
			}
			
			if (command.CommandResults[0].Columns.Count != view.Columns.Count)
			{
				return false;
			}
			
			for(int i=0; i<view.Columns.Count; i++)
			{
				if (command.CommandResults[0].Columns[i].Name != view.Columns[i].Name)
				{
					return false;
				}
				
				if (command.CommandResults[0].Columns[i].NativeType != view.Columns[i].NativeType)
				{
					return false;
				}
			}
			return true;
		}

		public string GetFunctionRelationshipCallParameters(ColumnSchemaCollection columns)
		{
			string output = "";
			for (int i = 0; i < columns.Count; i++)
			{
				output +=  "entity." + GetPropertyName(columns[i].Name);
				if (i < columns.Count - 1)
				{
					output += ", ";
				}
			}
			return output;
		}

		/// <summary>
		/// Checks to see if the table is a junction table, meaning it is used in a many-to-many relationship.
		/// </summary>
		/// <returns>Returns true when all primary keys have foreign key relationships.</returns>
		public bool IsJunctionTable(TableSchema table)
		{
			if (table.PrimaryKey == null || table.PrimaryKey.MemberColumns.Count <= 1)
				return false;

			for (int i = 0; i < table.PrimaryKey.MemberColumns.Count; i++)
				if (!table.PrimaryKey.MemberColumns[i].IsForeignKeyMember)
					return false;

			return true;			
		}
		
		public bool IsRelationOneToOne(TableKeySchema schema)
		{
			bool relationOkay = true;
			
			for(int i = 0; i < schema.PrimaryKeyMemberColumns.Count; i++)
				relationOkay = schema.PrimaryKeyMemberColumns[i].IsUnique;
				
			for(int i = 0; i < schema.ForeignKeyMemberColumns.Count; i++)
				relationOkay = schema.ForeignKeyMemberColumns[i].IsUnique;
				
			return relationOkay;
		}

		public bool IsRelationOneToMany(TableKeySchema schema)
		{
			bool relationOkay = true;
			
			for(int i = 0; i < schema.PrimaryKeyMemberColumns.Count; i++)
				relationOkay = schema.PrimaryKeyMemberColumns[i].IsUnique;
				
			for(int i = 0; i < schema.ForeignKeyMemberColumns.Count; i++)
				relationOkay = !schema.ForeignKeyMemberColumns[i].IsUnique;
				
			return relationOkay;
		}

		public bool IsRelationManyToOne(TableKeySchema schema)
		{
			bool relationOkay = true;
			
			for(int i = 0; i < schema.PrimaryKeyMemberColumns.Count; i++)
				relationOkay = !schema.PrimaryKeyMemberColumns[i].IsUnique;
				
			for(int i = 0; i < schema.ForeignKeyMemberColumns.Count; i++)
				relationOkay = schema.ForeignKeyMemberColumns[i].IsUnique;
				
			return relationOkay;
		}

		public TableKeySchemaCollection GetDepedentsOf (TableSchema table) 
		{
			List<TableKeySchema> foreignKeys = new List<TableKeySchema>();
			
			foreach(TableSchema foreignTable in SourceTables)
			{
				foreach(TableKeySchema key in foreignTable.ForeignKeys)
				{
					// check to see if the tables are linked
					if (key.PrimaryKeyTable.Name == table.Name)
						foreignKeys.Add(key);
				}
			}
			
			return new TableKeySchemaCollection(foreignKeys.ToArray());
		}
	}
}
