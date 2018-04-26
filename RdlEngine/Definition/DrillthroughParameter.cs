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
	/// A drillthrough parameter.
	///</summary>
	[Serializable]
	internal class DrillthroughParameter : ReportLink
	{
		Name _Name;			// Name of the parameter
		Expression _Value;	// (Variant) An expression that evaluates to the value to
							// hand in for the parameter to the Drillthough.
		Expression _Omit;	// (Boolean) Indicates the parameter should be skipped.
	
		internal DrillthroughParameter(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Name=null;
			_Value=null;
			_Omit=null;
			// Run thru the attributes
			foreach(XmlAttribute xAttr in xNode.Attributes)
			{
				switch (xAttr.Name)
				{
					case "Name":
						_Name = new Name(xAttr.Value);
						break;
				}
			}

			if (_Name == null)
			{	// Name is required for parameters
				OwnerReport.rl.LogError(8, "Parameter Name attribute required.'");
			}

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "Value":
						_Value = new Expression(r, this, xNodeLoop, ExpressionType.Variant);
						break;
					case "Omit":
						_Omit = new Expression(r, this, xNodeLoop, ExpressionType.Boolean);
						break;
					default:
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown Parameter element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
		}

		// Handle parsing of function in final pass
		override internal void FinalPass()
		{
			if (_Value != null)
				_Value.FinalPass();
			if (_Omit != null)
				_Omit.FinalPass();
			return;
		}

		internal Name Name
		{
			get { return  _Name; }
			set {  _Name = value; }
		}

		internal Expression Value
		{
			get { return  _Value; }
			set {  _Value = value; }
		}

		internal string ValueValue(Report rpt, Row r)
		{
			if (_Value == null)
				return "";

			return _Value.EvaluateString(rpt, r);
		}

		internal Expression Omit
		{
			get { return  _Omit; }
			set {  _Omit = value; }
		}

		internal bool OmitValue(Report rpt, Row r)
		{
			if (_Omit == null)
				return false;

			return _Omit.EvaluateBoolean(rpt, r);
		}
	}
}
