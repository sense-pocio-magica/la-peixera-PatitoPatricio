using System;
using System.Collections.Generic;

namespace PeixeraVirtual;

public class Simulador
    {
        private Peixera _peixera;
        private Random _random;
        private List<Criatura> _criatures;

        public Simulador()
        {
            _peixera = new Peixera(20, 20);
            _random = new Random();
            _criatures = new List<Criatura>();
        }

        public void Inicialitza()
        {
            for (int i = 0; i < 50; i++)
            {
                _criatures.Add(new PeixNormal(Sexe.Mascle, PosicioAleatoria(), DireccioAleatoria()));
                _criatures.Add(new PeixNormal(Sexe.Femella, PosicioAleatoria(), DireccioAleatoria()));
            }

            for (int i = 0; i < 10; i++)
            {
                _criatures.Add(new Tauro(Sexe.Mascle, PosicioAleatoria(), DireccioAleatoria()));
                _criatures.Add(new Tauro(Sexe.Femella, PosicioAleatoria(), DireccioAleatoria()));
            }

            for (int i = 0; i < 15; i++)
            {
                Posicio p = PosicioPerimetreAleatoria();
                bool horari = (_random.Next(2) == 0);
                _criatures.Add(new Pop(p, horari, _peixera));
            }

            for (int i = 0; i < 3; i++)
            {
                _criatures.Add(new Tortuga(Sexe.Mascle, PosicioAleatoria(), DireccioAleatoria()));
                _criatures.Add(new Tortuga(Sexe.Femella, PosicioAleatoria(), DireccioAleatoria()));
            }
        }

        public void Executa(int rondes)
        {
            for (int r = 1; r <= rondes; r++)
            {
                FesUnaRonda();
                MostraResultatsRonda(r);
            }
        }

        private void FesUnaRonda()
        {
            // 1) Mou
            for (int i = 0; i < _criatures.Count; i++)
            {
                if (_criatures[i].Viva)
                {
                    _criatures[i].Mou(_peixera);
                }
            }

            // 2) Agrupa per casella
            Dictionary<string, List<Criatura>> perCasella = new Dictionary<string, List<Criatura>>();

            for (int i = 0; i < _criatures.Count; i++)
            {
                Criatura c = _criatures[i];
                if (!c.Viva) continue;

                string clau = _peixera.ClauCasella(c.Posicio);

                if (!perCasella.ContainsKey(clau))
                {
                    perCasella[clau] = new List<Criatura>();
                }
                perCasella[clau].Add(c);
            }

            // 3) Interaccions
            List<Criatura> nous = new List<Criatura>();

            foreach (KeyValuePair<string, List<Criatura>> entrada in perCasella)
            {
                List<Criatura> llista = entrada.Value;

                // 3.1) Taurons mengen (excepte tortugues)
                bool hiHaTauro = HiHaEspecieViva(llista, Especie.Tauro);
                bool hiHaTortuga = HiHaEspecieViva(llista, Especie.Tortuga);

                if (hiHaTauro)
                {
                    // si hi ha tortuga, els taurons canvien direcció (no la mengen)
                    if (hiHaTortuga)
                    {
                        for (int i = 0; i < llista.Count; i++)
                        {
                            if (llista[i].Viva && llista[i].Especie == Especie.Tauro)
                            {
                                llista[i].CanviaDireccio();
                            }
                        }
                    }

                    // mengen peixos normals i pops
                    for (int i = 0; i < llista.Count; i++)
                    {
                        if (!llista[i].Viva) continue;

                        if (llista[i].Especie == Especie.PeixNormal || llista[i].Especie == Especie.Pop)
                        {
                            llista[i].Mor();
                        }
                    }
                }

                // 3.2) Odi (mateixa espècie i mateix sexe)
                GestionaOdiMateixSexe(llista);

                // 3.3) Reproducció
                GestionaReproduccio(llista, nous);
            }

            // 4) Afegim nous
            for (int i = 0; i < nous.Count; i++)
            {
                _criatures.Add(nous[i]);
            }

            // 5) Envelleixen els taurons
            for (int i = 0; i < _criatures.Count; i++)
            {
                if (_criatures[i].Viva && _criatures[i].Especie == Especie.Tauro)
                {
                    Tauro t = (Tauro)_criatures[i];
                    t.EnvelleixUnaRonda();
                }
            }

            // 6) Eliminem morts
            for (int i = _criatures.Count - 1; i >= 0; i--)
            {
                if (!_criatures[i].Viva)
                {
                    _criatures.RemoveAt(i);
                }
            }
        }

        private bool HiHaEspecieViva(List<Criatura> llista, Especie e)
        {
            for (int i = 0; i < llista.Count; i++)
            {
                if (llista[i].Viva && llista[i].Especie == e) return true;
            }
            return false;
        }

        private void GestionaOdiMateixSexe(List<Criatura> llista)
        {
            // POPS: si n'hi ha 2 o més, canvien direcció i no moren
            int pops = 0;
            for (int i = 0; i < llista.Count; i++)
            {
                if (llista[i].Viva && llista[i].Especie == Especie.Pop) pops++;
            }
            if (pops >= 2)
            {
                for (int i = 0; i < llista.Count; i++)
                {
                    if (llista[i].Viva && llista[i].Especie == Especie.Pop)
                    {
                        llista[i].CanviaDireccio();
                    }
                }
            }

            GestionaOdiPerEspecie(llista, Especie.PeixNormal);
            GestionaOdiPerEspecie(llista, Especie.Tauro);
            GestionaOdiPerEspecie(llista, Especie.Tortuga);
        }

        private void GestionaOdiPerEspecie(List<Criatura> llista, Especie especie)
        {
            int mascles = 0;
            int femelles = 0;

            for (int i = 0; i < llista.Count; i++)
            {
                if (!llista[i].Viva) continue;
                if (llista[i].Especie != especie) continue;

                if (llista[i].Sexe == Sexe.Mascle) mascles++;
                else if (llista[i].Sexe == Sexe.Femella) femelles++;
            }

            if (mascles >= 2)
            {
                for (int i = 0; i < llista.Count; i++)
                {
                    if (llista[i].Viva && llista[i].Especie == especie && llista[i].Sexe == Sexe.Mascle)
                        llista[i].Mor();
                }
            }

            if (femelles >= 2)
            {
                for (int i = 0; i < llista.Count; i++)
                {
                    if (llista[i].Viva && llista[i].Especie == especie && llista[i].Sexe == Sexe.Femella)
                        llista[i].Mor();
                }
            }
        }

        private void GestionaReproduccio(List<Criatura> llista, List<Criatura> nous)
        {
            GestionaReproduccioPerEspecie(llista, nous, Especie.PeixNormal);
            GestionaReproduccioPerEspecie(llista, nous, Especie.Tauro);
            GestionaReproduccioPerEspecie(llista, nous, Especie.Tortuga);
        }

        private void GestionaReproduccioPerEspecie(List<Criatura> llista, List<Criatura> nous, Especie especie)
        {
            int mascles = 0;
            int femelles = 0;

            Direccio? dirMascle = null;
            Direccio? dirFemella = null;
            Posicio pos = null;

            for (int i = 0; i < llista.Count; i++)
            {
                Criatura c = llista[i];
                if (!c.Viva) continue;
                if (c.Especie != especie) continue;

                if (pos == null) pos = c.Posicio;

                if (c.Sexe == Sexe.Mascle)
                {
                    mascles++;
                    if (dirMascle == null)
                    {
                        CriaturaAmbDireccio cd = (CriaturaAmbDireccio)c;
                        dirMascle = cd.Direccio;
                    }
                }
                else if (c.Sexe == Sexe.Femella)
                {
                    femelles++;
                    if (dirFemella == null)
                    {
                        CriaturaAmbDireccio cd = (CriaturaAmbDireccio)c;
                        dirFemella = cd.Direccio;
                    }
                }
            }

            int parelles = mascles < femelles ? mascles : femelles;
            if (parelles <= 0) return;

            for (int k = 0; k < parelles; k++)
            {
                Sexe sexeFill = (_random.Next(2) == 0) ? Sexe.Mascle : Sexe.Femella;

                Direccio dirFill = DireccioAleatoria();
                int intents = 0;

                while (intents < 20)
                {
                    bool igualMascle = (dirMascle != null && dirFill == dirMascle.Value);
                    bool igualFemella = (dirFemella != null && dirFill == dirFemella.Value);

                    if (!igualMascle && !igualFemella) break;

                    dirFill = DireccioAleatoria();
                    intents++;
                }

                Criatura nova = null;

                if (especie == Especie.PeixNormal)
                    nova = new PeixNormal(sexeFill, new Posicio(pos.X, pos.Y), dirFill);
                else if (especie == Especie.Tauro)
                    nova = new Tauro(sexeFill, new Posicio(pos.X, pos.Y), dirFill);
                else if (especie == Especie.Tortuga)
                    nova = new Tortuga(sexeFill, new Posicio(pos.X, pos.Y), dirFill);

                if (nova != null) nous.Add(nova);
            }
        }

        public void MostraResultatsRonda(int ronda)
        {
            int peixos = 0, taurons = 0, pops = 0, tortugues = 0;

            for (int i = 0; i < _criatures.Count; i++)
            {
                Criatura c = _criatures[i];
                if (!c.Viva) continue;

                if (c.Especie == Especie.PeixNormal) peixos++;
                else if (c.Especie == Especie.Tauro) taurons++;
                else if (c.Especie == Especie.Pop) pops++;
                else if (c.Especie == Especie.Tortuga) tortugues++;
            }

            Console.WriteLine("Ronda " + ronda + " -> Peixos: " + peixos +
                              " | Taurons: " + taurons +
                              " | Pops: " + pops +
                              " | Tortugues: " + tortugues);
        }

        public void MostraResultatsFinalsDetallats()
        {
            int peixos = 0, taurons = 0, pops = 0, tortugues = 0;
            int peixosM = 0, peixosF = 0;
            int tauronsM = 0, tauronsF = 0;
            int tortuguesM = 0, tortuguesF = 0;

            for (int i = 0; i < _criatures.Count; i++)
            {
                Criatura c = _criatures[i];
                if (!c.Viva) continue;

                if (c.Especie == Especie.PeixNormal)
                {
                    peixos++;
                    if (c.Sexe == Sexe.Mascle) peixosM++;
                    else if (c.Sexe == Sexe.Femella) peixosF++;
                }
                else if (c.Especie == Especie.Tauro)
                {
                    taurons++;
                    if (c.Sexe == Sexe.Mascle) tauronsM++;
                    else if (c.Sexe == Sexe.Femella) tauronsF++;
                }
                else if (c.Especie == Especie.Pop)
                {
                    pops++;
                }
                else if (c.Especie == Especie.Tortuga)
                {
                    tortugues++;
                    if (c.Sexe == Sexe.Mascle) tortuguesM++;
                    else if (c.Sexe == Sexe.Femella) tortuguesF++;
                }
            }

            Console.WriteLine("\nRESULTATS FINALS DETALLATS:");
            Console.WriteLine("Peixos normals: " + peixos + " (M " + peixosM + ", F " + peixosF + ")");
            Console.WriteLine("Taurons: " + taurons + " (M " + tauronsM + ", F " + tauronsF + ")");
            Console.WriteLine("Pops: " + pops);
            Console.WriteLine("Tortugues: " + tortugues + " (M " + tortuguesM + ", F " + tortuguesF + ")");
        }

        private Posicio PosicioAleatoria()
        {
            int x = _random.Next(0, _peixera.Amplada);
            int y = _random.Next(0, _peixera.Alcada);
            return new Posicio(x, y);
        }

        private Direccio DireccioAleatoria()
        {
            int v = _random.Next(0, 4);
            if (v == 0) return Direccio.Amunt;
            if (v == 1) return Direccio.Avall;
            if (v == 2) return Direccio.Esquerra;
            return Direccio.Dreta;
        }

        private Posicio PosicioPerimetreAleatoria()
        {
            int L = _peixera.LongitudPerimetre();
            int index = _random.Next(0, L);
            return _peixera.IndexPerimetreAPosicio(index);
        }
    }