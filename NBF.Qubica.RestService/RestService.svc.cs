using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using NBF.Qubica.Managers;
using NBF.Qubica.Classes;
using NLog;
using System.Diagnostics;
using System.Reflection;
using NBF.Qubica.Common;
using System.Net;
using System.Web;

namespace NBF.Qubica.RestWebService
{
    public class RestService : IRestService
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        public LoginResponse Login(LoginRequest loginRequest)
        {
            logger.Debug(Settings.MethodName());
            logger.Trace("Parameters: login: " + loginRequest.username + " password: " + loginRequest.password + " frequentbowlernumber:" + loginRequest.frequentbowlernumber);

            S_User user = UserManager.GetUserByNamePasswordAndFrequentbowlernumber(loginRequest.username, loginRequest.password, loginRequest.frequentbowlernumber);

            if (user != null)
            {
                user.logindatetime = DateTime.Now.Ticks;
                UserManager.Update(user);

                LoginResponse r = new LoginResponse { userid = user.id, status = "OK" };
                logger.Trace("Return: userid: " + r.userid + " status: " + r.status);
                return r;
            }
            else
            {
                LoginResponse r = new LoginResponse();
                r.status = "NOTOK";
                logger.Trace("Return: Status: " + r.status);
                return r;
            }
        }

        public LogoutResponse Logout(string id)
        {
            logger.Debug(Settings.MethodName());
            logger.Trace("Parameters: id: " + id);

            long userid;
            long.TryParse(id, out userid);

            S_User user = UserManager.GetUserById(userid);
            if (user != null)
            {
                user.logindatetime = null;
                UserManager.Update(user);

                logger.Trace("Return: Status: OK");
                return new LogoutResponse { status = "OK" };
            }
            else
            {
                LogoutResponse r = new LogoutResponse();
                r.status = "NOTOK";
                logger.Trace("Return: Status: " + r.status);
                return r;
            }
        }

        private bool isCorrectUser(string id)
        {
            logger.Debug(Settings.MethodName());
            logger.Trace("Parameters: id: " + id);

            long loggedInUserId;
            if (long.TryParse(id, out loggedInUserId))
            {
                if (UserManager.IsLoggedIn(loggedInUserId))
                {
                    logger.Trace("Return: true");
                    return true;
                }
                else
                {
                    logger.Trace("Not logged in (1), Session expired or not logged in.");
                    ErrorData errorData = new ErrorData("Not logged in.", "Session expired or not logged in.");
                    throw new WebFaultException<ErrorData>(errorData, HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                logger.Trace("Not logged in (2), Session expired or not logged in.");
                ErrorData errorData = new ErrorData("Incorrect user.", "The user could not be determined.");
                throw new WebFaultException<ErrorData>(errorData, HttpStatusCode.Unauthorized);
            }
        }

        public Profile GetProfile(string id, string otheruserid)
        {
            long thisUserId;
            long userid; 
            
            logger.Debug(Settings.MethodName());
            logger.Trace("Parameters: id: " + id + "otheruserid: " + otheruserid);

            if (isCorrectUser(id))
            {
                long.TryParse(id, out thisUserId);

                // voor het opvragen van een profiel van een andere gebruiker, gebruiken we het andere id
                if (!String.IsNullOrEmpty(otheruserid))
                    id = otheruserid;

                long.TryParse(id, out userid);

                S_User user = UserManager.GetUserById(userid);

                if (user != null)
                {
                    Profile profile = new Profile();

                    profile.user = new User();
                    profile.user.userid = user.id;
                    profile.user.city = user.city;
                    profile.user.name = user.name;
                    profile.user.username = user.username;

                    profile.user.scores = GameManager.GetProfileScores(user.username, user.frequentbowlernumber);
                    if (String.IsNullOrEmpty(otheruserid))
                        profile.user.is_favorite = false;
                    else
                        profile.user.is_favorite = FavoritManager.IsUserFavoritOfUser(thisUserId, userid);

                    logger.Trace("Return: userid: " + profile.user.userid);
                    logger.Trace("Return: city: " + profile.user.city);
                    logger.Trace("Return: name: " + profile.user.name);
                    logger.Trace("Return: username: " + profile.user.username);
                    logger.Trace("Return: is_favorite: " + profile.user.is_favorite);
                    return profile;
                }
            }

            return null;
        }

        public UserGames GetGames(string id, string otheruserid)
        {
            logger.Debug(Settings.MethodName());

            if (isCorrectUser(id))
            {
                // voor het opvragen van games van een andere gebruiker, gebruiken we het andere id
                if (!String.IsNullOrEmpty(otheruserid))
                    id = otheruserid;

                UserGames userGames = new UserGames();

                long userid;
                long.TryParse(id, out userid);

                S_User user = UserManager.GetUserById(userid);

                if (user != null)
                {
                    userGames = GameManager.GetGamesByUser(user);
                }

                return userGames;
            }

            return null;
        }

        public PlayedGames GetPlayedGames(string id, string centerid, string gamedate, string otheruserid)
        {
            logger.Debug(Settings.MethodName());
            
            PlayedGames playedGames = new PlayedGames();

            if (isCorrectUser(id))
            {
                // voor het opvragen van games van een andere gebruiker, gebruiken we het andere id
                if (!String.IsNullOrEmpty(otheruserid))
                    id = otheruserid;

                long userid;
                long bowlingcenterid;

                int year;
                int month;
                int day;

                try
                {
                    long.TryParse(centerid, out bowlingcenterid);
                    long.TryParse(id, out userid);
                    int.TryParse(gamedate.Substring(0, 4), out year);
                    int.TryParse(gamedate.Substring(4, 2), out month);
                    int.TryParse(gamedate.Substring(6, 2), out day);
                    DateTime gameDate = new DateTime(year, month, day);

                    S_User user = UserManager.GetUserById(userid);
                    S_BowlingCenter bowlingcenter = BowlingCenterManager.GetBowlingCenterById(bowlingcenterid);

                    if (user != null && bowlingcenter != null)
                    {
                        playedGames = GameManager.GetPlayedGamesByUserAndBowlingcenterAndDate(user, bowlingcenter, gameDate);
                    }
                }
                catch
                {
                }
            }

            return playedGames;
        }
        
        public Game GetGame(string id, string gid)
        {
            logger.Debug(Settings.MethodName());

            Game game = new Game();

            if (isCorrectUser(id))
            {
                long gameid;

                try
                {
                    long.TryParse(gid, out gameid);
                    game = GameManager.GetPlayedGameById(gameid);
                }
                catch
                {
                }
            }

            return game;
        }

        public Favorit[] GetFavorites(string id)
        {
            logger.Debug(Settings.MethodName());

            List<Favorit> favoritsList = new List<Favorit>();
            long userid;

            if (isCorrectUser(id))
            {
                try
                {
                    long.TryParse(id, out userid);

                    List<S_Favorit> favorits = FavoritManager.GetFavoritsByUserId(userid);

                    foreach (S_Favorit favorit in favorits)
                    {
                        S_User user = UserManager.GetUserById(favorit.favorituserId);
                        Favorit myFavorit = new Favorit();
                        myFavorit.userid = user.id;
                        myFavorit.name = user.name;
                        myFavorit.scores = GameManager.GetScoresByUser(user);

                        favoritsList.Add(myFavorit);
                    }
                }
                catch
                {
                }
            }

            return favoritsList.ToArray();
        }

        public FavoritStatus AddFavorite(FavoritRequest favoritRequest)
        {
            logger.Debug(Settings.MethodName());

            FavoritStatus favoritStatus = new FavoritStatus();

            if (isCorrectUser(favoritRequest.userid.ToString()))
            {
                try
                {
                    S_Favorit favorit = new S_Favorit();
                    favorit.userId = favoritRequest.userid;
                    favorit.favorituserId = favoritRequest.favorituserid;

                    long? i_id = FavoritManager.Insert(favorit);

                    if (i_id != null)
                    {
                        favoritStatus.status = "Added";
                        favoritStatus.user_message = "Gebruiker toegevoegd aan favorieten";
                    }
                    else
                        favoritStatus.status = "NotAdded";
                }
                catch
                {
                    favoritStatus.status = "NotAdded";
                }
            }

            return favoritStatus;
        }

        public FavoritStatus DeleteFavorite(string id, string favoritsid)
        {
            logger.Debug(Settings.MethodName());

            FavoritStatus favoritStatus = new FavoritStatus();

            if (isCorrectUser(id))
            {
                long userid;
                long.TryParse(id, out userid);

                long favorituserid;
                long.TryParse(favoritsid, out favorituserid);
                try
                {
                    long? d_id = FavoritManager.GetFavoritIdByUserIdFavoritId(userid, favorituserid);

                    if (d_id != null)
                    {
                        FavoritManager.Delete((long)d_id);
                        favoritStatus.status = "Deleted";
                        favoritStatus.user_message = "Gebruiker verwijderd uit favorieten";
                    }
                    else
                        favoritStatus.status = "NotDeleted";
                }
                catch
                {
                    favoritStatus.status = "NotDeleted";
                }
            }

            return favoritStatus;
        }

        public BowlingCenter[] GetBowlingCenters(string id)
        {
            logger.Debug(Settings.MethodName());

            if (isCorrectUser(id))
            {
                List<S_BowlingCenter> bowlingCenterList = BowlingCenterManager.GetBowlingCenters();

                if (bowlingCenterList != null)
                {
                    BowlingCenter[] bowlingCenters = new BowlingCenter[bowlingCenterList.Count()];

                    int i = 0;
                    foreach (S_BowlingCenter bowlingCenter in bowlingCenterList)
                    {
                        bowlingCenters[i] = new BowlingCenter();
                        bowlingCenters[i].bowlingcenterId = bowlingCenter.id;
                        bowlingCenters[i++].bowlingcenterName = bowlingCenter.name;
                    }

                    return bowlingCenters;
                }
            }

            return null;
        }

        public Center GetBowlingCenter(string id, string bowlingid)
        {
            logger.Debug(Settings.MethodName());

            if (isCorrectUser(id))
            {
                S_BowlingCenter bowlingCenter = BowlingCenterManager.GetBowlingCenterById(long.Parse(bowlingid));

                if (bowlingCenter != null)
                {
                    Center center = new Center();

                    center.address = bowlingCenter.address;
                    center.bowlingcenterId = long.Parse(bowlingid);
                    center.bowlingcenterName = bowlingCenter.name;
                    center.city = bowlingCenter.city;
                    center.email = bowlingCenter.email;
                    center.logo = bowlingCenter.logo;
                    center.phonenumber = bowlingCenter.phonenumber;

                    List<S_Advert> advertList = AdvertManager.GetAdvertsByBowlingCenterid(center.bowlingcenterId);
                    if (advertList != null)
                    {
                        center.adverts = new AdvertisementInfo[advertList.Count()];
                        int i = 0;

                        foreach (S_Advert advert in advertList)
                        {
                            center.adverts[i] = new AdvertisementInfo();
                            center.adverts[i].advertisement = advert.advertisement;
                            center.adverts[i].advertisement_www = advert.advertisement_www;
                            center.adverts[i++].advertisement_url = advert.advertisement_url;
                        }
                    }

                    List<S_Opentime> opentimeList = OpentimeManager.GetOpentimesByBowlingcenterId(center.bowlingcenterId);
                    if (opentimeList != null)
                    {
                        center.times = new Opentime[opentimeList.Count()];
                        int i = 0;
                        foreach (S_Opentime openTime in opentimeList)
                        {
                            center.times[i] = new Opentime();
                            center.times[i].Day = openTime.day.ToString();
                            center.times[i].Start = openTime.openTime;
                            center.times[i++].Finish = openTime.closeTime;
                        }
                    }

                    return center;
                }
            }

            return null;
        }

        public Lane[] GetBowlingCenterLanes(string id, string bowlingid)
        {
            logger.Debug(Settings.MethodName());

            if (isCorrectUser(id))
            {
                long bowlingCenterID = 0;
                long.TryParse(bowlingid, out bowlingCenterID);

                S_BowlingCenter bowlingCenter = BowlingCenterManager.GetBowlingCenterById(bowlingCenterID);

                if (bowlingCenter != null && bowlingCenter.numberOfLanes != null)
                {
                    Lane[] lanes = new Lane[(int)bowlingCenter.numberOfLanes];

                    for (int laneIndex = 1; laneIndex <= bowlingCenter.numberOfLanes; laneIndex++)
                    {
                        lanes[laneIndex - 1] = new Lane();
                        lanes[laneIndex - 1].lanenr = laneIndex;
                    }
                    return lanes;
                }
            }

            return null;
        }

        public Lane GetBowlingCenterLane(string id, string bowlingid, string laneid)
        {
            logger.Debug(Settings.MethodName());

            if (isCorrectUser(id))
            {
                S_BowlingCenter bowlingCenter = BowlingCenterManager.GetBowlingCenterById(long.Parse(bowlingid));

                if (bowlingCenter != null && laneid != null)
                {
                    try
                    {
                        int laneID = int.Parse(laneid);

                        if (laneID <= bowlingCenter.numberOfLanes)
                        {
                            Lane lane = new Lane();
                            lane.lanenr = laneID;

                            return lane;
                        }
                    }
                    catch
                    {
                    }
                }
            }

            return null;
        }

        public UsersList GetSearchResult(string id, string searchstring)
        {
            logger.Debug(Settings.MethodName());

            UsersList userList = new UsersList();

            if (isCorrectUser(id))
            {
                try
                {
                    userList.users = GameManager.SearchUser(searchstring);
                }
                catch
                {
                }
            }

            return userList;
        }

        public NbfInfo GetNbfInfo(string id)
        {
            logger.Debug(Settings.MethodName());

            NbfInfo nbfInfo = new NbfInfo();

            if (isCorrectUser(id))
            {
                try
                {
                    S_Federation federation = FederationManager.GetFederation();
                    if (federation != null)
                    {
                        nbfInfo.information = federation.information;
                        nbfInfo.logo = Conversion.UriToEscapedUri(federation.logo);
                    }
                }
                catch
                {
                }
            }

            return nbfInfo;
        }

        public AdvertisementInfo GetAdvertisement(string id)
        {
            logger.Debug(Settings.MethodName());
            logger.Trace("Parameters: id: " + id);

            AdvertisementInfo advertisementInfo = new AdvertisementInfo();

            if (isCorrectUser(id))
            {
                try
                {
                    S_Advert advert = AdvertManager.GetRandomAdvert();
                    if (advert != null)
                    {
                        advertisementInfo.advertisement = advert.advertisement;
                        advertisementInfo.advertisement_url = Conversion.UriToEscapedUri(advert.advertisement_url);
                        advertisementInfo.advertisement_www = Conversion.UriToEscapedUri(advert.advertisement_www);
                    }
                }
                catch
                {
                }
            }

            logger.Trace("Return: advertisement: " + advertisementInfo.advertisement);
            logger.Trace("Return: advertisement_url: " + advertisementInfo.advertisement_url);
            logger.Trace("Return: advertisement_www: " + advertisementInfo.advertisement_www);


            return advertisementInfo;
        }
    }
}
