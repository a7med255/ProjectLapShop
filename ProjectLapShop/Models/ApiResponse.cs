namespace ProjectLapShop.Models
{
    public class ApiResponse
    {
        // data coming from api
        public object Data { get; set; }
        // list of api errors
        public object Errors { get; set; }
        // api status code 200=sucess 400=failed
        public string StatusCode { get; set; }
    }
}
