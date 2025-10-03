namespace Hyprship.Database.Models;

public class Uuid7
{
    public static Guid New()
        => Guid.CreateVersion7();

    public static string Stamp()
        => Guid.CreateVersion7().ToString("N");
}