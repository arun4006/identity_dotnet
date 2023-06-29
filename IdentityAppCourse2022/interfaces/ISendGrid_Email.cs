namespace IdentityAppCourse2022.interfaces
{
    public interface ISendGrid_Email
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}
