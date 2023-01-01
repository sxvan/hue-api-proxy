namespace MyStromButton.Models
{
    public class LightGetResponse
    {
        public object[] Errors { get; set; }

        public LightGet[] Data { get; set; }
    }
}
