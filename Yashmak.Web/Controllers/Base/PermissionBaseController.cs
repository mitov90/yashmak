namespace Yashmak.Web.Controllers.Base
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Web.Mvc;

    using Yashmak.Data;
    using Yashmak.Data.Models;
    using Yashmak.Web.Infrastructure.Filters;
    using Yashmak.Web.ViewModels.Permission;

    [Log]
    [Authorize]
    public abstract class PermissionBaseController : BaseController
    {
        protected PermissionBaseController(IYashmakData data)
            : base(data)
        {
        }

        [NonAction]
        protected PermissionViewModel GetPermissionViewModel(int filenodeid, File fileNode)
        {
            var permission = this.CreateOrGetPermission(fileNode);
            var userWithAccess = fileNode.Permission.AuthorizedUsers;
            var people = string.Join(", ", userWithAccess.Select(u => u.Username));

            var fileView = new PermissionViewModel
                {
                    FileName = fileNode.FileName, 
                    Id = filenodeid, 
                    AccessType = permission.AccessType, 
                    People = people
                };
            return fileView;
        }

        [NonAction]
        protected HashSet<ShareName> GetAuthorizedUsers(PermissionViewModel permission)
        {
            var authorizedUsers = new HashSet<ShareName>();

            if (!string.IsNullOrWhiteSpace(permission.People))
            {
                var usernames = permission.People.Split(
                    new[] { ',', ' ' }, 
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (
                    var shareUser in
                        usernames.Select(username => new ShareName { Username = username }))
                {
                    this.Data.Sharenames.Add(shareUser);
                    authorizedUsers.Add(shareUser);
                }
            }

            return authorizedUsers;
        }

        [NonAction]
        private Permission CreateOrGetPermission(File fileNode)
        {
            if (fileNode.Permission != null)
            {
                return fileNode.Permission;
            }

            var permission = new Permission { AccessType = AccessType.Private, Id = fileNode.Id };

            fileNode.Permission = permission;
            fileNode.Permission.AuthorizedUsers = new Collection<ShareName>();
            this.Data.Permissions.Add(permission);
            this.Data.SaveChanges();
            return permission;
        }
    }
}