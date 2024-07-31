using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace demodoan1.Models;

public partial class DbDoAnTotNghiepContext : DbContext
{
    public DbDoAnTotNghiepContext()
    {
    }

    public DbDoAnTotNghiepContext(DbContextOptions<DbDoAnTotNghiepContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Banthao> Banthaos { get; set; }

    public virtual DbSet<Baocao> Baocaos { get; set; }

    public virtual DbSet<Binhluan> Binhluans { get; set; }

    public virtual DbSet<Butdanh> Butdanhs { get; set; }

    public virtual DbSet<Chuongtruyen> Chuongtruyens { get; set; }

    public virtual DbSet<Danhdau> Danhdaus { get; set; }

    public virtual DbSet<Danhgia> Danhgia { get; set; }

    public virtual DbSet<Giaodich> Giaodiches { get; set; }

    public virtual DbSet<Lichsudoc> Lichsudocs { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<Phanhoi> Phanhois { get; set; }

    public virtual DbSet<Phanhoibinhluan> Phanhoibinhluans { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Theloai> Theloais { get; set; }

    public virtual DbSet<Truyen> Truyens { get; set; }

    public virtual DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Banthao>(entity =>
        {
            entity.HasKey(e => e.MaBanThao).HasName("PRIMARY");

            entity.ToTable("banthao");

            entity.HasIndex(e => e.MaTruyen, "FK_banThao_Truyen");

            entity.Property(e => e.MaBanThao).HasColumnName("maBanThao");
            entity.Property(e => e.MaTruyen).HasColumnName("maTruyen");
            entity.Property(e => e.NgayCapNhap)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngayCapNhap");
            entity.Property(e => e.Ngaytao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngaytao");
            entity.Property(e => e.Noidung).HasColumnName("noidung");
            entity.Property(e => e.TenBanThao)
                .HasMaxLength(50)
                .HasColumnName("tenBanThao")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MaTruyenNavigation).WithMany(p => p.Banthaos)
                .HasForeignKey(d => d.MaTruyen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_banThao_Truyen");
        });

        modelBuilder.Entity<Baocao>(entity =>
        {
            entity.HasKey(e => e.MaBaoCao).HasName("PRIMARY");

            entity.ToTable("baocao");

            entity.HasIndex(e => e.MaNguoiDung, "FK_BaoCao_NguoiDung");

            entity.Property(e => e.MaBaoCao).HasColumnName("maBaoCao");
            entity.Property(e => e.Loaibaocao).HasColumnName("loaibaocao");
            entity.Property(e => e.MaNguoiDung).HasColumnName("maNguoiDung");
            entity.Property(e => e.MaThucThe).HasColumnName("maThucThe");
            entity.Property(e => e.NgayCapNhap)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngayCapNhap");
            entity.Property(e => e.Ngaytao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngaytao");
            entity.Property(e => e.Noidung)
                .HasColumnType("text")
                .HasColumnName("noidung");
            entity.Property(e => e.Tieude)
                .HasMaxLength(200)
                .HasColumnName("tieude")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Trangthai).HasColumnName("trangthai");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.Baocaos)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BaoCao_NguoiDung");
        });

        modelBuilder.Entity<Binhluan>(entity =>
        {
            entity.HasKey(e => e.MabinhLuan).HasName("PRIMARY");

            entity.ToTable("binhluan");

            entity.HasIndex(e => e.MaNguoiDung, "FK_binhLuan_NguoiDung");

            entity.HasIndex(e => e.MaTruyen, "FK_binhLuan_Truyen");

            entity.Property(e => e.MabinhLuan).HasColumnName("mabinhLuan");
            entity.Property(e => e.MaNguoiDung).HasColumnName("maNguoiDung");
            entity.Property(e => e.MaTruyen).HasColumnName("maTruyen");
            entity.Property(e => e.NgayCapNhap)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngayCapNhap");
            entity.Property(e => e.Ngaytao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngaytao");
            entity.Property(e => e.Noidung)
                .HasColumnType("text")
                .HasColumnName("noidung");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.Binhluans)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_binhLuan_NguoiDung");

            entity.HasOne(d => d.MaTruyenNavigation).WithMany(p => p.Binhluans)
                .HasForeignKey(d => d.MaTruyen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_binhLuan_Truyen");
        });

        modelBuilder.Entity<Butdanh>(entity =>
        {
            entity.HasKey(e => e.MaButDanh).HasName("PRIMARY");

            entity.ToTable("butdanh");

            entity.HasIndex(e => e.MaNguoiDung, "FK_ButDanh_NguoiDung");

            entity.Property(e => e.MaButDanh).HasColumnName("maButDanh");
            entity.Property(e => e.MaNguoiDung).HasColumnName("maNguoiDung");
            entity.Property(e => e.NgayCapNhap)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngayCapNhap");
            entity.Property(e => e.Ngaytao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngaytao");
            entity.Property(e => e.TenButDanh)
                .HasMaxLength(40)
                .HasColumnName("tenButDanh")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Trangthai).HasColumnName("trangthai");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.Butdanhs)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ButDanh_NguoiDung");
        });

        modelBuilder.Entity<Chuongtruyen>(entity =>
        {
            entity.HasKey(e => e.MaChuong).HasName("PRIMARY");

            entity.ToTable("chuongtruyen");

            entity.HasIndex(e => e.MaTruyen, "FK_ChuongTruyen_Truyen");

            entity.Property(e => e.MaChuong).HasColumnName("maChuong");
            entity.Property(e => e.MaTruyen).HasColumnName("maTruyen");
            entity.Property(e => e.NgayCapNhap)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngayCapNhap");
            entity.Property(e => e.Ngaytao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngaytao");
            entity.Property(e => e.NoiDung).HasColumnName("noiDung");
            entity.Property(e => e.Stt).HasColumnName("stt");
            entity.Property(e => e.TenChuong)
                .HasMaxLength(50)
                .HasColumnName("tenChuong")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.TrangThai).HasColumnName("trangThai");

            entity.HasOne(d => d.MaTruyenNavigation).WithMany(p => p.Chuongtruyens)
                .HasForeignKey(d => d.MaTruyen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChuongTruyen_Truyen");
        });

        modelBuilder.Entity<Danhdau>(entity =>
        {
            entity.HasKey(e => new { e.MaTruyen, e.MaNguoiDung })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("danhdau");

            entity.HasIndex(e => e.MaNguoiDung, "FK_danhdau_NguoiDung");

            entity.Property(e => e.MaTruyen).HasColumnName("maTruyen");
            entity.Property(e => e.MaNguoiDung).HasColumnName("maNguoiDung");
            entity.Property(e => e.Ngaytao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngaytao");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.Danhdaus)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_danhdau_NguoiDung");

            entity.HasOne(d => d.MaTruyenNavigation).WithMany(p => p.Danhdaus)
                .HasForeignKey(d => d.MaTruyen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_danhdau_Truyen");
        });

        modelBuilder.Entity<Danhgia>(entity =>
        {
            entity.HasKey(e => e.MaDanhGia).HasName("PRIMARY");

            entity.ToTable("danhgia");

            entity.HasIndex(e => e.MaNguoiDung, "FK_danhGia_NguoiDung");

            entity.HasIndex(e => e.MaTruyen, "FK_danhGia_Truyen");

            entity.Property(e => e.MaDanhGia).HasColumnName("maDanhGia");
            entity.Property(e => e.DiemDanhGia).HasColumnName("diemDanhGia");
            entity.Property(e => e.MaNguoiDung).HasColumnName("maNguoiDung");
            entity.Property(e => e.MaTruyen).HasColumnName("maTruyen");
            entity.Property(e => e.NgayCapNhap)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngayCapNhap");
            entity.Property(e => e.Ngaytao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngaytao");
            entity.Property(e => e.Noidung)
                .HasColumnType("text")
                .HasColumnName("noidung");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.Danhgia)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_danhGia_NguoiDung");

            entity.HasOne(d => d.MaTruyenNavigation).WithMany(p => p.Danhgia)
                .HasForeignKey(d => d.MaTruyen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_danhGia_Truyen");
        });

        modelBuilder.Entity<Giaodich>(entity =>
        {
            entity.HasKey(e => e.MaGiaoDich).HasName("PRIMARY");

            entity.ToTable("giaodich");

            entity.HasIndex(e => e.MaChuongTruyen, "FK_giaodich_ChuongTruyen");

            entity.HasIndex(e => e.MaNguoiDung, "FK_giaodich_NguoiDung");

            entity.Property(e => e.MaGiaoDich).HasColumnName("maGiaoDich");
            entity.Property(e => e.LoaiGiaoDich).HasColumnName("loaiGiaoDich");
            entity.Property(e => e.LoaiTien).HasColumnName("loaiTien");
            entity.Property(e => e.MaChuongTruyen).HasColumnName("maChuongTruyen");
            entity.Property(e => e.MaNguoiDung).HasColumnName("maNguoiDung");
            entity.Property(e => e.NgayCapNhap)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngayCapNhap");
            entity.Property(e => e.Ngaytao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngaytao");
            entity.Property(e => e.SoTien).HasColumnName("soTien");
            entity.Property(e => e.Trangthai).HasColumnName("trangthai");

            entity.HasOne(d => d.MaChuongTruyenNavigation).WithMany(p => p.Giaodiches)
                .HasForeignKey(d => d.MaChuongTruyen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_giaodich_ChuongTruyen");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.Giaodiches)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_giaodich_NguoiDung");
        });

        modelBuilder.Entity<Lichsudoc>(entity =>
        {
            entity.HasKey(e => new { e.MaChuongTruyen, e.MaNguoiDung })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("lichsudoc");

            entity.HasIndex(e => e.MaNguoiDung, "FK_lichsu_NguoiDung");

            entity.Property(e => e.MaChuongTruyen).HasColumnName("maChuongTruyen");
            entity.Property(e => e.MaNguoiDung).HasColumnName("maNguoiDung");
            entity.Property(e => e.Audio).HasColumnName("audio");
            entity.Property(e => e.NgayCapNhap)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngayCapNhap");
            entity.Property(e => e.Ngaytao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngaytao");
            entity.Property(e => e.TrangthaiDaDoc)
                .HasColumnType("bit(1)")
                .HasColumnName("trangthaiDaDoc");
            entity.Property(e => e.TrangthaiXoa).HasColumnName("trangthaiXoa");

            entity.HasOne(d => d.MaChuongTruyenNavigation).WithMany(p => p.Lichsudocs)
                .HasForeignKey(d => d.MaChuongTruyen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_lichsu_ChuongTruyen");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.Lichsudocs)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_lichsu_NguoiDung");
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(e => new { e.MaNguoiDung, e.LoaiThucTheLike, e.MaThucThe })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("likes");

            entity.Property(e => e.MaNguoiDung).HasColumnName("maNguoiDung");
            entity.Property(e => e.LoaiThucTheLike).HasColumnName("loaiThucTheLike");
            entity.Property(e => e.MaThucThe).HasColumnName("maThucThe");
            entity.Property(e => e.NgayCapNhap)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngayCapNhap");
            entity.Property(e => e.Ngaytao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngaytao");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.Likes)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Like_NguoiDung");
        });

        modelBuilder.Entity<Phanhoi>(entity =>
        {
            entity.HasKey(e => e.MaPhanHoi).HasName("PRIMARY");

            entity.ToTable("phanhoi");

            entity.HasIndex(e => e.MaNguoiDung, "FK_PhanHoi_NguoiDung");

            entity.Property(e => e.MaPhanHoi).HasColumnName("maPhanHoi");
            entity.Property(e => e.MaNguoiDung).HasColumnName("maNguoiDung");
            entity.Property(e => e.NgayCapNhap)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngayCapNhap");
            entity.Property(e => e.Ngaytao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngaytao");
            entity.Property(e => e.NoiDung)
                .HasColumnType("text")
                .HasColumnName("noiDung");
            entity.Property(e => e.Tieude)
                .HasMaxLength(200)
                .HasColumnName("tieude")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.TrangThai).HasColumnName("trangThai");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.Phanhois)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhanHoi_NguoiDung");
        });

        modelBuilder.Entity<Phanhoibinhluan>(entity =>
        {
            entity.HasKey(e => e.MaPhanHoiBinhLuan).HasName("PRIMARY");

            entity.ToTable("phanhoibinhluan");

            entity.HasIndex(e => e.MaBinhLuan, "FK_PhanHoibinhLuan_BinhLuan");

            entity.HasIndex(e => e.MaNguoiDung, "FK_PhanHoibinhLuan_NguoiDung");

            entity.Property(e => e.MaPhanHoiBinhLuan).HasColumnName("maPhanHoiBinhLuan");
            entity.Property(e => e.MaBinhLuan).HasColumnName("maBinhLuan");
            entity.Property(e => e.MaNguoiDung).HasColumnName("maNguoiDung");
            entity.Property(e => e.NgayCapNhap)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngayCapNhap");
            entity.Property(e => e.Ngaytao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngaytao");
            entity.Property(e => e.Noidung)
                .HasColumnType("text")
                .HasColumnName("noidung");

            entity.HasOne(d => d.MaBinhLuanNavigation).WithMany(p => p.Phanhoibinhluans)
                .HasForeignKey(d => d.MaBinhLuan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhanHoibinhLuan_BinhLuan");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.Phanhoibinhluans)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhanHoibinhLuan_NguoiDung");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.MaQuyen).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.MaQuyen).HasColumnName("maQuyen");
            entity.Property(e => e.NgayCapNhap)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngayCapNhap");
            entity.Property(e => e.NgayTao)
                .HasColumnType("datetime")
                .HasColumnName("ngayTao");
            entity.Property(e => e.TenQuyen)
                .HasMaxLength(20)
                .HasColumnName("tenQuyen")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<Theloai>(entity =>
        {
            entity.HasKey(e => e.MaTheLoai).HasName("PRIMARY");

            entity.ToTable("theloai");

            entity.Property(e => e.MaTheLoai).HasColumnName("maTheLoai");
            entity.Property(e => e.MoTa)
                .HasColumnType("mediumtext")
                .HasColumnName("moTa");
            entity.Property(e => e.NgayCapNhap)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngayCapNhap");
            entity.Property(e => e.Ngaytao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngaytao");
            entity.Property(e => e.TenTheLoai)
                .HasMaxLength(30)
                .HasColumnName("tenTheLoai")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Trangthai).HasColumnName("trangthai");
        });

        modelBuilder.Entity<Truyen>(entity =>
        {
            entity.HasKey(e => e.MaTruyen).HasName("PRIMARY");

            entity.ToTable("truyen");

            entity.HasIndex(e => e.MaTheLoai, "FK_Truyen_TheLoai");

            entity.HasIndex(e => e.MaButDanh, "FK_Truyen_maButDanh");

            entity.Property(e => e.MaTruyen).HasColumnName("maTruyen");
            entity.Property(e => e.AnhBia)
                .HasColumnType("text")
                .HasColumnName("anhBia");
            entity.Property(e => e.CongBo).HasColumnName("congBo");
            entity.Property(e => e.MaButDanh).HasColumnName("maButDanh");
            entity.Property(e => e.MaTheLoai).HasColumnName("maTheLoai");
            entity.Property(e => e.MoTa)
                .HasColumnType("mediumtext")
                .HasColumnName("moTa");
            entity.Property(e => e.NgayCapNhap)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngayCapNhap");
            entity.Property(e => e.Ngaytao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngaytao");
            entity.Property(e => e.TacGia)
                .HasMaxLength(100)
                .HasColumnName("tacGia")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.TenTruyen)
                .HasMaxLength(100)
                .HasColumnName("tenTruyen")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.TrangThai).HasColumnName("trangThai");

            entity.HasOne(d => d.MaButDanhNavigation).WithMany(p => p.Truyens)
                .HasForeignKey(d => d.MaButDanh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Truyen_maButDanh");

            entity.HasOne(d => d.MaTheLoaiNavigation).WithMany(p => p.Truyens)
                .HasForeignKey(d => d.MaTheLoai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Truyen_TheLoai");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.MaQuyen, "FK_tbusermaQuyen_maQuyen");

            entity.Property(e => e.MaNguoiDung).HasColumnName("maNguoiDung");
            entity.Property(e => e.AnhDaiDien)
                .HasMaxLength(255)
                .HasColumnName("anhDaiDien");
            entity.Property(e => e.DaXoa).HasColumnName("daXoa");
            entity.Property(e => e.Email)
                .HasMaxLength(40)
                .HasColumnName("email");
            entity.Property(e => e.GioiTinh)
                .HasMaxLength(10)
                .HasColumnName("gioiTinh")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.MaQuyen).HasColumnName("maQuyen");
            entity.Property(e => e.MatKhau)
                .HasMaxLength(30)
                .HasColumnName("matKhau");
            entity.Property(e => e.NgayCapNhap)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngayCapNhap");
            entity.Property(e => e.NgayHetHanVip)
                .HasColumnType("datetime")
                .HasColumnName("ngayHetHanVip");
            entity.Property(e => e.NgaySinh)
                .HasColumnType("datetime")
                .HasColumnName("ngaySinh");
            entity.Property(e => e.Ngaytao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ngaytao");
            entity.Property(e => e.SoChiaKhoa).HasColumnName("soChiaKhoa");
            entity.Property(e => e.SoDeCu).HasColumnName("soDeCu");
            entity.Property(e => e.SoXu).HasColumnName("soXu");
            entity.Property(e => e.TenNguoiDung)
                .HasMaxLength(30)
                .HasColumnName("tenNguoiDung")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.TrangThai).HasColumnName("trangThai");
            entity.Property(e => e.Vip).HasColumnName("vip");

            entity.HasOne(d => d.MaQuyenNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.MaQuyen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbusermaQuyen_maQuyen");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
