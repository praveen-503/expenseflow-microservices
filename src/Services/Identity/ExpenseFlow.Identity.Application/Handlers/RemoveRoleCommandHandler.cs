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

public class RemoveRoleCommandHandler : IRequestHandler<RemoveRoleCommand, Result<bool>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveRoleCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Result<bool>.Failure(new Error("Auth.UserNotFound", "User not found."));
        }

        var role = await _roleRepository.GetByNameAsync(request.RoleName, cancellationToken);
        if (role == null)
        {
            return Result<bool>.Failure(new Error("Roles.NotFound", "Role not found."));
        }

        var userWithRoles = await _userRepository.GetByEmailAsync(user.Email, cancellationToken);
        var userRoleRecord = userWithRoles?.UserRoles.FirstOrDefault(ur => ur.RoleId == role.Id);
        if (userRoleRecord == null)
        {
            return Result<bool>.Success(true); // Not assigned
        }

        userWithRoles!.UserRoles.Remove(userRoleRecord);
        _userRepository.Update(userWithRoles);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
