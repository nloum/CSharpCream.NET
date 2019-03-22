using System;
using System.Collections;

namespace  Cream
{
	
	public class TabooSearch:LocalSearch
	{
		public int TabooLength = 16;
		protected internal Operation[] taboo;
		protected internal int tabooI;
		
		public TabooSearch(Network network):this(network, Default, null)
		{
		}
		
		public TabooSearch(Network network, int option):this(network, option, null)
		{
		}
		
		public TabooSearch(Network network, String name):this(network, Default, name)
		{
		}
		
		public TabooSearch(Network network, int option, String name):base(network, option, name)
		{
			ExchangeRate = 0.8;
		}
		
		protected internal virtual void  ClearTaboo()
		{
			taboo = new Operation[TabooLength];
			for (int i = 0; i < taboo.Length; i++)
			{
				taboo[i] = null;
			}
			tabooI = 0;
		}
		
		protected internal virtual bool IsTaboo(Operation op, Operation[] localTaboo)
		{
			if (localTaboo == null)
				return false;
			for (int i = 0; i < localTaboo.Length; i++)
			{
				if (localTaboo[i] != null && op.IsTaboo(localTaboo[i]))
					return true;
			}
			return false;
		}
		
		protected internal virtual void  AddTaboo(Operation op)
		{
			taboo[tabooI] = op;
			tabooI = (tabooI + 1) % taboo.Length;
		}
		
		protected internal override void  StartSearch()
		{
			ClearTaboo();
			var thisSolverStrategy = SolverStrategy;
			solver = new DefaultSolver(network, option);
			SolverStrategy = thisSolverStrategy;
			solution = solver.FindFirst();
		}
		
		protected internal override void  NextSearch()
		{
			Operation locallyBestOp = null;
			Solution locallyBestSol = null;
			int locallyBest = IntDomain.MaxValue;
			solution = GetCandidate();
			Code code = solution.Code;
			while (!Aborted)
			{
				IEnumerator ops = code.Operations().GetEnumerator();
				while (ops.MoveNext() && !Aborted)
				{
					var op = (Operation) ops.Current;
                    if (IsTaboo(op, taboo))
                    {
                        continue;
                    }
				    code.To = network;
					op.ApplyTo(network);
					Solution sol = solver.FindBest(IterTimeout);
                    if (sol == null)
                    {
                        continue;
                    }
				    int objectiveIntValue = sol.ObjectiveIntValue;
                    if (!IsBetter(objectiveIntValue, locallyBest))
                    {
                        continue;
                    }
				    locallyBest = objectiveIntValue;
					locallyBestOp = op;
					locallyBestSol = sol;
				}
                if (locallyBestOp != null)
                {
                    break;
                }
			    ClearTaboo();
			}
			code.To = network;
		    if (locallyBestOp != null)
		    {
		        locallyBestOp.ApplyTo(network);
		    }
            
		    solution = locallyBestSol;
			AddTaboo(locallyBestOp);
		}
	}
}