using System;
using Domain.Entities;

namespace Application.Commands.Property
{
    public class UpdatePropertyCommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public string Location { get; set; }
        public float Price { get; set; }
        public PropertyStatus Status { get; set; }
        public int AgencyId { get; set; }
    }
} 