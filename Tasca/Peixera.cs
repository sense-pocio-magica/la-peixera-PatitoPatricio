using System;
using System.Collections.Generic;

namespace PeixeraVirtual;


public class Peixera
    {
        public int Amplada;
        public int Alcada;

        public Peixera(int amplada, int alcada)
        {
            Amplada = amplada;
            Alcada = alcada;
        }

        public Posicio AplicaWrap(Posicio p)
        {
            int x = p.X;
            int y = p.Y;

            if (x < 0) x = Amplada - 1;
            if (x >= Amplada) x = 0;

            if (y < 0) y = Alcada - 1;
            if (y >= Alcada) y = 0;

            return new Posicio(x, y);
        }

        public int LongitudPerimetre()
        {
            // per una graella N x N: 4*(N-1)
            return 4 * (Amplada - 1);
        }

        public Posicio IndexPerimetreAPosicio(int index)
        {
            int n = Amplada - 1;
            int L = LongitudPerimetre();

            while (index < 0) index += L;
            while (index >= L) index -= L;

            // Superior: (0,0) -> (n,0) (n+1 punts)
            if (index <= n)
            {
                return new Posicio(index, 0);
            }

            index = index - (n + 1);

            // Dreta: (n,1) -> (n,n) (n punts)
            if (index < n)
            {
                return new Posicio(n, index + 1);
            }

            index = index - n;

            // Inferior: (n-1,n) -> (0,n) (n punts)
            if (index < n)
            {
                return new Posicio(n - 1 - index, n);
            }

            index = index - n;

            // Esquerra: (0,n-1) -> (0,1) (n-1 punts)
            return new Posicio(0, (n - 1) - index);
        }

        public int PosicioAIndexPerimetre(Posicio p)
        {
            int n = Amplada - 1;

            // Superior
            if (p.Y == 0) return p.X;

            // Dreta
            if (p.X == n) return (n + 1) + (p.Y - 1);

            // Inferior
            if (p.Y == n) return (n + 1) + n + (n - 1 - p.X);

            // Esquerra
            if (p.X == 0) return (n + 1) + n + n + (n - 1 - p.Y);

            throw new Exception("La posició no és al perímetre.");
        }

        public string ClauCasella(Posicio p)
        {
            return p.X + "," + p.Y;
        }
    }