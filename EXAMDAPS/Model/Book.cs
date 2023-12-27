using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXAMDAPS.Model
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorId { get; set; }
        public string CategoryId { get; set; }
        public int Pages { get; set; }
        public int Cost { get; set; }
    }
}
