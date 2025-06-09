using Domain.Entities;

namespace Application.Queries.Agency
{
    public class GetAgencyByIdQuery
    {
        public int Id { get; set; }
    }

    public class AgencyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public string ContactEmail { get; set; }
    }
} 