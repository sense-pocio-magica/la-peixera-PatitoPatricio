using System;
using System.Collections.Generic;

namespace PeixeraVirtual;

public class Pop : Criatura
    {
        public bool SentitHorari;
        private int _indexPerimetre;

        public Pop(Posicio posicioPerimetre, bool sentitHorari, Peixera peixera)
            : base(Especie.Pop, Sexe.Cap, posicioPerimetre)
        {
            SentitHorari = sentitHorari;
            _indexPerimetre = peixera.PosicioAIndexPerimetre(posicioPerimetre);
        }

        public override void Mou(Peixera peixera)
        {
            int L = peixera.LongitudPerimetre();

            if (SentitHorari) _indexPerimetre++;
            else _indexPerimetre--;

            while (_indexPerimetre < 0) _indexPerimetre += L;
            while (_indexPerimetre >= L) _indexPerimetre -= L;

            Posicio = peixera.IndexPerimetreAPosicio(_indexPerimetre);
        }

        public override void CanviaDireccio()
        {
            SentitHorari = !SentitHorari;
        }
    }