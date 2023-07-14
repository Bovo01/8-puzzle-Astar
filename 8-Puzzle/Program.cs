using Astar;

internal class Program
{
    static void Main()
    {
        Console.WriteLine("Generated:");
        EightPuzzle puzzle = EightPuzzle.GeneratePuzzle();
        puzzle.PrintPuzzle();
        Console.WriteLine();

        Solver solver = new Solver(puzzle);
        State solution = solver.Solve();
        solution.Report();
    }
}