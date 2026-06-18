using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MyFactory.Infrastructure.Persistence;

#nullable disable

namespace MyFactory.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20270217120000_RemoveContactOwnerForeignKeys")]
    public partial class RemoveContactOwnerForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CONTACT_LINKS_CUSTOMERS_OwnerId",
                table: "CONTACT_LINKS");

            migrationBuilder.DropForeignKey(
                name: "FK_CONTACT_LINKS_EMPLOYEES_OwnerId",
                table: "CONTACT_LINKS");

            migrationBuilder.DropForeignKey(
                name: "FK_CONTACT_LINKS_USERS_OwnerId",
                table: "CONTACT_LINKS");

            migrationBuilder.DropIndex(
                name: "IX_CONTACT_LINKS_OwnerId",
                table: "CONTACT_LINKS");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CONTACT_LINKS_OwnerId",
                table: "CONTACT_LINKS",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CONTACT_LINKS_CUSTOMERS_OwnerId",
                table: "CONTACT_LINKS",
                column: "OwnerId",
                principalTable: "CUSTOMERS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CONTACT_LINKS_EMPLOYEES_OwnerId",
                table: "CONTACT_LINKS",
                column: "OwnerId",
                principalTable: "EMPLOYEES",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CONTACT_LINKS_USERS_OwnerId",
                table: "CONTACT_LINKS",
                column: "OwnerId",
                principalTable: "USERS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
