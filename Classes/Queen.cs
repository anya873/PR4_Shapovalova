using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Chess_Shapovalova.Classes
{
    public class Queen:Pawn
    {
         public Queen(int x, int y, bool black) : base(x, y, black) { }
        public override string ImageName => Black ? "black-queen.png" : "white-queen.png";
        public override void SelectFigure(object sender, MouseButtonEventArgs e)
        {
            bool moved = false;
            Pawn selected = MainWindow.mainWindow.Pawns.Find(p => p.Select == true);
            if (selected != null && selected!=this && selected.Black != this.Black)
            {
                if(CanMoveTo(selected.X, selected.Y, selected))
                {
                    MainWindow.mainWindow.gameBoard.Children.Remove(this.Figure);
                    Grid.SetColumn(selected.Figure, this.X);
                    Grid.SetRow(selected.Figure, this.Y);
                    selected.X = this.X;
                    selected.Y = this.Y;
                    selected.Deselect();
                    moved = true;
                }
            }
            if (!moved)
            {
                base.SelectFigure(sender, e);
            }
        }
        public bool CanMoveTo(int fromX, int fromY, Pawn attacker)
        {
            bool sameRow = fromY == this.Y;
            bool sameCol = fromX == this.X;
            bool sameDiag = Math.Abs(fromX-this.X)==Math.Abs(fromY-this.Y);
            if (!sameRow && !sameCol && !sameDiag) return false;
            int dx = Math.Sign(this.X - fromX);
            int dy = Math.Sign(this.Y - fromY);
            int x = fromX + dx;
            int y = fromY + dy;
            while(x!=this.X || y!=this.Y)
            {
                if(MainWindow.mainWindow.GetPawnAt(x, y) != null) return false;
                x += dx;
                y += dy;
            }
            return true;
        }
    }
}
