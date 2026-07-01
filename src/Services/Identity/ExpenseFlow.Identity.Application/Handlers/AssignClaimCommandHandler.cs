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

public class AssignClaimCommandHandler : IRequestHandler<AssignClaimCommand, Result<bool>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserClaimRepository _claimRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignClaimCommandHandler(IUserRepository userRepository, IUserClaimRepository claimRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _claimRepository = claimRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(AssignClaimCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Result<bool>.Failure(new Error("Auth.UserNotFound", "User not found."));
        }

        var userWithClaims = await _userRepository.GetByEmailAsync(user.Email, cancellationToken);
        if (userWithClaims != null && userWithClaims.UserClaims.Any(uc => uc.ClaimType == request.ClaimType && uc.ClaimValue == request.ClaimValue))
        {
            return Result<bool>.Success(true); // Already assigned
        }

        var claim = new UserClaim
        {
            UserId = user.Id,
            ClaimType = request.ClaimType,
            ClaimValue = request.ClaimValue
        };

        userWithClaims!.UserClaims.Add(claim);
        _userRepository.Update(userWithClaims);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
