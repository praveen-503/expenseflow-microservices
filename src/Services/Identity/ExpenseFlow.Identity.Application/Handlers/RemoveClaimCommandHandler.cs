using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ExpenseFlow.Identity.Domain.Common;
using ExpenseFlow.Identity.Domain.Entities;
using ExpenseFlow.Identity.Domain.Interfaces;
using ExpenseFlow.Identity.Application.Commands;

namespace ExpenseFlow.Identity.Application.Handlers;

public class RemoveClaimCommandHandler : IRequestHandler<RemoveClaimCommand, Result<bool>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveClaimCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(RemoveClaimCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Result<bool>.Failure(new Error("Auth.UserNotFound", "User not found."));
        }

        var userWithClaims = await _userRepository.GetByEmailAsync(user.Email, cancellationToken);
        var claimRecord = userWithClaims?.UserClaims.FirstOrDefault(uc => uc.ClaimType == request.ClaimType && uc.ClaimValue == request.ClaimValue);
        if (claimRecord == null)
        {
            return Result<bool>.Success(true); // Not present
        }

        userWithClaims!.UserClaims.Remove(claimRecord);
        _userRepository.Update(userWithClaims);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
