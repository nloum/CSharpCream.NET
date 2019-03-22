/*
* @(#)CSharpCreamSolver.cs
*/
using System;
using Cream;

public class CSharpCreamSolver
{
    [STAThread]
    public static void Main(String[] args)
    {
        Network net = new Network();
        IntVariable J = new IntVariable(net, 0, 9);
        IntVariable A = new IntVariable(net, 0, 9);
        IntVariable V = new IntVariable(net, 0, 9);
        IntVariable C = new IntVariable(net, 0, 9);
        IntVariable R = new IntVariable(net, 0, 9);
        IntVariable E = new IntVariable(net, 0, 9);
        IntVariable M = new IntVariable(net, 0, 9);
        IntVariable S = new IntVariable(net, 0, 9);
        IntVariable O = new IntVariable(net, 0, 9);
        IntVariable L = new IntVariable(net, 0, 9);
        new NotEquals(net, new IntVariable[] { J, A, V, C, R, E, M, S, O, L });
        J.NotEquals(0);
        C.NotEquals(0);
        S.NotEquals(0);
        IntVariable JAVA = J.Multiply(1000).Add(A.Multiply(100)).Add(V.Multiply(10)).Add(A);
        IntVariable CREAM = C.Multiply(10000).Add(R.Multiply(1000)).Add(E.Multiply(100)).Add(A.Multiply(10)).Add(M);
        IntVariable SOLVER = S.Multiply(100000).Add(O.Multiply(10000)).Add(L.Multiply(1000)).Add(V.Multiply(100)).Add(E.Multiply(10)).Add(R);
        JAVA.Add(CREAM).Equals(SOLVER);
        Solver solver = new DefaultSolver(net);
        for (solver.Start(); solver.WaitNext(); solver.Resume())
        {
            Solution solution = solver.Solution;
            Console.Out.WriteLine(solution.GetIntValue(JAVA) + " + " + solution.GetIntValue(CREAM) + " = " + solution.GetIntValue(SOLVER));
        }
        solver.Stop();
        Console.ReadLine();
    }
}