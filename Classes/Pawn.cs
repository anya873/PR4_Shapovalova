using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chess_Shapovalova.Classes
{
    public class Pawn
    {
        public int X {  get; set; }
        public int Y { get; set; }
        public bool Select = false;
        public bool Black = false;
        public Grid Figure { get; set; }
        public Pawn(int X, int Y,  bool Black)
        {
            this.X = X;
            this.Y = Y;
            this.Black = Black;
        }
        public virtual string ImageName => Black ? "Pawn (black).png" : "Pawn.png";
        public virtual void SelectFigure(object sender, MouseButtonEventArgs e)
        {
            Pawn SelectPawn = MainWindow.mainWindow.Pawns.Find(p => p.Select == true);
            if (SelectPawn == null && MainWindow.mainWindow.WhiteTurn == Black)
                return;

            bool attack = false;

            if (SelectPawn != null && SelectPawn != this && SelectPawn.Black != this.Black)
            {
                var moves = SelectPawn.GetPossibleMoves();
                if (moves.Contains((this.X, this.Y)))
                {
                    MainWindow.mainWindow.gameBoard.Children.Remove(this.Figure);
                    Grid.SetColumn(SelectPawn.Figure, this.X);
                    Grid.SetRow(SelectPawn.Figure, this.Y);
                    SelectPawn.X = this.X;
                    SelectPawn.Y = this.Y;
                    SelectPawn.Deselect();
                    MainWindow.mainWindow.ClearHighlights();

                    MainWindow.mainWindow.WhiteTurn = !MainWindow.mainWindow.WhiteTurn;
                    MainWindow.mainWindow.UpdateTurnLabel();

                    if (MainWindow.mainWindow.WhiteTurn)
                        MainWindow.mainWindow.WhiteAIMove();

                    attack = true;
                }
            }

            if (!attack)
            {
                MainWindow.mainWindow.OnSelect(this);

                if (this.Select)
                {
                    this.Figure.Background = new ImageBrush(new BitmapImage(
                        new Uri(@"pack://application:,,,/Images/" + this.ImageName)));
                    this.Select = false;
                    MainWindow.mainWindow.ClearHighlights();
                }
                else
                {
                    this.Figure.Background = new ImageBrush(new BitmapImage(
                        new Uri(@"pack://application:,,,/Images/" + this.ImageName)));
                    this.Select = true;
                    MainWindow.mainWindow.HighlightMoves(this);
                }
            }
        }


        public void Transform(int X, int Y)
        {
            if (X != this.X)
            {
                Deselect();
                return;
            }
            if (!Black && ((this.Y == 1 && this.Y + 2 == Y) || this.Y + 1 == Y) ||
                Black && ((this.Y == 6 && this.Y - 2 == Y) || this.Y - 1 == Y))
            {
                Grid.SetColumn(this.Figure, X);
                Grid.SetRow(this.Figure, Y);
                this.X = X;
                this.Y = Y;
            }
            Deselect();
        }

        public void Deselect()
        {
            Select = false;
            Figure.Background = new ImageBrush(new BitmapImage(
                new Uri(@"pack://application:,,,/Images/" + this.ImageName)));
        }

        public virtual List<(int, int)> GetPossibleMoves()
        {
            var moves = new List<(int, int)>();
            int dir = Black ? 1 : -1;

            int ny = Y + dir;
            if (ny >= 0 && ny < 8 && MainWindow.mainWindow.GetPawnAt(X, ny) == null)
                moves.Add((X, ny));

            if ((!Black && Y == 1) || (Black && Y == 6))
            {
                int ny2 = Y + 2 * dir;
                if (MainWindow.mainWindow.GetPawnAt(X, ny) == null &&
                    MainWindow.mainWindow.GetPawnAt(X, ny2) == null)
                    moves.Add((X, ny2));
            }

            foreach (int dx in new[] { -1, 1 })
            {
                int ax = X + dx;
                int ay = Y + dir;
                if (ax >= 0 && ax < 8 && ay >= 0 && ay < 8)
                {
                    Pawn target = MainWindow.mainWindow.GetPawnAt(ax, ay);
                    if (target != null && target.Black != Black)
                        moves.Add((ax, ay));
                }
            }
            return moves;
        }
    }

}
