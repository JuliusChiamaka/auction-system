using System;

namespace AuctionSystem.Domain.Entities.Base
{
    public interface IEntityBase
    {
        string Id { get; }
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}
