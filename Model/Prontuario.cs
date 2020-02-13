using System;

namespace Model
{
    public class Prontuario
    {
        public int codigo { get; set; }
        public string numero { get; set; }
        public string nome { get; set; }
        public string sexo { get; set; }
        public int idade { get; set; }
        public string classificacao { get; set; }
        public string nomeMae { get; set; }
        public string nomePai { get; set; }
        public DateTime dataNascimento { get; set; }        
        public string logradouro { get; set; }
        public string bairro { get; set; }
        public string cep { get; set; }
        public string cidade { get; set; }
        public string uf { get; set; }
        public DateTime dataRegistro { get; set; }
        public string telefoneFixo { get; set; }
        public string telefoneCelular { get; set; }
        public string origemEncaminhamento { get; set; }
        public string medicoResponsavel { get; set; }
        public int situacao { get; set; }
        public string observacao { get; set; }

    }
}
