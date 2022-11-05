namespace API
{
    public static class Service
    {
        // http
        private const string Protocol = "http";
        
        // local
        private const string IPAddress = "127.0.0.1";
        // LAN
        // private const string IPAddress = "10.24.9.220";
        
        // port
        private const string Port = "2333";
        
        private const string BaseApi = Protocol + "://" + IPAddress + ":" + Port;

        // account 
        public const string Login = BaseApi + "/" + "login";
        public const string Register = BaseApi + "/" + "register";
        public const string UpdatePassword = BaseApi + "/" + "password";

        // game
        private const string BaseGame = BaseApi + "/" + "game";
        public const string Archive = BaseGame + "/" + "archive";
        public const string Play = BaseGame + "/" + "play";

        // player
        public const string Player = BaseApi + "/" + "player";

        // character
        public const string Character = BaseApi + "/" + "character";
        
        // structure
        public const string Structure = BaseApi + "/" + "structure";
    }
}