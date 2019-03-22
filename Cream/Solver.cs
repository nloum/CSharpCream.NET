// --------------------------------------------------------------------------------------------------------------------
// <copyright company="PC" file="Solver.cs">
//   2008
// </copyright>
// <summary>
//   Defines the Solver type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cream
{
	using System;

	/// <summary>
	/// An abstract class for constraint solvers.
	/// A solver is constructed with a {@linkplain Network constraint network}
	/// which is used by the solver to find solutions.
	/// Please note that any network can not be simultaneously shared by two different solvers.
	/// Solvers can be used in three typical ways.
	/// As a subroutine: {@link #FindFirst()}, {@link #FindBest()}, etc.<pre>Solution solution = solver.FindFirst();</pre>As a handler caller: {@link #FindAll(ISolutionHandler handler)}, etc.<pre>solver.FindAll(new ISolutionHandler() {
	/// public synchronized void Solved(Solver solver, Solution solution) {
	/// .....
	/// }
	/// });</pre>As a coroutine: {@link #Start()}, {@link #WaitNext()}, {@link #Resume()}, {@link #Stop()}, etc.<pre>for (solver.Start(); solver.WaitNext(); solver.Resume()) {
	/// Solution solution = solver.getSolution();
	/// .....
	/// }
	/// solver.Stop();</pre>
	/// </summary>
	/// <seealso cref="Network">
	/// </seealso>
	/// <seealso cref="Solution">
	/// </seealso>
	/// <seealso cref="ISolutionHandler">
	/// </seealso>
	/// <since>1.0</since>
	/// <version>1.0, 01/12/08</version>
	/// <author>Based on Java Solver by : Naoyuki Tamura (tamura@kobe-u.ac.jp)
	/// C#: Ali Hmer (Hmer200a@uregina.ca)</author>
	public abstract class Solver : IThreadRunnable
	{
		#region Enums

		/// <summary>
		/// The strategy that the solver would follow
		/// </summary>
		public enum StrategyMethod
		{
			/// <summary>
			/// Step strategy
			/// </summary>
			Step = 0,

			/// <summary>
			/// Enumeration Strategy
			/// </summary>
			Enum = 1,

			/// <summary>
			/// Bisection Strategy
			/// </summary>
			Bisect = 2,

			/// <summary>
			/// Soft Constraints Strategy
			/// </summary>
			Soft = 3
		}

		#endregion

		#region Constants

		/// <summary> An option value specifying to return only better solutions</summary>
		public const int Better = 1 << 2;

		/// <summary> A constant value for the default option</summary>
		public const int Default = -1;

		/// <summary> An option value specifying to maximize the objective variable</summary>
		public const int Maximize = 1 << 1;

		/// <summary> An option value specifying to minimize the objective variable</summary>
		public const int Minimize = 1 << 0;

		/// <summary> A constant value for no options</summary>
		public const int None = 0;

		#endregion

		#region Fields

		private bool abort;

		protected internal Solution bestSolution;

		protected internal int bestValue;

		protected long count;

		protected internal bool debug;

		private static int IDCounter;

		private int id;

		private Monitor monitor;

		protected internal String name;

		protected internal Network network;

		protected internal int option;

		private bool ready;

		private bool running;

		protected internal Solution solution;

		/// <summary>
		/// Default solver strategy
		/// </summary>
		private StrategyMethod solverStrategy = StrategyMethod.Step;

		protected internal long startTime;

		private SupportClass.ThreadClass thread;

		protected internal long totalTimeout;

		#endregion

		#region Constructors

		/// <summary> Constructs a solver for the given network, option, and name
		/// (for invocation by subclass constructors).
		/// When <tt>option</tt> is <tt>Default</tt>,
		/// <tt>None</tt> is used if the network has no objective variable,
		/// or else <tt>Minimize</tt> is used.
		/// Solvers and subclasses have their ID number starting from 0.
		/// </summary>
		/// <param name="network">the constraint network
		/// </param>
		/// <param name="option">the option for search strategy, or Default for default search strategy
		/// </param>
		/// <param name="name">the name of the solver, or <tt>null</tt> for a default name
		/// </param>
		protected Solver(Network network, int option, String name)
		{
			this.network = network;
			if (option == Default)
			{
				option = network.Objective == null ? None : Minimize;
			}
			this.option = option;
			id = IDCounter++;
			this.name = name ?? GetType().FullName + id;
			ClearBest();
		}

		/// <summary> Constructs a solver for the given network and name
		/// (for invocation by subclass constructors).
		/// This constructor is equivalent to <tt>Solver(network, Default, name)</tt>.
		/// </summary>
		/// <param name="network">the constraint network
		/// </param>
		/// <param name="name">the name of the solver
		/// </param>
		protected Solver(Network network, String name)
			: this(network, Default, name)
		{
		}

		/// <summary> Constructs a solver for the given network
		/// (for invocation by subclass constructors).
		/// This constructor is equivalent to <tt>Solver(network, Default, null)</tt>.
		/// </summary>
		/// <param name="network">the constraint network
		/// </param>
		protected Solver(Network network)
			: this(network, Default, null)
		{
		}

		/// <summary> Constructs a solver for the given network and option
		/// (for invocation by subclass constructors).
		/// This constructor is equivalent to <tt>Solver(network, option, null)</tt>.
		/// </summary>
		/// <param name="network">the constraint network
		/// </param>
		/// <param name="option">the option for search strategy
		/// </param>
		protected Solver(Network network, int option)
			: this(network, option, null)
		{
		}

		#endregion

		#region Properties

		virtual protected internal bool Aborted
		{
			get
			{
				return abort;
			}

		}

		/// <summary> Returns the best solution this solver has been found.</summary>
		/// <returns> the best solution, or <tt>null</tt> if no solutions have been found
		/// </returns>
		virtual public Solution BestSolution
		{
			get
			{
				return bestSolution;
			}

		}

		/// <summary> Returns the best objective value this solver has been found.
		/// When no solutions have been found, this method returns
		/// <tt>IntDomain.MaxValue</tt> if the search strategy is Minimize,
		/// or <tt>IntDomain.MinValue</tt> if the search strategy is Maximize.
		/// </summary>
		/// <returns> the best objective value
		/// </returns>
		virtual public int BestValue
		{
			get
			{
				return bestValue;
			}

		}

		public bool GenerateOnlySameOrBetterWeightedSolutions { get; set; }

		/// <summary> Returns the ID number of this solver.</summary>
		/// <returns> the ID number of this solver
		/// </returns>
		virtual public int ID
		{
			get
			{
				return id;
			}

		}

		/// <summary> Returns the last solution this solver is found.</summary>
		/// <returns> the last solution, or <tt>null</tt> if no solutions have been found
		/// </returns>
		virtual public Solution Solution
		{
			get
			{
				return solution;
			}

		}

		/// <summary>
		/// Gets or sets SolverStrategy.
		/// </summary>
		public StrategyMethod SolverStrategy
		{
			get
			{
				return solverStrategy;
			}

			set
			{
				if (StrategyMethod.Step <= value && value <= StrategyMethod.Soft)
				{
					solverStrategy = value;
				}
				else
				{
					solverStrategy = StrategyMethod.Step;
				}
			}
		}

		#endregion

		#region Public Methods

		/// <summary> Clears the best solution this solver has been found.</summary>
		public void ClearBest()
		{
			bestSolution = null;
			bestValue = IsOption(Minimize) ? IntDomain.MaxValue : IntDomain.MinValue;
		}

		/// <summary> Invokes the handler for each solution.
		/// This method is equivalent to {@link #FindAll(ISolutionHandler, long) FindFirst(handler, 0)}.
		/// </summary>
		/// <param name="handler">solution handler
		/// </param>
		public virtual void FindAll(ISolutionHandler handler)
		{
			lock (this)
			{
				FindAll(handler, 0);
			}
		}

		/// <summary> Invokes the handler for each solution with the timeout.
		/// This method is implemented as follows:
		/// <pre>
		/// ClearBest();
		/// Start(handler, timeout);
		/// Join();
		/// </pre>
		/// </summary>
		/// <param name="handler">solution handler
		/// </param>
		/// <param name="timeout">timeout in milliseconds (non-positive value means no timeout)
		/// </param>
		public virtual void FindAll(ISolutionHandler handler, long timeout)
		{
			lock (this)
			{
				ClearBest();
				Start(handler, timeout);
				Join();
			}
		}

		/// <summary> Finds the best solution.
		/// This method is equivalent to {@link #FindBest(long) FindBest(0)}.
		/// </summary>
		/// <returns> the best solution, or <tt>null</tt> if no solutions found
		/// </returns>
		public virtual Solution FindBest()
		{
			lock (this)
			{
				return FindBest(0);
			}
		}

		/// <summary> Finds the best solution with the timeout.
		/// This method is implemented as follows:
		/// <pre>
		/// ClearBest();
		/// for (Start(timeout); WaitNext(); Resume()) {
		/// ;
		/// }
		/// Stop();
		/// return getBestSolution();
		/// </pre>
		/// </summary>
		/// <param name="timeout">timeout in milliseconds (non-positive value means no timeout)
		/// </param>
		/// <returns> the best solution, or <tt>null</tt> if no solutions found
		/// </returns>
		public virtual Solution FindBest(long timeout)
		{
			lock (this)
			{
				ClearBest();
				for (Start(timeout); WaitNext(); Resume())
				{
				}
				Stop();
				return BestSolution;
			}
		}

		/// <summary> Finds the first solution.
		/// This method is equivalent to {@link #FindFirst(long) FindFirst(0)}.
		/// </summary>
		/// <returns> the first solution, or <tt>null</tt> if no solutions found
		/// </returns>
		public virtual Solution FindFirst()
		{
			lock (this)
			{
				return FindFirst(0);
			}
		}

		/// <summary> Finds the first solution with the timeout.
		/// This method is implemented as follows:
		/// <pre>
		/// ClearBest();
		/// Start(timeout);
		/// WaitNext();
		/// Stop();
		/// return getBestSolution();
		/// </pre>
		/// </summary>
		/// <param name="timeout">timeout in milliseconds (non-positive value means no timeout)
		/// </param>
		/// <returns> the first solution, or <tt>null</tt> if no solutions found
		/// </returns>
		public virtual Solution FindFirst(long timeout)
		{
			lock (this)
			{
				ClearBest();
				Start(timeout);
				WaitNext();
				Stop();
				return BestSolution;
			}
		}

		/**
							 * Returns the number of solutions. 
							 * @return the number of solutions
							 */
		public long GetCount()
		{
			return count;
		}

		/**
								 * Returns the elapsed time in milli seconds.
								 * @return the elapsed time
								 */
		public long GetElapsedTime()
		{
			long time = (DateTime.Now.Ticks - 621355968000000000) / 10000;
			return time - startTime;
		}

		/// <summary> Gets the monitor.</summary>
		public virtual Monitor GetMonitor()
		{
			return monitor;
		}

		/// <summary> Returns the option value.</summary>
		/// <returns> the option value
		/// </returns>
		public virtual int GetOption()
		{
			return option;
		}

		/// <summary> Waits until the solver ends the execution.</summary>
		public virtual void Join()
		{
			lock (this)
			{
				if (debug)
				{
					Console.Out.WriteLine(name + " Join");
				}
				while (thread == null)
				{
					try
					{
						System.Threading.Monitor.Wait(this);
					}
					catch (System.Threading.ThreadInterruptedException)
					{
					}
				}
				while (running)
				{
					try
					{
						System.Threading.Monitor.Wait(this);
					}
					catch (System.Threading.ThreadInterruptedException)
					{
					}
				}
				thread = null;
			}
		}

		/// <summary> Resets the ID counter to be 0.</summary>
		public static void ResetIDCounter()
		{
			IDCounter = 0;
		}

		/// <summary> Resumes the execution of the solver.</summary>
		public virtual void Resume()
		{
			lock (this)
			{
				if (debug)
				{
					Console.Out.WriteLine(name + " Resume");
				}
				ready = false;
				System.Threading.Monitor.PulseAll(this);
			}
		}

		/// <summary> The body of the solver.
		/// This method is called from {@link Solver#Start} methods.
		/// </summary>
		public abstract void Run();

		/// <summary> Sets the mon.</summary>
		/// <param name="mon">monitor
		/// </param>
		public virtual void SetMonitor(Monitor mon)
		{
			monitor = mon;
			mon.ADD(this);
		}

		/// <summary> Starts the solver in a new thread, and immediately returns to the caller.
		/// The handler is called for each solution and at the end of the solver execution.
		/// You can Stop the solver anytime by calling the {@link #Stop()} method.
		/// </summary>
		/// <param name="handler">solution handler
		/// </param>
		public virtual void Start(ISolutionHandler handler)
		{
			lock (this)
			{
				Start(handler, 0);
			}
		}

		/// <summary> Starts the solver in a new thread with the timeout, and immediately returns to the caller.
		/// When the <tt>timeout</tt> milliseconds have been elapsed
		/// since the Start of the solver, it stops the execution.
		/// The handler is called for each solution and at the end of the solver execution.
		/// You can Stop the solver anytime by calling the {@link #Stop()} method.
		/// </summary>
		/// <param name="handler">solution handler
		/// </param>
		/// <param name="timeout">timeout in milliseconds (non-positive value means no timeout)
		/// </param>
		public virtual void Start(ISolutionHandler handler, long timeout)
		{
			lock (this)
			{
				(new SupportClass.ThreadClass(new System.Threading.ThreadStart(new HandlerInvoker(this, handler, timeout).Run))).Start();
			}
		}

		/// <summary> Starts the solver in a new thread, and immediately returns to the caller.
		/// The {@link #WaitNext()} and {@link #WaitNext(long timeout)} methods can be used
		/// to wait the next solution.
		/// When a solution is found, the solver suspends the execution until
		/// the {@link #Resume()} method is called.
		/// You can Stop the solver anytime by calling the {@link #Stop()} method.
		/// </summary>
		public virtual void Start()
		{
			lock (this)
			{
				Start(0);
			}
		}

		/// <summary> Starts the solver in a new thread with the timeout, and immediately returns to the caller.
		/// When the <tt>timeout</tt> milliseconds have been elapsed
		/// since the Start of the solver, it stops the execution.
		/// The {@link #WaitNext()} and {@link #WaitNext(long timeout)} methods can be used
		/// to wait the next solution, or to detect the timeout.
		/// When a solution is found, the solver suspends the execution until
		/// the {@link #Resume()} method is called.
		/// You can Stop the solver anytime by calling the {@link #Stop()} method.
		/// </summary>
		/// <param name="timeout">timeout in milliseconds (non-positive value means no timeout)
		/// </param>
		public virtual void Start(long timeout)
		{
			lock (this)
			{
				// For bug fix. (ParallelSolver would not Stop)
				// Modified by Muneyuki Kawatani 05/12/09
				thread = null;
				if (debug)
				{
					Console.Out.WriteLine(name + " Start");
				}
				startTime = (DateTime.Now.Ticks - 621355968000000000) / 10000;
				totalTimeout = timeout;
				abort = false;
				running = true;
				ready = false;
				solution = null;
				thread = new SupportClass.ThreadClass(new System.Threading.ThreadStart(Run));
				thread.Start();
				System.Threading.Monitor.PulseAll(this);
			}
		}

		/// <summary> Stops the execution of the solver.</summary>
		public virtual void Stop()
		{
			lock (this)
			{
				if (debug)
				{
					Console.Out.WriteLine(name + " Stop");
				}
				abort = true;
				while (running)
				{
					System.Threading.Monitor.PulseAll(this);
					try
					{
						System.Threading.Monitor.Wait(this);
					}
					catch (System.Threading.ThreadInterruptedException)
					{
					}
				}
				// For bug fix. (ParallelSolver would not Stop)
				// Modified by Muneyuki Kawatani 05/12/09
				// thread = null;
			}
		}

		/// <summary> Returns the name of this solver.</summary>
		/// <returns> the name of this solver
		/// </returns>
		public override String ToString()
		{
			return name;
		}

		/// <summary> Waits for the next solution, or the end of the solver execution with the timeout.
		/// It returns  <tt>true</tt> if the next solution is available within the timeout milliseconds,
		/// <tt>false</tt> if the solver ends the execution or the <tt>timeout</tt> milliseconds have been elapsed
		/// since the Start of this method.
		/// </summary>
		/// <param name="timeout">timeout in milliseconds (non-positive value means no timeout)
		/// </param>
		/// <returns> <tt>true</tt> if the next solution is available
		/// </returns>
		public virtual bool WaitNext(long timeout)
		{
			lock (this)
			{
				if (debug)
				{
					Console.Out.WriteLine(name + " WaitNext");
				}
				long deadline = Int64.MaxValue;
				if (timeout > 0)
				{
					deadline = (DateTime.Now.Ticks - 621355968000000000) / 10000 + timeout;
				}
				if (totalTimeout > 0)
				{
					deadline = Math.Min(deadline, startTime + totalTimeout);
				}
				while (running && !ready)
				{
					if (deadline == Int64.MaxValue)
					{
						try
						{
							System.Threading.Monitor.Wait(this);
						}
						catch (System.Threading.ThreadInterruptedException)
						{
						}
					}
					else
					{
						long t = deadline - (DateTime.Now.Ticks - 621355968000000000) / 10000;
						if (t <= 0)
							break;
						try
						{
							System.Threading.Monitor.Wait(this, TimeSpan.FromMilliseconds(Math.Max(10, t)));
						}
						catch (System.Threading.ThreadInterruptedException)
						{
						}
					}
				}
				if (!running)
				{
					// Failure
					return false;
				}
				if (!ready)
				{
					// Timeout
					return false;
				}
				return true;
			}
		}

		/// <summary> Waits for the next solution, or the end of the solver execution.
		/// It returns  <tt>true</tt> if the next solution is available, <tt>false</tt> if the solver ends the execution.
		/// </summary>
		/// <returns> <tt>true</tt> if the next solution is available
		/// </returns>
		public virtual bool WaitNext()
		{
			lock (this)
			{
				return WaitNext(0);
			}
		}

		#endregion

		#region Protected Methods

		protected internal virtual void Fail()
		{
			lock (this)
			{
				if (debug)
				{
					Console.Out.WriteLine(name + " Fail");
				}
				solution = null;
				running = false;
				System.Threading.Monitor.PulseAll(this);
			}
		}

		protected internal virtual bool IsBetter(int value1, int value2)
		{
			return IsOption(Minimize) ? value1 < value2 : value1 > value2;
		}

		protected internal virtual bool IsOption(int opt)
		{
			return (option & opt) != 0;
		}

		protected internal virtual void Success()
		{
			lock (this)
			{
				if (debug)
				{
					Console.Out.WriteLine(name + " Success");
				}
				if (abort)
					return;
				count++;
				System.Threading.Thread.Sleep(0);
				bool better = UpdateBest();
				if (IsOption(Better))
				{
					if (monitor != null)
					{
						//monitor.ADDData(this, bestValue);
						int value = solution.ObjectiveIntValue;
						monitor.ADDData(this, value);
					}
					if (!better)
						return;
				}
				else
				{
					if (monitor != null)
					{
						int value = solution.ObjectiveIntValue;
						monitor.ADDData(this, value);
					}
				}
				ready = true;
				System.Threading.Monitor.PulseAll(this);
				while (!abort && ready)
				{
					try
					{
						System.Threading.Monitor.Wait(this);
					}
					catch (System.Threading.ThreadInterruptedException)
					{
					}
				}
			}
		}

		protected internal virtual bool UpdateBest()
		{
			if (solution == null)
				return false;
			if (network.Objective == null)
			{
				bestSolution = solution;
				return true;
			}
			int value = solution.ObjectiveIntValue;
			if (IsBetter(value, bestValue))
			{
				bestSolution = solution;
				bestValue = value;
				return true;
			}
			return false;
		}

		#endregion

		private class HandlerInvoker : IThreadRunnable
		{
			private void InitBlock(Solver encloseInstance)
			{
				enclosingInstance = encloseInstance;
			}
			private Solver enclosingInstance;
			public Solver Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}

			}
			private ISolutionHandler handler;
			private long timeout;

			public HandlerInvoker(Solver enclosingInstance, ISolutionHandler handler, long timeout)
			{
				InitBlock(enclosingInstance);
				this.handler = handler;
				this.timeout = timeout;
			}

			public virtual void Run()
			{
				Enclosing_Instance.ClearBest();
				for (Enclosing_Instance.Start(timeout); Enclosing_Instance.WaitNext(); Enclosing_Instance.Resume())
				{
					if (Enclosing_Instance.Aborted)
						break;
					handler.Solved(Enclosing_Instance, Enclosing_Instance.solution);
				}
				// solution = null;
				handler.Solved(Enclosing_Instance, null);
				Enclosing_Instance.Stop();
			}
		}

	}
}