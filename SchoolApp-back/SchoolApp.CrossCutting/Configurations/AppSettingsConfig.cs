namespace SchoolApp.CrossCutting.Configurations;

public class AppSettingsConfig
{
    public Auth0Config Auth0Config { get; set; }
    public MongoConfig MongoConfig { get; set; }
}

public class Auth0Config
{
    public string Domain { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string Audience { get; set; }
    public string Connection { get; set; }
}

public class MongoConfig
{
    public string Connection { get; set; }
    public string Database { get; set; }
}