using System;
using System.Collections.Generic;
using System.IO;

namespace LabirintusJatek
{
    class Program
    {
        static Adatok adatok = new Adatok();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("1 - Magyar");
            Console.WriteLine("2 - English");
            Console.Write("Nyelv / Language: ");
            string nyelvValasz = Console.ReadLine();
            adatok.Angol = (nyelvValasz == "2");
            Console.Clear();

            Console.WriteLine(adatok.Angol ? "1 - Full Map" : "1 - Teljes térkép");
            Console.WriteLine(adatok.Angol ? "2 - Fog Map" : "2 - Vaktérkép");
            Console.Write(adatok.Angol ? "Game Mode: " : "Játékmód: ");
            string modValasz = Console.ReadLine();
            adatok.FedettTerkep = (modValasz == "2");
            Console.Clear();

            Console.Write(adatok.Angol ? "Map file name: " : "Pályafájl neve: ");
            string fajlNev = Console.ReadLine();
            adatok.FajlNev = fajlNev;

            if (!BetoltesPalya(fajlNev))
            {
                Console.WriteLine(adatok.Angol ? "Invalid map!" : "Hibás pálya!");
                Console.ReadKey();
                return;
            }

            adatok.Bejart = new bool[adatok.Sorok, adatok.Oszlopok];
            adatok.Bejart[adatok.JatekosY, adatok.JatekosX] = true;


            adatok.JatekKezdete = DateTime.Now; 
            adatok.IdozitoAktiv = true; 

            Jatek();
        }

        static int GetRoomNumber(char[,] map)
        {
            int termekSzama = 0;
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] == '█') termekSzama++; 
                }
            }
            return termekSzama;
        }

        static int GetSuitableEntrance(char[,] map)
        {
            int kijaratSzama = 0;
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (NyitottFel(map[0, x])) kijaratSzama++;
                if (NyitottLe(map[map.GetLength(0) - 1, x])) kijaratSzama++;
            }
            for (int y = 1; y < map.GetLength(0) - 1; y++)
            {
                if (NyitottBalra(map[y, 0])) kijaratSzama++;
                if (NyitottJobbra(map[y, map.GetLength(1) - 1])) kijaratSzama++;
            }
            return kijaratSzama;
        }

        static bool IsInvalidElement(char[,] map)
        {
            string validKarakterek = ".█" + adatok.FelNyitottak + adatok.LeNyitottak + adatok.BalNyitottak + adatok.JobbNyitottak;
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (!validKarakterek.Contains(map[y, x].ToString()))
                    {
                        return true; 
                    }
                }
            }
            return false; 
        }
        static List<string> GetUnavailableElements(char[,] map)
        {
            List<string> elerhetetlenPoziciok = new List<string>();
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    char aktualis = map[y, x];
                    if (aktualis == '.' || aktualis == '█') continue; 

                    bool vanKapcsolat = false;
                    if (y > 0 && NyitottFel(aktualis) && NyitottLe(map[y - 1, x])) vanKapcsolat = true;
                    if (y < map.GetLength(0) - 1 && NyitottLe(aktualis) && NyitottFel(map[y + 1, x])) vanKapcsolat = true;
                    if (x > 0 && NyitottBalra(aktualis) && NyitottJobbra(map[y, x - 1])) vanKapcsolat = true;
                    if (x < map.GetLength(1) - 1 && NyitottJobbra(aktualis) && NyitottBalra(map[y, x + 1])) vanKapcsolat = true;

                    if (!vanKapcsolat) elerhetetlenPoziciok.Add(y + ":" + x);
                }
            }
            return elerhetetlenPoziciok;
        }

        static char[,] GenerateLabyrinth(List<string> positionsList)
        {
            char[,] ujPalya = new char[adatok.Sorok, adatok.Oszlopok];
            for (int y = 0; y < ujPalya.GetLength(0); y++)
            {
                for (int x = 0; x < ujPalya.GetLength(1); x++)
                {
                    ujPalya[y, x] = '.';
                }
            }
            foreach (string pozicio in positionsList)
            {
                string[] reszek = pozicio.Split(':');
                int sor = int.Parse(reszek[0]);
                int oszlop = int.Parse(reszek[1]);
                ujPalya[sor, oszlop] = adatok.Palya[sor, oszlop];
            }
            return ujPalya;
        }

        static bool BetoltesPalya(string fajlNev)
        {
            if (!File.Exists(fajlNev)) return false;

            string[] sorok = File.ReadAllLines(fajlNev);
            adatok.Sorok = sorok.Length;
            adatok.Oszlopok = sorok[0].Length;
            adatok.Palya = new char[adatok.Sorok, adatok.Oszlopok];

            for (int y = 0; y < adatok.Sorok; y++)
            {
                if (sorok[y].Length != adatok.Oszlopok) return false;
                for (int x = 0; x < adatok.Oszlopok; x++)
                {
                    adatok.Palya[y, x] = sorok[y][x];
                }
            }

            List<string> osszesPozicio = new List<string>();
            for (int y = 0; y < adatok.Sorok; y++)
            {
                for (int x = 0; x < adatok.Oszlopok; x++)
                {
                    if (adatok.Palya[y, x] != '.')
                    {
                        osszesPozicio.Add(y + ":" + x);
                    }
                }
            }
            char[,] generaltPalya = GenerateLabyrinth(osszesPozicio);

            if (IsInvalidElement(adatok.Palya)) return false;
            if (GetUnavailableElements(adatok.Palya).Count > 0) return false;
            if (GetSuitableEntrance(adatok.Palya) < 2) return false;
            if (GetRoomNumber(adatok.Palya) < 1) return false;

            adatok.OsszesKincs = GetRoomNumber(adatok.Palya);
            for (int y = 0; y < adatok.Sorok; y++)
            {
                for (int x = 0; x < adatok.Oszlopok; x++)
                {
                    if (adatok.Palya[y, x] == '█') adatok.Kincsek.Add(y + ":" + x);
                }
            }

            List<(int x, int y)> kijaratok = Kijaratok();
            adatok.JatekosX = kijaratok[0].x;
            adatok.JatekosY = kijaratok[0].y;
            adatok.KijaratX = kijaratok[1].x;
            adatok.KijaratY = kijaratok[1].y;

            return true;
        }

        static List<(int x, int y)> Kijaratok()
        {
            List<(int x, int y)> kijaratok = new List<(int x, int y)>();
            for (int x = 0; x < adatok.Oszlopok; x++)
            {
                if (NyitottFel(adatok.Palya[0, x])) kijaratok.Add((x, 0));
                if (NyitottLe(adatok.Palya[adatok.Sorok - 1, x])) kijaratok.Add((x, adatok.Sorok - 1));
            }
            for (int y = 1; y < adatok.Sorok - 1; y++)
            {
                if (NyitottBalra(adatok.Palya[y, 0])) kijaratok.Add((0, y));
                if (NyitottJobbra(adatok.Palya[y, adatok.Oszlopok - 1])) kijaratok.Add((adatok.Oszlopok - 1, y));
            }
            return kijaratok;
        }

        static void Jatek()
        {
            bool fut = true;
            while (fut)
            {
                Console.Clear();
                Kirajzol();

                TimeSpan elteltIdo = DateTime.Now - adatok.JatekKezdete;
                if (adatok.IdozitoAktiv && elteltIdo.TotalSeconds >= 120) 
                {
                    Console.WriteLine();
                    Console.WriteLine(adatok.Angol ? "Time's up! You failed to escape the labyrinth!" : "Nem sikerült kijutni a labirintusból!");
                    Console.ReadKey();
                    return; 
                }

                int maradekoMasodperc = 120 - (int)elteltIdo.TotalSeconds;
                Console.WriteLine();
                Console.WriteLine(adatok.Angol ?
                    $"Time left: {maradekoMasodperc / 60}:{maradekoMasodperc % 60:D2} | WASD Move | F5 Save | F9 Load | Treasures: {adatok.TalaltKincsek}/{adatok.OsszesKincs}" :
                    $"Hátralévő idő: {maradekoMasodperc / 60}:{maradekoMasodperc % 60:D2} | WASD Mozgás | F5 Mentés | F9 Betöltés | Kincsek: {adatok.TalaltKincsek}/{adatok.OsszesKincs}");

                Console.Write(adatok.Angol ? "Possible directions: " : "Lehetséges irányok: ");
                List<char> iranyok = LehetsegesJaratok(adatok.JatekosX, adatok.JatekosY);
                if (iranyok.Contains('W')) Console.Write("W(↑) ");
                if (iranyok.Contains('S')) Console.Write("S(↓) ");
                if (iranyok.Contains('A')) Console.Write("A(←) ");
                if (iranyok.Contains('D')) Console.Write("D(→) ");
                Console.WriteLine();

                ConsoleKeyInfo gomb = Console.ReadKey();
                int ujX = adatok.JatekosX;
                int ujY = adatok.JatekosY;

                switch (gomb.Key)
                {
                    case ConsoleKey.W: ujY--; break;
                    case ConsoleKey.S: ujY++; break;
                    case ConsoleKey.A: ujX--; break;
                    case ConsoleKey.D: ujX++; break;
                    case ConsoleKey.F5:
                        Mentes();
                        break;
                    case ConsoleKey.F9:
                        BetoltMentes();
                        break;
                    case ConsoleKey.Escape: return;
                }

                if (gomb.Key == ConsoleKey.W || gomb.Key == ConsoleKey.S || gomb.Key == ConsoleKey.A || gomb.Key == ConsoleKey.D)
                {
                    if (!LephetE(adatok.JatekosX, adatok.JatekosY, ujX, ujY)) continue;

                    adatok.JatekosX = ujX;
                    adatok.JatekosY = ujY;
                    adatok.Bejart[adatok.JatekosY, adatok.JatekosX] = true;

                    string pozicio = adatok.JatekosY + ":" + adatok.JatekosX;
                    if (adatok.Kincsek.Contains(pozicio))
                    {
                        adatok.TalaltKincsek++;
                        adatok.Kincsek.Remove(pozicio);
                        adatok.Palya[adatok.JatekosY, adatok.JatekosX] = '╬';
                    }

                    if (adatok.JatekosX == adatok.KijaratX && adatok.JatekosY == adatok.KijaratY)
                    {
                        Console.WriteLine();
                        Console.WriteLine(adatok.Angol ? "Exit? (Y/N)" : "Kilépés? (I/N)");
                        ConsoleKeyInfo valasz = Console.ReadKey();
                        if ((!adatok.Angol && (valasz.Key == ConsoleKey.I || valasz.Key == ConsoleKey.N)) ||
                            (adatok.Angol && (valasz.Key == ConsoleKey.Y || valasz.Key == ConsoleKey.N)))
                        {
                            if ((!adatok.Angol && valasz.Key == ConsoleKey.I) ||
                                (adatok.Angol && valasz.Key == ConsoleKey.Y))
                            {
                                fut = false;
                            }
                        }
                    }
                }
            }
        }

        static void Mentes()
        {
            string mentesFajl = "minta.sav";
            StreamWriter sw = new StreamWriter(mentesFajl);
            
                sw.WriteLine(adatok.JatekosX);
                sw.WriteLine(adatok.JatekosY);
                sw.WriteLine(adatok.TalaltKincsek);
                sw.WriteLine(adatok.Angol);
                sw.WriteLine(adatok.FedettTerkep);
                for (int y = 0; y < adatok.Sorok; y++)
                {
                    for (int x = 0; x < adatok.Oszlopok; x++)
                    {
                        sw.Write(adatok.Bejart[y, x] ? "1" : "0");
                    }
                    sw.WriteLine();
                }
                sw.WriteLine(adatok.Kincsek.Count);
                foreach (string kincs in adatok.Kincsek)
                {
                    sw.WriteLine(kincs);
                }
                sw.Close();
            
            Console.WriteLine(adatok.Angol ? "Game saved!" : "Játék mentve!");
            Console.ReadKey();
        }

        static void BetoltMentes()
        {
            string mentesFajl = "minta.sav";
            if (!File.Exists(mentesFajl))
            {
                Console.WriteLine(adatok.Angol ? "No save file found!" : "Nincs mentés fájl!");
                Console.ReadKey();
                return;
            }

            StreamReader sr = new StreamReader(mentesFajl);
            
                adatok.JatekosX = int.Parse(sr.ReadLine());
                adatok.JatekosY = int.Parse(sr.ReadLine());
                adatok.TalaltKincsek = int.Parse(sr.ReadLine());
                adatok.Angol = bool.Parse(sr.ReadLine());
                adatok.FedettTerkep = bool.Parse(sr.ReadLine());
                for (int y = 0; y < adatok.Sorok; y++)
                {
                    string sor = sr.ReadLine();
                    for (int x = 0; x < adatok.Oszlopok; x++)
                    {
                        adatok.Bejart[y, x] = (sor[x] == '1');
                    }
                }
                int kincsekSzama = int.Parse(sr.ReadLine());
                adatok.Kincsek.Clear();
                for (int i = 0; i < kincsekSzama; i++)
                {
                    adatok.Kincsek.Add(sr.ReadLine());
                }
                sr.Close();
            
            Console.WriteLine(adatok.Angol ? "Game loaded!" : "Játék betöltve!");
            Console.ReadKey();
        }

        static List<char> LehetsegesJaratok(int x, int y)
        {
            List<char> iranyok = new List<char>();
            char aktualis = adatok.Palya[y, x];
            if (y > 0 && NyitottFel(aktualis) && NyitottLe(adatok.Palya[y - 1, x])) iranyok.Add('W');
            if (y < adatok.Sorok - 1 && NyitottLe(aktualis) && NyitottFel(adatok.Palya[y + 1, x])) iranyok.Add('S');
            if (x > 0 && NyitottBalra(aktualis) && NyitottJobbra(adatok.Palya[y, x - 1])) iranyok.Add('A');
            if (x < adatok.Oszlopok - 1 && NyitottJobbra(aktualis) && NyitottBalra(adatok.Palya[y, x + 1])) iranyok.Add('D');
            return iranyok;
        }

        static bool LephetE(int regiX, int regiY, int ujX, int ujY)
        {
            if (ujX < 0 || ujY < 0 || ujX >= adatok.Oszlopok || ujY >= adatok.Sorok) return false;
            char regiKarakter = adatok.Palya[regiY, regiX];
            char ujKarakter = adatok.Palya[ujY, ujX];
            if (ujKarakter == '.') return false;
            if (ujY < regiY) return NyitottFel(regiKarakter) && NyitottLe(ujKarakter);
            if (ujY > regiY) return NyitottLe(regiKarakter) && NyitottFel(ujKarakter);
            if (ujX < regiX) return NyitottBalra(regiKarakter) && NyitottJobbra(ujKarakter);
            if (ujX > regiX) return NyitottJobbra(regiKarakter) && NyitottBalra(ujKarakter);
            return false;
        }

        static bool NyitottFel(char c){ return adatok.FelNyitottak.Contains(c); }
        static bool NyitottLe(char c) { return adatok.LeNyitottak.Contains(c); }
        static bool NyitottBalra(char c) { return adatok.BalNyitottak.Contains(c); }
        static bool NyitottJobbra(char c) { return adatok.JobbNyitottak.Contains(c); }

        static void Kirajzol()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            for (int y = 0; y < adatok.Sorok; y++)
            {
                for (int x = 0; x < adatok.Oszlopok; x++)
                {
                    if (x == adatok.JatekosX && y == adatok.JatekosY)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("◉");
                        Console.ResetColor();
                        continue;
                    }

                    if (adatok.FedettTerkep && !adatok.Bejart[y, x])
                    {
                        Console.Write(" ");
                        continue;
                    }

                    char c = adatok.Palya[y, x];
                    if (c == '.')
                    {
                        Console.Write(" ");
                    }
                    else if (c == '█')
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("█");
                        Console.ResetColor();
                    }
                    else if (c == '╬')
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("╬");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(c);
                        Console.ResetColor();
                    }
                }
                Console.WriteLine();
            }
        }
    }
}