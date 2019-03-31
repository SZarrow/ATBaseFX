using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using ATBase.Core;
using Newtonsoft.Json;

namespace ATBase.Validation
{
    public abstract class ValidateModel : IModelValidation
    {
        private Boolean? _isValid = null;
        private static readonly ValidateResult DefaultValidateResult = new ValidateResult(true, null);

        protected ValidateModel() { }

        [JsonIgnore]
        public Boolean IsValid
        {
            get
            {
                if (_isValid == null)
                {
                    var validateResult = this.Validate();
                    if (!validateResult.Success)
                    {
                        this.ErrorMessage = validateResult.Message;
                        _isValid = false;
                    }
                    else
                    {
                        _isValid = this.AttributeValidate();
                    }
                }

                return _isValid.Value;
            }
        }

        [JsonIgnore]
        public String ErrorMessage { get; private set; }

        public virtual ValidateResult Validate() => DefaultValidateResult;

        private Boolean AttributeValidate()
        {
            var properties = from p in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                             where p.CustomAttributes != null
                             select p;

            foreach (var p in properties)
            {
                var cusAttrs = p.GetCustomAttributes();
                foreach (var cusAttr in cusAttrs)
                {
                    var validationAttr = cusAttr as ValidationAttribute;
                    if (validationAttr != null)
                    {
                        try
                        {
                            var isValid = validationAttr.IsValid(p.XGetValue(this));
                            if (!isValid)
                            {
                                this.ErrorMessage = validationAttr.ErrorMessage;
                                return isValid;
                            }
                        }
                        catch (Exception ex)
                        {
                            this.ErrorMessage = ex.Message;
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }

    public struct ValidateResult : IEquatable<ValidateResult>
    {
        public ValidateResult(Boolean success, String message)
        {
            this.Success = success;
            this.Message = message;
        }

        public Boolean Success { get; set; }
        public String Message { get; set; }

        public Boolean Equals(ValidateResult other)
        {
            return this.GetHashCode() == other.GetHashCode();
        }
    }
}
