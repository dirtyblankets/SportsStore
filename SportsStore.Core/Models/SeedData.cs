using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FizzWare.NBuilder;
using Faker.Extensions;

namespace SportsStore.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            if (false == context.Products.Any())
            {
                Random random = new Random();

                string[] Categories =
                {
                    "Watersports",
                    "Soccer",
                    "Football",
                    "Baseball",
                    "Tennis",
                    "Basketball",
                    "Ping Pong"
                };

                var products = Builder<Product>.CreateListOfSize(100)
                        .All()
                        .With(c => c.ProductID = 0)//force database to set ProductID
                        .With(c => c.Name = Faker.Lorem.Sentence())
                        .With(c => c.Description = Faker.Lorem.Sentences(1).ToArray()[0])
                        .With(c => c.Category = Categories[Faker.RandomNumber.Next(0, 4)])
                        .With(c => c.Price = random.Next(99, 9999) / 100M)
                        .Build();

                context.Products.AddRange(products);
                context.Products.AddRange(
                    new Product { Name = "Kayak", Description = "A boat for one person", Category = "Watersports", Price = 275 },
                    new Product { Name = "Lifejacket", Description = "Protective and fashionable", Category = "Watersports", Price = 48.95m },
                    new Product { Name = "Soccer Ball", Description = "FIFA-approved size and weight", Category = "Soccer", Price = 19.50m },
                    new Product { Name = "Corner Flags", Description = "Give your playing field a professional touch", Category = "Soccer", Price = 34.95m }
                );

                context.SaveChanges();
            }
        }

    }
}
