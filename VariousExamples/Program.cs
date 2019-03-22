/*
* @(#)Examples.java
*/
using System;
using Cream;

namespace VariousExamples
{
    /// <summary> Various example programs.
    /// 
    /// </summary>
    /// <author>  Naoyuki Tamura (tamura@kobe-u.ac.jp)
    /// </author>
    public class Examples
    {

        internal static void runExample(Network net, int opt)
        {
            Console.Out.WriteLine("# Problem");
            //UPGRADE_TODO: Method 'java.io.PrintStream.println' was converted to 'System.Console.Out.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintStreamprintln_javalangObject'"
            Console.Out.WriteLine(net);
            Console.Out.WriteLine("# Solutions");
            Solver solver = new DefaultSolver(net, opt);
            for (solver.Start(); solver.WaitNext(); solver.Resume())
            {
                Solution solution = solver.Solution;
                Console.Out.WriteLine(solution);
            }
            solver.Stop();
            long count = solver.GetCount();
            long time = solver.GetElapsedTime();
            Console.Out.WriteLine("Found " + count + " solutions in " + time + " milli seconds");
            Console.Out.WriteLine();
        }

        internal static void maxExample()
        {
            Network net = new Network();
            IntVariable x = new IntVariable(net, -1, 1, "x");
            IntVariable y = new IntVariable(net, -1, 1, "y");
            IntVariable z = x.Max(y);
            z.Name ="z";
            runExample(net, Solver.Default);
        }

        internal static void absExample()
        {
            Network net = new Network();
            IntVariable x = new IntVariable(net, -3, 2, "x");
            IntVariable y = x.Abs();
            y.NotEquals(2);
            y.Name ="y";
            runExample(net, Solver.Default);
        }

        internal static void signExample()
        {
            Network net = new Network();
            IntVariable x = new IntVariable(net, -2, 2, "x");
            IntVariable y = x.Sign();
            y.Name ="y";
            runExample(net, Solver.Default);
        }

        internal static void minimizeExample()
        {
            Network net = new Network();
            IntVariable x = new IntVariable(net, 1, 10, "x");
            IntVariable y = new IntVariable(net, 1, 10, "y");
            // x + y >= 10
            x.Add(y).Ge(10);
            // z = max(x, y)
            IntVariable z = x.Max(y);
            z.Name ="z";
            // minimize z
            net.Objective = z;
            runExample(net, Solver.Minimize | Solver.Better);
        }

        internal static void elementExample()
        {
            Network net = new Network();
            IntVariable x = new IntVariable(net, "x");
            IntVariable i = new IntVariable(net, "i");
            int n = 4;
            IntVariable[] v = new IntVariable[n];
            Random ran = new Random(1000);
            for (int j = 0; j < n; j++)
            {
                //v[j] = new IntVariable(net, ran.Next(200));
                v[j] = new IntVariable(net, 10 * (j + 1)+2);
            }
            new Element(net, x, i, v);
            runExample(net, Solver.Default);
        }

        internal static void relationExample()
        {
            Network net = new Network();
            IntVariable x = new IntVariable(net, "x");
            IntVariable y = new IntVariable(net, "y");
            bool[][] rel = new bool[][] { new bool[] { false, true, false, false }, new bool[] { false, false, false, true }, new bool[] { true, false, false, false }, new bool[] { false, false, true, false } };
            new Relation(net, x, rel, y);
            runExample(net, Solver.Default);
        }

        [STAThread]
        public static void Main(String[] args)
        {
            Console.WriteLine("Max Example");
            Console.WriteLine("-----------");
            maxExample();
            Console.ReadLine();
            Console.WriteLine("Abs Example");
            Console.WriteLine("-----------");
            absExample();
            Console.ReadLine();
            Console.WriteLine("Sign Example");
            Console.WriteLine("-----------");
            signExample();
            Console.ReadLine();
            Console.WriteLine("Minimize Example");
            Console.WriteLine("-----------");
            minimizeExample();
            Console.ReadLine();
            Console.WriteLine("Element Example");
            Console.WriteLine("-----------");
            elementExample();
            Console.ReadLine();
            Console.WriteLine("Relation Example");
            Console.WriteLine("-----------");
            relationExample();
            Console.ReadLine();
        }
    }
}