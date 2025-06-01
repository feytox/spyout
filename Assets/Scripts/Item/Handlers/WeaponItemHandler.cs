using JetBrains.Annotations;

public class WeaponItemHandler : ItemHandler
{
    public WeaponItemHandler([NotNull] ItemStack stack) : base(stack)
    {
    }

    /// Игроки, возможно, не поймут, что оружия нужно использовать на другую кнопку.
    /// Поэтому имеет смысл добавить возможность атаковать при взаимодействии
    /// <inheritdoc cref="ItemHandler.UseItem"/>
    public override bool UseItem(ICharacter character)
    {
        if (character is PlayerController player)
            return player.AttackInRange();

        return false;
    }
}