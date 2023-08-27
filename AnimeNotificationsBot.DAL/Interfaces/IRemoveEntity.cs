namespace AnimeNotificationsBot.DAL.Interfaces
{
    public interface IRemoveEntity: IEntity
    {
        public bool IsRemoved { get; set; }
    }
}
