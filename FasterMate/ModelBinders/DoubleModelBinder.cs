﻿namespace FasterMate.ModelBinders
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System.Globalization;

    public class DoubleModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext
                .ValueProvider
                .GetValue(bindingContext.ModelName);

            if (valueResult != ValueProviderResult.None && !String.IsNullOrWhiteSpace(valueResult.FirstValue))
            {
                double actualValue = 0;
                bool success = false;

                try
                {
                    string doubleValue = valueResult.FirstValue;
                    doubleValue = doubleValue.Replace(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
                    doubleValue = doubleValue.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);

                    actualValue = Convert.ToDouble(doubleValue, CultureInfo.CurrentCulture);
                    success = true;
                }
                catch (FormatException fe)
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, fe, bindingContext.ModelMetadata);
                }

                if (success)
                {
                    bindingContext.Result = ModelBindingResult.Success(actualValue);
                }
            }

            return Task.CompletedTask;
        }
    }
}
