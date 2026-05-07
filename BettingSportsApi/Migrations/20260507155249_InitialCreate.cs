using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BettingSportsApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MatchId = table.Column<int>(type: "INTEGER", nullable: false),
                    BetType = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Odds = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsWin = table.Column<bool>(type: "INTEGER", nullable: false),
                    Payout = table.Column<decimal>(type: "TEXT", nullable: false),
                    BetDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HomeTeam = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    AwayTeam = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    League = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    HomeOdds = table.Column<double>(type: "REAL", nullable: false),
                    AwayOdds = table.Column<double>(type: "REAL", nullable: false),
                    DrawOdds = table.Column<double>(type: "REAL", nullable: false),
                    HomeScore = table.Column<int>(type: "INTEGER", nullable: false),
                    AwayScore = table.Column<int>(type: "INTEGER", nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    MatchDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Balance = table.Column<decimal>(type: "TEXT", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStates", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Matches",
                columns: new[] { "Id", "AwayOdds", "AwayScore", "AwayTeam", "DrawOdds", "HomeOdds", "HomeScore", "HomeTeam", "IsCompleted", "League", "MatchDate" },
                values: new object[,]
                {
                    { 1, 3.2000000000000002, 0, "Бавария", 3.5, 2.1000000000000001, 0, "ПСЖ", false, "Champions League", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 4.0, 0, "Атлетико Мадрид", 3.6000000000000001, 1.8, 0, "Арсенал", false, "Champions League", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, 2.8999999999999999, 0, "Арсенал", 3.3999999999999999, 2.2999999999999998, 0, "Спортинг", false, "Champions League", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 3.1000000000000001, 0, "Ливерпуль", 3.2999999999999998, 2.2000000000000002, 0, "Галатасарай", false, "Champions League", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 3.7999999999999998, 0, "Осасуна", 3.3999999999999999, 1.95, 0, "Леванте", false, "La Liga", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, 4.5, 0, "Сельта", 3.7000000000000002, 1.75, 0, "Атлетико", false, "La Liga", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, 2.7999999999999998, 0, "Бетис", 3.2000000000000002, 2.3999999999999999, 0, "Реал Сосьедад", false, "La Liga", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, 2.6000000000000001, 0, "Вильярреал", 3.1000000000000001, 2.6000000000000001, 0, "Мальорка", false, "La Liga", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "UserStates",
                columns: new[] { "Id", "Balance", "LastUpdated" },
                values: new object[] { 1, 10000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bets");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "UserStates");
        }
    }
}
