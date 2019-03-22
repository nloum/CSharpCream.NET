using System;
using  Cream;
namespace  Cream
{
	
	public class SASearch:LocalSearch
	{
		virtual public double Temperature
		{
			get
			{
				lock (this)
				{
					return temperature;
				}
			}
			
			set
			{
				lock (this)
				{
					temperature = value;
				}
			}
			
		}
		private double gamma = 0.999;
		private double temperature = 100.0;
		
		public SASearch(Network network):this(network, DEFAULT, null)
		{
		}
		
		public SASearch(Network network, int option):this(network, option, null)
		{
		}
		
		public SASearch(Network network, String name):this(network, DEFAULT, name)
		{
		}
		
		public SASearch(Network network, int option, String name):base(network, option, name)
		{
			ExchangeRate = 0.05;
		}
		
		protected internal override void  startSearch()
		{
			base.startSearch();
			temperature = solution.ObjectiveIntValue / 10.0;
		}
		
		protected internal override void  nextSearch()
		{
			if (totalTimeout > 0)
			{
				long elapsedTime = Math.Max(1, (DateTime.Now.Ticks - 621355968000000000) / 10000 - startTime);
				double iterationRate = (double) iteration / elapsedTime;
				int expectedIteration = (int) (iterationRate * (totalTimeout - elapsedTime));
				if (expectedIteration > 0)
				{
					gamma = Math.Exp(Math.Log(1.0 / temperature) / expectedIteration);
					gamma = Math.Min(0.9999, gamma);
				}
			}
			temperature *= gamma;
			solution = candidate();
			int value_ = solution.ObjectiveIntValue;
			Code code = solution.Code;
			System.Collections.IList operations = code.operations();
			while (operations.Count > 0)
			{
				int i = (int) (operations.Count * SupportClass.Random.NextDouble());
				Object tempObject;
				tempObject = operations[i];
				operations.RemoveAt(i);
				Operation op = (Operation) tempObject;
				code.To = network;
				op.applyTo(network);
				Solution sol = solver.findBest(iterationTimeout);
				if (sol == null)
					continue;
				double delta = sol.ObjectiveIntValue - value_;
				if (isOption(MAXIMIZE))
				{
					delta = - delta;
				}
				if (delta < 0)
				{
					solution = sol;
					return ;
				}
				double p = Math.Exp((- delta) / temperature);
				if (p < SupportClass.Random.NextDouble())
					continue;
				solution = sol;
				return ;
			}
			solution = null;
		}
	}
}