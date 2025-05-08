using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThestralService.Infrastructure.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    ExternalOperatorId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Files = table.Column<string>(type: "jsonb", nullable: false),
                    IsUserAuthUserDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    IsFileManagerUserDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    IsUserManagementUserDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_Id",
                table: "Transfers",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_UserId",
                table: "Transfers",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transfers");
        }
    }
}
