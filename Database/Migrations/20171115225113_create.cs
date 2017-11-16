using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Database.Migrations
{
    public partial class create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    description = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<float>(type: "float4", nullable: false),
                    type = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    address = table.Column<string>(type: "text", nullable: true),
                    admin = table.Column<bool>(type: "bool", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true),
                    gender = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    phonenumber = table.Column<string>(type: "text", nullable: true),
                    salt = table.Column<string>(type: "text", nullable: true),
                    surname = table.Column<string>(type: "text", nullable: true),
                    username = table.Column<string>(type: "text", nullable: true),
                    verified = table.Column<bool>(type: "bool", nullable: false),
                    zipcode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "specifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    productid = table.Column<int>(type: "int4", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_specifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_specifications_products_productid",
                        column: x => x.productid,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_specifications_productid",
                table: "specifications",
                column: "productid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "specifications");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "products");
        }
    }
}
