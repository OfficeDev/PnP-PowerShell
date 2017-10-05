using System;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    /// <summary>
    /// Allows passing UserCustomAction objects in pipelines
    /// </summary>
    public sealed class UserCustomActionPipeBind
    {
        /// <summary>
        /// Id of the UserCustomAction
        /// </summary>
        public Guid? Id { get; private set; }

        /// <summary>
        /// Name of the UserCustomAction
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The UserCustomAction that is piped in
        /// </summary>
        public UserCustomAction UserCustomAction { get; private set; }

        /// <summary>
        /// Accepts an id of a UserCustomAction
        /// </summary>
        /// <param name="guid">Id of the UserCustomAction</param>
        public UserCustomActionPipeBind(Guid guid)
        {
            Id = guid;
        }

        /// <summary>
        /// Accepts a UserCustomAction to be passed in
        /// </summary>
        /// <param name="userCustomAction">UserCustomAction itself</param>
        public UserCustomActionPipeBind(UserCustomAction userCustomAction)
        {
            UserCustomAction = userCustomAction;
        }

        /// <summary>
        /// Accepts a name or id to be passed in
        /// </summary>
        /// <param name="id">Name or id of the UserCustomAction</param>
        public UserCustomActionPipeBind(string id)
        {
            // Added Guid checking first for backwards compatibility
            if (Guid.TryParse(id, out Guid result))
            {
                Id = result;
            }
            else
            {
                Name = id;
            }
        }
    }
}
