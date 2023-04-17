using FluentMigrator;

namespace Agora.Catalog.Infrastructure.Data.Migrations;

// TODO: name
[Migration(120319238912)]
public sealed class Initial : Migration
{
    private new const string Schema = "catalog";
    private const string CategoryTable = "category";
    private const string ProductTable = "product";
    private const string StoreTable = "store";

    public override void Up()
    {
        Create.Schema(Schema);

        Create.Sequence("category_id_seq")
            .InSchema(Schema);

        Create.Table(CategoryTable)
            .InSchema(Schema)
            .WithColumn("id").AsInt32().PrimaryKey() // With default next?
            .WithColumn("name").AsString(64).Unique().NotNullable() // ? Unsure about length.
            .WithColumn("description").AsString(255).Nullable()
            .WithColumn("image_url").AsString().Nullable()
            .WithColumn("parent_id").AsInt32().Nullable();

        Create.Table(ProductTable)
            .InSchema(Schema)
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("title").AsString().NotNullable()
            .WithColumn("description").AsString().NotNullable();

        Create.Table(StoreTable)
            .InSchema(Schema)
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("name").AsString().Unique().NotNullable() // TODO: Add max value also not nullable required?
            .WithColumn("lang").AsString(16).NotNullable();
    }

    public override void Down()
    {
        Delete.Table(CategoryTable).InSchema(Schema);
        Delete.Table(ProductTable).InSchema(Schema);
        Delete.Table(StoreTable).InSchema(Schema);

        Delete.Schema(Schema);
    }
}