using System;
using System.Collections.Generic;

namespace Roommates.Models
{
    public class ChoreReport
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ChoreCount { get; set; }
    }
}