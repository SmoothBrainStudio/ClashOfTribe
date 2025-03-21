using UnityEngine;
using HttpUtils;
using System.Collections.Generic;

public class HttpClientScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ////On initialise le fichier de configuration pour avoir les infos du serveur à interroger
        //RestClient.Init();
        //// on crée un client 
        //RestClient client = new RestClient();

        ////On envoie la requête et on récupère la réponse du serveur sous forme de string
        //string str = client.SendToWebService("Highscores/GetPage/0", HttpVerb.GET, null, "");
        //List<Score> scores;
        ////on désérialise la chaîne de caractères en (liste) d'objets désirés 
        //scores = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Score>>(str);

        ////on affiche le résultat dans la console
        //foreach(Score score in scores )
        //{
        //    Debug.Log(score.Name + "|" + score.Value + "|" +score.Rang);
        //}


        ////création d'un nouveau player :
        //Player player = new Player() { NickName = "pouetpouet", Email = "pouetfgdfgd@pouet.com" };

        ////envoi du nouveau plauer au service
        //string playerAddResponse = client.SendToWebService("Players/AddPlayer", HttpVerb.POST, player, "");
    }
}
