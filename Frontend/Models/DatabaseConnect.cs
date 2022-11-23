﻿using Microsoft.EntityFrameworkCore;

namespace Testapplication1.Database;

public record Rangers(Guid RangerID, string RangerName, string Login, string Password, int PhoneNumber, string Email, bool IsAdmin) { 
    public List<ConnectionTable> connectionTables { get; set; } = null!;
}

public record Notification(Guid ID, DateTime Time, int NodeID, float Latitude, float Longitude, string Sound_Type, int Probability, string Sound){
    public List<ConnectionTable> connectionTables { get; set; } = null!;
}

public record ConnectionTable(Guid ID, Guid RangersID, Guid NotificationID)
{
    public Rangers Ranger { get; set; } = null!;
    public Notification Notif { get; set; } = null!;

}

public class DatabaseConnect : DbContext
{
    
    public DbSet<Notification> Notifs { get; set; } = null!;
    public DbSet<Rangers> Ranger { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notification>()
            .HasKey(_=>_.ID);
            
        modelBuilder.Entity<Rangers>()
            .HasKey(_=>_.RangerID);

        modelBuilder.Entity<ConnectionTable>()
            .HasOne(_ => _.Ranger)
            .WithMany(_ => _.connectionTables)
            .HasForeignKey(_ => _.RangersID);

        modelBuilder.Entity<ConnectionTable>()
            .HasOne(_ => _.Notif)
            .WithMany(_ => _.connectionTables)
            .HasForeignKey(_ => _.NotificationID);
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder)
    {
        optionbuilder
        .UseNpgsql(@$"Host=localhost:5432;Username=postgres;Password={GetEnvironmentVar()};Database=Chengeta"); // System.Environment.GetEnvironmentVariable
    }

    private static string GetEnvironmentVar()
    {
        var value = Environment.GetEnvironmentVariable("PW_SQL");
            Console.WriteLine(value);
        if (value != null)
            
            return value;
        else
            return "";
    }
}