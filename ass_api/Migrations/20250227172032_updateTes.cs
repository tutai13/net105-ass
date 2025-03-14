using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ass_api.Migrations
{
    /// <inheritdoc />
    public partial class updateTes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDetails_Products_ProductID",
                table: "HoaDonDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_AspNetUsers_UserId",
                table: "HoaDons");

            migrationBuilder.DropIndex(
                name: "IX_HoaDons_UserId",
                table: "HoaDons");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDetails_ProductID",
                table: "HoaDonDetails");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "HoaDons");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "HoaDonDetails");

            migrationBuilder.AlterColumn<string>(
                name: "KhachHangID",
                table: "HoaDons",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_KhachHangID",
                table: "HoaDons",
                column: "KhachHangID");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDetails_SanPhamID",
                table: "HoaDonDetails",
                column: "SanPhamID");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDetails_Products_SanPhamID",
                table: "HoaDonDetails",
                column: "SanPhamID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_AspNetUsers_KhachHangID",
                table: "HoaDons",
                column: "KhachHangID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDetails_Products_SanPhamID",
                table: "HoaDonDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_AspNetUsers_KhachHangID",
                table: "HoaDons");

            migrationBuilder.DropIndex(
                name: "IX_HoaDons_KhachHangID",
                table: "HoaDons");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDetails_SanPhamID",
                table: "HoaDonDetails");

            migrationBuilder.AlterColumn<string>(
                name: "KhachHangID",
                table: "HoaDons",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "HoaDons",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductID",
                table: "HoaDonDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_UserId",
                table: "HoaDons",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDetails_ProductID",
                table: "HoaDonDetails",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDetails_Products_ProductID",
                table: "HoaDonDetails",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_AspNetUsers_UserId",
                table: "HoaDons",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
