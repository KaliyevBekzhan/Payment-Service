using Application.Dto;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IGetTopupInfoUseCase
{
    Task<Result<TopupInfoDto>> ExecuteAsync(int topupId);
}