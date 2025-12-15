using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppArbitro
{
        public class GoloData
        {
            public int Minuto { get; set; }
            public string Jogador { get; set; }
            public string Equipa { get; set; }
        }

        public class CartaoData
        {
            public int Minuto { get; set; }
            public string Jogador { get; set; }
            public string Equipa { get; set; }
            public string TipoCartao { get; set; } // amarelo | vermelho
        }

        public class SubData
        {
            public int Minuto { get; set; }
            public string Sai { get; set; }
            public string Entra { get; set; }
            public string Equipa { get; set; }
        }
    
}
