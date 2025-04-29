namespace RealEstateApp.Models
{
    public class ChangeRoleViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string CurrentRole { get; set; }
        public string NewRole { get; set; }
        public List<string> AvailableRoles { get; set; } = new List<string>();
    }
}
