using System;

namespace Cream.AllenTemporal {

	/// <summary>
	/// 
	/// </summary>
	public class AllenConstraint : Constraint {
		#region Fields

		private AllenDomain[] newDomains = new AllenDomain[2];

		private int relation;

		private Variable[] v;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="AllenConstraint"/> class.
		/// </summary>
		/// <param name="net">The net.</param>
		/// <param name="a">A.</param>
		/// <param name="v">The v.</param>
		private AllenConstraint(Network net, int a, Variable[] v)
			: base(net) {
			relation = a;
			this.v = v;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AllenConstraint"/> class.
		/// </summary>
		/// <param name="net">The net.</param>
		/// <param name="a">A.</param>
		/// <param name="v1">The v1.</param>
		/// <param name="v2">The v2.</param>
		public AllenConstraint(Network net, int a, Variable v1, Variable v2)
			: this(net, a, new[] { v1, v2 }) {
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the allen event.
		/// </summary>
		/// <value>The allen event.</value>
		public int AllenEvent {
			get {
				return relation;
			}
		}

		/// <summary>
		/// Gets the new domains.
		/// </summary>
		/// <value>The new domains.</value>
		public AllenDomain[] NewDomains {
			get {
				return newDomains;
			}
		}

		/// <summary>
		/// Gets the vars.
		/// </summary>
		/// <value>The vars.</value>
		public Variable[] Vars {
			get {
				return v;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Numerics to symbolic.
		/// </summary>
		/// <returns></returns>
		public bool NumericToSymbolic() {
			var d1 = (AllenDomain)Vars[0].Domain;
			var d2 = (AllenDomain)Vars[1].Domain;
			int latestEndTime1 = d1.Max() + d1.Duration;
			int latestEndTime2 = d2.Max() + d2.Duration;

			if ((latestEndTime1 < d2.Min()) && (relation != AllenEvents.PRECEDES)) // Precedes
            {
				return false;
			}
			if ((d1.Min() > latestEndTime2) && (relation != AllenEvents.PRECEDEDBY)) // Precededby
            {
				return false;
			}
			if (((d1.Duration != d2.Duration) ||
				 ((latestEndTime2 - d1.Min()) < d1.Duration) ||
				 ((latestEndTime1 - d2.Min()) < d1.Duration))) {
				if (relation == AllenEvents.EQUALS) {
					return false;
				}
				if (d1.Duration > d2.Duration) {
					if ((relation == AllenEvents.STARTS) || (relation == AllenEvents.FINISHES) ||
						(relation == AllenEvents.DURING)) {
						return false;
					}
				} else if (d1.Duration < d2.Duration) {
					if ((relation == AllenEvents.STARTEDBY) || (relation == AllenEvents.FINISHEDBY)
					 || (relation == AllenEvents.CONTAINS)) {
						return false;
					}
				}
			}
			if ((d1.Duration == d2.Duration) &&
				((relation == AllenEvents.DURING) || (relation == AllenEvents.CONTAINS) ||
				 (relation == AllenEvents.STARTS) || (relation == AllenEvents.STARTEDBY) ||
				 (relation == AllenEvents.FINISHES) || (relation == AllenEvents.FINISHEDBY))) {
				return false;
			}
			if (((d1.Min() + d1.Duration) > (latestEndTime2 - d2.Duration)) &&
				((relation == AllenEvents.MEETS) || (relation == AllenEvents.PRECEDES))) {
				return false;
			}
			if (((latestEndTime1 - d1.Duration) < (d2.Min() + d2.Duration)) &&
				((relation == AllenEvents.METBY) || (relation == AllenEvents.PRECEDEDBY))) {
				return false;
			}
			if ((d1.Min() > (latestEndTime2 - d2.Duration)) &&
				((relation == AllenEvents.STARTS) || (relation == AllenEvents.STARTEDBY)
			   || (relation == AllenEvents.CONTAINS))) {
				return false;
			}
			// the next is a new one
			if ((d1.Min() >= (latestEndTime2 - d2.Duration)) &&
			  (relation == AllenEvents.OVERLAPS)) {
				return false;
			}

			if (((d1.Min() + d1.Duration) > latestEndTime2) &&
				((relation == AllenEvents.FINISHES) || (relation == AllenEvents.FINISHEDBY) ||
				 (relation == AllenEvents.DURING))) {
				return false;
			}
			if ((latestEndTime1 < (d2.Min() + d2.Duration)) &&
				((relation == AllenEvents.FINISHES) || relation == AllenEvents.FINISHEDBY)) {
				return false;
			}

			if (((latestEndTime1 - d1.Duration) > (d2.Min() + d2.Duration)) && (relation == AllenEvents.OVERLAPPEDBY))
			// has been updated from < to >
            {
				return false;
			}
			if ((d1.Min() < d2.Min()) && (relation == AllenEvents.DURING)) {
				return false;
			}
			return true;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override String ToString() {
			String a = string.Empty;
			switch (relation) {

				case AllenEvents.PRECEDES:
					a = "PRECEDES"; break;

				case AllenEvents.PRECEDEDBY:
					a = "PRECEDEDBY"; break;

				case AllenEvents.EQUALS:
					a = "EQUAL"; break;

				case AllenEvents.MEETS:
					a = "MEET"; break;

				case AllenEvents.METBY:
					a = "METBY"; break;

				case AllenEvents.DURING:
					a = "DURING"; break;

				case AllenEvents.CONTAINS:
					a = "CONTAINS"; break;

				case AllenEvents.STARTS:
					a = "STARTS"; break;

				case AllenEvents.STARTEDBY:
					a = "STARTEDBY"; break;

				case AllenEvents.FINISHES:
					a = "FINISHES"; break;

				case AllenEvents.FINISHEDBY:
					a = "FINISHEDBY"; break;

				case AllenEvents.OVERLAPS:
					a = "OVERLAP"; break;

				case AllenEvents.OVERLAPPEDBY:
					a = "OVERLAPPEDBY"; break;
			}
			return "AllenConstraint(" + a + "," + ToString(v) + ")";
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Creates a Copy of this constraint for a new network <tt>net</tt>.
		/// </summary>
		/// <param name="net"></param>
		/// <returns>the Copy of this constraint</returns>
		protected internal override Constraint Copy(Network net) {
			return new AllenConstraint(net, relation, Copy(v, net));
		}

		/// <summary>
		/// Determines whether this instance is modified.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if this instance is modified; otherwise, <c>false</c>.
		/// </returns>
		protected internal override bool IsModified() {
			return IsModified(v);
		}

		/// <summary>
		/// Determines whether this instance is satisfied.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if this instance is satisfied; otherwise, <c>false</c>.
		/// </returns>
		protected internal override bool IsSatisfied() {
			return Satisfy(null);
		}

		/// <summary>
		/// Satisfies the specified trail.
		/// </summary>
		/// <param name="trail">The trail.</param>
		/// <returns></returns>
		protected internal override bool Satisfy(Trail trail) {
			switch (relation) {
				case AllenEvents.PRECEDES:
					return SatisfyPrecedes(Vars[0], Vars[1], trail);

				case AllenEvents.PRECEDEDBY:
					return SatisfyPrecededby(Vars[0], Vars[1], trail);

				case AllenEvents.EQUALS:
					return SatisfyEquals(Vars[0], Vars[1], trail);

				case AllenEvents.MEETS:
					return SatisfyMeets(Vars[0], Vars[1], trail);

				case AllenEvents.METBY:
					return SatisfyMetby(Vars[0], Vars[1], trail);

				case AllenEvents.DURING:
					return SatisfyDuring(Vars[0], Vars[1], trail);

				case AllenEvents.CONTAINS:
					return SatisfyContains(Vars[0], Vars[1], trail);

				case AllenEvents.STARTS:
					return SatisfyStarts(Vars[0], Vars[1], trail);

				case AllenEvents.STARTEDBY:
					return SatisfyStartedby(Vars[0], Vars[1], trail);

				case AllenEvents.FINISHES:
					return SatisfyFinishes(Vars[0], Vars[1], trail);

				case AllenEvents.FINISHEDBY:
					return SatisfyFinishedby(Vars[0], Vars[1], trail);

				case AllenEvents.OVERLAPS:
					return SatisfyOverlaps(Vars[0], Vars[1], trail);

				case AllenEvents.OVERLAPPEDBY:
					return SatisfyOverlappedby(Vars[0], Vars[1], trail);
			}
			return false;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Satisfies the contains.
		/// </summary>
		/// <param name="v1">The v1.</param>
		/// <param name="v2">The v2.</param>
		/// <param name="trail">The trail.</param>
		/// <returns></returns>
		private static bool SatisfyContains(Variable v1, Variable v2, Trail trail) {
			var d1 = (AllenDomain)v1.Domain;
			if (d1.Empty) return false;
			var d3 = (AllenDomain)d1.Clone();
			var d2 = (AllenDomain)v2.Domain;
			if (d2.Empty) return false;
			for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step) {
				bool found = false;
				for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step) {
					int lt1 = st1 + d1.Duration;
					int lt2 = st2 + d2.Duration;
					if ((st1 < st2) && (lt1 > lt2)) {
						found = true;
						break;
					}
				}
				if (!found) {
					d3.Remove(st1);
					if (d3.Size() == 0) {
						return false;
					}
				}
			}
			//v1.UpdateDomain(d3, trail);
			return true;
		}

		/// <summary>
		/// Satisfies the during.
		/// </summary>
		/// <param name="v1">The v1.</param>
		/// <param name="v2">The v2.</param>
		/// <param name="trail">The trail.</param>
		/// <returns></returns>
		private static bool SatisfyDuring(Variable v1, Variable v2, Trail trail) {
			var d1 = (AllenDomain)v1.Domain;
			if (d1.Empty) return false;
			var d3 = (AllenDomain)d1.Clone();
			var d2 = (AllenDomain)v2.Domain;
			if (d2.Empty) return false;
			for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step) {
				bool found = false;
				for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step) {
					int lt1 = st1 + d1.Duration;
					int lt2 = st2 + d2.Duration;
					if ((st1 > st2) && (lt1 < lt2)) {
						found = true;
						break;
					}
				}
				if (!found) {
					d3.Remove(st1);
					if (d3.Size() == 0) {
						return false;
					}
				}
			}
			//v1.UpdateDomain(d3, trail);
			return true;
		}

		/// <summary>
		/// Satisfies the equals.
		/// </summary>
		/// <param name="v1">The v1.</param>
		/// <param name="v2">The v2.</param>
		/// <param name="trail">The trail.</param>
		/// <returns></returns>
		private static bool SatisfyEquals(Variable v1, Variable v2, Trail trail) {
			var d1 = (AllenDomain)v1.Domain;
			if (d1.Empty) return false;
			var d3 = new AllenDomain(d1.Min(), d1.Max() + d1.Duration, d1.Duration, d1.Step);
			var d2 = (AllenDomain)v2.Domain;
			if (d2.Empty) return false;
			for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step) {
				bool found = false;
				for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step) {
					int lt1 = st1 + d1.Duration;
					int lt2 = st2 + d2.Duration;
					if ((st1 == st2) && (lt1 == lt2)) {
						found = true;
						break;
					}
				}
				if (!found) {
					d3.Remove(st1);
					if (d3.Size() == 0) {
						return false;
					}
				}
			}
			//v1.UpdateDomain(d3, trail);
			return true;
		}

		/// <summary>
		/// Satisfies the finishedby.
		/// </summary>
		/// <param name="v1">The v1.</param>
		/// <param name="v2">The v2.</param>
		/// <param name="trail">The trail.</param>
		/// <returns></returns>
		private static bool SatisfyFinishedby(Variable v1, Variable v2, Trail trail) {
			var d1 = (AllenDomain)v1.Domain;
			if (d1.Empty) return false;
			var d3 = (AllenDomain)d1.Clone();
			var d2 = (AllenDomain)v2.Domain;
			if (d2.Empty) return false;
			for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step) {
				bool found = false;
				for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step) {
					int lt1 = st1 + d1.Duration;
					int lt2 = st2 + d2.Duration;
					if ((st1 < st2) && (lt1 == lt2)) {
						found = true;
						break;
					}
				}
				if (!found) {
					d3.Remove(st1);
					if (d3.Size() == 0) {
						return false;
					}
				}
			}
			//v1.UpdateDomain(d3, trail);
			return true;
		}

		/// <summary>
		/// Satisfies the finishes.
		/// </summary>
		/// <param name="v1">The v1.</param>
		/// <param name="v2">The v2.</param>
		/// <param name="trail">The trail.</param>
		/// <returns></returns>
		private static bool SatisfyFinishes(Variable v1, Variable v2, Trail trail) {
			var d1 = (AllenDomain)v1.Domain;
			if (d1.Empty) return false;
			var d3 = (AllenDomain)d1.Clone();
			var d2 = (AllenDomain)v2.Domain;
			if (d2.Empty) return false;
			for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step) {
				bool found = false;
				for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step) {
					int lt1 = st1 + d1.Duration;
					int lt2 = st2 + d2.Duration;
					if ((st1 > st2) && (lt1 == lt2)) {
						found = true;
						break;
					}
				}
				if (!found) {
					d3.Remove(st1);
					if (d3.Size() == 0) {
						return false;
					}
				}
			}
			//v1.UpdateDomain(d3, trail);
			return true;
		}

		/// <summary>
		/// Satisfies the meets.
		/// </summary>
		/// <param name="v1">The v1.</param>
		/// <param name="v2">The v2.</param>
		/// <param name="trail">The trail.</param>
		/// <returns></returns>
		private static bool SatisfyMeets(Variable v1, Variable v2, Trail trail) {
			var d1 = (AllenDomain)v1.Domain;
			if (d1.Empty) return false;
			var d3 = (AllenDomain)d1.Clone();
			var d2 = (AllenDomain)v2.Domain;
			if (d2.Empty) return false;
			for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step) {
				bool found = false;
				for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step) {
					int lt1 = st1 + d1.Duration;
					if (lt1 == st2) {
						found = true;
						break;
					}
				}
				if (!found) {
					d3.Remove(st1);
					if (d3.Size() == 0) {
						return false;
					}
				}
			}
			//v1.UpdateDomain(d3, trail);
			return true;
		}

		/// <summary>
		/// Satisfies the metby.
		/// </summary>
		/// <param name="v1">The v1.</param>
		/// <param name="v2">The v2.</param>
		/// <param name="trail">The trail.</param>
		/// <returns></returns>
		private static bool SatisfyMetby(Variable v1, Variable v2, Trail trail) {
			var d1 = (AllenDomain)v1.Domain;
			if (d1.Empty) return false;
			var d3 = (AllenDomain)d1.Clone();
			var d2 = (AllenDomain)v2.Domain;
			if (d2.Empty) return false;
			for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step) {
				bool found = false;
				for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step) {
					int lt2 = st2 + d2.Duration;
					if (lt2 == st1) {
						found = true;
						break;
					}
				}
				if (!found) {
					d3.Remove(st1);
					if (d3.Size() == 0) {
						return false;
					}
				}
			}
			//v1.UpdateDomain(d3, trail);
			return true;
		}

		/// <summary>
		/// Satisfies the overlappedby.
		/// </summary>
		/// <param name="v1">The v1.</param>
		/// <param name="v2">The v2.</param>
		/// <param name="trail">The trail.</param>
		/// <returns></returns>
		private static bool SatisfyOverlappedby(Variable v1, Variable v2, Trail trail) {
			var d1 = (AllenDomain)v1.Domain;
			if (d1.Empty) return false;
			var d3 = (AllenDomain)d1.Clone();
			var d2 = (AllenDomain)v2.Domain;
			if (d2.Empty) return false;
			for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step) {
				bool found = false;
				for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step) {
					int lt1 = st1 + d1.Duration;
					int lt2 = st2 + d2.Duration;
					if ((st1 > st2) && (lt1 > lt2)) {
						found = true;
						break;
					}
				}
				if (!found) {
					d3.Remove(st1);
					if (d3.Size() == 0) {
						return false;
					}
				}
			}
			//v1.UpdateDomain(d3, trail);
			return true;
		}

		/// <summary>
		/// Satisfies the overlaps.
		/// </summary>
		/// <param name="v1">The v1.</param>
		/// <param name="v2">The v2.</param>
		/// <param name="trail">The trail.</param>
		/// <returns></returns>
		private static bool SatisfyOverlaps(Variable v1, Variable v2, Trail trail) {
			var d1 = (AllenDomain)v1.Domain;
			if (d1.Empty) return false;
			var d3 = (AllenDomain)d1.Clone();
			var d2 = (AllenDomain)v2.Domain;
			if (d2.Empty) return false;
			for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step) {
				bool found = false;
				for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step) {
					int lt1 = st1 + d1.Duration;
					int lt2 = st2 + d2.Duration;
					if ((st1 < st2) && (lt1 < lt2)) {
						found = true;
						break;
					}
				}
				if (!found) {
					d3.Remove(st1);
					if (d3.Size() == 0) {
						return false;
					}
				}
			}
			//v1.UpdateDomain(d3, trail);
			return true;
		}

		/// <summary>
		/// Satisfies the precededby.
		/// </summary>
		/// <param name="v1">The v1.</param>
		/// <param name="v2">The v2.</param>
		/// <param name="trail">The trail.</param>
		/// <returns></returns>
		private static bool SatisfyPrecededby(Variable v1, Variable v2, Trail trail) {
			var d1 = (AllenDomain)v1.Domain;
			if (d1.Empty) return false;
			var d3 = (AllenDomain)d1.Clone();
			var d2 = (AllenDomain)v2.Domain;
			if (d2.Empty) return false;
			for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step) {
				bool found = false;
				for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step) {
					int lt2 = st2 + d2.Duration;
					if (lt2 < st1) {
						found = true;
						break;
					}
				}
				if (!found) {
					d3.Remove(st1);
					if (d3.Size() == 0) {
						return false;
					}
				}
			}
			//v1.UpdateDomain(d3, trail);
			return true;
		}

		/// <summary>
		/// Satisfies the precedes.
		/// </summary>
		/// <param name="v1">The v1.</param>
		/// <param name="v2">The v2.</param>
		/// <param name="trail">The trail.</param>
		/// <returns></returns>
		private static bool SatisfyPrecedes(Variable v1, Variable v2, Trail trail) {
			var d1 = (AllenDomain)v1.Domain;
			if (d1.Empty) return false;
			var d3 = new AllenDomain(d1.Min(), d1.Max() + d1.Duration, d1.Duration, d1.Step);
			var d2 = (AllenDomain)v2.Domain;
			if (d2.Empty) return false;
			for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step) {
				bool found = false;
				for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step) {
					int lt1 = st1 + d1.Duration;
					if (lt1 < st2) {
						found = true;
						break;
					}
				}
				if (!found) {
					d3.Remove(st1);
					if (d3.Size() == 0) {
						return false;
					}
				}
			}
			//v1.UpdateDomain(d3, trail);
			return true;
		}

		/// <summary>
		/// Satisfies the startedby.
		/// </summary>
		/// <param name="v1">The v1.</param>
		/// <param name="v2">The v2.</param>
		/// <param name="trail">The trail.</param>
		/// <returns></returns>
		private static bool SatisfyStartedby(Variable v1, Variable v2, Trail trail) {
			var d1 = (AllenDomain)v1.Domain;
			if (d1.Empty) return false;
			var d3 = (AllenDomain)d1.Clone();
			var d2 = (AllenDomain)v2.Domain;
			if (d2.Empty) return false;
			for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step) {
				bool found = false;
				for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step) {
					int lt1 = st1 + d1.Duration;
					int lt2 = st2 + d2.Duration;
					if ((st1 == st2) && (lt1 > lt2)) {
						found = true;
						break;
					}
				}
				if (!found) {
					d3.Remove(st1);
					if (d3.Size() == 0) {
						return false;
					}
				}
			}
			//v1.UpdateDomain(d3, trail);
			return true;
		}

		/// <summary>
		/// Satisfies the starts.
		/// </summary>
		/// <param name="v1">The v1.</param>
		/// <param name="v2">The v2.</param>
		/// <param name="trail">The trail.</param>
		/// <returns></returns>
		private static bool SatisfyStarts(Variable v1, Variable v2, Trail trail) {
			var d1 = (AllenDomain)v1.Domain;
			if (d1.Empty) return false;
			var d3 = (AllenDomain)d1.Clone();
			var d2 = (AllenDomain)v2.Domain;
			if (d2.Empty) return false;
			for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step) {
				bool found = false;
				for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step) {
					int lt1 = st1 + d1.Duration;
					int lt2 = st2 + d2.Duration;
					if ((st1 == st2) && (lt1 < lt2)) {
						found = true;
						break;
					}
				}
				if (!found) {
					d3.Remove(st1);
					if (d3.Size() == 0) {
						return false;
					}
				}
			}
			//v1.UpdateDomain(d3, trail);
			return true;
		}

		#endregion

	}
}