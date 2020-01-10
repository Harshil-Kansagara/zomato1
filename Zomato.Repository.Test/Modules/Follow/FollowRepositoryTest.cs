using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Zomato.Repository.DataRepository;
using Zomato.Repository.FollowRepository;
using Zomato.Repository.Test.Bootstrap;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;
using System.Linq.Expressions;
using System.Linq;
using MockQueryable.Moq;

namespace Zomato.Repository.Test.Modules.FollowTesting
{
    [Collection("Register Dependency")]
    public class FollowRepositoryTest
    {
        private Mock<IDataRepository> _dataRepositoryMock { get; }
        private IFollowRepository _followRepository { get; }

        public FollowRepositoryTest(Initialize initialize)
        {
            _dataRepositoryMock = initialize.serviceProvider.GetService<Mock<IDataRepository>>();
            _followRepository = initialize.serviceProvider.GetService<IFollowRepository>();
        }

        [Fact]
        public async Task GetFollowingByUserId_IsNotNull()
        {
            var givenUserId = "cd3e4e2a-c8da-423e-a60d-53c616b41f90";
            var followingList = new List<Follow> {
                new Follow
                {
                    Id = 1
                }
            };

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Follow, bool>>>())).Returns(followingList.AsQueryable().BuildMock().Object);

            followingList = await _followRepository.GetFollowingByUserId(givenUserId);

            Assert.NotNull(followingList);
        }

        [Fact]
        public async Task GetFollowingByUserId_IsEmpty()
        {
            var givenUserId = "1";
            var followingList = new List<Follow>();

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Follow, bool>>>())).Returns(followingList.AsQueryable().BuildMock().Object);

            followingList = await _followRepository.GetFollowingByUserId(givenUserId);

            Assert.Empty(followingList);
        }

        [Fact]
        public async Task GetFollowerByUserId_IsNotNull()
        {
            var givenUserId = "0e1a4290-1f8a-4e70-82b1-ea4137fca300";
            var followingList = new List<Follow>{
                new Follow
                {
                    Id = 1
                }
            };

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Follow, bool>>>())).Returns(followingList.AsQueryable().BuildMock().Object);

            followingList = await _followRepository.GetFollowerByUserId(givenUserId);

            Assert.NotNull(followingList);
        }

        [Fact]
        public async Task GetFollowerByUserId_IsEmpty()
        {
            var givenUserId = "1";
            var followerList = new List<Follow>();

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Follow, bool>>>())).Returns(followerList.AsQueryable().BuildMock().Object);

            followerList = await _followRepository.GetFollowerByUserId(givenUserId);

            Assert.Empty(followerList);
        }

        [Fact]
        public async Task GetFollowList_IsNotNull()
        {
            var followList = new List<Follow>();

            _dataRepositoryMock.Setup(x => x.Get<Follow>()).Returns(Task.FromResult(followList));

            followList = await _followRepository.GetFollowList();

            Assert.NotNull(followList);
        }

        [Fact]
        public async Task AddFollow_Test()
        {
            var follow = new Follow();
            await _followRepository.AddFollower(follow);
            _dataRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Follow>()), Times.Once);
        }

        [Fact]
        public async Task DeleteFollow_Test()
        {
            var givenFollowerId = "1";
            var followList = new List<Follow>();
            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Follow, bool>>>())).Returns(followList.AsQueryable().BuildMock().Object);

            await _followRepository.RemoveFollower(givenFollowerId);
            _dataRepositoryMock.Verify(x => x.Remove(It.IsAny<Follow>()), Times.Exactly(followList.Count));
        }
    }
}
