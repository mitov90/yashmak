namespace Yashmak.Common
{
    using System.Linq;

    using Yashmak.Data.Models;

    public static class PermissionManager
    {
        public static Permission GetPermissionForId(File fileNode)
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

        public static bool CheckPermission(File fileNode, string curUserId, string curUserName)
        {
            Permission permission = GetPermissionForId(fileNode);
            return fileNode.UserId == curUserId ||
                   permission.AccessType == AccessType.Public ||
                   (permission.AccessType == AccessType.Custom &&
                    permission.AuthorizedUsers.Any(e => e.Username == curUserName));
        }

    }
}