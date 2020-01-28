using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zomato.DomainModel.Models;

namespace Zomato.DomainModel.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Cuisine> Cuisine { get; set; }
        public DbSet<Follow> Follow { get; set; }
        public DbSet<Like> Like { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderedItem> OrderedItem { get; set; }
        public DbSet<Restaurant> Restaurant { get; set; }
        public DbSet<RestaurantLocation> RestaurantLocation { get; set; }
        public DbSet<RestCategory> RestCategory { get; set; }
        public DbSet<RestCuisine> RestCuisine { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<UserAddress> UserAddress { get; set; }
        public DbSet<NotificationHub> NotificationHub { get; set; }
        public DbSet<OrderNotificationData> OrderNotificationData { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Cuisine>().HasData(
                new Cuisine() { CuisineId=1, CuisineName = "Chinese" },
                new Cuisine() { CuisineId = 2, CuisineName = "Punjabi" },
                new Cuisine() { CuisineId = 3, CuisineName = "South Indian" },
                new Cuisine() { CuisineId = 4, CuisineName = "Gujarati" },
                new Cuisine() { CuisineId = 5, CuisineName = "Fast Food" },
                new Cuisine() { CuisineId = 6, CuisineName = "Pizza" },
                new Cuisine() { CuisineId = 7, CuisineName = "Juices" },
                new Cuisine() { CuisineId = 8, CuisineName = "Ice Cream" }
                );

            builder.Entity<Category>().HasData(
                new Category() { CategoryId = 1, CategoryName = "Breakfast" },
                new Category() { CategoryId = 2, CategoryName = "Lunch" },
                new Category() { CategoryId = 3, CategoryName = "Dinner" },
                new Category() { CategoryId = 4, CategoryName = "Cafe" },
                new Category() { CategoryId = 5, CategoryName = "Dessert" }
                );

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Name=  "admin" },
                new IdentityRole() { Name="user" }
                );
        }
    }
}
