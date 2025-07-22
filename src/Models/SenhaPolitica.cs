namespace Sws.Models
{
    public class SenhaPolitica
    {
        public bool LogonCaseSentisitive { get; set; }
        public bool ImporValidade { get; set; }
        public int ImporValidadeDias { get; set; }
        public string SenhaPadraoNovos { get; set; } = string.Empty;
    }
}
