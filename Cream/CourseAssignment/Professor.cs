using System;

namespace Cream.CourseAssignment
{
    public class Professor
    {
        private int index = -1;
        private static int Count = 1;

        protected internal Network Network {get; set;}

        public String Name {get; set;}

        public int Courses { get; set; }
        
        public int RealNoOfCourses { get; set; }
        
        protected internal int Index
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

        public Professor(Network net): this(net, 0, null)
        {
            
        }

        public Professor(Network net, int courses)
            : this(net, courses, null)
        {
            
        }

        public Professor(Network net, int courses, string name)
        {
            Network = net;
            Courses = courses;
            RealNoOfCourses = courses;
            Name = name ?? "Prof" + (Count++);
            ((CourseNetwork)Network).Add(this);
        }

        public override string ToString()
        {
            return Name;
            
        }

        protected internal static String ToString(Professor[] pc)
        {
            String s = "";
            if (pc != null)
            {
                String delim = "";
                for (int i = 0; i < pc.Length; i++)
                {
                    s += (delim + pc[i]);
                    delim = ",";
                }
            }
            return "{" + s + "}";
        }
    }
}
