#nullable enable

/// <summary>
/// Интерфейс, представляющий инвентарь с фиксированным размером.
/// </summary>
public interface IInventory
{
    /// <summary>
    /// Размер инвентаря, определяющий количество доступных слотов.
    /// </summary>
    public int Size { get; }
    
    /// <summary>
    /// Индексатор для доступа к предмету в указанном слоте.
    /// </summary>
    /// <param name="slot">Индекс слота, к которому требуется доступ.</param>
    /// <returns>
    /// Предмет в указанном слоте или <c>null</c>, если слот пуст.
    /// </returns>
    public ItemStack? this[int slot] { get; }
    
    /// <summary>
    /// Устанавливает предмет в указанный слот.
    /// </summary>
    /// <param name="stack">Предмет, который нужно установить. Может быть <c>null</c>, чтобы очистить слот.</param>
    /// <param name="slot">Индекс слота, в который нужно установить предмет.</param>
    /// <returns>
    /// Предыдущий предмет, находившийся в указанном слоте, или <c>null</c>, если слот был пуст.
    /// </returns>
    public ItemStack? SetStack(ItemStack? stack, int slot);
    
    /// <summary>
    /// Удаляет предмет из указанного слота.
    /// </summary>
    /// <param name="slot">Индекс слота, из которого нужно удалить предмет.</param>
    /// <returns>
    /// Предыдущий предмет, находившийся в указанном слоте, или <c>null</c>, если слот был пуст.
    /// </returns>
    public ItemStack? PopStack(int slot) => SetStack(null, slot);
}