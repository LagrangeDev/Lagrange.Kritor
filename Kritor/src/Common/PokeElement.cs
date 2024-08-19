namespace Kritor.Common;

public partial class PokeElement {
    public static PokeElement Create(uint id, uint pokeType, uint strength) {
        return new() { Id = id, PokeType = pokeType, Strength = strength };
    }
}