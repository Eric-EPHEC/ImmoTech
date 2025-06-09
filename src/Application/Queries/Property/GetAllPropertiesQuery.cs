using System.Collections.Generic;
using Domain.Entities;

namespace Application.Queries.Property
{
    public class GetAllPropertiesQuery
    {
        public float? MinPrice { get; set; }
        public float? MaxPrice { get; set; }
        public string Location { get; set; }
        public PropertyStatus? Status { get; set; }
        public int? AgencyId { get; set; }
    }

    public class PropertyListDto
    {
        public List<PropertyDto> Properties { get; set; }
        public int TotalCount { get; set; }
    }
} 