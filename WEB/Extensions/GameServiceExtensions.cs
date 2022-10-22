using BLL.Handlers.Cells;
using BLL.Handlers.Fields;
using BLL.Handlers.GameFields;
using BLL.Handlers.Games;
using BLL.Handlers.PlayerGames;
using BLL.Handlers.Positions;
using BLL.Handlers.Ships;
using BLL.Handlers.ShipWrappers;
using BLL.Interfaces;
using BLL.Services;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories;
using MediatR;

namespace WEB.Extensions
{
    public static class GameServiceExtensions
    {
        public static IServiceCollection AddGameServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(typeof(ListGames.Handler).Assembly);
            services.AddMediatR(typeof(CreateGame.Handler).Assembly);
            services.AddMediatR(typeof(UpdateGame.Handler).Assembly);
            services.AddMediatR(typeof(CreatePlayerGame.Handler).Assembly);
            services.AddMediatR(typeof(List.Handler).Assembly);
            services.AddMediatR(typeof(CreateField.Handler).Assembly);
            services.AddMediatR(typeof(CreateGameField.Handler).Assembly);
            services.AddMediatR(typeof(UpdateCell.Handler).Assembly);
            services.AddMediatR(typeof(CreatePosition.Handler).Assembly);
            services.AddMediatR(typeof(CreateShip.Handler).Assembly);
            services.AddMediatR(typeof(DeleteShip.Handler).Assembly);
            services.AddMediatR(typeof(CreateShipWrapper.Handler).Assembly);
            services.AddMediatR(typeof(DeleteShipWrapper.Handler).Assembly);

            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IRepository<Game>, Repository<Game>>();
            services.AddScoped<IRepository<AppUser>, Repository<AppUser>>();
            services.AddScoped<IRepository<PlayerGame>, Repository<PlayerGame>>();
            services.AddScoped<IRepository<GameField>, Repository<GameField>>();
            services.AddScoped<IRepository<GameState>, Repository<GameState>>();
            services.AddScoped<IRepository<Direction>, Repository<Direction>>();    
            services.AddScoped<IRepository<Cell>, Repository<Cell>>();
            services.AddScoped<IRepository<Position>, Repository<Position>>();
            services.AddScoped<IRepository<Field>, Repository<Field>>();
            services.AddScoped<IRepository<ShipWrapper>, Repository<ShipWrapper>>();
            services.AddScoped<IRepository<Ship>, Repository<Ship>>();

            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IPlayerGameService, PlayerGameService>();
            services.AddScoped<IGameFieldService, GameFieldService>();
            services.AddScoped<IGameStateService, GameStateService>();
            services.AddScoped<IDirectionService, DirectionService>();
            services.AddScoped<ICellService, CellService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IFieldService, FieldService>();
            services.AddScoped<IShipWrapperService, ShipWrapperService>();

            return services;
        }
    }
}
