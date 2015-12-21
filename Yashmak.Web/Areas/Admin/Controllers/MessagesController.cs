using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using Yashmak.Data;
using Yashmak.Data.Models;
using Yashmak.Web.Areas.Admin.Controllers.Base;
using Yashmak.Web.Areas.Admin.ViewModels.Users;

namespace Yashmak.Web.Areas.Admin.Controllers
{
    public class MessagesController : AdminController
    {
        public MessagesController(IYashmakData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return View(Data.Messages.All().ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var message = Data.Messages.GetById(id);
            if (message == null)
            {
                return HttpNotFound();
            }

            return View(message);
        }

        public ActionResult Create()
        {
            ViewBag.users =
                Data.Users.All()
                    .ProjectTo<UserDropdownModel>()
                    .Select(
                        user => new SelectListItem
                        {
                            Text = user.UserName,
                            Value = user.Id
                        }).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "Id,Content,IsSeen,ReceiverId,IsDeleted,DeletedOn,CreatedOn,ModifiedOn")
            ] Message message)
        {
            if (ModelState.IsValid)
            {
                Data.Messages.Add(message);
                Data.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(message);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var message = Data.Messages.GetById(id);
            if (message == null)
            {
                return HttpNotFound();
            }

            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "Id,Content,IsSeen,ReceiverId,IsDeleted,DeletedOn,CreatedOn,ModifiedOn")
            ] Message message)
        {
            if (ModelState.IsValid)
            {
                Data.Messages.Update(message);
                Data.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(message);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var message = Data.Messages.GetById(id);
            if (message == null)
            {
                return HttpNotFound();
            }

            return View(message);
        }
           
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var message = Data.Messages.GetById(id);
            Data.Messages.Delete(message);
            Data.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}