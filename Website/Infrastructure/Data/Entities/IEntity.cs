using System;

namespace Website.Infrastructure.Data.Entities {
    public interface IEntity {
        int Id { get; }
    }

    public interface IDatabaseEntity : IEntity {
        DateTime CreateDate { get; set; }
        DateTime? UpdateDate { get; set; }
    }
}
