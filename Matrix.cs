using System;
using System.Collections.Generic;

namespace algorithms
{
    public class Matrix
    {
        public Matrix(int iloscPolPoziomo, int iloscPolPionowo)
        {
            this.IloscPolPionowo = iloscPolPionowo;
            this.IloscPolPoziomo = iloscPolPoziomo;
            this.ListaPunktow = new List<MatrixFiled<int>>();
        }

        private int IloscPolPoziomo { get; }
        private int IloscPolPionowo { get; }
        private List<MatrixFiled<int>> ListaPunktow  { get; set; }

        public void GenerateEmptyMatrix()
        {
            for (var wiersz = 1; wiersz <= this.IloscPolPoziomo; wiersz++)
            {
                for (var kolumna = 1; kolumna <= this.IloscPolPionowo; kolumna++)
                {
                    this.ListaPunktow.Add(new MatrixFiled<int>()
                    {
                        IndexPoziom = wiersz,
                        IndexPion = kolumna, 
                        State = FieldState.Nieodwiedzony,
                        ListaSasiadow = WygenerujListeSasiadow(wiersz, kolumna)
                    });
                }
            }
        }

        private static IEnumerable<int> WygenerujListeSasiadow(int wiersz, int kolumna)
        {
            var listaSasiadowPoziomo = new List<int>();
            var listaSasiadowPionowo = new List<int>();


        }

        public void ClearConsole()
        {
            Console.Clear();
        }
    }

    public class MatrixFiled<T>
    {
        public FieldState State { get; set; }
        public int IndexPoziom { get; set; }
        public int IndexPion { get; set; }
        public IEnumerable<T> ListaSasiadow { get; set; }
    }

    public enum FieldState
    {
        Nieodwiedzony,
        Odwiedzony,
        Zablokowany
    }
}