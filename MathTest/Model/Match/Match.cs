using MathTest.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTest.Model.Match
{
	internal abstract class Match
	{
		public int HomeGoals;

		public int VisitGoals;

		public Results Result;

		public Match(string matchRes)
		{
			var res = matchRes.Split(":");
			SetupProperties(int.Parse(res[0]), int.Parse(res[1]));
		}

		public Match(int homeGoals, int visitGoals)
		{
			SetupProperties(homeGoals, visitGoals);
		}

		private void SetupProperties(int homeGoals, int visitGoals)
		{
			if (homeGoals < 0 || visitGoals < 0)
			{
				throw new ArgumentException("Goals count cant be less than 0");
			}
			HomeGoals = homeGoals;
			VisitGoals = visitGoals;

			Result = GetMatchResult(homeGoals, visitGoals);
		}

		protected Results GetMatchResult(int homeGoals, int visitGoals)
		{
			return homeGoals == visitGoals
				? Results.Draw
				: homeGoals > visitGoals
				? Results.HomeWin
				: Results.VisitWin;
		}
	}
}
