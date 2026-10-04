using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Chess_Shapovalova.Classes
{
    public class Queen : Pawn
    {
        public Queen(int x, int y, bool black) : base(x, y, black) { }

        public override string ImageName => Black ? "black-queen.png" : "white-queen.png";

        public override List<(int, int)> GetPossibleMoves()
        {
            var moves = new List<(int, int)>();
            int[] dx = { 1, -1, 0, 0, 1, 1, -1, -1 };
            int[] dy = { 0, 0, 1, -1, 1, -1, 1, -1 };

            for (int i = 0; i < 8; i++)
            {
                int nx = X + dx[i];
                int ny = Y + dy[i];

                while (nx >= 0 && nx < 8 && ny >= 0 && ny < 8)
                {
                    Pawn target = MainWindow.mainWindow.GetPawnAt(nx, ny);

                    if (target == null)
                    {
                        moves.Add((nx, ny));
                    }
                    else
                    {
                        if (target.Black != Black)
                            moves.Add((nx, ny));
                        break;
                    }

                    nx += dx[i];
                    ny += dy[i];
                }
            }
            return moves;
        }
    }
}