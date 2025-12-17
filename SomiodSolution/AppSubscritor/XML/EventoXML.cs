using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSubscritor.XML
{
    [Serializable]
    public class EventoXml
    {
        public string Tipo { get; set; }
        public int Minuto { get; set; }
        public string Equipa { get; set; }
        public string Jogador { get; set; } // golo/cartão
        public string Sai { get; set; }     // sub
        public string Entra { get; set; }   // sub
        public string Cartao { get; set; }  // amarelo/vermelho
        
    }

}
