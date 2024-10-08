﻿using Carter;
using FluentValidation;
using MediatR;
using PipeReports.API.Contracts.Report;
using PipeReports.API.Database;
using PipeReports.API.Entities;
using PipeReports.API.Shared.Results;

namespace PipeReports.API.Features.Reports;

public static class CreateReport
{
    public sealed record Command() : IRequest<Result<int>>
    {
        public string Client { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Client).NotEmpty().WithMessage("Client property must have a value.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email property must have a value.");
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Result<int>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IValidator<Command> _validator;

        public Handler(ApplicationDbContext dbContext, IValidator<Command> validator)
        {
            _dbContext = dbContext;
            _validator = validator;
        }

        public async Task<Result<int>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
                return Error.Validation(validationResult.ToString());

            var report = new Report()
            {
                Client = request.Client,
                Email = request.Email,
            };

            await _dbContext.AddAsync(report, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return report.Id;
        }
    }
}

public class CreateReportEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/reports", async (CreateReportRequest request, ISender sender) =>
        {
            var command = new CreateReport.Command()
            {
                Client = request.Client,
                Email = request.Email,
            };

            var result = await sender.Send(command);

            return result.Match(
               () => Results.Ok(result.Value),
               Results.BadRequest);
        });
    }
}
