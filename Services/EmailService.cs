namespace Blog.Services;

public class EmailService
{
	public bool Send(
		string toName,
		string toEmail,
		string subject,
		string body,
		string fromName = "Equipe Freitas",
		string fromEmail = "rfcontatosvia@gmail.com")
	{
		return true;
	}
}