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
        private bool OdwiedzonoOstatniWierzcholek { get; set; } = false;


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

                foreach (var sasiedziWierzcholka in this.PobierzListeSasiednichWierzcholkow(wierzcholekDoSprawdzenia))
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

        private List<int> PobierzListeSasiednichWierzcholkow(int wierzcholekDoSprawdzenia)
        {
            return this.Matrix.GetWalkableNeighbors(wierzcholekDoSprawdzenia);
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
    }
}