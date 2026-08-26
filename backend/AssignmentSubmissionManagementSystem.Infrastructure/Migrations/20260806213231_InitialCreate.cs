using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AssignmentSubmissionManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "institutions",
                columns: table => new
                {
                    institutionid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutioncode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    institutionname = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    phonenumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("institutions_pkey", x => x.institutionid);
                    table.CheckConstraint("ck_institutions_code", "length(trim(institutioncode)) > 0");
                    table.CheckConstraint("ck_institutions_name", "length(trim(institutionname)) > 0");
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    roleid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rolename = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("roles_pkey", x => x.roleid);
                    table.CheckConstraint("ck_roles_name", "length(trim(rolename)) > 0");
                });

            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    departmentid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionid = table.Column<long>(type: "bigint", nullable: false),
                    departmentcode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    departmentname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("departments_pkey", x => x.departmentid);
                    table.UniqueConstraint("uq_departments_institution_department", x => new { x.institutionid, x.departmentid });
                    table.CheckConstraint("ck_departments_code", "length(trim(departmentcode)) > 0");
                    table.CheckConstraint("ck_departments_name", "length(trim(departmentname)) > 0");
                    table.ForeignKey(
                        name: "fk_departments_institutions",
                        column: x => x.institutionid,
                        principalTable: "institutions",
                        principalColumn: "institutionid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "latesubmissionpolicies",
                columns: table => new
                {
                    policyid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionid = table.Column<long>(type: "bigint", nullable: false),
                    policyname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    penaltypercentage = table.Column<int>(type: "integer", nullable: false, defaultValue: 25),
                    isactive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("latesubmissionpolicies_pkey", x => x.policyid);
                    table.UniqueConstraint("uq_latesubmissionpolicies_institution_policy", x => new { x.institutionid, x.policyid });
                    table.CheckConstraint("ck_latesubmissionpolicies_name", "length(trim(policyname)) > 0");
                    table.CheckConstraint("ck_latesubmissionpolicies_penalty", "penaltypercentage BETWEEN 0 AND 100");
                    table.ForeignKey(
                        name: "fk_latesubmissionpolicies_institutions",
                        column: x => x.institutionid,
                        principalTable: "institutions",
                        principalColumn: "institutionid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "subjects",
                columns: table => new
                {
                    subjectid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionid = table.Column<long>(type: "bigint", nullable: false),
                    subjectcode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    subjectname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("subjects_pkey", x => x.subjectid);
                    table.UniqueConstraint("uq_subjects_institution_subject", x => new { x.institutionid, x.subjectid });
                    table.CheckConstraint("ck_subjects_code", "length(trim(subjectcode)) > 0");
                    table.CheckConstraint("ck_subjects_name", "length(trim(subjectname)) > 0");
                    table.ForeignKey(
                        name: "fk_subjects_institutions",
                        column: x => x.institutionid,
                        principalTable: "institutions",
                        principalColumn: "institutionid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    userid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionid = table.Column<long>(type: "bigint", nullable: false),
                    roleid = table.Column<long>(type: "bigint", nullable: false),
                    fullname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    passwordhash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    phonenumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.userid);
                    table.UniqueConstraint("uq_users_institution_user", x => new { x.institutionid, x.userid });
                    table.CheckConstraint("ck_users_email", "length(trim(email)) > 0");
                    table.CheckConstraint("ck_users_fullname", "length(trim(fullname)) > 0");
                    table.ForeignKey(
                        name: "fk_users_institutions",
                        column: x => x.institutionid,
                        principalTable: "institutions",
                        principalColumn: "institutionid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_users_roles",
                        column: x => x.roleid,
                        principalTable: "roles",
                        principalColumn: "roleid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    courseid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionid = table.Column<long>(type: "bigint", nullable: false),
                    departmentid = table.Column<long>(type: "bigint", nullable: false),
                    coursecode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    coursename = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("courses_pkey", x => x.courseid);
                    table.UniqueConstraint("uq_courses_institution_course", x => new { x.institutionid, x.courseid });
                    table.CheckConstraint("ck_courses_code", "length(trim(coursecode)) > 0");
                    table.CheckConstraint("ck_courses_name", "length(trim(coursename)) > 0");
                    table.ForeignKey(
                        name: "fk_courses_institutiondepartments",
                        columns: x => new { x.institutionid, x.departmentid },
                        principalTable: "departments",
                        principalColumns: new[] { "institutionid", "departmentid" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "auditlogs",
                columns: table => new
                {
                    auditlogid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionid = table.Column<long>(type: "bigint", nullable: true),
                    userid = table.Column<long>(type: "bigint", nullable: false),
                    action = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    entityname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    entityid = table.Column<long>(type: "bigint", nullable: false),
                    oldvalues = table.Column<string>(type: "jsonb", nullable: true),
                    newvalues = table.Column<string>(type: "jsonb", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("auditlogs_pkey", x => x.auditlogid);
                    table.CheckConstraint("ck_auditlogs_action", "action IN\r\n(\r\n    'CREATE',\r\n    'UPDATE',\r\n    'DELETE',\r\n    'APPROVE',\r\n    'REJECT',\r\n    'SUBMIT',\r\n    'REVIEW',\r\n    'LOGIN',\r\n    'LOGOUT',\r\n    'PUBLISH'\r\n)");
                    table.CheckConstraint("ck_auditlogs_entityid", "entityid > 0");
                    table.CheckConstraint("ck_auditlogs_entityname", "length(trim(entityname)) > 0");
                    table.ForeignKey(
                        name: "fk_auditlogs_institutions",
                        column: x => x.institutionid,
                        principalTable: "institutions",
                        principalColumn: "institutionid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_auditlogs_users",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    notificationid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionid = table.Column<long>(type: "bigint", nullable: true),
                    userid = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    notificationtype = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    channel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "InApp"),
                    referenceid = table.Column<long>(type: "bigint", nullable: true),
                    isread = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    readat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("notifications_pkey", x => x.notificationid);
                    table.CheckConstraint("ck_notifications_channel", "channel IN ('InApp', 'Email', 'SMS', 'Push')");
                    table.CheckConstraint("ck_notifications_message", "length(trim(message)) > 0");
                    table.CheckConstraint("ck_notifications_readstate", "(isread = FALSE AND readat IS NULL)\r\nOR\r\n(isread = TRUE AND readat IS NOT NULL)");
                    table.CheckConstraint("ck_notifications_title", "length(trim(title)) > 0");
                    table.ForeignKey(
                        name: "fk_notifications_institutions",
                        column: x => x.institutionid,
                        principalTable: "institutions",
                        principalColumn: "institutionid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_notifications_users",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "studentprofiles",
                columns: table => new
                {
                    studentid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionid = table.Column<long>(type: "bigint", nullable: false),
                    userid = table.Column<long>(type: "bigint", nullable: false),
                    studentcode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    admissiondate = table.Column<DateOnly>(type: "date", nullable: true),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("studentprofiles_pkey", x => x.studentid);
                    table.UniqueConstraint("uq_studentprofiles_institution_student", x => new { x.institutionid, x.studentid });
                    table.CheckConstraint("ck_studentprofiles_studentcode", "length(trim(studentcode)) > 0");
                    table.ForeignKey(
                        name: "fk_studentprofiles_institutionusers",
                        columns: x => new { x.institutionid, x.userid },
                        principalTable: "users",
                        principalColumns: new[] { "institutionid", "userid" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "teacherprofiles",
                columns: table => new
                {
                    teacherid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionid = table.Column<long>(type: "bigint", nullable: false),
                    userid = table.Column<long>(type: "bigint", nullable: false),
                    departmentid = table.Column<long>(type: "bigint", nullable: false),
                    employeecode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    qualification = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    joiningdate = table.Column<DateOnly>(type: "date", nullable: true),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("teacherprofiles_pkey", x => x.teacherid);
                    table.UniqueConstraint("uq_teacherprofiles_institution_teacher", x => new { x.institutionid, x.teacherid });
                    table.CheckConstraint("ck_teacherprofiles_employeecode", "length(trim(employeecode)) > 0");
                    table.ForeignKey(
                        name: "fk_teacherprofiles_institutiondepartments",
                        columns: x => new { x.institutionid, x.departmentid },
                        principalTable: "departments",
                        principalColumns: new[] { "institutionid", "departmentid" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_teacherprofiles_institutionusers",
                        columns: x => new { x.institutionid, x.userid },
                        principalTable: "users",
                        principalColumns: new[] { "institutionid", "userid" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "batches",
                columns: table => new
                {
                    batchid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionid = table.Column<long>(type: "bigint", nullable: false),
                    courseid = table.Column<long>(type: "bigint", nullable: false),
                    batchcode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    batchname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    startyear = table.Column<int>(type: "integer", nullable: false),
                    endyear = table.Column<int>(type: "integer", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("batches_pkey", x => x.batchid);
                    table.UniqueConstraint("uq_batches_institution_batch", x => new { x.institutionid, x.batchid });
                    table.CheckConstraint("ck_batches_code", "length(trim(batchcode)) > 0");
                    table.CheckConstraint("ck_batches_name", "length(trim(batchname)) > 0");
                    table.CheckConstraint("ck_batches_yearrange", "endyear IS NULL OR endyear >= startyear");
                    table.ForeignKey(
                        name: "fk_batches_institutioncourses",
                        columns: x => new { x.institutionid, x.courseid },
                        principalTable: "courses",
                        principalColumns: new[] { "institutionid", "courseid" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "coursesubjects",
                columns: table => new
                {
                    coursesubjectid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionid = table.Column<long>(type: "bigint", nullable: false),
                    courseid = table.Column<long>(type: "bigint", nullable: false),
                    subjectid = table.Column<long>(type: "bigint", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("coursesubjects_pkey", x => x.coursesubjectid);
                    table.UniqueConstraint("uq_coursesubjects_institution_coursesubject", x => new { x.institutionid, x.coursesubjectid });
                    table.ForeignKey(
                        name: "fk_coursesubjects_institutioncourses",
                        columns: x => new { x.institutionid, x.courseid },
                        principalTable: "courses",
                        principalColumns: new[] { "institutionid", "courseid" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_coursesubjects_institutionsubjects",
                        columns: x => new { x.institutionid, x.subjectid },
                        principalTable: "subjects",
                        principalColumns: new[] { "institutionid", "subjectid" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "academicyears",
                columns: table => new
                {
                    academicyearid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionid = table.Column<long>(type: "bigint", nullable: false),
                    batchid = table.Column<long>(type: "bigint", nullable: false),
                    yearname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    yearorder = table.Column<int>(type: "integer", nullable: false),
                    isactive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("academicyears_pkey", x => x.academicyearid);
                    table.UniqueConstraint("uq_academicyears_institution_academicyear", x => new { x.institutionid, x.academicyearid });
                    table.CheckConstraint("ck_academicyears_name", "length(trim(yearname)) > 0");
                    table.CheckConstraint("ck_academicyears_yearorder", "yearorder > 0");
                    table.ForeignKey(
                        name: "fk_academicyears_institutionbatches",
                        columns: x => new { x.institutionid, x.batchid },
                        principalTable: "batches",
                        principalColumns: new[] { "institutionid", "batchid" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "teachersubjects",
                columns: table => new
                {
                    teachersubjectid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionid = table.Column<long>(type: "bigint", nullable: false),
                    teacherid = table.Column<long>(type: "bigint", nullable: false),
                    coursesubjectid = table.Column<long>(type: "bigint", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("teachersubjects_pkey", x => x.teachersubjectid);
                    table.UniqueConstraint("uq_teachersubjects_institution_teacher_coursesubject", x => new { x.institutionid, x.teacherid, x.coursesubjectid });
                    table.ForeignKey(
                        name: "fk_teachersubjects_institutioncoursesubjects",
                        columns: x => new { x.institutionid, x.coursesubjectid },
                        principalTable: "coursesubjects",
                        principalColumns: new[] { "institutionid", "coursesubjectid" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_teachersubjects_institutionteachers",
                        columns: x => new { x.institutionid, x.teacherid },
                        principalTable: "teacherprofiles",
                        principalColumns: new[] { "institutionid", "teacherid" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "studentenrollments",
                columns: table => new
                {
                    enrollmentid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionid = table.Column<long>(type: "bigint", nullable: false),
                    studentid = table.Column<long>(type: "bigint", nullable: false),
                    academicyearid = table.Column<long>(type: "bigint", nullable: false),
                    rollnumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    enrollmentdate = table.Column<DateOnly>(type: "date", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("studentenrollments_pkey", x => x.enrollmentid);
                    table.CheckConstraint("ck_studentenrollments_roll", "length(trim(rollnumber)) > 0");
                    table.ForeignKey(
                        name: "fk_studentenrollments_institutionacademicyears",
                        columns: x => new { x.institutionid, x.academicyearid },
                        principalTable: "academicyears",
                        principalColumns: new[] { "institutionid", "academicyearid" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_studentenrollments_institutionstudents",
                        columns: x => new { x.institutionid, x.studentid },
                        principalTable: "studentprofiles",
                        principalColumns: new[] { "institutionid", "studentid" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "assignments",
                columns: table => new
                {
                    assignmentid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionid = table.Column<long>(type: "bigint", nullable: false),
                    teacherid = table.Column<long>(type: "bigint", nullable: false),
                    coursesubjectid = table.Column<long>(type: "bigint", nullable: false),
                    academicyearid = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    totalmarks = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    duedate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    allowlatesubmission = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    latesubmissiondeadline = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    assignmentstatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Draft"),
                    isactive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("assignments_pkey", x => x.assignmentid);
                    table.UniqueConstraint("uq_assignments_institution_assignment", x => new { x.institutionid, x.assignmentid });
                    table.CheckConstraint("ck_assignments_latesubmission", "(allowlatesubmission = FALSE AND latesubmissiondeadline IS NULL)\r\nOR\r\n(allowlatesubmission = TRUE\r\n AND latesubmissiondeadline IS NOT NULL\r\n AND latesubmissiondeadline > duedate)");
                    table.CheckConstraint("ck_assignments_status", "assignmentstatus IN ('Draft', 'Published', 'Closed', 'Archived')");
                    table.CheckConstraint("ck_assignments_title", "length(trim(title)) > 0");
                    table.CheckConstraint("ck_assignments_totalmarks", "totalmarks > 0");
                    table.ForeignKey(
                        name: "fk_assignments_institutionacademicyears",
                        columns: x => new { x.institutionid, x.academicyearid },
                        principalTable: "academicyears",
                        principalColumns: new[] { "institutionid", "academicyearid" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_assignments_institutioncoursesubjects",
                        columns: x => new { x.institutionid, x.coursesubjectid },
                        principalTable: "coursesubjects",
                        principalColumns: new[] { "institutionid", "coursesubjectid" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_assignments_institutionteachers",
                        columns: x => new { x.institutionid, x.teacherid },
                        principalTable: "teacherprofiles",
                        principalColumns: new[] { "institutionid", "teacherid" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_assignments_institutionteachersubjects",
                        columns: x => new { x.institutionid, x.teacherid, x.coursesubjectid },
                        principalTable: "teachersubjects",
                        principalColumns: new[] { "institutionid", "teacherid", "coursesubjectid" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "assignmentattachments",
                columns: table => new
                {
                    attachmentid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionid = table.Column<long>(type: "bigint", nullable: false),
                    assignmentid = table.Column<long>(type: "bigint", nullable: false),
                    filename = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    filepath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    filetype = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    filesize = table.Column<long>(type: "bigint", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("assignmentattachments_pkey", x => x.attachmentid);
                    table.CheckConstraint("ck_assignmentattachments_filename", "length(trim(filename)) > 0");
                    table.CheckConstraint("ck_assignmentattachments_filepath", "length(trim(filepath)) > 0");
                    table.CheckConstraint("ck_assignmentattachments_filesize", "filesize IS NULL OR filesize >= 0");
                    table.ForeignKey(
                        name: "fk_assignmentattachments_institutionassignments",
                        columns: x => new { x.institutionid, x.assignmentid },
                        principalTable: "assignments",
                        principalColumns: new[] { "institutionid", "assignmentid" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "submissions",
                columns: table => new
                {
                    submissionid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionid = table.Column<long>(type: "bigint", nullable: false),
                    assignmentid = table.Column<long>(type: "bigint", nullable: false),
                    studentid = table.Column<long>(type: "bigint", nullable: false),
                    submissionversion = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    submissiontext = table.Column<string>(type: "text", nullable: true),
                    submittedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    islatesubmission = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    islatestsubmission = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    submissionstatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Submitted"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("submissions_pkey", x => x.submissionid);
                    table.UniqueConstraint("uq_submissions_institution_submission", x => new { x.institutionid, x.submissionid });
                    table.CheckConstraint("ck_submissions_content", "submissiontext IS NULL OR length(trim(submissiontext)) > 0");
                    table.CheckConstraint("ck_submissions_status", "submissionstatus IN ('Submitted', 'UnderReview', 'Reviewed', 'Returned')");
                    table.CheckConstraint("ck_submissions_version", "submissionversion > 0");
                    table.ForeignKey(
                        name: "fk_submissions_institutionassignments",
                        columns: x => new { x.institutionid, x.assignmentid },
                        principalTable: "assignments",
                        principalColumns: new[] { "institutionid", "assignmentid" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_submissions_institutionstudents",
                        columns: x => new { x.institutionid, x.studentid },
                        principalTable: "studentprofiles",
                        principalColumns: new[] { "institutionid", "studentid" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "assessments",
                columns: table => new
                {
                    assessmentid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionid = table.Column<long>(type: "bigint", nullable: false),
                    submissionid = table.Column<long>(type: "bigint", nullable: false),
                    teacherid = table.Column<long>(type: "bigint", nullable: false),
                    policyid = table.Column<long>(type: "bigint", nullable: true),
                    marksobtained = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    penaltypercentageapplied = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    finalmarks = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    feedback = table.Column<string>(type: "text", nullable: true),
                    reviewedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("assessments_pkey", x => x.assessmentid);
                    table.CheckConstraint("ck_assessments_marks", "marksobtained >= 0\r\nAND finalmarks >= 0\r\nAND finalmarks <= marksobtained");
                    table.CheckConstraint("ck_assessments_penaltypercentage", "penaltypercentageapplied BETWEEN 0 AND 100");
                    table.CheckConstraint("ck_assessments_policyusage", "(policyid IS NULL AND penaltypercentageapplied = 0)\r\nOR\r\n(policyid IS NOT NULL)");
                    table.ForeignKey(
                        name: "fk_assessments_institutionpolicies",
                        columns: x => new { x.institutionid, x.policyid },
                        principalTable: "latesubmissionpolicies",
                        principalColumns: new[] { "institutionid", "policyid" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_assessments_institutionsubmissions",
                        columns: x => new { x.institutionid, x.submissionid },
                        principalTable: "submissions",
                        principalColumns: new[] { "institutionid", "submissionid" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_assessments_institutionteachers",
                        columns: x => new { x.institutionid, x.teacherid },
                        principalTable: "teacherprofiles",
                        principalColumns: new[] { "institutionid", "teacherid" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "resubmissionrequests",
                columns: table => new
                {
                    requestid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionid = table.Column<long>(type: "bigint", nullable: false),
                    submissionid = table.Column<long>(type: "bigint", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: false),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("resubmissionrequests_pkey", x => x.requestid);
                    table.UniqueConstraint("uq_resubmissionrequests_institution_request", x => new { x.institutionid, x.requestid });
                    table.CheckConstraint("ck_resubmissionrequests_reason", "length(trim(reason)) > 0");
                    table.ForeignKey(
                        name: "fk_resubmissionrequests_institutionsubmissions",
                        columns: x => new { x.institutionid, x.submissionid },
                        principalTable: "submissions",
                        principalColumns: new[] { "institutionid", "submissionid" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "submissionattachments",
                columns: table => new
                {
                    attachmentid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionid = table.Column<long>(type: "bigint", nullable: false),
                    submissionid = table.Column<long>(type: "bigint", nullable: false),
                    filename = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    filepath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    filetype = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    filesize = table.Column<long>(type: "bigint", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("submissionattachments_pkey", x => x.attachmentid);
                    table.CheckConstraint("ck_submissionattachments_filename", "length(trim(filename)) > 0");
                    table.CheckConstraint("ck_submissionattachments_filepath", "length(trim(filepath)) > 0");
                    table.CheckConstraint("ck_submissionattachments_filesize", "filesize IS NULL OR filesize >= 0");
                    table.ForeignKey(
                        name: "fk_submissionattachments_institutionsubmissions",
                        columns: x => new { x.institutionid, x.submissionid },
                        principalTable: "submissions",
                        principalColumns: new[] { "institutionid", "submissionid" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "resubmissionapprovals",
                columns: table => new
                {
                    approvalid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institutionid = table.Column<long>(type: "bigint", nullable: false),
                    requestid = table.Column<long>(type: "bigint", nullable: false),
                    teacherid = table.Column<long>(type: "bigint", nullable: false),
                    approvalstatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    remarks = table.Column<string>(type: "text", nullable: true),
                    reviewedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isused = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    usedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("resubmissionapprovals_pkey", x => x.approvalid);
                    table.CheckConstraint("ck_resubmissionapprovals_reviewedat", "(approvalstatus = 'Pending' AND reviewedat IS NULL)\r\nOR\r\n(approvalstatus IN ('Approved', 'Rejected')\r\n AND reviewedat IS NOT NULL)");
                    table.CheckConstraint("ck_resubmissionapprovals_status", "approvalstatus IN ('Pending', 'Approved', 'Rejected')");
                    table.CheckConstraint("ck_resubmissionapprovals_usage", "(isused = FALSE AND usedat IS NULL)\r\nOR\r\n(isused = TRUE\r\n AND approvalstatus = 'Approved'\r\n AND usedat IS NOT NULL)");
                    table.ForeignKey(
                        name: "fk_resubmissionapprovals_institutionrequests",
                        columns: x => new { x.institutionid, x.requestid },
                        principalTable: "resubmissionrequests",
                        principalColumns: new[] { "institutionid", "requestid" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_resubmissionapprovals_institutionteachers",
                        columns: x => new { x.institutionid, x.teacherid },
                        principalTable: "teacherprofiles",
                        principalColumns: new[] { "institutionid", "teacherid" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_academicyears_batchid",
                table: "academicyears",
                column: "batchid");

            migrationBuilder.CreateIndex(
                name: "uq_academicyears_institution_batch_yearname",
                table: "academicyears",
                columns: new[] { "institutionid", "batchid", "yearname" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_academicyears_institution_batch_yearorder",
                table: "academicyears",
                columns: new[] { "institutionid", "batchid", "yearorder" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_assessments_institutionid_policyid",
                table: "assessments",
                columns: new[] { "institutionid", "policyid" });

            migrationBuilder.CreateIndex(
                name: "IX_assessments_institutionid_teacherid",
                table: "assessments",
                columns: new[] { "institutionid", "teacherid" });

            migrationBuilder.CreateIndex(
                name: "ix_assessments_teacherid",
                table: "assessments",
                column: "teacherid");

            migrationBuilder.CreateIndex(
                name: "uq_assessments_institution_submission",
                table: "assessments",
                columns: new[] { "institutionid", "submissionid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_assignmentattachments_institutionid_assignmentid",
                table: "assignmentattachments",
                columns: new[] { "institutionid", "assignmentid" });

            migrationBuilder.CreateIndex(
                name: "ix_assignments_academicyearid",
                table: "assignments",
                column: "academicyearid");

            migrationBuilder.CreateIndex(
                name: "ix_assignments_coursesubjectid",
                table: "assignments",
                column: "coursesubjectid");

            migrationBuilder.CreateIndex(
                name: "IX_assignments_institutionid_academicyearid",
                table: "assignments",
                columns: new[] { "institutionid", "academicyearid" });

            migrationBuilder.CreateIndex(
                name: "IX_assignments_institutionid_coursesubjectid",
                table: "assignments",
                columns: new[] { "institutionid", "coursesubjectid" });

            migrationBuilder.CreateIndex(
                name: "IX_assignments_institutionid_teacherid_coursesubjectid",
                table: "assignments",
                columns: new[] { "institutionid", "teacherid", "coursesubjectid" });

            migrationBuilder.CreateIndex(
                name: "ix_auditlogs_institution_user_createdat",
                table: "auditlogs",
                columns: new[] { "institutionid", "userid", "createdat" });

            migrationBuilder.CreateIndex(
                name: "IX_auditlogs_userid",
                table: "auditlogs",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "ix_batches_courseid",
                table: "batches",
                column: "courseid");

            migrationBuilder.CreateIndex(
                name: "IX_batches_institutionid_courseid",
                table: "batches",
                columns: new[] { "institutionid", "courseid" });

            migrationBuilder.CreateIndex(
                name: "uq_batches_institution_code",
                table: "batches",
                columns: new[] { "institutionid", "batchcode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_courses_departmentid",
                table: "courses",
                column: "departmentid");

            migrationBuilder.CreateIndex(
                name: "IX_courses_institutionid_departmentid",
                table: "courses",
                columns: new[] { "institutionid", "departmentid" });

            migrationBuilder.CreateIndex(
                name: "uq_courses_institution_code",
                table: "courses",
                columns: new[] { "institutionid", "coursecode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_coursesubjects_institutionid_subjectid",
                table: "coursesubjects",
                columns: new[] { "institutionid", "subjectid" });

            migrationBuilder.CreateIndex(
                name: "ix_coursesubjects_subjectid",
                table: "coursesubjects",
                column: "subjectid");

            migrationBuilder.CreateIndex(
                name: "uq_coursesubjects_institution_course_subject",
                table: "coursesubjects",
                columns: new[] { "institutionid", "courseid", "subjectid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_departments_institution_code",
                table: "departments",
                columns: new[] { "institutionid", "departmentcode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "institutions_institutioncode_key",
                table: "institutions",
                column: "institutioncode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_latesubmissionpolicies_institution_name",
                table: "latesubmissionpolicies",
                columns: new[] { "institutionid", "policyname" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_latesubmissionpolicies_onlyoneactive",
                table: "latesubmissionpolicies",
                column: "institutionid",
                unique: true,
                filter: "isactive = TRUE");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_institution_user_isread",
                table: "notifications",
                columns: new[] { "institutionid", "userid", "isread" });

            migrationBuilder.CreateIndex(
                name: "IX_notifications_userid",
                table: "notifications",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_resubmissionapprovals_institutionid_teacherid",
                table: "resubmissionapprovals",
                columns: new[] { "institutionid", "teacherid" });

            migrationBuilder.CreateIndex(
                name: "uq_resubmissionapprovals_institution_request",
                table: "resubmissionapprovals",
                columns: new[] { "institutionid", "requestid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_resubmissionrequests_institutionid_submissionid",
                table: "resubmissionrequests",
                columns: new[] { "institutionid", "submissionid" });

            migrationBuilder.CreateIndex(
                name: "roles_rolename_key",
                table: "roles",
                column: "rolename",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_studentenrollments_academicyearid",
                table: "studentenrollments",
                column: "academicyearid");

            migrationBuilder.CreateIndex(
                name: "uq_studentenrollments_institution_academicyear_roll",
                table: "studentenrollments",
                columns: new[] { "institutionid", "academicyearid", "rollnumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_studentenrollments_institution_enrollment",
                table: "studentenrollments",
                columns: new[] { "institutionid", "enrollmentid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_studentenrollments_institution_student_academicyear",
                table: "studentenrollments",
                columns: new[] { "institutionid", "studentid", "academicyearid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_studentenrollments_onlyoneactive",
                table: "studentenrollments",
                columns: new[] { "institutionid", "studentid" },
                unique: true,
                filter: "isactive = TRUE");

            migrationBuilder.CreateIndex(
                name: "uq_studentprofiles_institution_studentcode",
                table: "studentprofiles",
                columns: new[] { "institutionid", "studentcode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_studentprofiles_institution_user",
                table: "studentprofiles",
                columns: new[] { "institutionid", "userid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_subjects_institutionid",
                table: "subjects",
                column: "institutionid");

            migrationBuilder.CreateIndex(
                name: "uq_subjects_institution_code",
                table: "subjects",
                columns: new[] { "institutionid", "subjectcode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_submissionattachments_institutionid_submissionid",
                table: "submissionattachments",
                columns: new[] { "institutionid", "submissionid" });

            migrationBuilder.CreateIndex(
                name: "ix_submissions_assignmentid",
                table: "submissions",
                column: "assignmentid");

            migrationBuilder.CreateIndex(
                name: "IX_submissions_institutionid_studentid",
                table: "submissions",
                columns: new[] { "institutionid", "studentid" });

            migrationBuilder.CreateIndex(
                name: "ix_submissions_studentid",
                table: "submissions",
                column: "studentid");

            migrationBuilder.CreateIndex(
                name: "uq_submissions_institution_assignment_student_version",
                table: "submissions",
                columns: new[] { "institutionid", "assignmentid", "studentid", "submissionversion" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_submissions_onlyonelatest",
                table: "submissions",
                columns: new[] { "institutionid", "assignmentid", "studentid" },
                unique: true,
                filter: "islatestsubmission = TRUE");

            migrationBuilder.CreateIndex(
                name: "IX_teacherprofiles_institutionid_departmentid",
                table: "teacherprofiles",
                columns: new[] { "institutionid", "departmentid" });

            migrationBuilder.CreateIndex(
                name: "uq_teacherprofiles_institution_employeecode",
                table: "teacherprofiles",
                columns: new[] { "institutionid", "employeecode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_teacherprofiles_institution_user",
                table: "teacherprofiles",
                columns: new[] { "institutionid", "userid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_teachersubjects_coursesubjectid",
                table: "teachersubjects",
                column: "coursesubjectid");

            migrationBuilder.CreateIndex(
                name: "IX_teachersubjects_institutionid_coursesubjectid",
                table: "teachersubjects",
                columns: new[] { "institutionid", "coursesubjectid" });

            migrationBuilder.CreateIndex(
                name: "uq_teachersubjects_institution_teachersubject",
                table: "teachersubjects",
                columns: new[] { "institutionid", "teachersubjectid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_roleid",
                table: "users",
                column: "roleid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assessments");

            migrationBuilder.DropTable(
                name: "assignmentattachments");

            migrationBuilder.DropTable(
                name: "auditlogs");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "resubmissionapprovals");

            migrationBuilder.DropTable(
                name: "studentenrollments");

            migrationBuilder.DropTable(
                name: "submissionattachments");

            migrationBuilder.DropTable(
                name: "latesubmissionpolicies");

            migrationBuilder.DropTable(
                name: "resubmissionrequests");

            migrationBuilder.DropTable(
                name: "submissions");

            migrationBuilder.DropTable(
                name: "assignments");

            migrationBuilder.DropTable(
                name: "studentprofiles");

            migrationBuilder.DropTable(
                name: "academicyears");

            migrationBuilder.DropTable(
                name: "teachersubjects");

            migrationBuilder.DropTable(
                name: "batches");

            migrationBuilder.DropTable(
                name: "coursesubjects");

            migrationBuilder.DropTable(
                name: "teacherprofiles");

            migrationBuilder.DropTable(
                name: "courses");

            migrationBuilder.DropTable(
                name: "subjects");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "institutions");
        }
    }
}
