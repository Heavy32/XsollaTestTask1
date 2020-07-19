using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XsollaTestTask1.Models
{
    public class CreditCard : IValidatableObject
    {
        private string number;
        [Required]
        public string Number { get => number; set { number = Regex.Replace(value, @"\s+", ""); } }
        [Required]
        public string HolderName { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
        [Required]
        public string CVV { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Number.All(char.IsDigit))
            {
                yield return new ValidationResult("Card number contains wrong symbols");
            }

            if (Number.Length != 13 && Number.Length != 15 && Number.Length != 16 && Number.Length != 18 && Number.Length != 19)
            {  
                yield return new ValidationResult("Incorrect numbers' count");
            }

            if(DateTime.Now >= ExpirationDate)
            {
                yield return new ValidationResult("Card is not support due to the expiration date has been out");
            }

            if (!CVV.All(char.IsDigit))
            {
                yield return new ValidationResult("CVV/CVC contains wrong symbols");
            }

            if(CVV.Length != 3)
            {
                yield return new ValidationResult("CVV/CVC must have 3 symbols");
            }

            if (!LuhnValidation())
            {
                yield return new ValidationResult("Invalid card number");
            }
        }

        public bool LuhnValidation()
        {
            var NumberInArray = Number.Select(digit => (int)Char.GetNumericValue(digit));

            int sum = 0;
            for (int i = Number.Length % 2; i < Number.Length; i += 2)
            {
                if (NumberInArray.ElementAt(i) * 2 > 9)
                {
                    sum += (NumberInArray.ElementAt(i) * 2 - 9);
                }
                else
                {
                    sum += NumberInArray.ElementAt(i) * 2;
                }
            }

            for (int i = (Number.Length % 2 == 0) ? 1 : 0; i < Number.Length; i += 2)
            {
                sum += NumberInArray.ElementAt(i);
            }

            return (sum % 10 == 0);
        }
    }
}
