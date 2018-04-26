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
	/// TableCells definition and processing.
	///</summary>
	[Serializable]
	internal class TableCells : ReportLink
	{
        List<TableCell> _Items;			// list of TableCell

		internal TableCells(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			TableCell tc;
            _Items = new List<TableCell>();
			// Loop thru all the child nodes
			int colIndex=0;			// keep track of the column numbers
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "TableCell":
						tc = new TableCell(r, this, xNodeLoop, colIndex);
						colIndex += tc.ColSpan;
						break;
					default:	
						tc=null;		// don't know what this is
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown TableCells element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
				if (tc != null)
					_Items.Add(tc);
			}
			if (_Items.Count > 0)
                _Items.TrimExcess();
		}
		
		override internal void FinalPass()
		{
			foreach (TableCell tc in _Items)
			{
				tc.FinalPass();
			}
			return;
		}

		internal void Run(IPresent ip, Row row)
		{
			foreach (TableCell tc in _Items)
			{
				tc.Run(ip, row);
			}
			return ;
		}

		internal void RunPage(Pages pgs, Row row)
		{
			// Start each row in the same location
			//   e.g. if there are two embedded tables in cells they both start at same location
			Page savepg = pgs.CurrentPage;
			float savey = savepg.YOffset;
			Page maxpg = savepg;
			float maxy = savey;

			foreach (TableCell tc in _Items)
			{
				tc.RunPage(pgs, row);
				if (maxpg != pgs.CurrentPage)
				{	// a page break
					if (maxpg.PageNumber < pgs.CurrentPage.PageNumber)
					{
						maxpg = pgs.CurrentPage;
						maxy = maxpg.YOffset;
					}
				}
				else if (maxy > pgs.CurrentPage.YOffset)
				{
					// maxy = maxy;      TODO what was this meant to do
				}
				// restore the beginning start of the row
				pgs.CurrentPage = savepg;
				savepg.YOffset = savey;
			}
			pgs.CurrentPage = maxpg;
			savepg.YOffset = maxy;
			return ;
		}

        internal List<TableCell> Items
		{
			get { return  _Items; }
		}
	}
}
