using AirStore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AirStore.Models
{
    public class SeedData
    {
        private static void InitializeAir(ApplicationDbContext context)
        {
            context.Air.AddRange(
                        new Air
                        {
                            Name = "Alp Air",
                            Type = AirItemType.Continental,
                            Price = 14.99M
                        },
                        new Air
                        {
                            Name = "African Air",
                            Type = AirItemType.Tropical,
                            Price = 13.99M
                        },
                        new Air
                        {
                            Name = "Arctic Air",
                            Type = AirItemType.Arctic,
                            Price = 20.99M
                        },
                        new Air
                        {
                            Name = "Ocean Air",
                            Type = AirItemType.Maritime,
                            Price = 17.99M
                        },
                        new Air
                        {
                            Name = "Polar Bear Air",
                            Type = AirItemType.Polar,
                            Price = 14.99M
                        },
                        new Air
                        {
                            Name = "Ukrainian ATO Air",
                            Type = AirItemType.Continental,
                            Price = 4.99M
                        },
                        new Air
                        {
                            Name = "Varna Sea Air",
                            Type = AirItemType.Maritime,
                            Price = 2.99M
                        }
                    );
            context.SaveChanges();
        }

        private static void InitializeComment(ApplicationDbContext context, ApplicationUser user)
        {
            context.Comment.AddRange(
                        new Comment
                        {
                            Text = "Great web-site 1",
                            UserId = user.Id
                        },
                        new Comment
                        {
                            Text = "Great web-site 2",
                            UserId = user.Id
                        },
                        new Comment
                        {
                            Text = "Great web-site 3",
                            UserId = user.Id
                        },
                        new Comment
                        {
                            Text = "Great web-site 4",
                            UserId = user.Id
                        },
                        new Comment
                        {
                            Text = "Great web-site 5",
                            UserId = user.Id
                        },
                        new Comment
                        {
                            Text = "Great web-site 6",
                            UserId = user.Id
                        },
                        new Comment
                        {
                            Text = "Great web-site 7",
                            UserId = user.Id
                        },
                        new Comment
                        {
                            Text = "Great web-site 8",
                            UserId = user.Id
                        },
                        new Comment
                        {
                            Text = "Great web-site 9",
                            UserId = user.Id
                        },
                        new Comment
                        {
                            Text = "Great web-site 10",
                            UserId = user.Id
                        },
                        new Comment
                        {
                            Text = "Great web-site 11",
                            UserId = user.Id
                        },
                        new Comment
                        {
                            Text = "Great web-site 12",
                            UserId = user.Id
                        }
                    );
            context.SaveChanges();
        }

        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);

                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var _user = await userManager.FindByEmailAsync("admin@email.com");

            if (_user == null)
            {
                var poweruser = new ApplicationUser
                {
                    FirstName = "Admin",
                    LastName = "Kirk",
                    UserName = "admin@email.com",
                    Email = "admin@email.com",
                };
                string adminPassword = "admin";

                var createPowerUser = await userManager.CreateAsync(poweruser, adminPassword);
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(poweruser, "Admin");

                }
            }

            ApplicationUser _regularUser = await userManager.FindByEmailAsync("regular@email.com");
            if (_regularUser == null)
            {
                var regularuser = new ApplicationUser
                {
                    FirstName = "Regular",
                    LastName = "Kirk",
                    UserName = "regular@email.com",
                    Email = "regular@email.com",
                };
                string regularPassword = "regular";

                await userManager.CreateAsync(regularuser, regularPassword);
            }

            ApplicationUser regularUser = await userManager.FindByEmailAsync("regular@email.com");

            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (!context.Air.Any())
                {
                    InitializeAir(context);
                }
                if (!context.Comment.Any())
                {
                    InitializeComment(context, regularUser);
                }
            }
        }
    }
}