using System.ComponentModel.DataAnnotations.Schema;

namespace AnimeNotificationsBot.Common.Interfaces
{
    public interface IHasUniqueProperty
    {
        [NotMapped] 
        object UniqueProperty { get; }
    }
}
