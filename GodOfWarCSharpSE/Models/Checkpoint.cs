namespace GodOfWarCSharpSE.Models;

public record Checkpoint(
    string Category,
    string Name,
    string Wad1,
    bool Wad1Active,
    string Wad2,
    bool Wad2Active,
    string CurrCam,
    string CamWad,
    float XPos,
    float YPos,
    float ZPos,
    uint Stance,
    bool WorldIdle = false
);