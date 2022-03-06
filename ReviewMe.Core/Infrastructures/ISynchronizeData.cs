namespace ReviewMe.Core.Infrastructures;

public interface ISynchronizeData
{
    void SendRequestToRefreshTable(TableName tableName);
}