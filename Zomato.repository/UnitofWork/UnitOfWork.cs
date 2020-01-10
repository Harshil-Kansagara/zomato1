using System;
using System.Collections.Generic;
using System.Text;
using Zomato.DomainModel.Data;
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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ICategoryRepository CategoryRepository { get; private set; }
        public ICommentRepository CommentRepository { get; private set; }
        public ICuisineRepository CuisineRepository { get; private set; }
        public IFollowRepository FollowRepository { get; private set; }
        public ILikeRepository LikeRepository { get; private set; }
        public IMenuRepository MenuRepository { get; private set; }
        public IOrderRepository OrderRepository { get; private set; }
        public IOrderedItemRepository OrderedItemRepository { get; private set; }
        public IRestaurantRepository RestaurantRepository { get; private set; }
        public IRestCategoryRepository RestCategoryRepository { get; private set; }
        public IRestCuisineRepository RestCuisineRepository { get; private set; }
        public IRestLocationRepository RestLocationRepository { get; private set; }
        public IReviewRepository ReviewRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; } 
        public IUserAddressRepository UserAddressRepository { get; private set; }
        public IOrderNotificationRepository OrderNotificationRepository { get; private set; }

        //public ICategoryRepository _CategoryRepository => throw new NotImplementedException();

        //public ICommentRepository _CommentRepository => throw new NotImplementedException();

        //public ICuisineRepository _CuisineRepository => throw new NotImplementedException();

        //public IFollowRepository _FollowRepository => throw new NotImplementedException();

        //public ILikeRepository _LikeRepository => throw new NotImplementedException();

        //public IMenuRepository _MenuRepository => throw new NotImplementedException();

        //public IOrderRepository _OrderRepository => throw new NotImplementedException();

        //public IOrderedItemRepository _OrderedItemRepository => throw new NotImplementedException();

        //public IRestaurantRepository _RestaurantRepository => throw new NotImplementedException();

        //public IRestCategoryRepository _RestCategoryRepository => throw new NotImplementedException();

        //public IRestCuisineRepository _RestCuisineRepository => throw new NotImplementedException();

        //public IReviewRepository _ReviewRepository => throw new NotImplementedException();

        //public IUserAddressRepository _UserAddressRepository => throw new NotImplementedException();

        public UnitOfWork(ApplicationDbContext applicationDbContext, ICategoryRepository categoryRepository, ICommentRepository commentRepository, ICuisineRepository cuisineRepository, IFollowRepository followRepository, ILikeRepository likeRepository, IMenuRepository menuRepository, IOrderRepository orderRepository, IOrderedItemRepository orderedItemRepository, IRestaurantRepository restaurantRepository, IRestLocationRepository restLocationRepository, IRestCuisineRepository restCuisineRepository, IRestCategoryRepository restCategoryRepository, IReviewRepository reviewRepository,IUserRepository userRepository, IUserAddressRepository userAddressRepository, IOrderNotificationRepository orderNotificationRepository)
        {
            _applicationDbContext = applicationDbContext;
            CategoryRepository = categoryRepository;
            CommentRepository = commentRepository;
            CuisineRepository = cuisineRepository;
            FollowRepository = followRepository;
            LikeRepository = likeRepository;
            MenuRepository = menuRepository;
            OrderRepository = orderRepository;
            OrderedItemRepository = orderedItemRepository;
            RestaurantRepository = restaurantRepository;
            RestLocationRepository = restLocationRepository;
            RestCuisineRepository = restCuisineRepository;
            RestCategoryRepository = restCategoryRepository;
            ReviewRepository = reviewRepository;
            UserRepository = userRepository;
            UserAddressRepository = userAddressRepository;
            OrderNotificationRepository = orderNotificationRepository;
        }

        public int commit()
        {
            return _applicationDbContext.SaveChanges();
        }

        public void Dispose()
        {
            _applicationDbContext.Dispose();
        }
    }
}
