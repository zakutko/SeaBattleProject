using DAL.Data;
using DAL.Models;
using MediatR;

namespace BLL.Handlers.Fields
{
    public class CreateField
    {
        public class Command : IRequest
        {
            public Field Field { get; set; }
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
                await _context.Fields.AddAsync(request.Field);

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
