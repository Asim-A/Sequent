using System.Text.Json.Serialization;

namespace Sequent.FileLoader.Client;

public class RandomUserResponse
{
    public IList<RandomUser> Results { get; set; }
}

public record Name(string First, string Last);

public record Street(int Number, string Name);

public record Coordinates(string Latitude, string Longitude);

public record Location(Street Street, string City, string Country, Coordinates Coordinates);

public record Id(string Value);

public record Datum(DateTime Date, int Age);

public record Login(string Uuid, string Username);

public enum Gender
{
    Male,
    Female
}

public class RandomUser
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Gender Gender { get; set; }

    public Name Name { get; set; }
    public Location Location { get; set; }
    public string Email { get; set; }
    public Id Id { get; set; }
    public string Nat { get; set; }
    public Datum Dob { get; set; }
    public Datum Registered { get; set; }
    public Login Login { get; set; }
}
