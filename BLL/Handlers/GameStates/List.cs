using DAL.Data;
using DAL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BLL.Handlers.GameStates
{
    public class List
    {
        public class Query : IRequest<List<GameState>> { }

        public class Handler : IRequestHandler<Query, List<GameState>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<List<GameState>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.GameStates.ToListAsync(cancellationToken);
            }
        }
    }
}
