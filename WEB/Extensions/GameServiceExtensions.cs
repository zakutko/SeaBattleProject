using BLL.Handlers.Fields;
using BLL.Handlers.GameFields;
using BLL.Handlers.Games;
using BLL.Handlers.PlayerGames;
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

            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<IRepository<Game>, Repository<Game>>();
            services.AddScoped<IRepository<GameState>, Repository<GameState>>();
            services.AddScoped<IRepository<AppUser>, Repository<AppUser>>();
            services.AddScoped<IRepository<PlayerGame>, Repository<PlayerGame>>();
            services.AddScoped<IRepository<GameField>, Repository<GameField>>();

            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IGameStateService, GameStateService>();
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IPlayerGameService, PlayerGameService>();
            services.AddScoped<IGameFieldService, GameFieldService>();

            return services;
        }
    }
}
