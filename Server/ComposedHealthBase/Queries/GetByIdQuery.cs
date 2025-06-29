using ComposedHealthBase.Server.Database;
using ComposedHealthBase.Server.Entities;
using Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ComposedHealthBase.Server.Mappers;
using ComposedHealthBase.Shared.DTOs;

namespace ComposedHealthBase.Server.Queries
{
    public interface IGetByIdQuery<T, TDto, TContext>
    {
        Task<TDto> Handle(long id);
    }

    public class GetByIdQuery<T, TDto, TContext> : IGetByIdQuery<T, TDto, TContext>
    where T : BaseEntity<T>
    where TDto : IDto
    where TContext : IDbContext<TContext>
    {
        public IDbContext<TContext> _dbContext { get; }
        public IMapper<T, TDto> _mapper { get; }

        public GetByIdQuery(IDbContext<TContext> dbContext, IMapper<T, TDto> mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<TDto> Handle(long id)
        {
            var entity = await _dbContext.Set<T>().SingleOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity of type {typeof(T).Name} with id {id} not found");
            }
            return _mapper.Map(entity);
        }
    }
}