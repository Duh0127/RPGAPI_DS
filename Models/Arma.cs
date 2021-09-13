namespace RpgApi.Models
{
    public class Arma
    {
        public int Id { get; set; } = 1;
        public string Nome { get; set; } = "Arco e Flecha";
        public int Dano { get; set; } = 30;
        public Personagem Personagem { get; set; }
    }
}





