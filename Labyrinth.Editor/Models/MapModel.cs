using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinth.Editor.Models
{
    public class MapModel
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public char[,] Map { get; set; }

        public MapModel(int width, int height)
        {
            Width = width;
            Height = height;

            Map = new char[height, width];

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    Map[row, col] = '.';
                }
            }
        }
    }
}
