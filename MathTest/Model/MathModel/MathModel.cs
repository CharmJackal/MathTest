using MathTest.Enums;
using MathTest.Model.Match;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MathTest.Model.MathModel
{
	internal class MathModel
	{
		private const string defaultFileWithStatPath = @"..\..\..\Data\ModelStat.txt";

		public Dictionary<int, double> HomeGoalsProbability;

		public Dictionary<int, double> VisitGoalsProbability;

		public MathModel()
		{
			InitializeProbability();
			Console.WriteLine("Done");
		}

		public MathModelResult CalculateValue(RealMatch match, GoalsRange range)
		{
			var matchResultToProcess = range.GetGoalsQueryToProcess(match.GoalsQuery);

			var result = new MathModelResult();

			var matchToNormolize = new List<RealMatch>();

			foreach(var matchPart in matchResultToProcess)
			{
				var processedMatch = ProcessMatchPredicate(matchPart, match);

				matchToNormolize.Add(processedMatch);
				// Console.WriteLine(processedMatch.ShowGoalsQuery() + " с вероятностью " + processedMatch.ProbabilityPercentage * 100 + "%");

				switch (processedMatch.ResultOnInterval(range))
				{
					case Results.HomeWin:
						result.HomeWinProbability += processedMatch.ProbabilityPercentage;
						break;
					case Results.VisitWin:
						result.VisitWinProbability += processedMatch.ProbabilityPercentage;
						break;
					case Results.Draw:
						result.DrawProbability += processedMatch.ProbabilityPercentage;
						break;
					case Results.NotFinished:
						result.NotFinished += processedMatch.ProbabilityPercentage;
						break;
				} 
				
			}
			
			return NormolizeValue(result, matchToNormolize, range);
		}

		//TODO костыльное решенеие для ультимитвного метода
		private MathModelResult NormolizeValue(MathModelResult result, List<RealMatch> matches, GoalsRange range)
		{
			var coef = result.GetPercentCoeff();

			result.NormolizeValue();

			foreach(var match in matches)
			{
				match.ProbabilityPercentage *= coef;
				Console.WriteLine(match.ShowGoalsQuery() + " с вероятностью " + Math.Round(match.ProbabilityPercentage * 100, 2) + "% Статус на интервале " + match.ResultOnInterval(range));
			}
			Console.WriteLine($"Вероятности на интервали: \n" +
				$"Победа хозяев {Math.Round(result.HomeWinProbability * 100, 2)} \n" +
				$"Победа гостей {Math.Round(result.VisitWinProbability * 100, 2)} \n" +
				$"Ничья {Math.Round(result.DrawProbability * 100, 2)} \n" +
				$"Интервал не завершен {Math.Round(result.NotFinished * 100, 2)}");

			return result;
		}

		public RealMatch ProcessMatchPredicate(List<GoalType> goalsQueryToPredicate, RealMatch match)
		{
			var homeGoalsCount = goalsQueryToPredicate.Count(g => g == GoalType.Home);
			var visitGoalsCount = goalsQueryToPredicate.Count(g => g == GoalType.Visit);

			var homeGoalsProbability = ProbabilityCalculate(HomeGoalsProbability, homeGoalsCount, match.MinutesToEnd);
			var visitGoalsProbability = ProbabilityCalculate(VisitGoalsProbability, visitGoalsCount, match.MinutesToEnd);

			var resGoalsQuery = new List<GoalType>(match.GoalsQuery);
			resGoalsQuery.AddRange(goalsQueryToPredicate);


			return new RealMatch(resGoalsQuery, 0) {
				ProbabilityPercentage = (homeGoalsProbability * visitGoalsProbability)
			};
		}

		private double ProbabilityCalculate(Dictionary<int, double> probabilityTable, int goalsCount, int minutesToEnd)
		{
			var goalProbability = minutesToEnd / 90.0;
			return probabilityTable
				.Where(k => k.Key >= goalsCount)
				.Sum(v => v.Value * (Math.Pow(goalProbability, v.Key)));
				
			// .Where(k => k.Key == goalsCount)
				//.Select(v => v.Value)
				// .First();
		}

		private void InitializeProbability()
		{
			var parsedStat = ParseStatFromFile();

			HomeGoalsProbability = new Dictionary<int, double>();
			VisitGoalsProbability = new Dictionary<int, double>();

			foreach(var match in parsedStat)
			{
				match.ProbabilityPercentage /= 100;
				HomeGoalsProbability[match.HomeGoals] = HomeGoalsProbability.ContainsKey(match.HomeGoals)
					? HomeGoalsProbability[match.HomeGoals] + match.ProbabilityPercentage
					: match.ProbabilityPercentage;
				VisitGoalsProbability[match.VisitGoals] = VisitGoalsProbability.ContainsKey(match.VisitGoals)
					? VisitGoalsProbability[match.VisitGoals] + match.ProbabilityPercentage
					: match.ProbabilityPercentage;
			}
		}

		private static IList<StatisticMatch> ParseStatFromFile()
		{
			var result = new List<StatisticMatch>();
			
			foreach (string line in File.ReadLines(Path.Combine(Directory.GetCurrentDirectory(), defaultFileWithStatPath)))
			{
				result.Add(new StatisticMatch(line));
			}

			return result;
		}
	}
}
