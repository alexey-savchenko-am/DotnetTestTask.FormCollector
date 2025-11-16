using FormCollector.Application.Abstract;
using FormCollector.Domain;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Endpoint;

namespace FormCollector.Api.Submissions;

public class SearchSubmissionById(ISubmissionReader submissionReader)
    : IEndpoint<IResult, Guid>
{
    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/submissions/{id}", ([FromRoute] Guid id) => HandleAsync(id))
            .Produces<Submission>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("SearchSubmissionById")
            .WithTags("Submission Enpoints");
    }

    public async Task<IResult> HandleAsync(Guid request)
    {
        try
        {
            var submission = await submissionReader.SearchByIdAsync(request).ConfigureAwait(false);

            if (submission is not null)
            {
                return Results.Ok(submission);
            }

            return Results.NotFound();
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
