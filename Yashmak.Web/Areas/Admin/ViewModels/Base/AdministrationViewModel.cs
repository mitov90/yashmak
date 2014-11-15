namespace Yashmak.Web.Areas.Admin.ViewModels.Base
{
    using System;
    using System.Web.Mvc;

    public abstract class AdministrationViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public DateTime CreatedOn { get; set; }
        
        [HiddenInput(DisplayValue = false)]
        public DateTime? ModifiedOn { get; set; }
    }
}