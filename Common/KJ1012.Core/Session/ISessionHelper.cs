namespace KJ1012.Core.Session
{
    public interface ISessionHelper
    {
        string GetSession(string key);
        T GetSession<T>(string key);
        void RemoveSession(string key);
        void WriteSession(string key, string value);
        void WriteSession<T>(string key, T value);
    }
}