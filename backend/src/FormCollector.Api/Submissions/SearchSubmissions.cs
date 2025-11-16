using FormCollector.Application.Abstract;
using FormCollector.Application.Submissions;
using FormCollector.Domain;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Endpoint;

namespace FormCollector.Api.Submissions;

public class SearchSubmissions(ISubmissionReader submissionReader)
    : IEndpoint<IResult, SubmissionSearchDto>
{
    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/submissions",
                ([FromQuery] string? formId,
                 [FromQuery] string? query,
                 [FromQuery] int page,
                 [FromQuery] int itemsPerPage) => 
                    HandleAsync(new SubmissionSearchDto(formId, query, page, itemsPerPage)))
            .Produces<Submission>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("SearchSubmissions")
            .WithTags("Submission Enpoints");
    }

    public async Task<IResult> HandleAsync(SubmissionSearchDto request)
    {
        try
        {
            var result = await submissionReader.SearchAsync(request).ConfigureAwait(false);
            return Results.Ok(result);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
        catch
        {
            return Results.Problem(
                title: "Service error",
                detail: "Unable to process request right now.",
                statusCode: StatusCodes.Status503ServiceUnavailable
            );
        }
    }
}
