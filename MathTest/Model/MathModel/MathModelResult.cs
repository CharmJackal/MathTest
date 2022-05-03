using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTest.Model.MathModel
{
	public class MathModelResult
	{
		public double HomeWinProbability = 0;

		public double VisitWinProbability = 0;

		public double DrawProbability = 0;

		public double NotFinished = 0;

		public void NormolizeValue()
		{
			var coef = GetPercentCoeff();
			HomeWinProbability *= coef;
			VisitWinProbability *= coef;
			DrawProbability *= coef;
			NotFinished *= coef;
		}
		
		public double GetPercentCoeff()
		{
			var sum = HomeWinProbability + VisitWinProbability + DrawProbability + NotFinished;
			return 1 / sum;
		}
	}
}
