using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoadGuard.Backend.Migrations
{
  /// <inheritdoc />
  public partial class InitialCreate : Migration
  {
    /// <inheritdoc />
    protected override void Up( MigrationBuilder migrationBuilder )
    {
      migrationBuilder.CreateTable(
          name: "reports",
          columns: table => new
          {
            id = table.Column<Guid>( type: "uuid", nullable: false ),
            latitude = table.Column<double>( type: "double precision", nullable: false ),
            longitude = table.Column<double>( type: "double precision", nullable: false ),
            radius_meters = table.Column<double>( type: "double precision", nullable: false ),
            comment = table.Column<string>( type: "text", nullable: true ),
            created_at = table.Column<DateTime>( type: "timestamp with time zone", nullable: false ),
            expires_at = table.Column<DateTime>( type: "timestamp with time zone", nullable: false )
          },
          constraints: table =>
          {
            table.PrimaryKey( "pk_reports", x => x.id );
          } );

      migrationBuilder.CreateTable(
          name: "users",
          columns: table => new
          {
            id = table.Column<Guid>( type: "uuid", nullable: false ),
            username = table.Column<string>( type: "text", nullable: false ),
            password_hash = table.Column<string>( type: "text", nullable: false ),
            car_make = table.Column<string>( type: "text", nullable: true ),
            car_color = table.Column<string>( type: "text", nullable: true ),
            is_anonymous = table.Column<bool>( type: "boolean", nullable: false ),
            reputation_score = table.Column<double>( type: "double precision", nullable: false ),
            created_at = table.Column<DateTime>( type: "timestamp with time zone", nullable: false )
          },
          constraints: table =>
          {
            table.PrimaryKey( "pk_users", x => x.id );
          } );

      migrationBuilder.CreateTable(
          name: "driver_ratings",
          columns: table => new
          {
            id = table.Column<Guid>( type: "uuid", nullable: false ),
            from_user_id = table.Column<Guid>( type: "uuid", nullable: false ),
            to_user_id = table.Column<Guid>( type: "uuid", nullable: false ),
            value = table.Column<int>( type: "integer", nullable: false ),
            created_at = table.Column<DateTime>( type: "timestamp with time zone", nullable: false )
          },
          constraints: table =>
          {
            table.PrimaryKey( "pk_driver_ratings", x => x.id );
            table.ForeignKey(
                      name: "fk_driver_ratings_users_from_user_id",
                      column: x => x.from_user_id,
                      principalTable: "users",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Restrict );
            table.ForeignKey(
                      name: "fk_driver_ratings_users_to_user_id",
                      column: x => x.to_user_id,
                      principalTable: "users",
                      principalColumn: "id",
                      onDelete: ReferentialAction.Restrict );
          } );

      migrationBuilder.CreateIndex(
          name: "ix_driver_ratings_from_user_id",
          table: "driver_ratings",
          column: "from_user_id" );

      migrationBuilder.CreateIndex(
          name: "ix_driver_ratings_to_user_id",
          table: "driver_ratings",
          column: "to_user_id" );

      migrationBuilder.CreateIndex(
          name: "ix_users_username",
          table: "users",
          column: "username",
          unique: true );
    }

    /// <inheritdoc />
    protected override void Down( MigrationBuilder migrationBuilder )
    {
      migrationBuilder.DropTable(
          name: "driver_ratings" );

      migrationBuilder.DropTable(
          name: "reports" );

      migrationBuilder.DropTable(
          name: "users" );
    }
  }
}
