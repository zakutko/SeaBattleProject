using DAL.Data;
using DAL.Models;
using MediatR;

namespace BLL.Handlers.Games
{
    public class CreateGame
    {
        public class Command : IRequest
        {
            public Game Game { get; set; }
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
                await _context.Games.AddAsync(request.Game);

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
