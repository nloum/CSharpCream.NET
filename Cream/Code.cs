using System;

namespace  Cream
{
	
	public class Code : ICloneable
	{
		virtual public Network To
		{
			set
			{
				for (int i = 0; i < Conditions.Length; i++)
				{
					if (Conditions[i] == null)
					{
						value.GetConstraint(i).ClearCondition();
					}
					else
					{
						Conditions[i].To = value;
					}
				}
			}
			
		}
		public Condition[] Conditions;
		
		private Code()
		{
		}
		
		public Code(Network network)
		{
			System.Collections.IList constraints = network.Constraints;
			Conditions = new Condition[constraints.Count];
			for (int i = 0; i < Conditions.Length; i++)
			{
				Constraint c = network.GetConstraint(i);
				Conditions[i] = c.ExtractCondition();
			}
		}
		
		public virtual Object Clone()
		{
			var code = new Code {Conditions = new Condition[Conditions.Length]};
		    Conditions.CopyTo(code.Conditions, 0);
			return code;
		}
		
		public virtual System.Collections.IList Operations()
		{
			System.Collections.IList operations = new System.Collections.ArrayList();
			for (int i = 0; i < Conditions.Length; i++)
			{
				if (Conditions[i] != null)
				{
					SupportClass.CollectionSupport.AddAll(operations, Conditions[i].Operations());
				}
			}
			return operations;
		}
	}
}