using DAL.Data;
using DAL.Models;
using MediatR;

namespace BLL.Handlers.Positions
{
    public class CreatePosition
    {
        public class Command : IRequest
        {
            public Position Position { get; set; }
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
                await _context.Positions.AddAsync(request.Position);

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
