using System;
using Cream;
namespace SevenEleven
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Network net = new Network();

            IntVariable X = new IntVariable(net, 0, 708);
            IntVariable Y = new IntVariable(net, 0, 708);
            IntVariable Z = new IntVariable(net, 0, 708);
            IntVariable T = new IntVariable(net, 0, 708);
            X.Add(Y).Add(Z).Add(T).Equals(711);
            X.Ge(Y);
            Y.Ge(Z);
            Z.Ge(T);
            X.Multiply(Y).Multiply(Z).Multiply(T).Equals(711000000);
            Solver solver = new DefaultSolver(net);
            for (solver.Start(); solver.WaitNext(); solver.Resume())
            {
                Solution solution = solver.Solution;
                Console.Out.WriteLine();
                Console.Out.WriteLine(" {0:F} + {1:F} + {2:F} + {3:F} = {4:F} ", 
                                      solution.GetIntValue(X)/100.0, solution.GetIntValue(Y)/100.0, 
                                      solution.GetIntValue(Z)/100.0, solution.GetIntValue(T)/100.0,7.11 );
            }
            solver.Stop();
            Console.ReadLine();
        }
    }
}
