using Application.Dto;
using Application.Dto.Returns;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IGetTopupInfoUseCase
{
    Task<Result<ActionsDto>> ExecuteAsync(int topupId);
}