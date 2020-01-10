using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Zomato.Repository.CommentRepository;
using Zomato.Repository.DataRepository;
using Zomato.Repository.Test.Bootstrap;
using Zomato.DomainModel.Models;
using System.Linq.Expressions;
using System.Linq;
using MockQueryable.Moq;
using System.Threading.Tasks;

namespace Zomato.Repository.Test.Modules.CommentTesting
{
    [Collection("Register Dependency")]
    public class CommentRepositoryTest
    {
        private Mock<IDataRepository> _dataRepositoryMock { get; }
        private ICommentRepository _commentRepository { get; }

        public CommentRepositoryTest(Initialize initialize)
        {
            _dataRepositoryMock = initialize.serviceProvider.GetService<Mock<IDataRepository>>();
            _commentRepository = initialize.serviceProvider.GetService<ICommentRepository>();
        }

        [Fact]
        public async Task GetCommentByReviewId_IsNotNull()
        {
            var givenReviewId = 1;
            var commentList = new List<Comment> {
                new Comment
                {
                    CommentId = 1
                }
            };

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Comment, bool>>>())).Returns(commentList.AsQueryable().BuildMock().Object);

            commentList = await _commentRepository.GetCommentByReviewId(givenReviewId);

            Assert.NotNull(commentList);
           
        }

        [Fact]
        public async Task GetCommentByReviewId_IsEmpty()
        {
            var givenReviewId = 0;
            var commentList = new List<Comment>();

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Comment, bool>>>())).Returns(commentList.AsQueryable().BuildMock().Object);

            commentList = await _commentRepository.GetCommentByReviewId(givenReviewId);

            Assert.Empty(commentList);
        }

        [Fact]
        public async Task AddComment()
        {
            var comment = new Comment();
            await _commentRepository.AddComment(comment);
            _dataRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Comment>()), Times.Once);

        }

        [Fact]
        public async Task DeleteComment()
        {
            var givenReviewId = 1;
            var commentList = new List<Comment>();
            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<Comment, bool>>>())).Returns(commentList.AsQueryable().BuildMock().Object);

            await _commentRepository.DeleteComment(givenReviewId);
            _dataRepositoryMock.Verify(x => x.Remove(It.IsAny<Comment>()), Times.Exactly(commentList.Count));
            
        }
    }
}
