using System.Text.Json;

namespace EasyGames.Extensions;

// Tiny helpers to store complex objects (cart) in session as JSON
public static class SessionExtensions
{
    public static void SetJson(this ISession session, string key, object value) =>
        session.SetString(key, JsonSerializer.Serialize(value));

    public static T? GetJson<T>(this ISession session, string key) =>
        session.GetString(key) is string json ? JsonSerializer.Deserialize<T>(json) : default;
}
