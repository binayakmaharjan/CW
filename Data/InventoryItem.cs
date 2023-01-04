using System.ComponentModel.DataAnnotations;

namespace MotorInventorySystem.Data;
public class InventoryItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "Please provide the task name.")]
    public string ItemName { get; set; }

    [Required(ErrorMessage = "Please provide a Quantity.")]
    public int Quantity { get; set; }

    public string AddedBy { get; set; }
}
