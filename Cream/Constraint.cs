/*
* @(#)Constraint.cs
*/
using System;
using System.ComponentModel;

namespace  Cream
{
    public enum ConstraintTypes
    {
        Hard, Soft
    }

    /// <summary> An abstract class for constraints.
    /// A constraint is a component of a constraint network.
    /// See Network for example programs to construct constraints and
    /// Add them to a constraint network.
    /// </summary>
    /// <seealso cref="Network">
    /// </seealso>
    /// <since> 1.0
    /// </since>
    /// <version>  1.0, 01/12/08
    ///            Updated 1.1 05/01/08
    /// </version>
    /// 
    /// <author> Based on Java Solver by : Naoyuki Tamura (tamura@kobe-u.ac.jp)  
    ///          C#: Ali Hmer (Hmer200a@uregina.ca)
    /// </author>
    public abstract class Constraint
    {
        //virtual protected internal Condition Condition
        //{
        //}

        protected internal Network Network { get; set; }

        [DefaultValue(-1)]
        protected internal int Index { get; set; }

        [DefaultValue(ConstraintTypes.Hard)]
        protected internal ConstraintTypes CType { get; set; }

        [DefaultValue(0)]
        public int Weight { get; set; }

        /// <summary> Constructor.
        /// (for invocation by subclass constructors, typically implicit)
        /// </summary>
        protected internal Constraint(Network net)
        {
            Network = net;
            Index = -1;
            Network.ADD(this);
            CType = ConstraintTypes.Hard;
        }

        /// <summary> Constructor.
        /// (for invocation by subclass constructors, typically implicit)
        /// </summary>
        protected internal Constraint(Network net, ConstraintTypes cType)
        {
            Network = net;
            Index = -1;
            Network.ADD(this);
            CType = cType;
        }

        /// <summary> Constructor.
        /// (for invocation by subclass constructors, typically implicit)
        /// </summary>
        protected internal Constraint(Network net, ConstraintTypes cType, int weight)
        {
            Network = net;
            Index = -1;
            Weight = weight;
            Network.ADD(this);
            CType = cType;
        }

		
        /// <summary> Creates a Copy of this constraint for a new network <tt>net</tt>.</summary>
        /// <returns> the Copy of this constraint
        /// </returns>
        protected internal abstract Constraint Copy(Network net);
		
        protected internal virtual void  ClearCondition()
        {
        }
		
        protected internal virtual Condition ExtractCondition()
        {
            return null;
        }
		
        protected internal abstract bool IsModified();
		
        protected internal abstract bool Satisfy(Trail trail);

        protected internal abstract bool IsSatisfied();
		
        protected internal static Variable Copy(Variable v0, Network net)
        {
            int j = v0.Index;
            return net.GetVariable(j);
        }
		
        protected internal static Variable[] Copy(Variable[] v0, Network net)
        {
            var v = new Variable[v0.Length];
            for (int i = 0; i < v0.Length; i++)
            {
                int j = v0[i].Index;
                v[i] = net.GetVariable(j);
            }
            return v;
        }
		
        protected internal static bool IsModified(Variable[] v)
        {
            for (int i = 0; i < v.Length; i++)
            {
                if (v[i].IsModified())
                    return true;
            }
            return false;
        }
		
        protected internal static String ToString(Variable[] v)
        {
            String s = "";
            if (v != null)
            {
                String delim = "";
                for (int i = 0; i < v.Length; i++)
                {
                    s += (delim + v[i]);
                    delim = ",";
                }
            }
            return "{" + s + "}";
        }
		
        protected internal static String ToString(int[] a)
        {
            String s = "";
            if (a != null)
            {
                String delim = "";
                for (int i = 0; i < a.Length; i++)
                {
                    s += (delim + a[i]);
                    delim = ",";
                }
            }
            return "{" + s + "}";
        }
    }
}