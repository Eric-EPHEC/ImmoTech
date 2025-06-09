using System;
using Domain.Entities;

namespace Application.Commands.Property
{
    public class CreatePropertyCommand
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
        public int? AgencyId { get; set; }
    }
} 