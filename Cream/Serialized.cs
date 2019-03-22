using System;

namespace  Cream
{
	
	public class Serialized:Constraint
	{
		private Variable[] v;
		private int[] l;
		private int[] order;

        public Serialized(Network net, Variable[] v, int[] l)
            : this(net, v,l, ConstraintTypes.Hard)
        {
        }

        public Serialized(Network net, Variable[] v, int[] l, ConstraintTypes cType)
            : this(net, v, l, cType, 0)
        {
        }

        public Serialized(Network net, Variable[] v, int[] l, ConstraintTypes cType, int weight)
            : base(net, cType, weight)
        {
            this.v = new Variable[v.Length];
            v.CopyTo(this.v, 0);
            this.l = new int[l.Length];
            l.CopyTo(this.l, 0);
            order = null;
        }

        protected internal override Constraint Copy(Network net)
		{
			return new Serialized(net, Copy(v, net), l);
		}
		
		protected internal class SerializedCondition:Condition
		{
			private class AnonymousClassComparator : System.Collections.IComparer
			{
				public AnonymousClassComparator(SerializedCondition enclosingInstance)
				{
					InitBlock(enclosingInstance);
				}
				private void  InitBlock(SerializedCondition enclosingInstance)
				{
					EnclosingInstance = enclosingInstance;
				}

			    public SerializedCondition EnclosingInstance { get; set; }

			    public virtual int Compare(Object o1, Object o2)
				{
					int k1 = ((int[]) o1)[1];
					int k2 = ((int[]) o2)[1];
					return (k1 < k2)?- 1:((k1 == k2)?0:1);
				}
			}
			private void  InitBlock(Serialized enclosingInstance)
			{
				EnclosingInstance = enclosingInstance;
			}

		    override public Network To
			{
				set
				{
					var s = (Serialized) value.GetConstraint(index);
					if (code == null)
					{
						s.order = null;
					}
					else
					{
						s.order = new int[code.Length];
						for (int i = 0; i < s.order.Length; i++)
						{
							s.order[i] = code[i][0];
						}
					}
				}
				
			}

		    public Serialized EnclosingInstance { get; set; }

		    private int[][] code;
			
			public SerializedCondition(Serialized enclosingInstance)
			{
				InitBlock(enclosingInstance);
				index = EnclosingInstance.Index;
				code = new int[EnclosingInstance.v.Length][];
				for (int i = 0; i < EnclosingInstance.v.Length; i++)
				{
					code[i] = new int[3];
				}
				for (int i = 0; i < code.Length; i++)
				{
					Domain d = EnclosingInstance.v[i].Domain;
					code[i][0] = i;
					code[i][1] = ((IntDomain) d).Value();
					code[i][2] = EnclosingInstance.l[i];
				}
				System.Collections.IComparer comp = new AnonymousClassComparator(this);
				Array.Sort(code, comp);
			}
			
			public override System.Collections.IList Operations()
			{
				System.Collections.IList operations = new System.Collections.ArrayList();
				for (int i = 0; i < code.Length - 1; i++)
				{
					if (code[i][1] + code[i][2] == code[i + 1][1])
					{
						// adjacent
						Operation op = new Swap(EnclosingInstance, index, i, i + 1);
						operations.Add(op);
					}
				}
				return operations;
			}
		}
		
		private class Swap:Operation
		{
			private void  InitBlock(Serialized enclosingInstance)
			{
				EnclosingInstance = enclosingInstance;
			}

		    public Serialized EnclosingInstance { get; set; }

		    new private int index;
			private int i;
			private int j;
			
			public Swap(Serialized enclosingInstance, int index, int i, int j)
			{
				InitBlock(enclosingInstance);
				this.index = index;
				this.i = i;
				this.j = j;
			}
			
			public override void  ApplyTo(Network network)
			{
				var s = (Serialized) network.GetConstraint(index);
				int t = s.order[i]; s.order[i] = s.order[j]; s.order[j] = t;
			}
			
			public override bool IsTaboo(Operation op)
			{
				if (!(op is Swap))
					return false;
				var swap = (Swap) op;
				return index == swap.index && i == swap.i && j == swap.j;
			}
		}
		
		protected internal override void  ClearCondition()
		{
			order = null;
		}
		
		protected internal override Condition ExtractCondition()
		{
			return new SerializedCondition(this);
		}
		
		protected internal override bool IsModified()
		{
			return IsModified(v);
		}
		
		private bool SatisfySequential(Trail trail)
		{
			if (order == null)
				return true;
			for (int k = 0; k < order.Length - 1; k++)
			{
				int i = order[k];
				int j = order[k + 1];
				var d0 = (IntDomain) v[i].Domain;
				var d1 = (IntDomain) v[j].Domain;
				int diffMin = d1.Maximum() - l[i] + 1;
				int diffMax = d0.Minimum() + l[i] - 1;
				d0 = d0.Delete(diffMin, IntDomain.MaxValue);
				if (d0.Empty)
					return false;
				d1 = d1.Delete(IntDomain.MinValue, diffMax);
				if (d1.Empty)
					return false;
                if (trail != null)
                {
                    v[i].UpdateDomain(d0, trail);
                    v[j].UpdateDomain(d1, trail);
                }
			}
			return true;
		}
		
		private bool SatisfySerialized(Trail trail)
		{
			for (int i = 0; i < v.Length; i++)
			{
				for (int j = 0; j < v.Length; j++)
				{
					if (i == j)
						continue;
					var d0 = (IntDomain) v[i].Domain;
					var d1 = (IntDomain) v[j].Domain;
					int diffMin = d1.Maximum() - l[i] + 1;
					int diffMax = d1.Minimum() + l[j] - 1;
					if (diffMin <= diffMax)
					{
						d0 = d0.Delete(diffMin, diffMax);
						if (d0.Empty)
							return false;
                        if (trail != null)
                        {
                            v[i].UpdateDomain(d0, trail);
                        }
					}
				}
			}
			return true;
		}

        protected internal override bool IsSatisfied()
        {
            return Satisfy(null);
        }
        protected internal override bool Satisfy(Trail trail)
		{
			if (!SatisfySequential(trail))
				return false;
			return SatisfySerialized(trail);
		}
		
		public override String ToString()
		{
			return "Serialized(" + ToString(v) + "," + ToString(l) + ")";
		}
	}
}