using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using Core.Common;
using Core.Entity;
using Domain.Model.BlogPosts;
using Repository.BlogPostRepository;
using Repository.UserInfoRepository;

namespace Service.BlogPostService
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IUserInfoRepository _userInfoRepository;

        public BlogPostService(IBlogPostRepository blogPostRepository, IUserInfoRepository userInfoRepository)
        {
            _blogPostRepository = blogPostRepository;
            _userInfoRepository = userInfoRepository;
        }


        public Result<IList<BlogPost>> GetActivePosts()
        {
            return Result<IList<BlogPost>>.AsSuccess(_blogPostRepository.GetActivePosts());
        }

        public Result<IList<BlogPost>> GetActivePosts(Guid? id)
        {
            return Result<IList<BlogPost>>.AsSuccess(_blogPostRepository.GetActivePosts(id));
        }

        public Result<IList<BlogPost>> GetNotActivePosts()
        {
            return Result<IList<BlogPost>>.AsSuccess(_blogPostRepository.GetNotActivePosts());
        }
        public Result<IList<BlogPost>> GetNotActivePosts(Guid? id)
        {
            return Result<IList<BlogPost>>.AsSuccess(_blogPostRepository.GetNotActivePosts(id));
        }
        public Result<IList<BlogPost>> GetPosts(string userName)
        {
            return Result<IList<BlogPost>>.AsSuccess(_blogPostRepository.GetPosts(userName));
        }
        public Result<IList<BlogPost>> GetPosts(Guid? id, string userName)
        {
            return Result<IList<BlogPost>>.AsSuccess(_blogPostRepository.GetPosts(id,userName));
        }
        public Result<IList<BlogPost>> GetPosts(Guid? id)
        {
            return Result<IList<BlogPost>>.AsSuccess(_blogPostRepository.GetPosts(id));
        }
        public Result<IList<BlogPost>> GetActivePostsAndOrderBy()
        {
            return Result<IList<BlogPost>>.AsSuccess(_blogPostRepository.GetActivePostsAndOrderBy());
        }
        public Result AddPost(BlogPost model, string userName)
        {
            var user = _userInfoRepository.GetSingleUser(userName);

            var post = new BlogPost
            {
                Active = "False",
                Contents = model.Contents,
                Date = DateTime.Now.ToString(),
                Title = model.Title
            };

            _blogPostRepository.Insert(post);

            //var statistic = new Statistics
            //{
            //    BlogPost = post
            //};

            //_statisticRepository.Insert(statistic);
            
            user.Posts.Add(post);
          // ??  _userInfoRepository.Update(user);

            return Result.AsSuccess();
        }
        public Result UpdatePost(Guid id, BlogPost model, string userName)
        {
            var user = _userInfoRepository.GetSingleUser(userName);
            var post = _blogPostRepository.Get(id);

            post.Id = model.Id;
            post.Active = "False";
            post.Contents = model.Contents;
            post.Date = DateTime.Now.ToString();
            post.Title = model.Title;

            _blogPostRepository.Update(post);

            user.Posts.Add(post);
            // ??  _userInfoRepository.Update(user);

            return Result.AsSuccess();
        }
        public Result ChangePostActivate(Guid id)
        {
            var post = _blogPostRepository.Get(id);

            post.Active = post.Active == "True" ? "False" : "True";

            _blogPostRepository.Update(post);

            //TODO: yukarıda update yaparken user de güncelelnirken burda neden güncellenmiyor bir araştır.

            return Result.AsSuccess();
        }

        public Result DeletePost(Guid id)
        {
            var post = _blogPostRepository.Get(id);

            _blogPostRepository.Delete(post);

            return Result.AsSuccess();
        }
    }
}
