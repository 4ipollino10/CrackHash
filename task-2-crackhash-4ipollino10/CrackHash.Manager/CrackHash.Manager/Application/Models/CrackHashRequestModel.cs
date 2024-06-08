using JetBrains.Annotations;

namespace CrackHash.Manager.Application.Models;

/// <summary>
/// Запрос пользователя на взлом слова
/// </summary>
[PublicAPI]
public sealed record CrackHashRequestModel
{
    /// <summary>
    /// Захешированное слово в MD5
    /// </summary>
    public string Hash { get; set; } = null!;

    /// <summary>
    /// Максимальная длина слова
    /// </summary>
    public int MaxLength { get; set; }
}