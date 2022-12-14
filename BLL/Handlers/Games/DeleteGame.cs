using DAL.Data;
using DAL.Models;
using MediatR;

namespace BLL.Handlers.Games
{
    public class DeleteGame
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
                _context.Games.Remove(request.Game);

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
