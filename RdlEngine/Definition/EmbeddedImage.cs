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
	/// The defintion of an embedded images, including the actual image data and type.
	///</summary>
	[Serializable]
	internal class EmbeddedImage : ReportLink
	{
		Name _Name;			// Name of the image.
		string _MIMEType;	// The MIMEType for the image. Valid values are:
							// image/bmp, image/jpeg, image/gif, image/png, image/xpng.
		string _ImageData;	// Base-64 encoded image data.		
	
		internal EmbeddedImage(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Name=null;
			_MIMEType=null;
			_ImageData=null;
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
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "MIMEType":
						_MIMEType = xNodeLoop.InnerText;
						break;
					case "ImageData":
						_ImageData = xNodeLoop.InnerText;
						break;
					default:	
						this.OwnerReport.rl.LogError(4, "Unknown Report element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}

			if (this.Name == null)
			{
				OwnerReport.rl.LogError(8, "EmbeddedImage Name is required but not specified.");
			}
			else
			{
				try
				{
					OwnerReport.LUEmbeddedImages.Add(this.Name.Nm, this);		// add to referenceable embedded images
				}
				catch		// Duplicate name
				{
					OwnerReport.rl.LogError(4, "Duplicate EmbeddedImage  name '" + this.Name.Nm + "' ignored.");
				}
			}
			if (_MIMEType == null)
				OwnerReport.rl.LogError(8, "EmbeddedImage MIMEType is required but not specified for " + (this.Name == null? "'name not specified'": this.Name.Nm));

			if (_ImageData == null)
				OwnerReport.rl.LogError(8, "EmbeddedImage ImageData is required but not specified for " + (this.Name == null? "'name not specified'": this.Name.Nm));
		}
		
		override internal void FinalPass()
		{
			return;
		}

		internal Name Name
		{
			get { return  _Name; }
			set {  _Name = value; }
		}

		internal string MIMEType
		{
			get { return  _MIMEType; }
			set {  _MIMEType = value; }
		}

		internal string ImageData
		{
			get { return  _ImageData; }
			set {  _ImageData = value; }
		}
	}
}
