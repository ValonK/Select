using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Select.Extensions
{
	public static class StringExtensions
	{
		public static bool HasLineBreaks(this string expression)
		{
			return expression.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Length > 1;
		}
	}
}
