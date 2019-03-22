using System;

namespace  Cream
{
	
	public class IBBSearch:LocalSearch
	{
		virtual public long IterationTimeout
		{
			set
			{
				iterationTimeout = value;
			}
			
		}
		virtual public double ClearRate
		{
			set
			{
				clearRate = value;
			}
			
		}
		private double clearRate = 0.8;
		
		public IBBSearch(Network network):this(network, DEFAULT, null)
		{
		}
		
		public IBBSearch(Network network, int option):this(network, option, null)
		{
		}
		
		public IBBSearch(Network network, String name):this(network, DEFAULT, name)
		{
		}
		
		public IBBSearch(Network network, int option, String name):base(network, option, name)
		{
			ExchangeRate = 0.8;
		}
		
		protected internal virtual void  bbSearch()
		{
			if (Aborted)
				return ;
			for (solver.start(iterationTimeout); solver.waitNext(); solver.resume())
			{
				solution = solver.Solution;
				success();
				if (Aborted)
					break;
			}
			solver.stop();
			solution = solver.BestSolution;
		}
		
		protected internal override void  startSearch()
		{
			solver = new DefaultSolver(network, option);
			//solution = solver.findFirst();
			//solution = solver.findBest(iterationTimeout);
			bbSearch();
		}
		
		protected internal override void  nextSearch()
		{
			if (Aborted)
				return ;
			solution = candidate();
			Code code = solution.Code;
			code = (Code) code.Clone();
			Condition[] conditions = code.conditions;
			for (int i = 0; i < conditions.Length; i++)
			{
				if (SupportClass.Random.NextDouble() < clearRate)
				{
					conditions[i] = null;
				}
			}
			code.To = network;
			bbSearch();
		}
	}
}