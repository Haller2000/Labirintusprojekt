using System.Collections.Generic;
using System.IO;
using Labyrinth.Editor.Models;

namespace Labyrinth.Editor.Services
{
    public class MapFileService
    {
        public void SaveMap(MapModel map, string path)
        {
            List<string> lines = new List<string>();

            for (int row = 0; row < map.Height; row++)
            {
                string line = "";

                for (int col = 0; col < map.Width; col++)
                {
                    line += map.Map[row, col];
                }

                lines.Add(line);
            }

            File.WriteAllLines(path, lines);
        }

        public MapModel LoadMap(string path)
        {
            string[] lines = File.ReadAllLines(path);

            int height = lines.Length;
            int width = lines[0].Length;

            MapModel map = new MapModel(width, height);

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    map.Map[row, col] = lines[row][col];
                }
            }

            return map;
        }
    }
}