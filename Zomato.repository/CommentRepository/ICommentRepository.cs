using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;

namespace Zomato.Repository.CommentRepository
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetCommentByReviewId(int reviewId);
        Task<Comment> AddComment(Comment comment);
        Task DeleteComment(int reviewId);
    }
}
