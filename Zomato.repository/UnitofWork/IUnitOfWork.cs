using System;
using System.Collections.Generic;
using System.Text;
using Zomato.Repository.CategoryRepository;
using Zomato.Repository.CommentRepository;
using Zomato.Repository.CuisineRepository;
using Zomato.Repository.FollowRepository;
using Zomato.Repository.LikeRepository;
using Zomato.Repository.MenuRepository;
using Zomato.Repository.OrderRepository;
using Zomato.Repository.OrderedItemRepository;
using Zomato.Repository.RestaurantLocationRepository;
using Zomato.Repository.RestaurantRepository;
using Zomato.Repository.RestCategoryRepository;
using Zomato.Repository.RestCuisineRepository;
using Zomato.Repository.ReviewRepository;
using Zomato.Repository.UserAddressRepository;
using Zomato.Repository.UserRepository;
using Zomato.Repository.NotificationRepository;

namespace Zomato.Repository.UnitofWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository CategoryRepository { get; }
        ICommentRepository CommentRepository { get; }
        ICuisineRepository CuisineRepository { get; }
        IFollowRepository FollowRepository { get; }
        ILikeRepository LikeRepository { get; }
        IMenuRepository MenuRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderedItemRepository OrderedItemRepository { get; }
        IRestaurantRepository RestaurantRepository { get; }
        IRestLocationRepository RestLocationRepository { get; }
        IRestCategoryRepository RestCategoryRepository { get; }
        IRestCuisineRepository RestCuisineRepository { get; }
        IReviewRepository ReviewRepository { get; }
        IUserRepository UserRepository { get; }
        IUserAddressRepository UserAddressRepository { get; }
        IOrderNotificationRepository OrderNotificationRepository { get; } 
        int commit();
    }
}
