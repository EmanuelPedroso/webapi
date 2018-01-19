using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace newProject.Models
{
    public class Data
    {
        public Data(string dia, string mes, string ano, string hora, string minuto)
        {
            Dia = dia;
            Mes = mes;
            Ano = ano;
            Hora = hora;
            Minuto = minuto;
        }

        public Data()
        {

        }

        public string Dia { get; set; }
        public string Mes { get; set; }
        public string Ano { get; set; }
        public string Hora { get; set; }
        public string Minuto { get; set; }

        public string MontaData()
        {
            return $"{this.Dia}/{this.Mes}/{this.Ano} {this.Hora}:{this.Minuto}";
        }
    }

}
