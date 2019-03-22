/*
* Relation.java
*/
using System;
namespace Cream
{
	
	/// <summary> Relation constraints.
	/// Possible combinations of two integer variables can be
	/// defined by a two-dimentional array of boolean values.
	/// 
	/// </summary>
	/// <since> 1.4
	/// </since>
	/// <version>  1.4
	/// </version>
    /// <author>  Original java solver: Naoyuki Tamura (tamura@kobe-u.ac.jp)
    ///           C Sharp Solver: Ali Hmer (hmer200a@uregina.ca)
    /// </author>
	public class Relation:Constraint
	{
		private bool[][] rel;
		private Variable v0;
		private Variable v1;
		
		/// <summary> Adds a constraint meaning <tt>rel[v0][v1]</tt>
		/// to the network.
		/// 
		/// </summary>
		/// <param name="net">the network
		/// </param>
		/// <param name="v0">the first argument integer variable
		/// </param>
		/// <param name="v1">the second argument integer variable
		/// </param>
		/// <param name="rel">two-dimentional array of boolean values 
		/// </param>
        public Relation(Network net, Variable v0, bool[][] rel, Variable v1)
            : this(net, v0, rel, v1, ConstraintTypes.Hard)
        {
        }

        public Relation(Network net, Variable v0, bool[][] rel, Variable v1, ConstraintTypes cType)
            : this(net, v0, rel, v1, cType, 0)
        {
        }

        public Relation(Network net, Variable v0, bool[][] rel, Variable v1, ConstraintTypes cType, int weight)
            : base(net, cType, weight)
        {
            this.rel = rel;
            this.v0 = v0;
            this.v1 = v1;
        }

        /// <summary> Creates a Copy of this constraint for a new network <tt>net</tt>.</summary>
	    /// <returns> the Copy of this constraint
	    /// </returns>
	    protected override internal Constraint Copy(Network net)
		{
			return new Relation(net, Copy(v0, net), rel, Copy(v1, net));
		}
		
		protected override internal bool IsModified()
		{
			return v0.IsModified() || v1.IsModified();
		}

        protected internal override bool IsSatisfied()
        {
            return Satisfy(null);
        }
		protected override internal bool Satisfy(Trail trail)
		{
			int m = rel.Length;
			int n = rel[0].Length;
			// limit the domain of v0 to 0..m-1
			var d0 = (IntDomain) v0.Domain;
			d0 = d0.CapInterval(0, m - 1);
			if (d0.Empty)
				return false;
			// limit the domain of v1 to 0..n-1
			var d1 = (IntDomain) v1.Domain;
			d1 = d1.CapInterval(0, n - 1);
			if (d1.Empty)
				return false;
			// Delete impossible indices from v0
			for (int i = 0; i < m; i++)
			{
				if (!d0.Contains(i))
					continue;
				bool support = false;
				for (int j = 0; j < n; j++)
				{
					if (rel[i][j] && d1.Contains(j))
					{
						support = true;
						break;
					}
				}
				if (!support)
				{
					d0 = d0.Delete(i);
				}
			}
			if (d0.Empty)
				return false;
			// Delete impossible indices from v1
			for (int j = 0; j < n; j++)
			{
				if (!d1.Contains(j))
					continue;
				bool support = false;
				for (int i = 0; i < m; i++)
				{
					if (rel[i][j] && d0.Contains(i))
					{
						support = true;
						break;
					}
				}
				if (!support)
				{
					d1 = d1.Delete(j);
				}
			}
			if (d1.Empty)
				return false;
            if (trail != null)
            {
                v0.UpdateDomain(d0, trail);
                v1.UpdateDomain(d1, trail);
            }
		    return true;
		}
		
		public override String ToString()
		{
			return "Relation(" + v0 + "," + v1 + ")";
		}
	}
}