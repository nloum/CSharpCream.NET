using System;
using System.Collections;

namespace Cream.AllenTemporal {

	/// <summary>
	/// 
	/// </summary>
	public class AllenDomain : Domain {
		#region Internal Classes

		/// <summary>
		/// 
		/// </summary>
		private class AllenDomainIterator : IEnumerator {

			#region Fields


			private int mi, ma, d, s, choice;

			#endregion

			#region Constructors

			/// <summary>
			/// Initializes a new instance of the <see cref="AllenDomainIterator"/> class.
			/// </summary>
			/// <param name="enclosingInstance">The enclosing instance.</param>
			public AllenDomainIterator(AllenDomain enclosingInstance) {
				InitBlock(enclosingInstance);
			}

			#endregion

			#region Properties

			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			/// <value></value>
			/// <returns>
			/// The current element in the collection.
			/// </returns>
			/// <exception cref="T:System.InvalidOperationException">
			/// The enumerator is positioned before the first element of the collection or after the last element.
			/// </exception>
			public virtual Object Current {
				get {
					//if (!MoveNext())
					//	throw new ArgumentOutOfRangeException();
					var dom = new AllenDomain(choice, choice + d, d, s);  //ma+d
					//choice+=s;
					return dom;
				}

			}

			/// <summary>
			/// Gets the enclosing_ instance.
			/// </summary>
			/// <value>The enclosing instance.</value>
			public AllenDomain EnclosingInstance { get; private set; }

			#endregion

			#region Public Methods

			/// <summary>
			/// Advances the enumerator to the next element of the collection.
			/// </summary>
			/// <returns>
			/// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
			/// </returns>
			/// <exception cref="T:System.InvalidOperationException">
			/// The collection was modified after the enumerator was created.
			/// </exception>
			public virtual bool MoveNext() {
				if (EnclosingInstance.sizeField == 0)
					return false;
				while (choice <= EnclosingInstance.maxField) {
					if (EnclosingInstance.CONTAINS(choice))
						return true;
					choice += s;
				}
				return false;
			}

			/// <summary>
			/// Removes this instance.
			/// </summary>
			/// <exception cref="NotSupportedException"><c>NotSupportedException</c>.</exception>
			public virtual void Remove() {
				throw new NotSupportedException();
			}

			/// <summary>
			/// Sets the enumerator to its initial position, which is before the first element in the collection.
			/// </summary>
			/// <exception cref="T:System.InvalidOperationException">
			/// The collection was modified after the enumerator was created.
			/// </exception>
			virtual public void Reset() {
			}

			#endregion

			#region Private Methods

			/// <summary>
			/// Inits the block.
			/// </summary>
			/// <param name="eI">The e I.</param>
			private void InitBlock(AllenDomain eI) {
				EnclosingInstance = eI;
				mi = EnclosingInstance.minField;
				choice = mi;
				ma = EnclosingInstance.maxField;
				d = EnclosingInstance.Duration;
				s = EnclosingInstance.step;

			}

			#endregion

		}

		#endregion

		#region Public Fields

		public const int MinValue = unchecked((int)0xc0000001);
		public const int MaxValue = 0x3fffffff;
		public static AllenDomain EMPTY = new AllenDomain();
		public static AllenDomain FULL = new AllenDomain(MinValue, MaxValue);

		#endregion

		#region Private Fields

		private ArrayList intervals = new ArrayList();
		private int step;
		private int duration;
		private int minField;
		private int maxField;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="AllenDomain"/> class.
		/// </summary>
		public AllenDomain() {
		}

		/// <summary>
		/// AllenDomain Constructor with a starting time and latest time only
		/// The duration and step here are assumed 1
		/// </summary>
		/// <param name="st">starting time</param>
		/// <param name="lt">latest time</param>
		public AllenDomain(int st, int lt)
			: this(st, lt, 1, 1) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AllenDomain"/> class.
		/// </summary>
		/// <param name="st">The st.</param>
		/// <param name="lt">The lt.</param>
		/// <param name="duration">The duration.</param>
		public AllenDomain(int st, int lt, int duration)
			: this(st, lt, duration, 1) {

		}

		/// <summary>
		/// AllenDomain Constructor with a starting time, latest time, duration and a step
		/// </summary>
		/// <param name="st">starting time</param>
		/// <param name="lt">latest time</param>
		/// <param name="duration">duration</param>
		/// <param name="step">step</param>
		/// <exception cref="NotSupportedException">Maximum time must be greater than or equal minimum time!!!</exception>
		public AllenDomain(int st, int lt, int duration, int step) {
			int mi = Math.Max(st, MinValue);
			int ma = lt - duration;
			ma = Math.Min(ma, MaxValue);
			if (mi <= ma) {
				int[] interval;
				if (step == 1) {
					interval = new[] { mi, ma };
					intervals.Add(interval);
				} else {
					for (int i = mi; i <= ma; i += step) {
						interval = new[] { i, i };
						intervals.Add(interval);
					}
				}
				sizeField = ma - mi + 1;
				minField = mi;
				maxField = ma;
				this.step = step;
				this.duration = duration;
				UpdateSize();
			} else {
				throw new NotSupportedException("Maximum time must be greater than or equal minimum time!!!");
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the duration.
		/// </summary>
		/// <value>The duration.</value>
		public int Duration {
			get {
				return duration;
			}
		}

		/// <summary>
		/// Gets the step.
		/// </summary>
		/// <value>The step.</value>
		public int Step {
			get {
				return step;
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Updates the size.
		/// </summary>
		private void UpdateSize() {
			sizeField = 0;
			try {
				for (int i = 0; i < intervals.Count; ++i) {
					var interval = (int[])intervals[i];
					sizeField += interval[1] - interval[0] + 1;
				}
			} catch (IndexOutOfRangeException) {
			}
		}

		/// <summary>
		/// Updates the min max.
		/// </summary>
		private void UpdateMinMax() {
			if (sizeField > 0) {
				try {
					var interval = (int[])intervals[0];
					minField = interval[0];
					interval = (int[])intervals[intervals.Count - 1];
					maxField = interval[1];
				} catch (IndexOutOfRangeException) {
				}
			}
		}

		/// <summary>
		/// Indexes the of.
		/// </summary>
		/// <param name="elem">The elem.</param>
		/// <returns></returns>
		private int IndexOf(int elem) {
			if (elem < minField || maxField < elem)
				return -1;
			try {
				for (int i = 0; i < intervals.Count; i++) {
					var interval = (int[])intervals[i];
					if (elem < interval[0])
						return -1;
					if (elem <= interval[1])
						return i;
				}
			} catch (IndexOutOfRangeException) {
			}
			return -1;
		}

		/// <summary>
		/// Indexes the of.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <returns></returns>
		public int IndexOf(Object o) {
			if (!(o is ValueType))
				return -1;
			return IndexOf(Convert.ToInt32(o));
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Equalses the specified d0.
		/// </summary>
		/// <param name="d0">The d0.</param>
		/// <returns></returns>
		public override bool Equals(Domain d0) {
			if (this == d0)
				return true;
			if (!(d0 is AllenDomain))
				return false;
			var d = (AllenDomain)d0;
			if (intervals.Count != d.intervals.Count)
				return false;
			try {
				for (int i = 0; i < intervals.Count; i++) {
					var i0 = (int[])intervals[i];
					var i1 = (int[])d.intervals[i];
					if (i0[0] != i1[0] || i0[1] != i1[1])
						return false;
				}
			} catch (IndexOutOfRangeException) {
			}
			return true;
		}

		/// <summary>
		/// Clones this instance.
		/// </summary>
		/// <returns></returns>
		public override Object Clone() {
			var d = new AllenDomain();
			try {
				for (int i = 0; i < intervals.Count; ++i) {
					d.intervals.Add(((int[])intervals[i]).Clone());
				}
			} catch (IndexOutOfRangeException) {
			}
			d.sizeField = sizeField;
			d.minField = minField;
			d.maxField = maxField;
			d.step = step;
			d.duration = duration;
			return d;
		}

		/// <summary>
		/// Elementses this instance.
		/// </summary>
		/// <returns></returns>
		public override IEnumerator Elements() {
			IEnumerator iter = new AllenDomainIterator(this);
			return iter;
		}

		/// <summary>
		/// Mins this instance.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException"><c></c> is out of range.</exception>
		public virtual int Min() {
			if (sizeField == 0)
				throw new ArgumentOutOfRangeException();
			return minField;
		}

		/// <summary>
		/// Maxes this instance.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException"><c></c> is out of range.</exception>
		public virtual int Max() {
			if (sizeField == 0)
				throw new ArgumentOutOfRangeException();
			return maxField;
		}

		/// <summary>
		/// Values this instance.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException"><c></c> is out of range.</exception>
		public virtual int Value() {
			if (sizeField != 1)
				throw new ArgumentOutOfRangeException();
			return minField;
		}

		/// <summary>
		/// Elements this instance.
		/// </summary>
		/// <returns></returns>
		public override Object Element() {
			return Value();
		}

		/// <summary>
		/// CONTAINSs the specified elem.
		/// </summary>
		/// <param name="elem">The elem.</param>
		/// <returns></returns>
		public virtual bool CONTAINS(int elem) {
			return IndexOf(elem) >= 0;
		}

		/// <summary>
		/// Determines whether [contains] [the specified o].
		/// </summary>
		/// <param name="o">The o.</param>
		/// <returns>
		/// 	<c>true</c> if [contains] [the specified o]; otherwise, <c>false</c>.
		/// </returns>
		public override bool Contains(Object o) {
			if (!(o is ValueType))
				return false;
			return CONTAINS(Convert.ToInt32(o));
		}

		/// <summary>
		/// Inserts the specified o.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
		public override Domain Insert(Object o) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes the specified elem.
		/// </summary>
		/// <param name="elem">The elem.</param>
		public virtual void Remove(int elem) {
			int i = IndexOf(elem);
			if (i < 0)
				return;
			try {
				var interval = (int[])intervals[i];
				int lo = interval[0];
				int hi = interval[1];
				if (elem == lo && elem == hi) {
					intervals.RemoveAt(i);
				} else if (elem == lo) {
					interval[0] = lo + 1;
				} else if (elem == hi) {
					interval[1] = hi - 1;
				} else {
					interval[0] = elem + 1;
					intervals.Insert(i, new[] { lo, elem - 1 });
				}
				sizeField--;
				if (sizeField > 0) {
					UpdateMinMax();
				}
			} catch (IndexOutOfRangeException) {
			}
		}

		/// <summary>
		/// Removes the specified o.
		/// </summary>
		/// <param name="o">The o.</param>
		public virtual void Remove(Object o) {
			if (!(o is ValueType))
				return;
			Remove(Convert.ToInt32(o));
		}

		/// <summary>
		/// Deletes the specified elem.
		/// </summary>
		/// <param name="elem">The elem.</param>
		/// <returns></returns>
		public virtual AllenDomain Delete(int elem) {
			if (!CONTAINS(elem))
				return this;
			var d = (AllenDomain)Clone();
			d.Remove(elem);
			return d;
		}

		/// <summary>
		/// Deletes the specified o.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <returns></returns>
		public override Domain Delete(Object o) {
			if (!(o is ValueType))
				return this;
			return Delete(Convert.ToInt32(o));
		}

		/// <summary>
		/// Deletes the specified lo.
		/// </summary>
		/// <param name="lo">The lo.</param>
		/// <param name="hi">The hi.</param>
		/// <returns></returns>
		public virtual AllenDomain Delete(int lo, int hi) {
			if (sizeField == 0 || lo > hi || hi < minField || maxField < lo)
				return this;
			if (lo == hi)
				return Delete(lo);
			var d = new AllenDomain();
			try {
				for (int i = 0; i < intervals.Count; i++) {
					var interval = (int[])intervals[i];
					int mi = Math.Max(lo, interval[0]);
					int ma = Math.Min(hi, interval[1]);
					if (mi <= ma) {
						if (interval[0] < mi) {
							var inv = new int[2];
							inv[0] = interval[0];
							inv[1] = mi - 1;
							d.intervals.Add(inv);
						}
						if (ma < interval[1]) {
							var inv = new int[2];
							inv[0] = ma + 1;
							inv[1] = interval[1];
							d.intervals.Add(inv);
						}
					} else {
						var inv = new int[2];
						inv[0] = interval[0];
						inv[1] = interval[1];
						d.intervals.Add(inv);
					}
				}
			} catch (IndexOutOfRangeException) {
			}
			d.UpdateSize();
			d.UpdateMinMax();
			return d;
		}

		/// <summary>
		/// Caps the specified d.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <returns></returns>
		public override Domain Cap(Domain d) {
			if (!(d is AllenDomain))
				return EMPTY;
			var newD = new AllenDomain();
			AllenDomain d0 = this;
			var d1 = (AllenDomain)d;
			try {
				int i0 = 0;
				int i1 = 0;
				while (i0 < d0.intervals.Count && i1 < d1.intervals.Count) {
					var interval = (int[])d0.intervals[i0];
					int min0 = interval[0];
					int max0 = interval[1];
					interval = (int[])d1.intervals[i1];
					int min1 = interval[0];
					int max1 = interval[1];
					if (max0 < min1) {
						i0++;
						continue;
					}
					if (max1 < min0) {
						i1++;
						continue;
					}
					interval = new int[2];
					interval[0] = Math.Max(min0, min1);
					interval[1] = Math.Min(max0, max1);
					newD.intervals.Add(interval);
					if (max0 <= max1)
						i0++;
					if (max1 <= max0)
						i1++;
				}
			} catch (IndexOutOfRangeException) {
			}
			newD.UpdateSize();
			newD.UpdateMinMax();
			if (newD.Empty)
				return EMPTY;
			return newD;
		}

		/// <summary>
		/// Cups the specified d.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
		public override Domain Cup(Domain d) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Differences the specified d.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
		public override Domain Difference(Domain d) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Caps the interval.
		/// </summary>
		/// <param name="lo">The lo.</param>
		/// <param name="hi">The hi.</param>
		/// <returns></returns>
		public virtual AllenDomain CapInterval(int lo, int hi) {
			AllenDomain d = this;
			if (MinValue < lo)
				d = d.Delete(MinValue, lo - 1);
			if (hi < MaxValue)
				d = d.Delete(hi + 1, MaxValue);
			return d;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		private static String ToString(int x) {
			if (x == MinValue) {
				return "Min";
			}
			if (x == MaxValue) {
				return "Max";
			}
			return Convert.ToString(x);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override String ToString() {
			String s = string.Empty;
			String delim = string.Empty;
			try {
				for (int i = 0; i < intervals.Count; i++) {
					var interval = (int[])intervals[i];
					s += (delim + ToString(interval[0]));
					if (interval[0] < interval[1]) {
						s += ("-" + ToString(interval[1]));
					}
					delim = ",";
				}
			} catch (IndexOutOfRangeException) {
			}
			if (Size() == 1) {
				return s;
			}
			return "{" + s + "}";
		}

		#endregion
	}
}