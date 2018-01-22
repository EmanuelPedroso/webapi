using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using newProject.Models;

namespace newProject.Controllers
{
    
    [Route("api/Hours")]
    public class HoursController : Controller
    {
        private IMemoryCache _date;

        public HoursController(IMemoryCache memoryCache)
        {
            _date = memoryCache;
        }

        [HttpGet("{date}")]
        public string Get()
        {
            string novaData = _date.Get<string>("novaData");
            return novaData.ToString();
        }

        [HttpPut("{changeDate}")]
        public object Put([FromForm]DataControl dataControl)
        {
            var dataRecebida = ChangeDate(dataControl.Date, dataControl.Op, dataControl.Minutes);
            _date.Set<string>("novaData", dataRecebida);

            return new { success = true };
        }

        public string ChangeDate(string date, char op, int minutes)
        {
            var day = date.Substring(0, date.IndexOf("/"));
            date = date.Replace(day + "/", "");
            var month = "00";
            if (date.Length == 10)
            {//corrige erro quando dia e mês são iguais
                month = day;
            }
            else
            {
                month = date.Substring(0, date.IndexOf("/"));
            }
            date = date.Replace(month + "/", "");
            var year = date.Substring(0, date.IndexOf(" "));
            date = date.Replace(year + " ", "");
            var hour = date.Substring(0, date.IndexOf(":"));
            date = date.Replace(hour + ":", "");
            var minute = date;

            var dateInMinutes = (Int32.Parse(day) * 1440) + (Int32.Parse(month) * 43800) + (Int32.Parse(year) * 525600) + (Int32.Parse(hour) * 60) + Int32.Parse(minute);
            var dateInMinutesIncreased = 0;
            if (op == '+')
            {
                dateInMinutesIncreased = dateInMinutes + minutes;
            }
            else if (op == '-')
            {
                dateInMinutesIncreased = dateInMinutes - minutes;
            }

            var valorEmMinutos = dateInMinutesIncreased;
            var data = new Data();
            data.Ano = (valorEmMinutos / 525600).ToString();
            valorEmMinutos = valorEmMinutos % 525600;
            var testaMes = (valorEmMinutos / 43800).ToString();
            var dias=0;
            var posMesGrande = false;
            switch (testaMes)
            {
                case "1":
                    dias = 31;
                    posMesGrande = true;
                    break;
                case "3":
                case "5":
                case "7":
                case "8":
                case "10":
                case "12":
                    dias = 31;
                    break;
                case "2":
                    dias = 28;
                    posMesGrande = true;
                    break;
                default:
                    dias = 30;
                    posMesGrande = true;
                    break;
            }
            data.Mes = testaMes;
            valorEmMinutos = valorEmMinutos % 43800;

            //verifica quantos dias
            var diasRestantes = (valorEmMinutos / 1440).ToString();
            if (Int32.Parse(diasRestantes) <= dias)
            {
                data.Dia = diasRestantes;
            }
            else
            {
                var sobra = Int32.Parse(diasRestantes) - dias;
                data.Dia = sobra.ToString();
                valorEmMinutos = valorEmMinutos + (Int32.Parse(data.Dia) * 1440);
                var mesAdicional = Int32.Parse(data.Mes) + 1;
                data.Mes = mesAdicional.ToString();
            }
            var mesAnterior = 0;
            if (data.Dia == "0")
            {
                if (posMesGrande)
                {
                    data.Dia = "31";
                    mesAnterior = Int32.Parse(data.Mes) - 1;
                    data.Mes = mesAnterior.ToString();
                }
                else
                {
                    data.Dia = "01";
                }
            }
            else
            {
                mesAnterior = Int32.Parse(month);
            }
            valorEmMinutos = valorEmMinutos % 1440;
            if (day == "31")
            {
                valorEmMinutos = valorEmMinutos - 840;
            }
            data.Hora = (valorEmMinutos / 60).ToString();
            valorEmMinutos = valorEmMinutos % 60;
            data.Minuto = (valorEmMinutos).ToString();
            return data.MontaData();
        }
    }

   
}