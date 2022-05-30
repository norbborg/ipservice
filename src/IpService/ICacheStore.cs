namespace IpService;

public interface ICacheStore
{   
    void Add<T>(string key,T item, TimeSpan expiration);
 
    T? Get<T>(string key) where T : class;
}