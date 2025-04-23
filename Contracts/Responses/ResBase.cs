
namespace Contracts.Responses
{
    public class ResBase
    {
        public bool resultado { get; set; }
        public string detalle { get; set; }
        public List<string> errores { get; set; }
    }
}
