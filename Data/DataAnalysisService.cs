using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorInventorySystem.Data;

public static class DataAnalysisService
{
    public static List<DataAnalysisDTO> DataAnalysisDTO(Guid userId)
    {
        List<DataAnalysisDTO> dataAnalysisDTO = new List<DataAnalysisDTO>();
        var data = ApprovedService.GetAll(userId);
        var filterData = data.Where(x => x.Id == userId).ToList();
        foreach (var item in filterData)
        {
            dataAnalysisDTO.Add(new DataAnalysisDTO { TitleName = item.ItemName, ValueCount = item.Quantity });
        }
        return dataAnalysisDTO;
    }
}

public class DataAnalysisDTO
{
    public string TitleName { get; set; }
    public int ValueCount { get; set; }
}
