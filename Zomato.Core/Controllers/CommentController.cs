using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;
using Zomato.Repository.UnitofWork;

namespace Zomato.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;

        public CommentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{reviewId}/comment")]
        public async Task<List<CommentCollection>> GetComment(int reviewId)
        {
            List<CommentCollection> commentCollection = new List<CommentCollection>();
            var commentList = await _unitOfWork.CommentRepository.GetCommentByReviewId(reviewId);
            foreach (var comment in commentList)
            {
                var model = new CommentCollection();
                model.CommentId = comment.CommentId;
                model.CommentData = comment.CommentData;
                model.UserName = _unitOfWork.UserRepository.GetUsernameByUserId(comment.UserId).Result.UserName;
                commentCollection.Add(model);
            }
            return commentCollection;
        }

        [HttpPost]
        [Route("comment")]
        public async Task<IActionResult> AddComment(int reviewId,Comment comment)
        {
            if (ModelState.IsValid) { 
                var a = await _unitOfWork.CommentRepository.AddComment(comment);
                _unitOfWork.commit();
                return Ok(a);
            }
            return BadRequest();
        }

    }
}
