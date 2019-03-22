using System;

namespace  Cream
{
    public class Sequential:Constraint
    {
        private Variable[] v;
        private int[] l;

        public Sequential(Network net, Variable[] v, int[] l)
            : this(net, v, l, ConstraintTypes.Hard)
        {
        }

        public Sequential(Network net, Variable[] v, int[] l, ConstraintTypes cType)
            : this(net, v, l, cType, 0)
        {
        }

        public Sequential(Network net, Variable[] v, int[] l, ConstraintTypes cType, int weight)
            : base(net, cType, weight)
        {
            this.v = new Variable[v.Length];
            v.CopyTo(this.v, 0);
            this.l = new int[l.Length];
            l.CopyTo(this.l, 0);
        }

        protected internal override Constraint Copy(Network net)
        {
            return new Sequential(net, Copy(v, net), l);
        }
		
        protected internal override bool IsModified()
        {
            return IsModified(v);
        }

        protected internal override bool IsSatisfied()
        {
            return Satisfy(null);
        }
        protected internal override bool Satisfy(Trail trail)
        {
            for (int i = 0; i < v.Length - 1; i++)
            {
                int j = i + 1;
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
		
        public override String ToString()
        {
            return "Sequential(" + v + "," + ToString(l) + ")";
        }
    }
}