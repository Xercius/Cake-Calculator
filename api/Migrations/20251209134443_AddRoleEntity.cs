using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CakeCalculatorApi.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CakeShapes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CakeShapes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CakeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CakeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fillings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fillings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Frostings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frostings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CostPerUnit = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    HourlyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Size = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    BaseIngredients = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CakeSizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ShapeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Dimensions = table.Column<string>(type: "TEXT", nullable: true),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CakeSizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CakeSizes_CakeShapes_ShapeId",
                        column: x => x.ShapeId,
                        principalTable: "CakeShapes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cakes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    TemplateId = table.Column<int>(type: "INTEGER", nullable: false),
                    ExtraIngredients = table.Column<string>(type: "TEXT", nullable: true),
                    Labor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtherCosts = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cakes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cakes_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "CakeShapes",
                columns: new[] { "Id", "ImagePath", "IsActive", "Name", "SortOrder" },
                values: new object[,]
                {
                    { 1, null, true, "Round", 1 },
                    { 2, null, true, "Square", 2 },
                    { 3, null, true, "Rectangle", 3 }
                });

            migrationBuilder.InsertData(
                table: "CakeTypes",
                columns: new[] { "Id", "ImagePath", "IsActive", "Name", "SortOrder" },
                values: new object[,]
                {
                    { 1, null, true, "Vanilla", 1 },
                    { 2, null, true, "Chocolate", 2 },
                    { 3, null, true, "Red Velvet", 3 },
                    { 4, null, true, "Lemon", 4 },
                    { 5, null, true, "Carrot", 5 }
                });

            migrationBuilder.InsertData(
                table: "Fillings",
                columns: new[] { "Id", "ImagePath", "IsActive", "Name", "SortOrder" },
                values: new object[,]
                {
                    { 1, null, true, "Buttercream", 1 },
                    { 2, null, true, "Chocolate Ganache", 2 },
                    { 3, null, true, "Raspberry", 3 },
                    { 4, null, true, "Lemon Curd", 4 },
                    { 5, null, true, "Cream Cheese", 5 }
                });

            migrationBuilder.InsertData(
                table: "Frostings",
                columns: new[] { "Id", "ImagePath", "IsActive", "Name", "SortOrder" },
                values: new object[,]
                {
                    { 1, null, true, "Buttercream", 1 },
                    { 2, null, true, "Cream Cheese", 2 },
                    { 3, null, true, "Chocolate", 3 },
                    { 4, null, true, "Fondant", 4 },
                    { 5, null, true, "Whipped Cream", 5 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "HourlyRate", "Name" },
                values: new object[,]
                {
                    { 1, 25.00m, "Baker" },
                    { 2, 30.00m, "Decorator" },
                    { 3, 20.00m, "Delivery" }
                });

            migrationBuilder.InsertData(
                table: "CakeSizes",
                columns: new[] { "Id", "Dimensions", "ImagePath", "IsActive", "Name", "ShapeId", "SortOrder" },
                values: new object[,]
                {
                    { 1, "{\"roundDiameterIn\":6}", null, true, "6\" Round", 1, 1 },
                    { 2, "{\"roundDiameterIn\":8}", null, true, "8\" Round", 1, 2 },
                    { 3, "{\"roundDiameterIn\":10}", null, true, "10\" Round", 1, 3 },
                    { 4, "{\"lengthIn\":8,\"widthIn\":8}", null, true, "8\" Square", 2, 4 },
                    { 5, "{\"lengthIn\":13,\"widthIn\":9}", null, true, "9x13\" Sheet", 3, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cakes_TemplateId",
                table: "Cakes",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CakeSizes_ShapeId",
                table: "CakeSizes",
                column: "ShapeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cakes");

            migrationBuilder.DropTable(
                name: "CakeSizes");

            migrationBuilder.DropTable(
                name: "CakeTypes");

            migrationBuilder.DropTable(
                name: "Fillings");

            migrationBuilder.DropTable(
                name: "Frostings");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "CakeShapes");
        }
    }
}
