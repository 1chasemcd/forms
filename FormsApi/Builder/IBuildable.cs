using System;

namespace FormsApi.Builder;

public interface IBuildable<T>
{
    T Build();
}
