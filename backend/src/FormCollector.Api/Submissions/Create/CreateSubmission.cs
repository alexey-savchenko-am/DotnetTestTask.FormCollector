using FormCollector.Application.Abstract;
using FormCollector.Application.Submissions;
using FormCollector.Domain;
using MinimalApi.Endpoint;

namespace FormCollector.Api.Submissions.Create;

public class CreateSubmission(ISubmissionWriter submissionWriter)
    : IEndpoint<IResult, SubmissionCreateDto>
{
    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/submissions", this.HandleAsync)
           .Produces<Submission>(StatusCodes.Status201Created)
           .Produces(StatusCodes.Status400BadRequest)
           .WithName("CreateSubmission");
    }

    public async Task<IResult> HandleAsync(SubmissionCreateDto request)
    {
        if (request is null)
            return Results.BadRequest("SubmissionDto cannot be null.");

        try
        {
            var submission = await submissionWriter
                .CreateAsync(request, CancellationToken.None)
                .ConfigureAwait(false);

            return Results.Created($"/api/submissions/{submission.Id}", submission);
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
