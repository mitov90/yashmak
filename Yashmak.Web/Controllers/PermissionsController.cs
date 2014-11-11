namespace Yashmak.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using Yashmak.Data;
    using Yashmak.Data.Models;
    using Yashmak.Web.Models.Permission;

    public class PermissionsController : Controller
    {
        private readonly YashmakDbContext context;

        public PermissionsController(YashmakDbContext context)
        {
            this.context = context;
        }

        public ActionResult Share(int filenodeid)
        {
            var userId = this.User.Identity.GetUserId();
            var fileNode =
                this.context.Files.Where(f => f.Id == filenodeid)
                    .Include(f => f.Permission)
                    .FirstOrDefault();
            if (fileNode.UserId != userId)
            {
                return this.Content("Not your file/folder!");
            }

            var permission = this.CreateOrGetPermission(fileNode);
            var userWithAccess = this.context.ShareNames.Where(s => s.PermissionId == permission.Id);
            var people = string.Join(", ", userWithAccess.Select(u => u.Username));

            var fileView = new PermissionViewModel
                {
                    FileName = fileNode.FileName, 
                    Id = filenodeid, 
                    AccessType = permission.AccessType, 
                    People = people
                };

            return this.View(fileView);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Share(PermissionViewModel permission)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(permission);
            }

            var userId = this.User.Identity.GetUserId();
            var curFile =
                this.context.Files.FirstOrDefault(f => f.UserId == userId && f.Id == permission.Id);

            var authorizedUsers = new HashSet<ShareName>();

            if (!string.IsNullOrWhiteSpace(permission.People))
            {
                var usernames = permission.People.Split(
                    new[] { ',', ' ' }, 
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (var username in usernames)
                {
                    var shareUser = new ShareName { Username = username };
                    this.context.ShareNames.Add(shareUser);
                    authorizedUsers.Add(shareUser);
                }
            }

            if (curFile != null)
            {
                curFile.Permission = new Permission
                    {
                        AccessType = permission.AccessType, 
                        AuthorizedUsers = authorizedUsers
                    };
                this.context.SaveChanges();
                return this.RedirectToAction(
                    "Index", 
                    "Files", 
                    new { filenodeid = curFile.ParentId });
            }

            this.ModelState.AddModelError("Can not change this files!", "Permission denied!");
            return this.View(permission);
        }

        [NonAction]
        private Permission CreateOrGetPermission(File fileNode)
        {
            if (fileNode.Permission != null)
            {
                return fileNode.Permission;
            }

            var permission = new Permission { AccessType = AccessType.Private, Id = fileNode.Id };

            // authorize owner
            var autorizeOwner = new ShareName { Username = fileNode.User.UserName };
            this.context.ShareNames.Add(autorizeOwner);
            permission.AuthorizedUsers.Add(autorizeOwner);

            fileNode.Permission = permission;
            this.context.Permissions.Add(permission);
            this.context.SaveChanges();
            return permission;
        }

        [NonAction]
        private Permission GetPermissionForId(File fileNode)
        {
            Permission permission;
            while (true)
            {
                if (fileNode.Permission != null)
                {
                    permission = fileNode.Permission;
                    break;
                }

                if (fileNode.ParentId == null)
                {
                    permission = fileNode.Permission;
                    break;
                }

                fileNode = fileNode.Parent;
            }

            return permission ??
                   (new Permission { AccessType = AccessType.Private, Id = fileNode.Id });
        }
    }
}