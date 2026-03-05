using System;
using System.Collections.Generic;

namespace PeixeraVirtual;

public abstract class CriaturaAmbDireccio : Criatura
    {
        public Direccio Direccio;

        public CriaturaAmbDireccio(Especie especie, Sexe sexe, Posicio posicio, Direccio direccio)
            : base(especie, sexe, posicio)
        {
            Direccio = direccio;
        }

        public override void Mou(Peixera peixera)
        {
            int x = Posicio.X;
            int y = Posicio.Y;

            if (Direccio == Direccio.Amunt) y--;
            else if (Direccio == Direccio.Avall) y++;
            else if (Direccio == Direccio.Esquerra) x--;
            else if (Direccio == Direccio.Dreta) x++;

            Posicio = peixera.AplicaWrap(new Posicio(x, y));
        }

        public override void CanviaDireccio()
        {
            if (Direccio == Direccio.Amunt) Direccio = Direccio.Avall;
            else if (Direccio == Direccio.Avall) Direccio = Direccio.Amunt;
            else if (Direccio == Direccio.Esquerra) Direccio = Direccio.Dreta;
            else if (Direccio == Direccio.Dreta) Direccio = Direccio.Esquerra;
        }
    }