using Microsoft.SharePoint.Client;
using System;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class AppPipeBind
    {
        private readonly string _title;
        private readonly AppInstance _appInstance;
        private readonly Guid _id;

        public AppPipeBind(AppInstance instance)
        {
            _appInstance = instance;
        }


        public AppPipeBind(Guid guid)
        {
            _id = guid;
        }

        public AppPipeBind(string id)
        {
            if (!Guid.TryParse(id, out _id))
            {
                _title = id;
            }

        }

        public Guid Id => _id;

        public string Title => _title;

        public AppInstance Instance => _appInstance;

        internal AppInstance GetAppInstance(Web web)
        {
            AppInstance instance = null;
            if (Instance != null)
            {
                instance = Instance;
                return instance;
            }
            if (Id != Guid.Empty)
            {
                instance = web.GetAppInstanceById(Id);
                web.Context.Load(instance);
                web.Context.ExecuteQueryRetry();
                return instance;
            }
            if (!string.IsNullOrEmpty(Title))
            {

                var instances = web.GetAppInstances();
                web.Context.Load(instances);
                web.Context.ExecuteQueryRetry();
                foreach (var inst in instances)
                {
                    if (inst.Title == Title)
                    {
                        return inst;
                    }
                }
            }
            return null;
        }
    }
}
