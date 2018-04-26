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
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace fyiReporting.RDL
{
	///<summary>
	/// TableRows definition and processing.
	///</summary>
	[Serializable]
	internal class TableRows : ReportLink
	{
        List<TableRow> _Items;			// list of TableRow
		float _HeightOfRows;		// height of contained rows
		bool _CanGrow;				// if any TableRow contains a TextBox with CanGrow

		internal TableRows(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			TableRow t;
            _Items = new List<TableRow>();
			_CanGrow = false;
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "TableRow":
						t = new TableRow(r, this, xNodeLoop);
						break;
					default:	
						t=null;		// don't know what this is
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown TableRows element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
				if (t != null)
					_Items.Add(t);
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "For TableRows at least one TableRow is required.");
			else
                _Items.TrimExcess();
		}
		
		override internal void FinalPass()
		{
			_HeightOfRows = 0;
			foreach (TableRow t in _Items)
			{
				_HeightOfRows += t.Height.Points;
				t.FinalPass();
				_CanGrow |= t.CanGrow;
			}

			return;
		}

		internal void Run(IPresent ip, Row row)
		{
			foreach (TableRow t in _Items)
			{
				t.Run(ip, row);
			}
			return;
		}

		internal void RunPage(Pages pgs, Row row)
		{
			RunPage(pgs, row, false);
		}

		internal void RunPage(Pages pgs, Row row, bool bCheckRows)
		{
			if (bCheckRows)
			{	// we need to check to see if a row will fit on the page
				foreach (TableRow t in _Items)
				{
					Page p = pgs.CurrentPage;			// this can change after running a row
					float hrows = t.HeightOfRow(pgs, row);	// height of this row
					float height = p.YOffset + hrows;
					if (height > pgs.BottomOfPage)
					{
						p = OwnerTable.RunPageNew(pgs, p);
						OwnerTable.RunPageHeader(pgs, row, false, null);
					}
					t.RunPage(pgs, row);
				}
			}
			else
			{	// all rows will fit on the page
				foreach (TableRow t in _Items)
					t.RunPage(pgs, row);
			}
			return;
		}

		internal Table OwnerTable
		{
			get 
			{
				for (ReportLink rl = this.Parent; rl != null; rl = rl.Parent)
				{
					if (rl is Table)
						return rl as Table;
				}

				throw new Exception("Internal error.  TableRows must be owned eventually by a table.");
			}
		}

		internal float DefnHeight()
		{
			float height=0;
			foreach (TableRow tr in this._Items)
			{
				height += tr.Height.Points;
			}
			return height;
		}

		internal float HeightOfRows(Pages pgs, Row r)
		{
			if (!this._CanGrow)
				return _HeightOfRows;
			
			float height=0;
			foreach (TableRow tr in this._Items)
			{
				height += tr.HeightOfRow(pgs, r);
			}

			return Math.Max(height, _HeightOfRows);
		}

        internal List<TableRow> Items
		{
			get { return  _Items; }
		}
	}
}
