using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace DAL
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser
                    {
                        DisplayName = "Test",
                        UserName = "Test",
                        Email = "test@gmail.com"
                    },
                    new AppUser
                    {
                        DisplayName = "Test2",
                        UserName = "Test2",
                        Email = "test2@gmail.com"
                    }
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Qwerty.0");
                }
            }

            if (!context.CellStates.Any() || !context.Directions.Any() || !context.GameStates.Any() || !context.ShipSizes.Any() || !context.ShipStates.Any())
            {
                var cellStates = new List<CellState>
                {
                    new CellState
                    {
                        CellStateName = "Empty"
                    },
                    new CellState
                    {
                        CellStateName = "Busy"
                    },
                    new CellState
                    {
                        CellStateName = "Hit"
                    },
                    new CellState
                    {
                        CellStateName = "Miss"
                    },
                    new CellState
                    {
                        CellStateName = "AroundTheShip"
                    },
                    new CellState
                    {
                        CellStateName = "Destroyed"
                    }
                };

                var directions = new List<Direction>
                {
                    new Direction
                    {
                        DirectionName = "Horizontal"
                    },
                    new Direction
                    {
                        DirectionName = "Vertical"
                    }
                };

                var gameStates = new List<GameState>
                {
                    new GameState
                    {
                        GameStateName = "Initialized"
                    },
                    new GameState
                    {
                        GameStateName = "Preparation"
                    },
                    new GameState
                    {
                        GameStateName = "Started"
                    },
                    new GameState
                    {
                        GameStateName = "Finished"
                    }
                };

                var shipSizes = new List<ShipSize>
                {
                    new ShipSize
                    {
                        ShipSizeName = "One"
                    },
                    new ShipSize
                    {
                        ShipSizeName = "Two"
                    },
                    new ShipSize
                    {
                        ShipSizeName = "Three"
                    },
                    new ShipSize
                    {
                        ShipSizeName = "Four"
                    }
                };

                var shipStates = new List<ShipState>
                {
                    new ShipState
                    {
                        ShipStateName = "Alive"
                    },
                    new ShipState
                    {
                        ShipStateName = "Hit"
                    },
                    new ShipState
                    {
                        ShipStateName = "Killed"
                    },
                };
                await context.CellStates.AddRangeAsync(cellStates);
                await context.Directions.AddRangeAsync(directions);
                await context.GameStates.AddRangeAsync(gameStates);
                await context.ShipSizes.AddRangeAsync(shipSizes);
                await context.ShipStates.AddRangeAsync(shipStates);
                await context.SaveChangesAsync();
            }
        }
    }
}
