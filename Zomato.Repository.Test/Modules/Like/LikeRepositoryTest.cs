using Moq;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Zomato.Repository.DataRepository;
using Zomato.Repository.LikeRepository;
using Zomato.Repository.Test.Bootstrap;
using Xunit;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;
using System.Linq.Expressions;
using System.Linq;
using MockQueryable.Moq;

namespace Zomato.Repository.Test.Modules.LikeTesting
{
    [Collection("Register Dependency")]
    public class LikeRepositoryTest
    {
        private Mock<IDataRepository> _dataRepositoryMock { get; }
        private ILikeRepository _likeRepository { get; }

        public LikeRepositoryTest(Initialize initialize)
        {
            _dataRepositoryMock = initialize.serviceProvider.GetService<Mock<IDataRepository>>();
            _likeRepository = initialize.serviceProvider.GetService<ILikeRepository>();
        }

        [Fact]
        public async Task GetLikeByReviewId_IsNotNull()
        {
            var givenReviewId = 1;
            var likeList = new List<Like>
            {
                new Like
                {
                    LikeId = 1
                }
            };

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Like, bool>>>())).Returns(likeList.AsQueryable().BuildMock().Object);

            likeList = await _likeRepository.GetLikeByReviewId(givenReviewId);

            Assert.NotNull(likeList);
        }

        [Fact]
        public async Task GetLikeByReviewId_IsEmpty()
        {
            var givenReviewId = 0;
            var likeList = new List<Like>();

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Like, bool>>>())).Returns(likeList.AsQueryable().BuildMock().Object);

            likeList = await _likeRepository.GetLikeByReviewId(givenReviewId);

            Assert.Empty(likeList);
        }

        [Fact]
        public async Task DeleteLikeByReview_Test()
        {
            var givenReviewId = 1;
            var likeList = new List<Like>();

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Like, bool>>>())).Returns(likeList.AsQueryable().BuildMock().Object);

            await _likeRepository.DeleteLikeByReview(givenReviewId);

            _dataRepositoryMock.Verify(x => x.Remove(likeList), Times.Exactly(likeList.Count));
        }

        [Fact]
        public async Task DeleteLike_Test()
        {
            var givenLikeId = 1;
            var like = new Like();
            _dataRepositoryMock.Setup(x => x.Find<Like>(givenLikeId)).Returns(Task.FromResult(like));
            await _likeRepository.DeleteLike(givenLikeId);
            _dataRepositoryMock.Verify(x => x.Remove(It.IsAny<Like>()), Times.Once);
        }

        [Fact]
        public async Task AddLike_Test()
        {
            var like = new Like();
            await _likeRepository.AddLike(like);
            _dataRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Like>()), Times.Once);
        }
    }
}
