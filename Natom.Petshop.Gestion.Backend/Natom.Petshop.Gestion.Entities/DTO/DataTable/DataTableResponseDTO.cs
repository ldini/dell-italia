﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Natom.Petshop.Gestion.Entities.DTO.DataTable
{
    public class DataTableResponseDTO<TRecord>
    {
        [JsonProperty("recordsTotal")]
        public int RecordsTotal { get; set; }

        [JsonProperty("recordsFiltered")]
        public int RecordsFiltered { get; set; }

        [JsonProperty("extraData")]
        public object ExtraData { get; set; }

        [JsonProperty("records")]
        public List<TRecord> Records { get; set; }
    }
}
