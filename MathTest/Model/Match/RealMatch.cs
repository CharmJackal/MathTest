using MathTest.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTest.Model.Match
{
	class RealMatch : StatisticMatch
	{
		public int MinutesToEnd;

		public List<GoalType> GoalsQuery;

		public RealMatch(List<GoalType> goalsQuery, int minuntesToEnd) 
			: base(goalsQuery.Count( g => g == GoalType.Home),
				  goalsQuery.Count(g => g == GoalType.Visit))
		{
			if(minuntesToEnd < 0 || minuntesToEnd >= 90)
			{
				throw new ArgumentException("Incorrect time to match end");
			}

			GoalsQuery = new List<GoalType>(goalsQuery);
			MinutesToEnd = minuntesToEnd;
		}

		public static List<GoalType> ParseResultFromStr(string goalsQuery)
		{
			var result = new List<GoalType>();

			if (string.IsNullOrWhiteSpace(goalsQuery))
			{
				return result;
			}
			var convertedQuery = ConvertQueryForEnumValues(goalsQuery);

			foreach (var goal in convertedQuery.Split(','))
			{
				if (Enum.TryParse<GoalType>(goal, out var goalType))
				{
					result.Add(goalType);
				}
				else
				{
					throw new ArgumentException("Incorrect query of goals");
				}
			}

			return result;
		}

		public Results ResultOnInterval(GoalsRange goalsRange)
		{
			if (GoalsQuery.Count < goalsRange.End)
			{
				return Results.NotFinished;
			}
			var res = GoalsQuery.GetRange(goalsRange.Start-1, goalsRange.End - goalsRange.Start + 1);
			return GetMatchResult(res.Count(m => m == GoalType.Home), res.Count(m => m == GoalType.Visit));
		}

		public string ShowGoalsQuery()
		{
			var str = string.Join(",", GoalsQuery);

			return ConvertQueryFromEnumValues(str);
		}

		private static string ConvertQueryForEnumValues(string goalsQuery)
		{
			return goalsQuery?.Replace("Х", GoalType.Home.ToString()).Replace("Г", GoalType.Visit.ToString());
		}

		private static string ConvertQueryFromEnumValues(string goalsQuery)
		{
			return goalsQuery?.Replace(GoalType.Home.ToString(),"Х").Replace(GoalType.Visit.ToString(), "Г");
		}
	}
}
