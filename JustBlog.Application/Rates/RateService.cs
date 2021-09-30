using JustBlog.Data.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustBlog.Application.Rates
{
    public class RateService : IRateService
    {
        private readonly IUnitOfWork unitOfWork;

        public RateService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public bool ChangeRate(int userId, int postId)
        {
            var isVoted = unitOfWork.PostUserRateMapRepository.IsVotedByUserIdAndPostId(userId, postId);
            if (isVoted)
                unitOfWork.PostUserRateMapRepository.DownVote(userId, postId);
            else
                unitOfWork.PostUserRateMapRepository.AddVote(userId, postId);
            unitOfWork.SaveChanges();
            return true;
        }
    }
}
