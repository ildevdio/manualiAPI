namespace ManualiAPI.DTO;

public class UpdateUserDTO
{
    private string _username { get; set; }
    private string _email  { get; set; }
}

public class DeleteUserDTO
{
    private string _username  { get; set; }
    private string _email   { get; set; }
}