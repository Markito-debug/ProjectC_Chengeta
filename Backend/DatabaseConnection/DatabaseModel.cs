using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Mqttlistener
{
    // This records are like Classes used for setting up a DB format
    public record Rangers(Guid RangerID,string RangerName, string Username, string Password, int PhoneNumber, string Email, bool IsAdmin)
    {
        public bool LoggedIn { get; init; }
        public List<ConnectionTable> connectionTables { get; set; } = null!;
    }
    public record Notification(Guid ID, DateTime Time, int NodeID, float Latitude, float Longitude, string Sound_Type, int Probability, string Sound){
        public string? Status { get; set; }
        public string? Notes { get; set; }
        public List<ConnectionTable> connectionTables { get; set; } = null!;
    }

    public record ConnectionTable(Guid ID, Guid RangersID, Guid NotificationID)
    {
        public Rangers Ranger { get; set; } = null!;
        public Notification Notif { get; set; } = null!;

    }

    /// <summary>
    /// Moved Listener class from ListenerDataJSON and deleted the file
    /// Listener class is only needed when you want to add former data from JSON to the DB
    /// </summary>
    public class Listener
    {
        public int ID { get; set; }
        public string Time { get; set; } = null!;
        public int NodeID { get; set; } //int
        public float Latitude { get; set; } //float
        public float Longitude { get; set; } //float
        public string Sound_type { get; set; } = null!;
        public int Probability { get; set; } //int
        public string Sound { get; set; } = null!;
    }


    public class ListenerDb : DbContext
    {
        public ListenerDb() { }

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
            .UseNpgsql(@$"Host=localhost:5432;Username=postgres;Password={GetEnvironmentVar()};Database=Chengeta");
        }

        private static string GetEnvironmentVar()
        {
            var value = Environment.GetEnvironmentVariable("PW_SQL");
            if (value != null)

                return value;
            else
                return "";
        }
    }
}