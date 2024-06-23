namespace CustomerTestApp.WPF.Models
{
    /// <summary>
    /// The customer class holds information about a customer.
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// The Id of the customer.
        /// </summary>
        public int Id { get; set; }

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
        /// The customer's shallow clone method.
        /// </summary>
        /// <returns>A shallow clone of customer's object</returns>
        public Customer Clone()
        {
            return (Customer)this.MemberwiseClone();
        }
    }
}