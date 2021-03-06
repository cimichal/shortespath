﻿using System;
using System.Collections.Generic;

namespace algorithms
{
    public class Bfs
    {

        public Bfs(int indexPunktuStartowego, int indexPunktuKoncowego)
        {
            this.IndexPunktuStartowego = indexPunktuStartowego;
            this.IndexPunktuKoncowego = indexPunktuKoncowego;
            this.StosWierzcholkowDoOdwiedzenia = new Queue<int>();
            this.ListaOdwiedzonychWierzcholkow = new HashSet<int>();
        }

        private int IndexPunktuStartowego { get; }
        private int IndexPunktuKoncowego { get; }
        private Queue<int> StosWierzcholkowDoOdwiedzenia { get; }
        private HashSet<int> ListaOdwiedzonychWierzcholkow { get; }
        private bool OdwiedzonoOstatniWierzcholek { get; set; }
        private bool currentPointAvalaible { get; set; } = true;

        public Matrix Matrix { get; set; }

        public bool ObliczBfs()
        {
            if (!this.ListaWierzcholkowPosiadaPunktStartowy(DataStructureType.Matrix))
            {
                return true;
            }

            this.DodajNowyWierzcholekDoOdwiedzenia(this.IndexPunktuStartowego);

            while (this.StosWierzcholkowDoOdwiedzenia.Count > 0 && !this.OdwiedzonoOstatniWierzcholek)
            {
                var wierzcholekDoSprawdzenia = this.StosWierzcholkowDoOdwiedzenia.Dequeue();

                if (this.WierzcholeBylOdwiedzony(wierzcholekDoSprawdzenia))
                {
                    continue;
                }

                this.WirzcholekZostalOdwiedzony(wierzcholekDoSprawdzenia);

                if (wierzcholekDoSprawdzenia.Equals(this.IndexPunktuKoncowego))
                {
                    this.OdwiedzonoOstatniWierzcholek = true;
                }

                foreach (var sasiedziWierzcholka in this.PobierzListeSasiednichWierzcholkow(wierzcholekDoSprawdzenia,
                    DataStructureType.Matrix))
                {
                    if (!this.WierzcholeBylOdwiedzony(sasiedziWierzcholka))
                    {
                        this.DodajNowyWierzcholekDoOdwiedzenia(sasiedziWierzcholka);
                    }
                }
            }

            return true;
        }

        public List<int> NajkrotszaDroga()
        {
            var poprzednieDrogi = new Dictionary<int, int>();
            var kolejka = new Queue<int>();

            kolejka.Enqueue(this.IndexPunktuStartowego);

            while (kolejka.Count > 0)
            {
                var aktualnyWierzcholek = kolejka.Dequeue();
                foreach (var sasiad in this.PobierzListeSasiednichWierzcholkow(aktualnyWierzcholek,
                    DataStructureType.Matrix))
                {
                    if (poprzednieDrogi.ContainsKey(sasiad))
                    {
                        continue;
                    }

                    poprzednieDrogi[sasiad] = aktualnyWierzcholek;
                    kolejka.Enqueue(sasiad);
                }
            }

            return this.NajkrotszaDroga(poprzednieDrogi);
        }

        private bool ListaWierzcholkowPosiadaPunktStartowy(DataStructureType dataStructureType)
        {
            return dataStructureType == DataStructureType.Matrix && this.Matrix.IsItemAsMatrixPoint(this.IndexPunktuStartowego);
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

        private void DodajNowyWierzcholekDoOdwiedzenia(int wierzcholekDoOdwiedzenia)
        {
            this.StosWierzcholkowDoOdwiedzenia.Enqueue(wierzcholekDoOdwiedzenia);
        }

        private bool WierzcholeBylOdwiedzony(int wierzcholekDoSprawdzenia)
        {
            return this.ListaOdwiedzonychWierzcholkow.Contains(wierzcholekDoSprawdzenia);
        }

        private void WirzcholekZostalOdwiedzony(int sprawdzonyWierzcholek)
        {
            this.ListaOdwiedzonychWierzcholkow.Add(sprawdzonyWierzcholek);
            this.Matrix.SelectVisitedPoint(sprawdzonyWierzcholek);
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