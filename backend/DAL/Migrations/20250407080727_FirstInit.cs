using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class FirstInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Icon = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Emails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SenderEmail = table.Column<string>(type: "text", nullable: true),
                    ReceiverEmail = table.Column<string>(type: "text", nullable: true),
                    Subject = table.Column<string>(type: "text", nullable: true),
                    Body = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ShortDescription = table.Column<string>(type: "text", nullable: true),
                    NormalizedInfo = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Avatar = table.Column<string>(type: "text", nullable: true),
                    Cover = table.Column<string>(type: "text", nullable: true),
                    DateOfBirth = table.Column<long>(type: "bigint", nullable: false),
                    Images = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<decimal>(type: "numeric(8,6)", nullable: false),
                    Longitude = table.Column<decimal>(type: "numeric(9,6)", nullable: false),
                    Characteristics = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsEmailVerified = table.Column<bool>(type: "boolean", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiryTime = table.Column<long>(type: "bigint", nullable: false),
                    CityId = table.Column<int>(type: "integer", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccountCategory",
                columns: table => new
                {
                    AccountsId = table.Column<int>(type: "integer", nullable: false),
                    CategoriesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountCategory", x => new { x.AccountsId, x.CategoriesId });
                    table.ForeignKey(
                        name: "FK_AccountCategory_Accounts_AccountsId",
                        column: x => x.AccountsId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountCategory_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Businesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ViewCount = table.Column<int>(type: "integer", nullable: false),
                    Address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    OperatingHours = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Businesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Businesses_Accounts_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SenderId = table.Column<int>(type: "integer", nullable: false),
                    TargetId = table.Column<int>(type: "integer", nullable: false),
                    TargetType = table.Column<int>(type: "integer", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Response = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Accounts_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Friendships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RequesterId = table.Column<int>(type: "integer", nullable: false),
                    RequesteeId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friendships_Accounts_RequesteeId",
                        column: x => x.RequesteeId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Friendships_Accounts_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SenderId = table.Column<int>(type: "integer", nullable: false),
                    ReceiverId = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Accounts_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Accounts_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ShortDescription = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    NormalizedInfo = table.Column<string>(type: "text", nullable: true),
                    Images = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ViewCount = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    CityId = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedDate = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Places_Accounts_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Places_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CategoryPlace",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "integer", nullable: false),
                    PlacesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryPlace", x => new { x.CategoriesId, x.PlacesId });
                    table.ForeignKey(
                        name: "FK_CategoryPlace_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryPlace_Places_PlacesId",
                        column: x => x.PlacesId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Avatar", "Characteristics", "CityId", "Cover", "CreatedBy", "CreatedDate", "DateOfBirth", "Description", "Email", "FirstName", "Images", "IsEmailVerified", "LastName", "Latitude", "Longitude", "NormalizedInfo", "Password", "RefreshToken", "RefreshTokenExpiryTime", "Role", "ShortDescription", "Status", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, null, null, null, 0, 1744013246250L, 0L, null, "minhbee203@gmail.com", "Minh", null, true, "Bùi", 0m, 0m, null, "KNhl/iqbl/G9JNLIvQcrhA==;6CwOXsHyGt+15dKQ9PGBmB+D8l47mrmj8yFM9DVEsY4=", null, 0L, 1, null, 1, 0, 1744013246250L },
                    { 2, "avt1.png", null, null, null, 0, 1744013246283L, 0L, null, "user1@gmail.com", "1", null, true, "User", 16.09932790307512m, 108.24420359528237m, null, "K8hC7fX4jU3XzKVfxpMq7Q==;cDzyeNAV/IlcUnQlWxlUz8oNJu1fJIoJlHyFS9ddG4s=", null, 0L, 2, null, 1, 0, 1744013246283L },
                    { 3, "avt2.png", null, null, null, 0, 1744013246317L, 0L, null, "user2@gmail.com", "2", null, true, "User", 16.086021861878294m, 108.21735108528173m, null, "Hex/waynffzIyB5N9SpWmQ==;m+DHy1x9Q+vXrkquC7eS5ew3Adco+GtHPrYCC27lg5c=", null, 0L, 2, null, 1, 0, 1744013246317L }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Icon", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 0, 1744013246354L, "fa-futbol", "Thể thao", 0, 1744013246354L },
                    { 2, 0, 1744013246354L, "fa-book", "Học tập", 0, 1744013246354L },
                    { 3, 0, 1744013246354L, "fa-camera", "Quay & Chụp", 0, 1744013246354L },
                    { 4, 0, 1744013246354L, "fa-spa", "Làm đẹp", 0, 1744013246354L },
                    { 5, 0, 1744013246354L, "fa-plane", "Du lịch", 0, 1744013246354L },
                    { 6, 0, 1744013246354L, "fa-paw", "Thú cưng", 0, 1744013246354L },
                    { 7, 0, 1744013246354L, "fa-utensils", "Ăn uống", 0, 1744013246354L },
                    { 8, 0, 1744013246354L, "fa-game-console-handheld", "Chơi game", 0, 1744013246354L },
                    { 9, 0, 1744013246354L, "fa-music-note", "Ca hát", 0, 1744013246354L },
                    { 10, 0, 1744013246354L, "fa-microchip", "Công nghệ", 0, 1744013246354L },
                    { 11, 0, 1744013246354L, "fa-briefcase-blank", "Kinh doanh", 0, 1744013246354L }
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 11, 0, 1744013246354L, "Cao Bằng", 0, 1744013246354L },
                    { 12, 0, 1744013246354L, "Lạng Sơn", 0, 1744013246354L },
                    { 14, 0, 1744013246354L, "Quảng Ninh", 0, 1744013246354L },
                    { 15, 0, 1744013246354L, "Hải Phòng", 0, 1744013246354L },
                    { 17, 0, 1744013246354L, "Thái Bình", 0, 1744013246354L },
                    { 18, 0, 1744013246354L, "Nam Định", 0, 1744013246354L },
                    { 19, 0, 1744013246354L, "Phú Thọ", 0, 1744013246354L },
                    { 20, 0, 1744013246354L, "Thái Nguyên", 0, 1744013246354L },
                    { 21, 0, 1744013246354L, "Yên Bái", 0, 1744013246354L },
                    { 22, 0, 1744013246354L, "Tuyên Quang", 0, 1744013246354L },
                    { 23, 0, 1744013246354L, "Hà Giang", 0, 1744013246354L },
                    { 24, 0, 1744013246354L, "Lào Cai", 0, 1744013246354L },
                    { 25, 0, 1744013246354L, "Lai Châu", 0, 1744013246354L },
                    { 26, 0, 1744013246354L, "Sơn La", 0, 1744013246354L },
                    { 27, 0, 1744013246354L, "Điện Biên", 0, 1744013246354L },
                    { 28, 0, 1744013246354L, "Hoà Bình", 0, 1744013246354L },
                    { 29, 0, 1744013246354L, "Hà Nội", 0, 1744013246354L },
                    { 34, 0, 1744013246354L, "Hải Dương", 0, 1744013246354L },
                    { 35, 0, 1744013246354L, "Ninh Bình", 0, 1744013246354L },
                    { 36, 0, 1744013246354L, "Thanh Hóa", 0, 1744013246354L },
                    { 37, 0, 1744013246354L, "Nghệ An", 0, 1744013246354L },
                    { 38, 0, 1744013246354L, "Hà Tĩnh", 0, 1744013246354L },
                    { 39, 0, 1744013246354L, "Đồng Nai", 0, 1744013246354L },
                    { 41, 0, 1744013246354L, "Hồ Chí Minh", 0, 1744013246354L },
                    { 43, 0, 1744013246354L, "Đà Nẵng", 0, 1744013246354L },
                    { 47, 0, 1744013246354L, "Đắk Lắk", 0, 1744013246354L },
                    { 48, 0, 1744013246354L, "Đắk Nông", 0, 1744013246354L },
                    { 49, 0, 1744013246354L, "Lâm Đồng", 0, 1744013246354L },
                    { 56, 0, 1744013246354L, "Đồng Tháp", 0, 1744013246354L },
                    { 61, 0, 1744013246354L, "Bình Dương", 0, 1744013246354L },
                    { 62, 0, 1744013246354L, "Long An", 0, 1744013246354L },
                    { 63, 0, 1744013246354L, "Tiền Giang", 0, 1744013246354L },
                    { 64, 0, 1744013246354L, "Vĩnh Long", 0, 1744013246354L },
                    { 65, 0, 1744013246354L, "Cần Thơ", 0, 1744013246354L },
                    { 67, 0, 1744013246354L, "An Giang", 0, 1744013246354L },
                    { 68, 0, 1744013246354L, "Kiên Giang", 0, 1744013246354L },
                    { 69, 0, 1744013246354L, "Cà Mau", 0, 1744013246354L },
                    { 70, 0, 1744013246354L, "Tây Ninh", 0, 1744013246354L },
                    { 71, 0, 1744013246354L, "Bến Tre", 0, 1744013246354L },
                    { 72, 0, 1744013246354L, "Bà Rịa - Vũng Tàu", 0, 1744013246354L },
                    { 73, 0, 1744013246354L, "Quảng Bình", 0, 1744013246354L },
                    { 74, 0, 1744013246354L, "Quảng Trị", 0, 1744013246354L },
                    { 75, 0, 1744013246354L, "Thừa Thiên Huế", 0, 1744013246354L },
                    { 76, 0, 1744013246354L, "Quảng Ngãi", 0, 1744013246354L },
                    { 77, 0, 1744013246354L, "Bình Định", 0, 1744013246354L },
                    { 78, 0, 1744013246354L, "Phú Yên", 0, 1744013246354L },
                    { 79, 0, 1744013246354L, "Khánh Hòa", 0, 1744013246354L },
                    { 81, 0, 1744013246354L, "Gia Lai", 0, 1744013246354L },
                    { 82, 0, 1744013246354L, "Kon Tum", 0, 1744013246354L },
                    { 83, 0, 1744013246354L, "Sóc Trăng", 0, 1744013246354L },
                    { 84, 0, 1744013246354L, "Trà Vinh", 0, 1744013246354L },
                    { 85, 0, 1744013246354L, "Ninh Thuận", 0, 1744013246354L },
                    { 86, 0, 1744013246354L, "Bình Thuận", 0, 1744013246354L },
                    { 88, 0, 1744013246354L, "Vĩnh Phúc", 0, 1744013246354L },
                    { 89, 0, 1744013246354L, "Hưng Yên", 0, 1744013246354L },
                    { 90, 0, 1744013246354L, "Hà Nam", 0, 1744013246354L },
                    { 92, 0, 1744013246354L, "Quảng Nam", 0, 1744013246354L },
                    { 93, 0, 1744013246354L, "Bình Phước", 0, 1744013246354L },
                    { 94, 0, 1744013246354L, "Bạc Liêu", 0, 1744013246354L },
                    { 95, 0, 1744013246354L, "Hậu Giang", 0, 1744013246354L },
                    { 97, 0, 1744013246355L, "Bắc Cạn", 0, 1744013246355L },
                    { 98, 0, 1744013246355L, "Bắc Giang", 0, 1744013246355L },
                    { 99, 0, 1744013246355L, "Bắc Ninh", 0, 1744013246355L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountCategory_CategoriesId",
                table: "AccountCategory",
                column: "CategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CityId",
                table: "Accounts",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_OwnerId",
                table: "Businesses",
                column: "OwnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryPlace_PlacesId",
                table: "CategoryPlace",
                column: "PlacesId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_SenderId",
                table: "Feedbacks",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_RequesteeId",
                table: "Friendships",
                column: "RequesteeId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_RequesterId",
                table: "Friendships",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Places_CityId",
                table: "Places",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Places_OwnerId",
                table: "Places",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountCategory");

            migrationBuilder.DropTable(
                name: "Businesses");

            migrationBuilder.DropTable(
                name: "CategoryPlace");

            migrationBuilder.DropTable(
                name: "Emails");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Friendships");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
