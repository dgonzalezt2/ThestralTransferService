namespace ThestralService.Domain.Transfer.Dtos;

public class UserTransferCompleteDto
{
    public Guid TicketId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
