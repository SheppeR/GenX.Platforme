using Microsoft.Extensions.Options;

namespace GenX.Common.Options;

public interface IWritableOptions<out T> : IOptionsSnapshot<T> where T : class, new()
{
    void Update(Action<T> applyChanges);
}