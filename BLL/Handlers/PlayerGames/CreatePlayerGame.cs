using DAL.Data;
using DAL.Models;
using MediatR;

namespace BLL.Handlers.PlayerGames
{
    public class CreatePlayerGame
    {
        public class Command : IRequest
        {
            public PlayerGame PlayerGame { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.PlayerGames.AddAsync(request.PlayerGame);

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
