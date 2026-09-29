using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaterialBalanceAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Periods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Streams",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceNodeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestinationNodeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinBound = table.Column<decimal>(type: "decimal(23,18)", nullable: false),
                    MaxBound = table.Column<decimal>(type: "decimal(23,18)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Streams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PeriodId = table.Column<int>(type: "int", nullable: false),
                    StreamId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Value = table.Column<decimal>(type: "decimal(23,18)", nullable: false),
                    Tolerance = table.Column<decimal>(type: "decimal(23,18)", nullable: false),
                    IsMeasured = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Measurements_Periods_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "Periods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Measurements_Streams_StreamId",
                        column: x => x.StreamId,
                        principalTable: "Streams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_PeriodId",
                table: "Measurements",
                column: "PeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_StreamId",
                table: "Measurements",
                column: "StreamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Measurements");

            migrationBuilder.DropTable(
                name: "Periods");

            migrationBuilder.DropTable(
                name: "Streams");
        }
    }
}
