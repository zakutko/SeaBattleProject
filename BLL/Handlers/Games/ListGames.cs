using DAL.Data;
using DAL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BLL.Handlers.Games
{
    public class ListGames
    {
        public class Query : IRequest<List<Game>> { }

        public class Handler : IRequestHandler<Query, List<Game>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<List<Game>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Games.ToListAsync(cancellationToken);
            }
        }
    }
}
