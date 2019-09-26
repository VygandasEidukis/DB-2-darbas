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
                if (_Pokalbiai == 800000)
                    return int.MaxValue;
                return _Pokalbiai;
            }
            set { _Pokalbiai = value; }
        }
        private int _Sms;

        public int Sms
        {
            get
            {
                if (_Sms == 800000)
                    return int.MaxValue;
                return _Sms;
            }
            set { _Sms = value; }
        }
        private int _Internetas;

        public int Internetas
        {
            get
            {
                if (_Internetas == 800000)
                    return int.MaxValue;
                return _Internetas;
            }
            set { _Internetas = value; }
        }
        public double Kaina { get; set; }
        public bool selected { get; set; }

        public Planai()
        {
            selected = false;
        }
    }
}
