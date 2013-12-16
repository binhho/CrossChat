namespace Abo.Server.Infrastructure.Protocol
{
    public interface IDtoSerializer
    {
        byte[] Serialize<T>(T obj);

        T Deserialize<T>(byte[] bytes);
    }
}