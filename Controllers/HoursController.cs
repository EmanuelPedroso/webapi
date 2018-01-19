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
        private IMemoryCache date;

        public HoursController(IMemoryCache memoryCache)
        {
            date = memoryCache;
        }

        [HttpGet("{date}")]
        public string Get()
        {
            string novaData = date.Get<string>("novaData");
            return novaData.ToString();
        }

        [HttpPut("{changeDate}")]
        public object Put([FromForm]DataControl dataControl)
        {
            var dataRecebida = ChangeDate(dataControl.Date, dataControl.Op, dataControl.Minutes);
            date.Set<string>("novaData", dataRecebida);

            return new { success = true };
        }

        public string ChangeDate(string date, char op, int minutes)
        {
            var day = date.Substring(0, date.IndexOf("/"));
            date = date.Replace(day + "/", "");
            var month = date.Substring(0, date.IndexOf("/"));
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
            data.Mes = (valorEmMinutos / 43800).ToString();
            valorEmMinutos = valorEmMinutos % 43800;
            data.Dia = (valorEmMinutos / 1440).ToString();
            valorEmMinutos = valorEmMinutos % 1440;
            data.Hora = (valorEmMinutos / 60).ToString();
            valorEmMinutos = valorEmMinutos % 60;
            data.Minuto = (valorEmMinutos).ToString();

            return data.MontaData();
        }

    }

   
}