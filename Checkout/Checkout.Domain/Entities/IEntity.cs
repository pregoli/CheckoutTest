﻿using System;

namespace Checkout.Domain.Entities
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
