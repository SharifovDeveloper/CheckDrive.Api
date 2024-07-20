using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckDrive.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Car",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Mileage = table.Column<int>(type: "int", nullable: false),
                    MeduimFuelConsumption = table.Column<double>(type: "float", nullable: false),
                    FuelTankCapacity = table.Column<double>(type: "float", nullable: false),
                    RemainingFuel = table.Column<double>(type: "float", nullable: false),
                    ManufacturedYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Car", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UndeliveredMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SendingMessageStatus = table.Column<int>(type: "int", nullable: false),
                    ReviewId = table.Column<int>(type: "int", maxLength: 255, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UndeliveredMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Bithdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dispatcher",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dispatcher", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dispatcher_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Doctor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doctor_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Driver",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Driver", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Driver_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mechanic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mechanic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mechanic_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operator_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctorReview",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsHealthy = table.Column<bool>(type: "bit", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorReview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorReview_Doctor_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctor",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DoctorReview_Driver_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Driver",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MechanicAcceptance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Status = table.Column<int>(type: "int", maxLength: 255, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Distance = table.Column<double>(type: "float", nullable: false),
                    MechanicId = table.Column<int>(type: "int", nullable: false),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MechanicAcceptance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MechanicAcceptance_Car_CarId",
                        column: x => x.CarId,
                        principalTable: "Car",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MechanicAcceptance_Driver_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Driver",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MechanicAcceptance_Mechanic_MechanicId",
                        column: x => x.MechanicId,
                        principalTable: "Mechanic",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MechanicHandover",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsHanded = table.Column<bool>(type: "bit", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Status = table.Column<int>(type: "int", maxLength: 255, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Distance = table.Column<double>(type: "float", nullable: false),
                    MechanicId = table.Column<int>(type: "int", nullable: false),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MechanicHandover", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MechanicHandover_Car_CarId",
                        column: x => x.CarId,
                        principalTable: "Car",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MechanicHandover_Driver_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Driver",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MechanicHandover_Mechanic_MechanicId",
                        column: x => x.MechanicId,
                        principalTable: "Mechanic",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OperatorReview",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsGiven = table.Column<bool>(type: "bit", nullable: false),
                    OilAmount = table.Column<double>(type: "float", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    OilMarks = table.Column<int>(type: "int", maxLength: 255, nullable: false),
                    Status = table.Column<int>(type: "int", maxLength: 255, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OperatorId = table.Column<int>(type: "int", nullable: false),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatorReview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperatorReview_Car_CarId",
                        column: x => x.CarId,
                        principalTable: "Car",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OperatorReview_Driver_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Driver",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OperatorReview_Operator_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operator",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DispatcherReview",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FuelSpended = table.Column<double>(type: "float", nullable: false),
                    DistanceCovered = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DispatcherId = table.Column<int>(type: "int", nullable: false),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    OperatorId = table.Column<int>(type: "int", nullable: false),
                    MechanicId = table.Column<int>(type: "int", nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: false),
                    OperatorReviewId = table.Column<int>(type: "int", nullable: false),
                    MechanicHandoverId = table.Column<int>(type: "int", nullable: false),
                    MechanicAcceptanceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispatcherReview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DispatcherReview_Car_CarId",
                        column: x => x.CarId,
                        principalTable: "Car",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DispatcherReview_Dispatcher_DispatcherId",
                        column: x => x.DispatcherId,
                        principalTable: "Dispatcher",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DispatcherReview_Driver_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Driver",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DispatcherReview_MechanicAcceptance_MechanicAcceptanceId",
                        column: x => x.MechanicAcceptanceId,
                        principalTable: "MechanicAcceptance",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DispatcherReview_MechanicHandover_MechanicHandoverId",
                        column: x => x.MechanicHandoverId,
                        principalTable: "MechanicHandover",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DispatcherReview_Mechanic_MechanicId",
                        column: x => x.MechanicId,
                        principalTable: "Mechanic",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DispatcherReview_OperatorReview_OperatorReviewId",
                        column: x => x.OperatorReviewId,
                        principalTable: "OperatorReview",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DispatcherReview_Operator_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Operator",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_RoleId",
                table: "Account",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Dispatcher_AccountId",
                table: "Dispatcher",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DispatcherReview_CarId",
                table: "DispatcherReview",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_DispatcherReview_DispatcherId",
                table: "DispatcherReview",
                column: "DispatcherId");

            migrationBuilder.CreateIndex(
                name: "IX_DispatcherReview_DriverId",
                table: "DispatcherReview",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_DispatcherReview_MechanicAcceptanceId",
                table: "DispatcherReview",
                column: "MechanicAcceptanceId");

            migrationBuilder.CreateIndex(
                name: "IX_DispatcherReview_MechanicHandoverId",
                table: "DispatcherReview",
                column: "MechanicHandoverId");

            migrationBuilder.CreateIndex(
                name: "IX_DispatcherReview_MechanicId",
                table: "DispatcherReview",
                column: "MechanicId");

            migrationBuilder.CreateIndex(
                name: "IX_DispatcherReview_OperatorId",
                table: "DispatcherReview",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_DispatcherReview_OperatorReviewId",
                table: "DispatcherReview",
                column: "OperatorReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_AccountId",
                table: "Doctor",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorReview_DoctorId",
                table: "DoctorReview",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorReview_DriverId",
                table: "DoctorReview",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Driver_AccountId",
                table: "Driver",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Mechanic_AccountId",
                table: "Mechanic",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MechanicAcceptance_CarId",
                table: "MechanicAcceptance",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_MechanicAcceptance_DriverId",
                table: "MechanicAcceptance",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_MechanicAcceptance_MechanicId",
                table: "MechanicAcceptance",
                column: "MechanicId");

            migrationBuilder.CreateIndex(
                name: "IX_MechanicHandover_CarId",
                table: "MechanicHandover",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_MechanicHandover_DriverId",
                table: "MechanicHandover",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_MechanicHandover_MechanicId",
                table: "MechanicHandover",
                column: "MechanicId");

            migrationBuilder.CreateIndex(
                name: "IX_Operator_AccountId",
                table: "Operator",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OperatorReview_CarId",
                table: "OperatorReview",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_OperatorReview_DriverId",
                table: "OperatorReview",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_OperatorReview_OperatorId",
                table: "OperatorReview",
                column: "OperatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DispatcherReview");

            migrationBuilder.DropTable(
                name: "DoctorReview");

            migrationBuilder.DropTable(
                name: "UndeliveredMessage");

            migrationBuilder.DropTable(
                name: "Dispatcher");

            migrationBuilder.DropTable(
                name: "MechanicAcceptance");

            migrationBuilder.DropTable(
                name: "MechanicHandover");

            migrationBuilder.DropTable(
                name: "OperatorReview");

            migrationBuilder.DropTable(
                name: "Doctor");

            migrationBuilder.DropTable(
                name: "Mechanic");

            migrationBuilder.DropTable(
                name: "Car");

            migrationBuilder.DropTable(
                name: "Driver");

            migrationBuilder.DropTable(
                name: "Operator");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
