using CsvHelper;
using JuntosCodeChallenge.Domain.Customer.DTO;
using JuntosCodeChallenge.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;

namespace JuntosCodeChallenge.Infrastructure.CrossCutting
{
    public class FileHelper
    {
        public List<CustomerDTO> GetDynamicOnlineCSV(string url)
        {
            LogHelper log = new LogHelper();
            List<CustomerDTO> customers = new List<CustomerDTO>();

            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                log.Information($"Obtendo o CSV online. URL: {url}");
                using (var reader = new StreamReader(resp.GetResponseStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.RegisterClassMap<CustomerCSVDTOMap>();
                    customers = csv.GetRecords<CustomerDTO>().ToList();
                }
                log.Information("CSV obtido e lista carregada.");
            }
            catch (Exception)
            {
                throw;
            }

            return customers;
        }
    }
}