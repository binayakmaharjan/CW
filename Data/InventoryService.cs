using System.Text.Json;


namespace MotorInventorySystem.Data;

public static class InventoryService
{
    private static void SaveAll(Guid userId, List<InventoryItem> InventoryItem)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string appInventoryFilePath = Utils.GetInventoryFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(InventoryItem);
        File.WriteAllText(appInventoryFilePath, json);
    }


    public static List<InventoryItem> GetAll(Guid userId)
    {
        string appInventoryFilePath = Utils.GetInventoryFilePath();
        if (!File.Exists(appInventoryFilePath))
        {
            return new List<InventoryItem>();
        }

        var json = File.ReadAllText(appInventoryFilePath);

        return JsonSerializer.Deserialize<List<InventoryItem>>(json);
    }



    public static List<InventoryItem> Create(Guid userId, string addedBy, string itemName, int quantity)
    {
        if (quantity <= 0)
        {
            throw new Exception("Cannot Add Item with 0 quantity");

        }
        else if (string.IsNullOrEmpty(itemName))
        {
            throw new Exception("Item Name cannot be empty");

        }
        else if (itemName.GetType() != typeof(string))
        {
            throw new Exception("Item Name must be string");
        }
        else if (quantity.GetType() != typeof(int))
        {
            throw new Exception("Quantity must be integer");
        }
        else
        {
            List<InventoryItem> InventoryItem = GetAll(userId);
            InventoryItem.Add(new InventoryItem
            {
                AddedBy = addedBy,
                ItemName = itemName,
                Quantity = quantity,
            });
            SaveAll(userId, InventoryItem);
            return InventoryItem;

        }

    }



    public static List<InventoryItem> Update(Guid userId, Guid id, string itemName, int quantity)
    {

        List<InventoryItem> items = GetAll(userId);
        InventoryItem itemUpdate = items.FirstOrDefault(x => x.Id == id);

        if (itemUpdate == null)
        {
            throw new Exception("Item not found.");
        }
        else if (quantity <= 0)
        {
            throw new Exception("Cannot Add Item with 0 quantity");
        }
        else if (string.IsNullOrEmpty(itemName))
        {
            throw new Exception("Item Name cannot be empty");
        }
        else if (itemName.GetType() != typeof(string))
        {
            throw new Exception("Item Name must be string");
        }
        else if (quantity.GetType() != typeof(int))
        {
            throw new Exception("Quantity must be integer");
        }
        else
        {
            itemUpdate.ItemName = itemName;
            itemUpdate.Quantity = quantity;
            SaveAll(userId, items);
            return items;

        }


    }

    public static List<InventoryItem> WithdrawItem(Guid userId, Guid id, string itemName, int quantity)
    {
        DateTime currentTime = DateTime.Now;


        if (currentTime.Hour >= 9 && currentTime.Hour < 16 && currentTime.DayOfWeek >= DayOfWeek.Monday && currentTime.DayOfWeek <= DayOfWeek.Friday)
        {


            List<InventoryItem> items = GetAll(userId);
            InventoryItem itemUpdate = items.FirstOrDefault(x => x.Id == id);

            if (itemUpdate == null)
            {
                throw new Exception("Item not found.");
            }
            else if (quantity <= 0)
            {
                throw new Exception("Quantity cannot be 0 or less");
            }
            if (quantity > itemUpdate.Quantity)
            {
                throw new Exception("Withdraw quantity over the limit");
            }
            else
            {
                itemUpdate.ItemName = itemName;
                itemUpdate.Quantity = itemUpdate.Quantity - quantity;
                SaveAll(userId, items);
                return items;
            }



        }
        else
        {
            throw new Exception("The user cannot withdraw during this time.");
        }
    }
    public static List<InventoryItem> RejectWithdrawItem(Guid userId, Guid id, string itemName, int quantity)
    {
        List<InventoryItem> items = GetAll(userId);
        InventoryItem itemUpdate = items.FirstOrDefault(x => x.Id == id);

        if (itemUpdate == null)
        {
            throw new Exception("Item not found.");
        }
        else
        {
            itemUpdate.ItemName = itemName;
            itemUpdate.Quantity += quantity;
            SaveAll(userId, items);
            return items;
        }
    }
    public static List<InventoryItem> CancelWithdrawItem(Guid userId, Guid id, string itemName, int quantity)
    {
        List<InventoryItem> items = GetAll(userId);
        InventoryItem itemUpdate = items.FirstOrDefault(x => x.Id == id);

        if (itemUpdate == null)
        {
            throw new Exception("Item not found.");
        }

        itemUpdate.ItemName = itemName;
        itemUpdate.Quantity = itemUpdate.Quantity + quantity;
        SaveAll(userId, items);
        return items;
    }


    public static List<InventoryItem> Delete(Guid userId, Guid id)
    {
        List<InventoryItem> items = GetAll(userId);
        InventoryItem itemUpdate = items.FirstOrDefault(x => x.Id == id);

        if (itemUpdate == null)
        {
            throw new Exception("Item not found.");
        }

        items.Remove(itemUpdate);
        SaveAll(userId, items);
        return items;
    }










}
