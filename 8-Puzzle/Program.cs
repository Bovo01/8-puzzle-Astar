using Astar;
using System.Diagnostics;

internal class Program
{
    static void Main()
    {
        if (Goal.GOAL_INVERTIONS_PARITY != 0)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Goal can't be solved with a standard grid\n");
            Console.ForegroundColor = defaultColor;
        }
        Console.WriteLine("Generated:");
        EightPuzzle puzzle = EightPuzzle.GeneratePuzzle();
        puzzle.PrintPuzzle();
        Console.WriteLine();

        Solver solver = new Solver(puzzle);
        Stopwatch sw = new Stopwatch();
        sw.Start();
        State solution = solver.Solve();
        sw.Stop();
        Console.WriteLine("Solved in {0}s\n", sw.ElapsedMilliseconds / 1e3);
        solution.Report();
    }
}