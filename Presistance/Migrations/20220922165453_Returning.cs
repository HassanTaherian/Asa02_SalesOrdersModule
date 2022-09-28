﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class Returning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReturn",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "ReturnDateTime",
                table: "InvoiceItems");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnDateTime",
                table: "Invoices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReturn",
                table: "InvoiceItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturnDateTime",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "IsReturn",
                table: "InvoiceItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsReturn",
                table: "Invoices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnDateTime",
                table: "InvoiceItems",
                type: "datetime2",
                nullable: true);
        }
    }
}