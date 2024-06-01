using System.ComponentModel.DataAnnotations;

namespace TesteCacheRedisDecorator.Models;

public class Endereco
{
    [Key]
    public int IdEndereco { get; set; }
    public required string Logradouro { get; set; }
    public required string Numero { get; set; }
    public required string Bairro { get; set; }
    public int IdPessoa { get; set; }
    public virtual required Pessoa Pessoa { get; set; }
}