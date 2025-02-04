﻿using PayVerse.Domain.Primitives;

namespace PayVerse.Domain.Events;

/// <summary> 
/// Represents an abstract base class for domain events. 
/// </summary> 
/// <param name="Id">The unique identifier of the domain event.</param>
public abstract record DomainEvent(Guid Id) : IDomainEvent;