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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;

/* 
 * Tells the compiler to make sure the whole assembly is CLS Compliant, this 
 * is nessisary for users that may not be using C# to access and use this
 * assembly.
 */

[assembly : CLSCompliant(true)]
[assembly : ComVisible(false)]

/* 
 * These security permissions will garontee that ManagedFusion will not grant access to
 * anything that should not have access.  Refusing permission would ensure that your 
 * code cannot be misused by a malicious attack or a bug to alter files.
 */

[assembly : SecurityPermission(SecurityAction.RequestMinimum, ControlAppDomain=true)]
[assembly : SecurityPermission(SecurityAction.RequestMinimum, ControlPrincipal=true)]
[assembly : SecurityPermission(SecurityAction.RequestMinimum, Execution=true)]

/* 
 * General Information about an assembly is controlled through the following 
 * set of attributes. Change these attribute values to modify the information
 * associated with an assembly.
 */

#if DEBUG
[assembly : AssemblyTitle("ManagedFusion Core - DEBUG")]
[assembly : AssemblyConfiguration("Production/Stable - DEBUG")]
#else
[assembly : AssemblyTitle("ManagedFusion Core")]
[assembly : AssemblyConfiguration("Production/Stable")]
#endif
[assembly : AssemblyDescription("A description of the project can be found at www.ManagedFusion.net.  ManagedFusion is protected under the ManagedFusion Software License.")]
[assembly : AssemblyDefaultAlias("ManagedFusion")]
[assembly : AssemblyCompany("The ManagedFusion Project")]
[assembly : AssemblyProduct("ManagedFusion")]
[assembly : AssemblyCopyright("© Nicholas Berardi, All rights reserved.")]
[assembly : AssemblyTrademark("")]
[assembly : AssemblyCulture("")]	

/* 
 * Version information for an assembly consists of the following four values:
 * 
 *		Major Version
 *		Minor Version 
 *		Build Number
 *		Revision
 * 
 * You can specify all the values or you can default the Revision and Build Numbers 
 * by using the '*' as shown below:
 */

[assembly : AssemblyVersion("1.0")]						// Actual Version Information
[assembly : AssemblyFileVersion("1.0")]					// Version for Assembly File
[assembly : AssemblyInformationalVersion("2005.0")]		// Version for Product Information

/* 
 * In order to sign your assembly you must specify a key to use. Refer to the 
 * Microsoft .NET Framework documentation for more information on assembly signing.
 * 
 * Use the attributes below to control which key is used for signing. 
 * 
 * Notes: 
 *	(*)	If no key is specified, the assembly is not signed.
 *	(*)	KeyName refers to a key that has been installed in the Crypto Service
 *		Provider (CSP) on your machine. KeyFile refers to a file which contains
 *		a key.
 *	(*)	If the KeyFile and the KeyName values are both specified, the 
 *		following processing occurs:
 *		(1)	If the KeyName can be found in the CSP, that key is used.
 *		(2)	If the KeyName does not exist and the KeyFile does exist, the key 
 *			in the KeyFile is installed into the CSP and used.
 *	(*)	In order to create a KeyFile, you can use the sn.exe (Strong Name) utility.
 *		When specifying the KeyFile, the location of the KeyFile should be
 *		relative to the "project output directory". The location of the project output
 *		directory is dependent on whether you are working with a local or web project.
 *		For local projects, the project output directory is defined as
 *		<Project Directory>\obj\<Configuration>. For example, if your KeyFile is
 *		located in the project directory, you would specify the AssemblyKeyFile 
 *		attribute as [assembly: AssemblyKeyFile("..\\..\\mykey.snk")]
 *		For web projects, the project output directory is defined as
 *		%HOMEPATH%\VSWebCache\<Machine Name>\<Project Directory>\obj\<Configuration>.
 *	(*)	Delay Signing is an advanced option - see the Microsoft .NET Framework
 *		documentation for more information on this.
 */

[assembly : AssemblyDelaySign(false)]
[assembly : AssemblyKeyName("")]