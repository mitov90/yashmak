﻿namespace Yashmak.Web.Areas.Admin.Controllers.Base
{
    using System.Collections;
    using System.Data.Entity;
    using System.Web.Mvc;

    using AutoMapper;

    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;

    using Yashmak.Data;
    using Yashmak.Data.Common.Models;
    using Yashmak.Web.Areas.Admin.ViewModels.Base;

    public abstract class KendoGridAdministrationController : AdminController
    {
        protected KendoGridAdministrationController(IYashmakData data)
            : base(data)
        {
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var result = this.GetData().ToDataSourceResult(request);

            return this.Json(result);
        }

        [NonAction]
        protected virtual T Create<T>(object model) where T : class
        {
            if (model == null || !this.ModelState.IsValid)
            {
                return null;
            }

            var dbModel = Mapper.Map<T>(model);
            this.ChangeEntityStateAndSave(dbModel, EntityState.Added);
            return dbModel;
        }

        protected abstract T GetById<T>(object id) where T : class;

        protected abstract IEnumerable GetData();

        protected JsonResult GridOperation<T>(T model, [DataSourceRequest] DataSourceRequest request)
        {
            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }

        [NonAction]
        protected virtual void Update<TModel, TViewModel>(TViewModel model, object id) where TModel : AuditInfo
            where TViewModel : AdministrationViewModel
        {
            if (model != null && this.ModelState.IsValid)
            {
                var dbModel = this.GetById<TModel>(id);
                Mapper.Map(model, dbModel);
                this.ChangeEntityStateAndSave(dbModel, EntityState.Modified);
                model.ModifiedOn = dbModel.ModifiedOn;
            }
        }

        private void ChangeEntityStateAndSave(object dbModel, EntityState state)
        {
            var entry = this.Data.Context.Entry(dbModel);
            entry.State = state;
            this.Data.SaveChanges();
        }
    }
}