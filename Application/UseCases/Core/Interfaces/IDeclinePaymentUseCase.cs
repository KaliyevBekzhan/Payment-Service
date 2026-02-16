using Application.Dto;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IDeclinePaymentUseCase
{
    Task<Result> ExecuteAsync(UpdatePaymentStatusDto dto);
}