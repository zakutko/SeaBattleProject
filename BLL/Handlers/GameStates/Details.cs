using DAL.Data;
using DAL.Models;
using MediatR;

namespace BLL.Handlers.GameStates
{
    public class Details
    {
        public class Query : IRequest<GameState>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, GameState>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<GameState> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.GameStates.FindAsync(request.Id);
            }
        }
    }
}
