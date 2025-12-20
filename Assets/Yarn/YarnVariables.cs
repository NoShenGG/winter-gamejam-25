using Yarn.Unity;

[System.CodeDom.Compiler.GeneratedCode("YarnSpinner", "3.1.3.0")]
public partial class YarnVariables : Yarn.Unity.InMemoryVariableStorage, Yarn.Unity.IGeneratedVariableStorage {
    // Accessor for String $playername
    public string Playername {
        get => this.GetValueOrDefault<string>("$playername");
        set => this.SetValue<string>("$playername", value);
    }

}
