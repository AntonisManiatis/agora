using FluentMigrator;

namespace Agora.Catalogs.Infrastructure.Data.Migrations;

// TODO: name
[Migration(120319238912)]
public sealed class Initial : Migration
{
    public override void Down()
    {
        // ? does this cascade to tables?
        Delete.Schema("catalogs");
        Delete.Table("product");
    }

    public override void Up()
    {
        Create.Schema("catalogs");

        Create.Table("product")
            .InSchema("catalogs")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("title").AsString().NotNullable()
            .WithColumn("description").AsString().NotNullable();
    }
}