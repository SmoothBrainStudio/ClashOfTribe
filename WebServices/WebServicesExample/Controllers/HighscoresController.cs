using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebServicesExample.DAO;
using WebServicesExample.Models;

namespace WebServicesExample.Controllers
{
    public class HighscoresController : Controller
    {


        [HttpGet]
        [Route("/Highscores/{PlayerId}")]
        public Score? GetPlayerByPlayerId(int PlayerId)
        {
            HighscoresDAO scoresDAO = new HighscoresDAO();
            return scoresDAO.GetScoreByPlayerId(PlayerId);
        }

        [HttpGet]
        [Route("/Highscores/GetLeaderboard/")]
        public List<Score>? GetLeaderboard(int PageNumber)
        {
            HighscoresDAO scoresDAO = new HighscoresDAO();
            return scoresDAO.GetLeaderboard();
        }

        [HttpPost]
        [Route("/Highscores/Add")]
        public HttpResponseMessage AddPlayer([FromBody] Score _score)
        {
            //contrôle sur l'ajout fonctionne
            HighscoresDAO scoresDAO = new HighscoresDAO();
            if (scoresDAO.AddScore(_score))
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            //l'ajout a échoué
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        [Route("/Highscores/Update")]
        public bool ModifyPlayer([FromBody] Score _player)
        {
            HighscoresDAO scoresDAO = new HighscoresDAO();
            return scoresDAO.UpdateScore(_player);
        }



        [HttpDelete]
        [Route("/Highscores/Delete/{playerId}")]
        public bool DeletePlayer(int PlayerId)
        {
            HighscoresDAO scoresDAO = new HighscoresDAO();
            return scoresDAO.DeleteScoreWithPlayerId(PlayerId);
        }
    }
}
