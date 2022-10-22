using DAL.Data;
using DAL.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Handlers.Positions
{
    public class UpdatePosition
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
                _context.ChangeTracker.Clear();
                _context.Positions.Update(request.Position);

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
