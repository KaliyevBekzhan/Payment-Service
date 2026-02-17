using Application.Dto;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IAcceptPaymentUseCase
{
    Task<Result> ExecuteAsync(UpdatePaymentStatusDto dto);
}