using DAL.Data;
using DAL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BLL.Handlers.GameHistories
{
    public class ListGameHistories
    {
        public class Query : IRequest<List<GameHistory>> { }

        public class Handler : IRequestHandler<Query, List<GameHistory>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<List<GameHistory>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.GameHistories.ToListAsync(cancellationToken);
            }
        }
    }
}
