﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TesteCacheRedisDecorator.Contexts;

#nullable disable

namespace TesteCacheRedisDecorator.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240601071149_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("TesteCacheRedisDecorator.Models.Endereco", b =>
                {
                    b.Property<int>("IdEndereco")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Bairro")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("IdPessoa")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Logradouro")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Numero")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PessoaIdPessoa")
                        .HasColumnType("INTEGER");

                    b.HasKey("IdEndereco");

                    b.HasIndex("PessoaIdPessoa");

                    b.ToTable("Enderecos");
                });

            modelBuilder.Entity("TesteCacheRedisDecorator.Models.Pessoa", b =>
                {
                    b.Property<int>("IdPessoa")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Idade")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TipoPessoa")
                        .HasColumnType("INTEGER");

                    b.HasKey("IdPessoa");

                    b.ToTable("Pessoas");
                });

            modelBuilder.Entity("TesteCacheRedisDecorator.Models.Endereco", b =>
                {
                    b.HasOne("TesteCacheRedisDecorator.Models.Pessoa", "Pessoa")
                        .WithMany("Enderecos")
                        .HasForeignKey("PessoaIdPessoa")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pessoa");
                });

            modelBuilder.Entity("TesteCacheRedisDecorator.Models.Pessoa", b =>
                {
                    b.Navigation("Enderecos");
                });
#pragma warning restore 612, 618
        }
    }
}
