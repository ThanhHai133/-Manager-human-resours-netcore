using MHR_EF_Code.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace MHR_EF_Code.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<AppUser> userManager)
        {
            try
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roleNames = { "Admin", "Manager", "Employee" };
                IdentityResult roleResult;

                foreach (var roleName in roleNames)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                        Console.WriteLine($"Role '{roleName}' created successfully.");
                    }
                }

                var adminUser = new AppUser { UserName = "admin1", Email = "admin@1.com", Name = "Admin User", Address = "Admin Address" };
                string adminPassword = "Admin@123";
                var user = await userManager.FindByEmailAsync(adminUser.Email);
                Console.WriteLine($"Finding user by email: {adminUser.Email}");

                if (user == null)
                {
                    var createPowerUser = await userManager.CreateAsync(adminUser, adminPassword);
                    if (createPowerUser.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                        Console.WriteLine("Admin user created successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to create admin user.");
                        foreach (var error in createPowerUser.Errors)
                        {
                            Console.WriteLine($"Error: {error.Description}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Admin user already exists.");
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"DbUpdateException: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }
        }
    }
}
