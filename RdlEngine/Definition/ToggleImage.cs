/* ====================================================================
    Copyright (C) 2004-2006  fyiReporting Software, LLC

    This file is part of the fyiReporting RDL project.
	
    This library is free software; you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation; either version 2.1 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301  USA

    For additional information, email info@fyireporting.com or visit
    the website www.fyiReporting.com.
*/

using System;
using System.Xml;

namespace fyiReporting.RDL
{
	///<summary>
	/// Toggle image definition.
	///</summary>
	[Serializable]
	internal class ToggleImage : ReportLink
	{
		Expression _InitialState;	//(Boolean)
					//A Boolean expression, the value of which
					//determines the initial state of the toggle image.
					//True = �expanded� (i.e. a minus sign). False =
					//�collapsed� (i.e. a plus sign)		
	
		internal ToggleImage(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_InitialState=null;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "InitialState":
						_InitialState = new Expression(r, this, xNodeLoop, ExpressionType.Boolean);
						break;
					default:
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown ToggleImage element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
			if (_InitialState == null)
				OwnerReport.rl.LogError(8, "ToggleImage requires the InitialState element.");
		}

		// Handle parsing of function in final pass
		override internal void FinalPass()
		{
			if (_InitialState != null)
				_InitialState.FinalPass();
			return;
		}

		internal Expression InitialState
		{
			get { return  _InitialState; }
			set {  _InitialState = value; }
		}
	}
}
