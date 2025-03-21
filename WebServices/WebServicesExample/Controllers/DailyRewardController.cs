using Microsoft.AspNetCore.Mvc;
using WebServicesExample.DAO;

namespace WebServicesExample.Controllers
{
    public class DailyRewardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/DailyReward/Get/{playerId}")]
        public DailyReward GetDailyReward(int playerId)
        {
            DailyRewardDAO dailyDAO = new DailyRewardDAO();
            return dailyDAO.GetDailyRewardByPlayerId(playerId);
        }

        [HttpPost("/DailyReward/Add/{PlayerId}")]
        public bool AddDailyReward([FromBody] DailyReward dailyReward)
        {
            DailyRewardDAO dailyDAO = new DailyRewardDAO();

            return dailyDAO.AddDailyReward(dailyReward);
        }

        [HttpPut("/DailyReward/Update/")]
        public bool UpdateDailyReward([FromBody] DailyReward dailyReward)
        {
            DailyRewardDAO dailyRewardDAO = new DailyRewardDAO();

            return dailyRewardDAO.UpdateDailyReward(dailyReward);
        }

        [HttpDelete("/DailyReward/Delete/{playerId}")]
        public bool DeleteDailyReward(int playerId)
        {
            DailyRewardDAO dailyRewardDAO = new DailyRewardDAO();

            return dailyRewardDAO.DeleteDailyRewardWithPlayerId(playerId);
        }
    }
}
