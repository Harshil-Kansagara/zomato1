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
    }
}
