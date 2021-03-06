﻿/*
* @(#)FT06.cs
* Job-shop scheduling benchmark problem FT06
*/
using System;
using Cream;

public class FT06
{
    private static void ft06(Network net)
    {
        int n = 37;
        IntVariable[] v = new IntVariable[n];
        for (int i = 0; i < v.Length; i++)
        {
            v[i] = new IntVariable(net, 0, IntDomain.MaxValue);
        }
        IntVariable[][] jobs = new IntVariable[][] { new IntVariable[] { v[1], v[2], v[3], v[4], v[5], v[6], v[0] }, 
                                                     new IntVariable[] { v[7], v[8], v[9], v[10], v[11], v[12], v[0] }, 
                                                     new IntVariable[] { v[13], v[14], v[15], v[16], v[17], v[18], v[0] }, 
                                                     new IntVariable[] { v[19], v[20], v[21], v[22], v[23], v[24], v[0] }, 
                                                     new IntVariable[] { v[25], v[26], v[27], v[28], v[29], v[30], v[0] }, 
                                                     new IntVariable[] { v[31], v[32], v[33], v[34], v[35], v[36], v[0] } };
        int[][] jobs_pt = new int[][] { new int[] { 1, 3, 6, 7, 3, 6 }, 
                                        new int[] { 8, 5, 10, 10, 10, 4 }, 
                                        new int[] { 5, 4, 8, 9, 1, 7 }, 
                                        new int[] { 5, 5, 5, 3, 8, 9 }, 
                                        new int[] { 9, 3, 5, 4, 3, 1 }, 
                                        new int[] { 3, 3, 9, 10, 4, 1 } };
        IntVariable[][] machines = new IntVariable[][] { new IntVariable[] { v[2], v[11], v[16], v[20], v[29], v[34] }, 
                                                         new IntVariable[] { v[3], v[7], v[17], v[19], v[26], v[31] }, 
                                                         new IntVariable[] { v[1], v[8], v[13], v[21], v[25], v[36] }, 
                                                         new IntVariable[] { v[4], v[12], v[14], v[22], v[30], v[32] }, 
                                                         new IntVariable[] { v[6], v[9], v[18], v[23], v[27], v[35] }, 
                                                         new IntVariable[] { v[5], v[10], v[15], v[24], v[28], v[33] } };
        int[][] machines_pt = new int[][] { new int[] { 3, 10, 9, 5, 3, 10 }, 
                                            new int[] { 6, 8, 1, 5, 3, 3 }, 
                                            new int[] { 1, 5, 5, 5, 9, 1 }, 
                                            new int[] { 7, 4, 4, 3, 1, 3 }, 
                                            new int[] { 6, 10, 7, 8, 5, 4 }, 
                                            new int[] { 3, 10, 8, 9, 4, 9 } };
        for (int j = 0; j < jobs.Length; j++)
        {
            new Sequential(net, jobs[j], jobs_pt[j]);
        }
        for (int m = 0; m < machines.Length; m++)
        {
            new Serialized(net, machines[m], machines_pt[m]);
        }
        net.Objective = v[0];
    }

    [STAThread]
    public static void Main(String[] args)
    {
        Network net = new Network();
        ft06(net);

        String solverName = "ibb";
        int opt = Solver.Minimize;
        long timeout = 180000;
        if (args.Length >= 1)
        {
            solverName = args[0];
        }

        Solver solver;
        if (solverName.Equals("bb"))
        {
            solver = new DefaultSolver(net, opt, "bb");
        }
        else if (solverName.Equals("random"))
        {
            solver = new LocalSearch(net, opt, "rs");
        }
        else if (solverName.Equals("sa"))
        {
            solver = new SimulatedAnneallingSearch(net, opt, "sa");
        }
        else if (solverName.Equals("ibb"))
        {
            solver = new IterativeBranchAndBoundSearch(net, opt, "ibb");
        }
        else if (solverName.Equals("taboo"))
        {
            solver = new TabooSearch(net, opt, "taboo");
        }
        else
        {
            Solver sa = new SimulatedAnneallingSearch((Network)net.Clone(), opt, "sa");
            Solver ibb = new IterativeBranchAndBoundSearch((Network)net.Clone(), opt, "ibb");
            solver = new ParallelSolver(new Solver[] { sa, ibb });
        }
    	solver.SolverStrategy = Solver.StrategyMethod.Bisect;
        //Cream.Monitor monitor = new Monitor();
        //monitor.setX(0, (int)(timeout / 1000));
        //solver.setMonitor(monitor);

        Console.Out.WriteLine("Start " + solver + ", timeout = " + timeout + " msecs");

        Solution bestSolution;
        int c = 0;
        if (true)
        {
            for (solver.Start(timeout); solver.WaitNext(); solver.Resume())
            {
                Solution solution = solver.Solution;
                Console.Out.WriteLine(++c);
                Console.Out.WriteLine(solution);
                int value_Renamed = solution.ObjectiveIntValue;
                Console.Out.WriteLine(value_Renamed);
                Console.Out.WriteLine("=======================");
            }
            solver.Stop();
            bestSolution = solver.BestSolution;
        }
        else
        {
            bestSolution = solver.FindBest(timeout);
        }

        Console.Out.WriteLine("Best = " + bestSolution.ObjectiveIntValue);
        Console.Out.WriteLine("Best = " + bestSolution);
        Console.In.ReadLine();
    }
}