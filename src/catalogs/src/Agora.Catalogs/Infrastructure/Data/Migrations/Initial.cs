using FluentMigrator;

namespace Agora.Catalogs.Infrastructure.Data.Migrations;

// TODO: name
[Migration(120319238912)]
public sealed class Initial : Migration
{
    public override void Up()
    {
        Create.Schema("catalogs");

        Create.Table("product")
            .InSchema("catalogs")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("title").AsString().NotNullable()
            .WithColumn("description").AsString().NotNullable();

        Create.Table("store")
            .InSchema("catalogs")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("name").AsString().Unique().NotNullable() // TODO: Add max value also not nullable required?
            .WithColumn("lang").AsString(3).NotNullable(); // I think this is needed here, check what identifier we need. 3 letter iso? 2?
    }

    public override void Down()
    {
        // ? does this cascade to tables?
        Delete.Schema("catalogs");
        Delete.Table("product");
        Delete.Table("store");
    }
}