using System;

namespace Core.Entity
{
    public interface IEntityTimeStamp
    {
        DateTime CreatedDateTime { get; set; }
        DateTime? ModifiedDateTime { get; set; }
    }
}