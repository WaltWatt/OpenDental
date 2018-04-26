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
using System.IO;
using System.Collections;
using System.Text;

namespace fyiReporting.RDL
{
	/// <summary>
	/// A simple Lexer that is used by Parser.
	/// </summary>
	internal class Lexer
	{
		private TokenList tokens;
		private CharReader reader;

		/// <summary>
		/// Initializes a new instance of the Lexer class with the specified
		/// expression syntax to lex.
		/// </summary>
		/// <param name="expr">An expression to lex.</param>
		internal Lexer(string expr)
			: this(new StringReader(expr))
		{
			// use this
		}

		/// <summary>
		/// Initializes a new instance of the Lexer class with the specified
		/// TextReader to lex.
		/// </summary>
		/// <param name="source">A TextReader to lex.</param>
		internal Lexer(TextReader source)
		{
			// token queue
			tokens = new TokenList();

			// read the file contents
			reader = new CharReader(source);
		}

		/// <summary>
		/// Breaks the input stream onto the tokens list and returns it.
		/// </summary>
		/// <returns>The tokens list.</returns>
		internal TokenList Lex()
		{
			Token token = GetNextToken();
			while(true)
			{
				if(token != null)
					tokens.Add(token);
				else
				{
					tokens.Add(new Token(null, reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.EOF));
					return tokens;
				}

				token = GetNextToken();
			}
		}

		private Token GetNextToken()
		{
			while(!reader.EndOfInput())
			{
				char ch = reader.GetNext();

				// skipping whitespaces
				if(Char.IsWhiteSpace(ch))
				{
					continue;
				}
				switch(ch)
				{
					case '=':
						return new Token(ch.ToString(), reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.EQUAL);
					case '+':
						return new Token(ch.ToString(), reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.PLUS);
					case '-':
						return new Token(ch.ToString(), reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.MINUS);
					case '(':
						return new Token(ch.ToString(), reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.LPAREN);
					case ')':
						return new Token(ch.ToString(), reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.RPAREN);
					case ',':
						return new Token(ch.ToString(), reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.COMMA);
					case '^':
						return new Token(ch.ToString(), reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.EXP);
					case '%':
						return new Token(ch.ToString(), reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.MODULUS);
					case '!':
						if (reader.Peek() == '=')
						{
							reader.GetNext();	// go past the equal
							return new Token(ch.ToString(), reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.NOTEQUAL);
						}
						else
							return new Token(ch.ToString(), reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.NOT);
					case '&':
						return new Token(ch.ToString(), reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.PLUSSTRING);
					case '|':
						if (reader.Peek() == '|')
						{
							reader.GetNext();	// go past the '|'
							return new Token(ch.ToString(), reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.OR);
						}
						break;
					case '>':
						if (reader.Peek() == '=')
						{
							reader.GetNext();	// go past the equal
							return new Token(ch.ToString(), reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.GREATERTHANOREQUAL);
						}
						else
							return new Token(ch.ToString(), reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.GREATERTHAN);
					case '/':
						if (reader.Peek() == '*')
						{	// beginning of a comment of form /* a comment */
							reader.GetNext();	// go past the '*'
							ReadComment();
							continue;
						}
						else
							return new Token(ch.ToString(), reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.FORWARDSLASH);
					case '<':
						if (reader.Peek() == '=')
						{
							reader.GetNext();	// go past the equal
							return new Token(ch.ToString(), reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.LESSTHANOREQUAL);
						}
						else
							return new Token(ch.ToString(), reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.LESSTHAN);
					case '*':
						return new Token(ch.ToString(), reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.STAR);
					case '"':
					case '\'':
						return ReadQuoted(ch);
					default:
						break;
				} // end of swith
				if (Char.IsDigit(ch) || ch == '.')
					return ReadNumber(ch);
				else if (Char.IsLetter(ch) || ch == '_')
					return ReadIdentifier(ch);
				else
					return new Token(ch.ToString(), reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.OTHER);
			}
			return null;
		}

		// Reads a decimal number with optional exponentiation 
		private Token ReadNumber(char ch)
		{
			int startLine = reader.Line;
			int startCol = reader.Column;
			bool bDecimal = ch == '.'? true: false;
			bool bDecimalType=false;	// found d or D in number
			bool bFloat=false;			// found e or E in number
			char cPeek;

			string number = ch.ToString();
			while(!reader.EndOfInput() )
			{
				cPeek = reader.Peek();
				if (Char.IsWhiteSpace(cPeek))
					break;

				if (Char.IsDigit(cPeek))
					number += reader.GetNext();
				else if (cPeek == 'd' || cPeek == 'D' && !bFloat)
				{
					reader.GetNext();				// skip the 'd'
					bDecimalType = true;
					break;
				}
				else if (cPeek == 'e' || cPeek == 'E' && !bFloat)
				{
					number += reader.GetNext();		// add the 'e'
					cPeek = reader.Peek();
					if (cPeek == '-' || cPeek == '+')
					{
						number += reader.GetNext();
						bFloat = true;
						if (Char.IsDigit(reader.Peek()))
							continue;
					}
					throw new ParserException("Invalid number constant.");
				}
				else if (!bDecimal && !bFloat && cPeek == '.')	// can't already be decimal or float
				{
					bDecimal = true;
					number += reader.GetNext();
				}
				else
					break;	// another character
			}

			if (number.CompareTo(".") == 0)
				throw new ParserException("'.' should be followed by a number");

			TokenTypes t;
			if (bDecimalType)
				t = TokenTypes.NUMBER;
			else if (bFloat || bDecimal)
				t = TokenTypes.DOUBLE;
			else
				t = TokenTypes.INTEGER;

			return new Token(number, startLine, startCol, reader.Line, reader.Column, t);
		}

		// Reads an identifier:  
		//  Must consist of letters, digits, "_".  "!", "." are allowed
		//  but have special meaning that is disambiguated later
		private Token ReadIdentifier(char ch)
		{
			int startLine = reader.Line;
			int startCol = reader.Column;
			char cPeek;

			StringBuilder identifier = new StringBuilder(30);	// initial capacity 30 characters
			identifier.Append(ch.ToString());
			while(!reader.EndOfInput() )
			{
				cPeek = reader.Peek();
				if (Char.IsLetterOrDigit(cPeek) || cPeek == '.' || 
					cPeek == '!' || cPeek == '_')
					identifier.Append(reader.GetNext());
				else
					break;
			}

			string key = identifier.ToString().ToLower();
			if (key == "and")
				return new Token(identifier.ToString(), reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.AND);
			else if (key == "or")
				return new Token(identifier.ToString(), reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.OR);
			else if (key == "mod")
				return new Token(ch.ToString(), reader.Line, reader.Column, reader.Line, reader.Column, TokenTypes.MODULUS);

			// normal identifier
			return new Token(identifier.ToString(), startLine, startCol, reader.Line, reader.Column, TokenTypes.IDENTIFIER);
		}

		// Quoted string like " asdf " or ' asdf '
		private Token ReadQuoted(char ch)
		{
			char qChar = ch;
			int startLine = reader.Line;
			int startCol = reader.Column;
			StringBuilder quoted = new StringBuilder();

			while(!reader.EndOfInput())
			{
				ch = reader.GetNext();
				if (ch == '\\')
                {
                    char pChar = reader.Peek();
                    if (pChar == qChar)
                        ch = reader.GetNext();			// got one skip escape char
                    else if (pChar == 'n')
                    {
                        ch = '\n';
                        reader.GetNext();               // skip the character
                    }
                    else if (pChar == 'r')
                    {
                        ch = '\r';
                        reader.GetNext();               // skip the character
                    }
                }
				else if (ch == qChar)
					return new Token(quoted.ToString(), startLine, startCol, reader.Line, reader.Column, TokenTypes.QUOTE);

				quoted.Append(ch);
			}
			throw new ParserException("Unterminated string!");
		}

		// Comment string like /* this is a comment */
		private void ReadComment()
		{
			char ch;

			while(!reader.EndOfInput())
			{
				ch = reader.GetNext();
				if (ch == '*' && reader.Peek() == '/')
				{
					reader.GetNext();			// skip past the '/'
					return;
				}
			}
			throw new ParserException("Unterminated comment!");
		}


//		// Handles case of "<", "<=", and "<! ... xml string  !>
//		private Token ReadXML(char ch)
//		{
//			int startLine = reader.Line;
//			int startCol = reader.Column;
//
//			if (reader.EndOfInput())
//				return  new Token(ch.ToString(), startLine, startCol, startLine, startCol, TokenTypes.LESSTHAN);
//			ch = reader.GetNext();
//			if (ch == '=')
//				return  new Token("<=", startLine, startCol, reader.Line, reader.Column, TokenTypes.LESSTHANOREQUAL);
//			if (ch != '!')					// If it's not '!' then it's not XML
//			{
//				reader.UnGet();				// put back the character
//				return  new Token("<", startLine, startCol, reader.Line, reader.Column, TokenTypes.LESSTHAN);
//			}
//
//			string xml = "";				// intialize our string
//
//			while(!reader.EndOfInput())
//			{
//				ch = reader.GetNext();
//
//				if(ch == '!')				// check for end of XML denoted by "!>"
//				{
//					if (!reader.EndOfInput() && reader.Peek() == '>')
//					{
//						reader.GetNext();	// pull the '>' off the input
//						return new Token(xml, startLine, startCol, reader.Line, reader.Column, TokenTypes.XML);
//					}
//				}
//
//				xml += ch.ToString();
//			}
//			throw new ParserException("Unterminated XML clause!");
//		}

	}
}
