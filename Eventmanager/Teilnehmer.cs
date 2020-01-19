using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventmanager
{
    public class Teilnehmer
    {
        public string Startnummer { get; set; }

        public string Riege { get; set; }

        public string Starter { get; set; }

        public string Verein { get; set; }

        public string Disziplin { get; set; }

        public string Klasse { get; set; }

        public int Platz { get; set; }

        public Punkte Punkte { get; set; }

        public double PunkteGesamt { get; set; }

        public int KlasseGesamt { get; set; }

        public DateTime? StartVorbereitung { get; set; }

        public DateTime? StartWettkampf { get; set; }

        public DateTime? Datum { get; set; }

        public string Turniername { get; set; }

        public string Ort { get; set; }

        public string T1Name { get; set; }

        public string T2Name { get; set; }

        public string T3Name { get; set; }

        public string T4Name { get; set; }

        public string A1Name { get; set; }

        public string A2Name { get; set; }

        public string A3Name { get; set; }

        public string A4Name { get; set; }

        public string CJP { get; set; }

        public string Schwierigkeitskampfrichter { get; set; }

    }
}
