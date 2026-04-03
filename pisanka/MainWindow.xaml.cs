using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace pisanka
{
    public partial class MainWindow : Window
    {
        const int SIZE = 10;
        Border[,] cells = new Border[SIZE, SIZE];

        public MainWindow()
        {
            InitializeComponent();
            InitGrid();
        }

        void InitGrid()
        {
            for (int i = 0; i < SIZE; i++)
            {
                GameGrid.RowDefinitions.Add(new RowDefinition());
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int y = 0; y < SIZE; y++)
            {
                for (int x = 0; x < SIZE; x++)
                {
                    Border b = new Border
                    {
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(1),
                        Background = Brushes.White
                    };

                    Grid.SetRow(b, y);
                    Grid.SetColumn(b, x);
                    GameGrid.Children.Add(b);

                    cells[x, y] = b;
                }
            }
        }
    }
}