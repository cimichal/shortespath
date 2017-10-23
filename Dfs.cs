using System;
using System.Collections.Generic;

namespace algorithms
{
    public class Dfs<T>
    {
        public Dfs(Graph<T> graf, T indexPunktuStartowego)
        {
            this.Graf = graf;
            this.IndexPunktuStartowego = indexPunktuStartowego;
            this.StosWierzcholkowDoOdwiedzenia = new Stack<T>();
            this.ListaOdwiedzonychWierzcholkow = new HashSet<T>();
        }

        private T IndexPunktuStartowego { get; }
        private Graph<T> Graf { get; }
        private Stack<T> StosWierzcholkowDoOdwiedzenia { get; }

        // kazdy wierzcholek moze zostac odwiedzony wylacznie raz, dlatego hashset
        private HashSet<T> ListaOdwiedzonychWierzcholkow { get; }

        public bool ObliczDfs<T>()
        {
            if (!this.ListaWierzcholkowPosiadaPunktStartowy())
            {
                return true;
            }

            this.DodajNowyWierzcholekDoOdwiedzenia(this.IndexPunktuStartowego);

            while (this.StosWierzcholkowDoOdwiedzenia.Count > 0)
            {
                var wierzcholekDoSprawdzenia = this.StosWierzcholkowDoOdwiedzenia.Pop();

                if (this.WierzcholeBylOdwiedzony(wierzcholekDoSprawdzenia))
                {
                    continue;
                }

                this.WirzcholekZostalOdwiedzony(wierzcholekDoSprawdzenia);

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
            return this.Graf.ListaPowiazanychWierzcholkow.ContainsKey(this.IndexPunktuStartowego);
        }

        private IEnumerable<T> PobierzListeSasiednichWierzcholkow(T wierzcholekDoSprawdzenia)
        {
            return this.Graf.ListaPowiazanychWierzcholkow[wierzcholekDoSprawdzenia];
        }

        private void DodajNowyWierzcholekDoOdwiedzenia(T wierzcholekDoOdwiedzenia)
        {
            this.StosWierzcholkowDoOdwiedzenia.Push(wierzcholekDoOdwiedzenia);
        }

        private bool WierzcholeBylOdwiedzony(T wierzcholekDoSprawdzenia)
        {
            return this.ListaOdwiedzonychWierzcholkow.Contains(wierzcholekDoSprawdzenia);
        }

        private void WirzcholekZostalOdwiedzony(T sprawdzonyWierzcholek)
        {
            this.ListaOdwiedzonychWierzcholkow.Add(sprawdzonyWierzcholek);
            Console.WriteLine("Dfs : {0}", sprawdzonyWierzcholek);
        }
    }
}