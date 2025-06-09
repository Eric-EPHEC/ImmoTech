using Domain.Entities;

namespace Application.Commands.Agency
{
    public class CreateAgencyCommand
    {
        public string Name { get; set; }
        public Address Address { get; set; }
        public string ContactEmail { get; set; }
    }
} 