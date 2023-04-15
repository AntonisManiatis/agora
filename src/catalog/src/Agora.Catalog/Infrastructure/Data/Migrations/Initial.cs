using FluentMigrator;

namespace Agora.Catalog.Infrastructure.Data.Migrations;

// TODO: name
[Migration(120319238912)]
public sealed class Initial : Migration
{
    private new const string Schema = "catalog";
    private const string ProductTable = "product";
    private const string StoreTable = "store";

    public override void Up()
    {
        Create.Schema(Schema);

        Create.Table("product")
            .InSchema(Schema)
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("title").AsString().NotNullable()
            .WithColumn("description").AsString().NotNullable();

        Create.Table("store")
            .InSchema(Schema)
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("name").AsString().Unique().NotNullable() // TODO: Add max value also not nullable required?
            .WithColumn("lang").AsString(16).NotNullable();
    }

    public override void Down()
    {
        Delete.Table("product").InSchema(Schema);
        Delete.Table("store").InSchema(Schema);

        Delete.Schema(Schema);
    }
}