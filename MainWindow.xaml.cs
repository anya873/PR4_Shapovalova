using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chess_Shapovalova
{
    public partial class MainWindow : Window
    {
        public List<Classes.Pawn> Pawns = new List<Classes.Pawn>();
        public static MainWindow mainWindow;
        public bool WhiteTurn = true;

        public MainWindow()
        {
            InitializeComponent();
            MainWindow.mainWindow = this;

            for (int i = 0; i < 8; i++)
                Pawns.Add(new Classes.Pawn(i, 1, true));

            for (int i = 0; i < 8; i++)
                Pawns.Add(new Classes.Pawn(i, 6, false));

            Pawns.Add(new Classes.Queen(3, 0, true));
            Pawns.Add(new Classes.Queen(3, 7, false));

            CreateFigure();
        }

        public void CreateFigure()
        {
            foreach (Classes.Pawn Pawn in Pawns)
            {
                Pawn.Figure = new Grid() { Width = 50, Height = 50 };
                Pawn.Figure.Background = new ImageBrush(new BitmapImage(
                    new Uri(@"pack://application:,,,/Images/" + Pawn.ImageName)));

                Grid.SetColumn(Pawn.Figure, Pawn.X);
                Grid.SetRow(Pawn.Figure, Pawn.Y);
                Pawn.Figure.MouseDown += Pawn.SelectFigure;
                gameBoard.Children.Add(Pawn.Figure);
            }
        }

        public void OnSelect(Classes.Pawn SelectPawn)
        {
            foreach (Classes.Pawn Pawn in Pawns)
                if (Pawn != SelectPawn && Pawn.Select)
                    Pawn.Deselect(); 
        }

        public void SelectTile(object sender, MouseButtonEventArgs e)
        {
            Grid tile = sender as Grid;
            if (tile == null) return;

            int x = Grid.GetColumn(tile);
            int y = Grid.GetRow(tile);

            Classes.Pawn selected = Pawns.Find(p => p.Select);
            if (selected == null) return;

            var moves = selected.GetPossibleMoves();
            if (moves.Contains((x, y)))
            {
                Grid.SetColumn(selected.Figure, x);
                Grid.SetRow(selected.Figure, y);
                selected.X = x;
                selected.Y = y;
                selected.Deselect();
                ClearHighlights();

                WhiteTurn = !WhiteTurn;
                UpdateTurnLabel();
            }
        }
        public void UpdateTurnLabel()
        {
            turnLabel.Content = WhiteTurn ? "Ход белых" : "Ход чёрных";
        }

        public Classes.Pawn GetPawnAt(int x, int y)
        {
            return Pawns.Find(p => p.X == x && p.Y == y);
        }

        public Grid GetCellAt(int x, int y)
        {
            foreach (var child in gameBoard.Children)
            {
                if (child is Grid cell && !(cell.Background is ImageBrush))
                {
                    if (Grid.GetColumn(cell) == x && Grid.GetRow(cell) == y)
                        return cell;
                }
            }
            return null;
        }

        public void ClearHighlights()
        {
            foreach (var child in gameBoard.Children)
            {
                if (child is Grid cell && !(cell.Background is ImageBrush))
                {
                    int col = Grid.GetColumn(cell);
                    int row = Grid.GetRow(cell);
                    cell.Background = (col + row) % 2 == 0
                        ? new SolidColorBrush(Colors.White)
                        : new SolidColorBrush(Color.FromRgb(0x9A, 0x64, 0x00));
                }
            }
        }

        public void HighlightMoves(Classes.Pawn piece)
        {
            ClearHighlights();

            foreach (var (mx, my) in piece.GetPossibleMoves())
            {
                Grid cell = GetCellAt(mx, my);
                if (cell == null) continue;

                Classes.Pawn target = GetPawnAt(mx, my);
                if (target == null)
                    cell.Background = new SolidColorBrush(Colors.LightGreen);
                else if (target.Black != piece.Black)
                    cell.Background = new SolidColorBrush(Colors.LightCoral);
            }
        }
    }
}