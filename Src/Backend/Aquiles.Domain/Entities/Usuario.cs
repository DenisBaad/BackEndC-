﻿namespace Aquiles.Domain.Entities;
public class Usuario : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}
