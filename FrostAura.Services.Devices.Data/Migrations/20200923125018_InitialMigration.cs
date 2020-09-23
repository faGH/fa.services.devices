using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FrostAura.Services.Devices.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attributes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false, defaultValue: false),
                    TimeStamp = table.Column<DateTime>(nullable: false, defaultValue: DateTime.UtcNow),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attributes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false, defaultValue: false),
                    TimeStamp = table.Column<DateTime>(nullable: false, defaultValue: DateTime.UtcNow),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false, defaultValue: false),
                    TimeStamp = table.Column<DateTime>(nullable: false, defaultValue: DateTime.UtcNow),
                    Value = table.Column<string>(nullable: true),
                    DeviceId = table.Column<int>(nullable: false),
                    AttributeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceAttributes_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_DeviceAttributes_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceAttributes_AttributeId",
                table: "DeviceAttributes",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceAttributes_DeviceId",
                table: "DeviceAttributes",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_Name",
                table: "Devices",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceAttributes");

            migrationBuilder.DropTable(
                name: "Attributes");

            migrationBuilder.DropTable(
                name: "Devices");
        }
    }
}
