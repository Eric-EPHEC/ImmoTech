using Domain.Entities;

namespace Application.Commands.Agency
{
    public class UpdateAgencyCommand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public string ContactEmail { get; set; }
    }
} 