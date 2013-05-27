using System;
using System.Collections.Generic;
using System.Text;

namespace Earlz.MonoSump.Core
{
	public static class ToJsonExtension
	{
		public static string ToJson(this IList<bool[]> data)
		{
			var sb=new StringBuilder();
			sb.Append("{");
			sb.Append("\"channels\": [");
			for(int i=0;i<data.Count;i++)
			{
				sb.Append("[");
				for(int j=0;j<data[i].Length;j++)
				{
					sb.Append(BoolToData(data[i][j]));
					sb.Append(",");
				}
				sb.Remove(sb.Length-1, 1);
				sb.Append("],");
			}
			sb.Remove(sb.Length-1, 1);
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
		static string BoolToData(bool data)
		{
			if(data)
			{
				return "1";
			}
			return "0";
		}
	}
}

