using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PipeReports.API.Contracts;
using PipeReports.API.Database;
using PipeReports.API.Shared;

namespace PipeReports.API.Features.Reports;

public static class GetReport
{
    public sealed record Query(int Id) : IRequest<Result<ReportResponse>>;

    internal sealed class Handler : IRequestHandler<Query, Result<ReportResponse>>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<ReportResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var reportResponse = await _dbContext
                .Reports
                .AsNoTracking()
                .Select(x => new ReportResponse(x.Id,
                                                x.Client,
                                                x.Email)
                )
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

            if (reportResponse is null)
                return Result.Failure<ReportResponse>(new Error("GetReport.NotFound", $"Report with id = {request.Id} not found."));
            return reportResponse;
        }
    }
}

public class GetReportEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/reports/{id:int}", async (int id, ISender sender) =>
        {
            var query = new GetReport.Query(id);

            var result = await sender.Send(query);

            if (result.IsFailure)
            {
                return Results.NotFound(result.Error);
            }

            return Results.Ok(result.Value);
        });
    }
}
