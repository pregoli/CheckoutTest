using System;

namespace Checkout.Domain.Entities
{
    public interface IEntity
    {
        Guid TransactionId { get; set; }
    }
}
