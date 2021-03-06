namespace FasterMate.ViewModels.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    public class DateValidationAttribute : ValidationAttribute
    {
        private readonly int minYear;

        public DateValidationAttribute(int minYear)
        {
            this.minYear = minYear;
            this.ErrorMessage = $"The Birth date should be between {minYear} and the current date";
        }

        public override bool IsValid(object value)
        {
            if (value is DateTime dateValue)
            {
                return DateTime.Compare(dateValue, DateTime.Now) <= 0 && dateValue.Year >= this.minYear;
            }

            return false;
        }
    }
}
