namespace ReviewMe.Core;

public static class Utilities
{
    public static string GetProfilePhoto(string login)
        => $"https://localhost:8080/persons/login/{login}/photo";
}