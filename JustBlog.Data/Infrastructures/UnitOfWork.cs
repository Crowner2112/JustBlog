using JustBlog.Data.IRepositories;
using JustBlog.Data.Repositories;
using System.Threading.Tasks;

namespace JustBlog.Data.Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly JustBlogDbContext context;
        private ICategoryRepository categoryRepository;
        private IPostRepository postRepository;
        private ITagRepository tagRepository;
        private IPostTagMapRepository postTagMapRepository;
        private ICommentRepository commentRepository;
        private IPostUserRateMapRepository postUserRateMapRepository;

        public UnitOfWork(JustBlogDbContext context)
        {
            this.context = context;
        }

        public ICategoryRepository CategoryRepository => this.categoryRepository ??= new CategoryRepository(this.context);

        public IPostTagMapRepository PostTagMapRepository => this.postTagMapRepository ??= new PostTagMapRepository(this.context);

        public IPostRepository PostRepository => this.postRepository ??= new PostRepository(this.context);

        public ITagRepository TagRepository => this.tagRepository ??= new TagRepository(this.context);

        public ICommentRepository CommentRepository => this.commentRepository ??= new CommentRepository(this.context);

        public JustBlogDbContext JustBlogDbContext => this.context;

        public IPostUserRateMapRepository PostUserRateMapRepository => this.postUserRateMapRepository ??= new PostUserRateMapRepository(this.context);

        public void Dispose()
        {
            this.context.Dispose();
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await this.context.SaveChangesAsync();
        }
    }
}