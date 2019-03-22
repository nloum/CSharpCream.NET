using System.IO;
using Cream;
using Cream.CourseAssignment;
using System;

namespace ProfAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            CourseNetwork net = new CourseNetwork();
            StreamReader re = File.OpenText("courses.txt");//"Anum1.txt");//"fn1.txt"); //"numfile.txt");
            string input = re.ReadLine();
            int n;
            try
            {
                n = Convert.ToInt16(input);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to read no of  profs " );
                return;  
            }
            Professor[] professors = new Professor[n];
            for (int i=0;i<n;i++)
            {
                input = re.ReadLine();
                int m;
                string[] s = input.Split(' ');

                try
                {
                     m = Convert.ToInt16(s[1]);   // no of events
                }
                catch
                {
                    Console.WriteLine("Failed to read no of courses per prof "+s[0]);
                    return;

                } 
                professors[i] = new Professor(net, m, s[0]);   
            }
            


            IntVariable[] course = new IntVariable[6];
            int max = net.Professors.Count - 1;
            course[0] = new IntVariable(net, 0, max, "CS110");
            course[1] = new IntVariable(net, 0, max, "CS200");
            course[2] = new IntVariable(net, 0, max, "CS420");
            course[3] = new IntVariable(net, 0, max, "CS310");
            course[4] = new IntVariable(net, 0, max, "MA200");
            course[5] = new IntVariable(net, 0, max, "ST101");
            course[0].NotEquals(course[1]);
            course[2].NotEquals(course[3]);
            course[4].NotEquals(course[5]);
            course[4].Equals(1);
            re.Close();
            Professor dummyPorf = professors[net.Professors.Count - 1];
            dummyPorf.Courses = 0;    // always initialize with 0
            Count cc = new Count(net, course);
            

            int sum = 0;
            if (net.Professors != null)
            {
                foreach (Professor p in net.Professors)
                {
                    sum += p.Courses;
                }
            }
            if(sum < net.Variables.Count)
            {
                dummyPorf.Courses = net.Variables.Count - sum;
            }
            //net.Objective = course[5];
            //int opt = Solver.BETTER;
            Solver solver = new DefaultSolver(net);
            long timer = DateTime.Now.Ticks;
            int count = 0;
            for (solver.Start(); solver.WaitNext(); solver.Resume())
            {
                Solution sol = solver.Solution;
                for (int i = 0; i < net.Professors.Count; i++)
                {
                    int pcount = 0;
                    foreach (Variable v in net.Variables)
                    {
                        if (sol.GetIntValue(v) == i)
                        {
                            pcount++;
                        }
                    }
                    if (pcount < ((Professor)net.Professors[i]).Courses)
                    {
                        Console.WriteLine("Prof. " + ((Professor)net.Professors[i]).ToString() + " not consistent.. needs " + 
                            (((Professor)net.Professors[i]).Courses - pcount) + " assignment(s) more!!");
                    }
                }

                Console.Out.WriteLine();
                foreach (Variable v in net.Variables)
                {
                    Console.Out.WriteLine(v.Name + " = " + ((Professor) net.Professors[sol.GetIntValue(v)]).Name);
                }
                Console.Out.WriteLine("=================================");
                count++;
            }
            timer = DateTime.Now.Ticks - timer;
            Console.Out.WriteLine("timer: " + timer );
            Console.WriteLine("Count=" + count);
            Console.ReadLine();
        }


    }
}
