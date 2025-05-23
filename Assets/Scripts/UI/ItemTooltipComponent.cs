using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ItemTooltipComponent : MonoBehaviour
{
    private static readonly int Activate = Animator.StringToHash("Activate");

    [SerializeField] private TooltipType _tooltipType;

    private Animator _tooltipAnimator;

    void Start()
    {
        var playerInventory = PlayerController.GetInstance().Inventory;
        playerInventory.OnActiveItemChange += OnActiveItemChange;

        _tooltipAnimator = GetComponent<Animator>();
    }

    private void OnActiveItemChange(ItemStack stack)
    {
        if (stack is null || stack.IsEmpty)
            return;

        if (_tooltipType == TooltipType.Attack)
        {
            if (stack.Item.InteractHandler == ItemHandlerType.Weapon)
                TriggerTooltip();
            return;
        }

        TriggerTooltip();
    }

    private void TriggerTooltip()
    {
        _tooltipAnimator.SetTrigger(Activate);
    }
}

public enum TooltipType : byte
{
    UseItem = 0,
    DropItem = 1,
    Attack = 2
}