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
            .WithColumn("description").AsString(255).Nullable().WithDefaultValue("")
            .WithColumn("image_url").AsString().Nullable().WithDefaultValue("")
            .WithColumn("parent_id").AsInt32().Nullable();

        Create.Table("category_attribute")
            .InSchema(Schema)
            .WithColumn("id").AsInt32().Identity().PrimaryKey()
            .WithColumn("category_id").AsInt32().NotNullable()
            .WithColumn("name").AsString().Unique().NotNullable() // ? Length?
            .WithColumn("pick_one").AsBoolean().NotNullable().WithDefaultValue(false);

        Create.ForeignKey()
            .FromTable("category_attribute")
            .InSchema(Schema)
            .ForeignColumn("category_id")
            .ToTable("category")
            .InSchema(Schema)
            .PrimaryColumn("id")
            .OnDelete(System.Data.Rule.Cascade);

        Create.Table("category_attribute_option")
            .InSchema(Schema)
            .WithColumn("id").AsInt32().Identity().PrimaryKey()
            .WithColumn("attribute_id").AsInt32().NotNullable()
            .WithColumn("name").AsString().Unique().NotNullable(); // ? Length?

        Create.ForeignKey()
            .FromTable("category_attribute_option")
            .InSchema(Schema)
            .ForeignColumn("attribute_id")
            .ToTable("category_attribute")
            .InSchema(Schema)
            .PrimaryColumn("id")
            .OnDelete(System.Data.Rule.Cascade);

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
        Delete.Sequence("category_id_seq");

        // TODO: add deletes as well

        Delete.Table(CategoryTable).InSchema(Schema);
        Delete.Table(ProductTable).InSchema(Schema);
        Delete.Table(StoreTable).InSchema(Schema);

        Delete.Schema(Schema);
    }
}