using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebServicesExample.DAO;
using WebServicesExample.Utils;

namespace WebServicesExample.Controllers
{
    public class PlayerController : Controller
    {
        [HttpGet]
        [Route("/Players/{PlayerId}")]
        public Player? GetPlayerByPlayerId(int PlayerId)
        {
            PlayersDAO playersDAO = new PlayersDAO();
            return playersDAO.GetPlayerByPlayerId(PlayerId);
        }

        [HttpGet]
        [Route("/Players/All")]
        public List<Player> GetAllPlayers()
        {
            PlayersDAO playersDAO = new PlayersDAO();
            return playersDAO.GetAllPlayers();
        }

            [HttpPost]
        [Route("/Players/AddPlayer")]
        public HttpResponseMessage AddPlayer([FromBody] Player _player)
        {
            if (_player != null)
            {
                //controle sur l'email
                //if (!Utilities.IsValidEmail(_player.Email))
                //{
                //    return new HttpResponseMessage(HttpStatusCode.Forbidden);
                //}
                ////contrôle sur le surnom
                //else if (!System.Text.RegularExpressions.Regex.IsMatch(_player.NickName, @"^[a-zA-Z''-'\s]{1,40}$"))
                //{
                //    return new HttpResponseMessage(HttpStatusCode.Forbidden);
                //}
                //contrôle sur la taille du password
                //else if (player.EncryptedPassword.Length != 128)
                //{
                //    return new HttpResponseMessage(HttpStatusCode.Forbidden);
                //}

                //contrôle sur l'ajout fonctionne
                PlayersDAO playerDAO = new PlayersDAO();
                if (playerDAO.AddPlayer(_player))
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                //l'ajout a échoué
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        [HttpPut]
        [Route("/Players/Update")]
        public bool ModifyPlayer([FromBody] Player _player)
        {
            PlayersDAO playersDAO = new PlayersDAO();
            return playersDAO.UpdatePlayer(_player);
        }

        [HttpDelete]
        [Route("/Players/Delete/{playerId}")]
        public bool DeletePlayer(int PlayerId)
        {
            PlayersDAO playersDAO = new PlayersDAO();
            return playersDAO.DeletePlayerWithId(PlayerId);
        }
    }
}
