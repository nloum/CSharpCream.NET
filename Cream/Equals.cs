using System;

namespace  Cream
{
    public class Equals:Constraint
    {
        private Variable[] v;
		
        public Equals(Network net, Variable v0, Variable v1):this(net, new[]{v0, v1})
        {
        }

        public Equals(Network net, Variable v0, Variable v1, ConstraintTypes cType)
            : this(net, new[] { v0, v1 }, cType)
        {
        }

        public Equals(Network net, Variable v0, Variable v1, ConstraintTypes cType, int weight)
            : this(net, new[] { v0, v1 }, cType, weight)
        {
        }

        public Equals(Network net, Variable[] v)
            : this(net, v, ConstraintTypes.Hard)
        {
        }

        public Equals(Network net, Variable[] v, ConstraintTypes cType)
            : this(net, v, cType, 0)
        {
        }
        
        public Equals(Network net, Variable[] v, ConstraintTypes cType, int weight)
            : base(net, cType, weight)
        {
            this.v = new Variable[v.Length];
            v.CopyTo(this.v, 0);
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
            return new Equals(net, Copy(v, net));
        }
		
        protected internal override bool IsModified()
        {
            return IsModified(v);
        }

        protected internal override bool IsSatisfied()
        {
            return Satisfy(null);
        }

        //public int getMostSoftSatisfiedDomain()
        //{
        //    Domain d = v[0].Domain;
        //    if (CType == ConstraintTypes.Soft)
        //    {
        //        for (int i = 1; i < v.Length; i++)
        //        {
        //            d = d.Cap(v[i].Domain);
        //            if (d.EmptyDomain)
        //                return -1;
        //        }
        //    }
        //    return ((IntDomain) d).Min();
        //}
        
        protected internal override bool Satisfy(Trail trail)
        {
            Domain d = v[0].Domain;
            for (int i = 1; i < v.Length; i++)
            {
                d = d.Cap(v[i].Domain);
                if (d.Empty)
                    return false;
            }
            if (trail != null)
            {
                for (int i = 0; i < v.Length; i++)
                {
                    v[i].UpdateDomain(d, trail);
                }
            }
            return true;
        }
		
        public override String ToString()
        {
            return "Equals(" + ToString(v) + " "+CType+")";
        }
    }
}