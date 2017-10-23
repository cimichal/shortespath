using System;
using System.Collections.Generic;

namespace algorithms
{
    public class Graph<T>
    {
        public Graph(IEnumerable<T> wierzcholki, IEnumerable<Tuple<T, T>> krawedzie)
        {
            this.DodajWierzcholkiDoPowiazanejListy(wierzcholki);
            this.DodajKrawedzieDoPowiazanejListy(krawedzie);
        }

        public Dictionary<T, HashSet<T>> ListaPowiazanychWierzcholkow { get; } = new Dictionary<T, HashSet<T>>();

        private void DodajKrawedzieDoPowiazanejListy(IEnumerable<Tuple<T, T>> krawedzie)
        {
            foreach (var krawedz in krawedzie)
            {
                this.DodajKrawedz(krawedz);
            }
        }

        private void DodajKrawedz(Tuple<T, T> krawedz)
        {
            if (this.SprawdzCzyListaPosiadaPolaczoneWierzcholekIKrawedz(krawedz))
            {
                this.DodajSasiadaDlaWierzcholka(krawedz);
            }
        }

        private void DodajWierzcholkiDoPowiazanejListy(IEnumerable<T> wierzcholki)
        {
            foreach (var wierzcholek in wierzcholki)
            {
                this.ListaPowiazanychWierzcholkow[wierzcholek] = new HashSet<T>();
            }
        }

        private bool SprawdzCzyListaPosiadaPolaczoneWierzcholekIKrawedz(Tuple<T, T> krawedz)
        {
            var wierzcholkiSaPowiazaneZKrawedzia =
                false || this.ListaPowiazanychWierzcholkow.ContainsKey(krawedz.Item1) &&
                this.ListaPowiazanychWierzcholkow.ContainsKey(krawedz.Item2);

            return wierzcholkiSaPowiazaneZKrawedzia;
        }

        private void DodajSasiadaDlaWierzcholka(Tuple<T, T> krawedz)
        {
            this.ListaPowiazanychWierzcholkow[krawedz.Item1].Add(krawedz.Item2);
            this.ListaPowiazanychWierzcholkow[krawedz.Item2].Add(krawedz.Item1);
        }
    }
}