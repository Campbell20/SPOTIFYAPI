using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MetaDataManager.Models;

using SpotifyAPI.Web; //Base Namespace
using SpotifyAPI.Web.Auth; //All Authentication-related classes
using SpotifyAPI.Web.Enums; //Enums
using SpotifyAPI.Web.Models; //Models for the JSON-responses
using Newtonsoft.Json;

namespace MetaDataManager.Controllers
{
    public class HomeController : Controller
    {
        //private static SpotifyWebAPI _spotify;
        static ClientCredentialsAuth auth;
        public async Task<IActionResult> Index()
        {

            //Create the auth object
            auth = new ClientCredentialsAuth()
            {
                //Your client Id
                ClientId = "YOUR_CLIENT_ID",
                //Your client secret UNSECURE!!
                ClientSecret = "YOUR_SECRET_KEY",
                //How many permissions we need?
                Scope = Scope.UserReadPrivate,
            };
            //With this token object, we now can make calls
            Token token = auth.DoAuth();
            var spotify = new SpotifyWebAPI()
            {
                TokenType = token.TokenType,
                AccessToken = token.AccessToken,
                UseAuth = true
            };

            //SearchItem track = spotify.SearchItems("roadhouse+blues", SearchType.Album | SearchType.Playlist);
            //var track = spotify.SearchItems("roadhouse+blues", SearchType.Album | SearchType.Playlist);
            var track = spotify.SearchItems("journey", SearchType.Artist | SearchType.Playlist);

            
            ViewData["ArtistsJson"] = JsonConvert.SerializeObject(track.Artists);
            ViewData["Artists"] = track.Artists.Items.ToList();
           
    
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
