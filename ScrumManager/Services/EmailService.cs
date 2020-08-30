using RestSharp;
using RestSharp.Authenticators;
using System;

namespace ScrumManager.Services
{
    public class EmailService
	{
		public static IRestResponse SendEmail(string to, string subject, string body)
		{
			RestClient client = new RestClient();
			client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator = new HttpBasicAuthenticator("api", Secrets.MailGunAPIKey);
			RestRequest request = new RestRequest();
			request.AddParameter("domain", "sandbox229f682cee2c4780a73253fd83eaf4a4.mailgun.org", ParameterType.UrlSegment);
			request.Resource = "{domain}/messages";
			request.AddParameter("from", "ScrumManager <postmaster@sandbox229f682cee2c4780a73253fd83eaf4a4.mailgun.org>");
			request.AddParameter("to", to);
			request.AddParameter("subject", subject);
			request.AddParameter("text", body);
			request.Method = Method.POST;
			return client.Execute(request);
		}

		// You can see a record of this email in your logs: https://app.mailgun.com/app/logs.

		// You can send up to 300 emails/day from this sandbox server.
		// Next, you should add your own domain so you can send 10000 emails/month for free.
	}
}
