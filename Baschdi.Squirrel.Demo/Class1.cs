using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EvilBaschdi.Core;
using NuGet;
using Squirrel;

namespace Baschdi.Squirrel.Demo
{
    public class CheckForUpdates : ICheckForUpdates
    {
        private readonly IUpdateManager _updateManager;

        public CheckForUpdates(IUpdateManager updateManager)
        {
            _updateManager = updateManager ?? throw new ArgumentNullException(nameof(updateManager));
        }


        public async Task<UpdateInfo> TaskValue()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.Expect100Continue = true;
            var checkForUpdate = await _updateManager.CheckForUpdate();
            return checkForUpdate;
        }
    }

    public interface IUpdate : ITaskRun
    { }

    public class Update : IUpdate
    {
        private readonly IUpdateManager _updateManager;
        private readonly ICheckForUpdates _checkForUpdates;


        public Update(IUpdateManager updateManager, ICheckForUpdates checkForUpdates)
        {
            _updateManager = updateManager ?? throw new ArgumentNullException(nameof(updateManager));
            _checkForUpdates = checkForUpdates ?? throw new ArgumentNullException(nameof(checkForUpdates));
        }
  

        public async Task TaskRun()
        {SemanticVersion newVersion = null;
            var update = _checkForUpdates.TaskValue().Result;
            
            if (update.ReleasesToApply.Any())
            {
                newVersion = update.FutureReleaseEntry.Version;
                
                await _updateManager.UpdateApp();

                
            }
        }
    }

    public interface ICheckForUpdates : ITaskValue<UpdateInfo>
    {
    }

   
}