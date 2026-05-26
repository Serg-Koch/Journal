using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Journal.Migrations
{
    /// <inheritdoc />
    public partial class NoteModelUpdate0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDraft",
                table: "Note",
                newName: "IsPublished");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "Note",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Note");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "Note",
                newName: "IsDraft");
        }
    }
}
