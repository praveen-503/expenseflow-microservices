using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Expense.Api.Controllers;
using ExpenseFlow.Expense.Application.Commands;
using ExpenseFlow.Expense.Application.DTOs;
using ExpenseFlow.Expense.Application.Queries;
using ExpenseFlow.Expense.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace ExpenseFlow.Expense.Api.Tests.Controllers;

public class ExpensesControllerTests
{
    [Fact]
    public async Task Create_ReturnsUnauthorized_WhenUserIdClaimIsMissing()
    {
        var sender = new RecordingSender();
        var controller = CreateController(sender);

        var result = await controller.Create(new CreateExpenseRequest("Lunch", 12.50m, new DateTime(2026, 7, 28), "Team meal", Guid.NewGuid()));

        Assert.IsType<UnauthorizedResult>(result);
        Assert.Null(sender.LastRequest);
    }

    [Fact]
    public async Task Create_SendsCreateExpenseCommand_AndReturnsOkResult()
    {
        var sender = new RecordingSender
        {
            ResponseFactory = request => Result<Guid>.Success(Guid.Parse("11111111-1111-1111-1111-111111111111"))
        };
        var controller = CreateController(sender);
        var userId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        controller.SetUser(userId);
        var request = new CreateExpenseRequest("Lunch", 12.50m, new DateTime(2026, 7, 28), "Team meal", Guid.Parse("33333333-3333-3333-3333-333333333333"));

        var result = await controller.Create(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(Guid.Parse("11111111-1111-1111-1111-111111111111"), okResult.Value);
        var command = Assert.IsType<CreateExpenseCommand>(sender.LastRequest);
        Assert.Equal(request.Title, command.Title);
        Assert.Equal(request.Amount, command.Amount);
        Assert.Equal(request.ExpenseDate, command.ExpenseDate);
        Assert.Equal(request.Notes, command.Notes);
        Assert.Equal(request.CategoryId, command.CategoryId);
        Assert.Equal(userId, command.UserId);
    }

    [Fact]
    public async Task Update_SendsUpdateExpenseCommand_AndReturnsNoContent()
    {
        var sender = new RecordingSender
        {
            ResponseFactory = request => Result<bool>.Success(true)
        };
        var controller = CreateController(sender);
        var userId = Guid.Parse("44444444-4444-4444-4444-444444444444");
        controller.SetUser(userId);
        var id = Guid.Parse("55555555-5555-5555-5555-555555555555");
        var request = new UpdateExpenseRequest("Taxi", 45.00m, new DateTime(2026, 7, 28), "Airport transfer", Guid.Parse("66666666-6666-6666-6666-666666666666"));

        var result = await controller.Update(id, request);

        Assert.IsType<NoContentResult>(result);
        var command = Assert.IsType<UpdateExpenseCommand>(sender.LastRequest);
        Assert.Equal(id, command.Id);
        Assert.Equal(request.Title, command.Title);
        Assert.Equal(request.Amount, command.Amount);
        Assert.Equal(request.ExpenseDate, command.ExpenseDate);
        Assert.Equal(request.Notes, command.Notes);
        Assert.Equal(request.CategoryId, command.CategoryId);
        Assert.Equal(userId, command.UserId);
    }

    [Fact]
    public async Task Delete_SendsDeleteExpenseCommand_AndReturnsNoContent()
    {
        var sender = new RecordingSender
        {
            ResponseFactory = request => Result<bool>.Success(true)
        };
        var controller = CreateController(sender);
        var userId = Guid.Parse("77777777-7777-7777-7777-777777777777");
        controller.SetUser(userId);
        var id = Guid.Parse("88888888-8888-8888-8888-888888888888");

        var result = await controller.Delete(id);

        Assert.IsType<NoContentResult>(result);
        var command = Assert.IsType<DeleteExpenseCommand>(sender.LastRequest);
        Assert.Equal(id, command.Id);
        Assert.Equal(userId, command.UserId);
    }

    [Fact]
    public async Task GetSummary_SendsGetExpenseSummaryQuery_AndReturnsOkResult()
    {
        var sender = new RecordingSender
        {
            ResponseFactory = request => Result<ExpenseSummaryDto>.Success(
                new ExpenseSummaryDto(
                    120m,
                    new List<CategorySummaryDto>
                    {
                        new("Meals", 80m),
                        new("Travel", 40m)
                    },
                    80m,
                    40m,
                    60m,
                    70m,
                    50m))
        };
        var controller = CreateController(sender);
        var userId = Guid.Parse("99999999-9999-9999-9999-999999999999");
        controller.SetUser(userId);

        var result = await controller.GetSummary();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var summary = Assert.IsType<ExpenseSummaryDto>(okResult.Value);
        Assert.Equal(120m, summary.TotalExpense);
        Assert.Equal(2, summary.ExpensesByCategory.Count);
        Assert.Equal(60m, summary.AverageExpense);
        var query = Assert.IsType<GetExpenseSummaryQuery>(sender.LastRequest);
        Assert.Equal(userId, query.UserId);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenQueryFailsWithNotFoundError()
    {
        var sender = new RecordingSender
        {
            ResponseFactory = request => Result<ExpenseDto>.Failure(new Error("Expense.NotFound", "Expense was not found."))
        };
        var controller = CreateController(sender);
        var userId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        controller.SetUser(userId);
        var id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

        var result = await controller.GetById(id);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        var problemDetails = Assert.IsType<ProblemDetails>(notFound.Value);
        Assert.Equal(404, problemDetails.Status);
        Assert.Equal("Expense.NotFound", problemDetails.Title);
        var query = Assert.IsType<GetExpenseByIdQuery>(sender.LastRequest);
        Assert.Equal(id, query.Id);
        Assert.Equal(userId, query.UserId);
    }

    [Fact]
    public async Task GetAll_ReturnsBadRequest_WhenQueryFailsWithValidationError()
    {
        var sender = new RecordingSender
        {
            ResponseFactory = request => Result<IReadOnlyList<ExpenseDto>>.Failure(new Error("Expense.Validation", "Invalid request."))
        };
        var controller = CreateController(sender);
        var userId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
        controller.SetUser(userId);

        var result = await controller.GetAll();

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        var problemDetails = Assert.IsType<ProblemDetails>(badRequest.Value);
        Assert.Equal(400, problemDetails.Status);
        Assert.Equal("Expense.Validation", problemDetails.Title);
        var query = Assert.IsType<GetExpensesQuery>(sender.LastRequest);
        Assert.Equal(userId, query.UserId);
    }

    [Fact]
    public async Task UploadReceipt_ReturnsBadRequest_WhenFileIsMissing()
    {
        var sender = new RecordingSender();
        var controller = CreateController(sender);
        controller.SetUser(Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"));

        var result = await controller.UploadReceipt(Guid.NewGuid(), null!);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("File is required.", badRequest.Value);
        Assert.Null(sender.LastRequest);
    }

    [Fact]
    public async Task UploadReceipt_SendsUploadReceiptCommand_AndReturnsOkResult()
    {
        var sender = new RecordingSender
        {
            ResponseFactory = request => Result<string>.Success("https://storage.example.com/receipts/receipt.pdf")
        };
        var controller = CreateController(sender);
        var userId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
        controller.SetUser(userId);
        var expenseId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");
        var fileBytes = new byte[] { 1, 2, 3 };
        await using var stream = new MemoryStream(fileBytes);
        var file = new FormFile(stream, 0, fileBytes.Length, "file", "receipt.pdf")
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/pdf"
        };

        var result = await controller.UploadReceipt(expenseId, file);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("https://storage.example.com/receipts/receipt.pdf", okResult.Value);
        var command = Assert.IsType<UploadReceiptCommand>(sender.LastRequest);
        Assert.Equal(expenseId, command.ExpenseId);
        Assert.Equal(userId, command.UserId);
        Assert.Equal("receipt.pdf", command.FileName);
        Assert.Equal("application/pdf", command.ContentType);
        Assert.NotNull(command.FileStream);
    }

    [Fact]
    public async Task DeleteReceipt_SendsDeleteReceiptCommand_AndReturnsNoContent()
    {
        var sender = new RecordingSender
        {
            ResponseFactory = request => Result<bool>.Success(true)
        };
        var controller = CreateController(sender);
        var userId = Guid.Parse("12341234-1234-1234-1234-123412341234");
        controller.SetUser(userId);
        var id = Guid.Parse("43214321-4321-4321-4321-432143214321");

        var result = await controller.DeleteReceipt(id);

        Assert.IsType<NoContentResult>(result);
        var command = Assert.IsType<DeleteReceiptCommand>(sender.LastRequest);
        Assert.Equal(id, command.ExpenseId);
        Assert.Equal(userId, command.UserId);
    }

    private static ExpensesController CreateController(RecordingSender sender)
    {
        var controller = new ExpensesController(sender);
        controller.InitializeContext();
        return controller;
    }

    private sealed class RecordingSender : ISender
    {
        public object? LastRequest { get; private set; }
        public Func<object, object>? ResponseFactory { get; set; }

        public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : IRequest
        {
            LastRequest = request;
            return Task.CompletedTask;
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            LastRequest = request;
            if (ResponseFactory is null)
            {
                throw new InvalidOperationException("ResponseFactory must be set for this test.");
            }

            var response = ResponseFactory(request);
            if (response is Result genericResult && typeof(TResponse) == typeof(Result<bool>))
            {
                return Task.FromResult((TResponse)(object)Result<bool>.Success(genericResult.IsSuccess));
            }

            return Task.FromResult((TResponse)response);
        }

        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            LastRequest = request;
            return EmptyStream<TResponse>();
        }

        public Task<object?> Send(object request, CancellationToken cancellationToken = default)
        {
            LastRequest = request;
            if (ResponseFactory is null)
            {
                throw new InvalidOperationException("ResponseFactory must be set for this test.");
            }

            return Task.FromResult<object?>(ResponseFactory(request));
        }

        public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default)
        {
            LastRequest = request;
            return EmptyStream<object?>();
        }

        public Task Publish(object notification, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification => Task.CompletedTask;

        private static async IAsyncEnumerable<T> EmptyStream<T>()
        {
            yield break;
        }
    }
}

internal static class ControllerTestExtensions
{
    public static void SetUser(this ControllerBase controller, Guid userId)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                }, authenticationType: "Test"))
            }
        };
    }

    public static void InitializeContext(this ControllerBase controller)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity())
            }
        };
    }
}