using System.ComponentModel.DataAnnotations;
using TesteCacheRedisDecorator.Attributes;
using TesteCacheRedisDecorator.Models.Enums;

namespace TesteCacheRedisDecorator.Models;

[Cached(true)]
public class Pessoa
{

    [Key]
    public int IdPessoa { get; set; }
    public required string Nome { get; set; }
    public int? Idade { get; set; }
    public TipoPessoa TipoPessoa { get; set; }
    public virtual List<Endereco>? Enderecos { get; set; }
}