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
        var principal = _tokenGenerator.GetPrincipalFromExpiredToken(request.Token);
        if (principal == null)
        {
            return Result<AuthResponseDto>.Failure(new Error("Auth.InvalidToken", "Invalid access token."));
        }

        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email))
        {
            return Result<AuthResponseDto>.Failure(new Error("Auth.InvalidToken", "Claims are missing from token."));
        }

        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
        if (user == null)
        {
            return Result<AuthResponseDto>.Failure(new Error("Auth.UserNotFound", "User associated with token not found."));
        }

        var existingRefreshToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == request.RefreshToken);
        if (existingRefreshToken == null || existingRefreshToken.IsExpired || existingRefreshToken.IsRevoked)
        {
            return Result<AuthResponseDto>.Failure(new Error("Auth.InvalidRefreshToken", "Invalid, expired or revoked refresh token."));
        }

        // Rotate Refresh Token
        existingRefreshToken.IsRevoked = true;
        
        var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
        var newAccessToken = _tokenGenerator.GenerateToken(user, roles, Enumerable.Empty<Claim>());
        var newRefreshTokenStr = _tokenGenerator.GenerateRefreshToken();

        var newRefreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshTokenStr,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        user.RefreshTokens.Add(newRefreshToken);
        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var userDto = new UserDto(user.Id, user.Email, user.FirstName, user.LastName);
        return Result<AuthResponseDto>.Success(new AuthResponseDto(newAccessToken, newRefreshTokenStr, DateTime.UtcNow.AddHours(1), userDto));
    }

    public async Task<Result<bool>> LogoutAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(refreshToken, cancellationToken);
        if (user == null)
        {
            return Result<bool>.Success(true); // Already logged out or token invalid
        }

        var tokenRecord = user.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken);
        if (tokenRecord != null)
        {
            tokenRecord.IsRevoked = true;
            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            return Result<bool>.Failure(new Error("Auth.UserNotFound", "User not found."));
        }

        if (!_passwordHasher.VerifyPassword(currentPassword, user.PasswordHash))
        {
            return Result<bool>.Failure(new Error("Auth.InvalidCurrentPassword", "Incorrect current password."));
        }

        user.PasswordHash = _passwordHasher.HashPassword(newPassword);
        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
