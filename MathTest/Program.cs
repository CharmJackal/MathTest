using MathTest.Model.Match;
using MathTest.Model.MathModel;


namespace MathTest
{
	class Program
	{
		
		static void Main(string[] args)
		{
			var mathModel = new MathModel();

			var res = mathModel.CalculateValue(new RealMatch(RealMatch.ParseResultFromStr("Х"), 90), new GoalsRange("[2 .. 5]"));
		}

		
	}
}
