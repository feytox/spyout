#nullable enable
public abstract class ItemHandler
{
    protected readonly ItemStack Stack;

    protected ItemHandler(ItemStack stack) => Stack = stack;
    
    /// <summary>
    /// Использует предмет персонажем.
    /// </summary>
    /// <returns>true, если нужно проиграть звук и прервать следующие взаимодействия</returns>
    public abstract bool UseItem(ICharacter character);
}