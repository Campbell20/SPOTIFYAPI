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
       
       [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ArtistNameModel artistNameModel)
        {
             if (ModelState.IsValid)
                {
                     
                
                    //Create the auth object
            auth = new ClientCredentialsAuth()
            {
                //Your client Id
                ClientId = "6a2b2e87ffeb4fb6b9afd05314fe0eed",
                //Your client secret UNSECURE!!
                ClientSecret = "e18a056a9abc4a01b1d228ac4b043177",
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
            var track = spotify.SearchItems(artistNameModel.ArtistName, SearchType.Artist | SearchType.Playlist);
            var songName = spotify.SearchItems(artistNameModel.SongName, SearchType.Track | SearchType.Playlist);
           
            if(!string.IsNullOrEmpty(artistNameModel.SongName))
            {
                ViewData["SongJson"] =  JsonConvert.SerializeObject(songName.Tracks);
                ViewData["Songs"] = songName.Tracks.Items.ToList();
            }else if(!string.IsNullOrEmpty(artistNameModel.ArtistName))
            {
                ViewData["ArtistsJson"] = JsonConvert.SerializeObject(track.Artists);
                ViewData["Artists"] = track.Artists.Items.ToList();
            }


           }
    
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
