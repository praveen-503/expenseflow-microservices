using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExpenseFlow.Identity.Api.Common;
using ExpenseFlow.Identity.Application.Commands;
using Asp.Versioning;

namespace ExpenseFlow.Identity.Api.Controllers;

[ApiVersion("1.0")]
[Authorize(Policy = "AdminOnly")]
public class AdminController : ApiController
{
    public AdminController(ISender sender) : base(sender)
    {
    }

    [HttpPost("roles")]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
    {
        var command = new CreateRoleCommand(request.Name);
        var result = await Sender.Send(command);
        return HandleResult(result);
    }

    [HttpPost("users/roles")]
    public async Task<IActionResult> AssignRole([FromBody] UserRoleRequest request)
    {
        var command = new AssignRoleCommand(request.UserId, request.RoleName);
        var result = await Sender.Send(command);
        return HandleResult(result);
    }

    [HttpDelete("users/roles")]
    public async Task<IActionResult> RemoveRole([FromBody] UserRoleRequest request)
    {
        var command = new RemoveRoleCommand(request.UserId, request.RoleName);
        var result = await Sender.Send(command);
        return HandleResult(result);
    }

    [HttpPost("users/claims")]
    [Authorize(Policy = "CanManageClaims")]
    public async Task<IActionResult> AssignClaim([FromBody] UserClaimRequest request)
    {
        var command = new AssignClaimCommand(request.UserId, request.ClaimType, request.ClaimValue);
        var result = await Sender.Send(command);
        return HandleResult(result);
    }

    [HttpDelete("users/claims")]
    [Authorize(Policy = "CanManageClaims")]
    public async Task<IActionResult> RemoveClaim([FromBody] UserClaimRequest request)
    {
        var command = new RemoveClaimCommand(request.UserId, request.ClaimType, request.ClaimValue);
        var result = await Sender.Send(command);
        return HandleResult(result);
    }
}

public record CreateRoleRequest(string Name);
public record UserRoleRequest(Guid UserId, string RoleName);
public record UserClaimRequest(Guid UserId, string ClaimType, string ClaimValue);
