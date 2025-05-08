namespace ThestralService.Domain.Transfer.Entities;

using Domain.User.Dtos;
using SharedKernel;

public class Transfer : Entity
{
    private Transfer(
        Guid id,
        string userId,
        string externalOperatorId,
        string email
    ) : base(id)
    {
        Email = email;
        UserId = userId;
        ExternalOperatorId = externalOperatorId;
    }

    private Transfer() { }

    public string UserId { get; private set; }
    public string ExternalOperatorId { get; private set; }
    public string? UserName { get; private set; }
    public string Email { get; private set; }
    public Dictionary<string, string[]> Files { get; private set; } = [];
    public bool IsUserAuthUserDisabled { get; private set; }
    public bool IsFileManagerUserDisabled { get; private set; }
    public bool IsUserManagementUserDisabled { get; private set; }

    public void SetFiles(Dictionary<string,string[]> files)
    {
        Files = files;
    }

    public void SetUserManagementDisabled()
    {
        SetUpdated();
        IsUserManagementUserDisabled = true;
    }

    public void SetUserFileManagerDisabled()
    {
        SetUpdated();
        IsFileManagerUserDisabled = true; 
    }

    public void SetUserAuthDisabled()
    {
        SetUpdated();
        IsUserAuthUserDisabled = true;
    }

    public bool IsReadyToBeTransferred() => IsUserAuthUserDisabled && IsFileManagerUserDisabled && IsUserManagementUserDisabled;

    public void Update(UserManagementTransferResponseDto userManagementTransferResponseDto)
    {
        UserName = userManagementTransferResponseDto.Name;
    }

    public static Transfer BuildFromUserTransferRequest(string userId, UserTransferRequestDto userTransferRequestDto)
    {
        return new(Guid.NewGuid(),
            userId,
            userTransferRequestDto.ExternalOperatorId,
            userTransferRequestDto.UserEmail);
    }
}
