using HouseRent.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HouseRent.Web.Validation
{
    public class ValidationDictionary : IValidation
    {
        //Adds validations in the ModelStateDictionary of the controller
        private ModelStateDictionary modelState;

        public ValidationDictionary(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

        #region IValidationDictionary Members

        //Checks the status of the dictionary
        public bool IsValid => modelState.IsValid;

        //Adds the exact errors
        public void Error(string key, string msg)
        {
            modelState.AddModelError(key, msg);
        }

        #endregion
    }
}
