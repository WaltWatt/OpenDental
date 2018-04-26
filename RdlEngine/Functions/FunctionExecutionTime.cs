/* ====================================================================
    Copyright (C) 2004-2006  fyiReporting Software, LLC

    This file is part of the fyiReporting RDL project.
	
    This library is free software; you can redistribute it and/or modify
    it under the terms of the GNU Lesser General public License as published by
    the Free Software Foundation; either version 2.1 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General public License for more details.

    You should have received a copy of the GNU Lesser General public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301  USA

    For additional information, email info@fyireporting.com or visit
    the website www.fyiReporting.com.
*/
using System;
using System.IO;

using fyiReporting.RDL;


namespace fyiReporting.RDL
{
	/// <summary>
	/// DateTime Report started; actually the time that data is retrieved
	/// </summary>
	[Serializable]
	internal class FunctionExecutionTime : IExpr
	{
		/// <summary>
		/// DateTime report started
		/// </summary>
		public FunctionExecutionTime() 
		{
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.DateTime;
		}

		public bool IsConstant()
		{
			return false;
		}

		public IExpr ConstantOptimization()
		{	// not a constant expression
			return this;
		}

		// Evaluate is for interpretation  
		public object Evaluate(Report rpt, Row row)
		{
			return EvaluateDateTime(rpt, row);
		}
		
		public double EvaluateDouble(Report rpt, Row row)
		{	
			DateTime result = EvaluateDateTime(rpt, row);
			return Convert.ToDouble(result);
		}
		
		public decimal EvaluateDecimal(Report rpt, Row row)
		{
			DateTime result = EvaluateDateTime(rpt, row);

			return Convert.ToDecimal(result);
		}

		public string EvaluateString(Report rpt, Row row)
		{
			DateTime result = EvaluateDateTime(rpt, row);
			return result.ToString();
		}

		public DateTime EvaluateDateTime(Report rpt, Row row)
		{
			return rpt.ExecutionTime;
		}
		
		public bool EvaluateBoolean(Report rpt, Row row)
		{
			return false;
		}
	}
}
