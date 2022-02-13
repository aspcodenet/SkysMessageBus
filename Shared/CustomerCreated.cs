using System;

namespace Shared
{
    public class CustomerCreated
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public DateTime DateTimeUtc { get; set; }
    }
}
