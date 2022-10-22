using DAL.Data;
using DAL.Models;
using MediatR;

namespace BLL.Handlers.Ships
{
    public class DeleteShip
    {
        public class Command : IRequest
        {
            public Ship Ship { get; set; }
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
                _context.Ships.Remove(request.Ship);

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
