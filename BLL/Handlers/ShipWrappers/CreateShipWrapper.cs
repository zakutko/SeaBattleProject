using DAL.Data;
using DAL.Models;
using MediatR;

namespace BLL.Handlers.ShipWrappers
{
    public class CreateShipWrapper
    {
        public class Command : IRequest
        {
            public ShipWrapper ShipWrapper { get; set; }
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
                await _context.ShipWrappers.AddAsync(request.ShipWrapper);

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
