using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MVC.Migrations
{
    public partial class OrderHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PartialOrder",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<int>(type: "int4", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp", nullable: false),
                    OrderId = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<float>(type: "float4", nullable: false),
                    ProductId = table.Column<int>(type: "int4", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartialOrder", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartialOrder");
        }
    }
}
