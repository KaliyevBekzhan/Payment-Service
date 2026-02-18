namespace PaymentServiceApi.Dto;

public record AddRoleRequest(string Name, bool IsAdmin, int Priority);