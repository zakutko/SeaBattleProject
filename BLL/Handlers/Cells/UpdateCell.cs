using DAL.Data;
using DAL.Models;
using MediatR;

namespace BLL.Handlers.Cells
{
    public class UpdateCell
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
                _context.ChangeTracker.Clear();
                _context.Cells.Update(request.Cell);

                var cells = _context.Cells.ToList();
                cells.OrderBy(x => x.Id);

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
