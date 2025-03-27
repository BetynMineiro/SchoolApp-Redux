using MongoDB.Driver;

namespace SchoolApp.MongoDb.Adapter.Context;

public class MongoDbContext
{
    protected static string Database;

    public static MongoClient BuildMongoConnection(string connection, string databaseName)
    {
        Database = databaseName;
        return new MongoClient(connection);
    }
}