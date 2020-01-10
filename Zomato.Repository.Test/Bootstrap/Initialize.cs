using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Zomato.DomainModel.Models;
using Zomato.Repository.CategoryRepository;
using Zomato.Repository.CommentRepository;
using Zomato.Repository.CuisineRepository;
using Zomato.Repository.DataRepository;
using Zomato.Repository.FollowRepository;
using Zomato.Repository.LikeRepository;
using Zomato.Repository.MenuRepository;
using Zomato.Repository.NotificationRepository;
using Zomato.Repository.OrderedItemRepository;
using Zomato.Repository.OrderRepository;
using Zomato.Repository.RestaurantLocationRepository;
using Zomato.Repository.RestaurantRepository;
using Zomato.Repository.RestCategoryRepository;
using Zomato.Repository.RestCuisineRepository;
using Zomato.Repository.ReviewRepository;
using Zomato.Repository.UnitofWork;
using Zomato.Repository.UserAddressRepository;
using Zomato.Repository.UserRepository;

namespace Zomato.Repository.Test.Bootstrap
{
    public class Initialize
    {
        public IServiceProvider serviceProvider;

        public Initialize()
        {
            var services = new ServiceCollection();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICategoryRepository, CategoryRepository.CategoryRepository>();
            services.AddScoped<ICommentRepository, CommentRepository.CommentRepository>();
            services.AddScoped<ICuisineRepository, CuisineRepository.CuisineRepository>();
            services.AddScoped<IFollowRepository, FollowRepository.FollowRepository>();
            services.AddScoped<ILikeRepository, LikeRepository.LikeRepository>();
            services.AddScoped<IMenuRepository, MenuRepository.MenuRepository>();
            services.AddScoped<IOrderedItemRepository, OrderedItemRepository.OrderedItemRepository>();
            services.AddScoped<IOrderRepository, OrderRepository.OrderRepository>();
            services.AddScoped<IRestLocationRepository, RestLocationRepository>();
            services.AddScoped<IRestaurantRepository, RestaurantRepository.RestaurantRepository>();
            services.AddScoped<IRestCategoryRepository, RestCategoryRepository.RestCategoryRepository>();
            services.AddScoped<IRestCuisineRepository, RestCuisineRepository.RestCuisineRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository.ReviewRepository>();
            services.AddScoped<IUserAddressRepository, UserAddressRepository.UserAddressRepository>();
            services.AddScoped<IUserRepository, UserRepository.UserRepository>();
            services.AddScoped<IOrderNotificationRepository, OrderNotificationRepository>();
            services.AddScoped<IDataRepository, DataRepository.DataRepository>();

            var userStoreMock = new Mock<IUserStore<IdentityUser>>();
            services.AddScoped(x => userStoreMock);
            services.AddScoped(x => userStoreMock.Object);

            var userManagerMock = new Mock<UserManager<IdentityUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            services.AddScoped(x => userManagerMock);
            services.AddScoped(x => userManagerMock.Object);

            var signInManagerMock = new Mock<SignInManager<IdentityUser>>(userManagerMock.Object,
                 new Mock<IHttpContextAccessor>().Object,
                  new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<ILogger<SignInManager<IdentityUser>>>().Object,
                  new Mock<IAuthenticationSchemeProvider>().Object
                 );
            services.AddScoped(x => signInManagerMock);
            services.AddScoped(x => signInManagerMock.Object);

            var dataRepositoryMock = new Mock<IDataRepository>();
            services.AddSingleton(x => dataRepositoryMock);
            services.AddSingleton(x => dataRepositoryMock.Object);

            serviceProvider = services.BuildServiceProvider();

        }
    }
}
