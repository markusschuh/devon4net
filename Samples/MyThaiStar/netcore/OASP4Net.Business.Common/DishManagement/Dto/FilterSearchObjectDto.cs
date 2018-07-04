using Newtonsoft.Json;
using OASP4Net.Infrastructure.MVC.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OASP4Net.Business.Common.DishManagement.Dto
{
    public class FilterDtoSearchObject
    {
        [JsonProperty(PropertyName = "categories")]
        public CategorySearchDto[] Categories { get; set; }

        [JsonProperty(PropertyName = "searchBy")]
        public string SearchBy { get; set; }

        [JsonProperty(PropertyName = "sort")]
        public SortByDto[] sort { get; set; }

        [JsonProperty(PropertyName = "maxPrice")]
        public string MaxPrice { get; set; }

        [JsonProperty(PropertyName = "minLikes")]
        public string MinLikes { get; set; }

        public FilterDtoSearchObject()
        {
            MaxPrice = "0";
            MinLikes = "0";
            SearchBy = string.Empty;
            sort = new SortByDto[0];
        }

        public decimal GetMaxPrice()
        {
            return string.IsNullOrEmpty(MaxPrice) ? 0 : Convert.ToDecimal(MaxPrice);
        }

        public int GetMinLikes()
        {
            return string.IsNullOrEmpty(MinLikes) ? 0 : Convert.ToInt32(MinLikes);
        }

        public string GetSearchBy()
        {
            return SearchBy ?? string.Empty;
        }

        public IList<long> GetCategories()
        {
            var categories = Categories ?? new CategorySearchDto[0];
            return categories.Select(c => c.Id).ToList();
        }

    }
}
