using System.Collections.Generic;

namespace LabirintusJatek
{
    class Adatok
    {
        public char[,] Palya { get; set; }
        public int Sorok { get; set; }
        public int Oszlopok { get; set; }
        public int JatekosX { get; set; }
        public int JatekosY { get; set; }
        public int KijaratX { get; set; }
        public int KijaratY { get; set; }
        public int TalaltKincsek { get; set; }
        public int OsszesKincs { get; set; }
        public bool Angol { get; set; }
        public bool FedettTerkep { get; set; }
        public bool[,] Bejart { get; set; }
        public List<string> Kincsek { get; } = new List<string>();
        public string FajlNev { get; set; }

        public string FelNyitottak => "╬║╣╠╩╝╚█";
        public string LeNyitottak => "╬║╣╠╦╗╔█";
        public string BalNyitottak => "╬═╣╩╦╗╝█";
        public string JobbNyitottak => "╬═╠╩╦╚╔█";
        public DateTime JatekKezdete { get; set; }
        public bool IdozitoAktiv { get; set; } = true;

    }
}