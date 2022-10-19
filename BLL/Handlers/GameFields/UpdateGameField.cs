using DAL.Data;
using DAL.Models;
using MediatR;

namespace BLL.Handlers.GameFields
{
    public class UpdateGameField
    {
        public class Command : IRequest
        {
            public GameField GameField { get; set; }
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
                _context.ChangeTracker.Clear();
                _context.GameFields.Update(request.GameField);

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
