using System.Collections.Generic;
using Domain.Entities;

namespace Application.Queries.Agency
{
    public class GetAllAgenciesQuery
    {
        // Can add filtering parameters here if needed
    }

    public class AgencyListDto
    {
        public List<GetAgencyByIdResponse> Agencies { get; set; }
        public int TotalCount { get; set; }
    }
} 