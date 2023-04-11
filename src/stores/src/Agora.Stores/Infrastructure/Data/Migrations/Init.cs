using FluentMigrator;

namespace Agora.Stores.Infrastructure.Data.Migrations;

// ! Note: I intentionally haven't used the schema defined in Sql.cs because
// ! I feel like once a migration gets applied it shouldn't change. But the Sql.cs is always the
// ! currently valid DB schema

[Migration(1)]
public sealed class Init : Migration
{
    private const string TableName = "store";

    public override void Up()
    {
        Create.Schema("stores");

        Create.Table(TableName)
            .InSchema("stores")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("owner_id").AsGuid().NotNullable()
            .WithColumn("status").AsString().NotNullable().WithDefaultValue("Pending")
            .WithColumn("tin").AsString(9).Nullable();

        Create.Table("store_category")
            .InSchema("stores")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("store_id").AsGuid()
            .WithColumn("name").AsString(255).Unique().NotNullable();

        Create.ForeignKey()
            .FromTable("store_category").InSchema("stores").ForeignColumn("store_id")
            .ToTable("store").InSchema("stores").PrimaryColumn("id");

        // Products
        Create.Table("product")
            .InSchema("stores")
            .WithColumn("id").AsInt32().Identity() // ? Unsure about this.
            .WithColumn("product_id").AsGuid().NotNullable()
            .WithColumn("store_id").AsGuid().NotNullable()
            .WithColumn("sku").AsString(32)
            .WithColumn("quantity").AsInt32().NotNullable().WithDefaultValue(0);
    }

    public override void Down()
    {
        // ? Not sure if this cascades deletes.
        Delete.Schema("stores");

        Delete.Table(TableName);
        Delete.Table("store_category");
        Delete.Table("product");
    }
}