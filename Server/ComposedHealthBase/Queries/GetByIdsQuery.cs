
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
    public interface IGetByIdsQuery<T, TDto, TContext>
    {
        Task<IEnumerable<TDto>> Handle(List<long> id);
    }

    public class GetByIdsQuery<T, TDto, TContext> : IGetByIdsQuery<T, TDto, TContext>
    where T : BaseEntity<T>
    where TDto : IDto
    where TContext : IDbContext<TContext>
    {
        public IDbContext<TContext> _dbContext { get; }
        public IMapper<T, TDto> _mapper { get; }

        public GetByIdsQuery(IDbContext<TContext> dbContext, IMapper<T, TDto> mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TDto>> Handle(List<long> ids)
        {
            var entities = await _dbContext.Set<T>().Where(x => ids.Contains(x.Id)).ToListAsync();
            return _mapper.Map(entities.AsEnumerable<T>());
        }
    }
}