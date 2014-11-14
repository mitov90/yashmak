namespace Yashmak.Web.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    using Yashmak.Data;
    using Yashmak.Data.Models;
    using Yashmak.Web.Controllers.Base;
    using Yashmak.Web.ViewModels.Permission;

    public class PermissionsController : PermissionBaseController
    {
        public PermissionsController(IYashmakData data)
            : base(data)
        {
        }

        public ActionResult Share(int filenodeid)
        {
            var fileNode =
                this.Data.Files.All().Where(f => f.Id == filenodeid)
                    .Include(f => f.Permission)
                    .FirstOrDefault();
            if (fileNode != null && fileNode.UserId != this.UserId)
            {
                return this.Content("Not your file/folder!");
            }

            var fileView = this.GetPermissionViewModel(filenodeid, fileNode);

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

            var curFile =
                this.Data.Files.All()
                    .FirstOrDefault(f => f.UserId == this.UserId && f.Id == permission.Id);

            var authorizedUsers = this.GetAuthorizedUsers(permission);

            if (curFile == null)
            {
                this.ModelState.AddModelError("Can not change this files!", "Permission denied!");
                return this.View(permission);
            }

            curFile.Permission = new Permission
                {
                    AccessType = permission.AccessType, 
                    AuthorizedUsers = authorizedUsers
                };
            this.Data.SaveChanges();
            return this.RedirectToAction(
                "Index", 
                "Files", 
                new { filenodeid = curFile.ParentId });
        }
    }
}