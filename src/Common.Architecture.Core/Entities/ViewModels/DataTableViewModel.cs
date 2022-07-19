using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Architecture.Core.Entities.ViewModels
{
    public class DataTableViewModel
    {
        public long? UstId { get; set; }
        public DateTime? BasTar { get; set; }
        public DateTime? BitTar { get; set; }
        public string SortColumn { get; set; }
        public string SortColumnDirection { get; set; }
        public string SearchValue { get; set; }
        public int Skip { get; set; }
        public int PageSize { get; set; }
        public string Draw { get; set; }
        public List<object> CData { get; set; }


        public DataTableViewModel()
        {
        }

        public DataTableViewModel(HttpRequest request)
        {
            Draw = request.Form["draw"].FirstOrDefault();
            var start = request.Form["start"].FirstOrDefault();
            var length = request.Form["length"].FirstOrDefault();
            SortColumn = request.Form["columns[" + request.Form["order[0][column]"].FirstOrDefault() + "][data]"].FirstOrDefault();
            SortColumnDirection = request.Form["order[0][dir]"].FirstOrDefault();
            SearchValue = request.Form["search[value]"].FirstOrDefault();
            PageSize = Math.Min(length != null ? Convert.ToInt32(length) : 10, 1000);
            Skip = start != null ? Convert.ToInt32(start) : 0;
            // CData = request.Form["CData"].Count() >0 ? request.Form["CData"].ToList() : null ;
        }

        public DataTableViewModel(int pageSize, int skip)
        {
            Draw = null;
            SortColumn = "name";
            SortColumnDirection = null;
            SearchValue = null;
            PageSize = Math.Min(pageSize, 1000);
            Skip = skip;
        }
    }
}
