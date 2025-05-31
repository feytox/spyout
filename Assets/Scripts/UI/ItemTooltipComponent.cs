using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ItemTooltipComponent : MonoBehaviour
{
    private static readonly int Active = Animator.StringToHash("Active");

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
        {
            SetActive(false);
            return;
        }

        SetActive(_tooltipType != TooltipType.Attack || stack.Item.InteractHandler == ItemHandlerType.Weapon);
    }

    private void SetActive(bool state)
    {
        _tooltipAnimator.SetBool(Active, state);
    }
}

public enum TooltipType : byte
{
    UseItem = 0,
    DropItem = 1,
    Attack = 2
}