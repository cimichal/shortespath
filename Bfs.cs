using System;
using System.Collections.Generic;

namespace algorithms
{
    public class Bfs<T>
    {
        public Bfs(Graph<T> graf, T indexPunktuStartowego)
        {
            this.Graf = graf;
            this.IndexPunktuStartowego = indexPunktuStartowego;
            this.StosWierzcholkowDoOdwiedzenia = new Queue<T>();
            this.ListaOdwiedzonychWierzcholkow = new HashSet<T>();
        }

        private T IndexPunktuStartowego { get; }
        private Graph<T> Graf { get; }
        private Queue<T> StosWierzcholkowDoOdwiedzenia { get; }
        private HashSet<T> ListaOdwiedzonychWierzcholkow { get; }

        public bool ObliczBfs<T>()
        {
            if (!this.ListaWierzcholkowPosiadaPunktStartowy())
            {
                return true;
            }

            this.DodajNowyWierzcholekDoOdwiedzenia(this.IndexPunktuStartowego);

            while (this.StosWierzcholkowDoOdwiedzenia.Count > 0)
            {
                var wierzcholekDoSprawdzenia = this.StosWierzcholkowDoOdwiedzenia.Dequeue();

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

        public Func<T, IEnumerable<T>> ObliczNajkrotszaDroge()
        {
            var poprzednieDrogi = new Dictionary<T, T>();
            var kolejka = new Queue<T>();

            kolejka.Enqueue(this.IndexPunktuStartowego);

            while (kolejka.Count > 0)
            {
                var aktualnyWierzcholek = kolejka.Dequeue();
                foreach (var sasiad in this.PobierzListeSasiednichWierzcholkow(aktualnyWierzcholek))
                {
                    if (poprzednieDrogi.ContainsKey(sasiad))
                    {
                        continue;
                    }

                    poprzednieDrogi[sasiad] = aktualnyWierzcholek;
                    kolejka.Enqueue(sasiad);
                }
            }

            IEnumerable<T> NajkrotszaDroga(T wierzcholek)
            {
                var sciezka = new List<T>();

                while (!wierzcholek.Equals(this.IndexPunktuStartowego))
                {
                    sciezka.Add(wierzcholek);
                    wierzcholek = poprzednieDrogi[wierzcholek];
                }

                sciezka.Add(this.IndexPunktuStartowego);
                sciezka.Reverse();

                return sciezka;
            }

            return NajkrotszaDroga;
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
            this.StosWierzcholkowDoOdwiedzenia.Enqueue(wierzcholekDoOdwiedzenia);
        }

        private bool WierzcholeBylOdwiedzony(T wierzcholekDoSprawdzenia)
        {
            return this.ListaOdwiedzonychWierzcholkow.Contains(wierzcholekDoSprawdzenia);
        }

        private void WirzcholekZostalOdwiedzony(T sprawdzonyWierzcholek)
        {
            this.ListaOdwiedzonychWierzcholkow.Add(sprawdzonyWierzcholek);
            Console.WriteLine("Bfs : {0}", sprawdzonyWierzcholek);
        }
    }
}