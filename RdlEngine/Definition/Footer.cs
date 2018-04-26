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
using System.IO;

namespace fyiReporting.RDL
{
	///<summary>
	///  Definition of footer rows for a table or group.
	///</summary>
	[Serializable]
	internal class Footer : ReportLink
	{
		TableRows _TableRows;	// The footer rows for the table or group
		bool _RepeatOnNewPage;	// Indicates this footer should be displayed on
								// each page that the table (or group) is displayed		
	
		internal Footer(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_TableRows=null;
			_RepeatOnNewPage=false;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "TableRows":
						_TableRows = new TableRows(r, this, xNodeLoop);
						break;
					case "RepeatOnNewPage":
						_RepeatOnNewPage = XmlUtil.Boolean(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					default:
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown Footer element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
			if (_TableRows == null)
				OwnerReport.rl.LogError(8, "TableRows element is required with a Footer but not specified.");
		}
		
		override internal void FinalPass()
		{
			_TableRows.FinalPass();
			return;
		}

		internal void Run(IPresent ip, Row row)
		{
			_TableRows.Run(ip, row);
			return;
		}

		internal void RunPage(Pages pgs, Row row)
		{

			Page p = pgs.CurrentPage;
			if (p.YOffset + HeightOfRows(pgs, row) > pgs.BottomOfPage)
			{
				p = OwnerTable.RunPageNew(pgs, p);
				OwnerTable.RunPageHeader(pgs, row, false, null);
			}
			_TableRows.RunPage(pgs, row);

			return;
		}

		internal TableRows TableRows
		{
			get { return  _TableRows; }
			set {  _TableRows = value; }
		}

		internal float HeightOfRows(Pages pgs, Row r)
		{
			return _TableRows.HeightOfRows(pgs, r);
		}

		internal bool RepeatOnNewPage
		{
			get { return  _RepeatOnNewPage; }
			set {  _RepeatOnNewPage = value; }
		}

		internal Table OwnerTable
		{
			get 
			{ 
				ReportLink rl = this.Parent;
				while (rl != null)
				{
					if (rl is Table)
						return rl as Table;
					rl = rl.Parent;
				}
				return null;
			}
		}
	}
}
