using DAL.Data;
using DAL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BLL.Handlers.PlayerGames
{
    public class List
    {
        public class Query : IRequest<List<PlayerGame>> { }

        public class Handler : IRequestHandler<Query, List<PlayerGame>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<List<PlayerGame>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.PlayerGames.ToListAsync(cancellationToken);
            }
        }
    }
}
