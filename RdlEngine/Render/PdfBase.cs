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
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace fyiReporting.RDL
{
	/// <summary>
	/// This is the base object for all objects used within the pdf.
	/// </summary>
	internal class PdfBase
	{
		/// <summary>
		/// Stores the Object Number
		/// </summary>
		internal int objectNum;
		internal PdfAnchor xref;
		/// <summary>
		/// Constructor increments the object number to 
		/// reflect the currently used object number
		/// </summary>
		protected PdfBase(PdfAnchor pa)
		{
			xref=pa;
			xref.current++;
			objectNum=xref.current;
		}

		internal int Current
		{
			get { return xref.current; }
		}
		/// <summary>
		/// Convert the unicode string 16 bits to unicode bytes. 
		/// This is written to the file to create Pdf 
		/// </summary>
		/// <returns></returns>
		internal byte[] GetUTF8Bytes(string str,long filePos,out int size)
		{
			ObjectList objList=new ObjectList(objectNum,filePos);
			byte []abuf;

            byte[] ubuf = Encoding.Unicode.GetBytes(str);
			Encoding enc = Encoding.GetEncoding(1252);
			abuf = Encoding.Convert(Encoding.Unicode, enc, ubuf);
			size=abuf.Length;
			xref.offsets.Add(objList);

            return abuf;
		}

	}

	/// <summary>
	/// Holds the Byte offsets of the objects used in the Pdf Document
	/// </summary>
	internal class PdfAnchor
	{
        internal List<ObjectList> offsets;
		internal int current;
		internal bool CanCompress;
		
		internal PdfAnchor(bool bCompress)
		{
            offsets = new List<ObjectList>();
			current=0;
			CanCompress = bCompress;
		}

		internal void Reset()
		{
			offsets.Clear();
			current=0;
		}
	}

	/// <summary>
	/// For Adding the Object number and file offset
	/// </summary>
	internal class ObjectList:IComparable
	{
		internal long offset;
		internal int objNum;

		internal ObjectList(int objectNum,long fileOffset)
		{
			offset=fileOffset;
			objNum=objectNum;
		}
		#region IComparable Members

		public int CompareTo(object obj)
		{

			int result=0;
			result=(this.objNum.CompareTo(((ObjectList)obj).objNum));
			return result;
		}

		#endregion
	}
}
