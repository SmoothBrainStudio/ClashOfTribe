using UnityEngine;
using HttpUtils;
using System.Collections.Generic;

public class HttpClientScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ////On initialise le fichier de configuration pour avoir les infos du serveur � interroger
        //RestClient.Init();
        //// on cr�e un client 
        //RestClient client = new RestClient();

        ////On envoie la requ�te et on r�cup�re la r�ponse du serveur sous forme de string
        //string str = client.SendToWebService("Highscores/GetPage/0", HttpVerb.GET, null, "");
        //List<Score> scores;
        ////on d�s�rialise la cha�ne de caract�res en (liste) d'objets d�sir�s 
        //scores = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Score>>(str);

        ////on affiche le r�sultat dans la console
        //foreach(Score score in scores )
        //{
        //    Debug.Log(score.Name + "|" + score.Value + "|" +score.Rang);
        //}


        ////cr�ation d'un nouveau player :
        //Player player = new Player() { NickName = "pouetpouet", Email = "pouetfgdfgd@pouet.com" };

        ////envoi du nouveau plauer au service
        //string playerAddResponse = client.SendToWebService("Players/AddPlayer", HttpVerb.POST, player, "");
    }
}
