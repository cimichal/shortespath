using System;
using System.Collections.Generic;

namespace algorithms
{
    public class Dfs
    {
        public Dfs(int indexPunktuStartowego, int indexPunktuKoncowego)
        {
            this.IndexPunktuStartowego = indexPunktuStartowego;
            this.IndexPunktuKoncowego = indexPunktuKoncowego;
            this.StosWierzcholkowDoOdwiedzenia = new Stack<int>();
            this.ListaOdwiedzonychWierzcholkow = new HashSet<int>();
        }

        private int IndexPunktuStartowego { get; }
        private int IndexPunktuKoncowego { get; }
        public Matrix Matrix { get; set; }
        private Stack<int> StosWierzcholkowDoOdwiedzenia { get; }
        private HashSet<int> ListaOdwiedzonychWierzcholkow { get; }
        private bool OdwiedzonoOstatniWierzcholek { get; set; }
        private bool currentPointAvalaible { get; set; } = true;

        public bool ObliczDfs()
        {
            if (!this.ListaWierzcholkowPosiadaPunktStartowy())
            {
                return true;
            }

            this.DodajNowyWierzcholekDoOdwiedzenia(this.IndexPunktuStartowego);

            while (this.StosWierzcholkowDoOdwiedzenia.Count > 0 && !this.OdwiedzonoOstatniWierzcholek)
            {
                var wierzcholekDoSprawdzenia = this.StosWierzcholkowDoOdwiedzenia.Pop();

                if (this.WierzcholeBylOdwiedzony(wierzcholekDoSprawdzenia))
                {
                    continue;
                }

                this.WirzcholekZostalOdwiedzony(wierzcholekDoSprawdzenia);

                if (wierzcholekDoSprawdzenia.Equals(this.IndexPunktuKoncowego))
                {
                    this.OdwiedzonoOstatniWierzcholek = true;
                }

                foreach (var sasiedziWierzcholka in this.PobierzListeSasiednichWierzcholkow(wierzcholekDoSprawdzenia, DataStructureType.Matrix))
                {
                    if (!this.WierzcholeBylOdwiedzony(sasiedziWierzcholka))
                    {
                        this.DodajNowyWierzcholekDoOdwiedzenia(sasiedziWierzcholka);
                    }
                }
            }

            return true;
        }

        private bool ListaWierzcholkowPosiadaPunktStartowy()
        {
            return this.Matrix.IsItemAsMatrixPoint(this.IndexPunktuStartowego);
        }

        private IEnumerable<int> PobierzListeSasiednichWierzcholkow(int wierzcholekDoSprawdzenia,
            DataStructureType dataStructureType)
        {
            var index = wierzcholekDoSprawdzenia;

            switch (dataStructureType)
            {
                case DataStructureType.Matrix:
                    var result = this.Matrix.GetWalkableNeighbors(index);
                    return result;

                case DataStructureType.Graf:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(dataStructureType), dataStructureType, null);
            }

            return null;
        }

        public List<int> NajkrotszaDroga()
        {
            var poprzednieDrogi = new Dictionary<int, int>();
            var kolejka = new Stack<int>();

            kolejka.Push(this.IndexPunktuStartowego);

            while (kolejka.Count > 0)
            {
                var aktualnyWierzcholek = kolejka.Pop();
                foreach (var sasiad in this.PobierzListeSasiednichWierzcholkow(aktualnyWierzcholek,
                    DataStructureType.Matrix))
                {
                    if (poprzednieDrogi.ContainsKey(sasiad))
                    {
                        continue;
                    }

                    poprzednieDrogi[sasiad] = aktualnyWierzcholek;
                    kolejka.Push(sasiad);
                }
            }

            return this.NajkrotszaDroga(poprzednieDrogi);
        }
        
        private void DodajNowyWierzcholekDoOdwiedzenia(int wierzcholekDoOdwiedzenia)
        {
            this.StosWierzcholkowDoOdwiedzenia.Push(wierzcholekDoOdwiedzenia);
        }

        private bool WierzcholeBylOdwiedzony(int wierzcholekDoSprawdzenia)
        {
            return this.ListaOdwiedzonychWierzcholkow.Contains(wierzcholekDoSprawdzenia);
        }

        private void WirzcholekZostalOdwiedzony(int sprawdzonyWierzcholek)
        {
            this.ListaOdwiedzonychWierzcholkow.Add(sprawdzonyWierzcholek);
            this.Matrix.SelectVisitedPoint(sprawdzonyWierzcholek);
            //Console.WriteLine("Dfs : {0}", sprawdzonyWierzcholek);
        }

        private List<int> NajkrotszaDroga(Dictionary<int, int> poprzednieDrogi)
        {
            var sciezka = new List<int>();
            var current = this.IndexPunktuKoncowego;

            while (!current.Equals(this.IndexPunktuStartowego) && this.currentPointAvalaible)
            {
                sciezka.Add(current);

                if (poprzednieDrogi.ContainsKey(current))
                {
                    current = poprzednieDrogi[current];
                }
                else
                {
                    this.currentPointAvalaible = false;
                }

            }

            sciezka.Add(this.IndexPunktuStartowego);
            sciezka.Reverse();

            return sciezka;
        }
    }
}