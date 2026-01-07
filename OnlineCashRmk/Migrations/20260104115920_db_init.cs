using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCashRmk.Migrations
{
    /// <inheritdoc />
    public partial class db_init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buyers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Birthday = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Phone = table.Column<string>(type: "TEXT", nullable: true),
                    TemporyPercent = table.Column<int>(type: "INTEGER", nullable: false),
                    SpecialPercent = table.Column<int>(type: "INTEGER", nullable: false),
                    DiscountSum = table.Column<decimal>(type: "TEXT", nullable: false),
                    SumBuy = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buyers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CashMoneys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Create = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TypeOperation = table.Column<int>(type: "INTEGER", nullable: false),
                    Sum = table.Column<decimal>(type: "TEXT", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashMoneys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscountParamContainerModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PercentFromSale = table.Column<decimal>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountParamContainerModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocSynches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    TypeDoc = table.Column<int>(type: "INTEGER", nullable: false),
                    DocId = table.Column<int>(type: "INTEGER", nullable: false),
                    Create = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SynchStatus = table.Column<bool>(type: "INTEGER", nullable: false),
                    Synch = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocSynches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Goods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Article = table.Column<string>(type: "TEXT", nullable: true),
                    BarCode = table.Column<string>(type: "TEXT", nullable: true),
                    Unit = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    SpecialType = table.Column<int>(type: "INTEGER", nullable: false),
                    VPackage = table.Column<double>(type: "REAL", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Revaluations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Create = table.Column<DateTime>(type: "Date", nullable: false),
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Revaluations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Start = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Stop = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SumNoElectron = table.Column<decimal>(type: "TEXT", nullable: false),
                    SumElectron = table.Column<decimal>(type: "TEXT", nullable: false),
                    SumSell = table.Column<decimal>(type: "TEXT", nullable: false),
                    SummReturn = table.Column<decimal>(type: "TEXT", nullable: false),
                    SumIncome = table.Column<decimal>(type: "TEXT", nullable: false),
                    SumOutcome = table.Column<decimal>(type: "TEXT", nullable: false),
                    SumCredit = table.Column<decimal>(type: "TEXT", nullable: false),
                    SumAll = table.Column<decimal>(type: "TEXT", nullable: false),
                    ShopId = table.Column<int>(type: "INTEGER", nullable: false),
                    CashierId = table.Column<int>(type: "INTEGER", nullable: false),
                    isSynch = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stocktakings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Create = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CashMoney = table.Column<decimal>(type: "TEXT", nullable: false),
                    isSynch = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocktakings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Inn = table.Column<string>(type: "TEXT", nullable: true),
                    Kpp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Writeofs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    DateCreate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: true),
                    SumAll = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Writeofs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscountParamBirthdayModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DayEnable = table.Column<int>(type: "INTEGER", nullable: false),
                    TextSms = table.Column<string>(type: "TEXT", nullable: true),
                    DiscountSum = table.Column<decimal>(type: "TEXT", nullable: true),
                    DiscountPercent = table.Column<int>(type: "INTEGER", nullable: true),
                    DiscountParamContainerModelId = table.Column<int>(type: "INTEGER", nullable: true),
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsEnable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountParamBirthdayModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountParamBirthdayModel_DiscountParamContainerModel_DiscountParamContainerModelId",
                        column: x => x.DiscountParamContainerModelId,
                        principalTable: "DiscountParamContainerModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DiscountParamHolidaysModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateHoliday = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TextSms = table.Column<string>(type: "TEXT", nullable: true),
                    DiscountSum = table.Column<decimal>(type: "TEXT", nullable: true),
                    DiscountPercent = table.Column<int>(type: "INTEGER", nullable: true),
                    DiscountParamContainerModelId = table.Column<int>(type: "INTEGER", nullable: true),
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsEnable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountParamHolidaysModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountParamHolidaysModel_DiscountParamContainerModel_DiscountParamContainerModelId",
                        column: x => x.DiscountParamContainerModelId,
                        principalTable: "DiscountParamContainerModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DiscountParamNumBuyerModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumBuyer = table.Column<int>(type: "INTEGER", nullable: false),
                    TextSms = table.Column<string>(type: "TEXT", nullable: true),
                    DiscountSum = table.Column<decimal>(type: "TEXT", nullable: true),
                    DiscountPercent = table.Column<int>(type: "INTEGER", nullable: true),
                    DiscountParamContainerModelId = table.Column<int>(type: "INTEGER", nullable: true),
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsEnable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountParamNumBuyerModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountParamNumBuyerModel_DiscountParamContainerModel_DiscountParamContainerModelId",
                        column: x => x.DiscountParamContainerModelId,
                        principalTable: "DiscountParamContainerModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DiscountParamSumBuyModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SumBuyesMore = table.Column<decimal>(type: "TEXT", nullable: true),
                    DiscountPercent = table.Column<int>(type: "INTEGER", nullable: true),
                    DiscountSum = table.Column<decimal>(type: "TEXT", nullable: true),
                    DiscountParamContainerModelId = table.Column<int>(type: "INTEGER", nullable: true),
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsEnable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountParamSumBuyModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountParamSumBuyModel_DiscountParamContainerModel_DiscountParamContainerModelId",
                        column: x => x.DiscountParamContainerModelId,
                        principalTable: "DiscountParamContainerModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DiscountParamSumOneBuyModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SumBuy = table.Column<decimal>(type: "TEXT", nullable: false),
                    DiscountSum = table.Column<decimal>(type: "TEXT", nullable: true),
                    DiscountPercent = table.Column<int>(type: "INTEGER", nullable: true),
                    DiscountParamContainerModelId = table.Column<int>(type: "INTEGER", nullable: true),
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsEnable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountParamSumOneBuyModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountParamSumOneBuyModel_DiscountParamContainerModel_DiscountParamContainerModelId",
                        column: x => x.DiscountParamContainerModelId,
                        principalTable: "DiscountParamContainerModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DiscountParamWeeksModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DayNum = table.Column<int>(type: "INTEGER", nullable: false),
                    DayName = table.Column<string>(type: "TEXT", nullable: true),
                    TimeWith = table.Column<string>(type: "TEXT", nullable: true),
                    TimeBy = table.Column<string>(type: "TEXT", nullable: true),
                    DiscountPercent = table.Column<int>(type: "INTEGER", nullable: true),
                    DiscountParamContainerModelId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountParamWeeksModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountParamWeeksModel_DiscountParamContainerModel_DiscountParamContainerModelId",
                        column: x => x.DiscountParamContainerModelId,
                        principalTable: "DiscountParamContainerModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DiscountSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Discounts = table.Column<string>(type: "TEXT", nullable: true),
                    DiscountModelId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountSettings_DiscountParamContainerModel_DiscountModelId",
                        column: x => x.DiscountModelId,
                        principalTable: "DiscountParamContainerModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BarCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: true),
                    GoodId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BarCodes_Goods_GoodId",
                        column: x => x.GoodId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NewGoodsFromCash",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GoodId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewGoodsFromCash", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewGoodsFromCash_Goods_GoodId",
                        column: x => x.GoodId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RevaluationGoods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RevaluationId = table.Column<int>(type: "INTEGER", nullable: false),
                    GoodId = table.Column<int>(type: "INTEGER", nullable: false),
                    Count = table.Column<decimal>(type: "TEXT", nullable: true),
                    PriceOld = table.Column<decimal>(type: "TEXT", nullable: false),
                    PriceNew = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevaluationGoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RevaluationGoods_Goods_GoodId",
                        column: x => x.GoodId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RevaluationGoods_Revaluations_RevaluationId",
                        column: x => x.RevaluationId,
                        principalTable: "Revaluations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckSells",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BuyerId = table.Column<int>(type: "INTEGER", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TypeSell = table.Column<int>(type: "INTEGER", nullable: false),
                    IsElectron = table.Column<bool>(type: "INTEGER", nullable: false),
                    Sum = table.Column<decimal>(type: "TEXT", nullable: false),
                    SumDiscont = table.Column<decimal>(type: "TEXT", nullable: false),
                    SumAll = table.Column<decimal>(type: "TEXT", nullable: false),
                    ShiftId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckSells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckSells_Buyers_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Buyers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CheckSells_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Credits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Creditor = table.Column<string>(type: "TEXT", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Sum = table.Column<decimal>(type: "TEXT", nullable: false),
                    SumDiscont = table.Column<decimal>(type: "TEXT", nullable: false),
                    SumAll = table.Column<decimal>(type: "TEXT", nullable: false),
                    SumCredit = table.Column<decimal>(type: "TEXT", nullable: false),
                    ShiftId = table.Column<int>(type: "INTEGER", nullable: true),
                    isSynch = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Credits_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StocktakingGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StocktakingId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StocktakingGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StocktakingGroups_Stocktakings_StocktakingId",
                        column: x => x.StocktakingId,
                        principalTable: "Stocktakings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Arrivals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Num = table.Column<string>(type: "TEXT", nullable: true),
                    DateArrival = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SupplierId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arrivals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Arrivals_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WriteofGoods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WriteofId = table.Column<int>(type: "INTEGER", nullable: false),
                    GoodId = table.Column<int>(type: "INTEGER", nullable: false),
                    Count = table.Column<double>(type: "REAL", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WriteofGoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WriteofGoods_Goods_GoodId",
                        column: x => x.GoodId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WriteofGoods_Writeofs_WriteofId",
                        column: x => x.WriteofId,
                        principalTable: "Writeofs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckGoods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Count = table.Column<double>(type: "REAL", nullable: false),
                    Cost = table.Column<decimal>(type: "TEXT", nullable: false),
                    GoodId = table.Column<int>(type: "INTEGER", nullable: false),
                    CheckSellId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckGoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckGoods_CheckSells_CheckSellId",
                        column: x => x.CheckSellId,
                        principalTable: "CheckSells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckGoods_Goods_GoodId",
                        column: x => x.GoodId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CheckSellId = table.Column<int>(type: "INTEGER", nullable: false),
                    TypePayment = table.Column<int>(type: "INTEGER", nullable: false),
                    Income = table.Column<decimal>(type: "TEXT", nullable: false),
                    Sum = table.Column<decimal>(type: "TEXT", nullable: false),
                    Retturn = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckPayments_CheckSells_CheckSellId",
                        column: x => x.CheckSellId,
                        principalTable: "CheckSells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreditGoods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Count = table.Column<double>(type: "REAL", nullable: false),
                    Cost = table.Column<decimal>(type: "TEXT", nullable: false),
                    GoodId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreditId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditGoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditGoods_Credits_CreditId",
                        column: x => x.CreditId,
                        principalTable: "Credits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreditGoods_Goods_GoodId",
                        column: x => x.GoodId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreditPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DatePayment = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Sum = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreditId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditPayments_Credits_CreditId",
                        column: x => x.CreditId,
                        principalTable: "Credits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StocktakingGoods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StocktakingGroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    GoodId = table.Column<int>(type: "INTEGER", nullable: false),
                    CountFact = table.Column<decimal>(type: "TEXT", nullable: true),
                    CountDocMove = table.Column<decimal>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StocktakingGoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StocktakingGoods_Goods_GoodId",
                        column: x => x.GoodId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StocktakingGoods_StocktakingGroups_StocktakingGroupId",
                        column: x => x.StocktakingGroupId,
                        principalTable: "StocktakingGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArrivalGoods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ArrivalId = table.Column<int>(type: "INTEGER", nullable: false),
                    GoodId = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Count = table.Column<decimal>(type: "TEXT", nullable: false),
                    Nds = table.Column<string>(type: "TEXT", nullable: true),
                    ExpiresDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArrivalGoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArrivalGoods_Arrivals_ArrivalId",
                        column: x => x.ArrivalId,
                        principalTable: "Arrivals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArrivalGoods_Goods_GoodId",
                        column: x => x.GoodId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArrivalGoods_ArrivalId",
                table: "ArrivalGoods",
                column: "ArrivalId");

            migrationBuilder.CreateIndex(
                name: "IX_ArrivalGoods_GoodId",
                table: "ArrivalGoods",
                column: "GoodId");

            migrationBuilder.CreateIndex(
                name: "IX_Arrivals_SupplierId",
                table: "Arrivals",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_BarCodes_GoodId",
                table: "BarCodes",
                column: "GoodId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckGoods_CheckSellId",
                table: "CheckGoods",
                column: "CheckSellId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckGoods_GoodId",
                table: "CheckGoods",
                column: "GoodId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckPayments_CheckSellId",
                table: "CheckPayments",
                column: "CheckSellId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckSells_BuyerId",
                table: "CheckSells",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckSells_ShiftId",
                table: "CheckSells",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditGoods_CreditId",
                table: "CreditGoods",
                column: "CreditId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditGoods_GoodId",
                table: "CreditGoods",
                column: "GoodId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditPayments_CreditId",
                table: "CreditPayments",
                column: "CreditId");

            migrationBuilder.CreateIndex(
                name: "IX_Credits_ShiftId",
                table: "Credits",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountParamBirthdayModel_DiscountParamContainerModelId",
                table: "DiscountParamBirthdayModel",
                column: "DiscountParamContainerModelId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountParamHolidaysModel_DiscountParamContainerModelId",
                table: "DiscountParamHolidaysModel",
                column: "DiscountParamContainerModelId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountParamNumBuyerModel_DiscountParamContainerModelId",
                table: "DiscountParamNumBuyerModel",
                column: "DiscountParamContainerModelId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountParamSumBuyModel_DiscountParamContainerModelId",
                table: "DiscountParamSumBuyModel",
                column: "DiscountParamContainerModelId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountParamSumOneBuyModel_DiscountParamContainerModelId",
                table: "DiscountParamSumOneBuyModel",
                column: "DiscountParamContainerModelId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountParamWeeksModel_DiscountParamContainerModelId",
                table: "DiscountParamWeeksModel",
                column: "DiscountParamContainerModelId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountSettings_DiscountModelId",
                table: "DiscountSettings",
                column: "DiscountModelId");

            migrationBuilder.CreateIndex(
                name: "IX_NewGoodsFromCash_GoodId",
                table: "NewGoodsFromCash",
                column: "GoodId");

            migrationBuilder.CreateIndex(
                name: "IX_RevaluationGoods_GoodId",
                table: "RevaluationGoods",
                column: "GoodId");

            migrationBuilder.CreateIndex(
                name: "IX_RevaluationGoods_RevaluationId",
                table: "RevaluationGoods",
                column: "RevaluationId");

            migrationBuilder.CreateIndex(
                name: "IX_StocktakingGoods_GoodId",
                table: "StocktakingGoods",
                column: "GoodId");

            migrationBuilder.CreateIndex(
                name: "IX_StocktakingGoods_StocktakingGroupId",
                table: "StocktakingGoods",
                column: "StocktakingGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_StocktakingGroups_StocktakingId",
                table: "StocktakingGroups",
                column: "StocktakingId");

            migrationBuilder.CreateIndex(
                name: "IX_WriteofGoods_GoodId",
                table: "WriteofGoods",
                column: "GoodId");

            migrationBuilder.CreateIndex(
                name: "IX_WriteofGoods_WriteofId",
                table: "WriteofGoods",
                column: "WriteofId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArrivalGoods");

            migrationBuilder.DropTable(
                name: "BarCodes");

            migrationBuilder.DropTable(
                name: "CashMoneys");

            migrationBuilder.DropTable(
                name: "CheckGoods");

            migrationBuilder.DropTable(
                name: "CheckPayments");

            migrationBuilder.DropTable(
                name: "CreditGoods");

            migrationBuilder.DropTable(
                name: "CreditPayments");

            migrationBuilder.DropTable(
                name: "DiscountParamBirthdayModel");

            migrationBuilder.DropTable(
                name: "DiscountParamHolidaysModel");

            migrationBuilder.DropTable(
                name: "DiscountParamNumBuyerModel");

            migrationBuilder.DropTable(
                name: "DiscountParamSumBuyModel");

            migrationBuilder.DropTable(
                name: "DiscountParamSumOneBuyModel");

            migrationBuilder.DropTable(
                name: "DiscountParamWeeksModel");

            migrationBuilder.DropTable(
                name: "DiscountSettings");

            migrationBuilder.DropTable(
                name: "DocSynches");

            migrationBuilder.DropTable(
                name: "NewGoodsFromCash");

            migrationBuilder.DropTable(
                name: "RevaluationGoods");

            migrationBuilder.DropTable(
                name: "StocktakingGoods");

            migrationBuilder.DropTable(
                name: "WriteofGoods");

            migrationBuilder.DropTable(
                name: "Arrivals");

            migrationBuilder.DropTable(
                name: "CheckSells");

            migrationBuilder.DropTable(
                name: "Credits");

            migrationBuilder.DropTable(
                name: "DiscountParamContainerModel");

            migrationBuilder.DropTable(
                name: "Revaluations");

            migrationBuilder.DropTable(
                name: "StocktakingGroups");

            migrationBuilder.DropTable(
                name: "Goods");

            migrationBuilder.DropTable(
                name: "Writeofs");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Buyers");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "Stocktakings");
        }
    }
}
