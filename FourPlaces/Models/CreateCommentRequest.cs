using Newtonsoft.Json;

namespace FourPlaces.Models
{
	public class CreateCommentRequest
	{
		[JsonProperty("text")]
		public string Text { get; set; }
	}
}