using System;

namespace  Cream
{
	
	public class SimulatedAnneallingSearch:LocalSearch
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
		
		public SimulatedAnneallingSearch(Network network):this(network, Default, null)
		{
		}
		
		public SimulatedAnneallingSearch(Network network, int option):this(network, option, null)
		{
		}
		
		public SimulatedAnneallingSearch(Network network, String name):this(network, Default, name)
		{
		}
		
		public SimulatedAnneallingSearch(Network network, int option, String name):base(network, option, name)
		{
			ExchangeRate = 0.05;
		}
		
		protected internal override void  StartSearch()
		{
			base.StartSearch();
			temperature = solution.ObjectiveIntValue / 10.0;
		}
		
		protected internal override void  NextSearch()
		{
			if (totalTimeout > 0)
			{
				long elapsedTime = Math.Max(1, (DateTime.Now.Ticks - 621355968000000000) / 10000 - startTime);
				double iterationRate = (double) iteration / elapsedTime;
				var expectedIteration = (int) (iterationRate * (totalTimeout - elapsedTime));
				if (expectedIteration > 0)
				{
					gamma = Math.Exp(Math.Log(1.0 / temperature) / expectedIteration);
					gamma = Math.Min(0.9999, gamma);
				}
			}
			temperature *= gamma;
			solution = GetCandidate();
			int objectiveIntValue = solution.ObjectiveIntValue;
			Code code = solution.Code;
			System.Collections.IList operations = code.Operations();
			while (operations.Count > 0)
			{
				var i = (int) (operations.Count * SupportClass.Random.NextDouble());
			    object tempObject = operations[i];
				operations.RemoveAt(i);
				var op = (Operation) tempObject;
				code.To = network;
				op.ApplyTo(network);
				Solution sol = solver.FindBest(IterTimeout);
				if (sol == null)
					continue;
				double delta = sol.ObjectiveIntValue - objectiveIntValue;
				if (IsOption(Maximize))
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