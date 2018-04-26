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

namespace fyiReporting.RDL
{
	///<summary>
	/// Collection of matrix rows
	///</summary>
	[Serializable]
	internal class MatrixRows : ReportLink
	{
        List<MatrixRow> _Items;			// list of MatrixRow

		internal MatrixRows(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			MatrixRow m;
            _Items = new List<MatrixRow>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "MatrixRow":
						m = new MatrixRow(r, this, xNodeLoop);
						break;
					default:	
						m=null;		// don't know what this is
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown MatrixRows element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
				if (m != null)
					_Items.Add(m);
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "For MatrixRows at least one MatrixRow is required.");
			else
                _Items.TrimExcess();
		}
		
		override internal void FinalPass()
		{
			foreach (MatrixRow m in _Items)
			{
				m.FinalPass();
			}
			return;
		}

		internal float DefnHeight()
		{
			float height=0;
			foreach (MatrixRow m in _Items)
			{
				height += m.Height.Points;
			}
			return height;
		}

        internal List<MatrixRow> Items
		{
			get { return  _Items; }
		}

		/// <summary>
		/// CellCount requires a correctly configured Matrix structure.  This is true at runtime
		/// but not necessarily true during Matrix parse.
		/// </summary>
		internal int CellCount
		{
			get 
			{
				MatrixRow mr = _Items[0] as MatrixRow;
				return mr.MatrixCells.Items.Count;
			}
		}
	}
}
