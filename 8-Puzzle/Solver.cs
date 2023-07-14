// https://github.com/BlueRaja/High-Speed-Priority-Queue-for-C-Sharp
// Installazione : https://github.com/BlueRaja/High-Speed-Priority-Queue-for-C-Sharp/wiki/Getting-Started
using Priority_Queue;

namespace Astar
{
    internal class State : FastPriorityQueueNode
    {
        internal static readonly int[,] GOAL =
        {
            {1, 2, 3 },
            {4, 5, 6 },
            {7, 8, 0 },
        };
        /*internal static readonly int[,] GOAL =
        {
            {1, 2, 3, 4 },
            {5, 6, 7, 8 },
            {9, 10, 11, 12 },
            {13, 14, 15, 0 }
        };*/
        internal static readonly int GOAL_INVERTIONS_PARITY = EightPuzzle.GetInvertions(GOAL) % 2;
        static Position[]? GOAL_POSITIONS;

        internal EightPuzzle puzzle;
        internal int currCost;
        internal List<Action> moves;

        public State(EightPuzzle puzzle, int cost)
        {
            this.puzzle = puzzle;
            this.currCost = cost;
            this.moves = new List<Action>();
        }
        public State(EightPuzzle puzzle, int cost, List<Action> moves)
        {
            this.puzzle = puzzle;
            this.currCost = cost;
            this.moves = moves;
        }
        int Heuristic()
        {
            return puzzle.ManhattanDistance();
        }
        internal int Cost()
        {
            return currCost + Heuristic(); // A*
        }
        internal static Position GetGoalPosition(int val)
        {
            if (GOAL_POSITIONS == null)
            {
                // Genero l'array
                GOAL_POSITIONS = new Position[GOAL.Length];
                for (int i = 0; i < GOAL.GetLength(0); i++)
                {
                    for (int j = 0; j < GOAL.GetLength(1); j++)
                    {
                        GOAL_POSITIONS[GOAL[i, j]] = new Position(i, j);
                    }
                }
            }
            return GOAL_POSITIONS[val];
        }
        internal List<Action> GetActions()
        {
            return puzzle.GetActions();
        }
        internal State NextState(Action action)
        {
            EightPuzzle newPuzzle = new EightPuzzle(puzzle);
            newPuzzle.ExecAction(action);
            List<Action> moves = new List<Action>(this.moves) { action };
            return new State(newPuzzle, currCost + 1, moves);
        }
        internal bool IsGoal()
        {
            return GOAL.Cast<int>().SequenceEqual(puzzle.grid.Cast<int>());
        }
        public void Report()
        {
            Console.WriteLine("Solved:");
            puzzle.PrintPuzzle();
            Console.WriteLine("\nTotal moves: " + currCost);
            Console.Write("\nMoves to reach solution:\n");
            Console.WriteLine( string.Join(',', moves) );
        }
    }

    internal class Solver
    {
        FastPriorityQueue<State> frontier;

        public Solver(EightPuzzle puzzle)
        {
            frontier = new FastPriorityQueue<State>(100_000_000); // La PriorityQueue non si ridimensiona, quindi imposto una dimensione grande per evitare problemi
            frontier.Enqueue(new State(puzzle, 0), 0); // 0 perché è il primo, quindi la priorità non è rilevante
        }
        public Solver(int[,] grid) : this(new EightPuzzle(grid)) { }

        public State Solve()
        {
            State s = frontier.Dequeue(), s2;
            while (!s.IsGoal())
            {
                foreach (Action a in s.GetActions())
                {
                    s2 = s.NextState(a);
                    frontier.Enqueue(s2, s2.Cost());
                }
                s = frontier.Dequeue();
            }
            return s;
        }
    }
}
