using FormCollector.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace FormCollector.Infrastructure.Data.Config;

internal sealed class SubmissionConfig
    : IEntityTypeConfiguration<Submission>
{
    public void Configure(EntityTypeBuilder<Submission> builder)
    {
        builder.ToTable("submissions");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasColumnName("id")
            .HasConversion(submissionId => submissionId.Key, key => new SubmissionId(key))
            .ValueGeneratedNever()
            .IsRequired();

        builder.OwnsOne(s => s.FormData, data =>
        {
            data.Property(data => data.Id)
                .HasColumnName("form_id")
                .HasConversion(formId => formId.Value, val => new FormId(val))
                .IsRequired();

            data.Property(data => data.Name)
                .HasColumnName("form_name")  
                .IsRequired(false);
        });

        builder.OwnsOne(s => s.Payload, payload =>
        {
            payload.Property(p => p.Flattened)
                .HasColumnName("flattened_text")
                .IsRequired();

            payload.Property(p => p.Json)
                .HasColumnName("json_payload")
                .HasConversion(
                    v => v.RootElement.GetRawText(),
                    v => JsonDocument.Parse(v, new JsonDocumentOptions())
                )
                .IsRequired();
        });

        builder.Property(s => s.CreatedOnUtc)
            .HasColumnName("created_on_utc")
            .HasColumnType("timestamptz")
            .IsRequired();
    }
}
