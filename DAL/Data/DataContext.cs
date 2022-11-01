using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Cell> Cells { get; set; }
        public DbSet<CellState> CellStates { get; set; }
        public DbSet<Direction> Directions { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameField> GameFields { get; set; }
        public DbSet<GameState> GameStates { get; set; }
        public DbSet<PlayerGame> PlayerGames { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Ship> Ships { get; set; }
        public DbSet<ShipSize> ShipSizes { get; set; }
        public DbSet<ShipState> ShipStates { get; set; }
        public DbSet<ShipWrapper> ShipWrappers { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<GameHistory> GameHistories { get; set; }
    }
}
