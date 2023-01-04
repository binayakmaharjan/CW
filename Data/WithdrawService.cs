using System.Text.Json;

namespace MotorInventorySystem.Data;

public static class WithdrawService
{
    private static void SaveAll(Guid userId, List<WithdrawItem> withdrawl)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string withdrawlFilePath = Utils.GetWithdrawlFilePath(userId);

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(withdrawl);
        File.WriteAllText(withdrawlFilePath, json);
    }



    public static List<WithdrawItem> GetAll(Guid userId)
    {
        string appInventoryFilePath = Utils.GetWithdrawlFilePath(userId);
        if (!File.Exists(appInventoryFilePath))
        {
            return new List<WithdrawItem>();
        }

        var json = File.ReadAllText(appInventoryFilePath);

        return JsonSerializer.Deserialize<List<WithdrawItem>>(json);
    }



    public static List<WithdrawItem> Create(Guid userId, Guid itemId, string itemName, int quantity, string takerName)
    {
        if (quantity <= 0)
        {
            throw new Exception("Withdraw cannot be 0 or less");
        }

        else
        {
            List<WithdrawItem> WithdrawItems = GetAll(userId);
            WithdrawItems.Add(new WithdrawItem
            {

                Quantity = quantity,
                TakenBy = userId,
                TakerName = takerName,
                ItemId = itemId,
                IsApproved = false,
                ItemName = itemName,


            });
            SaveAll(userId, WithdrawItems);
            return WithdrawItems;

        }


    }



    public static List<WithdrawItem> Delete(Guid userId, Guid id)
    {
        List<WithdrawItem> WithdrawItems = GetAll(userId);
        WithdrawItem itemDelete = WithdrawItems.FirstOrDefault(x => x.Id == id);

        if (itemDelete == null)
        {
            throw new Exception("Item not found.");
        }

        WithdrawItems.Remove(itemDelete);
        SaveAll(userId, WithdrawItems);
        return WithdrawItems;
    }

}
