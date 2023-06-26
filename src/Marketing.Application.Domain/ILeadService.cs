namespace Marketing.Application.Domain
{
    public interface ILeadService
    {
        Task SubmitLeadAsync(Lead lead);
    }
}