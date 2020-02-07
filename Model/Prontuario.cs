using System;

namespace Model
{
    public class Prontuario
    {       
        public string numero { get; set; }
        public string nome { get; set; }
        public string sexo { get; set; }
        public string nomeMae { get; set; }
        public string nomePai { get; set; }
        public DateTime dataNascimento { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string CEP { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public DateTime dataRegistro { get; set; }
        public string telefoneFixo { get; set; }
        public string telefoneCelular { get; set; }
        public string OrigemEncaminhamento { get; set; }
        public string MedicoResponsavel { get; set; }
        public int Situacao { get; set; }
        public string Observacao { get; set; }

    }
}
