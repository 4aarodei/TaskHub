﻿namespace TaskHub.Models.Playlist;
public class WorkStation
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // Виправлено CS8618: Додано значення за замовчуванням
    public bool PlaylistState { get; set; } // true - згенеровано false - незгенеровано
}
