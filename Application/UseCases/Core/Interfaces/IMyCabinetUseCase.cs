using Application.Dto.Returns;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IMyCabinetUseCase
{
    Task<Result<MyCabinetDto>> ExecuteAsync(int userId);
}