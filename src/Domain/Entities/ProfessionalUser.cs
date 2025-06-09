namespace Domain.Entities;

public class ProfessionalUser : User
{
    public int AgencyId { get; set; }
    public Agency? Agency { get; set; }

}