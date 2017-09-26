namespace PurgeCDN.Web.Models
{
    public class DialogViewModel
    {
        public int NodeId { get; set; }
        public string NodeName { get; set; }
        public string ErrorMessage { get; set; }
        public string Status { get; set; } // ok, error
    }
}
