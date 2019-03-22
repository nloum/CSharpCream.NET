using System;

namespace  Cream
{
	
	public class IntComparison:Constraint
	{
		public const int Le = 0;
		public const int Lt = 1;
		public const int Ge = 2;
		public const int Gt = 3;
		private int comparison;
		private Variable[] v;

        public IntComparison(Network net, int comp, Variable v0, Variable v1)
            : this(net, comp, new[] { v0, v1 })
        {
        }

        public IntComparison(Network net, int comp, Variable v0, Variable v1, ConstraintTypes cType)
            : this(net, comp, new[] { v0, v1 }, cType)
        {
        }

        public IntComparison(Network net, int comp, Variable v0, Variable v1, ConstraintTypes cType, int weight)
            : this(net, comp, new[] { v0, v1 }, cType, weight)
        {
        }

        public IntComparison(Network net, int comp, Variable v0, int x1)
            : this(net, comp, v0, new IntVariable(net, x1))
        {
        }

        public IntComparison(Network net, int comp, Variable v0, int x1, ConstraintTypes cType)
            : this(net, comp, v0, new IntVariable(net, x1), cType)
        {
        }

        public IntComparison(Network net, int comp, Variable v0, int x1, ConstraintTypes cType, int weight)
            : this(net, comp, v0, new IntVariable(net, x1), cType, weight)
        {
        }

        public IntComparison(Network net, int comp, int x0, Variable v1)
            : this(net, comp, new IntVariable(net, x0), v1)
        {
        }

        public IntComparison(Network net, int comp, int x0, Variable v1, ConstraintTypes cType)
            : this(net, comp, new IntVariable(net, x0), v1, cType)
        {
        }

        public IntComparison(Network net, int comp, int x0, Variable v1, ConstraintTypes cType, int weight)
            : this(net, comp, new IntVariable(net, x0), v1, cType, weight)
        {
        }

        private IntComparison(Network net, int comp, Variable[] v)
            : this(net, comp, v, ConstraintTypes.Hard)
        {
        }

        private IntComparison(Network net, int comp, Variable[] v, ConstraintTypes cType)
            : this(net, comp, v, cType, 0)
        {
        }

        private IntComparison(Network net, int comp, Variable[] v, ConstraintTypes cType, int weight)
            : base(net, cType, weight)
        {
            comparison = comp;
            this.v = v;
        }

        public Variable[] Vars
        {
            get
            {
                return v;
            }
        }
        
        protected internal override Constraint Copy(Network net)
		{
			return new IntComparison(net, comparison, Copy(v, net));
		}
		
		protected internal override bool IsModified()
		{
			return IsModified(v);
		}
		
        private static bool SatisfyLE(Variable v0, Variable v1, Trail trail)
		{
			var d0 = (IntDomain) v0.Domain;
			var d1 = (IntDomain) v1.Domain;
			d0 = d0.CapInterval(IntDomain.MinValue, d1.Maximum());
			if (d0.Empty)
				return false;
            if (trail != null)
            {
                v0.UpdateDomain(d0, trail);
            }
            d1 = d1.CapInterval(d0.Minimum(), IntDomain.MaxValue);
			if (d1.Empty)
				return false;
            if (trail != null)
            {
                v1.UpdateDomain(d1, trail);
            }
            return true;
		}
		
		private static bool SatisfyLT(Variable v0, Variable v1, Trail trail)
		{
			var d0 = (IntDomain) v0.Domain;
			var d1 = (IntDomain) v1.Domain;
			d0 = d0.CapInterval(IntDomain.MinValue, d1.Maximum() - 1);
			if (d0.Empty)
				return false;
            if (trail != null)
            {
                v0.UpdateDomain(d0, trail);
            }
		    d1 = d1.CapInterval(d0.Minimum() + 1, IntDomain.MaxValue);
			if (d1.Empty)
				return false;
            if (trail != null)
            {
                v1.UpdateDomain(d1, trail);
            }
		    return true;
		}

        protected internal override bool IsSatisfied()
        {
            return Satisfy(null);
        }
		protected internal override bool Satisfy(Trail trail)
		{
			switch (comparison)
			{
				
				case Le: 
					return SatisfyLE(v[0], v[1], trail);
				
				case Lt: 
					return SatisfyLT(v[0], v[1], trail);
				
				case Ge: 
					return SatisfyLE(v[1], v[0], trail);
				
				case Gt: 
					return SatisfyLT(v[1], v[0], trail);
				}
			return false;
		}
		
		public override String ToString()
		{
			String c = "";
			switch (comparison)
			{
				
				case Le: 
					c = "Le"; break;
				
				case Lt: 
					c = "Lt"; break;
				
				case Ge: 
					c = "Ge"; break;
				
				case Gt: 
					c = "Gt"; break;
				}
			return "IntComparison(" + c + "," + ToString(v) + ")";
		}
	}
}