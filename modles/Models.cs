namespace Models
{
    public class RateLimiter
    {
        public string id { get; set; } = "";
        public string route { get; set; } = "";
        public int interval { get; set; } = 0;
        public int rate_limit { get; set; } = 0;
        public float last_updated { get; set; } = 0f;
        public float created_at { get; set; } = 0f;
        public string unique_id { get; set; } = "host";
    }


    class ProjectConfig
    {

        public string user_id { get; set; } = "";
        public Dictionary<string, RateLimiter> rate_limiter { get; set; } = new Dictionary<string, RateLimiter>();
        public Encryption encryption { get; set; } = new Encryption();
        public string base_url { get; set; } = "";
        public bool encryption_enabled { get; set; } = true;
        public bool rate_limiter_enabled { get; set; } = true;
        public float last_updated { get; set; } = 0f;
        public float created_at { get; set; } = 0f;

    }


    public class Encryption
    {

      public string  public_key { get; set; } = "";
      public string private_key { get; set; } = "";
      public int  use_count{ get; set; } = 0;   
      public float last_updated{ get; set; } = 0f;
      public float created_at{ get; set; } = 0f;

    }

}