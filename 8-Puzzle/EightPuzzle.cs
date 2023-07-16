using System;

namespace Astar
{
    class EightPuzzle
    {
        public int[,] grid { get; }
        private Position zeroPos;

        public EightPuzzle(int[,] grid)
        {
            if (grid.GetLength(0) != grid.GetLength(1))
                throw new Exception("Grid must be square");
            this.grid = grid;
            zeroPos = FindZero();
        }
        public EightPuzzle(EightPuzzle other)
        {
            grid = new int[other.grid.GetLength(0), other.grid.GetLength(0)];
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i,j] = other.grid[i, j];
                }
            }
            zeroPos = new Position(other.zeroPos);
        }

        private Position FindZero()
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == 0)
                        return new Position(i, j);
                }
            }
            throw new Exception("No zero to be found");
        }

        internal List<Action> GetActions()
        {
            List<Action> actions = new List<Action>(4);
            if (zeroPos.r > 0)
                actions.Add(new Action(-1, 0));
            if (zeroPos.c > 0)
                actions.Add(new Action(0, -1));
            if (zeroPos.r < grid.GetLength(0) - 1)
                actions.Add(new Action(1, 0));
            if (zeroPos.c < grid.GetLength(1) - 1)
                actions.Add(new Action(0, 1));
            return actions;
        }

        internal void ExecAction(Action action)
        {
            grid[zeroPos.r, zeroPos.c] = grid[zeroPos.r + action.r, zeroPos.c + action.c];
            grid[zeroPos.r + action.r, zeroPos.c + action.c] = 0;
            zeroPos.r += action.r;
            zeroPos.c += action.c;
        }

        public static EightPuzzle GeneratePuzzle()
        {
            int[,] grid = (int[,]) Goal.GOAL.Clone();
            Random rand = new Random();
            do {
                Shuffle(rand, grid);
            } while (!IsSolvable(grid));
            return new EightPuzzle(grid);
        }
        internal static int GetInvertions(int[,] grid)
        {
            int width = grid.GetLength(0);
            int inversions = 0, i1, j1, i2, j2;
            for (int x = 0; x < grid.Length; x++)
            {
                for (int y = x + 1; y < grid.Length; y++)
                {
                    i1 = x / width; j1 = x % width;
                    i2 = y / width; j2 = y % width;
                    if (grid[i1, j1] != 0 && grid[i2, j2] != 0 && grid[i1, j1] > grid[i2, j2])
                        inversions++;
                }
            }
            return inversions;
        }
        private static bool IsSolvable(int[,] grid)
        {
            return GetInvertions(grid) % 2 == Goal.GOAL_INVERTIONS_PARITY;
        }
        private static void Shuffle<T>(Random random, T[,] array)
        {
            int lengthRow = array.GetLength(1);

            for (int i = array.Length - 1; i > 0; i--)
            {
                int i0 = i / lengthRow;
                int i1 = i % lengthRow;

                int j = random.Next(i + 1);
                int j0 = j / lengthRow;
                int j1 = j % lengthRow;

                (array[j0, j1], array[i0, i1]) = (array[i0, i1], array[j0, j1]);
            }
        }

        internal int ManhattanDistance()
        {
            Position p;
            int h = 0;
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    p = Goal.GetGoalPosition(grid[i, j]);
                    h += Math.Abs(p.r - i) + Math.Abs(p.c - j);
                }
            }
            return h;
        }
        internal int MaxManhattanDistance()
        {
            Position p;
            int h = 0;
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    p = Goal.GetGoalPosition(grid[i, j]);
                    h = Math.Max(h, Math.Abs(p.r - i) + Math.Abs(p.c - j));
                }
            }
            return h;
        }

        public static string ToString(int[,] grid)
        {
            string s = "";
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    s += grid[i, j] + " ";
                }
                s += "\n";
            }
            return s;
        }
        override public string ToString()
        {
            return ToString(grid);
        }
        public static void PrintPuzzle(int[,] grid)
        {
            Console.Write(ToString(grid));
        }
        public void PrintPuzzle()
        {
            Console.Write(ToString());
        }
    }

    class Position
    {
        public int r, c; // row, column

        public Position(int r, int c)
        {
            this.r = r;
            this.c = c;
        }
        public Position(Position other)
        {
            r = other.r;
            c = other.c;
        }
    }

    class Action : Position
    {
        public Action(int x, int y) : base(x, y)
        {
            if (Math.Abs(x + y) != 1 || (x != 0 && y != 0))
                throw new Exception("Invalid action");
        }

        public override string ToString()
        {
            if (r == -1)
                return "UP";
            if (r == 1)
                return "DOWN";
            if (c == -1)
                return "LEFT";
            if (c == 1)
                return "RIGHT";
            throw new Exception("Unexpected action");
        }
    }
}