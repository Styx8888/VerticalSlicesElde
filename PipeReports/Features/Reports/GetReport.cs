using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PipeReports.API.Contracts.Report;
using PipeReports.API.Database;
using PipeReports.API.Shared.Results;

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
                .Where(x => x.Id == request.Id)
                .Select(x => new ReportResponse(x.Id,
                                                x.Client,
                                                x.Email)
                )
                .FirstOrDefaultAsync(cancellationToken);

            if (reportResponse is null)
                return Error.NotFound($"Report with id = {request.Id} not found.");
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

            return result.Match(
                () => Results.Ok(result.Value),
                error => Results.NotFound(error));

        });
    }
}
