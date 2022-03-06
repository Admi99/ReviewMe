using ReviewMe.Core.Enums;
using ReviewMe.Core.Infrastructures;

namespace ReviewMe.API.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class SynchronizeDataController : Controller
{
    private readonly ISynchronizeData _synchronizeData;

    public SynchronizeDataController(ISynchronizeData synchronizeData)
    {
        _synchronizeData = synchronizeData;
    }

    [HttpPost]
    public void SynchronizeEmployeeTable()
        => _synchronizeData.SendRequestToRefreshTable(TableName.Employees);
}