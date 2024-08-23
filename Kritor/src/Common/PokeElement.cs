namespace Kritor.Common;

public partial class PokeElement {
    public PokeElement SetId(uint id) {
        Id = id;
        return this;
    }

    public PokeElement SetPokeType(uint pokeType) {
        PokeType = pokeType;
        return this;
    }

    public PokeElement SetStrength(uint strength) {
        Strength = strength;
        return this;
    }
}