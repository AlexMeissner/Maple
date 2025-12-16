using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maple.Migrations
{
    /// <inheritdoc />
    public partial class GuidAndMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "LogEntries",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "LogEntries",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                table: "LogEntries");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "LogEntries");
        }
    }
}
