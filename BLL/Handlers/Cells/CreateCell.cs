using DAL.Data;
using DAL.Models;
using MediatR;

namespace BLL.Handlers.Cells
{
    public class CreateCell
    {
        public class Command : IRequest
        {
            public Cell Cell { get; set; }
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
                await _context.Cells.AddAsync(request.Cell);

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
