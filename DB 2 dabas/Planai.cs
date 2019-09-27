using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_2_dabas
{
    public class Planai
    {
        public int id { get; set; }
        public string PlanoPavadinimas { get; set; }
        public string Operatorius { get; set; }
        private int _Pokalbiai;

        public int Pokalbiai
        {
            get
            {
                return _Pokalbiai;
            }
            set { _Pokalbiai = value; }
        }
        private int _Sms;

        public int Sms
        {
            get
            {
                return _Sms;
            }
            set { _Sms = value; }
        }
        private int _Internetas;

        public int Internetas
        {
            get
            {
                return _Internetas;
            }
            set { _Internetas = value; }
        }
        public double Kaina { get; set; }
        public bool selected { get; set; }

        public Planai()
        {
            selected = false;
            Sms = 0;
            Pokalbiai = 0;
            Internetas = 0;
            Kaina = 0;
        }
    }
}
