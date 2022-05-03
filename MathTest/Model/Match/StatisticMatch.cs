using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTest.Model.Match
{
	internal class StatisticMatch : Match
	{
		public double ProbabilityPercentage = 0;

		//3:1 3,31%
		public StatisticMatch(string fromFile) : base(fromFile.Split(' ')[0])
		{
			var probability = fromFile.Split(' ')[1];
			ProbabilityPercentage = double.Parse(probability.Replace("%", string.Empty));
		}

		public StatisticMatch(int homeGoals, int visitGoals) : base(homeGoals,visitGoals)
		{
		}
	}
}
