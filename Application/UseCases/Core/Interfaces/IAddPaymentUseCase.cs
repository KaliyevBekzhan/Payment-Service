using Application.Dto;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IAddPaymentUseCase
{
    Task<Result> ExecuteAsync(AddPaymentDto dto);
}