using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Zomato.Repository.DataRepository;
using Zomato.Repository.ReviewRepository;
using Zomato.Repository.Test.Bootstrap;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;
using System.Linq.Expressions;
using System.Linq;
using MockQueryable.Moq;

namespace Zomato.Repository.Test.Modules.ReviewTesting
{
    [Collection("Register Dependency")]
    public class ReviewRepositoryTest
    {
        private Mock<IDataRepository> _dataRepositoryMock { get; }
        private IReviewRepository _reviewRepository { get; }

        public ReviewRepositoryTest(Initialize initialize)
        {
            _dataRepositoryMock = initialize.serviceProvider.GetService<Mock<IDataRepository>>();
            _reviewRepository = initialize.serviceProvider.GetService<IReviewRepository>();
        }

        [Fact]
        public async Task AddReview_Verify()
        {
            var review = new Review();
            await _reviewRepository.AddReview(review);
            _dataRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Review>()), Times.Once);
        }

        [Fact]
        public async Task DeleteReview_Verify()
        {
            var givenReviewId = 1;
            var review = new Review();
            _dataRepositoryMock.Setup(x => x.Find<Review>(givenReviewId)).Returns(Task.FromResult(review));
            await _reviewRepository.DeleteReview(givenReviewId);
            _dataRepositoryMock.Verify(x => x.Remove(review), Times.Once);
        }

        [Fact]
        public async Task GetReviewByRestaurantId_IsNotEmpty()
        {
            var givenReviewId = 1;
            var reviewList = new List<Review> {
                new Review
                {
                    ReviewId = 7,
                    ReviewData = "Nice Restaurant",
                    rating = 5,
                    RestaurantId = 2,
                    UserId = "0e1a4290-1f8a-4e70-82b1-ea4137fca300",
                }
            };

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Review, bool>>>())).Returns(reviewList.AsQueryable().BuildMock().Object);

            var reviews = await _reviewRepository.GetReviewByRestaurantId(givenReviewId);
            Assert.NotEmpty(reviews);
        }

        [Fact]
        public async Task GetReviewByUserId_IsNotEmpty()
        {
            var givenUserId = "0e1a4290-1f8a-4e70-82b1-ea4137fca300";
            var reviewList = new List<Review> {
                new Review
                {
                    ReviewId = 7,
                    ReviewData = "Nice Restaurant",
                    rating = 5,
                    RestaurantId = 2,
                    UserId = "0e1a4290-1f8a-4e70-82b1-ea4137fca300",
                }
            };

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Review, bool>>>())).Returns(reviewList.AsQueryable().BuildMock().Object);

            var reviews = await _reviewRepository.GetReviewByUserId(givenUserId);
            Assert.NotEmpty(reviews);
        }
    }
}
