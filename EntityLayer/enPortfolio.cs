using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EntityLayer
{
    public class enPortfolio
    {
        public class ContactFormModel
        {
            [Required(ErrorMessage = "Name is required")]
            [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email address")]
            public string Email { get; set; }

            [Phone(ErrorMessage = "Invalid phone number")]
            [StringLength(20)]
            public string Phone { get; set; }

            [Required(ErrorMessage = "Subject is required")]
            [StringLength(200, MinimumLength = 5, ErrorMessage = "Subject must be between 5 and 200 characters")]
            public string Subject { get; set; }

            [Required(ErrorMessage = "Message is required")]
            [StringLength(5000, MinimumLength = 10, ErrorMessage = "Message must be between 10 and 5000 characters")]
            public string Message { get; set; }

            [Display(Name = "Attachment")]
            public IFormFile Attachment { get; set; }
        }

        /// <summary>
        /// Error view model
        /// </summary>
        public class ErrorViewModel
        {
            public string RequestId { get; set; }

            public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        }
    }
}
