namespace Models
{
    public class RateLimiter
    {
        public string Id { get; set; } = "";
        public string Route { get; set; } = "";
        public float Interval { get; set; } = 0f;
        public int Rate_limit { get; set; } = 0;
        public float Last_updated { get; set; } = 0f;
        public float Created_at { get; set; } = 0f;
        public string unique_id { get; set; } = "host";
        public bool Is_active { get; set; } = false;
        public string Rate_limit_type { get; set; } = "ip Address";  //ip address, header, body, query_param
        public Dictionary<string ,Error_payload> Error_payload =new Dictionary<string, Error_payload>();

    }

    public class Error_payload
    {
        string Message  {get;set ;} = "max_limit_request_reached";
        string Uid  {get;set ;} = "";
    }


    class ProjectConfig
    {
        public string  User_id{get;set;} ="";

        public Dictionary<string ,RateLimiter>    Rate_limiter {get; set;} = new Dictionary<string, RateLimiter>();
        public Encryption  Encryption{get;set;}  = new  Encryption();
        public string Base_url {get;set ;} ="";
        public bool  Encryption_enabled {get ;set;} = false;
        public bool  Rate_limiter_enabled {get ;set;} = false;
        public float  Last_updated {get ;set;} = DateTime.Now.Ticks;
        public float  created_at {get ;set;} = DateTime.Now.Ticks;
    }
    
     




    public class Encryption
    {
        public string public_key { get; set; } = "";
        public string private_key { get; set; } = "";
        public int use_count { get; set; } = 0;
        public float last_updated { get; set; } = 0f;
        public float created_at { get; set; } = 0f;

    }

}