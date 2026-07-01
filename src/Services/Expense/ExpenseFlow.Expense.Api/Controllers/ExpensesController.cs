using System;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExpenseFlow.Expense.Api.Common;
using ExpenseFlow.Expense.Application.Commands;
using ExpenseFlow.Expense.Application.Queries;
using Asp.Versioning;

namespace ExpenseFlow.Expense.Api.Controllers;

[ApiVersion("1.0")]
[Authorize]
public class ExpensesController : ApiController
{
    public ExpensesController(ISender sender) : base(sender)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateExpenseRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var command = new CreateExpenseCommand(request.Title, request.Amount, request.ExpenseDate, request.Notes, request.CategoryId, userId);
        var result = await Sender.Send(command);
        return HandleResult(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateExpenseRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var command = new UpdateExpenseCommand(id, request.Title, request.Amount, request.ExpenseDate, request.Notes, request.CategoryId, userId);
        var result = await Sender.Send(command);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var command = new DeleteExpenseCommand(id, userId);
        var result = await Sender.Send(command);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var query = new GetExpenseByIdQuery(id, userId);
        var result = await Sender.Send(query);
        return HandleResult(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var query = new GetExpensesQuery(userId);
        var result = await Sender.Send(query);
        return HandleResult(result);
    }
}

public record CreateExpenseRequest(string Title, decimal Amount, DateTime ExpenseDate, string Notes, Guid CategoryId);
public record UpdateExpenseRequest(string Title, decimal Amount, DateTime ExpenseDate, string Notes, Guid CategoryId);
