using Application.Dto.Returns;
using Domain.Entity;
using FluentResults;

namespace Application.UseCases.Interfaces;

public interface IGetPaymentsForAdminUseCase
{
    Task<Result<IEnumerable<AdminPaymentsDto>>> ExecuteAsync(int userId);
}