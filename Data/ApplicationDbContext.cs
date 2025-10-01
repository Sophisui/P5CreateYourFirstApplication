using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using P5CreateYourFirstApplication.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace P5CreateYourFirstApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<FinitionModel> FinitionModel { get; set; }
        public DbSet<MarqueModel> MarqueModel { get; set; } 
        public DbSet<ModeleModel> ModeleModel { get; set; }
        public DbSet<VoitureModel> VoitureModel { get; set; }
        public DbSet<ReparationModel> ReparationModel { get; set; }
        public DbSet<TypeReparationModel> TypeReparationModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //VOITURE
            modelBuilder.Entity<VoitureModel>()
                .HasOne(v => v.Marque) //Chaque voiture a une marque
                .WithMany(m => m.Voitures) //Chaque marque a une voiture (relation un-à-un)
                .HasForeignKey(v => v.IdMarque) //La clé étrangère dans VoitureModel est IdMarque
                .OnDelete(DeleteBehavior.Restrict); //Empêche la suppression en cascade

            modelBuilder.Entity<VoitureModel>()
                .HasOne(v => v.Modele) //Chaque voiture a un modèle
                .WithMany(m => m.Voitures) //Chaque modèle peut avoir plusieurs voitures
                .HasForeignKey(v => v.IdModele) //La clé étrangère dans VoitureModel est IdModele
                .OnDelete(DeleteBehavior.Restrict); //Empêche la suppression en cascade

            modelBuilder.Entity<VoitureModel>()
                .HasOne(v => v.Finition) //Chaque voiture a une finition
                .WithMany(f => f.Voitures) //Chaque finition peut avoir plusieurs voitures
                .HasForeignKey(v => v.IdFinition) //La clé étrangère dans VoitureModel est IdFinition
                .OnDelete(DeleteBehavior.Restrict); //Empêche la suppression en cascade

            modelBuilder.Entity<VoitureModel>()
                .HasMany(v => v.Reparations) //Chaque voiture peut avoir plusieurs réparations
                .WithOne(r => r.Voiture) //Chaque réparation est liée à une voiture
                .HasForeignKey(r => r.IdVoiture) //La clé étrangère dans ReparationModel est CodeVin (IdVoiture????)
                .OnDelete(DeleteBehavior.Cascade); //Supprime les réparations si la voiture est supprimée

            //REPARATION
            modelBuilder.Entity<ReparationModel>()
                .HasOne(r => r.Voiture) //Chaque réparation est liée à une voiture
                .WithMany(v => v.Reparations) //Chaque voiture peut avoir plusieurs réparations
                .OnDelete(DeleteBehavior.Cascade); //Supprime les réparations si la voiture est supprimée
        }
    };
}
