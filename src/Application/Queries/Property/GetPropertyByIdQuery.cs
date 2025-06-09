using System;
using Domain.Entities;

namespace Application.Queries.Property
{
    public class GetPropertyByIdQuery
    {
        public Guid Id { get; set; }
    }

    public class PropertyDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public string Location { get; set; }
        public float Price { get; set; }
        public PropertyStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int AgencyId { get; set; }
        public AgencyDto Agency { get; set; }
        public List<PhotoDto> Photos { get; set; }
    }
} 