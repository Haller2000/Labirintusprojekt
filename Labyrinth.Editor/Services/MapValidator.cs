using Labyrinth.Editor.Models;
using System.Collections.Generic;

namespace Labyrinth.Editor.Services
{
    internal class MapValidator
    {
        private bool OpensLeft(char tile)
        {
            return tile == '═' ||
                   tile == '╬' ||
                   tile == '╣' ||
                   tile == '╗' ||
                   tile == '╝' ||
                   tile == '╦' ||
                   tile == '╩';
        }

        private bool OpensRight(char tile)
        {
            return tile == '═' ||
                   tile == '╬' ||
                   tile == '╠' ||
                   tile == '╔' ||
                   tile == '╚' ||
                   tile == '╦' ||
                   tile == '╩';
        }

        private bool OpensUp(char tile)
        {
            return tile == '║' ||
                   tile == '╬' ||
                   tile == '╩' ||
                   tile == '╣' ||
                   tile == '╠' ||
                   tile == '╝' ||
                   tile == '╚';
        }

        private bool OpensDown(char tile)
        {
            return tile == '║' ||
                   tile == '╬' ||
                   tile == '╦' ||
                   tile == '╣' ||
                   tile == '╠' ||
                   tile == '╗' ||
                   tile == '╔';
        }

        private bool IsRoadCharacter(char tile)
        {
            return tile == '═' ||
                   tile == '║' ||
                   tile == '╔' ||
                   tile == '╗' ||
                   tile == '╚' ||
                   tile == '╝' ||
                   tile == '╦' ||
                   tile == '╩' ||
                   tile == '╠' ||
                   tile == '╣' ||
                   tile == '╬';
        }

        private bool IsAllowedCharacter(char tile)
        {
            return tile == '.' ||
                   tile == '█' ||
                   IsRoadCharacter(tile);
        }

        public int GetRoomNumber(char[,] map)
        {
            int roomNumber = 0;

            int height = map.GetLength(0);
            int width = map.GetLength(1);

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    if (map[row, col] == '█')
                    {
                        roomNumber++;
                    }
                }
            }

            return roomNumber;
        }

        public int GetSuitableEntrance(char[,] map)
        {
            int entranceNumber = 0;

            int height = map.GetLength(0);
            int width = map.GetLength(1);

            // Bal oldal
            for (int row = 0; row < height; row++)
            {
                if (OpensLeft(map[row, 0]))
                {
                    entranceNumber++;
                }
            }

            // Jobb oldal
            for (int row = 0; row < height; row++)
            {
                if (OpensRight(map[row, width - 1]))
                {
                    entranceNumber++;
                }
            }

            // Felső oldal
            for (int col = 0; col < width; col++)
            {
                if (OpensUp(map[0, col]))
                {
                    entranceNumber++;
                }
            }

            // Alsó oldal
            for (int col = 0; col < width; col++)
            {
                if (OpensDown(map[height - 1, col]))
                {
                    entranceNumber++;
                }
            }

            return entranceNumber;
        }

        public bool IsInvalidElement(char[,] map)
        {
            int height = map.GetLength(0);
            int width = map.GetLength(1);

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    if (!IsAllowedCharacter(map[row, col]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public List<string> GetUnavailableElements(char[,] map)
        {
            List<string> unavailables = new List<string>();

            int height = map.GetLength(0);
            int width = map.GetLength(1);

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    char current = map[row, col];

                    if (!IsRoadCharacter(current))
                    {
                        continue;
                    }

                    bool canBeReached = false;

                    // Balról elérhető?
                    if (col > 0)
                    {
                        char left = map[row, col - 1];

                        if (OpensRight(left) && OpensLeft(current))
                        {
                            canBeReached = true;
                        }
                    }

                    // Jobbról elérhető?
                    if (col < width - 1)
                    {
                        char right = map[row, col + 1];

                        if (OpensLeft(right) && OpensRight(current))
                        {
                            canBeReached = true;
                        }
                    }

                    // Fentről elérhető?
                    if (row > 0)
                    {
                        char up = map[row - 1, col];

                        if (OpensDown(up) && OpensUp(current))
                        {
                            canBeReached = true;
                        }
                    }

                    // Lentről elérhető?
                    if (row < height - 1)
                    {
                        char down = map[row + 1, col];

                        if (OpensUp(down) && OpensDown(current))
                        {
                            canBeReached = true;
                        }
                    }

                    // Ha a pálya szélén kifelé nyit, akkor bejárat/kijárat,
                    // tehát kívülről elérhetőnek számít.
                    if (col == 0 && OpensLeft(current))
                    {
                        canBeReached = true;
                    }

                    if (col == width - 1 && OpensRight(current))
                    {
                        canBeReached = true;
                    }

                    if (row == 0 && OpensUp(current))
                    {
                        canBeReached = true;
                    }

                    if (row == height - 1 && OpensDown(current))
                    {
                        canBeReached = true;
                    }

                    if (!canBeReached)
                    {
                        unavailables.Add(row + ":" + col);
                    }
                }
            }

            return unavailables;
        }

        public bool HasInvalidRooms(char[,] map)
        {
            int height = map.GetLength(0);
            int width = map.GetLength(1);

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    if (map[row, col] == '█')
                    {
                        bool hasConnection = false;

                        if (col > 0 && OpensRight(map[row, col - 1]))
                        {
                            hasConnection = true;
                        }

                        if (col < width - 1 && OpensLeft(map[row, col + 1]))
                        {
                            hasConnection = true;
                        }

                        if (row > 0 && OpensDown(map[row - 1, col]))
                        {
                            hasConnection = true;
                        }

                        if (row < height - 1 && OpensUp(map[row + 1, col]))
                        {
                            hasConnection = true;
                        }

                        if (!hasConnection)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        // Ezek azért maradhatnak, hogy a MainWindow-ban egyszerűbb legyen a használat.
        public bool HasRoom(MapModel map)
        {
            return GetRoomNumber(map.Map) > 0;
        }

        public bool HasExit(MapModel map)
        {
            return GetSuitableEntrance(map.Map) > 0;
        }

        public bool HasInvalidCharacters(MapModel map)
        {
            return IsInvalidElement(map.Map);
        }

        public bool HasInvalidRooms(MapModel map)
        {
            return HasInvalidRooms(map.Map);
        }
    }
}