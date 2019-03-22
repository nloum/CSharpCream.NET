/*
* @(#)Variable.cs
*/
using System;

namespace  Cream
{
	
	/// <summary> Variables.
	/// A variable is a component of a {@linkplain Network constraint network}.
	/// A variable is constructed with an initial {@linkplain Domain domain}
	/// which specifies the set of Elements over which the variable ranges.
	/// See {@link Network} for example programs to construct variables and
	/// Add them to a constraint network.
	/// </summary>
	/// <seealso cref="Network">
	/// </seealso>
	/// <seealso cref="Domain">
	/// </seealso>
	/// <since> 1.0
	/// </since>
	/// <version>  1.0, 01/12/08
	/// </version>
    /// <author>  Based on Java Solver by : Naoyuki Tamura (tamura@kobe-u.ac.jp) - 
    ///           C#: Ali Hmer (Hmer200a@uregina.ca)
	/// </author>
	public class Variable : ICloneable
	{
	    protected internal Network Network { get; set; }

	    virtual protected internal int Index
		{
			get
			{
				return index;
			}
			
			set
			{
				index = value;
			}
			
		}
		public Domain Domain
		{
			get
			{
				return domain;
			}
			
			set
			{
				if (domain == null || !domain.Equals(value))
				{
					domain = value;
					modified = true;
				}
			}
			
		}
		/// <summary> Returns the name of this variable.</summary>
		/// <returns> the name of this variable
		/// </returns>
        private static int Count = 1;

	    private int index = -1;
        private Domain domain;
        private String name;
        private bool modified;
        private bool watch;
		
		public String Name
		{
			get
			{
				return name;
			}
            set
            {
                name = value;
            }
			
		}

        // <summary> Returns the watch flag.</summary>
        /// <returns> the watch flag
        /// </returns>
        /// <summary> Sets the watch flag.</summary>
        virtual public bool Watch
        {
            get
            {
                return watch;
            }

            set
            {
                watch = value;
            }

        }

        public bool IsValueType {get;set;}
		
		/// <summary> Constructs a variable of the network
		/// with an initial domain <tt>d</tt>
		/// and a default name.
		/// This constructor is equivalent to <tt>Variable(network, d, null)</tt>.
		/// </summary>
		/// <param name="net">the network
		/// </param>
		/// <param name="d">the initial domain
		/// </param>
		public Variable(Network net, Domain d):this(net, d, null)
		{
		}
		
		/// <summary> Constructs a variable of the network
		/// with an initial domain <tt>d</tt>
		/// and a name specified by the parameter <tt>name</tt>.
		/// When the parameter <tt>name</tt> is <tt>null</tt>,
		/// default names (<tt>v1</tt>, <tt>v2</tt>, and so on) are used.
		/// </summary>
		/// <param name="net">the network
		/// </param>
		/// <param name="d">the initial domain
		/// </param>
		/// <param name="name">the name of the variable, or <tt>null</tt> for a default name
		/// </param>
		public Variable(Network net, Domain d, String name)
		{
			Network = net;
			Domain = d;
			modified = true;
			watch = false;
			Name = name ?? "v" + (Count++);
			Network.ADD(this);
		}
		
        /// <summary>
        /// Updates the domain of the current variable with the parameter domain d after
        /// pushing the Trail trial
        /// </summary>
        /// <param name="d">the domain to be updated with</param>
        /// <param name="trail">the trail to be pushed</param>
		public virtual void  UpdateDomain(Domain d, Trail trail)
		{
			if (domain == null || !domain.Equals(d))
			{
				trail.Push(this);
				domain = d;
				modified = true;
				if (watch)
				{
					Console.Out.WriteLine(this + " = " + domain);
				}
			}
		}
		
		protected internal virtual void  SetModified()
		{
			modified = true;
		}
		
		protected internal virtual void  ClearModified()
		{
			modified = false;
		}
		
		public virtual bool IsModified()
		{
			return modified;
		}
		
		public virtual void  SetWatch()
		{
			watch = true;
		}
		
		public virtual void  ClearWatch()
		{
			watch = false;
		}
		
		/// <summary> Creates a Copy of this variable for a new network.</summary>
		/// <returns> the Copy of the variable
		/// </returns>
		protected internal virtual Variable Copy(Network net)
		{
			return new Variable(net, domain, name);
		}
		
		/// <summary> Returns the name of this variable.</summary>
		/// <returns> the name of this variable
		/// </returns>
		public override String ToString()
		{
			return name;
		}
		virtual public Object Clone()
		{
			return null;
		}
	}
}