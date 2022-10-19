using BLL.Handlers.GameStates;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace WEB.Controllers
{
    public class GameStateController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<List<GameState>>> GetGameStates()
        {
            return await Mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameState>> GetGameState(string id)
        {
            return await Mediator.Send(new Details.Query { Id = id });
        }
    }
}
