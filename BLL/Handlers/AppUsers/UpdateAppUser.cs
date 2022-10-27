using DAL.Data;
using DAL.Models;
using MediatR;

namespace BLL.Handlers.AppUsers
{
    public class UpdateAppUser
    {
        public class Command : IRequest
        {
            public AppUser AppUser { get; set; }
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
                _context.AppUsers.Update(request.AppUser);

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
