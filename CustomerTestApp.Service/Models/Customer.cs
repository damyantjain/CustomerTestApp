using System.ComponentModel.DataAnnotations;

namespace CustomerTestApp.Service.Models
{
    /// <summary>
    /// The customer class holds information about a customer.
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// The Id of the customer.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// The first name of the customer.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the customer.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The email of the customer.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The discount of the customer.
        /// </summary>
        public int Discount { get; set; }


        /// <summary>
        /// The boolean value that indicates if the customer can be removed.
        /// </summary>
        public bool CanBeRemoved { get; set; }
    }
}
