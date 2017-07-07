using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace NBF.Qubica.Classes
{
    [ServiceContract]
    public interface IRestService
    {
        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/v1/users/auth/login")]
        LoginResponse Login(LoginRequest loginRequest);

        [OperationContract]
        [WebInvoke(Method="DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/v1/users/auth/logout/{id}")]
        LogoutResponse Logout(string id);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/v1/users/{id}/profile/{*userid}")]
        Profile GetProfile(string id, string userid);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/v1/users/{id}/games/{*userid}")]
        UserGames GetGames(string id, string userid);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/v1/users/{id}/games/bowling/{bowlingcenterid}/{gamedate}/{*userid}")]
        PlayedGames GetPlayedGames(string id, string bowlingcenterid, string gamedate, string userid);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/v1/users/{id}/games/game/{gameid}")]
        Game GetGame(string id, string gameid);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/v1/users/{id}/profile/favorites")]
        Favorit[] GetFavorites(string id);

        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Json,
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/v1/users/profile/favorites")]
        FavoritStatus AddFavorite(FavoritRequest favoritsRequest);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/v1/users/{id}/profile/favorites/{favoritsid}")]
        FavoritStatus DeleteFavorite(string id, string favoritsid);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/v1/bowlingcenters/{id}")]
        BowlingCenter[] GetBowlingCenters(string id);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/v1/bowlingcenters/{id}/{bowlingid}")]
        Center GetBowlingCenter(string id, string bowlingid);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/v1/bowlingcenters/{id}/{bowlingid}/lanes")]
        Lane[] GetBowlingCenterLanes(string id, string bowlingid);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/v1/bowlingcenters/{id}/{bowlingid}/lanes/{laneid}")]
        Lane GetBowlingCenterLane(string id, string bowlingid, string laneid);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/v1/search/{id}/{searchstring}")]
        UsersList GetSearchResult(string id, string searchstring);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/v1/nbf/{id}")]
        NbfInfo GetNbfInfo(string id);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/v1/advertisement/{id}")]
        AdvertisementInfo GetAdvertisement(string id);
    }

    [DataContract]
    public class LoginResponse
    {
        [DataMember]
        public long userid { get; set; }

        [DataMember]
        public string status { get; set; }
    }

    [DataContract]
    public class LoginRequest
    {
        [DataMember]
        public string username { get; set; }

        [DataMember]
        public string password { get; set; }

        [DataMember]
        public int frequentbowlernumber { get; set; }
    }

    [DataContract]
    public class LogoutResponse
    {
        [DataMember]
        public string status { get; set; }
    }

    [DataContract]
    public class LogoutRequest
    {
        [DataMember]
        public long userid { get; set; }
    }

    [DataContract]
    public class Profile
    {
        [DataMember]
        public User user { get; set; }
    }

    public class User
    {
        public long userid { get; set; }
        public string username { get; set; }
        public string name  { get; set; }
        public string city { get; set; }
        public bool is_favorite { get; set; }
        public Scores scores { get; set; }
    }

    public class Scores
    {
        public int strikes { get; set; }
        public int spares { get; set; }
        public int splits { get; set; }
        public int gutters { get; set; }
        public int faults { get; set; }
        public int manualmodifieds { get; set; }
        public int total_score { get; set; }
    }

    [DataContract]
    public class UserGames
    {
        [DataMember]
        public long userid { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public Games[] games { get; set; }
    }

    public class Games
    {
        public long bowlingcenterid { get; set; }
        public string bowlingcenterName { get; set; }
        public DateTime bowlDate { get; set; }
        public GameScores scores { get; set; }
    }

    public class GameScores
    {
        public int strikes { get; set; }
        public int spares { get; set; }
        public int bestGame { get; set; }
    }

    [DataContract]
    public class PlayedGames
    {
        [DataMember]
        public long bowlingcenterid { get; set; }
        
        [DataMember]
        public string bowlingcenterName  { get; set; }
        
        [DataMember]
        public DateTime date_played { get; set; }
        
        [DataMember]
        public GamesPlayed[] games { get; set; }
    }
  
    public class GamesPlayed
    {
        public long gameid { get; set; }
        public string gameName { get; set; }
        public Score score { get; set; }
    }

    public class Score
    {
        public int strikes { get; set; }
        public int spares { get; set; }
        public int totalScore { get; set; }
    }

    [DataContract]
    public class Game
    {
        [DataMember]
        public long bowlingcenterid { get; set; }
        
        [DataMember]
        public string bowlingcenterName { get; set; }
        
        [DataMember]
        public DateTime datePlayed { get; set; }
        
        [DataMember]
        public SingleGame game { get; set; }
        
        [DataMember]
        public Scores scores { get; set; }
    }

    public class SingleGame
    {
        public Frame[] frames { get; set; }
    }

    public class Frame
    {
        public long frameid { get; set; }
        public Bowl bowl1 { get; set; }
        public Bowl bowl2 { get; set; }
        public Bowl bowl3 { get; set; }
        public Scores framescore { get; set; }
        public int progressive_total { get; set; }
    }

    public class Bowl
    {
        public int pins { get; set; }
        public bool is_strike { get; set; }
        public bool is_spare { get; set; }
        public bool is_gutter { get; set; }
        public bool is_split { get; set; }
        public bool is_foul { get; set; }
        public bool is_manuallymodified {  get; set; }
    }

    [DataContract]
    public class Favorit
    {
        [DataMember]
        public long userid { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public GameScores scores { get; set; } 
    }

    [DataContract]
    public class FavoritRequest
    {
        [DataMember]
        public long userid { get; set; }

        [DataMember]
        public long favorituserid { get; set; }
    }

    [DataContract]
    public class FavoritStatus
    {
        [DataMember]
        public string status { get; set; }

        [DataMember]
        public string user_message { get; set; }
    }

    [DataContract]
    public class BowlingCenter
    {
        [DataMember]
        public long bowlingcenterId { get; set; }

        [DataMember]
        public string bowlingcenterName { get; set; }
    }

    [DataContract]
    public class Center
    {
        [DataMember]
        public long bowlingcenterId { get; set; }

        [DataMember]
        public string bowlingcenterName { get; set; }

        [DataMember]
        public string logo { get; set; }

        [DataMember]
        public string address { get; set; }

        [DataMember]
        public string zipcode { get; set; }

        [DataMember]
        public string city { get; set; }

        [DataMember]
        public string phonenumber { get; set; }

        [DataMember]
        public string email { get; set; }

        [DataMember]
        public string website { get; set; }

        [DataMember]
        public AdvertisementInfo[] adverts { get; set; }

        [DataMember]
        public Opentime[] times { get; set; }
    }

    public class Opentime
    {
        public string Day { get; set; }
        public string Start { get; set; }
        public string Finish { get; set; }
    }

    [DataContract]
    public class Lane
    {
        [DataMember]
        public int lanenr { get; set; }
    }

    [DataContract]
    public class UsersList
    {
        [DataMember]
        public SearchUser[] users { get; set;}
    }

    public class SearchUser
    {
        public long userid { get; set; }
        public string name { get; set; }
        public string fullname { get; set; }
        public long freqentbowlernumber { get; set; }
        public GameScores scores { get; set; }
    }

    [DataContract]
    public class NbfInfo
    {
        [DataMember]
        public string information { get; set; }

        [DataMember]
        public string logo { get; set; }
    }

    [DataContract]
    public class AdvertisementInfo
    {
        [DataMember]
        public string advertisement { get; set; }

        [DataMember]
        public string advertisement_url { get; set; }
    
        [DataMember]
        public string advertisement_www { get; set; }
    }

    [DataContract]
    public class ErrorData
    {
        public ErrorData(string reason, string detailedInformation)
        {
            Reason = reason;
            DetailedInformation = detailedInformation;
        }

        [DataMember]
        public string Reason { get; private set; }

        [DataMember]
        public string DetailedInformation { get; private set; }
    }

}