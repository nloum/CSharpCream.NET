using System;

namespace Cream.CourseAssignment
{
    public class CourseNetwork : Network
    {
        
        private System.Collections.IList professors;

        public CourseNetwork()
		{
			Professors = new System.Collections.ArrayList();
		}
        public System.Collections.IList Professors
        {
            get
            {
                return professors;
            }
            set
            {
                professors = value;
            }
        }
        /// <summary> Adds a profCourse to this network.
        /// If the profCourse is already in the nework, this invocation has no effect.
        /// </summary>
        /// <param name="pc">the profCourse to be added
        /// </param>
        /// <returns> the variable itself
        /// </returns>
        /// <throws>  NullPointerException if <tt>v</tt> is <tt>null</tt> </throws>
        /// <throws>  IllegalArgumentException if <tt>v</tt> is already added to another network </throws>
        protected internal virtual Professor Add(Professor pc)
        {
            if (!professors.Contains(pc))
            {
                if (pc.Index >= 0)
                {
                    throw new ArgumentException();
                }
                pc.Index = professors.Count;
                professors.Add(pc);
            }
            return pc;
        }

		
    }
}
