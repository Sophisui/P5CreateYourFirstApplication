using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using P5CreateYourFirstApplication.Models;

namespace P5CreateYourFirstApplication.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            //DONNEES USER

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string roleName = "Admin";
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            string adminEmail = "sophie.peigne@hotmail.fr";
            string adminPassword = "VoitureAdmin123?";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, roleName);
                }
                else
                {
                    throw new Exception("Erreur création admin : " +
                                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            //DONNEES VOITURES

            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.VoitureModel.Any())
                {
                    return;
                }

                //1.Marques
                var mazda = new MarqueModel { NomMarque = "Mazda" };
                var jeep = new MarqueModel { NomMarque = "Jeep" };
                var renault = new MarqueModel { NomMarque = "Renault" };
                var ford = new MarqueModel { NomMarque = "Ford" };
                var honda = new MarqueModel { NomMarque = "Honda" };
                var volkswagen = new MarqueModel { NomMarque = "Volkswagen" };

                context.MarqueModel.AddRange(mazda, jeep, renault, ford, honda, volkswagen);
                await context.SaveChangesAsync();

                //2.Modèles
                var miata = new ModeleModel { NomModele = "Miata", IdMarque = mazda.Id };
                var liberty = new ModeleModel { NomModele = "Liberty", IdMarque = jeep.Id };
                var scenic = new ModeleModel { NomModele = "Scénic", IdMarque = renault.Id };
                var explorer = new ModeleModel { NomModele = "Explorer", IdMarque = ford.Id };
                var civic = new ModeleModel { NomModele = "Civic", IdMarque = honda.Id };
                var gti = new ModeleModel { NomModele = "GTI", IdMarque = volkswagen.Id };
                var edge = new ModeleModel { NomModele = "Edge", IdMarque = ford.Id };

                context.ModeleModel.AddRange(miata, liberty, scenic, explorer, civic, gti, edge);
                await context.SaveChangesAsync();

                //3.Finitions
                var le = new FinitionModel { NomFinition = "LE", IdModele = miata.Id };
                var sport = new FinitionModel { NomFinition = "Sport", IdModele = liberty.Id };
                var tce = new FinitionModel { NomFinition = "TCE", IdModele = scenic.Id };
                var xlt = new FinitionModel { NomFinition = "XLT", IdModele = explorer.Id };
                var lx = new FinitionModel { NomFinition = "LX", IdModele = civic.Id };
                var s = new FinitionModel { NomFinition = "S", IdModele = gti.Id };
                var sel = new FinitionModel { NomFinition = "SEL", IdModele = edge.Id };

                context.FinitionModel.AddRange(le, sport, tce, xlt, lx, s, sel);
                await context.SaveChangesAsync();

                //4.Voitures
                var voitures = new List<VoitureModel>
                {
                    new VoitureModel
                    {
                        CodeVin = "VIN001", Annee = 2019, PrixAchat = 1800, PrixVente = 9900,
                        DateAchat = new DateOnly(2022,1,7), DateVente = new DateOnly(2022,4,8),
                        EnReparation = false, Disponible = false, Vendu = true,
                        IdMarque = mazda.Id, IdModele = miata.Id, IdFinition = le.Id,
                        Couleur = "Rouge"
                    },
                    new VoitureModel
                    {
                        CodeVin = "VIN002", Annee = 2007, PrixAchat = 4500, PrixVente = 5350,
                        DateAchat = new DateOnly(2022,4,2), DateVente = new DateOnly(2022,4,9),
                        EnReparation = false, Disponible = false, Vendu = true,
                        IdMarque = jeep.Id, IdModele = liberty.Id, IdFinition = sport.Id,
                        Couleur = "Noir"
                    }
                };

                context.VoitureModel.AddRange(voitures);
                await context.SaveChangesAsync();

                //5.Types de Réparations
                var typeReparations = new List<TypeReparationModel>
                {
                    new TypeReparationModel { NomReparation = "Restauration complète", Description = "Travaux lourds de remise à neuf", CoutTypeReparation = 7600 },
                    new TypeReparationModel { NomReparation = "Roulements des roues avant", Description = "Roulements avant", CoutTypeReparation = 350 },
                    new TypeReparationModel { NomReparation = "Radiateur, freins", Description = "Remplacement radiateur et freins", CoutTypeReparation = 690 },
                    new TypeReparationModel { NomReparation = "Pneus, freins", Description = "Remplacement pneus et freins", CoutTypeReparation = 1100 },
                    new TypeReparationModel { NomReparation = "Climatisation, freins", Description = "Climatisation et freins", CoutTypeReparation = 475 },
                    new TypeReparationModel { NomReparation = "Pneus", Description = "Changement des pneus", CoutTypeReparation = 440 },
                    new TypeReparationModel { NomReparation = "Pneus, freins, climatisation", Description = "Pneus, freins et climatisation", CoutTypeReparation = 950 }
                };

                context.TypeReparationModel.AddRange(typeReparations);
                await context.SaveChangesAsync();

                //6.Réparations
                var reparations = new List<ReparationModel>
                {
                    new ReparationModel { CodeVin = "VIN001", IdVoiture = 1, TypeReparationId = 1, Description = "Restauration complète", CoutReparation = 7600, DateReparation = new DateOnly(2022,4,7), Statut = "Terminé" },
                    new ReparationModel { CodeVin = "VIN002", IdVoiture = 1, TypeReparationId = 1, Description = "Roulements avant", CoutReparation = 350, DateReparation = new DateOnly(2022,4,7), Statut = "Terminé" },
                    new ReparationModel { CodeVin = "VIN003", IdVoiture = 1, TypeReparationId = 1, Description = "Radiateur et freins", CoutReparation = 690, DateReparation = new DateOnly(2022,4,8), Statut = "Terminé" },
                    new ReparationModel { CodeVin = "VIN004", IdVoiture = 1, TypeReparationId = 1, Description = "Pneus et freins", CoutReparation = 1100, DateReparation = new DateOnly(2022,4,9), Statut = "Terminé" },
                    new ReparationModel { CodeVin = "VIN005", IdVoiture = 1, TypeReparationId = 1, Description = "Climatisation et freins", CoutReparation = 475, DateReparation = new DateOnly(2022,4,9), Statut = "Terminé" },
                    new ReparationModel { CodeVin = "VIN006", IdVoiture = 1, TypeReparationId = 1, Description = "Pneus", CoutReparation = 440, DateReparation = new DateOnly(2022,4,10), Statut = "Terminé" },
                    new ReparationModel { CodeVin = "VIN007", IdVoiture = 1, TypeReparationId = 1, Description = "Pneus, freins, climatisation", CoutReparation = 950, DateReparation = new DateOnly(2022,4,11), Statut = "Terminé" }
                };

                context.ReparationModel.AddRange(reparations);
                context.SaveChanges();
            }
        }
    }
}

