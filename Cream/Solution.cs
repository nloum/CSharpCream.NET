/*
* @(#)Solution.cs
*/
using System;
using Cream.AllenTemporal;

namespace  Cream
{
	
	/// <summary> Solutions.
	/// Solutions are returned by {@linkplain Solver constraint solvers}.
	/// A solution consists of {@linkplain Domain domains} for variables
	/// and {@linkplain Code a code}.
	/// </summary>
	/// <seealso cref="Solver">
	/// </seealso>
	/// <seealso cref="Domain">
	/// </seealso>
	/// <seealso cref="Code">
	/// </seealso>
	/// <since> 1.0
	/// </since>
	/// <version>  1.0, 01/12/08
	/// </version>
    /// <author>  Based on Java Solver by : Naoyuki Tamura (tamura@kobe-u.ac.jp) 
    ///           C#: Ali Hmer (Hmer200a@uregina.ca)
	/// </author>
	public class Solution
	{
		/// <summary> Returns the integer value of the objective variable in the solution.</summary>
		/// <returns> the integer value of the objective variable
		/// </returns>
		virtual public int ObjectiveIntValue
		{
			get
			{
				return ((IntDomain) objectiveDomain).Value();
			}
			
		}
		/// <summary> Returns the code of the solution.</summary>
		/// <returns> the code of the solution
		/// </returns>
		virtual public Code Code
		{
			get
			{
				return code;
			}
		}

        virtual public int Weight { get; set;}

        virtual public long Time { get; set; }

		private Network network;
		private Domain objectiveDomain;
		private Domain[] bindings;
		private Code code;
		
		/// <summary> Constructs a solution from the given network.</summary>
		/// <param name="network">the constraint network
		/// </param>
		public Solution(Network network)
		{
			this.network = network;
			objectiveDomain = null;
			if (network.Objective != null)
			{
				objectiveDomain = network.Objective.Domain;
			}
			System.Collections.IList variables = network.Variables;
			bindings = new Domain[variables.Count];
			System.Collections.IEnumerator vs = variables.GetEnumerator();
			while (vs.MoveNext())
			{
				var v = (Variable) vs.Current;
				bindings[v.Index] = v.Domain;
			}
            // Do th efollowing for Soft constraints only
            //SwapValuesToPreferences();



			code = new Code(network);
		}

        //private void SwapValuesToPreferences()
        //{
        //    var modCs = network.Constraints.Cast<Constraint>();
        //    var softConstraints = modCs.Where(cons => cons.CType == ConstraintTypes.Soft);
        //    foreach (IntVariable V0 in network.Variables)
        //    {
        //        //solution.GetIntValue(v);
        //        var softEqualsWithV0 =
        //            (softConstraints
        //                .Where(soft => soft is Equals)).Cast<Equals>()
        //                .Where(cons => (cons.Vars[0] == V0))
        //                //.Where(cons => ((IntDomain) cons.Vars[0].Domain)
        //                //                   .Contains(((IntDomain) cons.Vars[1].Domain).Min()))
        //                .Select(doms =>  doms.Vars[1]).Distinct();
        //    }
        //    //var list = GetIntValue(V0);
        //}



	    /// <summary> Returns the domain of the given variable in the solution.</summary>
		/// <param name="v">the variable
		/// </param>
		/// <returns> the domain of the variable
		/// </returns>
		public virtual Domain GetDomain(Variable v)
		{
			return bindings[v.Index];
		}
		
		/// <summary> Returns the integer value of the given variable in the solution.</summary>
		/// <param name="v">the variable
		/// </param>
		/// <returns> the integer value of the variable
		/// </returns>
		public virtual int GetIntValue(Variable v)
		{
		    if (v is IntVariable)
            {
                return ((IntDomain) GetDomain(v)).Value();
            }
		    if (v is AllenVariable)
		    {
		        return ((AllenDomain) GetDomain(v)).Value();
		    }
		    return int.MinValue;
		}

	    /// <summary> Returns a readable string representation of this solution.</summary>
		/// <returns> the readable string representation
		/// </returns>
		public override String ToString()
		{
			String s = "";
			/*
			if (network.getObjective() != null) {
			s += "Objective: " + network.getObjective() + "=" + getObjectiveValue() + "\n";
			}
			*/
			System.Collections.IEnumerator vs = network.Variables.GetEnumerator();
			String delim = "";
			while (vs.MoveNext())
			{
				var v = (Variable) vs.Current;
				s += (delim + v.Name + "=" + GetDomain(v));
				delim = ",";
			}
			return s;
		}
	}
}