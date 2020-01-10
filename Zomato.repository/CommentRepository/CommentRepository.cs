using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Data;
using Zomato.DomainModel.Models;
using Zomato.Repository.DataRepository;

namespace Zomato.Repository.CommentRepository
{
    public class CommentRepository : ICommentRepository
    {
        private IDataRepository _dataRepository;

        public CommentRepository(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public async Task<Comment> AddComment(Comment comment)
        {
            await _dataRepository.AddAsync(comment);
            return comment;
        }

        public async Task DeleteComment(int reviewId)
        {
            var commentList = await _dataRepository.Where<Comment>(x => x.ReviewId == reviewId).ToListAsync();
            foreach (var each in commentList)
            {
                _dataRepository.Remove(each);
            }
        }

        public async Task<List<Comment>> GetCommentByReviewId(int reviewId)
        {
            var a = await _dataRepository.Where<Comment>(x => x.ReviewId == reviewId).ToListAsync();
            return a;
        }
    }
}
