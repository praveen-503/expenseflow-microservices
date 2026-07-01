using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Identity.Domain.Common;
using ExpenseFlow.Identity.Domain.Entities;
using ExpenseFlow.Identity.Domain.Interfaces;
using ExpenseFlow.Identity.Application.DTOs;
using ExpenseFlow.Identity.Application.Interfaces;

namespace ExpenseFlow.Identity.Infrastructure.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public AuthenticationService(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator tokenGenerator,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AuthResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            return Result<AuthResponseDto>.Failure(new Error("Auth.InvalidCredentials", "Invalid email or password."));
        }

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            return Result<AuthResponseDto>.Failure(new Error("Auth.InvalidCredentials", "Invalid email or password."));
        }

        var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
        var token = _tokenGenerator.GenerateToken(user, roles, Enumerable.Empty<Claim>());
        var refreshTokenStr = _tokenGenerator.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshTokenStr,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        user.RefreshTokens.Add(refreshToken);
        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var userDto = new UserDto(user.Id, user.Email, user.FirstName, user.LastName);
        return Result<AuthResponseDto>.Success(new AuthResponseDto(token, refreshTokenStr, DateTime.UtcNow.AddHours(1), userDto));
    }

    public async Task<Result<AuthResponseDto>> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser != null)
        {
            return Result<AuthResponseDto>.Failure(new Error("Auth.EmailTaken", "Email is already in use."));
        }

        var passwordHash = _passwordHasher.HashPassword(request.Password);
        var user = new User
        {
            Email = request.Email,
            PasswordHash = passwordHash,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var token = _tokenGenerator.GenerateToken(user, Enumerable.Empty<string>(), Enumerable.Empty<Claim>());
        var refreshTokenStr = _tokenGenerator.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshTokenStr,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        user.RefreshTokens.Add(refreshToken);
        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var userDto = new UserDto(user.Id, user.Email, user.FirstName, user.LastName);
        return Result<AuthResponseDto>.Success(new AuthResponseDto(token, refreshTokenStr, DateTime.UtcNow.AddHours(1), userDto));
    }

    public async Task<Result<AuthResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default)
    {
        // Simple logic stub for refresh token verification
        return Result<AuthResponseDto>.Failure(new Error("Auth.InvalidRefreshToken", "Invalid or expired refresh token."));
    }
}
