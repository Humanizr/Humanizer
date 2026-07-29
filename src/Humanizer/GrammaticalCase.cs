namespace Humanizer;

/// <summary>
/// Options for specifying the desired grammatical case for the output words
/// </summary>
public enum GrammaticalCase
{
    /// <summary>
    /// Indicates the subject of a finite verb
    /// </summary>
    Nominative,
    /// <summary>
    /// Indicates the possessor of another noun
    /// </summary>
    Genitive,
    /// <summary>
    /// Indicates the indirect object of a verb
    /// </summary>
    Dative,
    /// <summary>
    /// Indicates the direct object of a verb
    /// </summary>
    Accusative,
    /// <summary>
    /// Indicates an object used in performing an action
    /// </summary>
    Instrumental,
    /// <summary>
    /// Indicates the object of a preposition
    /// </summary>
    Prepositional,
    /// <summary>
    /// Indicates motion away from a noun
    /// </summary>
    Ablative,
    /// <summary>
    /// Indicates accompaniment
    /// </summary>
    Comitative,
    /// <summary>
    /// Indicates the agent of a transitive verb in an ergative construction
    /// </summary>
    Ergative,
    /// <summary>
    /// Indicates location
    /// </summary>
    Locative,
    /// <summary>
    /// Indicates a form used before a case-marking postposition or suffix
    /// </summary>
    Oblique,
    /// <summary>
    /// Indicates a partial or indefinite quantity
    /// </summary>
    Partitive,
    /// <summary>
    /// Indicates direct address
    /// </summary>
    Vocative,
    /// <summary>
    /// Indicates motion out of or away from within
    /// </summary>
    Elative,
    /// <summary>
    /// Indicates motion into
    /// </summary>
    Illative,
    /// <summary>
    /// Indicates association or accompaniment expressed by the Malayalam sociative case
    /// </summary>
    Sociative,
    /// <summary>
    /// Indicates an endpoint or limit
    /// </summary>
    Terminative,
    /// <summary>
    /// Indicates transition into a state
    /// </summary>
    Translative,
}