using System.Collections.Generic;

namespace GeminiBacklog.Models
{
    public class BAUTaskModel : IssueModel
    {
        public IEnumerable<string> AssignedTo { get; set; }
        public int StatusOrder { get; set; }
        public string DueSoon { get; set; }
        public string ITOwner { get; set; }
    }
}