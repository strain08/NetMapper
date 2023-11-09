using NetMapper.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetMapper.Services;

public class PoolingService
{
    private const int POLL_INTERVAL_MSEC = 5000;
    private readonly IDriveListService driveListService;
    private readonly IUpdateModelState updateModelState;
    private readonly IUpdateSystemState updateSystemState;

    
#nullable disable
    public PoolingService() { }
#nullable restore
    
    //CTOR
    public PoolingService(
        IDriveListService driveListService,
        IUpdateModelState updateModel,
        IUpdateSystemState updateSystem)
    {
        this.driveListService = driveListService;
        updateModelState = updateModel;
        updateSystemState = updateSystem;
        // Get share and mapping states into model at regular intervals
        Task.Run(() => StateLoop(POLL_INTERVAL_MSEC));
    }

    private async void StateLoop(int timeMilliseconds)
    {
        List<Task> taskList = new();
        while (true)
        {
            foreach (var m in driveListService.DriveCollection)
                taskList.Add(Task.Run(() =>
                {
                    updateModelState.Update(m);
                    updateSystemState.Update(m);
                }));
            await Task.WhenAll(taskList);
            taskList.Clear();
            Thread.Sleep(timeMilliseconds);
        }
    }
}