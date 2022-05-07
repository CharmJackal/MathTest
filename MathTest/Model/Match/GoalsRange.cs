using MathTest.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MathTest.Model.Match
{
	internal class GoalsRange
	{
		public int Start;

		public int End;

		//[X .. Y]
		public GoalsRange(string goalsRange)
		{
			var regex = new Regex(@"\[(\d+) .. (\d+)\]");
			var res = regex.Match(goalsRange);

			if (res.Success)
			{
				Start = int.Parse(res.Groups[1].Value);
				End = int.Parse(res.Groups[2].Value);

				if(Start >= End
					|| Start <= 0
					|| End <= 0)
				{
					throw new ArgumentException("Wrong argument of goals interval");
				}
				
			}
			else
			{
				throw new ArgumentException("Wrong argument of goals interval");
			}
		}

		public List<List<GoalType>> GetGoalsQueryToProcess(List<GoalType> currentGoalsQuery)
		{
			if(currentGoalsQuery.Count >= End)
			{
				return null;
			}

			int localStart = Start > currentGoalsQuery.Count
				? currentGoalsQuery.Count
				: Start;

			return CombineGoalsFromN(End - localStart);
			//if start > currentGoalsQuery.Count

		}

		private static List<List<GoalType>> CombineGoalsFromN(int n)
		{
			if(n == 0) {
				return null;
			}
			var result = new List<List<GoalType>>
			{
				new List<GoalType>{GoalType.Home},
				new List<GoalType>{GoalType.Visit},
				new List<GoalType>()
			};

			for(int i = 1; i < n; i++)
			{
				var buflist = new List<List<GoalType>>();
				foreach (var q in result)
				{
					if(q.Count < i)
					{
						continue;
					}
					var toAddV = new List<GoalType>(q);
					var toAddH = new List<GoalType>(q);

					toAddV.Add(GoalType.Visit);
					toAddH.Add(GoalType.Home);

					buflist.Add(toAddH);
					buflist.Add(toAddV);
				}
				result.AddRange(buflist);
			}

			return result;
		}
	}
}
