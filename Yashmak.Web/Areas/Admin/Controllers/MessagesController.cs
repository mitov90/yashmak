namespace Yashmak.Web.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Yashmak.Data;
    using Yashmak.Data.Models;
    using Yashmak.Web.Areas.Admin.Controllers.Base;
    using Yashmak.Web.Areas.Admin.ViewModels.Users;

    public class MessagesController : AdminController
    {
        public MessagesController(IYashmakData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.View(this.Data.Messages.All().ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Message message = this.Data.Messages.GetById(id);
            if (message == null)
            {
                return this.HttpNotFound();
            }

            return View(message);
        }

        public ActionResult Create()
        {
            this.ViewBag.users =
                this.Data.Users.All()
                    .Project()
                    .To<UserDropdownModel>()
                    .Select(
                        user => new SelectListItem
                            {
                                Text = user.UserName, 
                                Value = user.Id
                            }).ToList();

            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "Id,Content,IsSeen,ReceiverId,IsDeleted,DeletedOn,CreatedOn,ModifiedOn")
            ] Message message)
        {
            if (this.ModelState.IsValid)
            {
                this.Data.Messages.Add(message);
                this.Data.SaveChanges();
                return this.RedirectToAction("Index");
            }

            return this.View(message);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Message message = this.Data.Messages.GetById(id);
            if (message == null)
            {
                return this.HttpNotFound();
            }

            return this.View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "Id,Content,IsSeen,ReceiverId,IsDeleted,DeletedOn,CreatedOn,ModifiedOn")
            ] Message message)
        {
            if (this.ModelState.IsValid)
            {
                this.Data.Messages.Update(message);
                this.Data.SaveChanges();
                return this.RedirectToAction("Index");
            }

            return this.View(message);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Message message = this.Data.Messages.GetById(id);
            if (message == null)
            {
                return this.HttpNotFound();
            }

            return View(message);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = this.Data.Messages.GetById(id);
            this.Data.Messages.Delete(message);
            this.Data.SaveChanges();
            return this.RedirectToAction("Index");
        }
    }
}