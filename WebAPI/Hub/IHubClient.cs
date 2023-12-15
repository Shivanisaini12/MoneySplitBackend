namespace WebAPI.Hub
{
    public interface IHubClient
    {
        Task BroadcastMessage();
    }
}
